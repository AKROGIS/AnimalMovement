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
# Creates a utilization (1-100%) raster based on a kernel density estimate of animal locations
# This tool uses the Kernel Density Estimate tool in the Spatial Analyst to calculate a utilization distribution raster.
# The smoothing factor used here is that from the kernel density and ecology literature, not the search radius used in ESRI's documentation.
# Since ESRI KDE uses a quartic approximation to the bivariate normal distribution (based on experimental testing described on the ESRI user forums) and .... the search radius is two times the smoothing factor,
# The extents of the raster is sufficent to capture the extents of the input data plus the search radius + the cell size, so all contours based on the raster should be closed.
# The UD raster is created from the density raster by slicing the values of the density raster into 100 equal intervals. Cells in the UD raster are assigned a 1 if the corresponding cell in the density raster are in the band with the highest density. Cells in the UD raster are assigned a 100 if the corresponding cell in the density raster are in the lowest density band.
# The cell size if not specified it defaults to the length of the longest side of the extents envelope divided by 2000. This will result in a raster of approximately 4 million cells.
# The raster will get the projection specified by the environment (usually this is the projection of the input). The format of the raster is determined by the file name provided. If there is no extension, then the file is an ESRI GRID file, .tif and .img extension are typically used. The raster can also be created in a geodatabase
# 
# Usage:
# Fixme
#
# Parameter 1:
# Locations_Layer
# Fixme
#
# Parameter 2:
# UD_Raster
# Fixme
#
# Parameter 3:
# Smoothing_Factor
# Fixme
#
# Scripting Syntax:
# UD_Raster_AnimalMovement (Locations_Layer, UD_Raster, Smoothing_Factor)
#
# Example1:
# Scripting Example
# The following example shows how this script can be used in the ArcGIS Python
# Window. It assumes that the script has been loaded into a toolbox,
# and the toolbox has been loaded into the active session of ArcGIS.
# It creates a UD raster with a smoothing factor of 4500
#  raster = r"C:\tmp\kde.tif"
#  donuts = r"C:\tmp\test.gdb\ud_donuts"
#  UD_Raster(4500)
#
# Example2:
# Command Line Example
# The following example shows how the script can be used from the operating
# system command line. It assumes that the script and the data sources are
# in the current directory and that the python interpeter is the path.
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
import os
import arcpy


def IsFloat(something):
    try:
        float(something)
    except (ValueError, TypeError):
        return False
    return True


def GetUDRaster(features, smoothingFactor, cellSize = None):
    cellSize, searchRadius = SetupRaster(features, smoothingFactor, cellSize)
    return GetProbabilityRaster(features, cellSize, searchRadius)


def SetupRaster(features, smoothingFactor, cellSize = None):
    envelope = arcpy.Describe(features).Extent
    maxDivisions = 2000
    if not IsFloat(cellSize):
        cellSize =  GetCellSize(envelope, maxDivisions)
    else:
        cellSize = float(cellSize) # all parameters from ArcToolbox are text 
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
        arcpy.AddError(sys.exc_info()[1])
        return False, None


if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        arcpy.AddError("Unable to checkout the Spatial Analyst Extension.  Quitting.")
        sys.exit()        

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
        arcpy.AddError("No output requested. Quitting.")
        sys.exit()

    try:
        smoothingFactor = float(smoothingFactor)
    except (ValueError, TypeError):
        arcpy.AddError("Smoothing Factor was not a valid number.")
        sys.exit()
        
    if not locationLayer:
        arcpy.AddError("No location layer was provided. Quitting.")
        sys.exit()
        
    if not arcpy.Exists(locationLayer):
        arcpy.AddError("Location layer cannot be found. Quitting.")
        sys.exit()

    #
    # Create density raster(s)
    #
    gotRaster, raster = GetUDRaster(locationLayer, smoothingFactor, cellSize)
    if gotRaster and rasterName:
        raster.save(rasterName)
