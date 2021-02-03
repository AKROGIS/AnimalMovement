# -*- coding: utf-8 -*-
"""
A unicode capable CSV compatibility module for Python 2 and Python 3.

Usage:

import csv
import csv23
with csv23.open("file.csv", "r") as in_file:
  reader = csv.reader(in_file, other_options)
  for row in reader:
    row = csv23.fix(row)
    # do stuff with row

# make a list of rows
with csv23.open("file.csv", "w") as out_file:
  writer = csv.writer(out_file, other_options)
  for rows in row:
    csv23.write(writer, row)
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import io
import sys


# pylint: disable=redefined-builtin
def open(filename, mode="r"):
    """
    Open a file for CSV mode in a Python 2 and 3 compatible way.

    mode must be one of "r" for reading or "w" for writing.
    """

    if sys.version_info[0] < 3:
        return io.open(filename, mode + "b")
    return io.open(filename, mode, encoding="utf-8", newline="")


def write(writer, row):
    """
    Write a row to a csv writer.

    writer is a csv.writer, and row is a list of unicode or number objects.
    """

    if sys.version_info[0] < 3:
        # Ignore the pylint error that unicode is undefined in Python 3
        # pylint: disable=undefined-variable

        writer.writerow(
            [
                item.encode("utf-8") if isinstance(item, unicode) else item
                for item in row
            ]
        )
    else:
        writer.writerow(row)


def fix(row):
    """Return a list of unicode strings from Python 2 or Python 3 strings."""

    if sys.version_info[0] < 3:
        return [item.decode("utf-8") for item in row]
    return row
