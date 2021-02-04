# -*- coding: utf-8 -*-
"""
MovementVectors.py
Created on: Fri Sep 28 2013
Created by: Regan Sarwas, GIS Team, Alaska Region, National Park Service
 A re-write of 9.3 version of tool for 10.1sp1 or greater.
    requires arcgis 10.1 for new data access cursors.
    requires arcgis 10.1sp1 for shape@wkt.
 Vectors are drawn connecting points in order of the datetime attribute
   if two features have the same datetime, then the ordering of the
   features will retain the database ordering (usually this is by feature
   creation, but it may be random), and they will get a speed of -1.
   If the animal id is provided, then the points are first sorted by
   animal, and the points for different animals are not connected.
 Notes:
  1) A projected coordinate system for the output is required
       The first of the following will be used:
         1) the spatialReference parameter for this tool,
         2) the output spatial reference specified in the environment
         3) the spatial reference of the input feature class
  2) Duration will always be in hours
  3) Speed will always be in projected units per hour
  4) The input feature must have an ArcGIS date field.  The shapefile date
     field does not have a time component, so you are limited to one point
     per day, and duration/speed will always be on 24 hour intervals
  5) Works with Animal Movements database
  6) Honors the definition query and selection set in ArcMap
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import os
import math
from operator import itemgetter

import arcpy

import utils


def CreateSchema(outputFeature, animalField, startFieldName, endFieldName,
                 durationFieldName, velocityFieldName, directionFieldName, spatialReference):
    # Create Feature Class...
    workspace, feature = os.path.split(outputFeature)
    arcpy.CreateFeatureclass_management(workspace, feature, "POLYLINE", "",
                                        "DISABLED", "DISABLED",
                                        spatialReference, "", "0", "0", "0")
    # Add Fields...
    if animalField:
        arcpy.AddField_management(outputFeature, animalField.name,
                                  animalField.type, "", "", "", "",
                                  "NON_NULLABLE", "REQUIRED", "")
    arcpy.AddField_management(outputFeature, startFieldName, "DATE", "", "",
                              "", "", "NON_NULLABLE", "REQUIRED", "")
    arcpy.AddField_management(outputFeature, endFieldName, "DATE", "", "",
                              "", "", "NON_NULLABLE", "REQUIRED", "")
    arcpy.AddField_management(outputFeature, durationFieldName, "DOUBLE", "",
                              "", "", "", "NULLABLE", "NON_REQUIRED", "")
    arcpy.AddField_management(outputFeature, velocityFieldName, "DOUBLE", "",
                              "", "", "", "NULLABLE", "NON_REQUIRED", "")
    arcpy.AddField_management(outputFeature, directionFieldName, "DOUBLE", "",
                              "", "", "", "NULLABLE", "NON_REQUIRED", "")


def CreateMovementVectors(telemetryLayer, outputFeature, animalFieldName,
                          dateFieldName, startFieldName, endFieldName,
                          durationFieldName, velocityFieldName, directionFieldName,
                          spatialReference):

    searchFields = [dateFieldName, 'SHAPE@XY']
    insertFields = [startFieldName, endFieldName, durationFieldName,
                    velocityFieldName, directionFieldName, 'SHAPE@WKT']
    if animalFieldName:
        hasAnimal = True
        animalField = arcpy.ListFields(telemetryLayer, animalFieldName)[0]
        searchFields = [animalFieldName] + searchFields
        insertFields = [animalFieldName] + insertFields
    else:
        hasAnimal = False
        animalField = None
    CreateSchema(outputFeature, animalField, startFieldName, endFieldName,
                 durationFieldName, velocityFieldName, directionFieldName, spatialReference)

    with arcpy.da.InsertCursor(outputFeature, insertFields) as updateCursor,\
            arcpy.da.SearchCursor(telemetryLayer, searchFields, spatial_reference=spatialReference) as searchCursor:
        # searchCursor does not support orderby for all datasources
        # particularly shapefiles, so I will do the sorting in python
        # the cursor returns each row as a comparable tuple
        rows = [row for row in searchCursor]
        #sort() is stable, so if a multiple rows have the same animal and
        # date, the points will be connected in data source order
        if hasAnimal:
            rows.sort(key=itemgetter(0, 1))
        else:
            rows.sort(key=itemgetter(0))
        previousRow = None
        for row in rows:
            if previousRow:
                line = BuildVelocityVector(previousRow, row, hasAnimal)
                if line:
                    updateCursor.insertRow(line)
            previousRow = row


def BuildVelocityVector(fix1, fix2, hasAnimal):
    animal = None
    if hasAnimal:
        if fix1[0] != fix2[0]:
            return None
        else:
            animal = fix1[0]
            offset = 1
    else:
        offset = 0

    date1 = fix1[offset+0]
    date2 = fix2[offset+0]
    days = (date2 - date1).days
    seconds = (date2 - date1).seconds
    hours = days * 24 + seconds / 3600.0
    x1, y1 = fix1[offset+1]
    x2, y2 = fix2[offset+1]
    length = math.sqrt((x2-x1)**2 + (y2-y1)**2)
    direction = 90 - math.atan2(y2-y1, x2-x1) * 180 / math.pi
    if direction < 0:
        direction += 360
    if days == 0 and seconds == 0:
        speed = -1
    else:
        speed = length/hours
    shape = "LINESTRING ({x1} {y1}, {x2} {y2})".format(x1=x1, y1=y1, x2=x2, y2=y2)
    if hasAnimal:
        return animal, date1, date2, hours, speed, direction, shape
    else:
        return date1, date2, hours, speed, direction, shape


if __name__ == "__main__":

    telemetryLayer = arcpy.GetParameterAsText(0)
    outputFeature = arcpy.GetParameterAsText(1)
    animalFieldName = arcpy.GetParameterAsText(2)
    dateFieldName = arcpy.GetParameterAsText(3)
    startFieldName = arcpy.GetParameterAsText(4)
    endFieldName = arcpy.GetParameterAsText(5)
    durationFieldName = arcpy.GetParameterAsText(6)
    velocityFieldName = arcpy.GetParameterAsText(7)
    directionFieldName = arcpy.GetParameterAsText(8)
    spatialReference = arcpy.GetParameter(9)

    test = False
    if test:
        telemetryLayer = r"C:\tmp\test.gdb\fix_ll"
        #telemetryLayer = r"C:\tmp\test.gdb\fix_a_c96"
        telemetryLayer = r"C:\tmp\vvtest.shp"
        outputFeature = r"C:\tmp\vv08.test.shp"
        #outputFeature = r"C:\tmp\test.gdb\vv09"
        animalFieldName = ""  # "AnimalId"
        dateFieldName = "FixDate"
        startFieldName = "StartTime"
        endFieldName = "EndTime"
        durationFieldName = "Duration_Hour"
        velocityFieldName = "Speed_PerHour"
        directionFieldName = "Direction_Degrees"
        #spatialReference = arcpy.SpatialReference()
        #spatialReference.loadFromString("Projected Coordinate Systems/State Systems/NAD 1927 Alaska Albers (US Feet)")
        spatialReference = None

    #
    # Default Values
    #
    if not startFieldName or startFieldName == "#":
        startFieldName = "Start_Time"
    if not endFieldName or endFieldName == "#":
        endFieldName = "End_Time"
    if not durationFieldName or durationFieldName == "#":
        durationFieldName = "Duration_Hour"
    if not velocityFieldName or velocityFieldName == "#":
        velocityFieldName = "Speed_PerHour"
    if not directionFieldName or directionFieldName == "#":
        directionFieldName = "Direction_Degrees"

    #
    # Input validation
    #
    if not arcpy.Exists(telemetryLayer):
        utils.die("Telemetry layer cannot be found. Quitting.")

    try:
        isPointFeature = arcpy.Describe(telemetryLayer).shapeType == "Point"
    except AttributeError:
        isPointFeature = False
    if not isPointFeature:
        utils.die("Telemetry layer is not a point feature. Quitting.")

    if not outputFeature:
        utils.die("No output requested. Quitting.")

    if arcpy.Exists(outputFeature) and not arcpy.env.overwriteOutput:
        utils.die("Output exists, and overwritting is denied. Quitting.")

    if animalFieldName:
        allFieldNames = [f.name for f in arcpy.ListFields(telemetryLayer)]
        if animalFieldName not in allFieldNames:
            utils.info("Telemetry layer does not have a field named '" +
                       animalFieldName + "'. Using nothing.")
            animalFieldName = None

    dateNames = [f.name for f in arcpy.ListFields(telemetryLayer, "", "Date")]
    if dateFieldName not in dateNames:
        utils.die("Telemetry layer does not have a date field named '" +
                  dateFieldName + "'. Quitting.")

    workspace, table = os.path.split(outputFeature)
    if not arcpy.Exists(workspace):
        utils.die("Output workspace does not exists. Quitting.")

    # file.shp will be converted to file_shp, so treat it special
    # FIXME will still fail on c:\tmp\fgdb.gdb\file.shp
    filename, ext = os.path.splitext(table)
    if ext.lower() == ".shp":
        table = filename
    newTableName = arcpy.ValidateTableName(table, workspace)
    if newTableName != table:
        if ext.lower() == ".shp":
            newTableName = newTableName + ext
        outputFeature = os.path.join(workspace, newTableName)
        utils.info("Feature class was renamed to " + newTableName +
                   " to meet workspace requirements.")

    newFieldName = arcpy.ValidateFieldName(startFieldName, workspace)
    if newFieldName != startFieldName:
        utils.info("Renamed field from  `" + startFieldName + "` to `" +
                   newFieldName + "` to meet workspace requirements.")
        startFieldName = newFieldName

    newFieldName = arcpy.ValidateFieldName(endFieldName, workspace)
    if newFieldName != endFieldName:
        utils.info("Renamed field from  `" + endFieldName + "` to `" +
                   newFieldName + "` to meet workspace requirements.")
        endFieldName = newFieldName

    newFieldName = arcpy.ValidateFieldName(durationFieldName, workspace)
    if newFieldName != durationFieldName:
        utils.info("Renamed field from  `" + durationFieldName + "` to `" +
                   newFieldName + "` to meet workspace requirements.")
        durationFieldName = newFieldName

    newFieldName = arcpy.ValidateFieldName(velocityFieldName, workspace)
    if newFieldName != velocityFieldName:
        utils.info("Renamed field from  `" + velocityFieldName + "` to `" +
                   newFieldName + "` to meet workspace requirements.")
        velocityFieldName = newFieldName

    newFieldName = arcpy.ValidateFieldName(directionFieldName, workspace)
    if newFieldName != directionFieldName:
        utils.info("Renamed field from  `" + directionFieldName + "` to `" +
                   newFieldName + "` to meet workspace requirements.")
        directionFieldName = newFieldName

    #
    # Set output coordinate system
    #
    if not spatialReference or not spatialReference.name:
        spatialReference = arcpy.env.outputCoordinateSystem

    if not spatialReference or not spatialReference.name:
        spatialReference = arcpy.Describe(telemetryLayer).spatialReference

    if not spatialReference or not spatialReference.name:
        utils.die("The telemetry layer does not have a coordinate system," +
                  " and you have not provided one. Quitting.")

    if spatialReference.type != 'Projected':
        utils.die("The output projection is '" + spatialReference.type +
                  "'.  It must be a projected coordinate system. Quitting.")

    #
    # Create Movement Vectors
    #
    CreateMovementVectors(telemetryLayer, outputFeature, animalFieldName,
                          dateFieldName, startFieldName, endFieldName,
                          durationFieldName, velocityFieldName, directionFieldName,
                          spatialReference)
