use IndTask
----------------------------------------------------------------------------------------------------------------------------------------------------------
create view AccountSelect AS
SELECT
	account_id,
	account_login,
	account_password,
	account_nickname,
	account_email,
	account_country,
	account_balance,
	account_birthdate,
	account_role,
	account_image
FROM Account 
Where ((Account.account_id = (SELECT account_id FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname())))
			AND (SELECT account_role FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname()))) = 1)) 
		OR ((Account.account_role < 2)
			AND (SELECT account_role FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname()))) = 0) 			
----------------------------------------------------------------------------------------------------------------------------------------------------------
create view AccountSelectForReport AS
SELECT
	account_id,	
	account_nickname,	
	account_country,	
	account_birthdate,
	account_image
FROM Account Where Account.account_role=1 and Account.account_id <> (SELECT account_id FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname())))
----------------------------------------------------------------------------------------------------------------------------------------------------------
create view ReportSelect AS
SELECT
	Report.report_id,
	Report.report_comment,
	Report.report_recieve_date,	
	Report.report_receiver_id,	
	Report.report_sender_id,	
	Account.account_nickname,
	Account.account_image
FROM Report 
	JOIN Account ON Account.account_id = Report.report_receiver_id 
WHERE
	((Report.report_sender_id = (SELECT account_id FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname())))
		AND (SELECT account_role FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname()))) = 1)) 
	OR ((Report.report_id > 0)
		AND (SELECT account_role FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname()))) = 0) 

drop view ReportSelect
select * from ReportSelect
----------------------------------------------------------------------------------------------------------------------------------------------------------
create view Receive_achievementsSelect AS
SELECT
	Receive_achievements.account_id,
	Account.account_login,
	Receive_achievements.achievement_id,
	Achievement.achievement_name,
	Achievement.achievement_description,
	Achievement.achievement_game_id,
	Game.game_name,
	Achievement.achievement_image,
	Receive_achievements.receive_achievement_date
FROM Receive_achievements
	JOIN Achievement ON Receive_achievements.achievement_id = Achievement.achievement_id
	JOIN Game ON Achievement.achievement_game_id = Game.game_id	
	JOIN Account On Receive_achievements.account_id = Account.account_id
WHERE 
	((Receive_achievements.account_id = (SELECT account_id FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname())))
		AND (SELECT account_role FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname()))) = 1)) 
	OR ((Achievement.achievement_id > 0)
		AND (SELECT account_role FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname()))) = 0) 

drop view Receive_achievementsSelect
select * from Receive_achievementsSelect
----------------------------------------------------------------------------------------------------------------------------------------------------------
create view AchievementSelect AS
SELECT
	Achievement.achievement_id,
	Achievement.achievement_name,
	Achievement.achievement_game_id,
	Game.game_name,
	Achievement.achievement_image
FROM Achievement	
	JOIN Game ON Achievement.achievement_game_id = Game.game_id	 	

drop view AchievementSelect
select * from AchievementSelect
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE VIEW GamesLibrary AS
SELECT
    Purchases.purchase_id,
	Purchases.purchaser_id,
	Purchases.purchase_game_id,
	Purchases.purshase_price,
	Purchases.purshase_date,
	Game.game_name,
	Game.game_description,
	Game.game_genre,
	Game.game_release_date,
	Game.game_developer,
	Game.game_publisher,
	Game.game_rating,
	Game.game_agelimit,
	Game.game_image
FROM
	Purchases	
	JOIN Game ON Purchases.purchase_game_id = Game.game_id and Purchases.purchaser_id = (SELECT account_id FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname())))

Select * from GamesLibrary
drop view GamesLibrary
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE VIEW GamesStore AS
SELECT
    Games_shop.game_price,
	Game.game_id,
	Game.game_name,
	Game.game_description,
	Game.game_genre,
	Game.game_release_date,
	Game.game_developer,
	Game.game_publisher,
	Game.game_rating,
	Game.game_agelimit,
	Game.game_image,
	Game.game_storeimage
FROM
	Games_shop	
	JOIN Game ON Games_shop.game_id = Game.game_id

Select * from GamesStore
drop view GamesStore
----------------------------------------------------------------------------------------------------------------------------------------------------------