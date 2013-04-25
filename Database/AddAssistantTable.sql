USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ProjectInvestigatorAssistants](
	[ProjectInvestigator] [sysname] NOT NULL,
	[Assistant] [sysname] NOT NULL,
 CONSTRAINT [PK_ProjectInvestigatorAssistants] PRIMARY KEY CLUSTERED 
(
	[ProjectInvestigator] ASC,
	[Assistant] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 25, 2013
-- Description:	Drops a user from the database if
--              it is no longer being referenced.
-- =============================================
CREATE TRIGGER [dbo].[AfterProjectInvestigatorAssistantDelete] 
ON [dbo].[ProjectInvestigatorAssistants] 
AFTER DELETE AS
BEGIN
	SET NOCOUNT ON;
	DECLARE
		@ProjectInvestigator sysname = NULL,
		@Assistant           sysname = NULL
	
	-- Loop over all the assistants to be deleted, and
	-- if 1) they are the not a PI, and
	--    2) they will no longer have a presense in the Assistants table,
	-- then drop them from the DB
	
	DECLARE del_cursor CURSOR FOR 
		SELECT [ProjectInvestigator], [Assistant] FROM deleted

	OPEN del_cursor;

	FETCH NEXT FROM del_cursor INTO @ProjectInvestigator, @Assistant;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- If this Assistant is a project investigator, then do nothing more
		IF EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigators] WHERE [Login] = @Assistant)
		BEGIN
			FETCH NEXT FROM del_cursor INTO @ProjectInvestigator, @Assistant;
			CONTINUE
		END

		-- If this Assistant is still an Assistant (for another project investigator after deleting), then do nothing more
		IF EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigatorAssistants] AS A
				   WHERE A.ProjectInvestigator <> @ProjectInvestigator
					 AND A.Assistant = @Assistant)
		BEGIN
			FETCH NEXT FROM del_cursor INTO @ProjectInvestigator, @Assistant;
			CONTINUE
		END
		
		-- This Assistant is not an Assistant on any projects, so remove the user from the database
		-- This will also, obviously, remove them from the Editor role
		-- If this fails, the user will still have permissions to run the stored procedures,
		--  however the stored procedures should stop them because they are not in the
		--  ProjectInvestigatorAssistants table
		IF EXISTS (SELECT 1 from sys.database_principals WHERE type = 'U' AND name = @Assistant)
			EXEC ('DROP USER ['  + @Assistant + ']') --  LOGIN defaults to the same name

		FETCH NEXT FROM del_cursor INTO @ProjectInvestigator, @Assistant;		
	END
	CLOSE del_cursor;
	DEALLOCATE del_cursor;
END



GO

ALTER TABLE [dbo].[ProjectInvestigatorAssistants]  WITH CHECK ADD  CONSTRAINT [FK_ProjectInvestigatorAssistants_ProjectInvestigators] FOREIGN KEY([ProjectInvestigator])
REFERENCES [dbo].[ProjectInvestigators] ([Login])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectInvestigatorAssistants] CHECK CONSTRAINT [FK_ProjectInvestigatorAssistants_ProjectInvestigators]
GO


