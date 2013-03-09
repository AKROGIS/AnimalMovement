USE [Animal_Movement]
GO

CREATE ROLE [ArgosProcessor] AUTHORIZATION [dbo]
GO


USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ArgosFilePlatformDates](
	[FileId] [int] NOT NULL,
	[PlatformId] [varchar](8) NOT NULL,
	[ProgramId] [varchar](8) NOT NULL,
	[FirstTransmission] [datetime2](7) NOT NULL,
	[LastTransmission] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ArgosFilePlatformDates] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[PlatformId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

GRANT INSERT ON [dbo].[ArgosFilePlatformDates] TO [ArgosProcessor] AS [dbo]
GO

ALTER TABLE [dbo].[ArgosFilePlatformDates]  WITH CHECK ADD  CONSTRAINT [FK_ArgosFilePlatformDates_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ArgosFilePlatformDates] CHECK CONSTRAINT [FK_ArgosFilePlatformDates_CollarFiles]
GO





USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ArgosFileProcessingIssues](
	[IssueId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FileId] [int] NOT NULL,
	[Issue] [nvarchar](max) NOT NULL,
	[PlatformId] [varchar](8) NULL,
	[CollarManufacturer] [varchar](16) NULL,
	[CollarId] [varchar](16) NULL,
 CONSTRAINT [PK_ArgosFileProcessingIssues] PRIMARY KEY CLUSTERED 
(
	[IssueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [Animal_Movement]
CREATE NONCLUSTERED INDEX [IX_ArgosFileProcessingIssues_CollarFiles] ON [dbo].[ArgosFileProcessingIssues] 
(
	[FileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [Animal_Movement]
CREATE NONCLUSTERED INDEX [IX_ArgosFileProcessingIssues_Collars] ON [dbo].[ArgosFileProcessingIssues] 
(
	[CollarManufacturer] ASC,
	[CollarId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [Animal_Movement]
CREATE NONCLUSTERED INDEX [IX_ArgosFileProcessingIssues_PlatformId] ON [dbo].[ArgosFileProcessingIssues] 
(
	[PlatformId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

GRANT DELETE ON [dbo].[ArgosFileProcessingIssues] TO [ArgosProcessor] AS [dbo]
GO

GRANT INSERT ON [dbo].[ArgosFileProcessingIssues] TO [ArgosProcessor] AS [dbo]
GO

ALTER TABLE [dbo].[ArgosFileProcessingIssues]  WITH CHECK ADD  CONSTRAINT [FK_ArgosFileProcessingIssues_ArgosPlatforms] FOREIGN KEY([PlatformId])
REFERENCES [dbo].[ArgosPlatforms] ([PlatformId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ArgosFileProcessingIssues] CHECK CONSTRAINT [FK_ArgosFileProcessingIssues_ArgosPlatforms]
GO

ALTER TABLE [dbo].[ArgosFileProcessingIssues]  WITH CHECK ADD  CONSTRAINT [FK_ArgosFileProcessingIssues_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ArgosFileProcessingIssues] CHECK CONSTRAINT [FK_ArgosFileProcessingIssues_CollarFiles]
GO

ALTER TABLE [dbo].[ArgosFileProcessingIssues]  WITH CHECK ADD  CONSTRAINT [FK_ArgosFileProcessingIssues_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ArgosFileProcessingIssues] CHECK CONSTRAINT [FK_ArgosFileProcessingIssues_Collars]
GO



