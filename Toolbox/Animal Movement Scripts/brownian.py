# -*- coding: utf-8 -*-
"""
Brownian Bridge Functions

From Analyzing Animal Movements Using Brownian Bridges
Jon S. Horne Et Al, Ecology, Vol 88, No. 9
The code for adehabitat (http://cran.r-project.org/web/packages/adehabitat/index.html)
was also used to develop this code.

Python version written by Regan Sarwas, regan_sarwas@nps.gov,
National Park Service, Alaska Region GIS Team, 2011

This code is public domain
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import datetime
import math

import arcpy

import utils


def Normal(x, mu, v):
    """Probability function for x for a Normal (Gaussian) distribution.
    mu is the mean value of x, and v is the variance of x."""

    prefix = 1 / math.sqrt(2.0 * math.pi * v)
    distanceSquared = (x - mu) ** 2
    y = prefix * math.exp((-distanceSquared / (2.0 * v)))
    return y


def Horne2dNormal(x1, x2, mu1, mu2, v):
    """Probability function for (x1, x2) for a Bivariate Normal
    (Gaussian) distribution with no correlation between x1 and x2,
    and equal variance in both dimensions. (mu1, mu2) is the mean
    value of (x1, x2) and v is a scalar variance.
    For equation 7 in Horne, et al (2007), where Zi = [x1,x2],
    mui(ti) = [mu1,mu2], and sigmai^2(ti) = v."""

    # the total area under this 2D surface is 1.0.
    # the total area at +/- 1 sigma is 46.6076%
    # the total area at +/- 2 sigma is 91.1097%
    # the total area at +/- 3 sigma is 99.5354%

    # prefix = 1 / math.sqrt(2.0 * math.pi * v)
    # prefix = 1 / (2.0 * math.pi * v)
    # distanceSquared = ((x1 - mu1)**2 + (x2 - mu2)**2)
    # return prefix * math.exp( (-distanceSquared / (2.0 * v)))

    # I used timeit(), and this is the fastest version of the code above
    return (
        0.15915494309189535
        * math.exp(((x1 - mu1) ** 2 + (x2 - mu2) ** 2) / (-2.0 * v))
        / v
    )


def IntegratePath(
    gridX, gridY, startX, startY, startV, endX, endY, endV, T, Vm, intervals
):
    """
    Return the value of the probability density function at (gridX, gridY), by step-wise
    integration of the straight line path from (startX, startY) to (endX, endY). intervals is
    the number of intervals, or steps to consider along the path. Vm is the mobility variance
    T is the total time from start to end.  startV and endV are the locational variance
    (usually gps error) of the start and end points respectively.

    """

    # Create a list of median value for each interval
    values = []
    unitWidth = T / intervals
    stepSize = 1.0 / intervals
    # a = Horne's alpha (0 to 100% of the distance from start to end)
    a = 0 + stepSize / 2.0
    accumulator = 0
    while a < 1.0:
        x = startX + a * (endX - startX)
        y = startY + a * (endY - startY)
        v = (
            T * a * (1.0 - a) * Vm + a ** 2 * endV + (1.0 - a) ** 2 * startV
        )  # variance at (x,y)
        accumulator += Horne2dNormal(gridX, gridY, x, y, v)
        a += stepSize
    return accumulator * unitWidth

    # Add up the area under the curve
    # total = 0
    # for i in range(intervals):
    #    height = values[i]
    #    total += height * width
    # return total


def IntegratePathFast(
    gridX, gridY, startX, startY, startV, endX, endY, endV, T, Vm, intervals, a
):
    """
    Return the value of the probability density function at (gridX, gridY), by step-wise
    integration of the straight line path from (startX, startY) to (endX, endY). intervals is
    the number of intervals, or steps to consider along the path. Vm is the mobility variance
    T is the total time from start to end.  startV and endV are the locational variance
    (usually gps error) of the start and end points respectively.

    """

    # Create a list of median value for each interval
    values = []
    width = 1.0 / intervals
    for i in range(intervals):
        # a = Horne's alpha (0 to 100% of the distance from start to end)
        a = (i + 0.5) / intervals
        x = startX + a[i][0] * (endX - startX)
        y = startY + a[i][0] * (endY - startY)
        v = T * a[i][1] * Vm + a[i][2] * endV + a[i][3] * startV  # variance at (x,y)
        r = Horne2dNormal(
            gridX, gridY, x, y, v
        )  # value of the normal distribution for (x,y,v) at the grid point
        values.append(r)

    # Add up the area under the curve
    total = 0
    for i in range(intervals):
        height = values[i]
        total += height * width
    return total
    # return sum(values) / intervals  # about 2 seconds faster for 2172600 segments


def EvaluateGridPoint(gridX, gridY, fixes, intervals):
    """Return the probability of finding an animal at location (gridX, gridY) by
    adding up the probablity functions for all the paths between fixes.
    fixes is a list of fixes where each fix has the following elements:
    (time, x, y, locational_variance, mobility_variance).
    fixes are assumed to be in chronological order, and time is a number.  The units
    of time is unimportant, so long as the mobility variance is appropriate for the
    units chosen. vm is the animal's mobility variance, and intervals is the number
    of intervals along the path to evaluate along the path."""

    total = 0
    tTotal = fixes[len(fixes) - 1][0] - fixes[0][0]
    for i in range(len(fixes) - 1):
        i1 = i + 1
        thisTime = fixes[i][0]
        nextTime = fixes[i1][0]
        # location
        thisX = fixes[i][1]
        nextX = fixes[i1][1]
        thisY = fixes[i][2]
        nextY = fixes[i1][2]
        # locational Variance
        thisVl = fixes[i][3]
        nextVl = fixes[i1][3]
        # mobility Variance
        thisVm = fixes[i][4]
        nextVm = fixes[i1][4]

        T = nextTime - thisTime
        Vm = (thisVm + nextVm) / 2.0
        r = IntegratePath(
            gridX, gridY, thisX, thisY, thisVl, nextX, nextY, nextVl, T, Vm, intervals
        )
        # pathWeight = T/tTotal
        total += r  # * pathWeight
    return total / tTotal


def CreateBBGrid(
    xMin, xMax, yMin, yMax, cellSize, fixes, intervals, searchArea=None, progressor=None
):
    """Build a brownian bridge grid by dividing the extents into
    square cells of size cellSize.  The grid is a list of rows.  Each
    row is a list of cell value.  Each row is built left to right, and the
    rows are built top to bottom.  See the EvaluateGridPoint() for details
    on the remaining parameters."""

    if progressor is not None:
        rowCount = 1 + int((yMax - yMin) / cellSize)
        if progressor == True:
            rowIndex = 0
            print("Start Building Grid {0}".format(rowCount))
        else:
            progressor.SetProgressor("step", "Building Grid...", 0, rowCount, 1)

    if searchArea is not None:
        point = arcpy.Point()

    grid = []
    # build the grid from the top down
    for y in utils.frange(yMax, yMin, -cellSize):
        row = []
        for x in utils.frange(xMin, xMax, cellSize):
            if searchArea is None:
                cell = EvaluateGridPoint(x, y, fixes, intervals)
                # cell = 1
            else:
                point.X, point.Y = x, y
                if searchArea.contains(point):
                    cell = EvaluateGridPoint(x, y, fixes, intervals)
                    # cell = 1
                else:
                    cell = 0
            row.append(cell)
        grid.append(row)

        if progressor is not None:
            if progressor == True:
                rowIndex += 1
                print("Finished row {0} of {1}".format(rowIndex, rowCount))
            else:
                progressor.SetProgressorPosition()

    return grid


###
### Routines for estimating the mobility variance
###


def CVL(fixes, lowerBound, upperBound, step, scaleFactor):
    """Return a list (with step number of items).  Each item is a tuple of
    (variance, likelihood).  Variances in the list are equally distributed between
    lowerBound to upperBound. The likelihood is based on Equation 7 in Horne, et al
    and I beleive it is a likelihood cross validation.
    fixes is a list of fixes where each fix is (time, x, y, locational_variance).
    fixes are assumed to be in chronological order, and time is a number.  The units
    of time are unimportant, as the mobility variance varies with the units chosen.

    The algorithm is as follows: Assume a mobility variance, vm.  For the first three fixes,
    pretend that fix2 is missing, and calculate the normal distribution at the observed
    location of fix2 based on the predicted (part way between fix1 and fix3) mean location of
    fix2.  Do this again for fixes 2,3,4 then 3,4,5 etc until the last 3 fixes.  The product
    of the normal distributions is the indicator of the likelihood of the assumed mobility
    variance.

    In Horne, et al (2007), only odd numbered fixes are considered independent.
    in the adehabitat code, each group of three is considered.  This code uses
    the second approach."""

    # print("In CVL(); len(fixes) =", len(fixes), "lowerBound =",lowerBound, "upperBound =",upperBound, "step =", step, "scaleFactor =", scaleFactor)

    if len(fixes) < 3:
        raise ValueError("Not enough fixes provided")

    results = []
    for vm in utils.frange(lowerBound, (upperBound + step), step):
        # print("vm = ",vm)
        likelihood = 1
        for i in range(1, (len(fixes) - 1)):
            # times
            prevTime = fixes[i - 1][0]
            thisTime = fixes[i][0]
            nextTime = fixes[i + 1][0]
            # locations
            prevX = fixes[i - 1][1]
            thisX = fixes[i][1]
            nextX = fixes[i + 1][1]
            prevY = fixes[i - 1][2]
            thisY = fixes[i][2]
            nextY = fixes[i + 1][2]
            # locational Variance
            prevV = fixes[i - 1][3]
            nextV = fixes[i + 1][3]

            T = nextTime - prevTime
            # a = alphai = percent of path
            a = (thisTime - prevTime) / T

            # (meanX, meanY) = mui(ti) = mean predicted location at time i
            meanX = prevX + a * (nextX - prevX)
            meanY = prevY + a * (nextY - prevY)

            # v = sigmai^2(ti) = total assumed brownian motion variance at time i
            v = T * a * (1 - a) * vm + (1 - a) ** 2 * prevV + a ** 2 * nextV

            # distanceSquared = ((thisX - meanX)**2 + (thisY - meanY)**2)
            # print("thisX =",thisX,"thisY =",thisY,"meanX =",meanX,"meanY =",meanY, "v =", v)
            # print("distanceSquared =",distanceSquared, "T =", T, "a =", a, "vm =", vm, "prevV =", prevV, "nextV =", nextV,)

            # (thisX, thisY) = Zi = location at which to get value of PDF
            normal = Horne2dNormal(thisX, thisY, meanX, meanY, v)
            # print("normal =",normal)

            # The normal is typically very small (the total area under the normal curve = 1.0)
            # the scale factor keeps the product from decaying to zero with a
            # large number of fixes.  Since the likelihoods are relative, the
            # scale factor will not alter the relative likelihood of the Vm
            likelihood *= normal * scaleFactor
            # print("likelihood =",likelihood)
            if likelihood == 0:
                break

        results.append((vm, likelihood))
    return results


def BestV(fixes, minV, maxV, steps, scaleFactor):
    """Returns a boolean flag and a result.  The boolean flag indicates if
    the scale factor was appropriate and the result is valid.  If the flag is false,
    a negative result indicates to the caller that the scaleFactor was too small,
    otherwise the scale factor was too large.  If the flag is true, this method
    returns the mobility variance between minV and maxV with the highest likelihood.
    The scalefactor is used to keep the likelihood from decaying to zero.
    See the CVL() method for a description of likelihood.  steps is the number of
    values to try between minV and maxV.  The precision of the mobility variance
    is proportional to the number of steps.   The time to compute is proportional to
    the number of fixes times the number of steps."""

    # print("In BestV() len(fixes) =",len(fixes), "minV =",minV, "maxV =",maxV, "steps =",steps, "scaleFactor =",scaleFactor)

    stepSize = (maxV - minV) / steps
    likelihoods = CVL(fixes, minV, maxV, stepSize, scaleFactor)

    # if all the likelihoods are 0, then the scaleFactor is too small
    total = sum([v[1] for v in likelihoods])  # sum up the likelihoods
    # print("total =", total)
    if total == 0:
        return False, -1

    # if any of the likelihoods are infinity, then the scaleFactor is too large
    anyInfinities = filter(math.isinf, [v[1] for v in likelihoods])
    # print("anyInfinities =", anyInfinities)
    if anyInfinities:
        return False, 1

    # print("scale factor is good")
    result = (0, 0)
    for pair in likelihoods:
        # print("v", pair[0], "likelihood", pair[1])
        if pair[1] > result[1]:
            result = pair
    return True, result[0]


def MobilityVariance(fixes, maxGuess, scaleFactorGuess=None, steps=10, error=0.001):
    """Return the most likely mobility variance for the set of fixes.  See CVL() for
    a description of the mobility variance likelihood.
    This generate step results between zero and maxGuess.  If the best guess is near
    the maxGuess, then the true max may be about maxGuess, so we double the max, and
    keep repeating until we know we have a good range.  Then we begin zeroing in on
    the best result, generating step results at each turn, until the the error bars
    on our guess are within the error requested.

    Note: this solution is not guaranteed to work if there are multiple maximums, or
    if there is no upper bound on the variance.  The graph of likelihood at suitable
    resolution should be reviewed to correctly identify the true solution."""

    # print("In MobilityVariance(), len(fixes) =",len(fixes),"maxGuess =",maxGuess,"steps =", "scaleFactorGuess =", scaleFactorGuess, steps,"error =",error)

    ### FIXME FIXME FIXME FIXME FIXME FIXME FIXME FIXME ###
    ###
    ### potential problems:
    ###  a factor of ten being too coarse and scalefactor oscillates
    ###    i.e 1e8 has all zero, but 1e9 has infinity.
    ###  selected scale factor too close to the 'edge'
    ###    i.e. refining or increasing the maxV requires a rework of the scalefactor
    ###
    ### FIXME FIXME FIXME FIXME FIXME FIXME FIXME FIXME ###

    maxV = maxGuess
    # The first guess is provided by the caller input, or at the mid point.
    if scaleFactorGuess is None:
        scaleFactor = 1e10
    else:
        scaleFactor = scaleFactorGuess
    multiple = 10.0
    scale_factors = set()
    success = False
    last_result = 1.0
    msg = "success = {0}, result = {1}, scaleFactor = {2}"
    while not success:
        success, result = BestV(fixes, 0, maxV, steps, scaleFactor)
        print(msg.format(success, result, scaleFactor))
        if not success:
            if last_result != result:
                multiple *= .5 # decrease multiple by 50%
            if result < 0:
                scaleFactor = scaleFactor * multiple
            else:
                scaleFactor = scaleFactor / multiple
            last_result = result
            if scaleFactor < 1e-15 or scaleFactor in scale_factors:
                raise ValueError("MobilityVariance failed to converge.")
            else:
                scale_factors.add(scaleFactor)
    print("Found the scale factor!")

    # gap = (maxV - minV)/steps where minV = 0
    gap = maxV / steps

    #    print(result, "+/-", gap, "scaleFactor =", scaleFactor)
    #    sys.exit()

    # make sure the guess was big enough.  keep doubling until we get it
    # FIXME - we may keep doubling until we throw a math exception.  Check and bail.
    while result > maxV - gap:
        maxV = maxV * 2
        success, result = BestV(fixes, 0, maxV, steps, scaleFactor)
        if not success:
            raise ValueError
            ###FIXME, check success and adjust scalefactor
        gap = maxV / steps
        # print(result, "+/-", gap)

    # keep zeroing in on the previous result until we have good resolution
    while gap > error:
        success, result = BestV(
            fixes, max(0, result - gap), result + gap, steps, scaleFactor
        )
        if not success:
            raise ValueError
            ###FIXME, check success and adjust scalefactor
        gap = 2 * gap / steps
        # print(result, "+/-", gap)
    return result
