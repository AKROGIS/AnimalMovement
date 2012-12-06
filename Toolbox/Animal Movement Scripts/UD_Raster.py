# ------------------------------------------------------------------------------
# UD_Raster.py
# Created: 2011-10-06
#
# Title:
# Utilization Distribution Raster
#
# Tags:
# home, range, animal, tracking, telemetry, ecology, kernel, density
#
# Summary:
# This tool uses the Kernel Density Estimator tool in the Spatial Analyst to calculate a utilization distribution raster.
# 
# Usage:
# The smoothing factor used here is that from the kernel density and ecology literature, not the search radius used in ESRI's documentation.
# Since ESRI KDE uses a quartic approximation to the bivariate normal distribution (based on experimental testing described on the ESRI user forums) and .... the search radius is two times the smoothing factor,
# The extents of the raster is sufficent to capture the extents of the input data plus the search radius + the cell size, so all contours based on the raster should be closed.
# The UD raster is created from the density raster by slicing the values of the density raster into 100 equal intervals. Cells in the UD raster are assigned a 1 if the corresponding cell in the density raster are in the band with the highest density. Cells in the UD raster are assigned a 100 if the corresponding cell in the density raster are in the lowest density band.
# The cell size if not specified it defaults to the length of the longest side of the extents envelope divided by 2000. This will result in a raster of approximately 4 million cells.
# The raster will get the projection specified by the environment (usually this is the projection of the input). The format of the raster is determined by the file name provided. If there is no extension, then the file is an ESRI GRID file, .tif and .img extension are typically used. The raster can also be created in a geodatabase
# This tool requires a license for spatial analyst.
#
# Parameter 1:
# Location_Layer
# Layer name (if in ArcMap) or path to a feature class of points (typically animal locations).  If a layer is used in ArcMap, and features are selected in that layer, only the selected featues are used, otherwise all the features in the layer's definition query are used in the analysis.  If this is a feature class then all the features are used in the analysis.  The distribution of the points should be evaluated to determine the appropriateness of this tool and the correct selection of input parameters.
#
# Parameter 2:
# Raster_File
# The new raster to create as output
#
# Parameter 3:
# Smoothing_Factor
# The smoothing factor is a parameterless number used to smooth the Kernel density estimate.  A small smoothing factor may omit locations between fixes.  A large smoothing factor may create an indistinct blob.  Lots of literature exists on selecting an appropriate smoothing factor.  The input for this tool is usually generated from the UD_Smoothing_Factor tool.
#
# Parameter 4:
# Cell_Size
# The default cell size is chosen to create a raster that is 2000 or less cells wide and 2000 or less cells high. The cell size is determined by the extents of the image will is dependent on the extents of the input locations.
#
# Parameter 5:
# Output_Projection
# Calculations and output must be done in a projected coordinate system (i..e not geographic - lat/long).  The projected coordinate system to use can be specified in three ways, 1) with this parameter, 2) with the output coordinate system in the environment, or 3) with the coordinate system of the input.  These options are listed in priority order, that is this paraeter will trump the environment, and the environment will trump the input data. if a projected coordinate system is not found then the program will abort.
#
# Scripting Syntax:
# UDRaster (Location_Layer, Raster_File, Smoothing_Factor, Cell_Size, Output_Projection) 
#
# Example1:
# Scripting Example
# The following example shows how this script can be used in the ArcGIS Python Window. It assumes that the script has been loaded into a toolbox, and the toolbox has been loaded into the active session of ArcGIS.
# It creates a UD raster with a smoothing factor of 4500
#  raster = r"C:\tmp\ud.tif"
#  fixes = r"C:\tmp\test.gdb\fixes"
#  UDRaster(fixes, raster, 4500)
#
# Example2:
# Command Line Example
# The following example shows how the script can be used from the operating system command line. It assumes that the script and the data sources are in the current directory and that the python interpeter is the path.
# It creates a UD raster with a smoothing factor of 4500
#  C:\folder> python UD_Raster.py test.gdb\location kde.tif 4500 
#
# Credits:
# Regan Sarwas, Alaska Region GIS Team, National Park Service
#
# Limitations:
# Public Domain
#
# Requirements
# arcpy module - requires ArcGIS v10+ and a valid license
# arcpy.sa module - requires a valid license to Spatial Analyst Extension
#
# Disclaimer:
# This software is provide "as is" and the National Park Service gives
# no warranty, expressed or implied, as to the accuracy, reliability,
# or completeness of this software. Although this software has been
# processed successfully on a computer system at the National Park
# Service, no warranty expressed or implied is made regarding the
# functioning of the software on another system or for general or
# scientific purposes, nor shall the act of distribution constitute any
# such warranty. This disclaimer applies both to individual use of the
# software and aggregate use with other software.
# ------------------------------------------------------------------------------

import sys
import arcpy
import utils


def GetUDRaster(features, smoothingFactor, cellSize = None):
    cellSize, searchRadius = SetupRaster(features, smoothingFactor, cellSize)
    return GetProbabilityRaster(features, cellSize, searchRadius)


def SetupRaster(features, smoothingFactor, cellSize = None):
    envelope = arcpy.Describe(features).Extent
    maxDivisions = 2000
    try:
        cellSize = float(cellSize) # all parameters from ArcToolbox are text 
    except (ValueError, TypeError):
        cellSize =  GetCellSize(envelope, maxDivisions)
    # FIXME explain why r=2*h
    searchRadius = 2 * smoothingFactor
    buffer = searchRadius + cellSize
    SetRasterExtents(envelope, buffer)
    return cellSize, searchRadius
    

def GetCellSize(envelope, maxDivisions):
    return max(envelope.xmax-envelope.xmin,
               envelope.ymax-envelope.ymin) / maxDivisions


def SetRasterExtents(envelope, buffer):
    arcpy.env.extent = arcpy.Extent(envelope.xmin-buffer,
                                    envelope.ymin-buffer,
                                    envelope.xmax+buffer,
                                    envelope.ymax+buffer)


def GetProbabilityRaster(features, cellSize, searchRadius):
    try:
        # if map units are meters, output units are per kmsq; therefore default scaling factor = 1,000,000
        # scaling factor should be large to avoid floating point errors.
        # ESRI uses a quartic approximation to the bivariate normal distribution.
        # the search radius for quartic does not map to the bandwidth for a ND
        populationField = "NONE"
        scaleFactor = "" #SQUARE_MAP_UNITS, SQUARE_MILES, SQUARE_KILOMETERS, ...
        kernel = arcpy.sa.KernelDensity(features, populationField, cellSize, searchRadius, scaleFactor)
        #classify the results into 100 equal interval bins, and invert (0..100 -> 100..0)
        raster = 101 - arcpy.sa.Slice(kernel,100,"EQUAL_INTERVAL")
        return True, raster
    except:
        utils.die(sys.exc_info()[1])


if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        utils.die("Unable to checkout the Spatial Analyst Extension.  Quitting.")

    locationLayer = arcpy.GetParameterAsText(0)
    rasterName = arcpy.GetParameterAsText(1)
    smoothingFactor = arcpy.GetParameterAsText(2)
    cellSize = arcpy.GetParameterAsText(3)

    test = False
    if test:
        locationLayer = r"C:\tmp\test2.gdb\w2011a0926"
        rasterName = r"C:\tmp\kd_test\w2011a901sf4500b.tif"
        smoothingFactor = "4500"
        cellSize = ""
    
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

    #
    # Create density raster(s)
    #
    gotRaster, raster = GetUDRaster(locationLayer, smoothingFactor, cellSize)
    if gotRaster and rasterName:
        raster.save(rasterName)
