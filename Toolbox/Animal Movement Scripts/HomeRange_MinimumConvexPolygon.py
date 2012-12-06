# ------------------------------------------------------------------------------
# HomeRange_MinimumConvexPolygon.py
# Created: 2011-11-08
#
# Title:
# Minimum Convex Polygon
#
# Tags:
# home, range, animal, movement, area, ecology, location, telemetry, mcp
#
# Summary:
# This tool calculates the minimum convex polygon (MCP) for a data set or a percentage of a data set.  The MCP is the boundary of all the points in the data set such that the boundary is convex, not concave (i.e. every internal angle is less than or equal to 180 degrees).
#
# Usage:
# The input locations must be points
# The input locations must be in a projected coordinate system
# The MCP will be in the input coordinate system
#
# Parameter 1:
# Location_Points
# The set of points to use to develop the MCP.
# This can be a feature class, or a layer in a map document.  If there is an active selection set on a map layer, then only the selected features will be used.  You can drag and drop from the maps table of contents, or from the file explorer.
#
# Parameter 2:
# Output_Features
# The new feature class for the calculated minimum convex polygon.
#
# Parameter 3:
# Percentage_of_Points
# This is the percentage of the points to use when calculating the MCP. Specify 100 percent if you want to use all the location points.  If this is less than 100 percent, then points will be removed based on the removal method until the specified percentage is reached.
# The input data set is not altered.  Points are not actually removed from Location Points, they are simply excluded from consideration when building the MCP.
# Default is 100%
#
# Parameter 4:
# Removal_Method
# The removal method only applies if the percentage of points is less than 100.
# Fixed Mean: Removes points furthest from the mean of all location points.
# Fixed Median: Removes points furthest from the median of all location points.
# Floating Mean: Removes points furthest from the mean of the remaining location points. The mean is recalculated after each point is removed.
# Floating Median: Removes points furthest from the median of the remaining location points. The median is recalculated after each point is removed.
# User Point: Removes points furthest from the point provided.
# Area Added: The area of the MCP of all remaining points is calculated, then each point on the MCP boundary is considered in turn by calculating the area of the MCP without that point.  The boundary point that contributes the largest increase in area is removed.  This process continues until a sufficient number of points are removed.  This method can be slow if a large number of points are going to be removed.
#
# Parameter 5:
# User_Point
# The user point is required and used only if the removal method is 'User Point'.
# Locations closest to this point are used to calculate the MCP.
#
# Parameter 6:
# Output_Projection
# Calculations and output must be done in a projected coordinate system (i..e not geographic - lat/long).  The projected coordinate system to use can be specified in three ways, 1) with this parameter, 2) with the output coordinate system in the environment, or 3) with the coordinate system of the input.  These options are listed in priority order, that is this paraeter will trump the environment, and the environment will trump the input data. if a projected coordinate system is not found then the program will abort.
#
# Scripting Syntax:
# MinimumConvexPolygon (Location_Points, Output_Features, Percentage_of_Points, Removal_Method, User_Point, Output_Projection) 
#
# Example1:
# Scripting Example
# The following example shows how this script can be used in the ArcGIS Python Window. It assumes that the script has been loaded into a toolbox, and the toolbox has been loaded into the active session of ArcGIS.
# It creates the MCP without the 3% of the points furthest from a floating median.
#  fixes = r"C:\tmp\test.gdb\fixes"
#  mcp = r"C:\tmp\test.gdb\mcp97"
#  MinimumConvexPolygon (fixes, mcp, "97" "Floating Median", "", "")
#
# Example2:
# Command Line Example
# The following example shows how the script can be used from the operating system command line. It assumes that the script and the data sources are in the current directory and that the python interpeter is the path.
# It creates the MCP without the 5% of the points furthest from a fixed mean.
#  C:\folder> MinimumConvexPolygon.py test.gdb\fixes test.gdb\mcp95 95
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

import arcpy
import numpy
import math
import utils

# We support lists of arcpy.Points(), and lists of (x,y) tuples
# The arcpy.Points are ~20% faster to load, and create MCPs,
# but the lists of (x,y) are ~500% faster at the remaining calculations
# with the fixed center methods, the bulk of the time is in loading
# points and creating MCPs, so arcpy.Points() are much faster,
# with the floating center methods, the opposite is true.
# For the Add_Area method, arcpy.Points is faster.

def GetArcpyPoints(pointsFeature, shapeName = None):
    points = []
    if not shapeName:
        shapeName = arcpy.Describe(pointsFeature).shapeFieldName
    for row in arcpy.SearchCursor(pointsFeature):
        shape = row.getValue(shapeName)
        points.append(shape.getPart())
    return points

def GetPoints(pointsFeature, shapeName = None):
    points = []
    if not shapeName:
        shapeName = arcpy.Describe(pointsFeature).shapeFieldName
    for row in arcpy.SearchCursor(pointsFeature):
        shape = row.getValue(shapeName)
        points.append( (shape.getPart().X, shape.getPart().Y) )
    return points

arcpyPointType = type(arcpy.Point())


def Distance(pt1, pt2):
    return math.sqrt(Distance2(pt1,pt2))


def Distance2(pt1, pt2):
    if type(pt1) == arcpyPointType:
        dx = pt2.X - pt1.X
        dy = pt2.Y - pt1.Y
    else:
        dx = pt2[0] - pt1[0]
        dy = pt2[1] - pt1[1]
    return dx*dx + dy*dy


def MeanPoint(points):
    if type(points[0]) == arcpyPointType:
        meanX = numpy.mean([point.X for point in points])
        meanY = numpy.mean([point.Y for point in points])
        return arcpy.Point(meanX, meanY)
    else:
        meanX = numpy.mean([point[0] for point in points])
        meanY = numpy.mean([point[1] for point in points])
        return (meanX, meanY)


def MedianPoint(points):
    if type(points[0]) == arcpyPointType:
        medianX = numpy.median([point.X for point in points])
        medianY = numpy.median([point.Y for point in points])
        return arcpy.Point(medianX, medianY)
    else:
        medianX = numpy.median([point[0] for point in points])
        medianY = numpy.median([point[1] for point in points])
        return (medianX, medianY)

    
def SamePoint(point1, point2, eps):
    if type(point1) == arcpyPointType:
        return abs(point1.X - point2.X) < eps and abs(point1.Y - point2.Y) < eps
    else:
        return abs(point1[0] - point2[0]) < eps and abs(point1[1] - point2[1]) < eps

        
def RemovePoints(allPoints, n, center):
    """ returns a copy of the list of allPoints, with the n most distant points from
    center removed."""
    if len(allPoints) <= n:
        return []
    if n <= 0:
        return allPoints[:]
    #Create a list of tuples (distance, pt)
    #if we sort a list of these tuples, they will be in distance order
    #I use the distance squared, since it is easier and just as effective.
    distances = [(Distance2(pt,center), pt) for pt in allPoints]
    distances.sort()
    return [item[1] for item in distances[:-n]]


def Mcp(pointList):
    if type(pointList[0]) != arcpyPointType:
        pointList = [arcpy.Point(point[0],point[1]) for point in pointList]
    #MinimumBoundingGeometry() will not accept a list or an arcpy.Array of geometries,
    #only a single geometry works (Polyline or Multipoint is much faster than a Polygon).
    points = arcpy.Multipoint(arcpy.Array(pointList))
    empty = arcpy.Geometry()
    mcpList = arcpy.MinimumBoundingGeometry_management(points, empty, "CONVEX_HULL", "ALL")
    return mcpList[0]

    
def FloatingCenter(locationLayer, mcpFeatureClass, percentUsed, centerMethod):
    points = GetPoints(locationLayer)
    countOfPointsToRemove = int((1.0 - percentUsed/100.0) * len(points))
    #This will limit the number of center point recalculations to 50
    #This can be a real time saver for very large datasets.
    pointsRemovedPerIteration = max(1, countOfPointsToRemove/50)
    utils.info("Removing " + str(countOfPointsToRemove) + " of " + str(len(points)) + " points.")
    countOfPointsRemoved = 0
    while countOfPointsRemoved < countOfPointsToRemove:
        if countOfPointsRemoved % pointsRemovedPerIteration == 0:
            center = centerMethod(points)
            utils.info("New Center Point at " + str(center) + ".")
        points = RemovePoints(points, pointsRemovedPerIteration, center)
        countOfPointsRemoved += pointsRemovedPerIteration
    mcp = Mcp(points)
    arcpy.CopyFeatures_management(mcp, mcpFeatureClass)

    
def FixedCenter(locationLayer, mcpFeatureClass, percentUsed, centerMethod):
    points = GetArcpyPoints(locationLayer)
    countOfPointsToRemove = int((1.0 - percentUsed/100.0) * len(points))
    utils.info("Removing " + str(countOfPointsToRemove) + " of " + str(len(points)) + " points.")
    center = centerMethod(points)
    utils.info("Center = " + str(center) + ".")
    points = RemovePoints(points, countOfPointsToRemove, center)
    mcp = Mcp(points)
    arcpy.CopyFeatures_management(mcp, mcpFeatureClass)


def AddArea(locationLayer, mcpFeatureClass, percentUsed):
    #using arcpyPoints here is 8% (143points) to 44% (18407points) faster
    #points = GetPoints(locationLayer)
    points = GetArcpyPoints(locationLayer)
    finalLength = int(0.5 + (percentUsed/100.0) * len(points))
    utils.info("Removing " + str(len(points) - finalLength) + " of " + str(len(points)) + " points.")
    arcpy.SetProgressor("step", "Finding points to ignore...", 0,len(points) - finalLength, 1)
    while finalLength < len(points):
        points = RemovePointWithMostArea(points)
        arcpy.SetProgressorPosition()
    mcp = Mcp(points)
    arcpy.CopyFeatures_management(mcp, mcpFeatureClass)


def RemovePointWithMostArea(allPoints):
    mcp = Mcp(allPoints)
    area = mcp.area
    if not area:
        raise ValueError("Insufficeint points to calculate an MCP") 
    #Assume only 1 part, and only one polygon per part,
    #Assume each polygon has sufficient vertices to remove 1 and still have an area (i.e. 4 or more)
    #The definition of a Minimum Convex Polygon guarantees this so long as there are sufficient points.
    boundaryPoints = mcp.getPart(0)  #returns an arcpy.Array()
    bestPoints, maximumArea = (None, 0)
    for point in boundaryPoints:
        if type(allPoints[0]) != arcpyPointType:
            point = (point.X, point.Y) #use only if using (x,y) instead of arcpy.point()
        #FIXME epsilon is dependent on spatial resolution and coordinate system.
        #If I assume that coordinates are meters, then 1 millimeter is sufficient.
        newPoints = RemovePoint(allPoints, point, 1e-3)
        mcp = Mcp(newPoints)
        deltaArea = area - mcp.area
        if deltaArea > maximumArea:
            bestPoints = newPoints
            maximumArea = deltaArea
    return bestPoints


def RemovePoint(points, point, eps):
    newPoints = []
    for pt in points:
        if SamePoint(pt, point, eps):
            continue
        newPoints.append(pt)
    return newPoints

    
def CreateMCP(locationLayer, mcpFeatureClass, percentUsed, removalMethod, userPoint):
    if percentUsed == 100:
        arcpy.MinimumBoundingGeometry_management(locationLayer, mcpFeatureClass, 
                                         "CONVEX_HULL", "ALL")
    else:
        if removalMethod == "Area_Added":
            AddArea(locationLayer, mcpFeatureClass, percentUsed)
        elif removalMethod == "User_Point":
            # the lambda function should act like MeanPoint(), except
            # it takes an (unused) list of points and return the constant user point
            FixedCenter(locationLayer, mcpFeatureClass, percentUsed, (lambda x: userPoint))
        elif removalMethod == "Floating_Mean":
            FloatingCenter(locationLayer, mcpFeatureClass, percentUsed, MeanPoint)
        elif removalMethod == "Floating_Median":
            FloatingCenter(locationLayer, mcpFeatureClass, percentUsed, MedianPoint)
        elif removalMethod == "Fixed_Median":
            FixedCenter(locationLayer, mcpFeatureClass, percentUsed, MedianPoint)
        elif removalMethod == "Fixed_Mean":
            FixedCenter(locationLayer, mcpFeatureClass, percentUsed, MeanPoint)
        else:
            utils.warn("Removal Method was unrecognized. Using Fixed_Mean.")
            FixedCenter(locationLayer, mcpFeatureClass, percentUsed, MeanPoint)



                
if __name__ == "__main__":

    status = arcpy.CheckProduct("ArcInfo")
    if status != "Available" and status != "AlreadyInitialized":
        utils.die("This tool requires an ArcInfo License.  License check status = " + status + ".  Quitting.")

    locationLayer = arcpy.GetParameterAsText(0)
    mcpFeatureClass = arcpy.GetParameterAsText(1)
    percentUsed = arcpy.GetParameterAsText(2)
    removalMethod = arcpy.GetParameterAsText(3)
    userPoint = arcpy.GetParameterAsText(4)

    test = False
    if test:
        #locationLayer = r"C:\tmp\test2.gdb\winter2011"
        locationLayer = r"C:\tmp\test2.gdb\w2011a0901"
        mcpFeatureClass = r"C:\tmp\test2.gdb\mcp_area_95_3"
        percentUsed = "99"
        removalMethod = "Area_Added" #Fixed_Mean, Fixed_Median, Floating_Mean, Floating_Median, User_Point, Area_Added 
        userPoint = "-300000 1760000"

    #
    # Input validation
    #
    try:
        percentUsed = float(percentUsed)
    except ValueError:
        utils.die("Percentage of Points was not a valid number. Quitting.")
    if percentUsed <= 0 or 100 < percentUsed:
        utils.warn("Percentage of Points was outside the range (0,100]. Input truncated to fit.")
    
    if removalMethod == "User_Point":
        if not userPoint:
            utils.die("User Point was not provided. Quitting.")
        else:
            try:
                #check for valid point (i.e two float numbers) in "x y"
                pts = userPoint.split()
                userPoint = arcpy.Point(float(pts[0]), float(pts[1]))
                #User_Point uses FixedCenter(), so it must be an arcpy.Point() not (x,y)
            except ValueError:
                utils.die("User Point was not valid. Quitting.")
                
    if not locationLayer:
        utils.die("No location layer was provided. Quitting.")
        
    if not arcpy.Exists(locationLayer):
        utils.die("Location layer cannot be found. Quitting.")

    if not mcpFeatureClass:
        utils.die("No output requested. Quitting.")

    #
    # Do the work
    #
    CreateMCP(locationLayer, mcpFeatureClass, percentUsed, removalMethod, userPoint)
