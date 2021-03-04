# -*- coding: utf-8 -*-
"""
Creates a SQL file to create records from a CSV file of VHF location data.

This parser is specific for a single input file format.
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import csv
import datetime
from io import open

import csv23


class Config(object):
    """Namespace for configuration parameters. Edit as needed."""

    # pylint: disable=useless-object-inheritance,too-few-public-methods

    # input file that was written to the database as a blob, and from which
    # location records will be created that reference this file
    csv_path = r"YuchWolfVHFLocations.csv"

    # Get this number from the database after the file above has been loaded.
    file_id = "58310"

    # Species of the data in the input file
    species = "Wolf"

    # The file and location are part of this specific project.
    project_id = "Yuch_Wolf"

    # The file to create with the commands to run in the SQL Server console.
    sql_file = "VHFLocations.sql"


def make_datetime(date, time):
    """Returns a Python date object from a date and time string."""

    month, day, year = date.split("/")
    hour, minute = 0, 0  # Assume Midnight if no time given
    if time and time != "0" and time != "-9":
        if len(time) == 3:
            hour, minute = time[0], time[1:3]
        else:
            hour, minute = time[0:2], time[2:4]
    return datetime.datetime(int(year), int(month), int(day), int(hour), int(minute))


def quote(item):
    """Wrap item in single quotes; replace single quote with double single quote."""

    try:
        item = item.replace("'", "''")
    except AttributeError:
        pass
    return "'{0}'".format(item)


def make_sql():
    """Print following sql statement as a single line of text."""

    sql = (
        "INSERT INTO [VHFLocations] ("
        "FileId, LineNumber, ProjectID, AnimalId, Species, "
        "GroupName, Description, LocalFixDate, Location"
        ") VALUES ({0})\n"
    )
    with csv23.open(Config.csv_path, "r") as csv_file:
        csv_reader = csv.reader(csv_file)
        with open(Config.sql_file, "w", encoding="utf-8") as sql_file:
            # throw away the header (two lines)
            next(csv_reader)
            next(csv_reader)
            line_number = 2
            for row in csv_reader:
                row = csv23.fix(row)
                line_number += 1
                date = make_datetime(row[0], row[12])
                animals = row[7:11]
                lat, lon = row[39], row[40]
                if lat and lon:
                    try:
                        lat = float(lat)
                        lon = float(lon)
                    except (ValueError, TypeError):
                        print("lat or lon is not a float at", line_number, lat, lon)
                    if lat and lon:
                        for animal in animals:
                            if animal and animal != "0":
                                values = [
                                    Config.file_id,
                                    "{0}".format(line_number),
                                    quote(Config.project_id),
                                    quote(animal),
                                    quote(Config.species),
                                    quote(row[5]),
                                    quote(row[43]),
                                    quote(date),
                                    build_geom(lat, lon),
                                ]
                                values = ",".join(values)
                                sql_file.write(sql.format(values))


def build_geom(lat, lon):
    """Return a SQL Server Geometry spec from a pair of lat and lon numbers."""
    return "geography::Point({0}, {1}, 4326)".format(lat, lon)


make_sql()
