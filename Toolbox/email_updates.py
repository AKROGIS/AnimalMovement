# -*- coding: utf-8 -*-
from __future__ import absolute_import, division, print_function, unicode_literals

import pyodbc
# pip install pyodbc
import smtplib

# The account running this script must have editor or better permissions on the DB

# updates maps the user's email to a project.  If a user wants multiple projects, then include them
# multiple times, once for each project.
# The project is the ProjectID in the Projects table in the Animal_Movement DB

updates = {
    'regan_sarwas@nps.gov': 'DENA_Wolves',
    'dmiller@polarnet.com': 'DENA_Wolves',
    'bridget_borg@nps.gov': 'DENA_Wolves',
}

# hostname of the SMTP email server
mailhost = 'mailer.itc.nps.gov'

# email account that will send the email
sender = 'regan_sarwas@nps.gov'

# Connection string to database
conn_string = "DRIVER={SQL Server Native Client 11.0};" + \
              "SERVER=INPAKROVMAIS;DATABASE=Animal_Movement;Trusted_Connection=Yes;"

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


def send_smtp_email(mailhost, from_addr, to_addrs, subject, text):
    smtp = smtplib.SMTP(mailhost)
    msg = "From: {0}\r\nTo: {1}\r\nSubject: {2}\r\n\r\n{3}".format(from_addr, ','.join(to_addrs), subject, text)
    smtp.sendmail(from_addr, to_addrs, msg)
    smtp.quit()


def format_locations(rows):
    results = "{0:8}\t{1:19}\t{2:9}\t{3}\r\n".format('AnimalId', 'LastFixDate_UTC', 'Lat_WGS84_DM', 'Lon_WGS84_DM')
    for row in rows:
        results += "{0:8}\t{1}\t{2}\t{3}\r\n".format(row[0],row[1],row[2],row[3])
    return results

def main():
    connection = pyodbc.connect(conn_string)
    for user, project in updates.items():
        # print(user)
        # print(project)
        sql = query.format(project)
        rows = connection.cursor().execute(sql).fetchall()
        # locations = format_locations(rows)
        print(locations)
        subject = 'Last known locations for {0}'.format(project)
        send_smtp_email(mailhost, sender, [user], subject, str(locations))


main()
