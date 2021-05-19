-- Displays the Iridium message time (in UTC) to local time
-- #################################################################
-- This is useful for comparison with emails filtered by date. Useful only on
-- gmail.  Outlook filters emails based on UTC delivery time which is very
-- close to the messageTime
-

select MessageTime, Imei from CollarDataIridiumMail where dateadd(hour, -8, convert(datetime2, MessageTime)) <
'2020-07-04' and dateadd(hour, -8, convert(datetime2, MessageTime)) >
'2020-07-03'
order by MessageTime desc
--, dateadd(hour, -8, convert(datetime2, MessageTime)) as mdate
