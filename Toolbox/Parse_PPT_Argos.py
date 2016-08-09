import csv


def make_csv(in_file, out_file):
    header = ['Id', 'Date', 'LC', 'IQ', 'Lat1', 'Lon1', 'Lat2', 'Lon2', 'Nb mes', 'Nb mes>-120dB',
              'Best level', 'Pass duration', 'NOPC', 'Calcul freq', 'Altitude']
    separators = ['  ' + item + ' : ' for item in header[1:]]
    with open(out_file, 'wb') as csv_file:
        csv_writer = csv.writer(csv_file)
        csv_writer.writerow(header)
        with open(in_file, 'r') as data:
            line0 = ''
            while True:
                line1 = data.readline()
                if not line1:
                    break
                line1 = line1.strip()
                if line1[:4] == 'Lat1':
                    line2 = data.readline().strip()
                    line3 = data.readline().strip()
                    line4 = data.readline().strip()
                    line = line0 + '  ' + line1 + '  ' + line2 + '  ' + line3 + '  ' + line4

                    for separator in separators:
                        line = line.replace(separator, '|')
                    row = [item.strip() for item in line.split('|')]
                    csv_writer.writerow(row)
                    line0 = line4
                    continue
                else:
                    line0 = line1
                    continue


def fix_date(d):
    date, time = d.split(' ')
    d, m, y = date.split('.')
    return "20{0}-{1}-{2}T{3}.000Z".format(y, m, d, time)


def fix_lat(l):
    l = l.replace('?', '')
    if l[-1:] == 'N':
        return float(l[:-1])
    if l[-1:] == 'S':
        return -1 * float(l[:-1])
    return l


def fix_lon(l):
    l = l.replace('?', '')
    if l[-1:] == 'E':
        return float(l[:-1])
    if l[-1:] == 'W':
        return -1 * float(l[:-1])
    return l


def fix_duration(d):
    return int('0' + d.replace('?', '').replace('s', '').strip())


def fix_alt(a):
    return int('0' + a.replace('?', '').replace('m', '').strip())


def fix_freq(a):
    f = float('0' + a.replace('?', '').replace('Hz', '').replace(' ', ''))
    return '{:E}'.format(f).replace('E+0', 'E')


def fix_nopc(n):
    return n.replace('?', '')


def fix_level(l):
    return l.replace(' dB', '')


def make_aws(in_file, out_file):
    header = ('"programNumber";"platformId";"platformType";"platformModel";"platformName";"platformHexId";' +
              '"satellite";"bestMsgDate";"duration";"nbMessage";"message120";"bestLevel";"frequency";' +
              '"locationDate";"latitude";"longitude";"altitude";"locationClass";"gpsSpeed";"gpsHeading";' +
              '"latitude2";"longitude2";"altitude2";"index";"nopc";"errorRadius";"semiMajor";"semiMinor";' +
              '"orientation";"hdop";"bestDate";"compression";"type";"alarm";"concatenated";"date";"level";' +
              '"doppler";"rawData"').replace('"', '').split(';')
    empty_row = ['']*len(header)
    with open(in_file, 'rb') as csv_file:
        csv_reader = csv.reader(csv_file)
        with open(out_file, 'wb') as csv_file2:
            csv_writer = csv.writer(csv_file2, delimiter=';', quoting=csv.QUOTE_ALL)
            csv_writer.writerow(header)
            csv_reader.next()  # throw away the header
            for row in csv_reader:
                date = fix_date(row[1])
                lat = fix_lat(row[4])
                lon = fix_lon(row[5])
                if not date or not lat or not lon:
                    continue # we must have a date/lat/long
                new_row = list(empty_row)
                new_row[0] = '2433'
                new_row[1] = row[0]
                new_row[7] = date
                new_row[13] = date
                new_row[17] = row[2]  # LC
                new_row[23] = row[3]  # IQ
                new_row[14] = lat
                new_row[15] = lon
                new_row[20] = fix_lat(row[6])  # Lat2
                new_row[21] = fix_lon(row[7])  # Lon2
                new_row[9] = int(row[8])  # Nb mes
                new_row[10] = int(row[9])  # Nb mes>-120dB
                new_row[11] = fix_level(row[10])  # Best level
                new_row[8] = fix_duration(row[11])  # Pass duration
                new_row[24] = fix_nopc(row[12])  # NOPC
                new_row[12] = fix_freq(row[13])  # Calcul freq
                new_row[16] = fix_alt(row[14])  # Altitude
                new_row[38] = "06A88"  # junk to get it to process in the DB.
                csv_writer.writerow(new_row)

# make_csv(r'DataArchiveARGOS103103.txt', r'DataArchiveARGOS103103.csv')
make_aws(r'DataArchiveARGOS103103.csv', r'DataArchiveARGOS103103.aws')
