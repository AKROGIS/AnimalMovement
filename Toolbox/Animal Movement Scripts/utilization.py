# -*- coding: utf-8 -*-
"""
Utilization Distribution.py
Created: 2011-10-06

Title:
Utilization Distribution

Tags:
contour, home, range, animal, tracking, telemetry, ecology, kernel, density

Summary:
Create the utilization polygons for a single animal

Usage:
This tool is the simplified way to call utilization_smoothing, utilization_raster, and utilization_isopleth in order.  It does nothing more and nothing less.
See those tools for additional information.

Parameter 1:
Location_Layer
Layer name (if in ArcMap) or path to a feature class of points (typically animal locations).  If a layer is used in ArcMap, and features are selected in that layer, only the selected featues are used, otherwise all the features in the layer's definition query are used in the analysis.  If this is a feature class then all the features are used in the analysis.  The distribution of the points should be evaluated to determine the appropriateness of this tool and the correct selection of input parameters.

Parameter 2:
hRef_Method
This is the method for calculating hRef, the reference (or base) smoothing factor for each data subset. Selecting the correct smoothing factor is key to meaningful results. Large smoothing factors may over smooth the results, adding area to the UD, and small smoothing factors may result in too much detail, and insufficient area in the UD.
Worton: Sqrt( (variation_in_x + variation_in_y)/2) / n^(1/6)
Tufto: (Sqrt(variation_in_x + variation_in_y)/2) / n^(1/6)
Fixed: User provided constant
References:
Worton and Tufto make assume about the distribution, correlation and variation of the data that should be verified on your dataset before using these values.
Tufto, J., Andersen, R. and Linnell, J. 1996. Habitat use and ecological correlates of home range size in a small cervid: the roe deer. J. Anim. Ecol. 65:715-724.
Worton, B.J. 1989. Kernel methods for estimating the utilization distribution in home-range studies. Ecology  70:164-168#

Parameter 3:
Fixed_hRef
This parameter is required if and only if the hRef Method is "Fixed".  The units of the smoothing factor are the same as the spatial coordinates of the location data.  If the data is unprojected then the the units are decimal degrees (this will probably yield incorrect or distorted results).

Parameter 4:
hRef_Modifier
The reference smoothing factor (hRef) can be adjusted in a number of ways to produce the final smoothing factor.
None: No adjustment is made; h = hRef
Proportion: A percentage of hRef is used. various investigators have suggested different percentages based on the type and distribution of the data under consideration.
LSCV: A least squares cross validation is done to select the value between 0.05*hRef and 2.0*hRef that minimizes the LSCV score (Worton1995) between all pairs of points. This function is not guaranteed to work correctly if there are duplicate locations (a warning is issued and duplicate points are offset by a unit amount). In addition, there is no guarantee of a minimum in the range checked (a warning is issued and hRef is used). LSCV is very slow, and is limited to no more than 2000 points in a data subset.
BCV2: This is the same as LSCV, except a slightly different scoring function (Sain et. al. 1994) is used.
References:
Worton, B. J. 1995. Using Monte Carlo simulation to evaluate kernel-based home range estimators. Journal of Wildlife Management 59:794-800.
Sain, S. R., K. A Baggerly, and D. W. Scott. 1994. Cross-validation of multivariate densities. Journal of the American Statistical Association 89:807-817

Parameter 5:
hRef_Proportion
This parameter is required if and only if the hRef Modifier is "Proportion". This is a percentage of hRef to use for the final smoothing factor.  Typical values are between .5 (50%) and 1 (100%), although any positive number is acceptable.

Parameter 6:
Isopleths
This is a list of isopleth values separated by commas, semicolons, or whitespace. The values provided should be appropriate for the input raster (integers between 1 and 99 for rasters created with the UD Raster tool).

Parameter 7:
Isopleth_Lines
The name of a new output polyline feature class. One of Lines, Polygons, or Donut_Polygons must be provided.  If this parameter is left blank, no lines will be created. The output feature class will have a field named 'contour' with the value of the isopleth, and one or more features for each isopleth requested that exists in the input raster.  There may be multiple polylines for each isopleth.  Polylines may not close, but they should if the input is a UD Raster from the UD Raster tool.
No smoothing is done, and depending on the cell size the output can be very dense (small cell size), or very blocky (large cell size)

Parameter 8:
Isopleth_Polygons
Name of the new output polygon feature class. One of Lines, Polygons, or Donut_Polygons must be provided
Contains a polygon for each isopleth.  Each polygon contains the entire are covered by the isopleth. These polygons are overlapping.  The polygons are written to the featureclass with the largest isopleth values first. (for UD analysis, this provides a correctly stacked results set). These polygons are created from the isopleth lines

Parameter 9:
Isopleth_Donuts
Name of the new output polygon feature class. One of Lines, Polygons, or Donut_Polygons must be provided
Contains a polygon for each isopleth range.  Assumes the isopleths are ordered with the largest values containing the most area (so the last range is a donut without a hole). There is no donut for the first range range (i.e. from the universe to the first isopleth). These polygons are created from the lines

Parameter 10:
Output_Projection
Calculations and output must be done in a projected coordinate system (i..e not geographic - lat/long).  The projected coordinate system to use can be specified in three ways, 1) with this parameter, 2) with the output coordinate system in the environment, or 3) with the coordinate system of the input.  These options are listed in priority order, that is this paraeter will trump the environment, and the environment will trump the input data. if a projected coordinate system is not found then the program will abort.

Parameter 11:
Cell_Size
The width and height of the cells in the kernel raster (in units of the output coordinate system).  The default is the cell size that will result in a raster with at least 2000 cells in the vertical and horizontal direction.

Scripting Syntax:
UtilizationDistribution (Location_Layer, hRef_Method, Fixed_hRef, hRef_Modifier, hRef_Proportion, Isopleths, Isopleth_Lines, Isopleth_Polygons, Isopleth_Donuts, Output_Projection, Cell_Size)

Example1:
Scripting Example
The following example shows how this script can be used in the ArcGIS Python Window. It assumes that the script has been loaded into a toolbox, and the toolbox has been loaded into the active session of ArcGIS.
It creates the 65%, 90% UD polylines for alla single animal using the smoothing facter as determined by LSCV
 fixes = r"C:\tmp\test.gdb\locations"
 lines = r"C:\tmp\test.gdb\contours"
 UtilizationDistribution_AnimalMovement (fixes, "Worton", "", "LSCV", "", "65;90", lines, "", "")

Example2:
Command Line Example
The following example shows how the script can be used from the operating system command line. It assumes that the script and the data sources are in the current directory and that the python interpeter is the path.
It creates the 65%, 90% UD polylines for alla single animal using the smoothing facter as determined by LSCV
 C:\folder> python UtilizationDistribution.py locations.shp Worton # LSCV # "65;90" test.gdb\contours

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

import arcpy
import numpy

import utilization_smoothing
import utilization_raster
import utilization_isopleth
import utils


if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        utils.die("Unable to checkout the Spatial Analyst Extension.  Quitting.")

    locationLayer = arcpy.GetParameterAsText(0)
    hRefmethod = arcpy.GetParameterAsText(1)
    fixedHRef = arcpy.GetParameterAsText(2)
    modifier = arcpy.GetParameterAsText(3)
    proportionAmount = arcpy.GetParameterAsText(4)
    isoplethInput = arcpy.GetParameterAsText(5)
    isoplethLines = arcpy.GetParameterAsText(6)
    isoplethPolys = arcpy.GetParameterAsText(7)
    isoplethDonuts = arcpy.GetParameterAsText(8)
    spatialReference =  arcpy.GetParameter(9)
    cellSize = arcpy.GetParameterAsText(10)

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
        spatialReference = arcpy.SpatialReference()
        spatialReference.loadFromString("PROJCS['NAD_1983_Alaska_Albers',GEOGCS['GCS_North_American_1983',DATUM['D_North_American_1983',SPHEROID['GRS_1980',6378137.0,298.257222101]],PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]],PROJECTION['Albers'],PARAMETER['False_Easting',0.0],PARAMETER['False_Northing',0.0],PARAMETER['Central_Meridian',-154.0],PARAMETER['Standard_Parallel_1',55.0],PARAMETER['Standard_Parallel_2',65.0],PARAMETER['Latitude_Of_Origin',50.0],UNIT['Meter',1.0]];-13752200 -8948200 10000;-100000 10000;-100000 10000;0.001;0.001;0.001;IsHighPrecision")
        cellSize = ""

    #
    # Input validation
    #
    if modifier.lower() == "proportion":
        try:
            proportionAmount = float(proportionAmount)
        except ValueError:
            utils.die("Proportion Amount was not a valid number. Quitting.")

    if hRefmethod.lower() == "fixed":
        try:
            fixedHRef = float(fixedHRef)
        except ValueError:
            utils.die("Fixed hRef was not a valid number. Quitting.")

    if not locationLayer:
        utils.die("No location layer was provided. Quitting.")

    if not arcpy.Exists(locationLayer):
        utils.die("Location layer cannot be found. Quitting.")

    if not (isoplethLines or isoplethPolys or isoplethDonuts):
        utils.die("No output requested. Quitting.")

    isoplethList = utilization_isopleth.GetIsoplethList(isoplethInput)
    if not isoplethList:
        utils.die("List of valid isopleths is empty. Quitting.")

    desc = arcpy.Describe(locationLayer) #Describe() is expensive, so do it only once
    shapeName = desc.shapeFieldName
    inputSR = desc.spatialReference
    usingInputSR = False

    if not spatialReference or not spatialReference.name:
        spatialReference = arcpy.env.outputCoordinateSystem

    if not spatialReference or not spatialReference.name:
        usingInputSR = True
        spatialReference = inputSR

    if not spatialReference or not spatialReference.name:
        utils.die("The fixes layer does not have a coordinate system, and you have not provided one. Quitting.")

    if spatialReference.type != 'Projected':
        utils.die("The output projection is '" + spatialReference.type + "'.  It must be a projected coordinate system. Quitting.")

    if usingInputSR or (inputSR and spatialReference and spatialReference.factoryCode == inputSR.factoryCode):
        spatialReference = None

    #
    # Calculate smoothing factor
    #
    if hRefmethod.lower() == "fixed":
        h = fixedHRef
    else:
        points = utils.GetPoints(locationLayer, spatialReference, shapeName)
        h = utilization_smoothing.GetSmoothingFactor(points, hRefmethod, modifier, proportionAmount)
    #
    # Create density raster
    #
    gotRaster, raster = utilization_raster.GetUDRaster(locationLayer, h, spatialReference, cellSize)
    if gotRaster:
        utils.info("Created the temporary KDE raster")
    else:
        utils.die("Unable to create KDE raster. Quitting.")
    #
    # Create isopleths
    #
    utilization_isopleth.CreateIsopleths(isoplethList, raster, isoplethLines, isoplethPolys, isoplethDonuts)