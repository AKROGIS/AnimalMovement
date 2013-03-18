USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ArgosDownloads](
	[TimeStamp] [datetime2](7) NOT NULL,
	[ProgramId] [varchar](8) NULL,
	[PlatformId] [varchar](8) NULL,
	[Days] [int] NULL,
	[FileId] [int] NULL,
	[ErrorMessage] [varchar](max) NULL,
 CONSTRAINT [PK_ArgosDownloads] PRIMARY KEY CLUSTERED 
(
	[TimeStamp] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [Animal_Movement]
CREATE NONCLUSTERED INDEX [IX_ArgosDownloads_PlatformId] ON [dbo].[ArgosDownloads] 
(
	[PlatformId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [Animal_Movement]
CREATE NONCLUSTERED INDEX [IX_ArgosDownloads_ProgamId] ON [dbo].[ArgosDownloads] 
(
	[ProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ArgosDownloads]  WITH CHECK ADD  CONSTRAINT [FK_ArgosDownloads_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
GO

ALTER TABLE [dbo].[ArgosDownloads] CHECK CONSTRAINT [FK_ArgosDownloads_CollarFiles]
GO

ALTER TABLE [dbo].[ArgosDownloads] ADD  CONSTRAINT [DF_ArgosDownloads_TimeStamp]  DEFAULT (getdate()) FOR [TimeStamp]
GO


