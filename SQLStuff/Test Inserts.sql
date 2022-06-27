use IndTask
--sysa
INSERT INTO Account(account_id, account_login, account_password, account_nickname, account_email, account_country, account_balance, account_birthdate, account_role) 
VALUES (1, 'sysa', '123', 'SystemAdministrator', NULL, NULL, 0, '11.09.2001',0);

INSERT INTO Account(account_login, account_password, account_nickname, account_birthdate) 
VALUES ('u1', '123', 'Vell','11.09.2002');

INSERT INTO Game(game_name, game_description, game_genre, game_release_date, game_developer, game_publisher, game_rating, game_agelimit) 
VALUES ('Witcher', '','Action, RPG', '19.05.2015', 'CDPR', 'CDPR', 10, 18);
INSERT INTO Game(game_name, game_genre, game_release_date, game_developer, game_publisher, game_rating, game_agelimit) 
VALUES ('Counter-Strike: Global Offencive', 'Action, Shooter', '19.05.2013', 'Valve', 'Valve', 9, 16);


delete from Games_shop where game_id=1
INSERT INTO Games_shop(game_id, game_price) 
VALUES (2, 1199);
INSERT INTO Games_shop(game_id, game_price) 
VALUES (3, 999);

update Games_shop set game_id=2 where game_id = 1

INSERT INTO Report(report_comment, report_recieve_date, report_sender_id, report_receiver_id) 
VALUES ('bad men', '24.01.2022',  2, 1);

INSERT INTO Purchases(purchase_id, purchaser_id, purchase_game_id, purshase_price, purshase_date) 
VALUES (1, 2, 1, 1000, '10.01.2022');
INSERT INTO Purchases(purchase_id, purchaser_id, purchase_game_id, purshase_price, purshase_date) 
VALUES (2, 2, 2, 699, '11.01.2022');
delete from Purchases where purchase_id=2

INSERT INTO Achievement(achievement_name, achievement_description, achievement_game_id, achievement_image) 
VALUES ('Awesome speedrun','During the passage of the game you were faster than the wind!' ,1,'E:\Универ\7 трим\БД и СУБД\практика\Задания\IndTask\speedrunImage.png');
INSERT INTO Achievement(achievement_name, achievement_description, achievement_game_id, achievement_image) 
VALUES ('Grand Witcher','For your merits, you are awarded the title of a great witcher!' ,1,'E:\Универ\7 трим\БД и СУБД\практика\Задания\IndTask\greatwitcherImage.png');

INSERT INTO Receive_achievements(achievement_id, account_id, receive_achievement_date) 
VALUES (1, 2, '08.01.2022');
INSERT INTO Receive_achievements(achievement_id, account_id, receive_achievement_date) 
VALUES (2, 2, '08.01.2022');
delete from Receive_achievements

select 
	Account.account_id,
	Account.account_nickname,
	Purchases.purchase_game_name
from Account
	join Purchases on Account.account_id=Purchases.purchaser_id
WHERE Account.account_id<=2

select
	Account.account_login,
	Achievement.achievement_name,
	Receive_achievements.receive_achievement_date
from Account
	JOIN Receive_achievements ON Receive_achievements.account_id=Account.account_id
	JOIN Achievement ON Achievement.achievement_id=Receive_achievements.achievement_id

	JOIN Account on Account.account_id=Purchases.purchaser_id
	JOIN Account on Account.account_id=Report.report_sender_id
	JOIN Account on Account.account_id=Receive_achievements.account_id

	JOIN Purchases on Purchases.purchase_game_id=Game.game_id
	JOIN Purchases on Purchases.purchaser_id=Account.account_id

    JOIN Report ON Report.report_sender_id=Account.account_id

	JOIN Receive_achievements ON Receive_achievements.achievement_id=Achievement.achievement_id
	JOIN Receive_achievements ON Receive_achievements.account_id=Account.account_id

    JOIN Achievement ON Achievement.achievement_id=Receive_achievements.achievement_id
	JOIN Achievement ON Achievement.achievement_game_id=Game.game_id

	JOIN Game ON Game.game_id=Purchases.purchase_game_id
	JOIN Game ON Game.game_id=Achievement.achievement_game_id
	
	JOIN Games_shop ON Games_shop.game_id=Game.game_id

	
	