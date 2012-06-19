USE [Animal_Movement]
GO
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Bear')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Caribou')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Moose')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Muskox')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Sheep')
INSERT [dbo].[LookupSpecies] ([Species]) VALUES (N'Wolf')
INSERT [dbo].[LookupQueryLayerServers] ([Location], [Connection]) VALUES (N'AKRO', N'INPAKROMS53AIS')
INSERT [dbo].[LookupQueryLayerServers] ([Location], [Connection]) VALUES (N'Regan''s Desktop', N'INPAKRO39088\SQL2008R2')
INSERT [dbo].[LookupQueryLayerServers] ([Location], [Connection]) VALUES (N'Regan''s Laptop', N'INPAKRO39306')
INSERT [dbo].[LookupGender] ([Sex]) VALUES (N'F')
INSERT [dbo].[LookupGender] ([Sex]) VALUES (N'M')
INSERT [dbo].[LookupGender] ([Sex]) VALUES (N'U')
INSERT [dbo].[LookupCollarModels] ([CollarModel]) VALUES (N'AdhocUpload')
INSERT [dbo].[LookupCollarModels] ([CollarModel]) VALUES (N'ArgosDoppler')
INSERT [dbo].[LookupCollarModels] ([CollarModel]) VALUES (N'ArgosGps')
INSERT [dbo].[LookupCollarModels] ([CollarModel]) VALUES (N'TelonicsGen3')
INSERT [dbo].[LookupCollarModels] ([CollarModel]) VALUES (N'TelonicsGen4')
INSERT [dbo].[LookupCollarModels] ([CollarModel]) VALUES (N'TelonicsGpsArgos')
INSERT [dbo].[LookupCollarModels] ([CollarModel]) VALUES (N'Unknown')
INSERT [dbo].[LookupCollarManufacturers] ([CollarManufacturer], [Name], [Website], [Description]) VALUES (N'Telonics', N'Telonics, Inc.', N'http://www.telonics.com', N'932 E. Impala Avenue Mesa, AZ, 85204-6699 USA Tel: 480-892-4444 FAX: 480-892-9139')
INSERT [dbo].[LookupCollarFileStatus] ([Code], [Name], [Description]) VALUES (N'A', N'Active', N'File is archived in the database and used to create movement vectors')
INSERT [dbo].[LookupCollarFileStatus] ([Code], [Name], [Description]) VALUES (N'I', N'Inactive', N'File is archived in the database, but not used in calculating movement vectors')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [TableName], [HasCollarIdColumn]) VALUES (N'A', N'Telonics', N'Telonics Store On Board', NULL, N'CollarDataTelonicsStoreOnBoard', N'N')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [TableName], [HasCollarIdColumn]) VALUES (N'B', N'Telonics', N'Ed Debevek''s File Format', NULL, N'CollarDataDebevekFormat', N'Y')
INSERT [dbo].[LookupCollarFileFormats] ([Code], [CollarManufacturer], [Name], [Description], [TableName], [HasCollarIdColumn]) VALUES (N'C', N'Telonics', N'Telonics Gen4 Condensed Output', N'This is the output file from the Telonics convertor', N'CollarDataTelonicsGen4Condensed', N'N')
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat]) VALUES (N'CollarID,', N'B')
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat]) VALUES (N'Fix #,Date,Time,Fix Status,Status Text,Velocity East(m/s),Velocity North(m/s),Velocity Up(m/s),Latitude,Longitude,Altitude(m),PDOP,HDOP,VDOP,TDOP,Temperature Sensor(deg.),Activity Sensor,Satellite Data,', N'A')
INSERT [dbo].[LookupCollarFileHeaders] ([Header], [FileFormat]) VALUES (N'Telonics Data Report', N'C')
