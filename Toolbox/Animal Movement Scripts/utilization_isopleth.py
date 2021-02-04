# -*- coding: utf-8 -*-
"""
UD_Isopleths.py
Created: 2011-10-06

Title:
Utilization Distribution Isopleths

Tags:
contour, home, range, animal, tracking, telemetry, ecology, kernel, density

Summary:
Creates polylines and/or polygons based on the requested list of isopleths. Input is a UD raster (typically created from the UD_Raster tool)

Usage:
Either a floating point or integer raster is acceptable as input.  There is no limits on the range of the cell values.
The isopleth values provided should be appropriate for the input raster. (Integers between 1 and 99 for rasters created with the UD Raster tool).
At least one of the output feature classes (lines, polygons, or donuts) must be requested

Parameter 1:
Isopleths
This is a list of isopleth values separated by commas, semicolons, or whitespace. The values provided should be appropriate for the input raster (integers between 1 and 99 for rasters created with the UD Raster tool).

Parameter 2:
Raster_Layer
The input raster. Any raster is acceptable, however this tool is typically used with a utilization distribution probability raster (created by the UD Raster tool).  UD Rasters have integer values in the range from 1 to 100 with 100 at the edges of the universe, and 1 at the center(s) of the concentration.  This is important when when selecting the Isopleths, and when stacking isopleth polygons.  See the discussion for the Polygon parameter.


Parameter 3:
Lines (optional)
The name of a new output polyline feature class. One of Lines, Polygons, or Donut_Polygons must be provided.  If this parameter is left blank, no lines will be created. The output feature class will have a field named 'contour' with the value of the isopleth, and one or more features for each isopleth requested that exists in the input raster.  There may be multiple polylines for each isopleth.  Polylines may not close, but they should if the input is a UD Raster from the UD Raster tool.
No smoothing is done, and depending on the cell size the output can be very dense (small cell size), or very blocky (large cell size)

Parameter 4:
Polygons (optional)
Name of the new output polygon feature class. One of Lines, Polygons, or Donut_Polygons must be provided
Contains a polygon for each isopleth.  Each polygon contains the entire are covered by the isopleth. These polygons are overlapping.  The polygons are written to the featureclass with the largest isopleth values first. (for UD analysis, this provides a correctly stacked results set). These polygons are created from the isopleth lines

Parameter 5:
Donut_Polygons (optional)
Name of the new output polygon feature class. One of Lines, Polygons, or Donut_Polygons must be provided
Contains a polygon for each isopleth range.  Assumes the isopleths are ordered with the largest values containing the most area (so the last range is a donut without a hole). There is no donut for the first range range (i.e. from the universe to the first isopleth). These polygons are created from the lines

Scripting Syntax:
UD_Isopleths_AnimalMovement (Isopleths, Raster_Layer, Lines, Polygons, Donut_Polygons)

Example1:
Scripting Example
The following example shows how this script can be used in the ArcGIS Python Window. It assumes that the script has been loaded into a toolbox, and the toolbox has been loaded into the active session of ArcGIS.
It creates the 65%, 90% UD polygons (with holes) in a file geodatabase
 raster = "C:/tmp/kde.tif"
 donuts = "C:/tmp/test.gdb/ud_donuts"
 UD_Isopleths("65;90", raster, "", "", donuts)

Example2:
Command Line Example
The following example shows how the script can be used from the operating system command line. It assumes that the script and the data sources are in the current directory and that the python interpeter is the path.
It creates the 50%, 90% and 95% UD polygons in a file geodatabase
 C:/folder> python UD_Isopleths.py "50,90,95" kde.tif # test.gdb/ud_poly #

Credits:
Regan Sarwas, Alaska Region GIS Team, National Park Service

Limitations:
Public Domain

Requirements
arcpy module - requires ArcGIS v10.1+ and a valid license
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

import utils


def CreateIsopleths(isopleths, raster1, lineFc, polyFc, donutFc):
    """Creates a set of Isopleths as polylines, polygons (entire area of
    each isopleth) or donuts (area between isopleths).  If only one
    isopleth is requested, then the donutFc is the same as the polyFc.

    isopleths is a list of floating point values expected in the raster.
    lineFc, polyFc, donutFc are the names of the feature classes to create.

    Assumes arcpy is declared globally and Spatial Analyst Extension
    (arcpy.sa) is available and that the input parameters has been validated
    will probably throw a (cryptic?) exception if these assumptions are not met.
    Assumes one of the last three parameters is non null,
    otherwise cycles may be wasted generating unused intermediate results."""

    # raster = 101 - arcpy.sa.Slice(raster1,100,"EQUAL_INTERVAL")
    if lineFc:
        arcpy.sa.ContourList(raster1, lineFc, isopleths)
        if polyFc:
            IsoplethLinesToPolygons(lineFc, polyFc)
        if donutFc:
            IsoplethLinesToDonuts(lineFc, donutFc)
    else:
        tmpLayer = r"in_memory\IsoplethPolylines"
        if arcpy.Exists(tmpLayer):
            arcpy.Delete_management(tmpLayer)
        arcpy.sa.ContourList(raster1, tmpLayer, isopleths)
        if polyFc:
            IsoplethLinesToPolygons(tmpLayer, polyFc)
        if donutFc:
            IsoplethLinesToDonuts(tmpLayer, donutFc)
        arcpy.Delete_management(tmpLayer)


def GetIsoplethList(isoplethInput):
    """isoplethInput is a string of floating point numbers.
    Returns a sorted python list of unique floats between 0 and 100.
    valid separators in the input string are comma(,) semicolon(;) colon(:)
    and whitespace in any combination.
    Any other non-numeric character(0123456789-+.eE) will throw a ValueError
    Numbers 0 or less and 100 or more are silently ignored.
    International numbers (comma as decimal separator) are not supported.
    test input: '100,0,   4.65e1, 45,   +5:10.1, 65,90,5 10;-4;;'
    test output: [5.0, 10.0, 10.1, 45.0, 46.5, 65.0, 90.0]"""

    isoplethInput = isoplethInput.replace(",", " ").replace(";", " ").replace(":", " ")
    try:
        isoplethList = [float(c) for c in isoplethInput.split()]
    except ValueError:
        utils.die("Un-recognized character in the isopleth list")

    isoplethList = [f for f in isoplethList if f > 0 and f < 100]
    isoplethList = list(set(isoplethList))  # remove duplicates
    isoplethList.sort()
    return isoplethList


def TestGetIsopleth():
    a = "100,0,   4.65e1, 45,   +5:10.1, 65,90,5 10;-4;;"
    print(a)
    print(GetIsoplethList(a))


def IsoplethLinesToPolygons(lineFC, polyFC, fieldname="contour"):
    """Builds a featureclass named polyFC, by creating a polygon
    from all the lines in lineFC that have the same value in the
    field named fieldname.  The largest values in fieldname are
    written first, this will provide the expected polygon stacking.

    arcpy.FeatureToPolygon_management creates polygons from lines,
    but it does not copy attributes, doesn't guarantee order, and
    doesn't merge polygons with the same value."""

    uniqueValues = GetUniqueValues(lineFC, fieldname)

    if not uniqueValues:
        utils.warn("No field named '" + fieldname + "' in " + lineFC)
        return

    workspace, featureClass = os.path.split(polyFC)
    arcpy.CreateFeatureclass_management(
        workspace,
        featureClass,
        "Polygon",
        lineFC,
        "SAME_AS_TEMPLATE",
        "SAME_AS_TEMPLATE",
        lineFC,
    )
    # I use lineFC as a template for polyFC, even though I don't need all fields
    # (typically there is only the 'contour' field, besides the standard fields)
    # I do this to avoid adding a field (schema lock bug with PGDB)
    # and I don't want to worry about the data type (float or int) of 'contour'

    lineDescription = arcpy.Describe(lineFC)
    polyDescription = arcpy.Describe(polyFC)

    uniqueValues = list(uniqueValues)
    uniqueValues.sort()
    uniqueValues.reverse()  # builds biggest to smallest
    fields = ["SHAPE@", fieldname]
    cursor = arcpy.da.InsertCursor(polyFC, fields)
    for value in uniqueValues:
        query = BuildQuery(lineFC, fieldname, value)
        with arcpy.da.SearchCursor(lineFC, ["SHAPE@"], where_clause=query) as lines:
            array = arcpy.Array()
            for line in lines:
                shape = line[0]
                for i in range(shape.partCount):
                    array.add(shape.getPart(i))
            newshape = arcpy.Polygon(array)
            cursor.insertRow([newshape, value])
    del cursor


def IsoplethLinesToDonuts(lineFC, polyFC, fieldname="contour"):
    """Builds a featureclass named polyFC, by creating a polygon
    from all the lines in lineFC that have the same value in the
    field named fieldname.  Polygons do not include the area of
    the isopleths contained within them (that is all polygons
    except the innermost are donuts).  In this case the polygons
    do not overlap, and there is no concern about order or stacking.
    If there is only one isopleth in lines, then this result is the
    same as IsoplethLinesToPolygons().

    arcpy.FeatureToPolygon_management creates polygons from lines,
    but it does not copy attributes, and doesn't merge polygons with
    the same value."""

    uniqueValues = GetUniqueValues(lineFC, fieldname)

    if not uniqueValues:
        utils.warn("No field named '" + fieldname + "' in " + lineFC)
        return

    workspace, featureClass = os.path.split(polyFC)
    arcpy.CreateFeatureclass_management(
        workspace,
        featureClass,
        "Polygon",
        lineFC,
        "SAME_AS_TEMPLATE",
        "SAME_AS_TEMPLATE",
        lineFC,
    )
    # I use lineFC as a template for polyFC, even though I don't need all fields
    # (typically there is only the 'contour' field, besides the standard fields)
    # I do this to avoid adding a field (schema lock bug with PGDB)
    # and I don't want to worry about the data type (float or int) of 'contour'

    lineDescription = arcpy.Describe(lineFC)
    polyDescription = arcpy.Describe(polyFC)

    # we will build polygon n and then subtract polygon n+1
    # to do this we need to sort biggest to smallest
    # the last polygon has nothing subtracted from it.
    # polygons with holes are created as an array of polygons
    uniqueValues = list(uniqueValues)
    uniqueValues.sort()
    uniqueValues.reverse()  # sort biggest to smallest
    fields = ["SHAPE@", fieldname]
    # InsertCursors do not support the with statement.
    cursor = arcpy.da.InsertCursor(polyFC, fields)
    lines2 = []
    for i in range(len(uniqueValues)):
        # We could use the inner polygon from the last loop
        # but ESRI's cursors are funny things, so we are doing
        # this the brute force way.  It's a little slower, but
        # not noticably
        value1 = uniqueValues[i]
        query1 = BuildQuery(lineFC, fieldname, value1)
        # TODO: Restructure to use with statement on two search cursors.
        lines1 = arcpy.da.SearchCursor(lineFC, ["SHAPE@"], where_clause=query1)
        if i == len(uniqueValues) - 1:  # last one
            lines2 = []
        else:
            value2 = uniqueValues[i + 1]
            query2 = BuildQuery(lineFC, fieldname, value2)
            lines2 = arcpy.da.SearchCursor(lineFC, ["SHAPE@"], where_clause=query2)
        array = arcpy.Array()
        for line in lines1:
            shape = line[0]
            for j in range(shape.partCount):
                array.add(shape.getPart(j))
        # lines1/2 are arcpy.Cursors, and cannot be added, so we iterate each separately
        for line in lines2:
            shape = line[0]
            for j in range(shape.partCount):
                array.add(shape.getPart(j))
        newshape = arcpy.Polygon(array)
        cursor.insertRow([newshape, value1])
        del lines1
        del lines2
    del cursor


def GetUniqueValues(featureClass, whereField):
    """Make a list of all values in the whereField of
    featureClass.  Each value in the returned list is unique.
    That is multiple identical values are only reported once."""

    # members in a set are guaranteed to be unique
    values = set()
    with arcpy.da.SearchCursor(featureClass, [whereField]) as cursor:
        for row in cursor:
            if row[0]:
                values.add(row[0])
    return values


def BuildQuery(featureClass, whereField, value):
    """Builds a valid query string for finding all records in
    featureclass that have value in whereField.
    Building a valid query is a pain. We need to correctly
    delimit the field name (with "" or []) depending on the database
    and correctly delimit the value based on the field type
    Does not correctly handle delimiting datetime data types.
    This will throw an exception if whereField is not in featureClass."""

    field = arcpy.AddFieldDelimiters(featureClass, whereField)
    fields = arcpy.ListFields(featureClass)
    type = [f.type.lower() for f in fields if f.name == whereField]
    if type and type[0] == "string":
        quote = "'"
    else:
        quote = ""
    return "{0} = {1}{2}{1}".format(field, quote, value)


if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        utils.die("Unable to checkout the Spatial Analyst Extension.  Quitting.")

    isoplethInput = arcpy.GetParameterAsText(0)
    rasterLayer = arcpy.GetParameterAsText(1)
    isoplethLines = arcpy.GetParameterAsText(2)
    isoplethPolys = arcpy.GetParameterAsText(3)
    isoplethDonuts = arcpy.GetParameterAsText(4)

    test = False
    if test:
        isoplethInput = "50,65,90,95"
        rasterLayer = r"C:\tmp\kd_test\prob_all_sr40k.tif"
        isoplethLines = r"C:\tmp\test2.gdb\clines4a"
        isoplethPolys = r"C:\tmp\test2.gdb\cpolys4a"
        isoplethDonuts = r"C:\tmp\test2.gdb\cdonut4a"

    #
    # Input validation
    #
    if not (isoplethLines or isoplethPolys or isoplethDonuts):
        utils.die("No output requested. Quitting.")

    isoplethList = GetIsoplethList(isoplethInput)
    if not isoplethList:
        utils.die("List of valid isopleths is empty. Quitting.")

    if not rasterLayer:
        utils.die("No raster layer was provided. Quitting.")

    if not arcpy.Exists(rasterLayer):
        utils.die("Raster layer cannot be found. Quitting.")

    #
    # Create isopleths
    #
    raster = arcpy.sa.Raster(
        rasterLayer
    )  # create a raster object from the text in rasterLayer
    CreateIsopleths(isoplethList, raster, isoplethLines, isoplethPolys, isoplethDonuts)
