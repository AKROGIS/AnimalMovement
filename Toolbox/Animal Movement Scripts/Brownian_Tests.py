# -*- coding: utf-8 -*-
"""
Brownian Bridge Tests.
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import datetime

import arcpy
import numpy

import Brownian


def CreateBBRaster(extents, cellSize, fixes, intervals):
    """Returns an arcpy.raster by building a brownian bridge grid,
    and converting that to a numpy.array then to a arcpy.raster.
    See CreateBBGrid() for details on building the grid."""

    grid = Brownian.CreateBBGrid(extents.XMin, extents.XMax, extents.YMin, extents.YMax, cellSize, fixes, intervals)
    array = numpy.array(grid)
    lowerLeft = arcpy.Point(extents.XMin, extents.YMin)
    raster = arcpy.NumPyArrayToRaster(array, lowerLeft, cellSize)
    return raster


def test1():
    minx,maxx = -87.0, 367.0 #Extents of data +/- 3sigma_l
    miny,maxy = -163.0, 163.0 #Extents of data +/- 3sigma_l +/- 3sigma_m
    cellSize = 1
    extents = arcpy.Extent(minx, miny, maxx, maxy)
    intervals = 25
    vm = 642  #sigma = 25.33m; 3sigma = 76.0m
    vl = 28.85**2 #            3sigma = 86.6m
    fix = [
        ( 0.0,  0.0, 0.0, vl, vm),
        ( 20.0, 280.0, 0.0, vl, vm),
    ]

    #grid = CreateBBGrid(extents, cellSize, fix, vm, intervals)
    #print(grid)
    nx = int((maxx-minx)/cellSize)
    ny = int((maxy-miny)/cellSize)
    nf = len(fix)
    ni = intervals
    n = nx*ny*nf*ni
    print("Estimated running time",0.000002*n,"seconds")
    start = datetime.datetime.now()
    raster = CreateBBRaster(extents, cellSize, fix, intervals)
    time = datetime.datetime.now()-start
    print("time",time, n, time/n)
    raster.save(r"C:\tmp\kd_test\bb1sq.tif")

def test2():
    minx,maxx = 1190.0, 1300.0
    miny,maxy = 3430.0, 3500.0
    cellSize = 1
    extents = arcpy.Extent(minx, miny, maxx, maxy)
    vm = 6.05
    intervals = 20
    fix = [
        ( 0.0,  1236.4, 3456.3, 4, vm),
        ( 20.0, 1286.4, 3456.3, 5, vm),
        ( 60.0, 1256.4, 3486.3, 3, vm),
        ( 70.0, 1236.4, 3476.3, 4, vm),
        ( 100.0, 1206.4, 3446.3, 5, vm),
    ]

    #grid = CreateBBGrid(extents, cellSize, fix, vm, intervals)
    #print(grid)
    nx = int((maxx-minx)/cellSize)
    ny = int((maxy-miny)/cellSize)
    nf = len(fix)
    ni = intervals
    n = nx*ny*nf*ni
    print("Estimated running time",0.000002*n,"seconds")
    start = datetime.datetime.now()
    raster = CreateBBRaster(extents, cellSize, fix, intervals)
    time = datetime.datetime.now()-start
    print("time",time, n, time/n)
    raster.save(r"C:\tmp\kd_test\bb1b.tif")

if __name__ == "__main__":
    test1()
