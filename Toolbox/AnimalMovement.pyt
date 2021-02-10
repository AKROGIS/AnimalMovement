# -*- coding: utf-8 -*-
"""
ArcGIS Toolbox/Tool Classes for the Animal Movement Tools.

Compatible with Python 2.7 and 3.5+ (ArcGIS 10.2+ and Pro 2.0+)

The toolbox class must meet specific requirements expected by the ArcGIS host.
See https://pro.arcgis.com/en/pro-app/latest/arcpy/geoprocessing_and_python/
    a-template-for-python-toolboxes.htm
for details.
The tool class must meet specific requirements expected by the ArcGIS host.
See https://pro.arcgis.com/en/pro-app/latest/arcpy/geoprocessing_and_python/
    defining-a-tool-in-a-python-toolbox.htm
for details.
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import arcpy

import animalmovement

# For development only; See
# https://pro.arcgis.com/en/pro-app/latest/arcpy/geoprocessing_and_python/editing-a-python-toolbox.htm
# import imp
# imp.reload(animalmovement)

# pylint: disable = invalid-name,no-self-use,unused-argument,too-few-public-methods
# Framework requirement of the class violate pylint standards.

# pylint: disable = useless-object-inheritance
# required for Python 2/3 compatibility


class Toolbox(object):
    """
    Define the toolbox.

    The name of the toolbox is the name of the .pyt file.
    The label is the display name as shown in the Geoprocessing pane.
    The alias is used as a command suffix for scripting in the python window.
    The description is shown in the Geoprocessing pane.
    """

    def __init__(self):
        self.label = "Animal Movements Toolbox"
        self.alias = "animalmovement"
        self.description = (
            "A collection of GIS tools for analyzing animal movement data."
        )

        # List of tool classes associated with this toolbox
        # They will be shown in categories in sort order (not the order here)
        self.tools = [
            AnimalTracks,
            BrownianBridgeIsopleths,
            BrownianBridgeRaster,
            KernelDensityIsopleths,
            KernelDensityMultiple,
            KernelDensityRaster,
            KernelDensitySingle,
            KernelDensitySmoothingFactor,
            MinimumConvexPolygon,
        ]


class AnimalTracks(object):
    """A tool to create movement vectors from sequential animal locations."""

    def __init__(self):
        self.label = "Animal Tracks"
        self.description = "Create movement vectors from sequential animal locations."
        self.canRunInBackground = True

    # - Telemetry Data - Feature Layer
    # - Output Name - Feature Class
    # - Animal Id  - Field
    # - Date - Field
    # - Output start field name - string (o)
    # - Output end field name - string (o)
    # - Output duration field name - string (o)
    # - Output speed field name - string (o)
    # - Output distance field name - string (o)
    # - Spatial Reference


class BrownianBridgeIsopleths(object):
    """A tool to create probability contours from sequential animal locations
    assuming brownian motion."""

    def __init__(self):
        self.label = "Brownian Bridge Isopleths"
        self.description = (
            "Create probability contours from sequential animal locations "
            "assuming brownian motion."
        )
        self.category = "Brownian Bridge Home Range"
        self.canRunInBackground = True


class BrownianBridgeRaster(object):
    """A tool to create a probability raster from sequential animal locations
    assuming brownian motion."""

    def __init__(self):
        self.label = "Brownian Bridge Raster"
        self.description = (
            "Create a probability raster from sequential animal locations "
            "assuming brownian motion."
        )
        self.category = "Brownian Bridge Home Range"
        self.canRunInBackground = True


class KernelDensityIsopleths(object):
    """A tool to create utilization contours from a kernel density raster."""

    def __init__(self):
        self.label = "Kernel Density Isopleths"
        self.description = "Create utilization contours from a kernel density raster."
        self.category = "Kernel Density Home Range"
        self.canRunInBackground = True


class KernelDensityMultiple(object):
    """A tool to create utilization contours from the locations of multiple animals."""

    def __init__(self):
        self.label = "Kernel Density (Multiple)"
        self.description = (
            "Create utilization contours from the locations of multiple animals."
        )
        self.category = "Kernel Density Home Range"
        self.canRunInBackground = True


class KernelDensityRaster(object):
    """A tool to create a utilization raster from random animal locations
    and a smoothing factor."""

    def __init__(self):
        self.label = "Kernel Density Raster"
        self.description = "Create a utilization raster from random animal locations and a smoothing factor."
        self.category = "Kernel Density Home Range"
        self.canRunInBackground = True


class KernelDensitySingle(object):
    """A tool to create utilization contours from an animal's random locations."""

    def __init__(self):
        self.label = "Kernel Density (Single)"
        self.description = (
            "Create utilization contours from an animal's random locations."
        )
        self.category = "Kernel Density Home Range"
        self.canRunInBackground = True


class KernelDensitySmoothingFactor(object):
    """A tool to calculate a suitable smoothing factor from random animal
    locations for kernel density analysis."""

    def __init__(self):
        self.label = "Kernel Density Smoothing Factor"
        self.description = (
            "Calculate a suitable smoothing factor from random animal "
            "locations for kernel density analysis."
        )
        self.category = "Kernel Density Home Range"
        self.canRunInBackground = True


class MinimumConvexPolygon(object):
    """A tool to create a minimum convex polygon containing animal locations."""

    def __init__(self):
        self.label = "Minimum Convex Polygon"
        self.description = (
            "Create a minimum convex polygon containing animal locations."
        )
        self.canRunInBackground = True

    def getParameterInfo(self):
        """Define parameter definitions"""
        in_features = arcpy.Parameter(
            name="in_features",
            displayName="Location Points",
            direction="Input",
            datatype="GPFeatureLayer",
            parameterType="Required",
        )
        in_features.filter.list = ["Points"]

        out_features = arcpy.Parameter(
            name="out_features",
            displayName="Polygon Feature Class",
            direction="Output",
            datatype="DEFeatureClass",
            parameterType="Required",
        )

        percent = arcpy.Parameter(
            name="keep_percent",
            displayName="Percent of points to keep",
            direction="Input",
            datatype="GPDouble",
            parameterType="Required",
        )
        percent.value = 100.0
        percent.filter.type = "Range"
        percent.filter.list = [1.0, 100.0]

        method = arcpy.Parameter(
            name="removal_method",
            displayName="Removal Method",
            direction="Input",
            datatype="GPString",
            parameterType="Optional",
            enabled=False,
        )
        method.filter.list = animalmovement.removal_methods
        method.value = method.filter.list[0]

        point = arcpy.Parameter(
            name="point",
            displayName="User Point",
            direction="Input",
            datatype="GPPoint",
            parameterType="Optional",
        )

        spatial_reference = arcpy.Parameter(
            name="spatial_reference",
            displayName="Spatial Reference",
            direction="Input",
            datatype="GPSpatialReference",
            parameterType="Optional",
        )

        return [in_features, out_features, percent, method, point, spatial_reference]

    def isLicensed(self):
        """Set whether tool is licensed to execute."""
        return True

    def updateParameters(self, parameters):
        """
        Modify the values and properties of parameters before internal
        validation is performed.

        This method is called whenever a parameter has been changed.
        """
        # method is only enabled if the percent is less than 100%
        parameters[3].enabled = parameters[2] < 100.0
        # A spatial reference required if the input feature class is not projected
        # TODO: Check for projected.
        if parameters[0].altered:
            parameters[4].parameterType = "Required"
        else:
            parameters[4].parameterType = "Optional"
        # A point is required if the method is User Point.
        if parameters[3].enabled and parameters[3].value == "User Point":
            parameters[5].parameterType = "Required"
        else:
            parameters[5].parameterType = "Optional"

    def updateMessages(self, parameters):
        """
        Modify the messages created by internal validation for each tool
        parameter.

        This method is called after internal validation.
        """
        # TODO: if the spatial reference is not projected then add error
        return

    def execute(self, parameters, messages):
        """Get the parameters and execute the task of the tool."""
        location_points_layer = parameters[0].value
        output_feature_class = parameters[1].valueAsText
        percent_points = parameters[2].value
        removal_method = parameters[3].valueAsText
        user_point = parameters[4].value
        projection = parameters[5].value
        animalmovement.create_mcp(
            location_points_layer,
            output_feature_class,
            percent_points,
            removal_method,
            user_point,
            projection,
        )
