USE [Animal_Movement]
GO
INSERT [dbo].[LookupCollarManufacturers] ([CollarManufacturer], [Name], [Website], [Description]) VALUES (N'Lotek', N'Lotek Wireless Inc. ', N'http://www.lotek.com/', N'
115 Pony Drive 
Newmarket, Ontario
Canada L3Y 7B5; 

Telephone: 905-836-6680')
INSERT [dbo].[LookupCollarManufacturers] ([CollarManufacturer], [Name], [Website], [Description]) VALUES (N'Televilt', N'Tellus GPS systems', N'http://wildlife.followit.se', N'Followit AB, Bandygatan 2, 711 34 Lindesberg; Tel: +46 (0)581-171 90; Fax: +46 (0)581-171 96; E-mail: wildlifesales@followit.se')
INSERT [dbo].[LookupCollarManufacturers] ([CollarManufacturer], [Name], [Website], [Description]) VALUES (N'Telonics', N'Telonics, Inc.', N'http://www.telonics.com', N'932 E. Impala Avenue Mesa, AZ, 85204-6699 USA Tel: 480-892-4444 FAX: 480-892-9139')
INSERT [dbo].[LookupCollarManufacturers] ([CollarManufacturer], [Name], [Website], [Description]) VALUES (N'Vectronic', N'Vectronic Aerospace GmbH', N'https://www.vectronic-aerospace.com/', N'Carl-Scheele-Str. 12, 12489 Berlin Germany; Tel: +49 30 6789 4990; Email: wildlife@vectronic-aerospace.com')
INSERT [dbo].[LookupCollarManufacturers] ([CollarManufacturer], [Name], [Website], [Description]) VALUES (N'VHF', N'Any VHF collar manufacturer', NULL, NULL)
GO
INSERT [dbo].[LookupCollarParameterFileFormats] ([Code], [CollarManufacturer], [Name], [Description]) VALUES (N'A', N'Telonics', N'Telonics Parameter File', N'Telonics Parameter File (*.tpf) for Gen4 GPS/Argos Collars')
INSERT [dbo].[LookupCollarParameterFileFormats] ([Code], [CollarManufacturer], [Name], [Description]) VALUES (N'B', N'Telonics', N'Telonics PTT Properties File', N'Telonics PTT Properties File (*.ppf) - for Gen3 GPS/Argos Collars')
GO
INSERT [dbo].[LookupCollarSensors] ([CollarManufacturer], [Code], [Name], [Description]) VALUES (N'Vectronic', N'act', N'Activity', NULL)
INSERT [dbo].[LookupCollarSensors] ([CollarManufacturer], [Code], [Name], [Description]) VALUES (N'Vectronic', N'gps', N'Position', NULL)
INSERT [dbo].[LookupCollarSensors] ([CollarManufacturer], [Code], [Name], [Description]) VALUES (N'Vectronic', N'mit', N'Mortality Implant', NULL)
INSERT [dbo].[LookupCollarSensors] ([CollarManufacturer], [Code], [Name], [Description]) VALUES (N'Vectronic', N'mor', N'Mortality', NULL)
INSERT [dbo].[LookupCollarSensors] ([CollarManufacturer], [Code], [Name], [Description]) VALUES (N'Vectronic', N'prx', N'Proximity', NULL)
INSERT [dbo].[LookupCollarSensors] ([CollarManufacturer], [Code], [Name], [Description]) VALUES (N'Vectronic', N'sep', N'Separation', NULL)
INSERT [dbo].[LookupCollarSensors] ([CollarManufacturer], [Code], [Name], [Description]) VALUES (N'Vectronic', N'trap', N'Trap Event', NULL)
INSERT [dbo].[LookupCollarSensors] ([CollarManufacturer], [Code], [Name], [Description]) VALUES (N'Vectronic', N'vit', N' Vaginal Implant', NULL)
GO
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'A', N'Telonics', N'Telonics Gen3 Store On Board', N'This is the output file from the Telonic Data Download Utility software for Gen3 collar download.', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'B', N'Telonics', N'Ed Debevek Sub File', N'This is a portion of a ''B'' file for a single collar', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'C', N'Telonics', N'Telonics Gen4 Output', N'This is the output file from TDC software - either condensed or complete, and transmitted either by Argos or by collar download', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'D', N'Telonics', N'Telonics Gen3 Output', N'This is the output file from the ADC-T03 software for GPS data transmitted by Argos.', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'E', N'Telonics', N'Argos Email with Telonics Messages', N'Data from the Argos Email Service with encoded Telonics Gen3 or Gen4 messages', N'Y', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'F', N'Telonics', N'Argos WebsService Download', N'Data from the Argos Web Service in CSV format with encoded Telonics Gen3 or Gen4 messages', N'Y', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'G', N'Telonics', N'Ed Debevek''s File Format', N'This is the csv version of the output from Ed Debevek''s website', N'Y', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'H', N'Telonics', N'Telonics Gen4 Store On Board', N'This is the datalog file stored on the Gen4 collars.  It contains the collar parameters, and can be batch processed with TDC to create a format ''C'' file', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'I', N'Telonics', N'Telonics Iridium Mail Download', N'This is the decoded file format that Telonics creates when downloading email messages from the Iridium system', N'N', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'J', N'VHF', N'Raw VHF Data', N'This is a file contains raw VHF data file in an unspecified and highly variable format', N'N', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'K', N'Telonics', N'Old Argos Only Email', N'Data from Argos - collated emails for Argos only collars (Archive only, not processed by database)', N'N', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'L', N'Telonics', N'Old Argos Only CSV', N'Data parsed from format K and used to create format F, archive only, not processed by database, ', N'N', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'M', N'Televilt', N'Televilt Summary', N'Data provided by biologists for old collars (not a current data file format)', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'N', N'Lotek', N'Lotek Webservice Iridium Download', N'Lotek/Iridium GPS Location data downloaded as CSV from https://webservice.lotek.com', N'N', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'O', N'Vectronic', N'Vectronic Location Download', N'Vectronic location data downloaded via the HTTP Wildlife API v2', N'N', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'P', N'Vectronic', N'Vectronic Activity Download', N'Vectronic activity data downloaded via the HTTP Wildlife API v2', N'N', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'Q', N'Vectronic', N'Vectronic Mortality Download', N'Vectronic mortality data downloaded via the HTTP Wildlife API v2', N'N', N'N')
GO
INSERT [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel]) VALUES (N'Lotek', N'GPS8000')
INSERT [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel]) VALUES (N'Televilt', N'Unknown')
INSERT [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel]) VALUES (N'Telonics', N'Gen3')
INSERT [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel]) VALUES (N'Telonics', N'Gen4')
INSERT [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel]) VALUES (N'Vectronic', N'300')
GO
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'"programNumber";"platformId";"platformType";"platformModel";"platformName";"platformHexId";"satellite";"bestMsgDate";"duration";"nbMessage";"message120";"bestLevel";"frequency";"locationDate";"latitude";"longitude";"altitude";"locationClass";"gpsSpeed";"gpsHeading";"latitude2";"longitude2";"altitude2";"index";"nopc";"errorRadius";"semiMajor";"semiMinor";"orientation";"hdop";"bestDate";"compression";"type";"alarm";"concatenated";"date";"level"', N'F', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'#fileTypeRecord,fileType', N'H', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'[{"idActivity":', N'P', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'[{"idMortality":', N'Q', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'[{"idPosition":', N'O', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'CollarID,', N'G', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'DATE,TIME,LAT,LON,PDOP,FIX', N'M', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Device ID,Date & Time [GMT],Date & Time [Local],Latitude,Longitude,Altitude,Fix Status,DOP,Temp [C],Main [V],Back [V]', N'N', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'EmailAddress,EmailUID,Imei,MessageTime,StatusCode,StatusString,Latitude,Longitude,CEPRadius,MessageLength,MessageBytes', N'I', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Fix #	Date	Time	Fix Status	Status Text	Velocity East(m/s)	Velocity North(m/s)	Velocity Up(m/s)	Latitude	Longitude	Altitude(m)	PDOP	HDOP	VDOP	TDOP	Temperature Sensor(deg.)	Activity Sensor	Satellite Data', N'A', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Fix #,Date,Time,Fix Status,Status Text,Velocity East(m/s),Velocity North(m/s),Velocity Up(m/s),Latitude,Longitude,Altitude(m),PDOP,HDOP,VDOP,TDOP,Temperature Sensor(deg.),Activity Sensor,Satellite Data,', N'A', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Id,Date,LC,IQ,Lat1,Lon1,Lat2,Lon2,Nb mes,Nb mes>-120dB,Best level,Pass duration,NOPC,Calcul freq,Altitude', N'L', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'No,GMT_Date,GMT_Time,LMT_Date,LMT_Time,ECEF_X,ECEF_Y,ECEF_Z,Latitude,Longitude,Height,DOP,Nav,Validated,Sats,Sat,C/N,Sat,C/N,Sat,C/N,Sat,C/N,Sat,C/N,Sat,C/N,Sat,C/N,Sat,C/N,Sat,C/N,Sat,C/N,Sat,C/N,Sat,C/N,Main,Bkup,Temp,Longitude,Latitude,Collar_ID,Remarks', N'N', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'PlatformId,', N'B', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Raw VHF Data', N'J', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Received:  from gemini.argosinc.com', N'K', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Telonics Data Report', N'C', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'TXDate,TXTime,PTTID,FixNum,FixQual,FixDate,FixTime,Longitude,Latitude', N'D', N'^20([0-9]{2}).[0,1][0-9].[0-3][0-9],[0-2][0-9]:[0-5][0-9]:[0-5][0-9],')
GO
INSERT [dbo].[LookupFileStatus] ([Code], [Name], [Description]) VALUES (N'A', N'Active', N'File is archived in the database and used to create movement vectors')
INSERT [dbo].[LookupFileStatus] ([Code], [Name], [Description]) VALUES (N'I', N'Inactive', N'File is archived in the database, but not used in calculating movement vectors')
GO
INSERT [dbo].[LookupGender] ([Sex]) VALUES (N'Female')
INSERT [dbo].[LookupGender] ([Sex]) VALUES (N'Male')
INSERT [dbo].[LookupGender] ([Sex]) VALUES (N'Unknown')
GO
INSERT [dbo].[LookupHomeRangeExclusionCodes] ([Code], [Description]) VALUES (1, N'Foray')
INSERT [dbo].[LookupHomeRangeExclusionCodes] ([Code], [Description]) VALUES (2, N'Dispersal/Pre-dispersal')
INSERT [dbo].[LookupHomeRangeExclusionCodes] ([Code], [Description]) VALUES (3, N'Duplicate Location')
INSERT [dbo].[LookupHomeRangeExclusionCodes] ([Code], [Description]) VALUES (4, N'Removed from Analysis')
INSERT [dbo].[LookupHomeRangeExclusionCodes] ([Code], [Description]) VALUES (5, N'Single Wandering Wolf (not dispersing/not a pack foray)')
INSERT [dbo].[LookupHomeRangeExclusionCodes] ([Code], [Description]) VALUES (6, N'Pack name change needed')
GO
INSERT [dbo].[LookupQueryLayerServers] ([Location], [Connection], [Database]) VALUES (N'AKRO', N'INPAKROVMAIS', N'Animal_Movement')
GO
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Bear')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Caribou')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Coyote')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Moose')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Muskox')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Sheep')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Wolf')
GO
