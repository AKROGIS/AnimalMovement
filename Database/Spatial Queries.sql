--------- Movements with Gen4 Temperature
   SELECT M.*, G1A.Temperature AS TemperatureStart, G2A.Temperature AS TemperatureEnd
     FROM Movements as M
     JOIN Locations AS L1 ON L1.ProjectId = M.ProjectId AND L1.AnimalId = M.AnimalId AND L1.FixDate = M.StartDate
     JOIN Locations AS L2 ON L2.ProjectId = M.ProjectId AND L2.AnimalId = M.AnimalId AND L2.FixDate = M.EndDate
     JOIN CollarFixes AS F1 ON L1.FixId = F1.FixId
     JOIN CollarFixes AS F2 ON L2.FixId = F2.FixId
LEFT JOIN CollarDataTelonicsGen4 AS G1 ON G1.FileId = F1.FileId AND G1.LineNumber = F1.LineNumber
LEFT JOIN CollarDataTelonicsGen4 AS G1A ON G1A.FileId = G1.FileId AND G1A.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1 AND G1A.AcquisitionStartTime = G1.AcquisitionStartTime AND G1A.Temperature IS NOT NULL
LEFT JOIN CollarDataTelonicsGen4 AS G2 ON G2.FileId = F2.FileId AND G2.LineNumber = F2.LineNumber
LEFT JOIN CollarDataTelonicsGen4 AS G2A ON G2A.FileId = G2.FileId AND G1A.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1 AND G2A.AcquisitionStartTime = G2.AcquisitionStartTime AND G2A.Temperature IS NOT NULL
    WHERE M.ProjectId = 'WACH'

--------- Locations with Gen4 Temperature    
   SELECT L.*, G2.Temperature
     FROM Locations as L
     JOIN CollarFixes AS F ON F.FixId = L.FixId
LEFT JOIN CollarDataTelonicsGen4 AS G1 ON G1.FileId = F.FileId AND G1.LineNumber = F.LineNumber
LEFT JOIN CollarDataTelonicsGen4 AS G2 ON G2.FileId = G1.FileId AND G2.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1 AND G2.AcquisitionStartTime = G1.AcquisitionStartTime AND G2.Temperature IS NOT NULL
    WHERE [Status] IS NULL AND L.ProjectId = 'WACH'

--------- Locations with Gen4 Activity Count
   SELECT L.*, G2.ActivityCount
     FROM Locations as L
     JOIN CollarFixes AS F ON F.FixId = L.FixId
LEFT JOIN CollarDataTelonicsGen4 AS G1 ON G1.FileId = F.FileId AND G1.LineNumber = F.LineNumber
LEFT JOIN CollarDataTelonicsGen4 AS G2 ON G2.FileId = G1.FileId AND G2.LineNumber BETWEEN G1.LineNumber -1 AND G1.LineNumber + 1 AND G2.AcquisitionStartTime = G1.AcquisitionStartTime AND G2.ActivityCount IS NOT NULL
    WHERE [Status] IS NULL AND L.ProjectId = 'WACH'