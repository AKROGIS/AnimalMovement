# -*- coding: utf-8 -*-
"""
Creates a SQL file to create records from a CSV file of VHF location data.
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import csv
import datetime
from io import open
import sys


# This parser is specific for a single input file as described here.
project_id = 'Yuch_Wolf'
species = 'Wolf'
filename = r'YuchWolfVHFLocations.csv'
file_id = '58310'  # Get this number from the database after the file above has been loaded.


def make_datetime(date, time):
    month, day, year = date.split('/')
    hour, minute = 0, 0  # Assume Midnight if no time given
    if time and time != '0' and time != '-9':
        if len(time) == 3:
            hour, minute = time[0], time[1:3]
        else:
            hour, minute = time[0:2], time[2:4]
    return datetime.datetime(int(year), int(month), int(day), int(hour), int(minute))

def open_csv_read(filename):
    """Open a file for CSV reading in a Python 2 and 3 compatible way."""
    if sys.version_info[0] < 3:
        return open(filename, "rb")
    return open(filename, "r", encoding="utf8", newline="")

def make_sql():
    header = 'FileId,LineNumber,ProjectID,AnimalId,Species,GroupName,Description,LocalFixDate,Location'
    sql = "INSERT INTO [VHFLocations] (" + header + ") VALUES ({0})\n"
    with open_csv_read(filename) as csv_file:
        csv_reader = csv.reader(csv_file)
        with open(r'VHFLocations.sql', 'w', encoding="utf-8") as sql_file:
            csv_reader.next()  # throw away the header (two lines)
            csv_reader.next()
            line = 2
            for row in csv_reader:
                line += 1
                date = make_datetime(row[0], row[12])
                wolves = row[7:11]
                lat, lon = row[39], row[40]
                if lat and lon:
                    try:
                        lat = float(lat)
                        lon = float(lon)
                    except (ValueError, TypeError):
                        print('lat or lon is not a float at', line, lat, lon)
                    if lat and lon:
                        for wolf in wolves:
                            if wolf and wolf != '0':
                                values = [file_id,
                                          str(line),
                                          "'" + project_id + "'",
                                          "'" + wolf + "'",
                                          "'" + species + "'",
                                          "'" + row[5] + "'",
                                          "'" + row[43].replace("'","''") + "'",
                                          "'" + str(date) + "'",
                                          build_geom(lat, lon)]
                                values = ','.join(values)
                                sql_file.write(sql.format(values))


def build_geom(lat, lon):
    return "geography::Point({0}, {1}, 4326)".format(lat, lon)


make_sql()
