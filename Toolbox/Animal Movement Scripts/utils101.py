# ------------------------------------------------------------------------------
# Utils101.py
# Created: 2013-10-24
#
# Utility functions for use with ArcGIS 10.1
# ------------------------------------------------------------------------------

import sys
import arcpy
import arcpy.da

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

def GetPoints(pointsFeature, sr = None):
    """returns a python list of (x,y) pairs"""
    with arcpy.da.SearchCursor(
             pointsFeature, 'SHAPE@XY', spatial_reference=sr
        ) as searchCursor:
        points = [row[0] for row in searchCursor]
    return points
