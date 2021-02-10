# -*- coding: utf-8 -*-
"""
Send an email with a database report to a list of users.

Typically run as a scheduled task to send a daily report.

The account running this script must have editor or better permissions on the DB

updates maps the user's email to a project.  If a user wants multiple projects,
then include them multiple times, once for each project.
The project is the ProjectID in the Projects table in the Animal_Movement DB

Third party requirements:
* pyodbc - https://pypi.python.org/pypi/pyodbc
"""

from __future__ import absolute_import, division, print_function, unicode_literals

import smtplib
import sys

import pyodbc


class Config(object):
    """Namespace for configuration parameters. Edit as needed."""

    # pylint: disable=useless-object-inheritance,too-few-public-methods

    # A list of (email address, Project ID) tuples
    # the email address will be send the database report for that project.
    reports = [
        ("regan_sarwas@nps.gov", "DENA_Wolves"),
    ]

    # hostname of the SMTP email server
    mailhost = "mailer.itc.nps.gov"

    # email account that will send the email
    sender = "regan_sarwas@nps.gov"

    server = "inpakrovmais"

    database = "Animal_Movement"

    # Query to get project locations from the database
    query = """
    SELECT AnimalId
        ,FixDate AS LastFixDate_UTC
    --	  ,Location.Lat AS Lat_WGS84
    --      ,Location.Long AS Lon_WGS84
        ,cast(cast(abs(Location.Lat) as Int) AS NVARCHAR) + N'° ' + convert(NVARCHAR, (60 * (Location.Lat - cast(Location.Lat as Int)))) + ''' ' + (case when Location.Lat < 0 then 'S' ELSE 'N' END)   AS Lat_WGS84_DM
        ,cast(cast(abs(Location.Long) as Int) AS NVARCHAR) + N'° ' + convert(NVARCHAR, (60 * (abs(Location.Long) - cast(abs(Location.Long) as Int)))) + ''' ' + (case when Location.Long < 0 then 'W' ELSE 'E' END) AS Lon_WGS84_DM
        FROM [Animal_Movement].[dbo].[MostRecentLocations]
    WHERE ProjectId = '{0}' AND dateadd(day, -30, getdate()) < fixDate
    ORDER BY FixDate DESC
    """


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


def send_smtp_email(mailhost, from_addr, to_addr, subject, text):
    """Send an email from_addr to_addr, using the mailhost server."""

    smtp = smtplib.SMTP(mailhost)
    msg = "From: {0}\r\nTo: {1}\r\nSubject: {2}\r\n\r\n{3}".format(
        from_addr, to_addr, subject, text
    )
    # print(msg.encode('utf-8'))
    smtp.sendmail(from_addr, [to_addr], msg.encode("utf-8"))
    smtp.quit()


def format_locations(rows):
    """Returns the rows as a single formatted text."""

    results = "{0:8}\t{1:19}\t{2:9}\t{3}\r\n".format(
        "AnimalId", "LastFixDate_UTC", "Lat_WGS84_DM", "Lon_WGS84_DM"
    )
    for row in rows:
        results += "{0:8}\t{1}\t{2}\t{3}\r\n".format(row[0], row[1], row[2], row[3])
    return results


def main():
    """Connect to the server, get the report and send the email."""

    connection = get_connection_or_die(Config.server, Config.database)
    for user, project in Config.reports:
        # print(user)
        # print(project)
        sql = Config.query.format(project)
        rows = connection.cursor().execute(sql).fetchall()
        locations = format_locations(rows)
        # print(locations)
        subject = "Last known locations for {0}".format(project)
        send_smtp_email(Config.mailhost, Config.sender, user, subject, locations)


if __name__ == "__main__":
    main()
