USE [Animal_Movement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE FUNCTION [dbo].[IsInvestigatorEditor] 
(
    @ProjectInvestigator sysname, 
    @User                sysname
)
RETURNS BIT
AS
BEGIN
    -- Check that @User is in the Investigator or the Editor Role
    IF NOT EXISTS( SELECT 1 FROM sys.sysmembers WHERE (USER_NAME(groupuid) = 'Editor' AND USER_NAME(memberuid) = @user)
                                                   OR (USER_NAME(groupuid) = 'Investigator' AND USER_NAME(memberuid) = @user))
        RETURN 0
    
    --Check that @User is either an Assistant or the PI
    IF EXISTS(     SELECT 1 FROM dbo.ProjectInvestigators AS P 
                LEFT JOIN [dbo].[ProjectInvestigatorAssistants] AS A 
                       ON A.ProjectInvestigator = P.[Login] 
                    WHERE @User = P.[Login]
                       OR @User = A.Assistant
             )
        RETURN 1
    RETURN 0
END




GO

GRANT EXECUTE ON [dbo].[IsInvestigatorEditor] TO [Viewer] AS [dbo]
GO


