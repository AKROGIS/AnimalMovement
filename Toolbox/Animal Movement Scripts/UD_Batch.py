# ------------------------------------------------------------------------------
# UD_Batch.py
# Created: 2011-10-06
#
# Title:
# Utilization Distribution Batch Process
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
# Location_Layer
# Layer name (if in ArcMap) or path to a feature class of points (typically animal locations).  If a layer is used in ArcMap, and features are selected in that layer, only the selected featues are used, otherwise all the features in the layer's definition query are used in the analysis.  If this is a feature class then all the features are used in the analysis.  The points should be in a projected coordinate system.  The distribution of the points should be evaluated to determine the appropriateness of this tool and the correct selection of input parameters.
#
# Parameter 2:
# Subset_Identifier
# The data set is subdivided by each unique value in this field.  Typically this is the name of the field with the animal identifiers, and the data is therefore grouped by animal.  Each data subset (i.e. each animal) is evaluated individually, and then the normalized results are combined for the final output.  A field must be provided, if you wish to analyze all the data without analyzing subsets first, then use the Utilization Distribution tool.
#
# Parameter 3:
# hRef_Method
# This is the method for calculating hRef, the reference (or base) smoothing factor for each data subset. Selecting the correct smoothing factor is key to meaningful results. Large smoothing factors may over smooth the results, adding area to the UD, and small smoothing factors may result in too much detail, and insufficient area in the UD.
# Worton: Sqrt( (variation_in_x + variation_in_y)/2) / n^(1/6)
# Tufto: (Sqrt(variation_in_x + variation_in_y)/2) / n^(1/6)
# Fixed: User provided constant
# Worton and Tufto make assume about the distribution, correlation and variation of the data that should be verified on your dataset before using these values.
# Tufto, J., Andersen, R. and Linnell, J. 1996. Habitat use and ecological correlates of home range size in a small cervid: the roe deer. J. Anim. Ecol. 65:715-724.
# Worton, B.J. 1989. Kernel methods for estimating the utilization distribution in home-range studies. Ecology  70:164-168#
#
# Parameter 4:
# Fixed_hRef
# This parameter is required if and only if the hRef Method is "Fixed".  The units of the smoothing factor are the same as the spatial coordinates of the location data.  If the data is unprojected then the the units are decimal degrees (this will probably yield incorrect or distorted results).
#
# Parameter 5:
# hRef_Modifier
# The reference smoothing factor (hRef) can be adjusted in a number of ways to produce the final smoothing factor.
# None: No adjustment is made; h = hRef
# Proportion: A percentage of hRef is used. various investigators have suggested different percentages based on the type and distribution of the data under consideration.
# LSCV: A least squares cross validation is done to select the value between 0.05*hRef and 2.0*hRef that minimizes the LSCV score (Worton1995) between all pairs of points. This function is not guaranteed to work correctly if there are duplicate locations (a warning is issued and duplicate points are offset by a unit amount). In addition, there is no guarantee of a minimum in the range checked (a warning is issued and hRef is used). LSCV is very slow, and is limited to no more than 2000 points in a data subset.
# BCV2: This is the same as LSCV, except a slightly different scoring function (Sain et. al. 1994) is used.
# Worton, B. J. 1995. Using Monte Carlo simulation to evaluate kernel-based home range estimators. Journal of Wildlife Management 59:794-800.
# Sain, S. R., K. A Baggerly, and D. W. Scott. 1994. Cross-validation of multivariate densities. Journal of the American Statistical Association 89:807-817
#
# Parameter 6:
# hRef_Proportion
# This parameter is required if and only if the hRef Modifier is "Proportion". This is a percentage of hRef to use for the final smoothing factor.  Typical values are between .5 (50%) and 1 (100%), although any positive number is acceptable.
#
# Parameter 7:
# hRef_To_Use
# A smoothing factor is calculated for each dataset per the previous parameters. This parameter clarifies which smoothing factor should be used for the analysis
# Minimum: The smallest of the subset smoothing factors is determined and then used to analyze all data subsets
# Maximum: The largest of the subset smoothing factors is determined and then used to analyze all data subsets
# Average: The average of the subset smoothing factors is determined and then used to analyze all data subsets
# ByDataset: Each data subset is analyzed with the smoothing factor specific to that subset.
#
# Parameter 8:
# Save_Rasters_
# Fixme
#
# Parameter 9:
# Raster_Folder
# Fixme
#
# Parameter 10:
# Isopleths
# Fixme
#
# Parameter 11:
# Isopleth_Lines
# Fixme
#
# Parameter 12:
# Isopleth_Polygons
# Fixme
#
# Parameter 13:
# Isopleth_Donuts
# Fixme
#
# Scripting Syntax:
# UD_Batch_AnimalMovement (Location_Layer, Subset_Identifier, hRef_Method, Fixed_hRef, hRef_Modifier, hRef_Proportion, hRef_To_Use, Save_Rasters_, Raster_Folder, Isopleths, Isopleth_Lines, Isopleth_Polygons, Isopleth_Donuts)
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



def GetSmoothingFactors(subsetIdentifier, uniqueValues, locationLayer, hRefmethod, modifier, proportionAmount):
    layer = "subsetForSmoothingFactor"
    hList = []
    for value in uniqueValues:
        query = UD_Isopleths.BuildQuery(locationLayer,subsetIdentifier,value)
        arcpy.AddMessage("Calculating h for " + query)
        if arcpy.Exists(layer):
            arcpy.Delete_management(layer)
        arcpy.MakeFeatureLayer_management(locationLayer, layer, query)
        try:
            points = UD_SmoothingFactor.GetPoints(layer)
            h = UD_SmoothingFactor.GetSmoothingFactor(points, hRefmethod, modifier, proportionAmount)
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

def BuildNormalizedRaster(subsetIdentifier, uniqueValues, locationLayer, hList, saveRasters, rasterFolder):
    n = 0
    layer = "subsetSelectionForRaster"
    cellSize, searchRadius = UD_Raster.SetupRaster(locationLayer, max(hList))
    hDict = {}
    for k,v in zip(uniqueValues,hList):
        hDict[k]=v
    for value in uniqueValues:
        query = UD_Isopleths.BuildQuery(locationLayer,subsetIdentifier,value)
        arcpy.AddMessage("Creating KDE raster for " + query)
        if arcpy.Exists(layer):
            arcpy.Delete_management(layer)
        arcpy.MakeFeatureLayer_management(locationLayer, layer, query)
        try:
            searchRadius = 2 * hDict[value]
            gotRaster, probRaster = UD_Raster.GetProbabilityRaster(layer, cellSize, searchRadius)
            if gotRaster:
                if saveRasters:
                    # Save individual probability rasters
                    name = os.path.join(rasterFolder,"praster_"+str(value)+".tif")
                    probRaster.save(name)
                if n:
                    raster = raster + probRaster
                    n = n + 1
                else:
                    raster = probRaster
                    n = 1
            else:
                arcpy.AddWarning("  Raster creation failed, not included in total.")                
        finally:
            arcpy.Delete_management(layer)

    if n == 0:
        return False, None
    raster = arcpy.sa.Slice(raster,100,"EQUAL_INTERVAL")
    if saveRasters:
        name = os.path.join(rasterFolder,"_praster_TOTAL.tif")
        raster.save(name)
    return True, raster

    

if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        arcpy.AddError("Unable to checkout the Spatial Analyst Extension.  Quitting.")
        sys.exit()        

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

    test = False
    if test:
        locationLayer = r"C:\tmp\test2.gdb\w2011a0901"
        subsetIdentifier = "AnimalId"
        hRefmethod = "WORTON" #Worton,Tufto,Silverman,Gaussian
        fixedHRef = "4000"
        modifier = "NONE" #NONE,PROPORTIONAL,LSCV,BCV2
        proportionAmount = "0.7"
        hRefToUse = "Average"
        saveRasters = "True"
        rasterFolder = r"C:\tmp\kd_test"
        isoplethInput = "50,65,90,95"
        isoplethLines = r"C:\tmp\test2.gdb\clines4a"
        isoplethPolys = r"C:\tmp\test2.gdb\cpolys4a"
        isoplethDonuts = r"C:\tmp\test2.gdb\cdonut4a"

    #
    # Input validation
    #
    if hRefmethod.lower() == "fixed":
        try:
            fixedHRef = float(fixedHRef)
        except ValueError:
            arcpy.AddError("Fixed hRef was not a valid number. Quitting.")
            sys.exit()
        
    if modifier.lower() == "proportion":
        try:
            proportionAmount = float(proportionAmount)
        except ValueError:
            arcpy.AddError("Proportion Amount was not a valid number. Quitting.")
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

    saveRasters = (saveRasters.lower() == "true")
    if saveRasters:
        if not os.path.exists(rasterFolder):
            os.mkdir(rasterFolder) #may throw an exception (thats ok)
        else:
            if not os.path.isdir(rasterFolder):
                arcpy.AddError(rasterFolder + " is not a folder. Quitting.")
                sys.exit()
                
    uniqueValues = None
    if subsetIdentifier in [field.name for field in arcpy.ListFields(locationLayer)]:
        uniqueValues = UD_Isopleths.GetUniqueValues(locationLayer,subsetIdentifier)
    if not uniqueValues:
        arcpy.AddError("Could not generate a list of unique values for "+subsetIdentifier+". Quitting.")
        sys.exit()

    #
    # Calculate smoothing factor(s)
    #               
    if hRefmethod.lower() == "fixed":
        hList = [fixedHRef for eachItem in uniqueValues]
    else:
        hList = GetSmoothingFactors(subsetIdentifier, uniqueValues, locationLayer, hRefmethod, modifier, proportionAmount)
        if hRefToUse.lower() != "bydataset":
            h = ChooseSmoothingFactor(hList, hRefToUse)
            arcpy.AddMessage("Using h = " + str(h) +" ("+hRefToUse+")")
            hList = [h for eachItem in uniqueValues]
    #
    # Create density raster(s)
    #
    gotRaster, raster = BuildNormalizedRaster(subsetIdentifier, uniqueValues, locationLayer, hList, saveRasters, rasterFolder)  
    if gotRaster:
        arcpy.AddMessage("Created the temporary KDE raster")
    else:
        arcpy.AddError("Unable to create KDE raster. Quitting.")
    #
    # Create isopleths (for total raster only)
    #
    UD_Isopleths.CreateIsopleths(isoplethList, raster, isoplethLines, isoplethPolys, isoplethDonuts)

