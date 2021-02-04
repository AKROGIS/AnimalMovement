# -*- coding: utf-8 -*-
"""
Read a CSV file in one format and convert to another CSV format.
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import csv
from io import open

import csv23


def make_csv(in_file, out_file):
    header = [
        "Id",
        "Date",
        "LC",
        "IQ",
        "Lat1",
        "Lon1",
        "Lat2",
        "Lon2",
        "Nb mes",
        "Nb mes>-120dB",
        "Best level",
        "Pass duration",
        "NOPC",
        "Calcul freq",
        "Altitude",
    ]
    separators = ["  {0} : ".format(item) for item in header[1:]]
    template = "{0}  {1}  {2}  {3}  {4}"
    with csv23.open(out_file, "w") as csv_file:
        csv_writer = csv.writer(csv_file)
        csv23.write(csv_writer, header)
        with open(in_file, "r", encoding="utf-8") as data:
            line0 = ""
            while True:
                line1 = data.readline()
                if not line1:
                    break
                line1 = line1.strip()
                if line1[:4] == "Lat1":
                    line2 = data.readline().strip()
                    line3 = data.readline().strip()
                    line4 = data.readline().strip()
                    line = template.format(line0, line1, line2, line3, line4)
                    for separator in separators:
                        line = line.replace(separator, "|")
                    row = [item.strip() for item in line.split("|")]
                    csv23.write(csv_writer, row)
                    line0 = line4
                    continue
                else:
                    line0 = line1
                    continue


def make_csv2(in_file, out_file):
    """Convert John's XLS data into database format L (as expected by the make_aws function below"""
    header = [
        "Id",
        "Date",
        "LC",
        "IQ",
        "Lat1",
        "Lon1",
        "Lat2",
        "Lon2",
        "Nb mes",
        "Nb mes>-120dB",
        "Best level",
        "Pass duration",
        "NOPC",
        "Calcul freq",
        "Altitude",
    ]
    with csv23.open(out_file, "w") as csv_file:
        csv_writer = csv.writer(csv_file)
        csv23.write(csv_writer, header)
        with csv23.open(in_file, "r") as data:
            csv_reader = csv.reader(data)
            next(csv_reader)  # remove old header
            for row in csv_reader:
                row = csv23.fix(row)
                date = fix_date2(row[4], row[8])
                loc_class = row[9] if row[9] else "B"
                new_row = [
                    row[1],
                    date,
                    loc_class,
                    row[10],
                    row[11],
                    row[12],
                    row[13],
                    row[14],
                    row[17],
                    row[18],
                    row[19],
                    row[20],
                    row[21],
                    row[22],
                    row[23],
                ]
                csv23.write(csv_writer, new_row)


def fix_date2(d, t):
    """ ('3/14/2002','05:21:43') => '14.03.02 05:21:43'"""
    m, d, y = d.split("/")
    m = int(m)
    d = int(d)
    y = int(y) - 2000
    if len(t) == 7:
        # pad "1:23:45" to "01:23:45"
        t = "0{0}".format(t)
    if not ":" in t:
        t = "{0:02d}:00:00".format(int(t))
    return "{0:02d}.{1:02d}.{2:02d} {3}".format(d, m, y, t)


def fix_date(d):
    date, time = d.split(" ")
    d, m, y = date.split(".")
    return "20{0}-{1}-{2}T{3}.000Z".format(y, m, d, time)


def fix_lat(l):
    l = l.replace("?", "")
    if l[-1:] == "N":
        return float(l[:-1])
    if l[-1:] == "S":
        return -1 * float(l[:-1])
    return l


def fix_lon(l):
    l = l.replace("?", "")
    if l[-1:] == "E":
        return float(l[:-1])
    if l[-1:] == "W":
        return -1 * float(l[:-1])
    return l


def fix_duration(d):
    """Returns 0 if d is empty string."""
    return int("0{0}".format(d.replace("?", "").replace("s", "").strip()))


def fix_alt(a):
    """Returns 0 if a is empty string."""
    return int("0{0}".format(a.replace("?", "").replace("m", "").strip()))


def fix_freq(a):
    """Returns 0 if a is empty string."""
    f = float("0{0}".format(a.replace("?", "").replace("Hz", "").replace(" ", "")))
    return "{:E}".format(f).replace("E+0", "E")


def fix_nopc(n):
    return n.replace("?", "")


def fix_level(l):
    return l.replace(" dB", "")


def make_aws(in_file, out_file):
    header = (
        (
            '"programNumber";"platformId";"platformType";"platformModel";'
            '"platformName";"platformHexId";"satellite";"bestMsgDate";"duration";'
            '"nbMessage";"message120";"bestLevel";"frequency";"locationDate";'
            '"latitude";"longitude";"altitude";"locationClass";"gpsSpeed";'
            '"gpsHeading";"latitude2";"longitude2";"altitude2";"index";"nopc";'
            '"errorRadius";"semiMajor";"semiMinor";"orientation";"hdop";'
            '"bestDate";"compression";"type";"alarm";"concatenated";"date";'
            '"level";"doppler";"rawData"'
        )
        .replace('"', "")
        .split(";")
    )
    empty_row = [""] * len(header)
    with csv23.open(in_file, "r") as csv_file:
        csv_reader = csv.reader(csv_file)
        with csv23.open(out_file, "w") as csv_file2:
            csv_writer = csv.writer(csv_file2, delimiter=";", quoting=csv.QUOTE_ALL)
            csv23.write(csv_writer, header)
            next(csv_reader)  # throw away the header
            for row in csv_reader:
                row = csv23.fix(row)
                date = fix_date(row[1])
                lat = fix_lat(row[4])
                lon = fix_lon(row[5])
                if not date or not lat or not lon:
                    continue  # we must have a date/lat/long
                new_row = list(empty_row)
                new_row[0] = "2433"
                new_row[1] = row[0]
                new_row[7] = date
                new_row[13] = date
                new_row[17] = row[2]  # LC
                new_row[23] = row[3]  # IQ
                new_row[14] = lat
                new_row[15] = lon
                new_row[20] = fix_lat(row[6])  # Lat2
                new_row[21] = fix_lon(row[7])  # Lon2
                new_row[9] = int("0{0}".format(row[8]))  # Nb mes
                new_row[10] = int("0{0}".format(row[9]))  # Nb mes>-120dB
                new_row[11] = fix_level(row[10])  # Best level
                new_row[8] = fix_duration(row[11])  # Pass duration
                new_row[24] = fix_nopc(row[12])  # NOPC
                new_row[12] = fix_freq(row[13])  # Calcul freq
                new_row[16] = fix_alt(row[14])  # Altitude
                new_row[38] = "06A88"  # junk to get it to process in the DB.
                csv23.write(csv_writer, new_row)


make_csv2(r"AllArgosDataClean2.csv", r"Johns_HandCrafted_Argos_Data.csv")
# make_csv(r'ArgosArchiveTest.txt', r'ArgosArchiveTest.csv')
make_aws(r"Johns_HandCrafted_Argos_Data.csv", r"Johns_HandCrafted_Argos_Data.aws")
