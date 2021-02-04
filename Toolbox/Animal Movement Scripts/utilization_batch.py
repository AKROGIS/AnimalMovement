# -*- coding: utf-8 -*-
"""
UD_Batch.py
Created: 2011-10-06

Title:
Utilization Distribution Batch Process

Tags:
contour, home, range, animal, tracking, telemetry, ecology, kernel, density

Summary:
Create the utilization polygons for a group of animals

Usage:
Output can be lines, polygons, or donuts (as described below)
Kernels for each value in the Subset_Identifier (usually an animal id) are generated and normalized.  These normalized rasters are then combined to create the overall utilization raster used to create the utilization polygons.  This removes the bias from differeing numbers of locations for each animal.
The kernel rasters for individual animals can be saved if desired.
The analysis and the output must be in a projected coordinate system.  If the input is not specified, you must provide the output projection.

Parameter 1:
Location_Layer
Layer name (if in ArcMap) or path to a feature class of points (typically animal locations).  If a layer is used in ArcMap, and features are selected in that layer, only the selected featues are used, otherwise all the features in the layer's definition query are used in the analysis.  If this is a feature class then all the features are used in the analysis.  The distribution of the points should be evaluated to determine the appropriateness of this tool and the correct selection of input parameters.

Parameter 2:
Subset_Identifier
The data set is subdivided by each unique value in this field.  Typically this is the name of the field with the animal identifiers, and the data is therefore grouped by animal.  Each data subset (i.e. each animal) is evaluated individually, and then the normalized results are combined for the final output.  A field must be provided, if you wish to analyze all the data without analyzing subsets first, then use the Utilization Distribution tool.

Parameter 3:
hRef_Method
This is the method for calculating hRef, the reference (or base) smoothing factor for each data subset. Selecting the correct smoothing factor is key to meaningful results. Large smoothing factors may over smooth the results, adding area to the UD, and small smoothing factors may result in too much detail, and insufficient area in the UD.
Worton: Sqrt( (variation_in_x + variation_in_y)/2) / n^(1/6)
Tufto: (Sqrt(variation_in_x + variation_in_y)/2) / n^(1/6)
Fixed: User provided constant
Worton and Tufto make assume about the distribution, correlation and variation of the data that should be verified on your dataset before using these values.
References:
Tufto, J., Andersen, R. and Linnell, J. 1996. Habitat use and ecological correlates of home range size in a small cervid: the roe deer. J. Anim. Ecol. 65:715-724.
Worton, B.J. 1989. Kernel methods for estimating the utilization distribution in home-range studies. Ecology  70:164-168#

Parameter 4:
Fixed_hRef
This parameter is required if and only if the hRef Method is "Fixed".  The units of the smoothing factor are the same as the spatial coordinates of the location data.  If the data is unprojected then the the units are decimal degrees (this will probably yield incorrect or distorted results).

Parameter 5:
hRef_Modifier
The reference smoothing factor (hRef) can be adjusted in a number of ways to produce the final smoothing factor.
None: No adjustment is made; h = hRef
Proportion: A percentage of hRef is used. various investigators have suggested different percentages based on the type and distribution of the data under consideration.
LSCV: A least squares cross validation is done to select the value between 0.05*hRef and 2.0*hRef that minimizes the LSCV score (Worton1995) between all pairs of points. This function is not guaranteed to work correctly if there are duplicate locations (a warning is issued and duplicate points are offset by a unit amount). In addition, there is no guarantee of a minimum in the range checked (a warning is issued and hRef is used). LSCV is very slow, and is limited to no more than 2000 points in a data subset.
BCV2: This is the same as LSCV, except a slightly different scoring function (Sain et. al. 1994) is used.
References:
Worton, B. J. 1995. Using Monte Carlo simulation to evaluate kernel-based home range estimators. Journal of Wildlife Management 59:794-800.
Sain, S. R., K. A Baggerly, and D. W. Scott. 1994. Cross-validation of multivariate densities. Journal of the American Statistical Association 89:807-817

Parameter 6:
hRef_Proportion
This parameter is required if and only if the hRef Modifier is "Proportion". This is a percentage of hRef to use for the final smoothing factor.  Typical values are between .5 (50%) and 1 (100%), although any positive number is acceptable.

Parameter 7:
hRef_To_Use
A smoothing factor is calculated for each dataset per the previous parameters. This parameter clarifies which smoothing factor should be used for the analysis
Minimum: The smallest of the subset smoothing factors is determined and then used to analyze all data subsets
Maximum: The largest of the subset smoothing factors is determined and then used to analyze all data subsets
Average: The average of the subset smoothing factors is determined and then used to analyze all data subsets
ByDataset: Each data subset is analyzed with the smoothing factor specific to that subset.

Parameter 8:
Save_Rasters_
The individual kernel rasters for each animal are not saved after they are used to create the combined raster.  If you would like to save them, check this box.

Parameter 9:
Raster_Folder
This option is only available if you are saving individual rasters.  Each raster is saved as a tif file in the folder specified here.  files are named with the values in the Subset_Identifier field

Parameter 10:
Isopleths
This is a list of isopleth values separated by commas, semicolons, or whitespace. The values provided should be appropriate for the input raster (integers between 1 and 99 for rasters created with the UD Raster tool).

Parameter 11:
Isopleth_Lines
The name of a new output polyline feature class. One of Lines, Polygons, or Donut_Polygons must be provided.  If this parameter is left blank, no lines will be created. The output feature class will have a field named 'contour' with the value of the isopleth, and one or more features for each isopleth requested that exists in the input raster.  There may be multiple polylines for each isopleth.  Polylines may not close, but they should if the input is a UD Raster from the UD Raster tool.
No smoothing is done, and depending on the cell size the output can be very dense (small cell size), or very blocky (large cell size)

Parameter 12:
Isopleth_Polygons
Name of the new output polygon feature class. One of Lines, Polygons, or Donut_Polygons must be provided
Contains a polygon for each isopleth.  Each polygon contains the entire are covered by the isopleth. These polygons are overlapping.  The polygons are written to the featureclass with the largest isopleth values first. (for UD analysis, this provides a correctly stacked results set). These polygons are created from the isopleth lines

Parameter 13:
Isopleth_Donuts
Name of the new output polygon feature class. One of Lines, Polygons, or Donut_Polygons must be provided
Contains a polygon for each isopleth range.  Assumes the isopleths are ordered with the largest values containing the most area (so the last range is a donut without a hole). There is no donut for the first range range (i.e. from the universe to the first isopleth). These polygons are created from the lines

Parameter 14:
Output_Projection
Calculations and output must be done in a projected coordinate system (i..e not geographic - lat/long).  The projected coordinate system to use can be specified in three ways, 1) with this parameter, 2) with the output coordinate system in the environment, or 3) with the coordinate system of the input.  These options are listed in priority order, that is this paraeter will trump the environment, and the environment will trump the input data. if a projected coordinate system is not found then the program will abort.

Parameter 15:
Cell_Size
The width and height of the cells in the kernel raster (in units of the output coordinate system).  The default is the cell size that will result in a raster with at least 2000 cells in the vertical and horizontal direction.

Scripting Syntax:
UD_Batch_AnimalMovement (Location_Layer, Subset_Identifier, hRef_Method, Fixed_hRef, hRef_Modifier, hRef_Proportion, hRef_To_Use, Save_Rasters_, Raster_Folder, Isopleths, Isopleth_Lines, Isopleth_Polygons, Isopleth_Donuts, Output_Projection, Cell_Size)

Example1:
Scripting Example
The following example shows how this script can be used in the ArcGIS Python Window. It assumes that the script has been loaded into a toolbox, and the toolbox has been loaded into the active session of ArcGIS.
It creates the 65%, 90% UD polygons (with holes) for a group of animals using the average of each animals smoothing facter as determined by LSCV
 fixes = "C:/tmp/test.gdb/locations"
 donuts = "C:/tmp/test.gdb/ud_donuts"
 UD_Batch_AnimalMovement (fixes, "AnimalId", "Worton", "", "LSCV", "", "Average", "","", "65;90", "", "", donuts)

Example2:
Command Line Example
The following example shows how the script can be used from the operating system command line. It assumes that the script and the data sources are in the current directory and that the python interpeter is the path.
It creates the 65%, 90% UD polygons (with holes) for a group of animals using the average of each animals smoothing facter as determined by LSCV
 C:/folder> python UD_Batch.py locations.shp AnimalId Worton # LSCV # Average # "65;90" # # test.gdb/ud_donuts


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

import os

import arcpy
import numpy

import utilization_smoothing
import utilization_raster
import utilization_isopleth
import utils

# pylint: disable=invalid-name, missing-function-docstring
# TODO: Fix names and add doc strings


def GetSmoothingFactors(
    subsetIdentifier,
    uniqueValues,
    locationLayer,
    hRefmethod,
    modifier,
    proportionAmount,
    sr=None,
    shapeName=None,
):
    layer = "subsetForSmoothingFactor"
    hList = []
    for value in uniqueValues:
        query = utilization_isopleth.BuildQuery(locationLayer, subsetIdentifier, value)
        utils.info("Calculating h for {0}.".format(query))
        if arcpy.Exists(layer):
            arcpy.Delete_management(layer)
        arcpy.MakeFeatureLayer_management(locationLayer, layer, query)
        try:
            points = utils.get_points(layer, sr)
            if len(points) < 3:
                utils.warn(
                    "Insufficient locations ({0}) for {1}.".format(len(points), value)
                )
            else:
                h = utilization_smoothing.GetSmoothingFactor(
                    points, hRefmethod, modifier, proportionAmount
                )
                hList.append(h)
        finally:
            arcpy.Delete_management(layer)
    return hList


def ChooseSmoothingFactor(hList, hRefToUse):
    if hRefToUse.lower() == "minimum":
        return min(hList)
    if hRefToUse.lower() == "maximum":
        return max(hList)
    return numpy.average(hList)


def BuildNormalizedRaster(
    subsetIdentifier,
    uniqueValues,
    locationLayer,
    hList,
    saveRasters,
    rasterFolder,
    sr=None,
    cellSize=None,
    save_isopleths=False,
):
    n = 0
    layer = "subsetSelectionForRaster"
    savedState, searchRadius = utilization_raster.SetRasterEnvironment(
        locationLayer, max(hList), sr, cellSize
    )
    try:
        hDict = {}
        for k, v in zip(uniqueValues, hList):
            hDict[k] = v
        for value in uniqueValues:
            query = utilization_isopleth.BuildQuery(
                locationLayer, subsetIdentifier, value
            )
            utils.info("Creating KDE raster for {0}.".format(query))
            if arcpy.Exists(layer):
                arcpy.Delete_management(layer)
            arcpy.MakeFeatureLayer_management(locationLayer, layer, query)
            try:
                searchRadius = 2 * hDict[value]
                gotRaster, probRaster = utilization_raster.GetNormalizedKernelRaster(
                    layer, searchRadius
                )
                if gotRaster:
                    if save_isopleths:
                        lines, polys, donuts = None, None, None
                        if isoplethLines:
                            lines = "{0}_{1}".format(isoplethLines, value)
                        if isoplethPolys:
                            polys = "{0}_{1}".format(isoplethPolys, value)
                        if isoplethDonuts:
                            donuts = "{0}_{1}".format(isoplethDonuts, value)
                        utilization_isopleth.CreateIsopleths(
                            isoplethList, probRaster, lines, polys, donuts
                        )
                    if saveRasters:
                        # Save individual probability rasters
                        name = "praster_{0}.tif".format(value)
                        path = os.path.join(rasterFolder, name)
                        probRaster.save(path)
                    if n:
                        raster = raster + probRaster
                        n = n + 1
                    else:
                        raster = probRaster
                        n = 1
                else:
                    msg = "  Raster creation failed, not included in total. {0}"
                    utils.warn(msg.format(probRaster))
            finally:
                arcpy.Delete_management(layer)
    finally:
        utilization_raster.RestoreRasterEnvironment(savedState)

    if n == 0:
        return False, None
    # renormalize from 1 to 100
    raster = arcpy.sa.Slice(raster, 100, "EQUAL_INTERVAL")
    if saveRasters:
        name = os.path.join(rasterFolder, "_praster_TOTAL.tif")
        raster.save(name)
    return True, raster


if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        utils.die("Unable to checkout the Spatial Analyst Extension.  Quitting.")

    locationLayer = arcpy.GetParameterAsText(0)
    subsetIdentifier = arcpy.GetParameterAsText(1)
    hRefmethod = arcpy.GetParameterAsText(2)
    fixedHRef = arcpy.GetParameterAsText(3)
    modifier = arcpy.GetParameterAsText(4)
    proportionAmount = arcpy.GetParameterAsText(5)
    hRefToUse = arcpy.GetParameterAsText(6)
    saveRasters = arcpy.GetParameterAsText(7)
    rasterFolder = arcpy.GetParameterAsText(8)
    isoplethInput = arcpy.GetParameterAsText(9)
    isoplethLines = arcpy.GetParameterAsText(10)
    isoplethPolys = arcpy.GetParameterAsText(11)
    isoplethDonuts = arcpy.GetParameterAsText(12)
    spatialReference = arcpy.GetParameter(13)
    cellSize = arcpy.GetParameterAsText(14)
    save_isopleths = arcpy.GetParameterAsText(14)

    test = False
    if test:
        locationLayer = r"C:\tmp\test.gdb\fix2_ll"
        subsetIdentifier = "AnimalId"
        hRefmethod = "FIXED"  # Fixed,Worton,Tufto,Silverman,Gaussian
        fixedHRef = "4000"
        modifier = "NONE"  # NONE,PROPORTIONAL,LSCV,BCV2
        proportionAmount = "0.7"
        hRefToUse = "Average"
        saveRasters = "True"
        rasterFolder = r"C:\tmp\kd_test"
        isoplethInput = "50,65,90,95"
        isoplethLines = r"C:\tmp\test.gdb\clines4a"
        isoplethPolys = r"C:\tmp\test.gdb\cpolys4a"
        isoplethDonuts = r"C:\tmp\test.gdb\cdonut4a"
        spatialReference = arcpy.SpatialReference()
        spatialReference.loadFromString(
            "PROJCS['NAD_1983_Alaska_Albers',GEOGCS['GCS_North_American_1983',DATUM['D_North_American_1983',SPHEROID['GRS_1980',6378137.0,298.257222101]],PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]],PROJECTION['Albers'],PARAMETER['False_Easting',0.0],PARAMETER['False_Northing',0.0],PARAMETER['Central_Meridian',-154.0],PARAMETER['Standard_Parallel_1',55.0],PARAMETER['Standard_Parallel_2',65.0],PARAMETER['Latitude_Of_Origin',50.0],UNIT['Meter',1.0]];-13752200 -8948200 10000;-100000 10000;-100000 10000;0.001;0.001;0.001;IsHighPrecision"
        )
        # arcpy.env.outputCoordinateSystem = spatialReference
        arcpy.env.outputCoordinateSystem = None
        cellSize = "#"
        save_isopleths = "True"

    #
    # Input validation
    #
    if hRefmethod.lower() == "fixed":
        try:
            fixedHRef = float(fixedHRef)
        except ValueError:
            utils.die("Fixed hRef was not a valid number. Quitting.")

    if modifier.lower() == "proportion":
        try:
            proportionAmount = float(proportionAmount)
        except ValueError:
            utils.die("Proportion Amount was not a valid number. Quitting.")

    if not locationLayer:
        utils.die("No location layer was provided. Quitting.")

    if not arcpy.Exists(locationLayer):
        utils.die("Location layer cannot be found. Quitting.")

    if not (isoplethLines or isoplethPolys or isoplethDonuts):
        utils.die("No output requested. Quitting.")

    isoplethList = utilization_isopleth.GetIsoplethList(isoplethInput)
    if not isoplethList:
        utils.die("List of valid isopleths is empty. Quitting.")

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
            "The fixes layer does not have a coordinate system, "
            "and you have not provided one. Quitting."
        )

    if spatialReference.type != "Projected":
        msg = (
            "The output projection is '{0}'. "
            "It must be a projected coordinate system. Quitting."
        )
        utils.die(msg.format(spatialReference.type))

    if usingInputSR or (
        inputSR
        and spatialReference
        and spatialReference.factoryCode == inputSR.factoryCode
    ):
        spatialReference = None

    saveRasters = saveRasters.lower() == "true"
    if saveRasters:
        if not os.path.exists(rasterFolder):
            os.mkdir(rasterFolder)  # may throw an exception (thats ok)
        else:
            if not os.path.isdir(rasterFolder):
                utils.die("{0} is not a folder. Quitting.".format(rasterFolder))

    uniqueValues = None
    if subsetIdentifier in [field.name for field in arcpy.ListFields(locationLayer)]:
        uniqueValues = utilization_isopleth.GetUniqueValues(
            locationLayer, subsetIdentifier
        )
    if not uniqueValues:
        msg = "Could not generate a list of unique values for {0}'. Quitting."
        utils.die(msg.format(subsetIdentifier))

    #
    # Calculate smoothing factor(s)
    #
    if hRefmethod.lower() == "fixed":
        hList = [fixedHRef for eachItem in uniqueValues]
    else:
        hList = GetSmoothingFactors(
            subsetIdentifier,
            uniqueValues,
            locationLayer,
            hRefmethod,
            modifier,
            proportionAmount,
            spatialReference,
            shapeName,
        )
        if hRefToUse.lower() != "bydataset":
            h = ChooseSmoothingFactor(hList, hRefToUse)
            utils.info("Using h = {0} ({2})".format(h, hRefToUse))
            hList = [h for eachItem in uniqueValues]
    #
    # Create density raster(s)
    #
    gotRaster, raster = BuildNormalizedRaster(
        subsetIdentifier,
        uniqueValues,
        locationLayer,
        hList,
        saveRasters,
        rasterFolder,
        spatialReference,
        cellSize,
        save_isopleths,
    )
    if gotRaster:
        utils.info("Created the temporary KDE raster")
    else:
        utils.die("Unable to create KDE raster. Quitting.")
    #
    # Create isopleths (for total raster only)
    #
    utilization_isopleth.CreateIsopleths(
        isoplethList, raster, isoplethLines, isoplethPolys, isoplethDonuts
    )
