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

def GetPoints(pointsFeature, shapeName = None):
    points = []
    if not shapeName:
        shapeName = arcpy.Describe(pointsFeature).shapeFieldName
    for row in arcpy.SearchCursor(pointsFeature):
        shape = row.getValue(shapeName)
        points.append( (shape.getPart().X, shape.getPart().Y) )
    return points

def frange(x, y, jump):
  while x < y:
    yield x
    x += jump

def DistancesSquared(points):
    allSquaredDistances = []
    n = len(points)
    for i in range(n):
        for j in range(n):
            if i == j:
                continue
            dx = points[j][0] - points[i][0]
            dy = points[j][1] - points[i][1]
            d2 = dx*dx + dy*dy
            allSquaredDistances.append(d2)
    return allSquaredDistances

# Cross Validation function for h.
# approximates(?) the mean integrated square error (MISE) between the true density function and the kde
# we want the h that returns the minimum CV
def CV(allDistancesSquared, n, h):
    term1 = 1.0 / (math.pi * n * h * h)
    term2a = 1.0 / (4.0 * math.pi)
    term2b = -1.0 / (4.0 * h * h)
    term3a = 1.0 / math.pi
    term3b = -1.0 / (2.0 * h * h)
    term4 = n*n*h*h    
    #print "term1",term1,"term2a",term2a,"term3a",term3a,"term2b",term2b,"term3b",term3b,"term4",term4
    total = 0.0
    for d in allDistancesSquared:
        if d == 0:
            arcpy.AddWarning("Warning duplicate locations found, results may be invalid.")
            arcpy.AddWarning("        Separating the locations by 1 unit.")
            d = 1
        term2 = term2a * math.exp(d * term2b)
        term3 = term3a * math.exp(d * term3b) 
        term5 = (term2 - term3)/term4        
        total = total + term5
        #print "d",d,"term2",term2,"term3",term3,"term5",term5, "total", total
    result = term1 + total
    #print "CV", h, result
    return result


def BCV2(allDistancesSquared, n, h):
    term1 = 1.0 / (4 * math.pi * h * h * (n - 1))
    term2 = 8*(n-1)*(n-2)*h*h*math.pi    
    #print "h",h,"n",n,"term1",term1,"term2",term2
    total = 0.0
    for d in allDistancesSquared:
        if d == 0:
            arcpy.AddWarning("Warning duplicate locations found, results may be invalid.")
            arcpy.AddWarning("        Separating the locations by 1 unit.")
            d = 1
        D = d/(h*h)
        D2 = D * D
        term3 = (D2 -8*D + 8) * math.exp(-D2/2) / term2     
        total = total + term3
        #print "d",d,"D",D,"D2",D2,"term3",term3,"total", total
    result = term1 + total
    #print "BCV2", h, result
    return result

def Search(func, allSquaredDistances, n, h_ref, min_percent, max_percent, step):

    h_min = min_percent * h_ref
    h_max = max_percent * h_ref
    h_step = step  * h_ref

    h_res = h_max
    minErr = func(allSquaredDistances, n, h_max)

    for h_test in frange(h_min, h_max, h_step):
        err = func(allSquaredDistances, n, h_test)
        if (err < minErr):
            minErr = err
            h_res = h_test     
    return h_res

def Minimize(func, h, points):
    n = len(points)
    if n > 2000:
        raise ValueError("Too many points for Cross Validation, limit is 2000")
        msg = "Too many points for Cross Validation, limit is 2000, using hRef."
        arcpy.AddWarning(msg)
        return h
    
    allSquaredDistances = DistancesSquared(points)

    h1 = Search(func, allSquaredDistances, n, h, 0.05, 2.0, 0.1)
    if h1 < 0.10 * h or h1 > 1.91 * h:
        # then it is the min or max value checked
        msg = "Cross Validation using "+func.__name__+" failed to minimize, using hRef."
        arcpy.AddWarning(msg)
        return h
#    return h1
    #print h1
    h2 = Search(func, allSquaredDistances, n, h1, 0.89, 1.11, 0.01)
    #print h2
    h3 = Search(func, allSquaredDistances, n, h2, 0.989, 1.011, 0.001)
    return h3

def HrefWorton(points):
    n = len(points)
    x = [point[0] for point in points]
    y = [point[1] for point in points]    
    varx = numpy.var(x) * n/(n-2.0) #remove sampling bias
    vary = numpy.var(y) * n/(n-2.0)
    #print "varx", varx, "vary", vary
    std = math.sqrt(0.5 * (varx + vary))
    return std / math.pow(n, 1/6.0)
    
def HrefTufto(points):
    n = len(points)
    x = [point[0] for point in points]
    y = [point[1] for point in points]    
    varx = numpy.var(x) * n/(n-2.0) #remove sampling bias
    vary = numpy.var(y) * n/(n-2.0)
    #print "varx", varx, "vary", vary
    std = 0.5 * math.sqrt(varx + vary)
    return std / math.pow(n, 1/6.0)

def HrefGaussianApproximation(points):
    # for univariate normally distributed data
    # from http://en.wikipedia.org/wiki/Kernel_density_estimation
    n = len(points)
    x = [point[0] for point in points]
    y = [point[1] for point in points]    
    stdx = numpy.std(x) * n/(n-1.0) #remove sampling v. population bias
    stdy = numpy.std(y) * n/(n-1.0) #remove sampling v. population bias
    #print "stdx", stdx, "stdy", stdy
    #This is regan's guess as to how to combine stdx and stdy
    #std = 0.5 *(stdx + stdy) #average
    std = math.sqrt(stdx*stdx + stdy*stdy) #distance
    g = std * math.pow(4.0/(3.0*n),0.2)
    return g

def HrefSilverman(points):
    #FIXME
    return HrefWorton(points)

def GetSmoothingFactor(points, hRefmethod, modifier, proportionAmount):
    hRef = 0
    if hRefmethod.lower() == "worton":
        hRef = HrefWorton(points)
    elif hRefmethod.lower() == "tufto":
        hRef = HrefTufto(points)
    elif hRefmethod.lower() == "silverman":
        hRef = HrefSilverman(points)
    elif hRefmethod.lower() == "gaussian":
        hRef = HrefGaussianApproximation(points)

    if hRef == 0:
        arcpy.AddError("No valid hRef method was provided. Quitting.")
        sys.exit()

    if modifier.lower() == "proportion":
        h = proportionAmount * hRef
    elif modifier.lower() == "lscv":
        h = Minimize(LSCV, hRef, points)
    elif modifier.lower() == "bcv2":
        h = Minimize(BCV2, hRef, points)
    else:
        h = hRef

    arcpy.AddMessage("hRef (" + hRefmethod + ") = " + str(hRef))    
    arcpy.AddMessage("Using h = " +  str(h))
    return h

def IsFloat(something):
    try:
        float(something)
    except (ValueError, TypeError):
        return False
    return True

def GetUDRaster(features, cellSize, smoothingFactor):
    cellSize, searchRadius = SetupRaster(features, smoothingFactor, cellSize)
    return GetProbabilityRaster(features, cellSize, searchRadius)

def SetupRaster(features, smoothingFactor, cellSize = None):
    envelope = arcpy.Describe(features).Extent
    maxDivisions = 2000
    if not IsFloat(cellSize):
        cellSize =  GetCellSize(envelope, maxDivisions)
    else:
        cellSize = float(cellSize) #all parameters are text from toolbox
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
        print sys.exc_info()[1]
        arcpy.AddError(sys.exc_info()[1])
        return False, None





def CreateIsopleths(isopleths, raster, lineFc, polyFc, donutFc):
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
    
    if lineFc:   
        arcpy.sa.ContourList(raster, lineFc, isopleths)
        if polyFc:
            IsoplethLinesToPolygons(lineFc, polyFc)
        if donutFc:
            IsoplethLinesToDonuts(lineFc, donutFc)
    else:
        tmpLayer = "in_memory\IsoplethPolylines"
        if arcpy.Exists(tmpLayer):
            arcpy.Delete_management(tmpLayer)
        arcpy.sa.ContourList(raster, tmpLayer, isopleths)
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
    
    isoplethInput = isoplethInput.replace(',',' ').replace(';',' ').replace(':',' ')
    try:
        isoplethList = [float(c) for c in isoplethInput.split()]
    except ValueError:
        print "Un-recognized character in the isopleth list"
        sys.exit()

    isoplethList = [f for f in isoplethList if f > 0 and f < 100]
    isoplethList = list(set(isoplethList)) #remove duplicates
    isoplethList.sort()
    return isoplethList

def TestGetIsopleth():
    a = '100,0,   4.65e1, 45,   +5:10.1, 65,90,5 10;-4;;'
    print a
    print GetIsoplethList(a)


def IsoplethLinesToPolygons(lineFC, polyFC, fieldname="contour"):
    """Builds a featureclass named polyFC, by creating a polygon
    from all the lines in lineFC that have the same value in the
    field named fieldname.  The largest values in fieldname are
    written first, this will provide the expected polygon stacking. 
    
    arcpy.FeatureToPolygon_management creates polygons from lines,
    but it does not copy attributes, doesn't guarantee order, and
    doesn't merge polygons with the same value."""

    uniqueValues = GetUniqueValues(lineFC,fieldname)

    if not uniqueValues:
        arcpy.AddWarning("No field named '"+fieldname+"' in "+lineFC)
        return

    workspace,featureClass = os.path.split(polyFC)
    arcpy.CreateFeatureclass_management(workspace,featureClass, 
                                        "Polygon", lineFC, "SAME_AS_TEMPLATE",
                                        "SAME_AS_TEMPLATE", lineFC)       
    # I use lineFC as a template for polyFC, even though I don't need all fields
    # (typically there is only the 'contour' field, besides the standard fields)
    # I do this to avoid adding a field (schema lock bug with PGDB)
    # and I don't want to worry about the data type (float or int) of 'contour'
    
    lineDescription = arcpy.Describe(lineFC)
    polyDescription = arcpy.Describe(polyFC)

    uniqueValues = list(uniqueValues)
    uniqueValues.sort()
    uniqueValues.reverse() #builds biggest to smallest
    polys = arcpy.InsertCursor(polyFC)
    for value in uniqueValues:
        query = BuildQuery(lineFC,fieldname,value)
        lines = arcpy.SearchCursor(lineFC,query)
        poly = polys.newRow()
        array = arcpy.Array()
        for line in lines:
            shape = line.getValue(lineDescription.shapeFieldName)
            for i in range(shape.partCount):
                array.add(shape.getPart(i))
        newshape = arcpy.Polygon(array)
        poly.setValue(polyDescription.shapeFieldName, newshape)
        poly.setValue(fieldname, value)
        polys.insertRow(poly)

    #Close/Delete row/cursor objects to remove the exclusive lock
    del poly
    del polys

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

    uniqueValues = GetUniqueValues(lineFC,fieldname)

    if not uniqueValues:
        arcpy.AddWarning("No field named '"+fieldname+"' in "+lineFC)
        return

    workspace,featureClass = os.path.split(polyFC)
    arcpy.CreateFeatureclass_management(workspace,featureClass, 
                                        "Polygon", lineFC, "SAME_AS_TEMPLATE",
                                        "SAME_AS_TEMPLATE", lineFC)       
    # I use lineFC as a template for polyFC, even though I don't need all fields
    # (typically there is only the 'contour' field, besides the standard fields)
    # I do this to avoid adding a field (schema lock bug with PGDB)
    # and I don't want to worry about the data type (float or int) of 'contour'
    
    lineDescription = arcpy.Describe(lineFC)
    polyDescription = arcpy.Describe(polyFC)

    #we will build polygon n and then subtract polygon n+1
    #to do this we need to sort biggest to smallest
    #the last polygon has nothing subtracted from it.
    #polygons with holes are created as an array of polygons
    uniqueValues = list(uniqueValues)
    uniqueValues.sort()
    uniqueValues.reverse() #sort biggest to smallest
    polys = arcpy.InsertCursor(polyFC)
    lines2 = []
    for i in range(len(uniqueValues)):
        # We could use the inner polygon from the last loop
        # but ESRI's cursors are funny things, so we are doing
        # this the brute force way.  It's a little slower, but
        # not noticably
        value1 = uniqueValues[i]
        query1 = BuildQuery(lineFC,fieldname,value1)
        lines1 = arcpy.SearchCursor(lineFC,query1)
        if i == len(uniqueValues) - 1: #last one
            lines2 = []
        else:
            value2 = uniqueValues[i+1]
            query2 = BuildQuery(lineFC,fieldname,value2)
            lines2 = arcpy.SearchCursor(lineFC,query2)
        poly = polys.newRow()
        array = arcpy.Array()
        for line in lines1:
            shape = line.getValue(lineDescription.shapeFieldName)
            for j in range(shape.partCount):
                array.add(shape.getPart(j))
        #lines1/2 are arcpy.Cursors, and cannot be added, so we iterate each separately
        for line in lines2:
            shape = line.getValue(lineDescription.shapeFieldName)
            for j in range(shape.partCount):
                array.add(shape.getPart(j))
        newshape = arcpy.Polygon(array)
        poly.setValue(polyDescription.shapeFieldName, newshape)
        poly.setValue(fieldname, value1)
        polys.insertRow(poly)

    #Close/Delete row/cursor objects to remove the exclusive lock
    del poly
    del polys


def GetUniqueValues(featureClass,whereField):
    """Make a list of all values in the whereField of
    featureClass.  Each value in the returned list is unique.
    That is multiple identical values are only reported once."""
    
    # members in a set are guaranteed to be unique
    values = set([]) 
    rows = arcpy.SearchCursor(featureClass,"","",whereField)
    for row in rows:
        if not row.isNull(whereField):
            values.add(row.getValue(whereField))
    return values


def BuildQuery(featureClass,whereField,value):
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
    return field + " = " + quote + str(value) + quote

def GetSmoothingFactors(subsetIdentifier, uniqueValues, locationLayer, hRefmethod, modifier, proportionAmount):
    layer = "subsetForSmoothingFactor"
    hList = []
    for value in uniqueValues:
        query = BuildQuery(locationLayer,subsetIdentifier,value)
        arcpy.AddMessage("Calculating h for " + query)
        if arcpy.Exists(layer):
            arcpy.Delete_management(layer)
        arcpy.MakeFeatureLayer_management(locationLayer, layer, query)
        try:
            points = GetPoints(layer)
            h = GetSmoothingFactor(points, hRefmethod, modifier, proportionAmount)
            hList.append(h)
        finally:
            arcpy.Delete_management(layer)
    return hList

def ChooseSmoothingFactor(hList, hRefToUse):
    if hRefToUse.lower() != "minimum":
        return min(hList)
    if hRefToUse.lower() != "maximum":
        return max(hList)
    return numpy.average(hList)

def BuildNormalizedRaster(subsetIdentifier, uniqueValues, locationLayer, hList, saveRasters, rasterFolder):
    n = 0
    layer = "subsetSelectionForRaster"
    cellSize, searchRadius = SetupRaster(locationLayer, max(hList))
    hDict = {}
    for k,v in zip(uniqueValues,hList):
        hDict[k]=v
    for value in uniqueValues:
        query = BuildQuery(locationLayer,subsetIdentifier,value)
        arcpy.AddMessage("Creating KDE raster for " + query)
        if arcpy.Exists(layer):
            arcpy.Delete_management(layer)
        arcpy.MakeFeatureLayer_management(locationLayer, layer, query)
        try:
            searchRadius = 2 * hDict[value]
            gotRaster, probRaster = GetProbabilityRaster(layer, cellSize, searchRadius)
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

    isoplethList = GetIsoplethList(isoplethInput)
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
        uniqueValues = GetUniqueValues(locationLayer,subsetIdentifier)
    if not uniqueValues:
        arcpy.AddError("Could not generate a list of unique values for "+subsetIdentifier+". Quitting.")
        sys.exit()

    if hRefmethod.lower() == "fixed":
        hList = [fixedHRef for eachItem in uniqueValues]
    else:
        hList = GetSmoothingFactors(subsetIdentifier, uniqueValues, locationLayer, hRefmethod, fixedHRef, modifier, proportionAmount)
        if hRefToUse.lower() != "bydataset":
            h = ChooseSmoothingFactor(hList, hRefToUse)
            arcpy.AddMessage("Using h = " + str(h) +" ("+hRefToUse+")")
            hList = [h for eachItem in uniqueValues]
        
    #
    # rasters
    #
    gotRaster, raster = BuildNormalizedRaster(subsetIdentifier, uniqueValues, locationLayer, hList, saveRasters, rasterFolder)  
    if gotRaster:
        arcpy.AddMessage("Created the temporary KDE raster")
    else:
        arcpy.AddError("Unable to create KDE raster. Quitting.")
        
    #
    # isopleths (for total raster only)
    #
    CreateIsopleths(isoplethList, raster, isoplethLines, isoplethPolys, isoplethDonuts)

