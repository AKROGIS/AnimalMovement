# ------------------------------------------------------------------------------
# UD_SmoothingFactor.py
# Created: 2011-10-11
#
# Title:
# Utilization Distribution Smoothing Factor for Kernel Density Estimator
#
# Tags:
# contour, home, range, animal, tracking, telemetry, ecology, kernel, density
#
# Summary:
# Calculates the smoothing factor to use with the kernel density estimator
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



if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        arcpy.AddError("Unable to checkout the Spatial Analyst Extension.  Quitting.")
        sys.exit()        

    locationLayer = arcpy.GetParameterAsText(0)
    hRefmethod = arcpy.GetParameterAsText(1)
    modifier = arcpy.GetParameterAsText(2)
    proportionAmount = arcpy.GetParameterAsText(3)
    scaleToUnitVariance = arcpy.GetParameterAsText(4)

    test = False
    if test:
        locationLayer = r"C:\tmp\test2.gdb\w2011a0901"
        hRefmethod = "WORTON" #Worton,Tufto,Silverman,Gaussian
        modifier = "NONE" #NONE,PROPORTIONAL,LSCV,BCV2
        proportionAmount = "0.7"
        scaleToUnitVariance = "False"

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

    points = GetPoints(locationLayer)
    h = GetSmoothingFactor(points, hRefmethod, modifier, proportionAmount)
    arcpy.SetParameterAsText(4, str(h))
            
        