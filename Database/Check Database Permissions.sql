/****** Script for SelectTopNRows command from SSMS  ******/
use Animal_Movement
go
-- permissions by role
SELECT P2.[name], P1.state_desc, P1.permission_name, O.name
  FROM [sys].[database_permissions] AS P1
 INNER JOIN [sys].[database_principals] AS P2
 ON P1.grantee_principal_id = P2.[principal_id]
 INNER JOIN [sys].[all_objects] AS O
 ON O.[object_id] = P1.major_id
 WHERE P2.Name <> 'public' and P2.Name <> 'guest'
 order by P2.Name, O.Name -- p1.permission_name
GO

--Permissions by table
select T.name, o.name, p.permission_name
from sys.tables as T
left join sys.database_permissions as P
on p.major_id = T.object_id
left join sys.database_principals as o
on o.principal_id = p.grantee_principal_id 
where T.is_ms_shipped = 0
order by t.name

--permission by view
select T.name, o.name, p.permission_name
from sys.views as T
left join sys.database_permissions as P
on p.major_id = T.object_id
left join sys.database_principals as o
on o.principal_id = p.grantee_principal_id 
where T.is_ms_shipped = 0
order by t.name