-- Remove foreign keys to Animals Table
---------------------------------------

USE [Animal_Movement]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Movements_Animals]') AND parent_object_id = OBJECT_ID(N'[dbo].[Movements]'))
ALTER TABLE [dbo].[Movements] DROP CONSTRAINT [FK_Movements_Animals]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollarDeployments_Animals]') AND parent_object_id = OBJECT_ID(N'[dbo].[CollarDeployments]'))
ALTER TABLE [dbo].[CollarDeployments] DROP CONSTRAINT [FK_CollarDeployments_Animals]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Locations_Animals]') AND parent_object_id = OBJECT_ID(N'[dbo].[Locations]'))
ALTER TABLE [dbo].[Locations] DROP CONSTRAINT [FK_Locations_Animals]
GO



-- Rename Animals table
--------------------------------------

sp_rename 'Animals', 'OldAnimals'
GO
sp_rename 'PK_Animals', 'PK_OldAnimals'
GO
sp_rename 'FK_Animals_Gender', 'FK_OldAnimals_Gender'
GO
sp_rename 'FK_Animals_Projects', 'FK_OldAnimals_Projects'
GO
sp_rename 'FK_Animals_Species', 'FK_OldAnimals_Species'
GO



-- Create New Animals table, indexes, and constraints
-----------------------------------------------------

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Animals](
	[ProjectId] [varchar](16) NOT NULL,
	[AnimalId] [varchar](16) NOT NULL,
	[Species] [varchar](32) NULL,
	[Gender] [char](1) NULL,
	[MortalityDate] [DateTime2](7) NULL,
	[GroupName] [nvarchar](500) NULL,
	[Description] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Animals] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[AnimalId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Gender] FOREIGN KEY([Gender])
REFERENCES [dbo].[LookupGender] ([Sex])
GO

ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Gender]
GO

ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
GO

ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Projects]
GO

ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Species] FOREIGN KEY([Species])
REFERENCES [dbo].[LookupSpecies] ([Species])
GO

ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Species]
GO




-- Copy data from old to new Animals Table
------------------------------------------

INSERT INTO [Animal_Movement].[dbo].[Animals]
SELECT [ProjectId]
       ,[AnimalId]
       ,[Species]
       ,[Gender]
       ,NULL
       ,[GroupName]
       ,[Description]
FROM [Animal_Movement].[dbo].[OldAnimals]




-- Replace foreign keys to Animals Table
----------------------------------------

ALTER TABLE [dbo].[CollarDeployments]  WITH CHECK ADD  CONSTRAINT [FK_CollarDeployments_Animals] FOREIGN KEY([ProjectId], [AnimalId])
REFERENCES [dbo].[Animals] ([ProjectId], [AnimalId])
GO

ALTER TABLE [dbo].[CollarDeployments] CHECK CONSTRAINT [FK_CollarDeployments_Animals]
GO

ALTER TABLE [dbo].[Movements]  WITH CHECK ADD  CONSTRAINT [FK_Movements_Animals] FOREIGN KEY([ProjectId], [AnimalId])
REFERENCES [dbo].[Animals] ([ProjectId], [AnimalId])
GO

ALTER TABLE [dbo].[Movements] CHECK CONSTRAINT [FK_Movements_Animals]
GO

ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Animals] FOREIGN KEY([ProjectId], [AnimalId])
REFERENCES [dbo].[Animals] ([ProjectId], [AnimalId])
GO

ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Animals]
GO



-- Drop Old Animals table
--------------------------------------

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OldAnimals_Gender]') AND parent_object_id = OBJECT_ID(N'[dbo].[OldAnimals]'))
ALTER TABLE [dbo].[OldAnimals] DROP CONSTRAINT [FK_OldAnimals_Gender]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OldAnimals_Projects]') AND parent_object_id = OBJECT_ID(N'[dbo].[OldAnimals]'))
ALTER TABLE [dbo].[OldAnimals] DROP CONSTRAINT [FK_OldAnimals_Projects]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OldAnimals_Species]') AND parent_object_id = OBJECT_ID(N'[dbo].[OldAnimals]'))
ALTER TABLE [dbo].[OldAnimals] DROP CONSTRAINT [FK_OldAnimals_Species]
GO

USE [Animal_Movement]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OldAnimals]') AND type in (N'U'))
DROP TABLE [dbo].[OldAnimals]
GO




