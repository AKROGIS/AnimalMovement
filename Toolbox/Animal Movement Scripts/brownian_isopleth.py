# -*- coding: utf-8 -*-
"""
brownian_isopleths.py
Created: 2012-12-04
Edited: 2021-02-04

Title:
Brownian Bridge Isopleths

Tags:
contour, home, range, animal, tracking, telemetry, ecology, brownian, bridge

Summary:
Creates polylines and/or polygons based on the requested list of isopleths
Input is a Brownain Bridge probability raster.

Usage:
The input raster will classified into 100 equal interval bins and inverted.
I.e. the smallest cell values will go to 100 and the highest cell values will
go to 1, with the remaining values scaled linearly.

The isopleth values must be in the range 1 to 99 representing the probability
(as a percent) the animal will be found within the area of that isopleth.
The larger then number, the greater the area encompassed.

At least one of the output feature classes (lines, polygons, or donuts) must be
requested.

Parameter 1:
Isopleths
This is a list of isopleth values separated by commas, colons, semicolons, or
whitespace. The values provided should be in the range [0...100] where the
larger then number, the greater the area encompassed.  Values outside the
range are silently ignored.

Parameter 2:
Raster_Layer
The input raster. This should be a probability distribution raster (as created
by the BB Raster tool). Probability distribution raster have values in the range
from 0 to 1 indicating the probability of finding the animal in this cell
compared with the other cells. The total of all cells in the raster must equal
one.

Parameter 3:
Lines (optional)
The name of a new output polyline feature class. One of `Lines`, `Polygons`, or
`Donut_Polygons` must be provided. If this parameter is left blank, no lines will
be created. The output feature class will have a field named 'contour' with the
value of the isopleth, and one or more features for each isopleth requested that
exists in the input raster. There may be multiple polylines for each isopleth.
Polylines may not close, but they should if the input is a UD raster from the
UD Raster tool.

No smoothing is done, and depending on the cell size the output can be very
dense (small cell size), or very blocky (large cell size).

Parameter 4:
Polygons (optional)
Name of the new output polygon feature class. One of `Lines`, `Polygons`, or
`Donut_Polygons` must be provided.

Contains a polygon for each isopleth. Each polygon contains the entire are
covered by the isopleth. These polygons are overlapping. The polygons are
written to the featureclass with the largest isopleth values first. (for UD
analysis, this provides a correctly stacked results set). These polygons are
created from the isopleth lines.

Parameter 5:
Donut_Polygons (optional)
The name of a new output polygon feature class. One of `Lines`, `Polygons`, or
`Donut_Polygons` must be provided.

Contains a polygon for each isopleth range. Assumes the isopleths are ordered
with the largest values containing the most area (so the last range is a donut
without a hole). There is no donut for the first range range (i.e. from the
universe to the first isopleth). These polygons are created from the lines.

Scripting Syntax:
brownian_isopleths_AnimalMovement(Isopleths, Raster_Layer, Lines, Polygons, Donut_Polygons)

Example1:
Scripting Example
The following example shows how this script can be used in the ArcGIS Python
Window. It assumes that the script has been loaded into a toolbox, and the
toolbox has been loaded into the active session of ArcGIS.

It creates the 65%, 90% polygons (with holes) in a file geodatabase
  raster = r"C:/tmp/bb.tif"
  donuts = r"C:/tmp/test.gdb/bb_donuts"
  brownian_isopleths("65;90", raster, "", "", donuts)

Example2:
Command Line Example
The following example shows how the script can be used from the operating system
command line. It assumes that the script and the data sources are in the current
directory and that the python interpeter is the path.

It creates the 50%, 90% and 95% polygons in a file geodatabase
  C:/folder> python brownian_isopleths.py "50,90,95" bb.tif # test.gdb/bb_poly #

Credits:
Regan Sarwas, Alaska Region GIS Team, National Park Service

Limitations:
Public Domain

Requirements
arcpy module - requires ArcGIS v10+ and a valid license
arcpy.sa module - requires a valid license to Spatial Analyst Extension
utilization_isopleth.py module - See this module for additional requirements

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

import arcpy

import utils
import utilization_isopleth


def isopleth(iso_input, raster, lines_name=None, poly_name=None, donut_name=None):
    """
    Creates isopleth (contour line) feature classes for the raster_name.

    iso_input is either a list of list of floats, or a string of comma separated
    numbers that will be converted to a list of floats

    raster is either an arcpy.raster object, or a data source path (string) or
    a arcpy.layer object.

    The others are ArcGIS data source paths (strings).

    It is assumed that the input has already been validated.

    No return value.
    """

    if not iso_input.instance_of(list):
        iso_input = utilization_isopleth.GetIsoplethList(iso_input)

    if not raster.instance_of(arcpy.Raster):
        raster = arcpy.sa.Raster(raster)

    # Normalize the raster
    # classify the results into 100 equal interval bins, and invert (0..100 -> 100..0)
    norm_raster = 101 - arcpy.sa.Slice(raster, 100, "EQUAL_INTERVAL")
    utilization_isopleth.CreateIsopleths(
        iso_input, norm_raster, lines_name, poly_name, donut_name
    )


def validate(iso_input, raster_name, lines_name=None, poly_name=None, donut_name=None):
    """
    Validate the input.

    Exit with a message if the input is not valid.  Otherwise return a tuple
    containing a list of floats, and an arcpy.Raster object.
    """

    if arcpy.CheckOutExtension("Spatial") != "CheckedOut":
        utils.die("Unable to checkout the Spatial Analyst Extension. Quitting.")

    if not raster_name:
        utils.die("No Brownian Bridge Raster was provided. Quitting.")

    if not arcpy.Exists(raster_name):
        utils.die("Brownian Bridge Raster cannot be found. Quitting.")

    try:
        raster = arcpy.sa.Raster(raster_name)
    except arcpy.ExecuteError as ex:
        utils.die(
            "Brownian Bridge Raster cannot be loaded.\n{0}\n Quitting.".format(ex)
        )
    if not (lines_name or poly_name or donut_name):
        utils.die("No output requested. Quitting.")

    try:
        iso_list = utilization_isopleth.GetIsoplethList(iso_input)
    except AttributeError:
        utils.die("Unable to interpret the list of isopleths.")

    if not iso_list:
        utils.die("List of valid isopleths is empty. Quitting.")

    return (iso_list, raster)


def test():
    """Set up manual test parameters and process."""

    iso_input = "50,65,90,95"
    raster_name = r"C:\tmp\test.gdb\bb9"
    iso_lines = r"C:\tmp\test.gdb\lines1"
    iso_polys = r"C:\tmp\test.gdb\polys1"
    iso_donuts = r"C:\tmp\test.gdb\donut1"
    iso_list, raster = validate(
        iso_input, raster_name, iso_lines, iso_polys, iso_donuts
    )
    isopleth(iso_list, raster, iso_lines, iso_polys, iso_donuts)


def main():
    """Get the input from arcpy (tbx or command line) and process."""

    iso_input = arcpy.GetParameterAsText(0)
    raster_name = arcpy.GetParameterAsText(1)
    iso_lines = arcpy.GetParameterAsText(2)
    iso_polys = arcpy.GetParameterAsText(3)
    iso_donuts = arcpy.GetParameterAsText(4)
    iso_list, raster = validate(
        iso_input, raster_name, iso_lines, iso_polys, iso_donuts
    )
    isopleth(iso_list, raster, iso_lines, iso_polys, iso_donuts)


if __name__ == "__main__":
    # test()
    main()
