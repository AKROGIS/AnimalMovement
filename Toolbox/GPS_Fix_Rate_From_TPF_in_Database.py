# -*- coding: utf-8 -*-
"""
Reads the ParameterFiles in Animal Movements database and prints
(or saves to a csv file) the GPS schedule found in the file
The generated list can be filtered by the owner of the file.

This script was written for python 2.7 and may work for 3.x

Third party requirements:
* pyodbc - https://pypi.python.org/pypi/pyodbc
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import csv
import sys

import pyodbc


def get_connection_or_die(server, database):
    """
    Get a Trusted pyodbc connection to the SQL Server database on server.

    Try several connection strings.
    See https://github.com/mkleehammer/pyodbc/wiki/Connecting-to-SQL-Server-from-Windows

    Exit with an error message if there is no successful connection.
    """
    drivers = [
        "{ODBC Driver 17 for SQL Server}",  # supports SQL Server 2008 through 2017
        "{ODBC Driver 13.1 for SQL Server}",  # supports SQL Server 2008 through 2016
        "{ODBC Driver 13 for SQL Server}",  # supports SQL Server 2005 through 2016
        "{ODBC Driver 11 for SQL Server}",  # supports SQL Server 2005 through 2014
        "{SQL Server Native Client 11.0}",  # DEPRECATED: released with SQL Server 2012
        # '{SQL Server Native Client 10.0}',    # DEPRECATED: released with SQL Server 2008
    ]
    conn_template = "DRIVER={0};SERVER={1};DATABASE={2};Trusted_Connection=Yes;"
    for driver in drivers:
        conn_string = conn_template.format(driver, server, database)
        try:
            connection = pyodbc.connect(conn_string)
            return connection
        except pyodbc.Error:
            pass
    print("Rats!! Unable to connect to the database.")
    print("Make sure you have an ODBC driver installed for SQL Server")
    print("and your AD account has the proper DB permissions.")
    print("Contact akro_gis_helpdesk@nps.gov for assistance.")
    sys.exit()


def print_gps_lines(connection, pi):
    data = set()
    if pi is None:
        sql = "SELECT Contents FROM CollarParameterFiles;"
    else:
        sql = "SELECT Contents FROM CollarParameterFiles WHERE Owner = ?;"
    try:
        if pi is None:
            rows = connection.cursor().execute(sql).fetchall()
        else:
            rows = connection.cursor().execute(sql, pi).fetchall()
    except pyodbc.Error as ex:
        err = "Database error:\n{0}\n{1}".format(sql, ex)
        print(err)
        rows = []
    for row in rows:
        # Row contents is a binary blob of the file contents.
        file_contents = row.Contents.decode("utf-8")
        for line in file_contents.split("\n"):
            if " " in line:
                line_title = line.split(" ")[0]
                if "gps" in line_title.lower():
                    # data.add(line)
                    data.add(line_title)
    for line in sorted(list(data)):
        print(line)


"""
As of 2020-11-13:

sections.argossatellite.parameters.turnOffGPSPredictionAfterNDays
sections.auxiliary1ScheduleSet.parameters.gpsScheduleAdvancedSchedule
sections.auxiliary1ScheduleSet.parameters.gpsScheduleType
sections.auxiliary1ScheduleSet.parameters.gpsScheduleUpdatePeriod
sections.auxiliary2ScheduleSet.parameters.gpsScheduleAdvancedSchedule
sections.auxiliary2ScheduleSet.parameters.gpsScheduleType
sections.auxiliary2ScheduleSet.parameters.gpsScheduleUpdatePeriod
sections.auxiliary3ScheduleSet.parameters.gpsScheduleAdvancedSchedule
sections.auxiliary3ScheduleSet.parameters.gpsScheduleType
sections.auxiliary3ScheduleSet.parameters.gpsScheduleUpdatePeriod
sections.collarWriteup.parameters.optionTreeGpsCommonParts
sections.factoryRoofScheduleSet.parameters.gpsScheduleAdvancedSchedule
sections.factoryRoofScheduleSet.parameters.gpsScheduleType
sections.factoryRoofScheduleSet.parameters.gpsScheduleUpdatePeriod
sections.geofence.parameters.insideGpsScheduleAdvancedSchedule
sections.geofence.parameters.insideGpsScheduleType
sections.geofence.parameters.insideGpsScheduleUpdatePeriod
sections.gps.parameters.allowVhfWhileNavigating
sections.gps.parameters.continuousNavigationMode
sections.gps.parameters.fixTimeout
sections.gps.parameters.gpsScheduleAdvancedSchedule
sections.gps.parameters.gpsScheduleType
sections.gps.parameters.gpsScheduleUpdatePeriod
sections.gps.parameters.nmeaBaudRate
sections.gps.parameters.nmeaSentences
sections.gps.parameters.passiveAntennaFlag
sections.gps.parameters.qfpScheduleAdvancedSchedule
sections.gps.parameters.qfpScheduleType
sections.gps.parameters.qfpScheduleUpdatePeriod
sections.gps.parameters.qfpTimeout
sections.gps.parameters.qfpWithoutGpsTolerance
sections.gps.parameters.strategy
sections.gpsSchedule.parameters.schedule
sections.hardware.parameters.gpsSleepThreshold
sections.hardware.parameters.gpsSleepThresholdBoosted
sections.hardware.parameters.gpsSleepThresholdUnboosted
sections.underwater.parameters.useGpsInHaulout
sections.units.parameters.gpsFirmwareVersionList
sections.vhf.parameters.gpsVhfBeaconControl
"""


def read(connection, pi):
    data = []
    if pi is None:
        sql = "SELECT FileId, FileName, Contents FROM CollarParameterFiles;"
    else:
        sql = "SELECT FileId, FileName, Contents FROM CollarParameterFiles WHERE Owner = ?;"
    try:
        if pi is None:
            rows = connection.cursor().execute(sql).fetchall()
        else:
            rows = connection.cursor().execute(sql, pi).fetchall()
    except pyodbc.Error as ex:
        err = "Database error:\n{0}\n{1}".format(sql, ex)
        print(err)
        rows = []
    for row in rows:
        # row contents is a binary blob of the file contents.
        file_contents = row.Contents.decode("utf-8")
        for schedule in read_simple(file_contents):
            data.append(("simple", row.FileId, row.FileName, "", "", "", schedule))
        for schedule in read_advanced(file_contents):
            data.append((["advanced", row.FileId, row.FileName] + schedule))
    return data


def read_advanced(file_contents):
    """Read advanced GPS schedules from the unicode file_contents."""

    data = []
    schedule = []
    in_schedule = False
    in_season = False
    # A line may have on
    for line in file_contents.split("\n"):
        if in_schedule:
            # print(line)
            if in_season:
                if line.startswith("   }"):
                    if schedule:
                        data.append(schedule)
                        schedule = []
                    in_season = False
                else:
                    if "{" in line:
                        value = (
                            line.replace("onPeriod", "")
                            .replace("{", "")
                            .replace("}", "")
                            .strip()
                        )
                    else:
                        value = line.strip()
                    schedule.append(value)
            elif line.startswith("}"):
                in_schedule = False
            elif line.startswith("   season {"):
                in_season = True
        elif line.startswith(
            "sections.gps.parameters.gpsScheduleAdvancedSchedule"
        ) or line.startswith("sections.gpsSchedule.parameters.schedule"):
            # print(line)
            in_schedule = True
    return data


def read_simple(file_contents):
    """Read simple GPS schedules from the unicode file_contents."""

    data = []
    for line in file_contents.split("\n"):
        interval = None
        if line.startswith("sections.gps.parameters.gpsScheduleUpdatePeriod"):
            # print(line)
            try:
                interval = line.replace(
                    "sections.gps.parameters.gpsScheduleUpdatePeriod", ""
                )
                interval = interval.replace("{", "").replace("}", "")
                interval = interval.strip()
            except:
                print("Error in", line)
            if interval and interval != "0":
                data.append(interval)
    return data


def open_csv_write(filename):
    """Open a file for CSV writing in a Python 2 and 3 compatible way."""
    if sys.version_info[0] < 3:
        return open(filename, "wb")
    return open(filename, "w", encoding="utf8", newline="")


def write_csv_row(writer, row):
    """writer is a csv.writer, and row is a list of unicode or number objects."""
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


def main(pi=None, csvfile=None, server="inpakrovmais", db="Animal_Movement"):

    conn = get_connection_or_die(server, db)

    # Only need to do once, to verify search parameters for other functions
    # print_gps_lines(conn, pi)

    if csvfile is None:
        fmt = "{1:<5}{0:<10}{3:<12}{4:<12}{5:<10}{6:<30}{2:<50}"
        print(fmt.format("Type", "Id", "Name", "Start", "Stop", "Interval", "Period"))
        for item in read(conn, pi):
            print(fmt.format(*item))
    else:
        with open_csv_write(csvfile) as f:
            out = csv.writer(f)
            row = [
                "Type",
                "TPF_FileId",
                "TPF_Filename",
                "Start",
                "Stop",
                "Interval",
                "Period",
            ]
            write_csv_row(out, row)
            for item in read(conn, pi):
                write_csv_row(out, item)


if __name__ == "__main__":
    # main(None, r'C:\tmp\list.csv')
    # main(r'nps\bborg', r'C:\tmp\list.csv')
    main(r"nps\msorum", None)
    # main(r'nps\bborg', None)
