use IndTask
create table Account
(
account_id int primary key,
account_login nvarchar(50) unique not null,
account_password nvarchar(50) not null,
account_nickname nvarchar(50) not null,
account_email nvarchar(50),
account_country nvarchar(50),
account_balance money default 0,
account_birthdate datetime not null,
account_role int not null default 1,
account_image nvarchar(1000) default 'pack://application:,,,/Resources/default_image.png'
)

create table Game
(
game_id int primary key,
game_name nvarchar(50) not null,
game_description nvarchar(1000) null default '',
game_genre nvarchar(50) not null, 
game_release_date datetime not null,
game_developer nvarchar(50) not null,
game_publisher nvarchar(50) not null,
game_rating tinyint not null,
game_agelimit tinyint not null,
game_image nvarchar(100) default 'pack://application:,,,/Resources/default_image.png',
game_storeimage nvarchar(1000) default 'pack://application:,,,/Resources/default_image.png' 
)

create table Games_shop
(
game_id int not null unique,
constraint game_id foreign key (game_id)  references game(game_id) ON DELETE CASCADE,
game_price money not null,
)

create table Report
(
report_id int primary key,
report_comment nvarchar(250),
report_recieve_date datetime not null,
report_sender_id int not null,
report_receiver_id int not null,
constraint report_sender_id foreign key (report_sender_id)  references Account(account_id) ON DELETE CASCADE,
constraint report_receiver_id foreign key (report_receiver_id)  references Account(account_id) ON DELETE NO ACTION,
)

create table Purchases
(
purchase_id int primary key,
purchaser_id int not null,
constraint purchaser_id foreign key (purchaser_id)  references Account(account_id) ON DELETE CASCADE,
purchase_game_id int not null default '',
constraint purchase_game_id foreign key (purchase_game_id)  references Game(game_id) ON DELETE NO ACTION,
purchase_game_name nvarchar(50) null default '',
purchase_purchaser_login nvarchar(50) null default '',
purshase_price money not null,
purshase_date datetime not null,
)

create table Achievement
(
achievement_id int primary key,
achievement_name nvarchar(50) not null,
achievement_description nvarchar(1000) null default '',
achievement_game_id int not null,
constraint achievement_game_id foreign key (achievement_game_id)  references game(game_id) ON DELETE CASCADE,
achievement_image nvarchar(100) default 'pack://application:,,,/Resources/default_image.png'
)

create table Receive_achievements
(
achievement_id int not null,
constraint achievement_id foreign key (achievement_id)  references Achievement (achievement_id) ON DELETE CASCADE,
account_id int not null,
constraint account_id foreign key (account_id)  references Account(account_id) ON DELETE CASCADE,
receive_achievement_date datetime not null,
)