USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 25, 2013
-- Description:	Adds a new assistant for a PI
-- =============================================
CREATE PROCEDURE [dbo].[ProjectInvestigatorAssistant_Insert] 
    @ProjectInvestigator sysname,
    @Assistant           sysname
AS
BEGIN
    SET NOCOUNT ON;

    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();
    
    --If editor is already an editor on the project, then there is nothing to do, so return
    IF EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigatorAssistants] WHERE ProjectInvestigator = @ProjectInvestigator AND Assistant = @Assistant)
    BEGIN
        RETURN 0
    END
    
    -- If the caller is not the assistant's PI, then error and return
    IF @ProjectInvestigator <> @Caller
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') cannot create an assistant for another principal investigator (' + @ProjectInvestigator + ')';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END
    
    DECLARE @LoginCreated BIT = 0;
    DECLARE @UserCreated BIT = 0;
    DECLARE @RoleMemberAdded BIT = 0;
    
    BEGIN TRY
        -- If the Assistant is not a database user then add them.
        IF NOT EXISTS (select 1 from sys.database_principals WHERE type = 'U' AND name = @Assistant)
        BEGIN
            EXEC ('CREATE USER ['  + @Assistant + ']') --  LOGIN defaults to the same name
            SET @UserCreated = 1
        END
            
        -- If the Assistant does not have the editor role, then add it.
        IF NOT EXISTS (SELECT 1 from sys.database_role_members as U 
                       INNER JOIN sys.database_principals AS P1  
                       ON U.member_principal_id = P1.principal_id
                       INNER JOIN sys.database_principals AS P2 
                       ON U.role_principal_id = p2.principal_id 
                       WHERE p1.name = @Assistant AND p2.name = 'Editor' )
        BEGIN
            --EXEC ('ALTER ROLE Editor ADD MEMBER ['  + @Editor + ']')  --SQL2012 syntax
            EXEC sp_addrolemember 'Editor', @Assistant
            SET @RoleMemberAdded = 1
        END
            
        -- Add them to the Project Editors table
        INSERT [dbo].[ProjectInvestigatorAssistants] (ProjectInvestigator, Assistant) VALUES (@ProjectInvestigator, @Assistant)
    END TRY
    BEGIN CATCH
        -- Roleback operations
        IF @RoleMemberAdded = 1
        BEGIN
            EXEC sp_droprolemember 'Editor', @Assistant
        END
        IF @UserCreated = 1
        BEGIN
            EXEC ('DROP USER ['  + @Assistant + ']')
        END
        EXEC [dbo].[Utility_RethrowError]
        RETURN 1
    END CATCH
END


GO

GRANT EXECUTE ON [dbo].[ProjectInvestigatorAssistant_Insert] TO [Investigator] AS [dbo]
GO


USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Regan Sarwas
-- Create date: April 25, 2013
-- Description:	Removes an Assistant from a PI
-- =============================================
CREATE PROCEDURE [dbo].[ProjectInvestigatorAssistant_Delete] 
    @ProjectInvestigator sysname,
    @Assistant           sysname
AS
BEGIN
    SET NOCOUNT ON;

    -- Permissions only allow Investigators to execute this procedure
    -- The Invesitigator role has Alter Any User, and Alter Role 'Editor' permissions

    --If this isn't a valid assistant, then there is nothing to do, so return
    IF NOT EXISTS(SELECT 1 FROM [dbo].[ProjectInvestigatorAssistants] WHERE ProjectInvestigator = @ProjectInvestigator AND Assistant = @Assistant)
    BEGIN
        RETURN
    END
        
    -- Get the name of the caller
    DECLARE @Caller sysname = ORIGINAL_LOGIN();
    
    -- If the caller is not the PI for the project, then error and return
    IF @ProjectInvestigator <> @Caller
    BEGIN
        DECLARE @message1 nvarchar(100) = 'You ('+@Caller+') cannot delete the assistant for another principal investigator (' + @ProjectInvestigator + ')';
        RAISERROR(@message1, 18, 0)
        RETURN 1
    END

    -- This is the key step from the applications standpoint
    DELETE FROM [dbo].[ProjectInvestigatorAssistants] WHERE ProjectInvestigator = @ProjectInvestigator AND Assistant = @Assistant
    
END


GO

GRANT EXECUTE ON [dbo].[ProjectInvestigatorAssistant_Delete] TO [Investigator] AS [dbo]
GO
