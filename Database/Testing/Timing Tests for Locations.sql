-- ========================================
-- Timing tests for insert/delete of locations
--   and subsequent updates to movements
--
-- To test:
--   1) Copy the first two lines to one query window,
--   2) run only the first line, to get good FixIds
--   3) modify the delete statement to use max FiXId shown
--   4) copy the remainder to another query window, and edit the fixIds
--   5) turn on show execution plan and show client statistics
--   6) run the delete query
--   7) run the insert queries
--   8) loop on 6/7 for several runs to get good statistics
--
-- Typical numbers are 9.5milliseconds for delete and 11.5 for insert
-- All recommended queries have been added.  I'm guessing the query compilation
-- takes care of removing the function/stored procedure calls.
-- Optimization strategy:
--   1) remove cursor and find set logic to update movements (unlikely)
--   2) check for need to do deletes before doing so
-- ========================================

select top 3 FixId from CollarFixes order by FixId
delete from Locations where ProjectId = 'test' AND AnimalId = '12a4' AND FixId < 30000



Insert Locations (ProjectId, AnimalId, FixDate, FixId, Location)
VALUES ('test', '12a4','2000-01-01 08:00', 27056, GEOGRAPHY::Point(-154.5, 60.5,4326))

Insert Locations (ProjectId, AnimalId, FixDate, FixId, Location)
VALUES ('test', '12a4','2000-01-01 16:00', 27057, GEOGRAPHY::Point(-154.4, 60.6,4326))

Insert Locations (ProjectId, AnimalId, FixDate, FixId, Location)
VALUES ('test', '12a4','2000-01-01 12:00', 27058, GEOGRAPHY::Point(-154.6, 60.4,4326))