using DataBaseIndTask.CustomQueryBuilder;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace DataBaseIndTask
{
    public class CustomQuaryBuilderVM : ViewModelBase
    {
        private enum ETables
        {
            t_account = 0,
            t_game = 1,
            t_games_shop = 2,
            t_report = 3,
            t_purchases = 4,
            t_achievement = 5,
            t_receive_achievements = 6,
        };
        private string _sqlQuary;
        private AccountTable _accountTable;
        private GameTable _gameTable;
        private GamesShopTable _gamesShopTable;
        private ReportTable _reportTable;
        private PurchasesTable _purchasesTable;
        private AchievementTable _achievementTable;
        private ReceiveAchievementsTable _receiveAchievementsTable;
        private CustomQueryBuilderWindow _customQueryBuilderWindow;
        public CustomQuaryBuilderVM()
        {
            _sqlQuary = string.Empty;
            _accountTable = new AccountTable(this);
            _gameTable = new GameTable(this);
            _gamesShopTable = new GamesShopTable(this);
            _reportTable = new ReportTable(this);
            _purchasesTable = new PurchasesTable(this);
            _achievementTable = new AchievementTable(this);
            _receiveAchievementsTable = new ReceiveAchievementsTable(this);
            UsedFieldsList = new List<string>();
            FormerMetaData();
            ConditionList = new ObservableCollection<ConditionVM>();
            _usedTablesList = new ObservableCollection<string>();
        }

        public List<string> UsedFieldsList;
        public string SQLQuery
        {
            get
            {
                return _sqlQuary;
            }
            set
            {
                _sqlQuary = value;
            }
        }

        #region Binding Tables Properties 
        public AccountTable AccountTable { get { return _accountTable; } }
        public GameTable GameTable { get { return _gameTable; } }
        public GamesShopTable GamesShopTable { get { return _gamesShopTable; } }
        public ReportTable ReportTable { get { return _reportTable; } }
        public PurchasesTable PurchasesTable { get { return _purchasesTable; } }
        public AchievementTable AchievementTable { get { return _achievementTable; } }
        public ReceiveAchievementsTable ReceiveAchievementsTable { get { return _receiveAchievementsTable; } }

        #endregion
        #region ShowSQLQueryCommand
        private RelayCommand _showSQLQueryCommand;
        public RelayCommand ShowSQLQueryCommand =>
            _showSQLQueryCommand ?? (_showSQLQueryCommand = new RelayCommand(ShowSQLQuery));
        private void ShowSQLQuery()
        {
            MessageBox.Show(SQLQuery);
        }
        #endregion
        #region RunSQLQueryCommand
        private RelayCommand _runSQLQueryCommand;
        public RelayCommand RunSQLQueryCommand =>
            _runSQLQueryCommand ?? (_runSQLQueryCommand = new RelayCommand(RunSQLQuery));
        private void RunSQLQuery()
        {
            if (UsedFieldsList.Count == 0)
                return;
            List<string> fieldsNames = new List<string>();
            foreach (var s in UsedFieldsList)
                fieldsNames.Add(GetFieldName(s));
            try
            {
                List<List<string>> result = DataProvider.Default.CustomQuery(SQLQuery, fieldsNames);
                DataTable dataTable = new DataTable();
                foreach (var s in result[0])
                {
                    dataTable.Columns.Add(s, typeof(string));
                }
                for (int i = 1; i < result.Count; i++)
                {
                    DataRow row = dataTable.NewRow();
                    for (int j = 0; j < result[i].Count; j++)
                        row[j] = result[i][j];
                    dataTable.Rows.Add(row);
                }
                _customQueryBuilderWindow.ResultDataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error!"); }
        }
        #endregion
        #region OpenWindowCommand
        private RelayCommand _openWindowCommand;
        public RelayCommand OpenWindowCommand =>
            _openWindowCommand ?? (_openWindowCommand = new RelayCommand(OpenWindow));
        private void OpenWindow()
        {
            _customQueryBuilderWindow = new CustomQueryBuilderWindow(this);
            _customQueryBuilderWindow.Show();
        }
        #endregion
        #region ChooseAllCommand
        private RelayCommand _chooseAllCommand;
        public RelayCommand ChooseAllCommand =>
            _chooseAllCommand ?? (_chooseAllCommand = new RelayCommand(ChooseAll));
        public void ChooseAll()
        {
            foreach (TreeViewItem item in _customQueryBuilderWindow.TreeViewSection.Items)
            {
                item.IsExpanded = true;
                foreach (TreeViewItem field in item.Items)
                    if (((CheckBox)field.Header).IsChecked == false)
                        ((CheckBox)field.Header).IsChecked = true;
            }
        }
        #endregion
        #region ClearAllCommand
        private RelayCommand _clearAllCommand;
        public RelayCommand ClearAllCommand =>
            _clearAllCommand ?? (_clearAllCommand = new RelayCommand(ClearAll));
        public void ClearAll()
        {
            foreach (TreeViewItem item in _customQueryBuilderWindow.TreeViewSection.Items)
            {
                item.IsExpanded = true;
                foreach (TreeViewItem field in item.Items)
                    if (((CheckBox)field.Header).IsChecked == true)
                        ((CheckBox)field.Header).IsChecked = false;
            }
        }
        #endregion
        #region Condition part
        public ConditionVM SelectedCondition { get; set; }
        public ObservableCollection<ConditionVM> ConditionList { get; set; }
        private RelayCommand _addConditionCommand;
        public RelayCommand AddConditionCommand =>
            _addConditionCommand ?? (_addConditionCommand = new RelayCommand(AddCondition));
        public void AddCondition()
        {
            AddNewConditionWindow addNewConditionWindow = new AddNewConditionWindow(_usedTablesList, ConditionList, this);
            addNewConditionWindow.Show();
        }
        private RelayCommand _deleteConditionCommand;
        public RelayCommand DeleteConditionCommand =>
            _deleteConditionCommand ?? (_deleteConditionCommand = new RelayCommand(DeleteCondition));
        public void DeleteCondition()
        {
            if (SelectedCondition != null)
                ConditionList.Remove(SelectedCondition);
            UpdateSQLQuery();
        }
        #endregion

        public void UpdateSQLQuery()
        {
            SQLQuery = string.Empty;
            UpdateTablesUsedInQuery();
            UpdateSelectPart();
            UpdateFromPart();
            UpdateJoinPart();
            UpdateWherePart();
            SQLQuery += _selectPart + _fromPart + _joinPart + _wherePart;
        }
        private string _selectPart;
        private string _fromPart;
        private string _joinPart;
        private string _wherePart;
        private Dictionary<string, bool> _tablesUsedInQuery;
        private ObservableCollection<string> _usedTablesList;
        private string _mainTable;
        private Dictionary<string, Dictionary<string, List<string>>> _metaData;
        private Dictionary<string, bool> _joinedTables;
        private void FormerMetaData()
        {
            _tablesUsedInQuery = new Dictionary<string, bool>();
            _tablesUsedInQuery.Add("Account", false);
            _tablesUsedInQuery.Add("Game", false);
            _tablesUsedInQuery.Add("Report", false);
            _tablesUsedInQuery.Add("Purchases", false);
            _tablesUsedInQuery.Add("Games_shop", false);
            _tablesUsedInQuery.Add("Achievement", false);
            _tablesUsedInQuery.Add("Receive_achievements", false);

            _joinedTables = new Dictionary<string, bool>();
            _joinedTables.Add("Account", false);
            _joinedTables.Add("Game", false);
            _joinedTables.Add("Report", false);
            _joinedTables.Add("Purchases", false);
            _joinedTables.Add("Games_shop", false);
            _joinedTables.Add("Achievement", false);
            _joinedTables.Add("Receive_achievements", false);

            _metaData = new Dictionary<string, Dictionary<string, List<string>>>();
            Dictionary<string, List<string>> tempTable;
            List<string> tempArr;

            #region metaData Account
            tempTable = new Dictionary<string, List<string>>();
            tempArr = new List<string>();
            tempArr.Add("    JOIN Report ON Report.report_sender_id=Account.account_id\n");
            tempTable.Add("Report", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.account_id=Account.account_id\n");
            tempTable.Add("Receive_achievements", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Purchases ON Purchases.purchaser_id=Account.account_id\n");
            tempTable.Add("Purchases", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.account_id=Account.account_id\n");
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_id=Receive_achievements.achievement_id\n");
            tempTable.Add("Achievement", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Purchases ON Purchases.purchaser_id=Account.account_id\n");
            tempArr.Add("    JOIN Game ON Game.game_id=Purchases.purchase_game_id\n");
            tempTable.Add("Game", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Purchases ON Purchases.purchaser_id=Account.account_id\n");
            tempArr.Add("    JOIN Game ON Game.game_id=Purchases.purchase_game_id\n");
            tempArr.Add("    JOIN Games_shop ON Games_shop.game_id=Game.game_id\n");
            tempTable.Add("Games_shop", tempArr);

            _metaData.Add("Account", tempTable);
            #endregion
            #region metaData Game
            tempTable = new Dictionary<string, List<string>>();
            tempArr = new List<string>();
            tempArr.Add("    JOIN Purchases on Purchases.purchase_game_id=Game.game_id\n");
            tempArr.Add("    JOIN Account on Account.account_id=Purchases.purchaser_id\n");
            tempArr.Add("    JOIN Report ON Report.report_sender_id=Account.account_id\n");
            tempTable.Add("Report", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_game_id=Game.game_id\n");
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.achievement_id=Achievement.achievement_id\n");
            tempTable.Add("Receive_achievements", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Purchases on Purchases.purchase_game_id=Game.game_id\n");
            tempTable.Add("Purchases", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_game_id=Game.game_id\n");
            tempTable.Add("Achievement", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Purchases on Purchases.purchase_game_id=Game.game_id\n");
            tempArr.Add("    JOIN Account on Account.account_id=Purchases.purchaser_id\n");
            tempTable.Add("Account", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Games_shop ON Games_shop.game_id=Game.game_id\n");
            tempTable.Add("Games_shop", tempArr);

            _metaData.Add("Game", tempTable);
            #endregion
            #region metaData Games_shop
            tempTable = new Dictionary<string, List<string>>();
            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Games_shop.game_id\n");
            tempArr.Add("    JOIN Purchases on Purchases.purchase_game_id=Game.game_id\n");
            tempArr.Add("    JOIN Account on Account.account_id=Purchases.purchaser_id\n");
            tempArr.Add("    JOIN Report ON Report.report_sender_id=Account.account_id\n");
            tempTable.Add("Report", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Games_shop.game_id\n");
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_game_id=Game.game_id\n");
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.achievement_id=Achievement.achievement_id\n");
            tempTable.Add("Receive_achievements", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Games_shop.game_id\n");
            tempArr.Add("    JOIN Purchases on Purchases.purchase_game_id=Game.game_id\n");
            tempTable.Add("Purchases", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Games_shop.game_id\n");
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_game_id=Game.game_id\n");
            tempTable.Add("Achievement", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Games_shop.game_id\n");
            tempArr.Add("    JOIN Purchases on Purchases.purchase_game_id=Game.game_id\n");
            tempArr.Add("    JOIN Account on Account.account_id=Purchases.purchaser_id\n");
            tempTable.Add("Account", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Games_shop.game_id\n");
            tempTable.Add("Game", tempArr);

            _metaData.Add("Games_shop", tempTable);
            #endregion
            #region metaData Report
            tempTable = new Dictionary<string, List<string>>();
            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Report.report_sender_id\n");
            tempArr.Add("    JOIN Purchases ON Purchases.purchaser_id=Account.account_id\n");
            tempArr.Add("    JOIN Game ON Game.game_id=Purchases.purchase_game_id\n");
            tempArr.Add("    JOIN Games_shop ON Games_shop.game_id=Game.game_id\n");
            tempTable.Add("Games_shop", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Report.report_sender_id\n");
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.account_id=Account.account_id\n");
            tempTable.Add("Receive_achievements", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Report.report_sender_id\n");
            tempArr.Add("    JOIN Purchases ON purchaser_id=Account.account_id\n");
            tempTable.Add("Purchases", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Report.report_sender_id\n");
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.account_id=Account.account_id\n");
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_id=Receive_achievements.achievement_id\n");
            tempTable.Add("Achievement", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Report.report_sender_id\n");
            tempTable.Add("Account", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Report.report_sender_id\n");
            tempArr.Add("    JOIN Purchases ON Purchases.purchaser_id=Account.account_id\n");
            tempArr.Add("    JOIN Game ON Game.game_id=Purchases.purchase_game_id\n");
            tempTable.Add("Game", tempArr);

            _metaData.Add("Report", tempTable);
            #endregion
            #region metaData Purchases
            tempTable = new Dictionary<string, List<string>>();
            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Purchases.purchase_game_id\n");
            tempArr.Add("    JOIN Games_shop ON Games_shop.game_id=Game.game_id\n");
            tempTable.Add("Games_shop", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Purchases.purchaser_id\n");
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.account_id=Account.account_id\n");
            tempTable.Add("Receive_achievements", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Purchases.purchaser_id\n");
            tempArr.Add("    JOIN Report ON Report.report_sender_id=Account.account_id\n");
            tempTable.Add("Report", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Purchases.purchase_game_id\n");
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_game_id=Game.game_id\n");
            tempTable.Add("Achievement", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Purchases.purchaser_id\n");
            tempTable.Add("Account", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Purchases.purchase_game_id\n");
            tempTable.Add("Game", tempArr);

            _metaData.Add("Purchases", tempTable);
            #endregion
            #region metaData Achievement
            tempTable = new Dictionary<string, List<string>>();
            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Achievement.achievement_game_id\n");
            tempArr.Add("    JOIN Games_shop ON Games_shop.game_id=Game.game_id\n");
            tempTable.Add("Games_shop", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.achievement_id=Achievement.achievement_id\n");
            tempTable.Add("Receive_achievements", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.achievement_id=Achievement.achievement_id\n");
            tempArr.Add("    JOIN Account ON Account.account_id=Receive_achievements.account_id\n");
            tempArr.Add("    JOIN Report ON Report.report_sender_id=Account.account_id\n");
            tempTable.Add("Report", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Achievement.achievement_game_id\n");
            tempArr.Add("    JOIN Purchases ON Purchases.purchase_game_id=Game.game_id\n");
            tempTable.Add("Purchases", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Receive_achievements ON Receive_achievements.achievement_id=Achievement.achievement_id\n");
            tempArr.Add("    JOIN Account ON Account.account_id=Receive_achievements.account_id\n");
            tempTable.Add("Account", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Game ON Game.game_id=Achievement.achievement_game_id\n");
            tempTable.Add("Game", tempArr);

            _metaData.Add("Achievement", tempTable);
            #endregion
            #region metaData Receive_achievements
            tempTable = new Dictionary<string, List<string>>();
            tempArr = new List<string>();
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_id=Receive_achievements.achievement_id\n");
            tempArr.Add("    JOIN Game ON Game.game_id=Achievement.achievement_game_id\n");
            tempArr.Add("    JOIN Games_shop ON Games_shop.game_id=Game.game_id\n");
            tempTable.Add("Games_shop", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_id=Receive_achievements.achievement_id\n");
            tempTable.Add("Achievement", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Receive_achievements.account_id\n");
            tempArr.Add("    JOIN Report ON Report.report_sender_id=Account.account_id\n");
            tempTable.Add("Report", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Receive_achievements.account_id\n");
            tempArr.Add("    JOIN Purchases ON Purchases.purchaser_id=Account.account_id\n");
            tempTable.Add("Purchases", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Account ON Account.account_id=Receive_achievements.account_id\n");
            tempTable.Add("Account", tempArr);

            tempArr = new List<string>();
            tempArr.Add("    JOIN Achievement ON Achievement.achievement_id=Receive_achievements.achievement_id\n");
            tempArr.Add("    JOIN Game ON Game.game_id=Achievement.achievement_game_id\n");
            tempTable.Add("Game", tempArr);

            _metaData.Add("Receive_achievements", tempTable);
            #endregion
        }
        private string GetTableName(string field)
        {
            string tableName = string.Empty;
            foreach (char c in field)
                if (c == '.')
                    return tableName;
                else
                    tableName += c;
            return string.Empty;
        }
        private string GetFieldName(string field)
        {
            string fieldName = string.Empty;
            foreach (char c in field)
                if (c == '.')
                    fieldName = string.Empty;
                else
                    fieldName += c;
            return fieldName;
        }
        private void UpdateTablesUsedInQuery()
        {
            _mainTable = string.Empty;
            if (UsedFieldsList.Count >= 1)
                _mainTable = GetTableName(UsedFieldsList[0]);
            foreach (var s in UsedFieldsList)
                _tablesUsedInQuery[GetTableName(s)] = true;
            _usedTablesList.Clear();
            foreach (var s in UsedFieldsList)
                if (!_usedTablesList.Contains(GetTableName(s)))
                    _usedTablesList.Add(GetTableName(s));
        }
        private List<string> GetJoinTables(string secondTable)
        {
            switch (_mainTable)
            {
                case "Account":
                    switch (secondTable)
                    {
                        case "Account":
                            _joinedTables[secondTable] = true;
                            break;
                        case "Game":
                            _joinedTables["Game"] = true;
                            _joinedTables["Purchases"] = true;
                            break;
                        case "Games_shop":
                            _joinedTables["Game"] = true;
                            _joinedTables["Purchases"] = true;
                            _joinedTables["Games_shop"] = true;
                            break;
                        case "Report":
                            _joinedTables["Report"] = true;
                            break;
                        case "Purchases":
                            _joinedTables["Purchases"] = true;
                            break;
                        case "Achievement":
                            _joinedTables["Receive_achievements"] = true;
                            _joinedTables["Achievement"] = true;
                            break;
                        case "Receive_achievements":
                            _joinedTables["Receive_achievements"] = true;
                            break;
                    }
                    break;
                case "Game":
                    switch (secondTable)
                    {
                        case "Account":
                            _joinedTables["Account"] = true;
                            _joinedTables["Purchases"] = true;
                            break;
                        case "Game":
                            _joinedTables["Game"] = true;
                            break;
                        case "Games_shop":
                            _joinedTables["Games_shop"] = true;
                            break;
                        case "Report":
                            _joinedTables["Account"] = true;
                            _joinedTables["Purchases"] = true;
                            _joinedTables["Report"] = true;
                            break;
                        case "Purchases":
                            _joinedTables["Purchases"] = true;
                            break;
                        case "Achievement":
                            _joinedTables["Achievement"] = true;
                            break;
                        case "Receive_achievements":
                            _joinedTables["Achievement"] = true;
                            _joinedTables["Receive_achievements"] = true;
                            break;
                    }
                    break;
                case "Games_shop":
                    switch (secondTable)
                    {
                        case "Account":
                            _joinedTables["Game"] = true;
                            _joinedTables["Purchases"] = true;
                            _joinedTables["Account"] = true;
                            break;
                        case "Game":
                            _joinedTables["Game"] = true;
                            break;
                        case "Games_shop":
                            _joinedTables["Games_shop"] = true;
                            break;
                        case "Report":
                            _joinedTables["Game"] = true;
                            _joinedTables["Purchases"] = true;
                            _joinedTables["Account"] = true;
                            _joinedTables["Report"] = true;
                            break;
                        case "Purchases":
                            _joinedTables["Game"] = true;
                            _joinedTables["Purchases"] = true;
                            break;
                        case "Achievement":
                            _joinedTables["Game"] = true;
                            _joinedTables["Achievement"] = true;
                            break;
                        case "Receive_achievements":
                            _joinedTables["Game"] = true;
                            _joinedTables["Achievement"] = true;
                            _joinedTables["Receive_achievements"] = true;
                            break;
                    }
                    break;
                case "Report":
                    switch (secondTable)
                    {
                        case "Account":
                            _joinedTables["Account"] = true;
                            break;
                        case "Game":
                            _joinedTables["Account"] = true;
                            _joinedTables["Purchases"] = true;
                            _joinedTables["Game"] = true;
                            break;
                        case "Games_shop":
                            _joinedTables["Account"] = true;
                            _joinedTables["Purchases"] = true;
                            _joinedTables["Game"] = true;
                            _joinedTables["Games_shop"] = true;
                            break;
                        case "Report":
                            _joinedTables["Report"] = true;
                            break;
                        case "Purchases":
                            _joinedTables["Account"] = true;
                            _joinedTables["Purchases"] = true;
                            break;
                        case "Achievement":
                            _joinedTables["Account"] = true;
                            _joinedTables["Receive_achievements"] = true;
                            _joinedTables["Achievement"] = true;
                            break;
                        case "Receive_achievements":
                            _joinedTables["Account"] = true;
                            _joinedTables["Receive_achievements"] = true;
                            break;
                    }
                    break;
                case "Purchases":
                    switch (secondTable)
                    {
                        case "Account":
                            _joinedTables["Account"] = true;
                            break;
                        case "Game":
                            _joinedTables["Game"] = true;
                            break;
                        case "Games_shop":
                            _joinedTables["Game"] = true;
                            _joinedTables["Games_shop"] = true;
                            break;
                        case "Report":
                            _joinedTables["Account"] = true;
                            _joinedTables["Report"] = true;
                            break;
                        case "Purchases":
                            _joinedTables["Purchases"] = true;
                            break;
                        case "Achievement":
                            _joinedTables["Game"] = true;
                            _joinedTables["Achievement"] = true;
                            break;
                        case "Receive_achievements":
                            _joinedTables["Account"] = true;
                            _joinedTables["Receive_achievements"] = true;
                            break;
                    }
                    break;
                case "Achievement":
                    switch (secondTable)
                    {
                        case "Account":
                            _joinedTables["Receive_achievements"] = true;
                            _joinedTables["Account"] = true;
                            break;
                        case "Game":
                            _joinedTables["Game"] = true;
                            break;
                        case "Games_shop":
                            _joinedTables["Game"] = true;
                            _joinedTables["Games_shop"] = true;
                            break;
                        case "Report":
                            _joinedTables["Receive_achievements"] = true;
                            _joinedTables["Account"] = true;
                            _joinedTables["Report"] = true;
                            break;
                        case "Purchases":
                            _joinedTables["Game"] = true;
                            _joinedTables["Purchases"] = true;
                            break;
                        case "Achievement":
                            _joinedTables["Achievement"] = true;
                            break;
                        case "Receive_achievements":
                            _joinedTables["Receive_achievements"] = true;
                            break;
                    }
                    break;
                case "Receive_achievements":
                    switch (secondTable)
                    {
                        case "Account":
                            _joinedTables["Account"] = true;
                            break;
                        case "Game":
                            _joinedTables["Achievement"] = true;
                            _joinedTables["Game"] = true;
                            break;
                        case "Games_shop":
                            _joinedTables["Achievement"] = true;
                            _joinedTables["Game"] = true;
                            _joinedTables["Games_shop"] = true;
                            break;
                        case "Report":
                            _joinedTables["Account"] = true;
                            _joinedTables["Report"] = true;
                            break;
                        case "Purchases":
                            _joinedTables["Account"] = true;
                            _joinedTables["Purchases"] = true;
                            break;
                        case "Achievement":
                            _joinedTables["Achievement"] = true;
                            break;
                        case "Receive_achievements":
                            _joinedTables["Receive_achievements"] = true;
                            break;
                    }
                    break;
            }
            return _metaData[_mainTable][secondTable];
        }
        private void UpdateSelectPart()
        {
            _selectPart = string.Empty;
            _selectPart = "SELECT\n";
            for (int i = 0; i < UsedFieldsList.Count; i++)
            {
                if (i == 0)
                    _selectPart += "    " + UsedFieldsList[i];
                else
                    _selectPart += ",\n    " + UsedFieldsList[i];
            }
        }
        private void UpdateFromPart()
        {
            _fromPart = string.Empty;
            _fromPart += "\nFROM\n    " + _mainTable+"\n";
        }
        private void UpdateJoinPart()
        {
            _joinPart = string.Empty;
            _joinedTables["Account"] = false;
            _joinedTables["Game"] = false;
            _joinedTables["Games_shop"] = false;
            _joinedTables["Report"] = false;
            _joinedTables["Achievement"] = false;
            _joinedTables["Purchases"] = false;
            _joinedTables["Receive_achievements"] = false;
            int count = 0;
            foreach (var s in _tablesUsedInQuery)
                if (s.Value)
                    count++;
            if (count == 1)
                return;            
            for (int i = 1; i < UsedFieldsList.Count; i++)
            {
                string secondTableName = GetTableName(UsedFieldsList[i]);
                if (_mainTable != secondTableName && !_joinedTables[secondTableName])
                {
                    List<string> tempGetJoinTables = GetJoinTables(secondTableName);
                    foreach (var s in tempGetJoinTables)
                        if (!_joinPart.Contains(s))
                            _joinPart += s;
                }
            }
        }
        private void UpdateWherePart()
        {
            if (ConditionList.Count != 0)
                _wherePart = "WHERE\n";
            else
                _wherePart = string.Empty;
            foreach (var s in ConditionList)
                _wherePart += "    "+s.ToString()+'\n';
        }        
    }
}