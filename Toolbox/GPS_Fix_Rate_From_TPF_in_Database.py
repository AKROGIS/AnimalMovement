'''
DESCRIBE ME

This script was written for python 2.7 and has an external dependency on
the **pyodbc** python module. It can be installed with **pip install pyodbc**
'''

import sys


def get_connection_or_die(pyodbc, server, db):
    # See https://github.com/mkleehammer/pyodbc/wiki/Connecting-to-SQL-Server-from-Windows
    drivers = [ '{ODBC Driver 17 for SQL Server}',    # supports SQL Server 2008 through 2017
                '{ODBC Driver 13.1 for SQL Server}',  # supports SQL Server 2008 through 2016
                '{ODBC Driver 13 for SQL Server}',    # supports SQL Server 2005 through 2016
                '{ODBC Driver 11 for SQL Server}',    # supports SQL Server 2005 through 2014
                '{SQL Server Native Client 11.0}',    # DEPRECATED: released with SQL Server 2012
                # '{SQL Server Native Client 10.0}',    # DEPRECATED: released with SQL Server 2008
    ]
    conn_template = "DRIVER={0};SERVER={1};DATABASE={2};Trusted_Connection=Yes;"
    for driver in drivers:
        conn_string = conn_template.format(driver, server, db)
        try:
            connection = pyodbc.connect(conn_string)
            return connection
        except pyodbc.Error:
            pass
    print("Rats!! Unable to connect to the database.")
    print("Make sure you have an ODBC driver installed for SQL Server")
    print("and your AD account has the proper DB permissions.")
    print("Contact regan_sarwas@nps.gov for assistance.")
    sys.exit()


def read(connection, pi):
    data = []
    sql = "SELECT FileName, Contents FROM CollarParameterFiles WHERE Owner = ?;" # and FileName = '643101.tpf';"
    try:
        rows = connection.cursor().execute(sql, pi).fetchall()
    except Exception as de:
        err = "Database error:\n" + str(sql) + '\n' + str(de)
        print(err)
        rows = []
    for row in rows:
        schedule = []
        in_schedule = False
        have_schedule = False
        for line in [str(l) for l in row.Contents.split('\n')]:
            # line = line.replace('\r','')
            #print(line)
            if line.startswith('}'):
                in_schedule = False
                #if schedule:
                #    break
                #else:
                #    continue
            if in_schedule:
                if line and not (line.startswith('   season {') or line.startswith('   }')):
                    schedule.append(line.strip())
                continue
            if line.startswith('sections.gpsSchedule.parameters.schedule') or \
               line.startswith('sections.gps.parameters.gpsScheduleAdvancedSchedule'): # or \
               #line.startswith('sections.gps.parameters.qfpScheduleAdvancedSchedule'):
                in_schedule = True
                continue
        # print((row.FileName, schedule))
        sched = '|'.join(schedule)
        data.append((row.FileName, sched))
    return data


def main(pi, csvfile=None, server='inpakrovmais', db='Animal_Movement'):
    conn = None
    try:
        import pyodbc
    except ImportError:
        pyodbc = None
        pydir = os.path.dirname(sys.executable)
        print 'pyodbc module not found, make sure it is installed with'
        print pydir + r'\Scripts\pip.exe install pyodbc'
        print 'Don''t have pip?'
        print 'Download <https://bootstrap.pypa.io/get-pip.py> to ' + pydir + r'\Scripts\get-pip.py'
        print 'Then run'
        print sys.executable + ' ' + pydir + r'\Scripts\get-pip.py'
        sys.exit()
    conn = get_connection_or_die(pyodbc, server, db)

    gps_data = read(conn, pi)
    if csvfile is None:
        print("{0:<45}{1}".format("File", "Schedule"))
        for item in gps_data:
            print("{0:<45}{1}".format(item[0], item[1]))
    else:
        import csv
        with open(csvfile, 'wb') as f:
            out = csv.writer(f)
            out.writerow(["TPF_File", "Schedule"])
            for item in gps_data:
                out.writerow(item)


if __name__ == '__main__':
    main(r'nps\bborg', r'C:\tmp\list.csv')
