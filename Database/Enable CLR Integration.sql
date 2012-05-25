-- http://msdn.microsoft.com/en-us/library/w2kae45k(v=VS.90).aspx
-- http://msdn.microsoft.com/en-us/library/ms131048(v=SQL.100).aspx
sp_configure 'clr enabled', 1
GO
RECONFIGURE
GO