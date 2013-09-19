# ---------------------------------------------------------------------------
# MovementVectors.py
# Created on: Fri Sep 28 2013
# Created by: Regan Sarwas, GIS Team, Alaska Region, National Park Service
#  re-write of 9.3 version of tool for 10.1+
# ---------------------------------------------------------------------------

#FIXME length units
#test projection


# Import system modules
import os, math
#import sys, string, os, datetime
import arcpy #, arcpy.da
import utils

def CreateSchema(outputFeature, animalField, startFieldName, endFieldName,
                 durationFieldName, velocityFieldName, spatialReference):
    # Create Feature Class...
    workspace,feature = os.path.split(outputFeature)
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


def CreateMovementVectors(telemetryLayer, outputFeature, animalFieldName,
                          dateFieldName, startFieldName, endFieldName,
                          durationFieldName, velocityFieldName,
                          spatialReference):

    searchFields = [dateFieldName, 'SHAPE@XY']
    insertFields = [startFieldName, endFieldName, durationFieldName,
                    velocityFieldName, 'SHAPE@WKT']
    if animalFieldName:
        hasAnimal = True
        animalField = arcpy.ListFields(telemetryLayer,animalFieldName)[0]
        searchFields = [animalFieldName] + searchFields
        insertFields = [animalFieldName] + insertFields
    else:
        hasAnimal = False
        animalField = None
    CreateSchema(outputFeature, animalField, startFieldName, endFieldName,
                 durationFieldName, velocityFieldName, spatialReference)

    with arcpy.da.InsertCursor(
             outputFeature,insertFields
        ) as updateCursor, arcpy.da.SearchCursor(
             telemetryLayer, searchFields, spatial_reference=spatialReference
        ) as searchCursor:
        # searchCursor does not support orderby for all datasourses
        # particularly shapefiles, so I will do the sorting in python
        # the cursor returns each row as a comparable tuple 
        rows = [row for row in searchCursor]
        rows.sort()
        previousRow = None
        for row in rows:
            if previousRow:
                line = BuildVelocityVector(previousRow, row, hasAnimal)
                if line:
                    updateCursor.insertRow(line)
            previousRow = row


def BuildVelocityVector(fix1, fix2, hasAnimal):
    # if hasAnimal and animal1 != animal2 return None
    # if hasAnimal then fix = (animal, date, (x,y)) else (date, (x,y))
    # returns ({animal}, date1, date2, duration, velocity, shape@WKT)
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
    x1,y1 = fix1[offset+1]
    x2,y2 = fix2[offset+1]
    length = math.sqrt((x2-x1)**2 + (y2-y1)**2)
    #FIXME check units of spatial reference, and convert to meters
    if days == 0 and seconds == 0:
        speed = -1
    else:
        speed = length/hours
    shape = "LINESTRING ({x1} {y1}, {x2} {y2})".format(
                          x1=x1, y1=y1, x2=x2, y2=y2)
    if hasAnimal:
        return (animal, date1, date2, hours, speed, shape)
    else:
        return (date1, date2, hours, speed, shape)


if __name__ == "__main__":

    telemetryLayer = arcpy.GetParameterAsText(0)
    outputFeature = arcpy.GetParameterAsText(1)
    animalFieldName = arcpy.GetParameterAsText(2)
    dateFieldName = arcpy.GetParameterAsText(3)
    startFieldName = arcpy.GetParameterAsText(4)
    endFieldName = arcpy.GetParameterAsText(5)
    durationFieldName = arcpy.GetParameterAsText(6)
    velocityFieldName = arcpy.GetParameterAsText(7)
    spatialReference = arcpy.GetParameter(8)


    test = True
    if test:
        #telemetryLayer = r"C:\tmp\test.gdb\fix_ll"
        telemetryLayer = r"C:\tmp\test.gdb\fix_a_c96"
        outputFeature = r"C:\tmp\test.gdb\vv05"
        animalFieldName = "" #"AnimalId"
        dateFieldName = "FixDate"
        startFieldName = "StartTime"
        endFieldName = "EndTime"
        durationFieldName = "Timespan_H"
        velocityFieldName = "Speed_MtPH"
        #spatialReference = arcpy.SpatialReference()
        #spatialReference.loadFromString("PROJCS['NAD_1983_Alaska_Albers',GEOGCS['GCS_North_American_1983',DATUM['D_North_American_1983',SPHEROID['GRS_1980',6378137.0,298.257222101]],PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]],PROJECTION['Albers'],PARAMETER['False_Easting',0.0],PARAMETER['False_Northing',0.0],PARAMETER['Central_Meridian',-154.0],PARAMETER['Standard_Parallel_1',55.0],PARAMETER['Standard_Parallel_2',65.0],PARAMETER['Latitude_Of_Origin',50.0],UNIT['Meter',1.0]];-13752200 -8948200 10000;-100000 10000;-100000 10000;0.001;0.001;0.001;IsHighPrecision")
        spatialReference = None
        
    #
    # Default Values
    #
    if not startFieldName or startFieldName == "#":
        startFieldName = "StartTime"
    if not endFieldName or endFieldName == "#":
        endFieldName = "EndTime"
    if not durationFieldName or durationFieldName == "#":
        durationFieldName = "Timespan_H"
    if not velocityFieldName or velocityFieldName == "#":
        velocityFieldName = "Speed_MtPH"

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
    
    if not (outputFeature):
        utils.die("No output requested. Quitting.")

    if arcpy.Exists(outputFeature) and not arcpy.env.overwriteOutput:
        utils.die("Output exists, and overwritting is denied. Quitting.")

    if animalFieldName:
        allFieldNames = [f.name for f in arcpy.ListFields(telemetryLayer)]
        if animalFieldName not in allFieldNames:
            utils.info("Telemetry layer does not have a field named '"+
                  animalFieldName+"'. Using nothing.")
            animalFieldName = None
        
    dateNames = [f.name for f in arcpy.ListFields(telemetryLayer,"","Date")]
    if dateFieldName not in dateNames:
        utils.die("Telemetry layer does not have a date field named '"+
                  dateFieldName+"'. Quitting.")

    workspace,table = os.path.split(outputFeature)
    if not arcpy.Exists(workspace):
        utils.die("Output workspace does not exists. Quitting.")
        
    newTableName = arcpy.ValidateTableName(table,workspace)
    if newTableName != table:
        outputFeature = os.path.join(workspace,newTableName)
        utils.info("Feature class was renamed to " + newTableName +
                   " to meet workspace requirements.")

    newFieldName = arcpy.ValidateFieldName(startFieldName,workspace)
    if newFieldName != startFieldName:
        utils.info("Renamed field from  `" + startFieldName + "` to `" +
                   newFieldName + "` to meet workspace requirements.")
        startFieldName = newFieldName
    
    newFieldName = arcpy.ValidateFieldName(endFieldName,workspace)
    if newFieldName != endFieldName:
        utils.info("Renamed field from  `" + endFieldName + "` to `" +
                   newFieldName + "` to meet workspace requirements.")
        endFieldName = newFieldName
    
    newFieldName = arcpy.ValidateFieldName(durationFieldName,workspace)
    if newFieldName != durationFieldName:
        utils.info("Renamed field from  `" + durationFieldName + "` to `" +
                   newFieldName + "` to meet workspace requirements.")
        durationFieldName = newFieldName
    
    newFieldName = arcpy.ValidateFieldName(velocityFieldName,workspace)
    if newFieldName != velocityFieldName: 
        utils.info("Renamed field from  `" + velocityFieldName + "` to `" +
                   newFieldName + "` to meet workspace requirements.")
        velocityFieldName = newFieldName
    
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
                          durationFieldName, velocityFieldName,
                          spatialReference)
