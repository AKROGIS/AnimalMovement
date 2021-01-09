/****** Script for SelectTopNRows command from SSMS  ******/
use master
go
SELECT P2.[name], P1.permission_name
  FROM [master].[sys].[server_permissions] AS P1
 INNER JOIN [master].[sys].[server_principals] AS P2
 ON P1.grantee_principal_id = P2.[principal_id]
 WHERE P2.Name not like '##%'
 order by p1.permission_name
GO