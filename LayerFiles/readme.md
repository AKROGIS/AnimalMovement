# Layer Files for Animal Movements

These are ArcMap layer files that connect to the enterprise database as a
domain user for accessing the animal movement data as a "query" layer in
ArcMap.  The layer files typically use the tables that are only visible by
Project Investigators and their assistants.  Layer files with an `_NPS`
suffix are usable by all domain users, but they do not show the most recent
30 days worth of data.

The "Create Map File" button in the Animal Movements application can also be
used to create layer files.  However, while the "Create Map File" provides a
lot of flexibility in selecting the project, animals and dates for filtering
the query, it does not provide the structure, symbology, dynamic date range,
or extra attributes that these layer files provide.

These layer files can be slow to draw, so they usually have the display turned
off.  Once they are added to your map, decide which sub layers you want to see
and turn just those on.

The tables that feed these layers contain **ALL** movement data. It will
_never_ finish drawing if you do not limit the data retrieved from the database
with a `WHERE` clause. All these layer files filter on at least the `ProjectId`
If you can also filter on a date range, or a set of `AnimalId`s, your layer
will perform much faster.  Each time that you pan or zoom, ArcMap will re-query
the database.  You should use the
[Feature Caching](https://desktop.arcgis.com/en/arcmap/latest/map/working-with-arcmap/working-with-the-feature-cache.htm)
as often as you can.

Query Layers are not editable.  The only way that you can change the data in
the animal Movements database is through the Animal Movements app and the
Edit Location Status (aka Paw Print) button in the Animal Movements AddIn.

**Note:**
The layer files have evolved over time.  They will not all look exactly like
the example below, and some of the older ones may contain errors.  If you think
you might be missing data (especially in the table view), check the "primary
key". Also check the layer symbology.  There should be no definiton query set
in any of these layers (the filtering is done in the SQL).

**Note:**
If you edit these layer files, it is important that you set the "primary key"
It should _just_ be `FixID` for location based layers, and _just_ `ProjectId`,
`AnimalId`, and `StartDate` for vector based data. Make sure `Gender` is not
also selected (by default it usually is which is annoying).

## Example Layer File

Most layer files are a derivative of this structure. The example is for the
Denali Bear Project.  The name of the layer file typically suggests which
project is selected by the query.

### Last 30 Days Group

These layers are much faster to draw, than the "All Time" layers.  When the
project is active, this is also what is most interesting day to day.

* Valid Locations (last 30 days)

  ```sql
  SELECT * FROM ValidLocations
  WHERE [ProjectId] = 'DENA_Bears'
  AND FixDate > dateadd(day, -30, getdate())
  ```

* Velocity Vectors (last 30 days)

  ```sql
  SELECT * FROM VelocityVectors
  WHERE [ProjectId]= 'DENA_Bears'
  AND LocalDateTime > dateadd(day, -31, getdate())
  ```

* Last Known Location (last 30 days)

  This includes the collar frequency and lat/long as text which is useful for
  radio tracking the animal from a plane. The table view of this layer can
  be printed before flying.  See also the `email_updates.py` script which
  emails a similar table view to select users every morning.

  ```sql
  SELECT
    R.ProjectID, R.AnimalId, R.FixDate, C.Frequency,
    R.Location.Lat as Latitude, R.Location.Long as Longitude,
    Location
  FROM MostRecentLocations as R
  JOIN CollarFixes as F on F.fixid = R.FixId
  JOIN Collars as C on C.CollarID = F.CollarID
  AND C.CollarManufacturer = F.CollarManufacturer
  WHERE R.ProjectId = 'DENA_Bears'
  AND R.FixDate > dateadd(day, -30, getdate())
  ```

### All Time Group

Same as above without the date restriction in the `WHERE` clause. And without
the suffix "(last 30 days)" in the name. It also has these extra layers

* Last Location of Known Mortalities

  ```sql
  SELECT * FROM LastLocationOfKnownMortalities
  WHERE [ProjectId] = 'DENA_Bears'
  ```

* Invalid Location

  Locations hidden by the user, usually because they are bad GPS locations.
  They can be unhidden (i.e. made valid again) with the Animal Movements AddIn.

  ```sql
  SELECT * FROM InvalidLocations
  WHERE [ProjectId] = 'DENA_Bears'
  ```

* No Movement Points

  These are movement vectors where the start and end points are the same.

  ```sql
  SELECT * FROM NoMovement
  WHERE [ProjectId]= 'DENA_Bears'
  ```

### Extra Data Group (optional)

These queries link in extra data when available from the Telonics Gen4 collar
file format.  This supplemental data does not exist for all collars, and is
_very_ slow to retrieve.  The tricky part is that this "extra" data may be
on the line before or after the line with the actual location data.

* Just 3D Locations

  Only locations designated as "3D" as opposed to "2D" or worse. This is the
  measure of the GPS accuracy, with "3D" being the best.
  ```sql
  SELECT
    L.*, G2.GpsFixAttempt, G2.GpsAltitude, G2.GpsHorizontalError,
    G2.GpsHorizontalDilution AS HDOP, G2.GpsPositionalDilution AS PDOP
  FROM Locations as L
  JOIN CollarFixes AS F
    ON F.FixId = L.FixId
  LEFT JOIN CollarDataTelonicsGen4 AS G1
    ON G1.FileId = F.FileId
    AND G1.LineNumber = F.LineNumber
  LEFT JOIN CollarDataTelonicsGen4 AS G2
    ON G2.FileId = G1.FileId
    AND G2.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1
    AND G2.AcquisitionStartTime = G1.AcquisitionStartTime
    AND G2.GpsFixAttempt IS NOT NULL
  WHERE [Status] IS NULL
    AND G2.GpsFixAttempt = 'Succeeded (3D)'
    AND L.ProjectId = 'DENA_Bears'
  ```

* All Locations with GPS Status

  Adds fields to evaluate the GPS accuracy when available.

  ```sql
  SELECT
    L.*, G2.GpsFixAttempt, G2.GpsAltitude, G2.GpsHorizontalError,
    G2.GpsHorizontalDilution AS HDOP, G2.GpsPositionalDilution AS PDOP
  FROM Locations as L
  JOIN CollarFixes AS F
    ON F.FixId = L.FixId
  LEFT JOIN CollarDataTelonicsGen4 AS G1
    ON G1.FileId = F.FileId
    AND G1.LineNumber = F.LineNumber
  LEFT JOIN CollarDataTelonicsGen4 AS G2
    ON G2.FileId = G1.FileId
    AND G2.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1
    AND G2.AcquisitionStartTime = G1.AcquisitionStartTime
    AND G2.GpsFixAttempt IS NOT NULL
  WHERE [Status] IS NULL
    AND L.ProjectId = 'DENA_Bears'
  ```

* Locations with Activity Count

  Adds data from the `activity` sensor and the thermometer when available.

  ```sql
  SELECT L.*, G2.ActivityCount
  FROM ValidLocations as L
  JOIN CollarFixes AS F
    ON F.FixId = L.FixId
  LEFT JOIN CollarDataTelonicsGen4 AS G1
    ON G1.FileId = F.FileId
    AND G1.LineNumber = F.LineNumber
  LEFT JOIN CollarDataTelonicsGen4 AS G2
    ON G2.FileId = G1.FileId
    AND G2.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1
    AND G2.AcquisitionStartTime = G1.AcquisitionStartTime
  WHERE L.ProjectId = 'DENA_Bears'
    AND G2.ActivityCount IS NOT NULL
  ```

* Locations with Temp

  Adds data from the `activity` sensor and the thermometer when available.

  ```sql
  SELECT L.*, G2.Temperature
  FROM ValidLocations as L
  JOIN CollarFixes AS F
    ON F.FixId = L.FixId
  LEFT JOIN CollarDataTelonicsGen4 AS G1
    ON G1.FileId = F.FileId
    AND G1.LineNumber = F.LineNumber
  LEFT JOIN CollarDataTelonicsGen4 AS G2
    ON G2.FileId = G1.FileId
    AND G2.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1
    AND G2.AcquisitionStartTime = G1.AcquisitionStartTime
  WHERE L.ProjectId = 'DENA_Bears'
    AND G2.Temperature IS NOT NULL
  ```

* Movements with Temp

  Same as above, but adds "TemperatureStart" and "TemperatureEnd" to the line
  between two points.

  ```sql
  SELECT
    M.*, G1A.Temperature AS TemperatureStart, G2A.Temperature AS TemperatureEnd
  FROM VelocityVectors as M
  JOIN Locations AS L1
    ON L1.ProjectId = M.ProjectId
    AND L1.AnimalId = M.AnimalId
    AND L1.FixDate = M.StartDate
  JOIN Locations AS L2
    ON L2.ProjectId = M.ProjectId
    AND L2.AnimalId = M.AnimalId
    AND L2.FixDate = M.EndDate
  JOIN CollarFixes AS F1
    ON L1.FixId = F1.FixId
  JOIN CollarFixes AS F2
    ON L2.FixId = F2.FixId
  LEFT JOIN CollarDataTelonicsGen4 AS G1
    ON G1.FileId = F1.FileId
    AND G1.LineNumber = F1.LineNumber
  LEFT JOIN CollarDataTelonicsGen4 AS G1A
    ON G1A.FileId = G1.FileId
    AND G1A.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1
    AND G1A.AcquisitionStartTime = G1.AcquisitionStartTime
  LEFT JOIN CollarDataTelonicsGen4 AS G2
    ON G2.FileId = F2.FileId AND G2.LineNumber = F2.LineNumber
  LEFT JOIN CollarDataTelonicsGen4 AS G2A
    ON G2A.FileId = G2.FileId
    AND G1A.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1
    AND G2A.AcquisitionStartTime = G2.AcquisitionStartTime
  WHERE M.ProjectId = 'DENA_Bears'
    AND G1A.Temperature IS NOT NULL
    AND G2A.Temperature IS NOT NULL
  ```

## Example "NPS" Layers

Example layer that is visible to all NPS users.  Change `DENA_Wolves` to the
project you are interested in.

* Valid Locations (all but last 30 days)

  ```sql
  SELECT * FROM ValidLocations_NPS WHERE [ProjectId] IN ('DENA_Wolves')
  ```

* Velocity Vectors (all but last 30 days)

  ```sql
  SELECT * FROM VelocityVectors WHERE [ProjectId] IN ('DENA_Wolves')
  ```

## Other Layer Files

Some special case layer files are listed here

* `Black River Moose for BLM.lyr` -
  This layer uses the fully qualified domain name for the server, so that
  other DOI users can see the server only NPS users can resolve the short
  server name.  BLM is the cooperator on this project.

* `LACL BrownBear Locations at 1h30m interval.lyr` -
Lake Clark Bear filtered to just exclude date not on a
90 minute interval.

  ```sql
  select * from ValidLocations
  where ProjectId = 'LACL_BrownBear'
    and (
         ('2015-03-15' <= LocalDateTime and LocalDateTime < '2015-07-15')
      or ('2016-03-15' <= LocalDateTime and LocalDateTime < '2016-07-15')
    )
    and (
      (DATEPART(hour,FixDate) in (0,3,6,9,12,15,18,21)
        and DATEPART(minute,FixDate) between 0 and 5
      )
      or (DATEPART(hour,FixDate) in (2,5,8,11,14,17,20,23)
        and DATEPART(minute,FixDate) between 55 and 59
      )
      or (DATEPART(hour,FixDate) in (1,4,7,10,13,16,19,22)
        and DATEPART(minute,FixDate) between 25 and 35
      )
    )
  ```

* `VHF Locations.lyr` -
Only data that has been loaded into the VHF data table. This is not GPS
data, and is sporadic locations of select animals.  This data usually pre-dates
the GPS data.
* `WACH Locations from 2018-09-01 to 2019-09-01 subsampled to 8hrs.lyr` -
The Western Arctic Caribou Herd filtered per the name.
* `WACH Suspicious Locations.lyr` -
* Selected locations that are suspect (usually at the end of a very short
movement vector).
* `YUCH Locations with John's Exclusion Home Range Codes.lyr` -
John Burch created a special table for tagging select locations as outliers for his home range analysis. These locations were determined
by manual review.
* `YUCH PPT and Televilt Wolves.lyr` -
The PPT and Televilt collars are GPS collars that were used before
Telonics.  This data is quite old.
* `YUCH Wolves(+1).lyr` -
Plus one is for a wolf that moved to Yukon Charley from Denali. It was
collared in the DENA wolf project and most of its data is still in Denali.  The database did not consider that an animal might wear the same collar in two different projects.
