# -*- coding: utf-8 -*-
"""
UD_SmoothingFactor.py
Created: 2011-10-11

Title:
Utilization Distribution Smoothing Factor for Kernel Density Estimator

Tags:
contour, home, range, animal, tracking, telemetry, ecology, kernel, density

Summary:
Calculates the smoothing factor to use with the kernel density estimator

Usage:
The reference smoothing factor (href) is also know as hopt, the optimal smoothing factor, and is generally considered to perform poorly compared with the LSCV method (Horne & Garton Smoothing Paramete for Kernel Home-Range Analysis, Journal of Wildlife Management 70(3)
LSCV uses the the Warton or Tufto method to set the starting point but both should converge to the same LSCV solution, so the choice is immaterial.

Parameter 1:
Point_Layer
Layer name (if in ArcMap) or path to a feature class of points (typically animal locations).  If a layer is used in ArcMap, and features are selected in that layer, only the selected featues are used, otherwise all the features in the layer's definition query are used in the analysis.  If this is a feature class then all the features are used in the analysis.  The distribution of the points should be evaluated to determine the appropriateness of this tool and the correct selection of input parameters.

Parameter 2:
hRef_Method
This is the method for calculating hRef, the reference (or base) smoothing factor for each data subset. Selecting the correct smoothing factor is key to meaningful results. Large smoothing factors may over smooth the results, adding area to the UD, and small smoothing factors may result in too much detail, and insufficient area in the UD.
Worton: Sqrt( (variation_in_x + variation_in_y)/2) / n^(1/6)
Tufto: (Sqrt(variation_in_x + variation_in_y)/2) / n^(1/6)
Worton and Tufto make assume about the distribution, correlation and variation of the data that should be verified on your dataset before using these values.
References:
Tufto, J., Andersen, R. and Linnell, J. 1996. Habitat use and ecological correlates of home range size in a small cervid: the roe deer. J. Anim. Ecol. 65:715-724.
Worton, B.J. 1989. Kernel methods for estimating the utilization distribution in home-range studies. Ecology  70:164-168#

Parameter 3:
Modifier
The reference smoothing factor (hRef) can be adjusted in a number of ways to produce the final smoothing factor.
None: No adjustment is made, the smooth factor = hRef
Proportion: A percentage of hRef is used. various investigators have suggested different percentages based on the type and distribution of the data under consideration.
LSCV: A least squares cross validation is done to select the value between 0.05*hRef and 2.0*hRef that minimizes the LSCV score (Worton1995) between all pairs of points. This function is not guaranteed to work correctly if there are duplicate locations (a warning is issued and duplicate points are offset by a unit amount). In addition, there is no guarantee of a minimum in the range checked (a warning is issued and hRef is used). LSCV is very slow, and is limited to no more than 2000 points in a data subset.
BCV2: This is the same as LSCV, except a slightly different scoring function (Sain et. al. 1994) is used.
References:
Worton, B. J. 1995. Using Monte Carlo simulation to evaluate kernel-based home range estimators. Journal of Wildlife Management 59:794-800.
Sain, S. R., K. A Baggerly, and D. W. Scott. 1994. Cross-validation of multivariate densities. Journal of the American Statistical Association 89:807-817

Parameter 4:
Proportion
This parameter is required if and only if the hRef Modifier is "Proportion". This is a percentage of hRef to use for the final smoothing factor.  Typical values are between .5 (50%) and 1 (100%), although any positive number is acceptable.

Parameter 5:
Analysis_Projection
Calculations must be done in a projected coordinate system (i..e not geographic - lat/long).  The projected coordinate system to use can be specified in three ways, 1) with this parameter, 2) with the output coordinate system in the environment, or 3) with the coordinate system of the input.  These options are listed in priority order, that is this paraeter will trump the environment, and the environment will trump the input data. if a projected coordinate system is not found then the program will abort.

Scripting Syntax:
UDSmoothingFactor (Point_Layer, hRef_Method, Modifier, Proportion, Analysis_Projection)

Example1:
Scripting Example
The following example shows how this script can be used in the ArcGIS Python Window. It assumes that the script has been loaded into a toolbox, and the toolbox has been loaded into the active session of ArcGIS.
This calculates the least squares cross validation smoothing factor for the fixes in the shapefile
 fixes = r"C:\tmp\locations.shp"
 h = UDSmoothingFactor (fixes, "Worton", "LSCV", "")

Example2:
Command Line Example
The following example shows how the script can be used from the operating system command line. It assumes that the script and the data sources are in the current directory and that the python interpeter is the path.
This prints the least squares cross validation smoothing factor for the fixes in the FGDB
 C:\folder> python UD_SmoothingFactor.py test.gdb\fixes Worton LSCV

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

import math

import arcpy
import numpy

import utils


def DistancesSquared(points):
    small = 1e-3
    allSquaredDistances = []
    n = len(points)
    for i in range(n):
        #for j in range(i+1, n):  #unique set of distances dij = dji is faster; however produces different LSCV
        for j in range(n):
            if i == j:
                continue
            dx = points[j][0] - points[i][0]
            dy = points[j][1] - points[i][1]
            d2 = dx*dx + dy*dy
            if d2 < small:
                utils.warn("Distance from %d to %d is too small (%g).  Using %g" % (i,j,d2, small))
                d2 = small
            allSquaredDistances.append(d2)
    return allSquaredDistances

# Cross Validation function for h.
# approximates(?) the mean integrated square error (MISE) between the true density function and the kde
# we want the h that returns the minimum CV
def LSCV(allDistancesSquared, n, h):
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
            utils.warn("Warning duplicate locations found, results may be invalid.")
            utils.warn("        Separating the locations by 1 unit.")
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
        if d == 0.0:
            utils.warn("Warning duplicate locations found, results may be invalid.")
            utils.warn("        Separating the locations by 1 unit.")
            d = 1
        D = d/(h*h)
        D2 = D * D
        term3 = (D2 -8*D + 8) * math.exp(-D2/2) / term2
        total = total + term3
        #print "d",d,"D",D,"D2",D2,"term3",term3,"total", total
    result = term1 + total
    #print "BCV2", h, result
    return result

def Search(func, allSquaredDistances, n, h_ref, min_percent, max_percent, step_percent):

    h_min = min_percent * h_ref
    h_max = max_percent * h_ref
    h_step = step_percent * h_ref

    h_res = h_max
    minErr = func(allSquaredDistances, n, h_max)

    for h_test in utils.frange(h_min, h_max, h_step):
        err = func(allSquaredDistances, n, h_test)
        if (err < minErr):
            minErr = err
            h_res = h_test
    return h_res

def Minimize(func, h, points):
    n = len(points)
    if n > 2000:
        #raise ValueError("Too many points for Cross Validation, limit is 2000")
        msg = "Too many points for Cross Validation, limit is 2000, using 0.7 * hRef."
        utils.warn(msg)
        return 0.7*h

    allSquaredDistances = DistancesSquared(points)

    min_percent = 0.05
    max_percent = 2.00
    step_percent = 0.10
    h1 = Search(func, allSquaredDistances, n, h, min_percent, max_percent, step_percent)
    if h1 <= min_percent * h or h1 >= max_percent * h:
        # then it is the min or max value checked
        msg = "Cross Validation using "+func.__name__+" failed to minimize, using 0.7 * hRef."
        utils.warn(msg)
        return 0.7*h
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
    elif not hrefmethod or hrefmethod == "#":
        hRef = HrefWorton(points)

    if hRef == 0:
        utils.die("No valid hRef method was provided. Quitting.")

    if modifier.lower() == "proportion":
        h = proportionAmount * hRef
    elif modifier.lower() == "lscv":
        h = Minimize(LSCV, hRef, points)
    elif modifier.lower() == "bcv2":
        h = Minimize(BCV2, hRef, points)
    else:
        h = hRef

    utils.info("hRef (" + hRefmethod + ") = " + str(hRef))
    utils.info("Using h = " +  str(h))
    return h



if __name__ == "__main__":

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        utils.die("Unable to checkout the Spatial Analyst Extension.  Quitting.")

    locationLayer = arcpy.GetParameterAsText(0)
    hRefmethod = arcpy.GetParameterAsText(1)
    modifier = arcpy.GetParameterAsText(2)
    proportionAmount = arcpy.GetParameterAsText(3)
    spatialReference = arcpy.GetParameter(4)

    test = False
    if test:
        locationLayer = r"C:\tmp\test.gdb\fix_ll"
        hRefmethod = "WORTON" #Worton,Tufto,Silverman,Gaussian
        modifier = "NONE" #NONE,PROPORTIONAL,LSCV,BCV2
        proportionAmount = "0.7"
        spatialReference = arcpy.SpatialReference()
        spatialReference.loadFromString("PROJCS['NAD_1983_Alaska_Albers',GEOGCS['GCS_North_American_1983',DATUM['D_North_American_1983',SPHEROID['GRS_1980',6378137.0,298.257222101]],PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]],PROJECTION['Albers'],PARAMETER['False_Easting',0.0],PARAMETER['False_Northing',0.0],PARAMETER['Central_Meridian',-154.0],PARAMETER['Standard_Parallel_1',55.0],PARAMETER['Standard_Parallel_2',65.0],PARAMETER['Latitude_Of_Origin',50.0],UNIT['Meter',1.0]];-13752200 -8948200 10000;-100000 10000;-100000 10000;0.001;0.001;0.001;IsHighPrecision")
        #arcpy.env.outputCoordinateSystem = spatialReference
        arcpy.env.outputCoordinateSystem = None
        #spatialReference = None

    if modifier.lower() == "proportion":
        try:
            proportionAmount = float(proportionAmount)
        except ValueError:
            utils.die("Proportion Amount was not a valid number. Quitting.")

    if not locationLayer:
        utils.die("No location layer was provided. Quitting.")

    if not arcpy.Exists(locationLayer):
        utils.die("Location layer cannot be found. Quitting.")

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

    points = utils.GetPoints(locationLayer, spatialReference, shapeName)
    h = GetSmoothingFactor(points, hRefmethod, modifier, proportionAmount)
    arcpy.SetParameterAsText(4, str(h))
