# -*- coding: utf-8 -*-
"""
Utility functions for use with ArcGIS < 10.1
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import sys

import arcpy


def die(msg):
        arcpy.AddError(msg)
        print("ERROR: " + str(msg))
        sys.exit()

def warn(msg):
        arcpy.AddWarning(msg)
        print("Warning: " + str(msg))

def info(msg):
        arcpy.AddMessage(msg)
        print("Info: " + str(msg))

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

def frange(x, y, jump):
    """Return a range of numbers from x to y by jump increments.
    It is intended to be a floating point version of range()."""

    if jump == 0:
        raise ValueError, "jump must be non-zero"
    if jump > 0:
        while x < y:
            yield x
            x += jump
    else:
        while x > y:
            yield x
            x += jump

def GetPoints(pointsFeature, sr = None, shapeName = None):
    """returns a python list of (x,y) pairs"""
    points = []
    if not shapeName:
        shapeName = arcpy.Describe(pointsFeature).shapeFieldName
    for row in arcpy.SearchCursor(pointsFeature,"",sr):
        shape = row.getValue(shapeName)
        points.append( (shape.getPart().X, shape.getPart().Y) )
    return points

def GetArcpyPoints(pointsFeature, sr = None, shapeName = None):
    """returns a python list of arcpy.Points()"""
    points = []
    if not shapeName:
        shapeName = arcpy.Describe(pointsFeature).shapeFieldName
    for row in arcpy.SearchCursor(pointsFeature, "", sr):
        shape = row.getValue(shapeName)
        points.append(shape.getPart())
    return points

