# -*- coding: utf-8 -*-
"""
Utility functions for use with ArcGIS 10.1
Created: 2013-10-24
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


def is_float(something):
    try:
        float(something)
    except (ValueError, TypeError):
        return False
    return True


def is_int(something):
    try:
        int(something)
    except (ValueError, TypeError):
        return False
    return True


def float_range(start, stop, step):
    """Return a range of numbers from x to y by jump increments.
    It is intended to be a floating point version of range()."""

    if step == 0:
        raise ValueError("jump must be non-zero")
    if step > 0:
        while start < stop:
            yield start
            start += step
    else:
        while start > stop:
            yield start
            start += step


def get_points(point_feature, spatial_reference=None):
    """returns a python list of (x,y) pairs"""
    with arcpy.da.SearchCursor(
        point_feature, "SHAPE@XY", spatial_reference=spatial_reference
    ) as cursor:
        points = [row[0] for row in cursor]
    return points
