use IndTask

create login u1 with password = '123', default_database = IndTask, check_policy = off, check_expiration = off
create user u1 for login u1
drop user u1
create role Player
alter role Player
add member u1

create login sysa with password = '123', default_database = IndTask, check_policy = off, check_expiration = off
exec sp_changedbowner 'sa', 'true'
create user sysa for login sysa
ALTER SERVER ROLE [sysadmin] ADD MEMBER sysa;
-----------------------------------------------------------------------------
--used tables and views
--report
grant insert on Report to Player
grant select on ReportSelect to Player
grant select on AccountSelectForReport to Player 
--profile
grant update, delete, select on AccountSelect to Player
--library
grant select on GamesLibrary to Player
--store
grant select on GamesStore to Player
grant insert on Purchases to Player
--Achievements
grant select on AchievementSelect to Player
grant select on Receive_achievementsSelect to Player
grant insert on Receive_achievements to Player

------------------------------------------------------------------------------
--tables
select * from Account
select * from Game
select * from Games_shop
select * from Report
select * from Purchases
select * from Achievement
select * from Receive_achievements
--views
select * from AccountSelect
select * from GamesLibrary
select * from GamesStore
select * from ReportSelect
select * from AccountSelectForReport
select * from Receive_achievementsSelect
SELECT CURRENT_USER; 