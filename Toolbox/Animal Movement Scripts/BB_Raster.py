
#Issues:
# - Output raster has no spatial reference
# - Input must be projected (future will support WGS84, with a user selected projection)
# - If we are calculating a number of rasters that we want to add or compare,
#     we will need to use a standard extents/cellsize - make this user definable
# - Grouping does not work (use per group or per dataset extents/cellsize?)
# - Does not support line feature classes
# - May not find mobility variance - product may overflow or underflow (to 0)
# - Optimizing technique may clip the results (I only calculate cells within a buffer distance of the path)



# ------------------------------------------------------------------------------
# BB_Raster.py
# Created: 2011-12-06
#
# Title:
# Brownian Bridge Distribution Raster
#
# Tags:
# home, range, animal, tracking, telemetry, ecology, probability, utilization, density, mobility, random, walk, location, variance 
#
# Summary:
# Creates a probability density raster based on brownian motion (random walk) bridging temporily adjacent locations.
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
import math
import os
import arcpy
import numpy
import datetime
import Brownian


def IsFloat(something):
    try:
        float(something)
    except (ValueError, TypeError):
        return False
    return True

def IsInt(something):
    try:
        int(something)
    except (ValueError, TypeError):
        return False
    return True


def GetGridExtents(featureExtents, varianceLocation, varianceMobility):
    #variance = sigma^2; 3sigma = 99% probability
    sigmaLocation = math.sqrt(varianceLocation)
    sigmaMobility = math.sqrt(varianceMobility)
    buffer = 3*sigmaLocation + 3*sigmaMobility
    return arcpy.Extent(featureExtents.XMin-buffer,
                        featureExtents.YMin-buffer,
                        featureExtents.XMax+buffer,
                        featureExtents.YMax+buffer)


def CalcCellSize(extents, countOfFixes, countOfIntervals, timePerUnit, totalTime):
    """
    Return the cellsize based on the time it will take to create the raster.save

    The time to create the raster is based on the number of cells in the grid,
    and the number of calculation at cell times the time for each calculation.
    The user specifies the time they are willing to spend, which will determine
    The number of cells, from which the cellSize can be determined.

    """    
    timePerCell = countOfFixes * countOfIntervals * timePerUnit
    numberOfCells = totalTime / timePerCell
    gridRatio = (extents.XMax - extents.XMin) / (extents.YMax - extents.YMin) # X/Y = cols/rows
    countOfRows = math.sqrt(numberOfCells/gridRatio)
    cellSize = (extents.YMax - extents.YMin)/countOfRows
    return cellSize


def GetGroupings(groupingFields, dateFieldDelimited):
    results = {}
    results[""] = dateFieldDelimited + " is not null"
    return results


def GetMinutes(newTime, firstTime):
    if firstTime == None:
        return 0
    else:
        if type(newTime) == datetime.datetime:
            delta = newTime - firstTime
            minutes = delta.days*24*60 + delta.seconds / 60.0
            return minutes
        if type(newTime) == float:
            #assume units are in minutes
            return newTime - firstTime
        if type(newTime) == int:
            return newTime - firstTime
    raise TypeError, "Unsupported Date type"


def GetFixedRasterName(rasterName, groupName):
    if not groupName:
        return rasterName
    else:
        raise NameError, "GetFixRasterName Function not complete"
    
    
def BuildFixesFromLines(features, shapeFieldName, dateField, groupingFields,
                        locationVarianceField, mobilityVarianceConstant, spatialReference):
    pass


def BuildFixesFromPoints(features, shapeFieldName, dateField, groupingFields,
                         locationVarianceField, mobilityVarianceField, spatialReference):
    
    fieldNames = [f.name for f in arcpy.ListFields(features)]
    if not dateField or dateField not in fieldNames:
        raise ValueError, "date field is not found in the dataset"

    #FIXME - verify field types   
    dateFieldDelimited = arcpy.AddFieldDelimiters(features, dateField)
    sort = dateField  + " A"
    fields = shapeFieldName + ";" + dateField
    if locationVarianceField and locationVarianceField in fieldNames:
        fields += ";" + arcpy.AddFieldDelimiters(features, locationVarianceField)
    else:
        locationVarianceField = None
    if mobilityVarianceField and mobilityVarianceField in fieldNames:
        fields += ";" + arcpy.AddFieldDelimiters(features, mobilityVarianceField)
    else:
        mobilityVarianceField = None
    spatialRef = spatialReference
    
    results = {}
    #print groupingFields, dateFieldDelimited
    for groupName, whereClaus in GetGroupings(groupingFields, dateFieldDelimited).iteritems():
        #print groupName, whereClaus
        #arcpy.AddMessage("Where = " + where + " Fields = " + fields + " Sort = " + sort)
        #FIXME - ESRI BUG - reprojection does not work if the data is in a FGDB and a sort order is given.
        #gp.AddMessage("Spatial Ref = " + spatialRef.Name)
        fixes = []
        firstTime = None
        #print whereClaus, spatialRef, fields, sort
        points = arcpy.SearchCursor(features, whereClaus, spatialRef, fields, sort)
        for point in points:
            fix = [0,0,0,0,0]
            newTime = point.getValue(dateField)
            if firstTime == None:
                firstTime = newTime
            fix[0] = GetMinutes(newTime, firstTime)
            fix[1] = point.getValue(shapeFieldName).getPart().X
            fix[2] = point.getValue(shapeFieldName).getPart().Y
            if locationVarianceField:
                fix[3] = point.getValue(locationVarianceField)
            if mobilityVarianceField:
                fix[4] = point.getValue(mobilityVarianceField)
            fixes.append(fix)
            
        results[groupName] = fixes
        print "fixes", len(fixes), "first fix:", fixes[0]
        arcpy.AddMessage("fixes "+ str(len(fixes)) +" first fix: " + str(fixes[0]))
    return results
    

def EstimateLocationVariance():
    #Estimating locational variance.
    # Method 1:
    #  use 832m^2 (sigma = 28.85m), the experimental result from 18 Lotex 3300L collars
    #  in 48 test sites in Idaho. Reference: Horne, et al (2007)
    # Method 2:
    #  use a 10m EPE value (typical for a Garmin).  Garmin EPE = CEP = 50% probality = 1.18 sigma
    #  1 sigma = 10m/1.18 = 8.47m; variance = sigma^2, therefore location_variance = 71.82m
    #  reference: http://gpsinformation.net/main/errors.htm
    return 832.0

# Regarding optimization
#  for some animal paths, much of the grid may be empty, if we create a buffer on the animals
#  path, then we can check each grid point and only analyze those close to the path.
#  building a buffer, GetBufferForPath() with 444 segments took 0.7 seconds (pretty much negligible)
#  checking 50 columns and 68 rows for intersection with buffer took 2.18 seconds (0.00064 seconds
#  per point.  Time to calculate each interval is approximately 0.000002.  Therefore a problem
#  with 0.00064/0.000002 = 320 intervals (or 16 segments) takes as long to check a grid point as
#  it does to calculate it.  Most problems will be much larger and the time to check will be a
#  small percent to the time to fully analyze.
#  if x is the time to check to see if a grid point should be analyzed,
#  and m is the path size (number of segments * the number of intervals/segment) 
#  and y is the time to analyze a grid point (y=m*c)
#  and n is the number of grid points.
#  and p is the percentage of points that can be skipped (i.e. outside buffer)
#  then it is worth optimizing if n*(x + (1-p)*y) < n*y
#  simplifing yields  x/y < p.  or x/c < p*m
#  using the experimental results of x = 0.00064 and c = 0.000002,  p*m > 320
#  if we assume the default of 20 intervals per segment (s), then p*s > 16
#  if there are 16 segments or more, then p > 100% (i.e. no cells to check - impossible)
#  if there are 32 segments  then we must have at least 50% empty cells.
#  if there are 160 segments then we must have at least 10% empty cells.
#  most problems will be well more than 10%




def  GetBufferForPath(extents, cellSize, fixes, maxTime):
    """
    Comments Here.
    """
    # Get the path and maximum total variance for buffer size.
    maxVarianceLocation = 0
    maxVarianceMobility = 0
    pointList = []
    for fix in fixes:
        pointList.append(arcpy.Point(fix[1], fix[2]))
        if fix[3] > maxVarianceLocation:
            maxVarianceLocation = fix[3]
        if fix[4] > maxVarianceMobility:
            maxVarianceMobility = fix[4]
    sigmaLocation = math.sqrt(maxVarianceLocation)
    sigmaMobility = math.sqrt(maxVarianceMobility * maxTime)
    buffer = 3*(sigmaLocation) + 3*sigmaMobility
    # Create a buffer polygon around the path.
    polyline = arcpy.Polyline(arcpy.Array(pointList))
    empty = arcpy.Geometry()
    geometryList = arcpy.Buffer_analysis(polyline, empty, buffer)
    return geometryList[0]


def CreateBBRaster(extents, cellSize, searchArea, fixes, intervals, spatialReference):
    """Returns an arcpy.raster by building a brownian bridge grid,
    and converting that to a numpy.array then to a arcpy.raster.
    See CreateBBGrid() for details on building the grid."""

    start = datetime.datetime.now()

    #grid = Brownian.CreateBBGrid(extents.XMin, extents.XMax, extents.YMin, extents.YMax, cellSize, fixes, intervals, None, None)
    grid = Brownian.CreateBBGrid(extents.XMin, extents.XMax, extents.YMin, extents.YMax, cellSize, fixes, intervals, searchArea, arcpy)

    print "Got Grid", datetime.datetime.now() - start
    arcpy.AddMessage("Got Grid " + str(datetime.datetime.now() - start))
    start = datetime.datetime.now()

    array = numpy.array(grid)
    lowerLeft = arcpy.Point(extents.XMin, extents.YMin)
    raster = arcpy.NumPyArrayToRaster(array, lowerLeft, cellSize)
    arcpy.DefineProjection_management(raster,spatialReference)

    print "Got raster", datetime.datetime.now() - start
    arcpy.AddMessage("Got raster " + str(datetime.datetime.now() - start))
    return raster


def BrownianBridge(features, rasterName, dateField, groupingFields,
                   cellSizeConstant, intervalsConstant,
                   locationVarianceField, locationVarianceConstant,
                   mobilityVarianceField, mobilityVarianceConstant,
                   spatialReference):
    """Features can be a line or point feature.  They must be projected, or a projection must be provided.
    They must have a time field, x field, y field.
    Locational variance must be provided as a field name, or as a constant value.
    Mobility variance can be provided as a field name, or as a constant value, if neither is provided,
    then a mobility variance is estimated and applied at all fixes.
    A group of fields can be provided to group the features.  There will be one raster per group."""
    try:

        start = datetime.datetime.now()
        print "Begin processing Brownian Bridge", start
        print "Reading features..."
        arcpy.AddMessage("Reading features...")

        #Get Intervals
        if IsInt(intervalsConstant):
            intervals = int(intervalsConstant)
        else:
            intervals = 10
                
        #get a collection of fixes, one for each grouping.
        describe = arcpy.Describe(features)
        shapeFieldName = describe.shapeFieldName
        shapeType = describe.shapeType
        if shapeType.lower() == 'polyline':
            fixSets = BuildFixesFromLines(features, shapeFieldName, dateField, groupingFields,
                                          locationVarianceField, mobilityVarianceField, spatialReference)
        elif shapeType.lower() == 'point':
            fixSets = BuildFixesFromPoints(features, shapeFieldName, dateField, groupingFields,
                                           locationVarianceField, mobilityVarianceField, spatialReference)
        else:
            raise TypeError, "Features must be points or lines."

        print "Got Fix Sets", datetime.datetime.now() - start
        arcpy.AddMessage("Got Fix Sets " + str(datetime.datetime.now() - start))
        start = datetime.datetime.now()

        for groupName,fixes in fixSets.iteritems():
            #print "groupName",groupName, "len(fixes)",len(fixes)
            #print fixes[0]
            #print fixes[1]
            if len(fixes) < 2:
                if name == '':
                    raise ValueError, "The feature class does not have two or more fix locations."
                else:
                    print("The group named " + groupName + "does not have enough fix locations.  Skipping")
                    arcpy.AddWarning("The group named " + groupName + "does not have enough fix locations.  Skipping")
                    continue

            #Get Extents - based on selection of fixes, not feature class.
            xmin = xmax = fixes[0][1]
            ymin = ymax = fixes[0][2]
            prevTime = False
            maxTime = 0
            for fix in fixes:
                if fix[1] < xmin:
                    xmin = fix[1]
                if fix[1] > xmax:
                    xmax = fix[1]
                if fix[2] < ymin:
                    ymin = fix[2]
                if fix[2] > ymax:
                    ymax = fix[2]
                if not prevTime:
                    delta = fix[0] - prevTime
                    prevTime = fix[0]
                    if delta > maxTime:
                        maxTime = delta
            extents = arcpy.Extent(xmin, ymin, xmax, ymax)

            #print "Got extents",xmin, ymin, xmax, ymax
           
            #Get LocationalVariance
            if not locationVarianceField:
                if IsFloat(locationVarianceConstant):
                    locationVariance = float(locationVarianceConstant)
                else:
                    locationVariance = EstimateLocationVariance()
                for fix in fixes:
                    fix[3] = locationVariance
                maxLocationVariance = locationVariance
            else:
                maxLocationVariance = 0
                for fix in fixes:
                    if fix[3] == None or fix[3] <= 0:
                        raise ValueError, "Invalid location variance in data. Use constant or none for default."
                    if fix[3] > maxLocationVariance:
                        maxLocationVariance = fix[3]

            #print "maxLocationVariance",maxLocationVariance
            
            #Get MobilityVariance
            if not mobilityVarianceField:
                if IsFloat(mobilityVarianceConstant):
                    mobilityVariance = float(mobilityVarianceConstant)
                else:
                    print "  Calculating most likely mobility variance..."
                    arcpy.AddMessage("  Calculating most likely mobility variance...")
                    #FIXME, guess and scalefactor should depend on fix data,
                    # i.e number of fixes, estimate of Vm, max velocity or such.
                    # The method Brownian.MobilityVariance() will validate and adjust
                    # as necessary, but using good estimates reduces computation.
                    guess = 1000000.0
                    scaleFactor = 1e10
                    mobilityVariance = Brownian.MobilityVariance(fixes, guess, scaleFactor)
                    arcpy.AddMessage("  Got mobility variance of %.2f" % mobilityVariance)
                for fix in fixes:
                    fix[4] = mobilityVariance
                maxMobilityVariance = mobilityVariance
            else:
                maxMobilityVariance = 0
                for fix in fixes:
                    if fix[4] == None or fix[4] <= 0:
                        raise ValueError, "Invalid mobility variance in data. Use constant or none for default."
                    if fix[4] > maxMobilityVariance:
                        maxMobilityVariance = fix[4]

            print "  Got Variances: Vl", maxLocationVariance, "Vm", maxMobilityVariance, datetime.datetime.now() - start
            start = datetime.datetime.now()

            #Buffer the extents to include the variance            
            extents = GetGridExtents(extents, maxLocationVariance, maxMobilityVariance * maxTime)
            
            #Get cellSize
            overhead = 0.000000429  #per gridpoint
            secondsPerCheck = 0.00051443 #per gridpoint (includes overhead)
            secondsPerCalculation = 0.000002756 #236
            if IsFloat(cellSizeConstant):
                cellSize = float(cellSizeConstant) # all parameters from ArcToolbox are text
            else:
                #by default, limit the size of the grid to what can be processed in 1 minute.
                totalTime = 60 #seconds
                cellSize = CalcCellSize(extents, len(fixes), intervals, secondsPerCalculation, totalTime)

            print "  Got extents and cellsize", cellSize, datetime.datetime.now() - start
            start = datetime.datetime.now()

            searchArea = GetBufferForPath(extents, cellSize, fixes, maxTime)

            print "  Got buffer", datetime.datetime.now() - start
            arcpy.AddMessage("  Got buffer " + str(datetime.datetime.now() - start))
            start = datetime.datetime.now()

            PrintCellSizeEvaluation(cellSize, maxLocationVariance, maxMobilityVariance)
            PrintRunTime(extents, cellSize, searchArea, len(fixes), intervals, secondsPerCheck, secondsPerCalculation)

            if not ShouldUseSearchArea(extents, cellSize, searchArea, len(fixes), intervals, secondsPerCheck, secondsPerCalculation):
                searchArea = None
                "  Not using a search buffer"
                arcpy.AddMessage("")
            start = datetime.datetime.now()
            raster = CreateBBRaster(extents, cellSize, searchArea, fixes, intervals, spatialReference)
            newName = GetFixedRasterName(rasterName, groupName)
            if arcpy.Exists(newName):
                arcpy.Delete_management(newName)
            raster.save(newName)
            print "Done.", datetime.datetime.now() - start
            arcpy.AddMessage("Done " + str(datetime.datetime.now() - start))
    except:
        print(sys.exc_info()[1])
        arcpy.AddError(sys.exc_info()[1])

def ShouldUseSearchArea(extents, cellSize, searchArea, nFixes, intervals, secondsPerCheck, secondsPerCalculation):
    timeRatio = secondsPerCheck  / secondsPerCalculation
    gridArea = (extents.XMax - extents.XMin) * (extents.YMax -extents.YMin)
    percent = searchArea.area/gridArea
    return (1 - percent) > timeRatio / ((nFixes -1) * intervals)

def PrintCellSizeEvaluation(cellSize, varianceLocation, varianceMobility):
    sigmaLocation = math.sqrt(varianceLocation)
    sigmaMobility = math.sqrt(varianceMobility)
    print "  Calculated cell size", cellSize
    arcpy.AddMessage("  Calculated cell size %.2f" % cellSize)
    print "  To capture detail of location variance, cell size should be between", sigmaLocation/10, sigmaLocation
    print "  To capture detail of mobility variance, cell size should be between", sigmaMobility/10, sigmaMobility
    upperMinimum = min(sigmaLocation*2, sigmaMobility/2)
    lowerMinimum = min(sigmaLocation/2.5, sigmaMobility/10)
    print "  Prefered cell size is less than", upperMinimum
    print "  Prefered cell size is greater than", lowerMinimum
    arcpy.AddMessage("  Prefered cell size is between %.2f and %.2f"%(lowerMinimum,upperMinimum))
    if cellSize > upperMinimum:
        n = int(cellSize/upperMinimum)
        n = n*n
        print "  Processing will take",n,"times as long if cell size is", upperMinimum
        arcpy.AddMessage("  Processing will take %d times as long if cell size is %.2f"%(n,upperMinimum))
        

def PrintRunTime(extents, cellSize, searchArea, nFixes, intervals, secondsPerCheck, secondsPerCalculation):
    gridArea = (extents.XMax - extents.XMin) * (extents.YMax -extents.YMin)
    searchPercent = searchArea.area/gridArea
    msg = "  Buffer area %.0f, Grid Area %.0f, Percent to search %.2f" % (searchArea.area, gridArea, searchPercent*100)
    print msg
    arcpy.AddMessage(msg)
    skipPercent = 1 - searchPercent

    rows = int( 1 + (extents.YMax - extents.YMin) / cellSize)
    cols = int( 1 + (extents.XMax - extents.XMin) / cellSize)
    segments = nFixes-1
    n = cols * rows * segments * intervals
    
    msg =  "  cols %d, rows %d, segments %d, intervals %d, total %d" %(cols,rows,segments,intervals,n)
    print msg
    arcpy.AddMessage(msg)
    seconds = cols * rows * (secondsPerCheck + searchPercent * (segments*intervals) * secondsPerCalculation)
    seconds = int(seconds)
    if seconds > 60:
        minutes = seconds / 60
        seconds = seconds - minutes * 60
        if minutes > 60:
            hours = minutes / 60
            minutes = minutes - hours * 60
            if hours > 24:
                days = hours / 24
                hours = hours - days * 24
                time = "%d days and %d hours" % (days, hours) 
            else:
                time = "%d hours and %d minutes" % (hours, minutes)
        else:
            time = "%d minutes and %d seconds" % (minutes, seconds)
    else:
        time = "%d seconds" % (seconds)
            
    print "  Estimated running time",time
    arcpy.AddMessage("  Estimated running time %s"%time)

if __name__ == "__main__":

    fixesLayer = arcpy.GetParameterAsText(0)
    rasterName = arcpy.GetParameterAsText(1)
    dateFieldName = arcpy.GetParameterAsText(2)
    groupingFieldNames = arcpy.GetParameterAsText(3)
    cellSizeConstant = arcpy.GetParameterAsText(4)
    IntegrationIntervalConstant = arcpy.GetParameterAsText(5)
    locationVarianceConstant = arcpy.GetParameterAsText(6)
    locationVarianceFieldName = arcpy.GetParameterAsText(7)
    mobilityVarianceConstant = arcpy.GetParameterAsText(8)
    mobilityVarianceFieldName = arcpy.GetParameterAsText(9)
    spatialReference = arcpy.GetParameter(10)

    test = False
    if test:
        #fixesLayer = r"C:\tmp\test2.gdb\migrate"
        fixesLayer = r"C:\tmp\LACL_Wolves.gdb\LC0906"
        #fixesLayer = r"C:\tmp\test2.gdb\bbtest"
        rasterName = r"C:\tmp\LACL_Wolves.gdb\LC0906_bb2"
        dateFieldName = "FixDate"
        groupingFieldNames = ""
        cellSizeConstant = "1000"
        IntegrationIntervalConstant = "#"
        locationVarianceFieldName = ""
        locationVarianceConstant = None
        mobilityVarianceFieldName = ""
        mobilityVarianceConstant = ""
        sr = arcpy.SpatialReference()
        sr.loadFromString("PROJCS['NAD_1983_Alaska_Albers',GEOGCS['GCS_North_American_1983',DATUM['D_North_American_1983',SPHEROID['GRS_1980',6378137.0,298.257222101]],PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]],PROJECTION['Albers'],PARAMETER['False_Easting',0.0],PARAMETER['False_Northing',0.0],PARAMETER['Central_Meridian',-154.0],PARAMETER['Standard_Parallel_1',55.0],PARAMETER['Standard_Parallel_2',65.0],PARAMETER['Latitude_Of_Origin',50.0],UNIT['Meter',1.0]];-13752200 -8948200 10000;-100000 10000;-100000 10000;0.001;0.001;0.001;IsHighPrecision")
        spatialReference = sr

    testHorne = False
    if testHorne:
        rasterName = r"C:\tmp\kd_test\bb_h_smallv.tif"
        extents = arcpy.Extent(-90,-180,370,180)
        cellSize = 1.0
        intervals = 10
        searchArea = None
        fixes = [
            #time, x, y, locational_variance, mobility_variance
            [ 0.0,   0, 0, 8.32, 6.42],
            [ 20.0, 280, 0, 8.32, 6.42],
        ]
        print "Horne Test", extents, cellSize, searchArea, fixes, intervals
        raster = CreateBBRaster(extents, cellSize, searchArea, fixes, intervals)
        #arcpy.Delete_management(rasterName)
        raster.save(rasterName)
        sys.exit()

    #
    # Input validation
    #
    if not rasterName:
        print("No output requested. Quitting.")
        arcpy.AddError("No output requested. Quitting.")
        sys.exit()

    if not fixesLayer:
        print("No fixes layer was provided. Quitting.")
        arcpy.AddError("No fixes layer was provided. Quitting.")
        sys.exit()
        
    if not arcpy.Exists(fixesLayer):
        print("Location layer cannot be found. Quitting.")
        arcpy.AddError("Location layer cannot be found. Quitting.")
        sys.exit()

    if spatialReference == None:
        spatialReference = arcpy.env.outputCoordinateSystem
        
    if spatialReference == None:
        spatialReference = arcpy.Describe(fixesLayer).spatialReference

    if spatialReference == None:
        print("The fixes layer does not have a coordinate system, and you have not provided one. Quitting.")
        arcpy.AddError("The fixes layer does not have a coordinate system, and you have not provided one. Quitting.")
        sys.exit()
        
    if spatialReference.type != 'Projected':
        print("The fixes layer is not a projected coordinate system, or you have not provided one. Quitting.")
        arcpy.AddError("The fixes layer is not a projected coordinate system, or you have not provided one. Quitting.")
        sys.exit()
        
    #
    # Create brownian bridge raster(s)
    #
    BrownianBridge(fixesLayer, rasterName, dateFieldName, groupingFieldNames,
                       cellSizeConstant, IntegrationIntervalConstant,
                       locationVarianceFieldName, locationVarianceConstant,
                       mobilityVarianceFieldName, mobilityVarianceConstant,
                       spatialReference
                   )


