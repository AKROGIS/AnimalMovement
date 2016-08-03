USE [Animal_Movement]
GO
INSERT [dbo].[LookupCollarManufacturers] ([CollarManufacturer], [Name], [Website], [Description]) VALUES (N'Lotek', N'Lotek Wireless Inc. ', N'http://www.lotek.com/', N'
115 Pony Drive 
Newmarket, Ontario
Canada L3Y 7B5; 

Telephone: 905-836-6680')
INSERT [dbo].[LookupCollarManufacturers] ([CollarManufacturer], [Name], [Website], [Description]) VALUES (N'Televilt', N'Tellus GPS systems', N'http://wildlife.followit.se', N'Followit AB, Bandygatan 2, 711 34 Lindesberg; Tel: +46 (0)581-171 90; Fax: +46 (0)581-171 96; E-mail: wildlifesales@followit.se')
INSERT [dbo].[LookupCollarManufacturers] ([CollarManufacturer], [Name], [Website], [Description]) VALUES (N'Telonics', N'Telonics, Inc.', N'http://www.telonics.com', N'932 E. Impala Avenue Mesa, AZ, 85204-6699 USA Tel: 480-892-4444 FAX: 480-892-9139')
INSERT [dbo].[LookupCollarParameterFileFormats] ([Code], [CollarManufacturer], [Name], [Description]) VALUES (N'A', N'Telonics', N'Telonics Parameter File', N'Telonics Parameter File (*.tpf) for Gen4 GPS/Argos Collars')
INSERT [dbo].[LookupCollarParameterFileFormats] ([Code], [CollarManufacturer], [Name], [Description]) VALUES (N'B', N'Telonics', N'Telonics PTT Properties File', N'Telonics PTT Properties File (*.ppf) - for Gen3 GPS/Argos Collars')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'A', N'Telonics', N'Telonics Gen3 Store On Board', N'This is the output file from the Telonic Data Download Utility software for Gen3 collar download.', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'B', N'Telonics', N'Ed Debevek Sub File', N'This is a portion of a ''B'' file for a single collar', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'C', N'Telonics', N'Telonics Gen4 Output', N'This is the output file from TDC software - either condensed or complete, and transmitted either by Argos or by collar download', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'D', N'Telonics', N'Telonics Gen3 Output', N'This is the output file from the ADC-T03 software for GPS data transmitted by Argos.', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'E', N'Telonics', N'Argos Email with Telonics Messages', N'Data from the Argos Email Service with encoded Telonics Gen3 or Gen4 messages', N'Y', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'F', N'Telonics', N'Argos WebsService Download', N'Data from the Argos Web Service in CSV format with encoded Telonics Gen3 or Gen4 messages', N'Y', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'G', N'Telonics', N'Ed Debevek''s File Format', N'This is the csv version of the output from Ed Debevek''s website', N'Y', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'H', N'Telonics', N'Telonics Gen4 Store On Board', N'This is the datalog file stored on the Gen4 collars.  It contains the collar parameters, and can be batch processed with TDC to create a format ''C'' file', N'N', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [ArgosData], [RequiresCollar]) VALUES (N'I', N'Telonics', N'Telonics Iridium Mail Download', N'This is the decoded file format that Telonics creates when downloading email messages from the Iridium system', N'N', N'N')
INSERT [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel]) VALUES (N'Lotek', N'GPS8000')
INSERT [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel]) VALUES (N'Televilt', N'Unknown')
INSERT [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel]) VALUES (N'Telonics', N'Gen3')
INSERT [dbo].[LookupCollarModels] ([CollarManufacturer], [CollarModel]) VALUES (N'Telonics', N'Gen4')
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'"programNumber";"platformId";"platformType";"platformModel";"platformName";"platformHexId";"satellite";"bestMsgDate";"duration";"nbMessage";"message120";"bestLevel";"frequency";"locationDate";"latitude";"longitude";"altitude";"locationClass";"gpsSpeed";"gpsHeading";"latitude2";"longitude2";"altitude2";"index";"nopc";"errorRadius";"semiMajor";"semiMinor";"orientation";"hdop";"bestDate";"compression";"type";"alarm";"concatenated";"date";"level"', N'F', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'#fileTypeRecord,fileType', N'H', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'CollarID,', N'G', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'EmailAddress,EmailUID,Imei,MessageTime,StatusCode,StatusString,Latitude,Longitude,CEPRadius,MessageLength,MessageBytes', N'I', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Fix #	Date	Time	Fix Status	Status Text	Velocity East(m/s)	Velocity North(m/s)	Velocity Up(m/s)	Latitude	Longitude	Altitude(m)	PDOP	HDOP	VDOP	TDOP	Temperature Sensor(deg.)	Activity Sensor	Satellite Data', N'A', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Fix #,Date,Time,Fix Status,Status Text,Velocity East(m/s),Velocity North(m/s),Velocity Up(m/s),Latitude,Longitude,Altitude(m),PDOP,HDOP,VDOP,TDOP,Temperature Sensor(deg.),Activity Sensor,Satellite Data,', N'A', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'PlatformId,', N'B', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'Telonics Data Report', N'C', NULL)
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat], [Regex]) VALUES (N'TXDate,TXTime,PTTID,FixNum,FixQual,FixDate,FixTime,Longitude,Latitude', N'D', N'^20([0-9]{2}).[0,1][0-9].[0-3][0-9],[0-2][0-9]:[0-5][0-9]:[0-5][0-9],')
INSERT [dbo].[LookupFileStatus] ([Code], [Name], [Description]) VALUES (N'A', N'Active', N'File is archived in the database and used to create movement vectors')
INSERT [dbo].[LookupFileStatus] ([Code], [Name], [Description]) VALUES (N'I', N'Inactive', N'File is archived in the database, but not used in calculating movement vectors')
INSERT [dbo].[LookupGender] ([Sex]) VALUES (N'Female')
INSERT [dbo].[LookupGender] ([Sex]) VALUES (N'Male')
INSERT [dbo].[LookupGender] ([Sex]) VALUES (N'Unknown')
INSERT [dbo].[LookupQueryLayerServers] ([Location], [Connection], [Database]) VALUES (N'AKRO', N'INPAKROVMAIS', N'Animal_Movement')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Bear')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Caribou')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Coyote')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Moose')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Muskox')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Sheep')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Wolf')
