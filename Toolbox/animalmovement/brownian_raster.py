# -*- coding: utf-8 -*-
"""
BB_Raster.py
Created: 2011-12-06

Title:
Create Brownian Bridge Probability Raster

Tags:
home, range, animal, tracking, telemetry, ecology, probability, utilization,
density, mobility, random, walk, location, variance

Summary:
Creates a probability distribution raster depicting the probability of an animal
having been in a cell, based on some locations in time.

Usage:
The probability distribution raster is created using the Brownian Bridge method
described in  Analyzing Animal Movements Using Brownian Bridges by Jon S. Horn
 Et Al, Ecology, Vol 88, No. 9.

In brief, the Brownian Bridge method adds the locational variance (GPS error) at
the fix locations and the mobility variance (deviation from the straight line
path between temporally adjacent fixes using a random walk) to get a probability
distribution for the animal's location.

If the mobility variance is not provided, it is calclulated as follows: Assume a
mobility variance, vm. For the first three fixes. pretend that fix2 is missing,
and calculate the normal distribution at the observed location of fix2 based on
the predicted (part way between fix1 and fix3) mean location of fix2. Do this
again for fixes 2,3,4 then 3,4,5 etc until the last 3 fixes. The product of the
normal distributions is the indicator of the likelihood of the assumed mobility
variance.

This algorithm is computationally expensive. The time required to process a data
set is proportional to the number of cells times the number of locations times
the number of integration intervals. The number of cells is based on the extents
of the raster (fixed by the spatial distribution of the locations), and the cell
size. Increasing the integration intervals or decreasing the cell size will
increase the time required to obtain a solution, but will also increase the
resolution (or detail) available in the results. In most cases, there is no
benefit from adjusting the default value of the integration interval. If the
cell size is omitted, a cell size is chosen that should result in a the ability
to deliver a solution in less than a minute. The program will output the
cellsize used. Future runs can specify a cell size depending on the resolution
desired, and the amount of time you are willing to wait to obtain a result. The
processing time increases as the square of the reduction in cell size. For
example, if a cell size of 10000 meters takes 1 minute to run, a cellsize of
5000 meters will takes 4 minutes, a cell size of 1000 meters will take 100
minutes, and a cellsize of 100 meters will take 10,000 minutes (almost 7 days).
Choose your cell size carefully.

This tool tried to eliminate unnecessary processing by creating a buffer around
the animal's straight line path, and assigning a value of zero to cells outside
the buffer. The size of the buffer is based on the variance values, and it will
include all likely locations. However, if the data is tightly clustered, the
accumulation of overlapping low probabilities at the periphery may appear
cropped by this buffer. Contact the author if you think this is a problem for
your analysis.

Issues:
  * Output raster has no spatial reference
  * If we are calculating a number of rasters that we want to add or compare we
    will need to use a standard extents/cellsize - make this user definable
  * Grouping does not work (use per group or per dataset extents/cellsize?)
  * Does not support line feature classes
  * May not find mobility variance - product may overflow or underflow (to 0)
  * Optimizing technique may clip the results (I only calculate cells within a
    buffer distance of the path)#

Parameter 1:
Fixes
Fixes can be points or lines, but only points are supported at this time.

Parameter 2:
Raster
The results raster. A probability distribution raster with values in the range
from 0 to 1 indicating the probability of finding the animal in this cell
compared with the other cells. The total of all cells in the raster will equal
one.

Parameter 3:
Date_Field
Field for ordering the fixes for the walk through time. This is a temporary
(in memory only) sort. The data on disk is not changed.

Parameter 4:
Grouping_Fields
Grouping is not yet implemented.
The input dataset must represent only one animal
Each unique value (set of values) in the grouping fields will be used to create
a separate raster. These rasters will then be normalized and added to create
combined raster.

Parameter 5:
Cell_Size_Constant
Default cell size will be calculated based on a maximum running time of about
1 minute. You will probably want to run this again with a smaller cell size.
The program will provide some guidance on selecting a cell size based on your
data. Note: halving the cell size will quadruple the running time.

Parameter 6:
Integration_Intervals
Each segment is divided into this many intervals, for integrating the results
along the path. Increasing this number will increase the processing time,
but will provide more fine grained results.

Parameter 7:
Location_Variance_Constant
This is the GPS error. A default of 832m is used based on experimental results
from Horne, et al (2007) based on Lotex 3300L collars at 48 test sites in Idaho

Parameter 8:
Location_Variance_Field
A field in the database that provides locational variance for each fix

Parameter 9:
Mobility_Variance_Constant
A constant mobility variance value to use for all location points.
If this is not provided, a value is estimated based on Horne, et all (2007)

Parameter 10:
Mobility_Variance_Field
A field the provides the mobility variance at each fix

Parameter 11:
Output_Spatial_Reference
Calculations and output must be done in a projected coordinate system (i..e not
geographic - lat/long). The projected coordinate system to use can be specified
in three ways, 1) with this parameter, 2) with the output coordinate system in
the environment, or 3) with the coordinate system of the input. These options
are listed in priority order, that is this parameter will trump the environment,
and the environment will trump the input data. if a projected coordinate system
is not found then the program will abort.

Scripting Syntax:
BrownianBridgesRaster(
    Fixes, Raster, Date_Field, Grouping_Fields, Cell_Size_Constant,
    Integration_Intervals, Location_Variance_Constant, Location_Variance_Field,
    Mobility_Variance_Constant, Mobility_Variance_Field,
    Output_Spatial_Reference
)

Example1:
Scripting Example
The following example shows how this script can be used in the ArcGIS Python
Window. It assumes that the script has been loaded into a toolbox, and the
toolbox has been loaded into the active session of ArcGIS.

It creates a brownian probability raster with the default parameters
  fixes = r"C:\tmp\test.gdb\fixes"
  raster = r"C:\tmp\test.gdb\bb1"
  BrownianBridgesRaster (fixes, raster, "FixDate")

Example2:
Command Line Example
The following example shows how the script can be used from the operating system
command line. It assumes that the script and the data sources are in the current
directory and that the python interpeter is the path.

It creates a brownian probability raster with the default parameters and a cell
size of 100.
  C:\folder> BrownianBridgesRaster.py "test.gdb\fixes" "test.gdb\bb1" FixDate # 100

Credits:
Regan Sarwas, Alaska Region GIS Team, National Park Service

Limitations:
Public Domain

Requirements
  * arcpy module - requires ArcGIS v10.1+ and a valid license
  * arcpy.sa module - requires a valid license to Spatial Analyst Extension

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

import datetime
import math
import sys

import arcpy
import numpy

import brownian
import utils

# pylint: disable=invalid-name, missing-function-docstring
# TODO: Fix names and add doc strings


def GetGridExtents(featureExtents, varianceLocation, varianceMobility):
    # variance = sigma^2; 3sigma = 99% probability
    sigmaLocation = math.sqrt(varianceLocation)
    sigmaMobility = math.sqrt(varianceMobility)
    buffer = 3 * sigmaLocation + 3 * sigmaMobility
    return arcpy.Extent(
        featureExtents.XMin - buffer,
        featureExtents.YMin - buffer,
        featureExtents.XMax + buffer,
        featureExtents.YMax + buffer,
    )


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
    gridRatio = (extents.XMax - extents.XMin) / (
        extents.YMax - extents.YMin
    )  # X/Y = cols/rows
    countOfRows = math.sqrt(numberOfCells / gridRatio)
    cellSize = (extents.YMax - extents.YMin) / countOfRows
    return cellSize


def GetGroupings(groupingFields, dateFieldDelimited):
    results = {}
    results[""] = "{0} is not null".format(dateFieldDelimited)
    return results


def GetMinutes(newTime, firstTime):
    if firstTime == None:
        return 0
    else:
        if type(newTime) == datetime.datetime:
            delta = newTime - firstTime
            minutes = delta.days * 24 * 60 + delta.seconds / 60.0
            return minutes
        if type(newTime) == float:
            # assume units are in minutes
            return newTime - firstTime
        if type(newTime) == int:
            return newTime - firstTime
    raise TypeError("Unsupported Date type")


def GetFixedRasterName(rasterName, groupName):
    if not groupName:
        return rasterName
    else:
        raise NameError("GetFixRasterName Function not complete")


def BuildFixesFromLines(
    features,
    shapeFieldName,
    dateField,
    groupingFields,
    locationVarianceField,
    mobilityVarianceConstant,
    spatialReference,
):
    # TODO: Implement (or delete)
    return {}


def BuildFixesFromPoints(
    features,
    shapeFieldName,
    dateField,
    groupingFields,
    locationVarianceField,
    mobilityVarianceField,
    spatialReference,
):

    fieldNames = [f.name for f in arcpy.ListFields(features)]
    if not dateField or dateField not in fieldNames:
        raise ValueError("date field is not found in the dataset")

    # FIXME - verify field types
    dateFieldDelimited = arcpy.AddFieldDelimiters(features, dateField)
    sort = "ORDER BY {0}".format(dateField)
    fields = ["SHAPE@XY", dateField]
    if locationVarianceField and locationVarianceField in fieldNames:
        fields += [locationVarianceField]
    else:
        locationVarianceField = None
    if mobilityVarianceField and mobilityVarianceField in fieldNames:
        fields += [mobilityVarianceField]
    else:
        mobilityVarianceField = None
    if (
        spatialReference.factoryCode
        != arcpy.Describe(features).spatialReference.factoryCode
    ):
        # FIXME - ESRI BUG - reprojection does not work if the data is in a FGDB
        # and a sort order is given.
        sort = ""
        msg = (
            "Due to a bug in ArcGIS 10, data cannot be both sorted and projected "
            "on the fly. Since a projection is required for your data sorting is "
            "turned off. This is OK for data in the Animal Movements Database, "
            "however other data must be correctly pre-sorted by date or you will "
            "get incorrect results. If you can't guarantee pre-sorted data, then "
            "re-project your data first."
        )
        utils.warn(msg)

    results = {}
    # print(groupingFields, dateFieldDelimited)
    groupings = GetGroupings(groupingFields, dateFieldDelimited)
    for groupName in groupings:
        whereClaus = groupings[groupName]
        # print(groupName, whereClaus)
        # utils.info("Where = {0}; Fields = {1}; Sort = {1}".format(where, fields, sort))
        # FIXME - ESRI BUG - reprojection does not work if the data is in a
        # FGDB and a sort order is given.
        # utils.info("Spatial Ref = {0}".format(spatialRef.Name))
        fixes = []
        firstTime = None
        # print(whereClaus, spatialRef, fields, sort)
        # fields: 0 = shape, 1 = date, 2 = locVar, 3 = mobVar 2 and 3 are optional,
        # mobVar may be @ 2 if locVar is None
        with arcpy.da.SearchCursor(
            features,
            fields,
            where_clause=whereClaus,
            spatial_reference=spatialReference,
            sql_clause=(None, sort),
        ) as cursor:
            for row in cursor:
                fix = [0, 0, 0, 0, 0]
                newTime = row[1]
                if firstTime == None:
                    firstTime = newTime
                fix[0] = GetMinutes(newTime, firstTime)
                fix[1] = row[0][0]
                fix[2] = row[0][1]
                if locationVarianceField:
                    fix[3] = row[2]
                    if mobilityVarianceField:
                        fix[4] = row[3]
                else:
                    if mobilityVarianceField:
                        fix[4] = row[2]
                fixes.append(fix)

        results[groupName] = fixes
        utils.info("fixes {0}; first fix: {1}".format(len(fixes), fixes[0]))
    return results


def EstimateLocationVariance():
    # Estimating locational variance.
    # Method 1:
    #  use 832m^2 (sigma = 28.85m), the experimental result from 18 Lotex 3300L collars
    #  in 48 test sites in Idaho. Reference: Horne, et al (2007)
    # Method 2:
    #  use a 10m EPE value (typical for a Garmin). Garmin EPE = CEP = 50% probability = 1.18 sigma
    #  1 sigma = 10m/1.18 = 8.47m; variance = sigma^2, therefore location_variance = 71.82m
    #  reference: http://gpsinformation.net/main/errors.htm
    return 832.0


# Regarding optimization
#  for some animal paths, much of the grid may be empty, if we create a buffer
#  on the animals path, then we can check each grid point and only analyze those
#  close to the path. building a buffer, GetBufferForPath() with 444 segments
#  took 0.7 seconds (pretty much negligible) checking 50 columns and 68 rows for
#  intersection with buffer took 2.18 seconds (0.00064 seconds per point. Time
#  to calculate each interval is approximately 0.000002. Therefore a problem
#  with 0.00064/0.000002 = 320 intervals (or 16 segments) takes as long to check
#  a grid point as it does to calculate it. Most problems will be much larger
#  and the time to check will be a small percent to the time to fully analyze.
#  if x is the time to check to see if a grid point should be analyzed,
#  and m is the path size (number of segments * the number of intervals/segment)
#  and y is the time to analyze a grid point (y=m*c)
#  and n is the number of grid points.
#  and p is the percentage of points that can be skipped (i.e. outside buffer)
#  then it is worth optimizing if n*(x + (1-p)*y) < n*y
#  simplifying yields  x/y < p. or x/c < p*m
#  using the experimental results of x = 0.00064 and c = 0.000002,  p*m > 320
#  if we assume the default of 20 intervals per segment (s), then p*s > 16
#  if there are 16 segments or more, then p > 100% (i.e. no cells to check - impossible)
#  if there are 32 segments  then we must have at least 50% empty cells.
#  if there are 160 segments then we must have at least 10% empty cells.
#  most problems will be well more than 10%


def GetBufferForPath(extents, cellSize, fixes, maxTime):
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
    buffer = 3 * (sigmaLocation) + 3 * sigmaMobility
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

    # grid = brownian.CreateBBGrid(extents.XMin, extents.XMax, extents.YMin,
    # extents.YMax, cellSize, fixes, intervals, None, None)
    grid = brownian.CreateBBGrid(
        extents.XMin,
        extents.XMax,
        extents.YMin,
        extents.YMax,
        cellSize,
        fixes,
        intervals,
        searchArea,
        arcpy,
    )

    utils.info("Got Grid {0}".format(datetime.datetime.now() - start))
    start = datetime.datetime.now()

    array = numpy.array(grid)
    lowerLeft = arcpy.Point(extents.XMin, extents.YMin)
    raster = arcpy.NumPyArrayToRaster(array, lowerLeft, cellSize)
    arcpy.DefineProjection_management(raster, spatialReference)

    utils.info("Got raster {0}".format(datetime.datetime.now() - start))
    return raster


def create_raster(
    features,
    rasterName,
    dateField,
    groupingFields,
    cellSizeConstant,
    intervalsConstant,
    locationVarianceField,
    locationVarianceConstant,
    mobilityVarianceField,
    mobilityVarianceConstant,
    spatialReference,
):
    """
    Features can be a line or point feature.
    They must be projected, or a projection must be provided.
    They must have a time field, x field, y field.
    Locational variance must be provided as a field name, or as a constant value.
    Mobility variance can be provided as a field name, or as a constant value,
    if neither is provided, then a mobility variance is estimated and applied at
    all fixes. CAUTION: estimating mobility variance will likely fail.
    A group of fields can be provided to group the features. There will be one
    raster per group.
    """
    try:
        start = datetime.datetime.now()
        print("Begin processing Brownian Bridge", start)
        utils.info("Reading features...")

        # Get Intervals
        if utils.is_int(intervalsConstant):
            intervals = int(intervalsConstant)
        else:
            intervals = 10

        # get a collection of fixes, one for each grouping.
        desc = arcpy.Describe(features)
        shapeFieldName = desc.shapeFieldName
        shapeType = desc.shapeType
        if shapeType.lower() == "polyline":
            fixSets = BuildFixesFromLines(
                features,
                shapeFieldName,
                dateField,
                groupingFields,
                locationVarianceField,
                mobilityVarianceField,
                spatialReference,
            )
        elif shapeType.lower() == "point":
            fixSets = BuildFixesFromPoints(
                features,
                shapeFieldName,
                dateField,
                groupingFields,
                locationVarianceField,
                mobilityVarianceField,
                spatialReference,
            )
        else:
            raise TypeError("Features must be points or lines.")

        utils.info("Got Fix Sets {0}".format(datetime.datetime.now() - start))
        start = datetime.datetime.now()

        for groupName in fixSets:
            fixes = fixSets[groupName]
            # print("groupName",groupName, "len(fixes)",len(fixes))
            # print(fixes[0])
            # print(fixes[1])
            if len(fixes) < 2:
                msg = "The group named {0} does not have enough fix locations. Skipping"
                utils.warn(msg.format(groupName))
                continue

            # Get Extents - based on selection of fixes, not feature class.
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

            # print("Got extents",xmin, ymin, xmax, ymax)

            # Get LocationalVariance
            if not locationVarianceField:
                if utils.is_float(locationVarianceConstant):
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
                        raise ValueError(
                            "Invalid location variance in data. Use constant or none for default."
                        )
                    if fix[3] > maxLocationVariance:
                        maxLocationVariance = fix[3]

            # print("maxLocationVariance",maxLocationVariance)

            # Get MobilityVariance
            if not mobilityVarianceField:
                if utils.is_float(mobilityVarianceConstant):
                    mobilityVariance = float(mobilityVarianceConstant)
                else:
                    utils.info("  Calculating most likely mobility variance...")
                    # FIXME, guess and scalefactor should depend on fix data,
                    # i.e number of fixes, estimate of Vm, max velocity or such.
                    # The method brownian.MobilityVariance() will validate and adjust
                    # as necessary, but using good estimates reduces computation.
                    guess = 1000000.0
                    scaleFactor = 1e10
                    mobilityVariance = brownian.MobilityVariance(
                        fixes, guess, scaleFactor
                    )
                    utils.info("  Got mobility variance of %.2f" % mobilityVariance)
                for fix in fixes:
                    fix[4] = mobilityVariance
                maxMobilityVariance = mobilityVariance
            else:
                maxMobilityVariance = 0
                for fix in fixes:
                    if fix[4] == None or fix[4] <= 0:
                        raise ValueError(
                            "Invalid mobility variance in data. Use constant or none for default."
                        )
                    if fix[4] > maxMobilityVariance:
                        maxMobilityVariance = fix[4]

            print(
                "  Got Variances: Vl",
                maxLocationVariance,
                "Vm",
                maxMobilityVariance,
                datetime.datetime.now() - start,
            )
            start = datetime.datetime.now()

            # Buffer the extents to include the variance
            extents = GetGridExtents(
                extents, maxLocationVariance, maxMobilityVariance * maxTime
            )

            # Get cellSize
            overhead = 0.000000429  # per gridpoint
            secondsPerCheck = 0.00051443  # per gridpoint (includes overhead)
            secondsPerCalculation = 0.000002756  # 236
            if utils.is_float(cellSizeConstant):
                cellSize = float(
                    cellSizeConstant
                )  # all parameters from ArcToolbox are text
            else:
                # by default, limit the size of the grid to what can be processed in 1 minute.
                totalTime = 60  # seconds
                cellSize = CalcCellSize(
                    extents, len(fixes), intervals, secondsPerCalculation, totalTime
                )

            print(
                "  Got extents and cellsize", cellSize, datetime.datetime.now() - start
            )
            start = datetime.datetime.now()

            searchArea = GetBufferForPath(extents, cellSize, fixes, maxTime)

            utils.info("  Got buffer {0}".format(datetime.datetime.now() - start))
            start = datetime.datetime.now()

            PrintCellSizeEvaluation(cellSize, maxLocationVariance, maxMobilityVariance)
            PrintRunTime(
                extents,
                cellSize,
                searchArea,
                len(fixes),
                intervals,
                secondsPerCheck,
                secondsPerCalculation,
            )

            if not ShouldUseSearchArea(
                extents,
                cellSize,
                searchArea,
                len(fixes),
                intervals,
                secondsPerCheck,
                secondsPerCalculation,
            ):
                searchArea = None
                utils.info("  Not using a search buffer")
            start = datetime.datetime.now()
            raster = CreateBBRaster(
                extents, cellSize, searchArea, fixes, intervals, spatialReference
            )
            newName = GetFixedRasterName(rasterName, groupName)
            if arcpy.Exists(newName):
                arcpy.Delete_management(newName)
            raster.save(newName)
            utils.info("Done {0}".format(datetime.datetime.now() - start))
    except:
        utils.die(sys.exc_info()[1])


def ShouldUseSearchArea(
    extents,
    cellSize,
    searchArea,
    nFixes,
    intervals,
    secondsPerCheck,
    secondsPerCalculation,
):
    timeRatio = secondsPerCheck / secondsPerCalculation
    gridArea = (extents.XMax - extents.XMin) * (extents.YMax - extents.YMin)
    percent = searchArea.area / gridArea
    return (1 - percent) > timeRatio / ((nFixes - 1) * intervals)


def PrintCellSizeEvaluation(cellSize, varianceLocation, varianceMobility):
    sigmaLocation = math.sqrt(varianceLocation)
    sigmaMobility = math.sqrt(varianceMobility)
    utils.info("  Calculated cell size %.2f" % cellSize)
    upperMinimum = min(sigmaLocation * 2, sigmaMobility / 2)
    lowerMinimum = min(sigmaLocation / 2.5, sigmaMobility / 10)
    # print("  To capture detail of location variance, cell size should be between",
    # sigmaLocation/10, sigmaLocation)
    # print("  To capture detail of mobility variance, cell size should be between",
    # sigmaMobility/10, sigmaMobility)
    utils.info(
        "  Preferred cell size is between %.2f and %.2f" % (lowerMinimum, upperMinimum)
    )
    if cellSize > upperMinimum:
        n = int(cellSize / upperMinimum)
        n = n * n
        utils.info(
            "  Processing will take %d times as long if cell size is %.2f"
            % (n, upperMinimum)
        )


def PrintRunTime(
    extents,
    cellSize,
    searchArea,
    nFixes,
    intervals,
    secondsPerCheck,
    secondsPerCalculation,
):
    gridArea = (extents.XMax - extents.XMin) * (extents.YMax - extents.YMin)
    searchPercent = searchArea.area / gridArea
    msg = "  Buffer area %.0f, Grid Area %.0f, Percent to search %.2f" % (
        searchArea.area,
        gridArea,
        searchPercent * 100,
    )
    utils.info(msg)
    skipPercent = 1 - searchPercent

    rows = int(1 + (extents.YMax - extents.YMin) / cellSize)
    cols = int(1 + (extents.XMax - extents.XMin) / cellSize)
    segments = nFixes - 1
    n = cols * rows * segments * intervals

    msg = "  cols %d, rows %d, segments %d, intervals %d, total %d" % (
        cols,
        rows,
        segments,
        intervals,
        n,
    )
    utils.info(msg)
    seconds = (
        cols
        * rows
        * (
            secondsPerCheck
            + searchPercent * (segments * intervals) * secondsPerCalculation
        )
    )
    seconds = int(seconds)
    if seconds > 60:
        minutes = seconds // 60
        seconds = seconds - minutes * 60
        if minutes > 60:
            hours = minutes // 60
            minutes = minutes - hours * 60
            if hours > 24:
                days = hours // 24
                hours = hours - days * 24
                time = "%d days and %d hours" % (days, hours)
            else:
                time = "%d hours and %d minutes" % (hours, minutes)
        else:
            time = "%d minutes and %d seconds" % (minutes, seconds)
    else:
        time = "%d seconds" % (seconds)

    utils.info("  Estimated running time %s" % time)


def validate(
    fixes_name,
    raster_name,
    date_fieldname,
    grouping_fieldnames,
    cell_size_constant,
    integration_interval,
    location_variance_fieldname,
    location_variance_constant,
    mobility_variance_fieldname,
    mobility_variance_constant,
    spatial_reference,
):
    """
    Validate the input.

    Exit with a message if the input is not valid.  Otherwise return a tuple
    containing most of the validated format of the input parameters.
    """
    no_value = ["#", "", "None", "NONE", "none"]

    if not fixes_name:
        utils.die("No fixes layer was provided. Quitting.")

    if not arcpy.Exists(fixes_name):
        utils.die("Location layer cannot be found. Quitting.")

    date_field_names = [
        field.name for field in arcpy.ListFields(fixes_name, None, "Date")
    ]
    double_field_names = [
        field.name for field in arcpy.ListFields(fixes_name, None, "Double")
    ]
    all_field_names = [field.name for field in arcpy.ListFields(fixes_name)]
    bad_field_msg = "Field {0} does not exist in {1}. Quitting."

    if not raster_name:
        utils.die("No output requested. Quitting.")

    if date_fieldname in no_value:
        date_fieldname = None
    if date_fieldname and date_fieldname not in date_field_names:
        utils.die(bad_field_msg.format(date_fieldname, fixes_name))

    if grouping_fieldnames in no_value:
        grouping_fieldnames = None
    if grouping_fieldnames:
        grouping_fieldnames = (
            grouping_fieldnames.replace(",", " ").replace(";", " ").replace(":", " ")
        )
        grouping_fieldnames = grouping_fieldnames.split()
        for group_fieldname in grouping_fieldnames:
            if group_fieldname not in all_field_names:
                utils.die(bad_field_msg.format(group_fieldname, fixes_name))

    if cell_size_constant in no_value:
        cell_size_constant = None
    else:
        try:
            cell_size_constant = float(cell_size_constant)
        except ValueError:
            utils.die("Cell size is not a number. Quitting.")

    if integration_interval in no_value:
        integration_interval = None
    else:
        try:
            integration_interval = int(integration_interval)
        except ValueError:
            utils.die("Cell size is not a number. Quitting.")

    if location_variance_fieldname in no_value:
        locationVarianceFieldName = None
    if (
        location_variance_fieldname
        and location_variance_fieldname not in double_field_names
    ):
        utils.die(bad_field_msg.format(location_variance_fieldname, fixes_name))

    if location_variance_constant in no_value:
        location_variance_constant = None
    else:
        try:
            location_variance_constant = float(location_variance_constant)
        except ValueError:
            utils.die("Location Variance is not a number. Quitting.")

    if mobility_variance_fieldname in no_value:
        mobility_variance_fieldname = None
    if (
        mobility_variance_fieldname
        and mobility_variance_fieldname not in double_field_names
    ):
        utils.die(bad_field_msg.format(mobility_variance_fieldname, fixes_name))

    if mobility_variance_constant in no_value:
        mobility_variance_constant = None
    else:
        try:
            mobility_variance_constant = float(mobility_variance_constant)
        except ValueError:
            utils.die("Mobility Variance is not a number. Quitting.")

    if not spatial_reference or not spatial_reference.name:
        spatial_reference = arcpy.env.outputCoordinateSystem

    if not spatial_reference or not spatial_reference.name:
        spatial_reference = arcpy.Describe(fixes_name).spatialReference

    if not spatial_reference or not spatial_reference.name:
        utils.die(
            "The fixes layer does not have a coordinate system, "
            "and you have not provided one. Quitting."
        )

    if spatial_reference.type != "Projected":
        msg = (
            "The output projection is '{0}'. "
            "It must be a projected coordinate system. Quitting."
        )
        utils.die(msg.format(spatial_reference.type))

    return (
        date_fieldname,
        grouping_fieldnames,
        cell_size_constant,
        integration_interval,
        location_variance_fieldname,
        location_variance_constant,
        mobility_variance_fieldname,
        mobility_variance_constant,
        spatial_reference,
    )


def validate_create_raster(
    fixes_name,
    raster_name,
    date_fieldname,
    grouping_fieldnames,
    cell_size_constant,
    integration_interval,
    location_variance_fieldname,
    location_variance_constant,
    mobility_variance_fieldname,
    mobility_variance_constant,
    spatial_reference,
):
    """Validate input and create raster."""

    (
        date_fieldname,
        grouping_fieldnames,
        cell_size_constant,
        integration_interval,
        location_variance_fieldname,
        location_variance_constant,
        mobility_variance_fieldname,
        mobility_variance_constant,
        spatial_reference,
    ) = validate(
        fixes_name,
        raster_name,
        date_fieldname,
        grouping_fieldnames,
        cell_size_constant,
        integration_interval,
        location_variance_fieldname,
        location_variance_constant,
        mobility_variance_fieldname,
        mobility_variance_constant,
        spatial_reference,
    )
    create_raster(
        fixes_name,
        raster_name,
        date_fieldname,
        grouping_fieldnames,
        cell_size_constant,
        integration_interval,
        location_variance_fieldname,
        location_variance_constant,
        mobility_variance_fieldname,
        mobility_variance_constant,
        spatial_reference,
    )


def test():
    """Set up manual test parameters and process."""

    fixes_name = r"C:\tmp\test.gdb\fix_a_c96"
    raster_name = r"C:\tmp\test.gdb\bb7"
    date_fieldname = "FixDate"
    grouping_fieldnames = ""
    cell_size_constant = "#"
    integration_interval = "#"
    location_variance_fieldname = "#"
    location_variance_constant = None
    mobility_variance_fieldname = ""
    mobility_variance_constant = ""
    spatial_reference = arcpy.SpatialReference()
    spatial_reference.loadFromString(
        "PROJCS['NAD_1983_Alaska_Albers',GEOGCS['GCS_North_American_1983',"
        "DATUM['D_North_American_1983',SPHEROID['GRS_1980',6378137.0,298.257222101]],"
        "PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]],"
        "PROJECTION['Albers'],PARAMETER['False_Easting',0.0],PARAMETER['False_Northing',0.0],"
        "PARAMETER['Central_Meridian',-154.0],PARAMETER['Standard_Parallel_1',55.0],"
        "PARAMETER['Standard_Parallel_2',65.0],PARAMETER['Latitude_Of_Origin',50.0],"
        "UNIT['Meter',1.0]];-13752200 -8948200 10000;-100000 10000;-100000 10000;"
        "0.001;0.001;0.001;IsHighPrecision"
    )
    arcpy.Delete_management(raster_name)
    validate_create_raster(
        fixes_name,
        raster_name,
        date_fieldname,
        grouping_fieldnames,
        cell_size_constant,
        integration_interval,
        location_variance_fieldname,
        location_variance_constant,
        mobility_variance_fieldname,
        mobility_variance_constant,
        spatial_reference,
    )


def test_horne():
    """Set up manual test parameters and process."""

    rasterName = r"C:\tmp\kd_test\bb_h_smallv.tif"
    extents = arcpy.Extent(-90, -180, 370, 180)
    cellSize = 1.0
    intervals = 10
    searchArea = None
    fixes = [
        # time, x, y, locational_variance, mobility_variance
        [0.0, 0, 0, 8.32, 6.42],
        [20.0, 280, 0, 8.32, 6.42],
    ]
    print("Horne Test", extents, cellSize, searchArea, fixes, intervals)
    raster = CreateBBRaster(extents, cellSize, searchArea, fixes, intervals, None)
    arcpy.Delete_management(rasterName)
    raster.save(rasterName)


def main():
    """Get the input from arcpy (tbx or command line) and process."""

    fixes_name = arcpy.GetParameterAsText(0)
    raster_name = arcpy.GetParameterAsText(1)
    date_fieldname = arcpy.GetParameterAsText(2)
    grouping_fieldnames = arcpy.GetParameterAsText(3)
    cell_size_constant = arcpy.GetParameterAsText(4)
    integration_interval = arcpy.GetParameterAsText(5)
    location_variance_fieldname = arcpy.GetParameterAsText(7)
    location_variance_constant = arcpy.GetParameterAsText(6)
    mobility_variance_fieldname = arcpy.GetParameterAsText(9)
    mobility_variance_constant = arcpy.GetParameterAsText(8)
    spatial_reference = arcpy.GetParameterAsText(10)

    validate_create_raster(
        fixes_name,
        raster_name,
        date_fieldname,
        grouping_fieldnames,
        cell_size_constant,
        integration_interval,
        location_variance_fieldname,
        location_variance_constant,
        mobility_variance_fieldname,
        mobility_variance_constant,
        spatial_reference,
    )


if __name__ == "__main__":
    # test()
    # test_horne()
    main()
