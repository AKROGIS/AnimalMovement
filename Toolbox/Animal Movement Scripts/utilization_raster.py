# -*- coding: utf-8 -*-
"""
UD_Raster.py
Created: 2011-10-06

Title:
Utilization Distribution Raster

Tags:
home, range, animal, tracking, telemetry, ecology, kernel, density

Summary:
This tool uses the Kernel Density Estimator tool in the Spatial Analyst to calculate a utilization distribution raster.

Usage:
The smoothing factor used here is that from the kernel density and ecology literature, not the search radius used in ESRI's documentation.
Since ESRI KDE uses a quartic approximation to the bivariate normal distribution (based on experimental testing described on the ESRI user forums) and .... the search radius is two times the smoothing factor,
The extents of the raster is sufficent to capture the extents of the input data plus the search radius + the cell size, so all contours based on the raster should be closed.
The UD raster is created from the density raster by slicing the values of the density raster into 100 equal intervals. Cells in the UD raster are assigned a 1 if the corresponding cell in the density raster are in the band with the highest density. Cells in the UD raster are assigned a 100 if the corresponding cell in the density raster are in the lowest density band.
The cell size if not specified it defaults to the length of the longest side of the extents envelope divided by 2000. This will result in a raster of approximately 4 million cells.
The raster will get the projection specified by the environment (usually this is the projection of the input). The format of the raster is determined by the file name provided. If there is no extension, then the file is an ESRI GRID file, .tif and .img extension are typically used. The raster can also be created in a geodatabase
This tool requires a license for spatial analyst.

Parameter 1:
Location_Layer
Layer name (if in ArcMap) or path to a feature class of points (typically animal locations).  If a layer is used in ArcMap, and features are selected in that layer, only the selected featues are used, otherwise all the features in the layer's definition query are used in the analysis.  If this is a feature class then all the features are used in the analysis.  The distribution of the points should be evaluated to determine the appropriateness of this tool and the correct selection of input parameters.

Parameter 2:
Raster_File
The new raster to create as output

Parameter 3:
Smoothing_Factor
The smoothing factor is a parameterless number used to smooth the Kernel density estimate.  A small smoothing factor may omit locations between fixes.  A large smoothing factor may create an indistinct blob.  Lots of literature exists on selecting an appropriate smoothing factor.  The input for this tool is usually generated from the UD_Smoothing_Factor tool.

Parameter 4:
Cell_Size
The default cell size is chosen to create a raster that is 2000 or less cells wide and 2000 or less cells high. The cell size is determined by the extents of the image will is dependent on the extents of the input locations.

Parameter 5:
Output_Projection
Calculations and output must be done in a projected coordinate system (i..e not geographic - lat/long).  The projected coordinate system to use can be specified in three ways, 1) with this parameter, 2) with the output coordinate system in the environment, or 3) with the coordinate system of the input.  These options are listed in priority order, that is this paraeter will trump the environment, and the environment will trump the input data. if a projected coordinate system is not found then the program will abort.

Scripting Syntax:
UDRaster (Location_Layer, Raster_File, Smoothing_Factor, Cell_Size, Output_Projection)

Example1:
Scripting Example
The following example shows how this script can be used in the ArcGIS Python Window. It assumes that the script has been loaded into a toolbox, and the toolbox has been loaded into the active session of ArcGIS.
It creates a UD raster with a smoothing factor of 4500
 raster = r"C:/tmp/ud.tif"
 fixes = r"C:/tmp/test.gdb/fixes"
 UDRaster(fixes, raster, 4500)

Example2:
Command Line Example
The following example shows how the script can be used from the operating system command line. It assumes that the script and the data sources are in the current directory and that the python interpeter is the path.
It creates a UD raster with a smoothing factor of 4500
 C:/folder> python UD_Raster.py test.gdb/location kde.tif 4500

Credits:
Regan Sarwas, Alaska Region GIS Team, National Park Service

Limitations:
Public Domain

Requirements
arcpy module - requires ArcGIS v10+ and a valid license
arcpy.sa module - requires a valid license to Spatial Analyst Extension

Disclaimer:
This software is provide "as is" and the National Park Service gives
no warranty, expressed or implied, as to the accuracy, reliability,
or completeness of this software. Although this software has been
processed successfully on a computer system at the National Park
Service, no warranty expressed or implied is made regarding the
functioning of the software on another system or for general or
scientific purposes, nor shall the act of distribution constitute any
such warranty. This disclaimer applies both to individual use of the
software and aggregate use with other software.
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import sys

import arcpy

import utils


def GetUDRaster(features, smoothingFactor, sr=None, cellSize=None):
    savedState, searchRadius = SetRasterEnvironment(
        features, smoothingFactor, sr, cellSize
    )
    try:
        return GetNormalizedKernelRaster(features, searchRadius)
    finally:
        RestoreRasterEnvironment(savedState)


def SetupRaster(features, smoothingFactor, sr=None, cellSize=None):
    # Describe() will get the envelope in the feature's Spatial Reference
    # extent = arcpy.Describe(features).extent
    # This will return the extent in the environment's Output Spatial Reference
    mcpList = arcpy.MinimumBoundingGeometry_management(
        features, arcpy.Geometry(), "ENVELOPE", "ALL"
    )
    extent = mcpList[0].extent
    if utils.is_float(cellSize):
        cellSize = float(cellSize)  # all parameters from ArcToolbox are text
    else:
        cellSize = DefaultCellSize(extent)
    # FIXME explain why r=2*h
    searchRadius = 2 * smoothingFactor
    return extent, cellSize, searchRadius


def DefaultCellSize(extent):
    return max(extent.XMax - extent.XMin, extent.YMax - extent.YMin) / 2000.0


class SavedState:
    pass  # used as a namespace for saving multiple state values


def SetRasterEnvironment(features, smoothingFactor, sr=None, cellSize=None):
    # Save Existing State
    savedState = SavedState()
    savedState.cellSize = arcpy.env.cellSize
    savedState.extent = arcpy.env.extent
    savedState.sr = arcpy.env.outputCoordinateSystem
    # Set New State
    arcpy.env.outputCoordinateSystem = sr
    # SetupRaster() depends on arcpy.env.outputCoordinateSystem
    extent, cellSize, searchRadius = SetupRaster(
        features, smoothingFactor, sr, cellSize
    )
    print(extent.XMin, extent.YMin, extent.XMax, extent.YMax, cellSize, searchRadius)
    arcpy.env.cellSize = cellSize
    buffer = searchRadius + cellSize
    arcpy.env.extent = arcpy.Extent(
        extent.XMin - buffer,
        extent.YMin - buffer,
        extent.XMax + buffer,
        extent.YMax + buffer,
    )
    return savedState, searchRadius


def RestoreRasterEnvironment(savedState):
    arcpy.env.outputCoordinateSystem = savedState.sr
    arcpy.env.extent = savedState.extent
    arcpy.env.cellSize = savedState.cellSize


def GetNormalizedKernelRaster(features, searchRadius):
    gotRaster, raster = GetKernelRaster(features, searchRadius)
    if gotRaster:
        return NormalizeRaster(raster, 100)
    return gotRaster, raster


def GetKernelRaster(features, searchRadius):
    try:
        # ESRI uses a quartic approximation to the bivariate normal distribution.
        # the search radius for quartic does not map to the bandwidth for a normal distribution
        populationField = "NONE"
        cellSize = None  # use value set in environment
        scaleFactor = None  # SQUARE_MAP_UNITS, SQUARE_MILES, SQUARE_KILOMETERS, ...
        # scaling factor should be large to avoid floating point errors.
        # if map units are meters or feet, output units are per sq_km or sq_miles, respectively; therefore default scaling factor > 1,000,000

        kernel = arcpy.sa.KernelDensity(
            features, populationField, cellSize, searchRadius, scaleFactor
        )

        return True, kernel
    except Exception as ex:
        return False, "{0}".format(ex)


def NormalizeRaster(raster, bins):
    # classify the results into bins equal interval bins, and invert (0..bins -> bins..1)
    try:
        raster = (1 + bins) - arcpy.sa.Slice(raster, bins, "EQUAL_INTERVAL")
        return True, raster
    except Exception as ex:
        return False, "{0}".format(ex)


if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        utils.die("Unable to checkout the Spatial Analyst Extension.  Quitting.")

    locationLayer = arcpy.GetParameterAsText(0)
    rasterName = arcpy.GetParameterAsText(1)
    smoothingFactor = arcpy.GetParameterAsText(2)
    cellSize = arcpy.GetParameterAsText(3)
    spatialReference = arcpy.GetParameter(4)

    test = False
    if test:
        # locationLayer = r"C:\tmp\test.gdb\fix_ll"
        locationLayer = r"C:\tmp\test.gdb\fix_a_c96"
        rasterName = r"C:\tmp\test.gdb\kde_4"
        smoothingFactor = "8000"
        cellSize = "700"
        spatialReference = arcpy.SpatialReference()
        spatialReference.loadFromString(
            "PROJCS['NAD_1983_Alaska_Albers',GEOGCS['GCS_North_American_1983',DATUM['D_North_American_1983',SPHEROID['GRS_1980',6378137.0,298.257222101]],PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]],PROJECTION['Albers'],PARAMETER['False_Easting',0.0],PARAMETER['False_Northing',0.0],PARAMETER['Central_Meridian',-154.0],PARAMETER['Standard_Parallel_1',55.0],PARAMETER['Standard_Parallel_2',65.0],PARAMETER['Latitude_Of_Origin',50.0],UNIT['Meter',1.0]];-13752200 -8948200 10000;-100000 10000;-100000 10000;0.001;0.001;0.001;IsHighPrecision"
        )
        # arcpy.env.outputCoordinateSystem = spatialReference
        arcpy.env.outputCoordinateSystem = None
        # spatialReference = None

    #
    # Input validation
    #
    if not rasterName:
        utils.die("No output requested. Quitting.")

    try:
        smoothingFactor = float(smoothingFactor)
    except (ValueError, TypeError):
        utils.die("Smoothing Factor was not a valid number.")

    if not locationLayer:
        utils.die("No location layer was provided. Quitting.")

    if not arcpy.Exists(locationLayer):
        utils.die("Location layer cannot be found. Quitting.")

    desc = arcpy.Describe(locationLayer)  # Describe() is expensive, so do it only once
    shapeName = desc.shapeFieldName
    inputSR = desc.spatialReference
    usingInputSR = False

    if not spatialReference or not spatialReference.name:
        spatialReference = arcpy.env.outputCoordinateSystem

    if not spatialReference or not spatialReference.name:
        usingInputSR = True
        spatialReference = inputSR

    if not spatialReference or not spatialReference.name:
        utils.die(
            "The fixes layer does not have a coordinate system, and you have not provided one. Quitting."
        )

    if spatialReference.type != "Projected":
        utils.die(
            "The output projection is '"
            + spatialReference.type
            + "'.  It must be a projected coordinate system. Quitting."
        )

    if usingInputSR or (
        inputSR
        and spatialReference
        and spatialReference.factoryCode == inputSR.factoryCode
    ):
        spatialReference = None

    #
    # Create density raster(s)
    #
    gotRaster, raster = GetUDRaster(
        locationLayer, smoothingFactor, spatialReference, cellSize
    )
    if gotRaster and rasterName:
        raster.save(rasterName)
    else:
        utils.die("Unable to create raster: " + raster)
