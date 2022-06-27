using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace DataBaseIndTask
{
    class DataProvider
    {
        private static DataProvider _default;
        private static string _login;
        private static string _password;
        public static DataProvider Default
            => _default ?? (_default = new DataProvider());
        private SqlConnection Connection { get; set; }
        private SqlConnection TrustedConnection { get; set; }
        public DataProvider()
        {
            if (_login == string.Empty || _password == string.Empty)
                throw new Exception("Connection ERROR: Enter your data");
            Connection = new SqlConnection(string.Format("Data Source=.\\SQLEXPRESS; Initial Catalog = IndTask; user id='{0}'; password ='{1}'", _login, _password));
            Connection.Open();
            TrustedConnection = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog = IndTask; Integrated Security=true");
        }
        public static void SetLoginData(string login, string password)
        {
            _login = login;
            _password = password;
        }        
        public void AddAccount(Account account)
        {
            TrustedConnection.Open();
            var sqlCommand = new SqlCommand(string.Format("INSERT INTO Account(account_login, account_password, account_nickname, account_birthdate, account_role) VALUES('{0}', '{1}', '{2}', '{3}', '{4}'); ", account.account_login, account.account_password, account.account_nickname, account.account_birthdate, account.account_role));
            sqlCommand.Connection = TrustedConnection;
            sqlCommand.ExecuteNonQuery();
            sqlCommand = new SqlCommand(string.Format("create login {0} with password = '{1}', default_database = IndTask, check_policy = off, check_expiration = off", account.account_login, account.account_password));
            sqlCommand.Connection = TrustedConnection;
            sqlCommand.ExecuteNonQuery();
            sqlCommand = new SqlCommand(string.Format("create user {0} for login {0}", account.account_login));
            sqlCommand.Connection = TrustedConnection;
            sqlCommand.ExecuteNonQuery();
            switch (account.account_role)
            {
                case 0:
                    {
                        sqlCommand = new SqlCommand(string.Format("alter server role [sysadmin] add member {0}", account.account_login));
                        sqlCommand.Connection = Connection;
                        sqlCommand.ExecuteNonQuery();
                    }
                    break;
                case 1:
                    {
                        sqlCommand = new SqlCommand(string.Format("alter role Player add member {0}", account.account_login));
                        sqlCommand.Connection = TrustedConnection;
                        sqlCommand.ExecuteNonQuery();
                    }
                    break;
            }            
            TrustedConnection.Close();
        }
        public Account GetLastAccount()
        {
            var sqlCommand = new SqlCommand("SELECT * FROM AccountSelect WHERE account_id=(SELECT max(AccountSelect.account_id) FROM AccountSelect)");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            Account account = null;
            while (reader.Read())
            {
                account = new Account();
                account.account_id = (int)reader["account_id"];
                account.account_login = (string)reader["account_login"];
                account.account_password = (string)reader["account_password"];
                account.account_nickname = (string)reader["account_nickname"];
                try { account.account_email = (string)reader["account_email"]; }
                catch { account.account_email = string.Empty; }
                try { account.account_country = (string)reader["account_country"]; }
                catch { account.account_country = string.Empty; }
                account.account_balance = (decimal)reader["account_balance"];
                account.account_birthdate = (DateTime)reader["account_birthdate"];
                account.account_role = (int)reader["account_role"];
                account.account_image = (string)reader["account_image"];
            }
            reader.Close();
            return account;
        }        
        public void CloseConnection()
        {
            Connection.Close();
            Connection.Dispose();
            SqlConnection.ClearAllPools();
            Connection = null;
            _default = null;
        }
        public List<Account> GetAccountListPlayer()
        {
            var sqlCommand = new SqlCommand("select * from AccountSelect");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            var AccountList = new List<Account>();
            while (reader.Read())
            {
                var account = new Account();
                account.account_id = (int)reader["account_id"];
                account.account_login = (string)reader["account_login"];
                account.account_password = (string)reader["account_password"];
                account.account_nickname = (string)reader["account_nickname"];
                try { account.account_email = (string)reader["account_email"]; }
                catch { account.account_email = string.Empty; }
                try { account.account_country = (string)reader["account_country"]; }
                catch { account.account_country = string.Empty; }
                account.account_balance = (decimal)reader["account_balance"];
                account.account_birthdate = (DateTime)reader["account_birthdate"];
                account.account_role = (int)reader["account_role"];
                account.account_image = (string)reader["account_image"];
                AccountList.Add(account);
            }
            reader.Close();
            return AccountList;
        }             
        public void UpdateAccount(Account account)
        {
            //получить старый пароль
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("select account_password from AccountSelect where account_id={0}", account.account_id));
            sqlCommand.Connection = Connection;
            string oldpassword = (string)sqlCommand.ExecuteScalar();
            //обновить пароль           
            sqlCommand = new SqlCommand(string.Format("ALTER LOGIN {0} WITH PASSWORD = '{1}' OLD_PASSWORD = '{2}'; ", account.account_login, account.account_password, oldpassword));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
            //обновить запись в таблице            
            sqlCommand = new SqlCommand(string.Format("update AccountSelect set account_password = '{0}', account_nickname ='{1}', account_email = '{2}',account_country ='{3}',account_balance = {4}, account_image = '{5}', account_birthdate = '{7}' where account_id={6}", account.account_password, account.account_nickname, account.account_email, account.account_country, (account.account_balance).ToString().Replace(',', '.'), account.account_image, account.account_id, account.account_birthdate));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public List<Report> GetReportListPlayer()
        {
            var sqlCommand = new SqlCommand("select * from ReportSelect");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            var ReportList = new List<Report>();
            while (reader.Read())
            {
                var report = new Report()
                {
                    report_id = (int)reader["report_id"],
                    report_comment = (string)reader["report_comment"],
                    report_recievedate = (DateTime)reader["report_recieve_date"],
                    report_senderid = (int)reader["report_sender_id"],
                    report_receiverid = (int)reader["report_receiver_id"],
                    report_receivernickname = (string)reader["account_nickname"],
                    report_receiverimage = (string)reader["account_image"],
                };
                ReportList.Add(report);
            }
            reader.Close();
            return ReportList;
        }
        public List<AccountForReport> GetAccountForReport()
        {
            var sqlCommand = new SqlCommand("select * from AccountSelectForReport");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            var AccountForReportList = new List<AccountForReport>();
            while (reader.Read())
            {
                var accountForReport = new AccountForReport();
                accountForReport.account_id = (int)reader["account_id"];
                accountForReport.account_nickname = (string)reader["account_nickname"];
                try { accountForReport.account_country = (string)reader["account_country"]; }
                catch { accountForReport.account_country = string.Empty; }
                accountForReport.account_birthdate = Convert.ToString(reader["account_birthdate"]).Substring(0, Convert.ToString(reader["account_birthdate"]).Length - 8);
                accountForReport.account_image = (string)reader["account_image"];
                AccountForReportList.Add(accountForReport);
            }
            reader.Close();
            return AccountForReportList;
        }
        public void AddReport(Report report)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("INSERT INTO Report(report_comment, report_recieve_date, report_sender_id, report_receiver_id) VALUES ('{0}', '{1}',  {2}, {3});", report.report_comment, report.report_recievedate, report.report_senderid, report.report_receiverid));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public Report GetLastReport()
        {
            var sqlCommand = new SqlCommand("SELECT * FROM ReportSelect WHERE report_id=(SELECT max(ReportSelect.report_id) FROM ReportSelect)");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            Report report = null;
            while (reader.Read())
            {
                report = new Report()
                {
                    report_id = (int)reader["report_id"],
                    report_comment = (string)reader["report_comment"],
                    report_recievedate = (DateTime)reader["report_recieve_date"],
                    report_senderid = (int)reader["report_sender_id"],
                    report_receiverid = (int)reader["report_receiver_id"],
                    report_receivernickname = (string)reader["account_nickname"],
                    report_receiverimage = (string)reader["account_image"],
                };
            }
            reader.Close();
            return report;
        }
        public List<GameInLibrary> GetGameInLibraryList()
        {
            var sqlCommand = new SqlCommand("select * from GamesLibrary");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            var GameInLibraryList = new List<GameInLibrary>();
            while (reader.Read())
            {
                var gameInLibrary = new GameInLibrary();
                try
                {
                    gameInLibrary.purchase_id = (int)reader["purchase_id"];
                    gameInLibrary.purchaser_id = (int)reader["purchaser_id"];
                    gameInLibrary.purchase_game_id = (int)reader["purchase_game_id"];
                    gameInLibrary.purchase_price = (decimal)reader["purshase_price"];
                    gameInLibrary.purchase_date = (DateTime)reader["purshase_date"];
                    gameInLibrary.game_name = (string)reader["game_name"];
                    gameInLibrary.game_description = (string)reader["game_description"];
                    gameInLibrary.game_genre = (string)reader["game_genre"];
                    gameInLibrary.game_releasedate = (DateTime)reader["game_release_date"];
                    gameInLibrary.game_developer = (string)reader["game_developer"];
                    gameInLibrary.game_publisher = (string)reader["game_publisher"];
                    gameInLibrary.game_rating = (byte)reader["game_rating"];
                    gameInLibrary.game_agelimit = (byte)reader["game_agelimit"];
                    gameInLibrary.game_image = (string)reader["game_image"];
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                GameInLibraryList.Add(gameInLibrary);
            }
            reader.Close();
            return GameInLibraryList;
        }
        public List<GameInStore> GetGameInStoreList()
        {
            var sqlCommand = new SqlCommand("select * from GamesStore");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            var GameInStoreList = new List<GameInStore>();
            while (reader.Read())
            {
                var gameInStore = new GameInStore();
                try
                {
                    gameInStore.game_price = (decimal)reader["game_price"];
                    gameInStore.game_id = (int)reader["game_id"];
                    gameInStore.game_name = (string)reader["game_name"];
                    gameInStore.game_description = (string)reader["game_description"];
                    gameInStore.game_genre = (string)reader["game_genre"];
                    gameInStore.game_releasedate = (DateTime)reader["game_release_date"];
                    gameInStore.game_developer = (string)reader["game_developer"];
                    gameInStore.game_publisher = (string)reader["game_publisher"];
                    gameInStore.game_rating = (byte)reader["game_rating"];
                    gameInStore.game_agelimit = (byte)reader["game_agelimit"];
                    gameInStore.game_image = (string)reader["game_image"];
                    gameInStore.game_storeimage = (string)reader["game_storeimage"];
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                GameInStoreList.Add(gameInStore);
            }
            reader.Close();
            return GameInStoreList;
        }
        public void AddPurchase(Purchase purchase)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("INSERT INTO Purchases(purchaser_id, purchase_game_id, purshase_price, purshase_date) VALUES ({0}, {1}, {2}, '{3}');", purchase.purchaser_id, purchase.purchase_game_id, purchase.purchase_price, purchase.purchase_date));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public int GetLastPurchaseID()
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand("select max(purchase_id) from GamesLibrary");
            sqlCommand.Connection = Connection;
            return (int)sqlCommand.ExecuteScalar();
        }
        public List<Achievement> GetAchievementList()
        {
            var sqlCommand = new SqlCommand("select * from AchievementSelect");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();

            var AchievementList = new List<Achievement>();
            while (reader.Read())
            {
                var achievement = new Achievement()
                {
                    achievement_id = (int)reader["achievement_id"],
                    achievement_name = (string)reader["achievement_name"],
                    achievement_game_id = (int)reader["achievement_game_id"],
                    achievement_game_name = (string)reader["game_name"],
                    achievement_image = (string)reader["achievement_image"],
                };
                AchievementList.Add(achievement);
            }
            reader.Close();
            return AchievementList;
        }
        public List<ReceiveAchievement> GetReceiveAchievementList()
        {
            var sqlCommand = new SqlCommand("select * from Receive_achievementsSelect");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();

            var ReceiveAchievementList = new List<ReceiveAchievement>();
            while (reader.Read())
            {
                var ReceiveAchievement = new ReceiveAchievement()
                {
                    receiveAchievement_account_id = (int)reader["account_id"],
                    receiveAchievement_account_login = (string)reader["account_login"],
                    receiveAchievement_id = (int)reader["achievement_id"],
                    receiveAchievement_name = (string)reader["achievement_name"],
                    receiveAchievement_game_id = (int)reader["achievement_game_id"],
                    receiveAchievement_game_name = (string)reader["game_name"],
                    receiveAchievement_image = (string)reader["achievement_image"],
                    receiveAchievement_date = (DateTime)reader["receive_achievement_date"],
                    receiveAchievement_description = (string)reader["achievement_description"],
                };
                ReceiveAchievementList.Add(ReceiveAchievement);
            }
            reader.Close();
            return ReceiveAchievementList;
        }
        public void AddReceivedAchievement(int unReceivedAchievementID)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("INSERT INTO Receive_achievements(achievement_id, receive_achievement_date) VALUES ({0}, '{1}');", unReceivedAchievementID, DateTime.Now));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public ReceiveAchievement GetReceiveAchievement(int receivedAchievementID)
        {
            var sqlCommand = new SqlCommand(string.Format("select * from Receive_achievementsSelect where achievement_id = {0}", receivedAchievementID));
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();

            ReceiveAchievement ReceiveAchievement = null;
            while (reader.Read())
            {
                ReceiveAchievement = new ReceiveAchievement()
                {
                    receiveAchievement_id = (int)reader["achievement_id"],
                    receiveAchievement_name = (string)reader["achievement_name"],
                    receiveAchievement_game_id = (int)reader["achievement_game_id"],
                    receiveAchievement_game_name = (string)reader["game_name"],
                    receiveAchievement_image = (string)reader["achievement_image"],
                    receiveAchievement_date = (DateTime)reader["receive_achievement_date"],
                    receiveAchievement_description = (string)reader["achievement_description"],
                };
            }
            reader.Close();
            return ReceiveAchievement;
        }
        public bool IsAccountCurrent(Account account)
        {
            return account.account_login == _login && account.account_password == _password;
        }
        public void DeleteAccount(Account account)
        {            
            if (account.account_login == _login && account.account_password == _password)
            {                
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete your current account? After deletion, you will be redirected to the login window.", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
                Connection.Close();
                SqlConnection.ClearAllPools();
            }
            TrustedConnection.Close();
            TrustedConnection.Open();
            //удалить из таблицы
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("delete from Account where account_id = {0}", account.account_id));
            sqlCommand.Connection = TrustedConnection;
            sqlCommand.ExecuteNonQuery();
            //удалить пользователя            
            sqlCommand = new SqlCommand(string.Format("drop user {0}", account.account_login));
            sqlCommand.Connection = TrustedConnection;
            sqlCommand.ExecuteNonQuery();
            sqlCommand = new SqlCommand(string.Format("drop login {0}", account.account_login));
            sqlCommand.Connection = TrustedConnection;
            sqlCommand.ExecuteNonQuery();
            TrustedConnection.Close();            
        }
        public void DeleteReport(Report report)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("delete from Report where report_id = {0}", report.report_id));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public List<Game> GetGameList()
        {
            var sqlCommand = new SqlCommand("select * from Game");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            var GameList = new List<Game>();
            while (reader.Read())
            {
                var game = new Game();
                try
                {
                    game.game_id = (int)reader["game_id"];
                    game.game_name = (string)reader["game_name"];
                    game.game_description = (string)reader["game_description"];
                    game.game_genre = (string)reader["game_genre"];
                    game.game_releasedate = (DateTime)reader["game_release_date"];
                    game.game_developer = (string)reader["game_developer"];
                    game.game_publisher = (string)reader["game_publisher"];
                    game.game_rating = (byte)reader["game_rating"];
                    game.game_agelimit = (byte)reader["game_agelimit"];
                    game.game_image = (string)reader["game_image"];
                    game.game_storeimage = (string)reader["game_storeimage"];
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                GameList.Add(game);
            }
            reader.Close();
            return GameList;
        }
        public void DeleteGame(Game game)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("delete from Game where game_id = {0}", game.game_id));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public void UpdateGame(Game game)
        {
            //обновить запись в таблице   
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("update Game set game_name = '{0}', game_description = '{1}', game_genre = '{2}', game_release_date = '{3}', game_developer = '{4}', game_publisher = '{5}', game_rating={6}, game_agelimit={7}, game_image = '{8}',game_storeimage = '{9}' where game_id = {10}",
                game.game_name, game.game_description, game.game_genre, game.game_releasedate, game.game_developer, game.game_publisher, game.game_rating, game.game_agelimit, game.game_image, game.game_storeimage, game.game_id));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public void AddGame(Game game)
        {
            var sqlCommand = new SqlCommand(string.Format("INSERT INTO Game(game_name, game_description, game_genre, game_release_date, game_developer, game_publisher, game_rating, game_agelimit) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7});",
                game.game_name, game.game_description, game.game_genre, game.game_releasedate, game.game_developer, game.game_publisher, game.game_rating, game.game_agelimit));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();            
        }
        public Game GetLastGame()
        {
            var sqlCommand = new SqlCommand("SELECT * FROM Game WHERE game_id=(SELECT max(Game.game_id) FROM Game)");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            Game game = null;
            while (reader.Read())
            {
                game = new Game()
                {
                    game_id = (int)reader["game_id"],
                    game_name = (string)reader["game_name"],
                    game_agelimit = (byte)reader["game_agelimit"],
                    game_description = (string)reader["game_description"],
                    game_developer = (string)reader["game_developer"],
                    game_genre = (string)reader["game_genre"],
                    game_image = (string)reader["game_image"],
                    game_storeimage = (string)reader["game_storeimage"],
                    game_publisher = (string)reader["game_publisher"],
                    game_rating = (byte)reader["game_rating"],
                    game_releasedate = (DateTime)reader["game_release_date"],
                };
            }
            reader.Close();
            return game;
        }
        public void UpdateGameinStore(int oldID, int newID, int price)
        {
            SqlCommand sqlCommand = new SqlCommand(string.Format("update Games_shop set game_id = {0}, game_price = {1} where game_id = {2}",newID, price, oldID));
            sqlCommand.Connection = Connection;            
            sqlCommand.ExecuteNonQuery();
        }
        public void DeleteGameFromStore(GameInStore gameInStore)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("delete from Games_shop where game_id = {0}", gameInStore.game_id));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public void AddGameInStore(int game_id, int game_price)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("INSERT INTO Games_shop(game_id, game_price) VALUES({0}, {1}); ", game_id, game_price));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public List<Purchase> GetPurchaseList()
        {
            var sqlCommand = new SqlCommand("select * from Purchases");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            var PurchaseList = new List<Purchase>();
            while (reader.Read())
            {
                var purchase = new Purchase();
                try
                {
                    purchase.purchase_id = (int)reader["purchase_id"];
                    purchase.purchaser_id = (int)reader["purchaser_id"];
                    purchase.purchase_game_id = (int)reader["purchase_game_id"];
                    purchase.purchase_price = (decimal)reader["purshase_price"];
                    purchase.purchase_date = (DateTime)reader["purshase_date"];
                    purchase.purchase_game_name = (string)reader["purchase_game_name"];
                    purchase.purchase_purchaser_login = (string)reader["purchase_purchaser_login"];
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                PurchaseList.Add(purchase);
            }
            reader.Close();
            return PurchaseList;
        }
        public List<AchievementAdmin> GetAchievementListAdmin()
        {
            var sqlCommand = new SqlCommand("select * from Achievement");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();

            var AchievementListAdmin = new List<AchievementAdmin>();
            while (reader.Read())
            {
                var achievement = new AchievementAdmin()
                {
                    achievement_id = (int)reader["achievement_id"],
                    achievement_name = (string)reader["achievement_name"],
                    achievement_game_id = (int)reader["achievement_game_id"],
                    achievement_description = (string)reader["achievement_description"],                    
                    achievement_image = (string)reader["achievement_image"],
                };
                AchievementListAdmin.Add(achievement);
            }
            reader.Close();
            return AchievementListAdmin;
        }
        public void UpdateAchievement(AchievementAdmin achievementAdmin)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("update Achievement set achievement_name = '{0}', achievement_description = '{1}', achievement_game_id = {2}, achievement_image = '{3}' where achievement_id = {4}",
                achievementAdmin.achievement_name, achievementAdmin.achievement_description, achievementAdmin.achievement_game_id, achievementAdmin.achievement_image, achievementAdmin.achievement_id));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public void DeleteAchievement(AchievementAdmin achievementAdmin)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("delete from Achievement where achievement_id = {0}", achievementAdmin.achievement_id));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public void AddAchievement(AchievementAdmin achievementAdmin)
        {
            SqlCommand sqlCommand = null;
            sqlCommand = new SqlCommand(string.Format("INSERT INTO Achievement(achievement_name, achievement_description, achievement_game_id) VALUES('{0}', '{1}', '{2}'); ", achievementAdmin.achievement_name, achievementAdmin.achievement_description, achievementAdmin.achievement_game_id));
            sqlCommand.Connection = Connection;
            sqlCommand.ExecuteNonQuery();
        }
        public AchievementAdmin GetLastAchievement()
        {
            var sqlCommand = new SqlCommand("SELECT * FROM Achievement WHERE achievement_id=(SELECT max(Achievement.achievement_id) FROM Achievement)");
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            AchievementAdmin achievementAdmin = null;
            while (reader.Read())
            {
                achievementAdmin = new AchievementAdmin()
                {
                    achievement_id = (int)reader["achievement_id"],
                    achievement_description = (string)reader["achievement_description"],
                    achievement_game_id = (int)reader["achievement_game_id"],
                    achievement_name = (string)reader["achievement_name"],
                    achievement_image = (string)reader["achievement_image"],
                };
            }
            reader.Close();
            return achievementAdmin;
        }
        public List<List<string>> CustomQuery(string query, List<string> resultColumnsHeaders)
        {
            List<List<string>> result = new List<List<string>>();
            result.Add(resultColumnsHeaders);

            var sqlCommand = new SqlCommand(query);
            sqlCommand.Connection = Connection;
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                List<string> row = new List<string>();
                try
                {
                    foreach (var s in resultColumnsHeaders)
                        row.Add(Convert.ToString(reader[s]));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                result.Add(row);
            }
            reader.Close();
            return result;
        }
    }
}