# ------------------------------------------------------------------------------
# UD_SmoothingFactor.py
# Created: 2012-12-05
#
# Utility functions for use with ArcGIS
# ------------------------------------------------------------------------------

import sys
import arcpy

def die(msg):
        arcpy.AddError(msg)
        print("ERROR: " + msg)
        sys.exit()

def warn(msg):
        arcpy.AddWarning(msg)
        print("Warning: " + msg)

def info(msg):
        arcpy.AddMessage(msg)
        print("Info: " + msg)

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


