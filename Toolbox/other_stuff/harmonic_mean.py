# -*- coding: utf-8 -*-
"""
Calculate the Harmonic mean of all distances between n points.

With n points there are approx n²/2 distances, so this can become a
very time consuming problem for large numbers of points.

CAUTION this is an O(n²) algorithm.  Avoid large datasets. On my 3 GHz Xeon,
it takes about 0.6e-6*n² seconds, where n is the number of points in your dataset.
1000 points ~ .6 seconds, 10,000 points ~ 1 minute, 100,000 points ~ 1hour 40minutes.
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import math
import random
import datetime

import arcpy


# pylint: disable=invalid-name, missing-function-docstring
# TODO: Fix names and add doc strings

# Python 2/3 compatible xrange() cabability
# pylint: disable=undefined-variable,redefined-builtin
if sys.version_info[0] < 3:
    range = xrange


def GetPoints(pointsFeature, shapeName, idName):
    # return a tuple (x,y,m) for each oid, m will be calculated later
    data = {}

    if not shapeName:
        shapeName = arcpy.Describe(pointsFeature).shapeFieldName
    if not idName:
        idName = arcpy.Describe(pointsFeature).OIDFieldName

    fields = [idName, "SHAPE@XY"]
    with arcpy.da.SearchCursor(pointsFeature, fields) as cursor:
        for row in cursor:
            oid = row[0]
            x, y = row[1]
            data[oid] = (x, y, 0.0)
    return data


def HarmonicMeanDistance(points):
    # Low memory solution; takes twice as long, because distance ab and ba are both calculated
    n = len(points)
    msg = (
        "Object IDs {0} and {1} are at the same location ({2},{3}). "
        "Using distance of 1."
    )
    for k1 in points:
        x1, y1, _ = points[k1]
        # print(k1,x1,y1,m1)
        z = 0
        for k2 in points:
            if k1 == k2:
                continue
            x2, y2, _ = points[k2]
            x = x2 - x1
            y = y2 - y1
            d = math.sqrt(x * x + y * y)
            # print(i,j,d)
            if d == 0:
                print(msg.format(k1, k2, x1, x2))
                d = 1
            z = z + 1.0 / d
        hm = (n - 1.0) / z  # with n points there are n-1 distances (skip i = j)
        # print(z, hm)
        points[k1] = (x1, y1, hm)


def HarmonicMeanDistance_a(xs, ys):
    # Low memory solution; takes twice as long, because distance ab and ba are both calculated
    n = len(xs)
    h = []
    msg = "distance is zero at {0} = ({2}, {3}) and {1} = ({4}, {5})"
    for i in range(n):
        z = 0
        for j in range(n):
            if i == j:
                continue
            x = xs[j] - xs[i]
            y = ys[j] - ys[i]
            d = math.sqrt(x * x + y * y)
            print(i, j, d)
            if d == 0:
                print(msg.format(i, j, xs[i], ys[i], xs[j], ys[j]))
                d = 1
            z = z + 1.0 / d
        hm = (n - 1.0) / z  # with n points there are n-1 distances (skip i = j)
        print(z, hm)
        h.append(hm)
    return h


def HarmonicMeanDistance2(xs, ys):
    # Low memory solution; takes twice as long, because distance ab and ba are both calculated
    n = len(xs)
    h = []
    msg = "distance is zero at {0} = ({2}, {3}) and {1} = ({4}, {5})"
    for i in range(n):
        z = 0
        for j in range(n):
            if i == j:
                continue
            x = xs[j] - xs[i]
            y = ys[j] - ys[i]
            d = x * x + y * y
            # print(i,j,d)
            if d == 0:
                print(msg.format(i, j, xs[i], ys[i], xs[j], ys[j]))
                d = 1
            z = z + 1.0 / d
        hm = (n - 1.0) / z  # with n points there are n-1 distances (skip i = j)
        # print(z, hm)
        h.append(hm)
    return h


def MeanDistance(xs, ys):
    # Low memory solution; takes twice as long, because distance ab and ba are both calculated
    n = len(xs)
    h = []
    msg = "distance is zero at {0} = ({2}, {3}) and {1} = ({4}, {5})"
    for i in range(n):
        z = 0
        for j in range(n):
            if i == j:
                continue
            x = xs[j] - xs[i]
            y = ys[j] - ys[i]
            d = math.sqrt(x * x + y * y)
            # print(i,j,d)
            if d == 0:
                print(msg.format(i, j, xs[i], ys[i], xs[j], ys[j]))
                d = 1
            z = z + d
        m = z / (n - 1.0)  # with n points there are n-1 distances (skip i = j)
        # print(z, m)
        h.append(m)
    return h


def MeanDistance2(xs, ys):
    # Low memory solution; takes twice as long, because distance ab and ba are both calculated
    n = len(xs)
    h = []
    msg = "distance is zero at {0} = ({2}, {3}) and {1} = ({4}, {5})"
    for i in range(n):
        z = 0
        for j in range(n):
            if i == j:
                continue
            x = xs[j] - xs[i]
            y = ys[j] - ys[i]
            d = x * x + y * y
            # print(i,j,d)
            if d == 0:
                print(msg.format(i, j, xs[i], ys[i], xs[j], ys[j]))
                d = 1
            z = z + d
        m = z / (n - 1.0)  # with n points there are n-1 distances (skip i = j)
        # print(z, m)
        h.append(m)
    return h


def test(n):
    data = {}
    for i in range(n):
        x = random.uniform(0, 1)
        y = random.uniform(0, 1)
        data[i] = (x, y, 0)
    # print(data)
    start = datetime.datetime.now()
    HarmonicMeanDistance(data)
    # hm = HarmonicMeanDistance(x,y)
    # hm2 = HarmonicMeanDistance2(x,y)
    # md = MeanDistance(x,y)
    # md2 = MeanDistance2(x,y)
    diff = datetime.datetime.now() - start
    ms = diff.microseconds
    msg = "Harmonic Mean (n={1}) took {0}ms ({2}ms/point or {3}µs/point²)"
    print(msg.format(ms, n, (ms * 1.0) / n, (1000.0 * ms) / n / n))

    for i, item in enumerate(data):
        h = item[2]
        if i == 0:
            hmin = h
            hmax = h
            min_index = i
            max_index = i
        else:
            if h < hmin:
                hmin = h
                min_index = i
            elif hmax < h:
                hmax = h
                max_index = i

    # print("Min Index",min_index, "Max Index",max_index)
    # print("Average (x,y)", numpy.average(x), numpy.average(y))
    print("Harmonic Mean {0} {1}".format(min_index, data[min_index]))
    print("Furthest point {0} {1}".format(max_index, data[max_index]))
    if n < 21:
        print(data)
        # print([int(e*1000) for e in x])
        # print([int(e*1000) for e in y])
        # print([int(e*1000) for e in hm])
        # print([int(e*1000) for e in hm2])
        # print([int(e*1000) for e in md])
        # print([int(e*1000) for e in md2])


def FieldExists(fc, fieldName, fieldType):
    for f in arcpy.ListFields(fc):
        if f.name == fieldName:
            if f.type == fieldType:
                return True
            raise TypeError(
                "FieldName '{0}' exists, but is of the wrong type".format(fieldName)
            )
    return False


def test2():
    feature = r"C:\tmp\test2.gdb\winter2011"
    # feature = r"C:\tmp\test2.gdb\w2011a0901"
    fieldName = "H_Mean"
    description = arcpy.Describe(feature)
    shapeName = description.shapeFieldName
    idName = description.OIDFieldName
    data = GetPoints(feature, shapeName, idName)
    HarmonicMeanDistance(data)

    if not FieldExists(feature, fieldName, "Double"):
        arcpy.AddField_management(
            feature, fieldName, "Double", "", "", "", "", "NULLABLE", "NON_REQUIRED", ""
        )

    fields = [idName, fieldName]
    with arcpy.da.UpdateCursor(feature, fields) as cursor:
        for row in cursor:
            oid = row[0]
            row[1] = data[oid][2]
            cursor.updateRow(row)


test2()
