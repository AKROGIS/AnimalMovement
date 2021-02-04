# -*- coding: utf-8 -*-
"""
Functions and tests for numerical (piecewise) integration
"""

from __future__ import absolute_import, division, print_function, unicode_literals


def Integrate1D(f, x1, x2, n):
    #integrate f(x) from x1 to x2 using piecewise integration with n pieces
    #Uses the Newton-Cotes rectangle (or midpoint) rule

    stepSize = float(x2 - x1) / n
    x = x1 + stepSize / 2
    accumulator = 0.0
    while x < x2:
        #area = f(x) * stepSize
        #accumulator += area
        accumulator += f(x)
        x += stepSize
    #return accumulator
    return accumulator * stepSize

def Integrate2D(f, x1, x2, y1, y2, n):
    #integrate f(x) from x1 to x2 using piecewise integration with n pieces
    #Uses the Newton-Cotes rectangle (or midpoint) rule

    xStepSize = float(x2 - x1) / n
    yStepSize = float(y2 - y1) / n
    x = x1 + xStepSize / 2
    y0 = y1 + yStepSize / 2
    unitArea = xStepSize * yStepSize
    accumulator = 0.0
    while x < x2:
        y = y0
        while y < y2:
            #area = f(x,y) * unitArea
            #accumulator += area
            accumulator += f(x,y)
            y += yStepSize
        x += xStepSize
    #return accumulator
    return accumulator * unitArea

def func(x):
    #return 2*x
    return Normal(x,0,1)

def func2(x,y):
    #return 2*x*y
    return Horne2dNormal(x,y,0,0,1)

def func2p(r,a):
    #polar function, r = radius, a = angle in radians
    x = r * math.cos(a)
    y = r * math.sin(a)
    return Horne2dNormal(x,y,0,0,1)

if __name__ == '__main__':
    #test2()
    #print(Normal(2,0,1))
    #print(Normal2D(0,2,0,0,1))
    #print(HorneNormal2D(0,2,0,0,1))
    #print(Horne2dNormal(2,2,0,0,1))
    if False:
        print(Integrate1D(func,0,1,10)*2)
        print(Integrate1D(func,1,2,10))
        print(Integrate1D(func,2,3,10))
        print(Integrate1D(func,-3,3,10))
        print(Integrate1D(func,-8,8,10))
    if False:
        print(Integrate2D(func2,-1,1,-1,1,100))
        print(Integrate2D(func2,-2,2,-2,2,100))
        print(Integrate2D(func2,2,3,2,3,10))
        print(Integrate2D(func2,-3,3,-3,3,100))
    if True:
        print(Integrate2D(func2p,0,1,0,math.pi/2,100)*4)
        print(Integrate2D(func2p,0,2,0,math.pi/2,100)*4)
        print(Integrate2D(func2p,0,3,0,math.pi/2,100)*4)
