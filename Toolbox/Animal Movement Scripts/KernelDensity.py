import sys
import os
import math
import random
import datetime
import numpy
import arcpy

arcpy.CheckOutExtension("Spatial")

def GetPoints1(pointsFeature, shapeName):
    print "Get Points", datetime.datetime.now()
    x = []
    y = []
    if not shapeName:
        shapeName = arcpy.Describe(pointsFeature).shapeFieldName
    for row in arcpy.SearchCursor(pointsFeature):
        shape = row.getValue(shapeName)
        x.append(shape.getPart().X)
        y.append(shape.getPart().Y)
    print "Got Points", datetime.datetime.now()
    return x,y

def sumSqDev(l,m):
    s = 0
    for i in l:
        s = s + (i-m)*(i-m)
    return s




def GetPointsRandom(count, radius):
    points = []
    for i in range(count):
        #x = random.uniform(-radius,radius)
        #y = random.uniform(-radius,radius)
        x = random.normalvariate(0, radius)
        y = random.normalvariate(0, radius)
        points.append( (x,y) )
    return points

def GetPoints(pointsFeature, shapeName):
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
            print "CV Score: Warning duplicate locations found, results may be invalid."
            print "CV Score:         Separating the locations by 1 unit."
            d = 1
        term2 = term2a * math.exp(d * term2b)
        term3 = term3a * math.exp(d * term3b) 
        term5 = (term2 - term3)/term4        
        total = total + term5
        #print "d",d,"term2",term2,"term3",term3,"term5",term5, "total", total
    result = term1 + total
    print "CV", h, result
    return result


def BCV2(allDistancesSquared, n, h):
    term1 = 1.0 / (4 * math.pi * h * h * (n - 1))
    term2 = 8*(n-1)*(n-2)*h*h*math.pi    
    #print "h",h,"n",n,"term1",term1,"term2",term2
    total = 0.0
    for d in allDistancesSquared:
        if d == 0:
            print "BCV2 Score: Warning duplicate locations found, results may be invalid."
            print "BCV2 Score:         Separating the locations by 1 unit."
            d = 1
        D = d/(h*h)
        D2 = D * D
        term3 = (D2 -8*D + 8) * math.exp(-D2/2) / term2     
        total = total + term3
        #print "d",d,"D",D,"D2",D2,"term3",term3,"total", total
    result = term1 + total
    print "BCV2", h, result
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
    allSquaredDistances = DistancesSquared(points)

    h1 = Search(func, allSquaredDistances, n, h, 0.05, 2.0, 0.1)
    if h1 < 0.10 * h or h1 > 1.91 * h:
        # then it is the min or max value checked
        print "Cross Validation using",func.__name__,"failed to minimize, using h_ref."
        return h
#    return h1
    print h1
    h2 = Search(func, allSquaredDistances, n, h1, 0.89, 1.11, 0.01)
    print h2
    h3 = Search(func, allSquaredDistances, n, h2, 0.989, 1.011, 0.001)
    return h3

def HrefWorton(points):
    n = len(points)
    x = [point[0] for point in points]
    y = [point[1] for point in points]    
    varx = numpy.var(x) * n/(n-2.0) #remove sampling bias
    vary = numpy.var(y) * n/(n-2.0)
    print "varx", varx, "vary", vary
    std = math.sqrt(0.5 * (varx + vary))
    return std / math.pow(n, 1/6.0)
    
def HrefTufto(points):
    n = len(points)
    x = [point[0] for point in points]
    y = [point[1] for point in points]    
    varx = numpy.var(x) * n/(n-2.0) #remove sampling bias
    vary = numpy.var(y) * n/(n-2.0)
    print "varx", varx, "vary", vary
    std = 0.5 * math.sqrt(varx + vary)
    return std / math.pow(n, 1/6.0)

def GausianApproximation(points):
    n = len(points)
    x = [point[0] for point in points]
    y = [point[1] for point in points]    
    stdx = numpy.std(x) * n/(n-1.0) #remove sampling v. population bias
    stdy = numpy.std(y) * n/(n-1.0) #remove sampling v. population bias
    print "stdx", stdx, "stdy", stdy
    std = math.sqrt(stdx*stdx + stdy*stdy)
    g = std * math.pow(4.0/(3.0*n),0.2)
    return g

def GausianApproximation2(points):
    n = len(points)
    x = [point[0] for point in points]
    y = [point[1] for point in points]    
    stdx = numpy.std(x) * n/(n-1.0) #remove sampling v. population bias
    stdy = numpy.std(y) * n/(n-1.0) #remove sampling v. population bias
    print "stdx", stdx, "stdy", stdy
    std = 0.5 *(stdx + stdy)
    g = std * math.pow(4.0/(3.0*n),0.2)
    return g

def GetH(method, fixed_value, fixed_percent, locationsFC, shapeFieldName):
    h_methods = ["Manual", "LSCV", "AdHoc_Tufto", "adHoc_Silverman", "Worton", "hRef"]

    if method == h_methods[0]:
        return fixed_value

    points = GetPointsRandom(300,100)    
    #points = GetPoints(locationsFC,shapeFieldName)
    #print points
    if method == h_methods[1]:
        h = HrefTufto(points)
        print "HrefTufto", h
        h = HrefWorton(points)
        print "HrefWorton", h
        h = GausianApproximation(points)
        print "GausianApproximation (dist)", h
        h = GausianApproximation2(points)
        print "GausianApproximation (avg)", h
        h1 = Minimize(CV, h, points)
        print "LSCV", h1
        #h1 = Minimize(BCV2, h, points)
        #print "BCV2", h1
        return h1
    if method == h_methods[2]:
        h = HrefWorton(points)
        return fixed_percent * h
    if method == h_methods[3]:
        h = HrefTufto(points)
        return fixed_percent * h
    if method == h_methods[4]:
        return Worton(x,y)
    if method == h_methods[5]:
        return fixed_percent * hRef(x,y)
    raise ArgumentError






def Golden(d2s, min, max):
    print "Goldon Min", Score(d2s,min)
    return Score(d2s,max)

def Golden2(xs, ys, min, max):
    print "Goldon2 Min", Score2(xs, ys, min)
    return Score2(xs, ys, max)
 
def LSCV1(xs,ys):
    #build the distance squared list
    d2 = []
    n = len(xs)
    for i in range(n):
        for j in range(n):
            x = xs[j] - xs[i]
            y = ys[j] - ys[i]
            d2.append(x*x + y*y)

    h = AdHocH(xs,ys)
    #h = GausianApproximation(xs,ys)
    hmin = h*0.25  #Worton .1
    hmax = h*1.0   #Worton 1.5
    return Golden(d2, hmin, hmax)    

def LSCV2(xs,ys):
    h = AdHocH(xs,ys)
    #h = GausianApproximation(xs,ys)
    hmin = h*0.25  #Worton .1
    hmax = h*1.0   #Worton 1.5
    return Golden2(xs, ys, hmin, hmax)    

def GetUniqueValues(featureClass,whereField):
    values = set([]) #members in a set are always unique
    rows = arcpy.SearchCursor(featureClass,"","",whereField)
    for row in rows:
        if not row.isNull(whereField):
            values.add(row.getValue(whereField))
    return values

def BuildQuery(featureClass,whereField,value):
    # this will throw an exception if whereField is not in featureClass
    # building a valid query is a pain.
    # we need to correctly delimit the field name (with "" or [])
    # and the value based on the field type
    # does not correctly handle delimiting date types.
    field = arcpy.AddFieldDelimiters(featureClass, whereField)
    fields = arcpy.ListFields(featureClass)
    type = [f.type.lower() for f in fields if f.name == whereField]
    if type and type[0] == "string":
        quote = "'"
    else:
        quote = ""
    return field + " = " + quote + str(value) + quote

def PolylineToPolygon(lineFC, polyFC):
    #arcpy.FeatureToPolygon_management does not copy attributes, and
    #doesn't guarantee order, and doesn't merge iso lines

    workspace,featureClass = os.path.split(polyFC)
    arcpy.CreateFeatureclass_management(workspace,featureClass, 
                                        "Polygon", lineFC, "SAME_AS_TEMPLATE",
                                        "SAME_AS_TEMPLATE", lineFC)

    arcpy.AddMessage("Empty feature class has been created")

    lineDescription = arcpy.Describe(lineFC)
    polyDescription = arcpy.Describe(polyFC)

    # workaround for bug (#NIM064306)
    # wherein ValidateFieldName(field,workspace\feature_dataset)
    # returns incorrect results.  Workaround: remove the feature_dataset
    workspace = workspace.lower()
    if workspace.rfind(".mdb") > 0:
        workspace = workspace[:workspace.rfind(".mdb")+4]
    else:
        if workspace.rfind(".gdb") > 0:
            workspace = workspace[:workspace.rfind(".gdb")+4]
        
    # Since the input an output may not be in the same workspace, the field
    # naming requirements may not be the same (i.e. truncated for shapefiles).
    # Therefore, create a simple field mapping from input to output
    fields = {}
    for field in lineDescription.fields:
        name = field.name
        if (name != lineDescription.shapeFieldName and
            name != lineDescription.OIDFieldName and
            field.editable): #skip uneditable fields like Shape_Length
            fields[name] = arcpy.ValidateFieldName(name,workspace)
            #print workspace, name, "=>", fields[name]  

    #create the cursors
    polys = arcpy.InsertCursor(polyFC)
    lines = arcpy.SearchCursor(lineFC)
    for line in lines:
        poly = polys.newRow()
        shape = line.getValue(lineDescription.shapeFieldName)
        #convert polyline to shape
        newshape = shape
        poly.setValue(polyDescription.shapeFieldName,newshape)
        for field in fields:
            poly.setValue(fields[field], line.getValue(field))
        polys.insertRow(poly)

    #Close/Delete row/cursor objects to remove the exclusive lock
    try:
        del poly
    except NameError:
        pass #this happens if poly is not defined (i.e. there are no lines)
    del polys



def ContourLinesToPolygons(lineFC, polyFC):
    #arcpy.FeatureToPolygon_management does not copy attributes, and
    #doesn't guarantee order, and doesn't merge iso lines
    #this does.

    workspace,featureClass = os.path.split(polyFC)
    arcpy.CreateFeatureclass_management(workspace,featureClass, 
                                        "Polygon", lineFC, "SAME_AS_TEMPLATE",
                                        "SAME_AS_TEMPLATE", lineFC)
    
    # I use lineFC as a template for polyFC, even though I don't use all fields
    # (typically there is only the Contour field, besides the standard fields)
    # I do this to avoid adding a field (schema lock bug with PGDB)
    # and I don't want to figure out if Contour is a float or int
    
    lineDescription = arcpy.Describe(lineFC)
    polyDescription = arcpy.Describe(polyFC)

    uniqueValues = GetUniqueValues(lineFC,"Contour")

    if uniqueValues:
        uniqueValues = list(uniqueValues)
        uniqueValues.sort()
        uniqueValues.reverse() #build biggest to smallest
        polys = arcpy.InsertCursor(polyFC)
        for value in uniqueValues:
            query = BuildQuery(lineFC,"Contour",value)
            lines = arcpy.SearchCursor(lineFC,query)
            poly = polys.newRow()
            array = arcpy.Array()
            for line in lines:
                shape = line.getValue(lineDescription.shapeFieldName)
                for i in range(shape.partCount):
                    array.add(shape.getPart(i))
            newshape = arcpy.Polygon(array)
            poly.setValue(polyDescription.shapeFieldName, newshape)
            poly.setValue("Contour", value)
            polys.insertRow(poly)

        #Close/Delete row/cursor objects to remove the exclusive lock
        del poly
        del polys


def GetContourList(contourInput):
    contourInput = contourInput.replace(',',' ').replace(';',' ').replace(':',' ')
    try:
        contourList = [float(c) for c in contourInput.split()]
    except ValueError:
        print "Un-recognized character in the contour list"
        sys.exit()

    contourList = [f for f in contourList if f > 0 and f < 100]
    contourList = list(set(contourList)) #remove duplicates
    contourList.sort()
    return contourList

def GetProbabilityRaster(features, cellSize, searchRadius):
    try:
        # if map units are meters, output units are per kmsq; therefore default scaling factor = 1,000,000
        # scaling factor should be large to avoid floating point errors.
        # ESRI uses a quartic approximation to the bivariate normal distribution.
        # the search radius for quartic does not map to the bandwidth for a ND
        populationField = "NONE"
        scaleFactor = "" #SQUARE_MAP_UNITS, SQUARE_MILES, SQUARE_KILOMETERS, ...
        kernel = arcpy.sa.KernelDensity(locations, populationField, cellSize, searchRadius, scaleFactor)
        #classify the results into 100 equal interval bins, and invert (0..100 -> 100..0)
        raster = 101 - arcpy.sa.Slice(kernel,100,"EQUAL_INTERVAL")
        gotRaster = True
        return False, None
    except:
        print "exception"
        return False, None


#Globals
h_method = "LSCV"
h_manual = 20000
#locations = r"C:\tmp\test2.gdb\w2011a0901"
locations = r"C:\tmp\test2.gdb\w2011a0926"
#locations = r"C:\tmp\test2.gdb\winter2011"
whereField = ""
#whereField = "AnimalId"
cellSize = 1000
rasterName = ""
contourLines = ""
contourPolys = ""
#if dir is specified, and a valid directory,
# then individual probability rasters are saved in dir
dir = ""
#dir = r"C:\tmp\kd_test"

#You can't make contour polys unless you also make contour lines
rasterName = r"C:\tmp\kd_test\prob_all_sr40k.tif"
#contourLines = r"C:\tmp\test2.gdb\clines2a"
#contourPolys = r"C:\tmp\test2.gdb\cpolys2a"

#contourInput - string as a list of floating point numbers
#valid separators: comma(,) semicolon(;) colon(:) and whitespace in any combination
#any other non-numeric character(0123456789-+.eE) is an error
#numbers 0 or less and 100 or more are silently ignored
#numbers do not need to be in any order
#international numbers (comma as decimal separator) are not supported
#test: contourInput = "  45  +5:10, 65,90, 95,10;-4;;"
contourInput = "50,65,90,95"



#Get some information about the locations
description = arcpy.Describe(locations)
data_envelope = description.Extent
shapeName = description.shapeFieldName

#Get the contour list from the user's input
contourList = GetContourList(contourInput)

#get the smoothing factor from user's input
h = GetH(h_method, h_manual, 1.0, locations, shapeName)

print h
sys.exit()

#Double the smoothing factor to get the search radius.
#This seems counterintuitive, I would guess bandwidth = diameter, however
#because ESRI is using a quartic aproximation to the bivariate normal,
#the approximation is only equivalent to the 3rd order when r = 2s (s = standard deviation)
#there seems to be an implicit understanding that h and s are the same.
searchRadius = h * 2

print "Smoothing Factor",h,"Search Radius",searchRadius

#Use the environment to set the extent used by the raster creation tool
#The raster should extend beyond the data so that contours are not clipped
#everything within the searchRadius of a point should be non-zero
#add cellsize to avoid floating point rounding errors
buffer = searchRadius + cellSize 
arcpy.env.extent = arcpy.Extent(data_envelope.xmin-buffer,
                                data_envelope.ymin-buffer,
                                data_envelope.xmax+buffer,
                                data_envelope.ymax+buffer)

gotRaster = False
uniqueValues = None
if whereField in [field.name for field in arcpy.ListFields(locations)]:
    uniqueValues = GetUniqueValues(locations,whereField)

if uniqueValues:
    print "Normalizing by", whereField, "then adding"
    n = 0
    layer = "animalSelection"
    for value in uniqueValues:
        query = BuildQuery(locations,whereField,value)
        print "query", query
        if arcpy.Exists(layer):
            arcpy.Delete_management(layer)
        arcpy.MakeFeatureLayer_management(locations, layer, query)
        try:
            kernel = arcpy.sa.KernelDensity(layer, "NONE", cellSize, searchRadius, "")
            probRaster = 101 - arcpy.sa.Slice(kernel,100,"EQUAL_INTERVAL")
            if dir and os.path.isdir(dir):
                # Save individual probability rasters
                name = os.path.join(dir,"praster_"+str(value)+".tif")
                probRaster.save(name)
            if n:
                raster = raster + probRaster
                n = n + 1
            else:
                raster = probRaster
                n = 1
        finally:
            arcpy.Delete_management(layer)
    raster = arcpy.sa.Slice(raster,100,"EQUAL_INTERVAL")
    gotRaster = True
else:
    print "Using all data as is"
    gotRaster, raster = GetProbabilityRaster(locations, cellSize, searchRadius)

if gotRaster and rasterName:
    raster.save(rasterName)
    
if contourLines and contourList:
    arcpy.sa.ContourList(raster, contourLines, contourList)

if contourPolys and contourLines:
    ContourLinesToPolygons(contourLines, contourPolys)
