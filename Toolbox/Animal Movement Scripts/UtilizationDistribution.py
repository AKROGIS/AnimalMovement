# ------------------------------------------------------------------------------
# UD_Isopleths.py
# Created: 2011-10-06
#
# Title:
# Utilization Distribution Isopleths
#
# Tags:
# contour, home, range, animal, tracking, telemetry, ecology, kernel, density
#
# Summary:
# Creates polylines and/or polygons based on the requested list of isopleths
# Input is a probability raster (typically created from the UD_Raster tool)
#
# Usage:
# Fixme
#
# Parameter 1:
# Isopleth_Values
# Fixme
#
# Parameter 2:
# Raster_Layer
# Fixme
#
# Parameter 3:
# Isopleth_Lines
# Fixme
#
# Parameter 4:
# Isopleth_Polygons
# Fixme
#
# Parameter 5:
# Isopleth_Donuts
# Fixme
#
# Scripting Syntax:
# UD_Isopleths_AnimalMovement (Isopleth_Values, Raster_Layer, Isopleth_Lines,
#                              Isopleth_Polygons, Isopleth_Donuts)
#
# Example1:
# Scripting Example
# The following example shows how this script can be used in the ArcGIS Python
# Window. It assumes that the script has been loaded into a toolbox,
# and the toolbox has been loaded into the active session of ArcGIS.
# It creates the 65%, 90% UD polygons (with holes) in a file geodatabase
#  raster = r"C:\tmp\kde.tif"
#  donuts = r"C:\tmp\test.gdb\ud_donuts"
#  UD_Isopleths("65;90", raster, "", "", donuts)
#
# Example2:
# Command Line Example
# The following example shows how the script can be used from the operating
# system command line. It assumes that the script and the data sources are
# in the current directory and that the python interpeter is the path.
# It creates the 65%, 90% UD polygons in a file geodatabase
#  C:\folder> python UD_Isopleths.py "50,90,95" kde.tif # test.gdb\ud_poly # 
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
import numpy

import UD_SmoothingFactor
import UD_Raster
import UD_Isopleths

if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        arcpy.AddError("Unable to checkout the Spatial Analyst Extension.  Quitting.")
        sys.exit()        

    locationLayer = arcpy.GetParameterAsText(0)
    hRefmethod = arcpy.GetParameterAsText(1)
    fixedHRef = arcpy.GetParameterAsText(2)
    modifier = arcpy.GetParameterAsText(3)
    proportionAmount = arcpy.GetParameterAsText(4)
    isoplethInput = arcpy.GetParameterAsText(5)
    isoplethLines = arcpy.GetParameterAsText(6)
    isoplethPolys = arcpy.GetParameterAsText(7)
    isoplethDonuts = arcpy.GetParameterAsText(8)
    #scaleToUnitVariance = arcpy.GetParameterAsText(9)

    test = False
    if test:
        locationLayer = r"C:\tmp\test2.gdb\w2011a0901"
        hRefmethod = "WORTON" #Worton,Tufto,Silverman,Gaussian,Fixed
        fixedHRef = "4000"
        modifier = "NONE" #PROPORTION,LSCV,BCV2, none/null
        proportionAmount = "0.7"
        isoplethInput = "50,65,90,95"
        isoplethLines = r"C:\tmp\test2.gdb\clines4a"
        isoplethPolys = r"C:\tmp\test2.gdb\cpolys4a"
        isoplethDonuts = r"C:\tmp\test2.gdb\cdonut4a"
        #scaleToUnitVariance = "False"

    #
    # Input validation
    #
    if modifier.lower() == "proportion":
        try:
            proportionAmount = float(proportionAmount)
        except ValueError:
            arcpy.AddError("Proportion Amount was not a valid number. Quitting.")
            sys.exit()
            
    if hRefmethod.lower() == "fixed":
        try:
            fixedHRef = float(fixedHRef)
        except ValueError:
            arcpy.AddError("Fixed hRef was not a valid number. Quitting.")
            sys.exit()
                
    if not locationLayer:
        arcpy.AddError("No location layer was provided. Quitting.")
        sys.exit()
        
    if not arcpy.Exists(locationLayer):
        arcpy.AddError("Location layer cannot be found. Quitting.")
        sys.exit()

    if not (isoplethLines or isoplethPolys or isoplethDonuts):
        arcpy.AddError("No output requested. Quitting.")
        sys.exit()

    isoplethList = UD_Isopleths.GetIsoplethList(isoplethInput)
    if not isoplethList:
        arcpy.AddError("List of valid isopleths is empty. Quitting.")
        sys.exit()

    #
    # Calculate smoothing factor
    #
    if hRefmethod.lower() == "fixed":
        h = fixedHRef
    else:
        points = UD_SmoothingFactor.GetPoints(locationLayer)
        h = UD_SmoothingFactor.GetSmoothingFactor(points, hRefmethod, modifier, proportionAmount)
    #
    # Create density raster
    #
    gotRaster, raster = UD_Raster.GetUDRaster(locationLayer, h)
    if gotRaster:
        arcpy.AddMessage("Created the temporary KDE raster")
    else:
        arcpy.AddError("Unable to create KDE raster. Quitting.")   
    #
    # Create isopleths
    #
    UD_Isopleths.CreateIsopleths(isoplethList, raster, isoplethLines, isoplethPolys, isoplethDonuts)