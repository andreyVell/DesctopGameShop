use IndTask
----------------------------------------------------------------------------------------------------------------------------------------------------------
create trigger delete_acc on AccountSelect
instead of DELETE as
begin
	delete from Account where account_id =(Select account_id from deleted)
	delete from AccountSelect where account_id =(Select account_id from deleted)
end
----------------------------------------------------------------------------------------------------------------------------------------------------------
create trigger updateyourself on AccountSelect
instead of UPDATE as
begin
	declare 		
		@account_id int,
		@account_login nvarchar(50),
		@account_password nvarchar(50),
		@account_nickname nvarchar(50),
		@account_email nvarchar(50),
		@account_country nvarchar(50),
		@account_balance money,
		@account_birthdate datetime,
		@account_role int,
		@account_image nvarchar(1000)
	SELECT @account_id = INSERTED.account_id,
		@account_login = INSERTED.account_login,
		@account_password = INSERTED.account_password,
		@account_nickname = INSERTED.account_nickname, 
		@account_email = INSERTED.account_email, 
		@account_country = INSERTED.account_country, 
		@account_balance = INSERTED.account_balance, 
		@account_birthdate = INSERTED.account_birthdate, 
		@account_role = INSERTED.account_role,
		@account_image = INSERTED.account_image
    FROM INSERTED
	UPDATE Account
	SET account_id = @account_id,
		account_login = @account_login,
		account_password = @account_password,
		account_nickname = @account_nickname, 
		account_email = @account_email, 
		account_country = @account_country, 
		account_balance = @account_balance, 
		account_birthdate = @account_birthdate, 
		account_role = @account_role, 
		account_image = @account_image
	Where Account.account_id = (SELECT account_id FROM inserted)
	UPDATE AccountSelect
	SET account_id = @account_id,
		account_login = @account_login,
		account_password = @account_password,
		account_nickname = @account_nickname, 
		account_email = @account_email, 
		account_country = @account_country, 
		account_balance = @account_balance, 
		account_birthdate = @account_birthdate, 
		account_role = @account_role,
		account_image = @account_image 
	Where AccountSelect.account_id = (SELECT account_id FROM inserted)
end

drop trigger updateyourself
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TRIGGER Account_insert ON Account 
INSTEAD OF INSERT 
AS										
BEGIN
	DECLARE @cur_value INT
	DECLARE @ins_value INT
	DECLARE @next_value INT

	SELECT @cur_value = MAX(account_id) + 1 FROM Account
	IF(@cur_value IS NULL) SET @cur_value = 1

	SELECT @ins_value = MAX(inserted.account_id) FROM inserted

	IF(@ins_value > @cur_value)
		SET @next_value = @ins_value
	ELSE
		SET @next_value = @cur_value 	

	SELECT * INTO #t FROM inserted

	UPDATE #t SET account_id = @next_value	

	INSERT INTO Account
	SELECT * FROM #t
END
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TRIGGER Report_insert ON Report 
INSTEAD OF INSERT 
AS										
BEGIN
	DECLARE @cur_value INT
	DECLARE @ins_value INT
	DECLARE @next_value INT

	SELECT @cur_value = MAX(report_id) + 1 FROM Report
	IF(@cur_value IS NULL) SET @cur_value = 1

	SELECT @ins_value = MAX(inserted.report_id) FROM inserted

	IF(@ins_value > @cur_value)
		SET @next_value = @ins_value
	ELSE
		SET @next_value = @cur_value 	

	SELECT * INTO #t FROM inserted

	UPDATE #t SET report_id = @next_value	

	INSERT INTO Report
	SELECT * FROM #t
END
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TRIGGER Game_insert ON Game
INSTEAD OF INSERT 
AS										
BEGIN
	DECLARE @cur_value INT
	DECLARE @ins_value INT
	DECLARE @next_value INT

	SELECT @cur_value = MAX(game_id) + 1 FROM Game
	IF(@cur_value IS NULL) SET @cur_value = 1

	SELECT @ins_value = MAX(inserted.game_id) FROM inserted

	IF(@ins_value > @cur_value)
		SET @next_value = @ins_value
	ELSE
		SET @next_value = @cur_value 	

	SELECT * INTO #t FROM inserted

	UPDATE #t SET game_id = @next_value	

	INSERT INTO Game
	SELECT * FROM #t
END
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TRIGGER Purchases_insert_accountCheck ON Purchases
INSTEAD OF INSERT 
AS										
BEGIN
	DECLARE @cur_value INT
	DECLARE @ins_value INT
	DECLARE @next_value INT

	SELECT @cur_value = MAX(purchase_id) + 1 FROM Purchases
	IF(@cur_value IS NULL) SET @cur_value = 1

	SELECT @ins_value = MAX(inserted.purchase_id) FROM inserted

	IF(@ins_value > @cur_value)
		SET @next_value = @ins_value
	ELSE
		SET @next_value = @cur_value 	

	SELECT * INTO #t FROM inserted

	UPDATE #t SET purchase_id = @next_value	
	UPDATE #t SET purchase_game_name = (SELECT game_name from game where game_id=#t.purchase_game_id)
	UPDATE #t SET purchase_purchaser_login = (SELECT CONVERT(varchar(20),suser_sname()))

	--check account condition
	DECLARE @account_balance INT
	DECLARE @account_date DATETIME
	DECLARE @game_price INT
	DECLARE @game_agelimit INT
	   
	SET @account_balance = (SELECT Account.account_balance FROM Account WHERE Account.account_login=(SELECT CONVERT(varchar(20),suser_sname())))
	SET @game_price = (SELECT purshase_price FROM #t)
	SET @game_agelimit= (SELECT Game.game_agelimit FROM Game Where Game.game_id = (SELECT purchase_game_id FROM #t))
	SET @account_date = (SELECT Account.account_birthdate FROM Account WHERE Account.account_login=(SELECT CONVERT(varchar(20),suser_sname())))
	
	IF (@account_balance>=@game_price AND DATEDIFF(YEAR, @account_date, GETDATE())>=@game_agelimit)
		INSERT INTO Purchases SELECT * FROM #t
END

drop trigger Purchases_insert_accountCheck
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TRIGGER PurchaseGameNameUpdateAfter_GameUpdate ON Game
AFTER UPDATE
AS
BEGIN
	UPDATE Purchases set purchase_game_name = (SELECT game_name FROM inserted) where purchase_game_id = (SELECT game_id FROM inserted)
END

drop trigger PurchaseGameNameUpdateAfter_GameUpdate
select * from Purchases
delete from Purchases
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TRIGGER Achievement_insert ON Achievement
INSTEAD OF INSERT 
AS										
BEGIN
	DECLARE @cur_value INT
	DECLARE @ins_value INT
	DECLARE @next_value INT

	SELECT @cur_value = MAX(achievement_id) + 1 FROM Achievement
	IF(@cur_value IS NULL) SET @cur_value = 1

	SELECT @ins_value = MAX(inserted.achievement_id) FROM inserted

	IF(@ins_value > @cur_value)
		SET @next_value = @ins_value
	ELSE
		SET @next_value = @cur_value 	

	SELECT * INTO #t FROM inserted

	UPDATE #t SET achievement_id = @next_value	

	INSERT INTO Achievement
	SELECT * FROM #t
END
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TRIGGER Receive_achievements_insert ON Receive_achievements
INSTEAD OF INSERT 
AS										
BEGIN
	SELECT * INTO #t FROM inserted	
	UPDATE #t SET account_id = (SELECT account_id FROM Account Where Account.account_login = (SELECT CONVERT(varchar(20),suser_sname())))
	INSERT INTO Receive_achievements
	SELECT * FROM #t
END

drop trigger Receive_achievements_insert
----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TRIGGER Report_delete_after_account_delete ON Account
INSTEAD OF DELETE
AS
BEGIN
	delete from Report where report_receiver_id = (SELECT account_id FROM deleted) 
	delete from Account where account_id = (SELECT account_id FROM deleted) 
END
drop trigger Report_delete_after_account_delete
----------------------------------------------------------------------------------------------------------------------------------------------------------