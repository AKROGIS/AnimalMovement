# -*- coding: utf-8 -*-
"""
Functions and tests for numerical (piecewise) integration
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import math

import brownian

# pylint: disable=invalid-name, missing-function-docstring
# TODO: Fix names and add doc strings


def Integrate1D(f, x1, x2, n):
    # integrate f(x) from x1 to x2 using piecewise integration with n pieces
    # Uses the Newton-Cotes rectangle (or midpoint) rule

    stepSize = (x2 - x1) / n
    x = x1 + stepSize / 2
    accumulator = 0.0
    while x < x2:
        # area = f(x) * stepSize
        # accumulator += area
        accumulator += f(x)
        x += stepSize
    # return accumulator
    return accumulator * stepSize


# pylint: disable=too-many-arguments
def Integrate2D(f, x1, x2, y1, y2, n):
    # integrate f(x, y) from x1 to x2 and y1 to y2 using piecewise integration
    # with n pieces.
    # Uses the Newton-Cotes rectangle (or midpoint) rule

    xStepSize = (x2 - x1) / n
    yStepSize = (y2 - y1) / n
    x = x1 + xStepSize / 2
    y0 = y1 + yStepSize / 2
    unitArea = xStepSize * yStepSize
    accumulator = 0.0
    while x < x2:
        y = y0
        while y < y2:
            # area = f(x,y) * unitArea
            # accumulator += area
            accumulator += f(x, y)
            y += yStepSize
        x += xStepSize
    # return accumulator
    return accumulator * unitArea


def func(x):
    # return 2*x
    return brownian.Normal(x, 0, 1)


def func2(x, y):
    # return 2*x*y
    return brownian.Horne2dNormal(x, y, 0, 0, 1)


def func2p(r, a):
    # polar function, r = radius, a = angle in radians
    x = r * math.cos(a)
    y = r * math.sin(a)
    return brownian.Horne2dNormal(x, y, 0, 0, 1)


if __name__ == "__main__":
    test = 4
    if test == 1:
        print(brownian.Normal(2, 0, 1))
        print(brownian.Normal2D(0, 2, 0, 0, 1))
        print(brownian.HorneNormal2D(0, 2, 0, 0, 1))
        print(brownian.Horne2dNormal(2, 2, 0, 0, 1))
    elif test == 2:
        print(Integrate1D(func, 0, 1, 10) * 2)
        print(Integrate1D(func, 1, 2, 10))
        print(Integrate1D(func, 2, 3, 10))
        print(Integrate1D(func, -3, 3, 10))
        print(Integrate1D(func, -8, 8, 10))
    elif test == 3:
        print(Integrate2D(func2, -1, 1, -1, 1, 100))
        print(Integrate2D(func2, -2, 2, -2, 2, 100))
        print(Integrate2D(func2, 2, 3, 2, 3, 10))
        print(Integrate2D(func2, -3, 3, -3, 3, 100))
    elif test == 4:
        print(Integrate2D(func2p, 0, 1, 0, math.pi / 2, 100) * 4)
        print(Integrate2D(func2p, 0, 2, 0, math.pi / 2, 100) * 4)
        print(Integrate2D(func2p, 0, 3, 0, math.pi / 2, 100) * 4)
