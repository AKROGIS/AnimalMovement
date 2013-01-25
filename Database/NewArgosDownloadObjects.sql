USE [Animal_Movement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LookupGeneralStatus](
	[Code] [char](1) NOT NULL,
	[Status] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_LookupGeneralStatus] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArgosPrograms](
	[ProgramId] [varchar](8) NOT NULL,
	[ProgramName] [nvarchar](255) NULL,
	[UserName] [sysname] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Investigator] [sysname] NOT NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[Remarks] [nvarchar](255) NULL,
 CONSTRAINT [PK_ArgosPrograms] PRIMARY KEY CLUSTERED 
(
	[ProgramId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArgosPlatforms](
	[PlatformId] [varchar](8) NOT NULL,
	[ProgramId] [varchar](8) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Remarks] [nvarchar](255) NULL,
 CONSTRAINT [PK_ArgosPlatforms] PRIMARY KEY CLUSTERED 
(
	[PlatformId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArgosDownloads](
	[CollarManufacturer] [varchar](16) NOT NULL,
	[CollarId] [varchar](16) NOT NULL,
	[TimeStamp] [datetime2](7) NOT NULL,
	[FileId] [int] NULL,
	[ErrorMessage] [varchar](255) NULL,
 CONSTRAINT [PK_ArgosDownloads] PRIMARY KEY CLUSTERED 
(
	[CollarManufacturer] ASC,
	[CollarId] ASC,
	[TimeStamp] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[DownloadableCollars]
AS

SELECT CD.ProjectId, C.CollarManufacturer, C.CollarId
      ,I.Email, P.[UserName], P.[Password], A.PlatformId
      ,datediff(day,D.[TimeStamp],getdate()) AS [Days]
  FROM
	           ArgosPlatforms AS A
	INNER JOIN ArgosPrograms AS P
	        ON A.ProgramId = P.ProgramId
	INNER JOIN ProjectInvestigators AS I
	        ON I.Login = P.Investigator
	INNER JOIN Collars AS C
	        ON C.AlternativeId = A.PlatformId
    INNER JOIN CollarDeployments as CD
            ON C.CollarManufacturer = CD.CollarManufacturer AND C.CollarId = CD.CollarId
     LEFT JOIN (
               SELECT CollarManufacturer, CollarId, Max([Timestamp]) AS [Timestamp]
                 FROM ArgosDownloads
                WHERE ErrorMessage IS NULL
                GROUP BY CollarManufacturer, CollarId
               ) AS D
            ON C.CollarManufacturer = D.CollarManufacturer AND C.CollarId = D.CollarId

 WHERE A.[Status] = 'A'
   AND P.[Status] = 'A'
   AND (P.EndDate IS NULL OR getdate() < P.EndDate)
   AND (C.DisposalDate IS NULL OR getdate() < C.DisposalDate)
   AND (CD.RetrievalDate IS NULL OR getdate() < CD.RetrievalDate)
GO
ALTER TABLE [dbo].[ArgosDownloads]  WITH CHECK ADD  CONSTRAINT [FK_ArgosDownloads_CollarFiles] FOREIGN KEY([FileId])
REFERENCES [dbo].[CollarFiles] ([FileId])
GO
ALTER TABLE [dbo].[ArgosDownloads] CHECK CONSTRAINT [FK_ArgosDownloads_CollarFiles]
GO
ALTER TABLE [dbo].[ArgosDownloads]  WITH CHECK ADD  CONSTRAINT [FK_ArgosDownloads_Collars] FOREIGN KEY([CollarManufacturer], [CollarId])
REFERENCES [dbo].[Collars] ([CollarManufacturer], [CollarId])
GO
ALTER TABLE [dbo].[ArgosDownloads] CHECK CONSTRAINT [FK_ArgosDownloads_Collars]
GO
ALTER TABLE [dbo].[ArgosPlatforms]  WITH CHECK ADD  CONSTRAINT [FK_ArgosPlatforms_ArgosPrograms] FOREIGN KEY([ProgramId])
REFERENCES [dbo].[ArgosPrograms] ([ProgramId])
GO
ALTER TABLE [dbo].[ArgosPlatforms] CHECK CONSTRAINT [FK_ArgosPlatforms_ArgosPrograms]
GO
ALTER TABLE [dbo].[ArgosPlatforms]  WITH CHECK ADD  CONSTRAINT [FK_ArgosPlatforms_LookupGeneralStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[LookupGeneralStatus] ([Code])
GO
ALTER TABLE [dbo].[ArgosPlatforms] CHECK CONSTRAINT [FK_ArgosPlatforms_LookupGeneralStatus]
GO
ALTER TABLE [dbo].[ArgosPrograms]  WITH CHECK ADD  CONSTRAINT [FK_ArgosPrograms_LookupGeneralStatus] FOREIGN KEY([Status])
REFERENCES [dbo].[LookupGeneralStatus] ([Code])
GO
ALTER TABLE [dbo].[ArgosPrograms] CHECK CONSTRAINT [FK_ArgosPrograms_LookupGeneralStatus]
GO
ALTER TABLE [dbo].[ArgosPrograms]  WITH CHECK ADD  CONSTRAINT [FK_ArgosPrograms_ProjectInvestigators] FOREIGN KEY([Investigator])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
GO
ALTER TABLE [dbo].[ArgosPrograms] CHECK CONSTRAINT [FK_ArgosPrograms_ProjectInvestigators]
GO
