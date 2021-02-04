# -*- coding: utf-8 -*-
"""
Utility functions for use with ArcGIS 10.1
Created: 2013-10-24
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import sys

import arcpy


def die(msg):
    """Print error message to console and ArcGIS and exit."""
    arcpy.AddError(msg)
    print("ERROR: {0}".format(msg))
    sys.exit()


def warn(msg):
    """Print warning message to console and ArcGIS and exit."""
    arcpy.AddWarning(msg)
    print("Warning: {0}".format(msg))


def info(msg):
    """Print info message to console and ArcGIS and exit."""
    arcpy.AddMessage(msg)
    print("Info: {0}".format(msg))


def is_float(something):
    """Return True is something is convertible to a floating point number."""
    try:
        float(something)
    except (ValueError, TypeError):
        return False
    return True


def is_int(something):
    """Return True is something is convertible to a integer number."""
    try:
        int(something)
    except (ValueError, TypeError):
        return False
    return True


def float_range(start, stop, step=1.0):
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
