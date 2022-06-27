using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DataBaseIndTask.CustomQueryBuilder
{
    class AddNewConditionVM:ViewModelBase
    {
        private AddNewConditionWindow _parentWindow;
        private ObservableCollection<string> _tablesUsedInQuery;
        private ObservableCollection<string> _curField;
        private ObservableCollection<string> _criterionList;
        private ObservableCollection<string> _linkList;
        private ObservableCollection<ConditionVM> _conditionList;
        private List<string> _accountFileds;
        private List<string> _gameFileds;
        private List<string> _gamesShopFileds;
        private List<string> _reportFileds;
        private List<string> _purchasesFileds;
        private List<string> _achievementFileds;
        private List<string> _receiveAchievementsFileds;
        private string _table;
        private string _field;
        private string _criterion;
        private string _link;
        private string _value;
        private DateTime _valueDate;
        private int _valueType; //0-string 1-DateTime
        public AddNewConditionVM(ObservableCollection<string> tablesUsedInQuery, ObservableCollection<ConditionVM> ConditionList, AddNewConditionWindow parentWindow)
        {
            _tablesUsedInQuery = tablesUsedInQuery;
            _conditionList = ConditionList;
            _parentWindow = parentWindow;
            #region metadata tables
            _valueDate = DateTime.Today;
            _table = string.Empty;
            _field = string.Empty;
            _criterion = string.Empty;
            _link = string.Empty;
            _value = string.Empty;
            _curField = new ObservableCollection<string>();
            _criterionList = new ObservableCollection<string>();
            _criterionList.Add("=");
            _criterionList.Add(">");
            _criterionList.Add(">=");
            _criterionList.Add("<");
            _criterionList.Add("<=");
            _criterionList.Add("<>");
            _linkList = new ObservableCollection<string>();
            _linkList.Add("AND");
            _linkList.Add("OR");
            _accountFileds = new List<string>();
            _gameFileds = new List<string>();
            _gamesShopFileds = new List<string>();
            _reportFileds = new List<string>();
            _purchasesFileds = new List<string>();
            _achievementFileds = new List<string>();
            _receiveAchievementsFileds = new List<string>();
            _accountFileds.Add("account_id");
            _accountFileds.Add("account_login");
            _accountFileds.Add("account_password");
            _accountFileds.Add("account_nickname");
            _accountFileds.Add("account_email");
            _accountFileds.Add("account_country");
            _accountFileds.Add("account_balance");
            _accountFileds.Add("account_birthdate");
            _accountFileds.Add("account_role");
            _accountFileds.Add("account_image");

            _gameFileds.Add("game_id");
            _gameFileds.Add("game_name");
            _gameFileds.Add("game_description");
            _gameFileds.Add("game_genre");
            _gameFileds.Add("game_release_date");
            _gameFileds.Add("game_developer");
            _gameFileds.Add("game_publisher");
            _gameFileds.Add("game_rating");
            _gameFileds.Add("game_agelimit");
            _gameFileds.Add("game_image");
            _gameFileds.Add("game_storeimage");

            _gamesShopFileds.Add("game_id");
            _gamesShopFileds.Add("game_price");

            _reportFileds.Add("report_id");
            _reportFileds.Add("report_comment");
            _reportFileds.Add("report_receive_date");
            _reportFileds.Add("report_sender_id");
            _reportFileds.Add("report_receiver_id");

            _purchasesFileds.Add("purchase_id");
            _purchasesFileds.Add("purchaser_id");
            _purchasesFileds.Add("purchase_game_id");
            _purchasesFileds.Add("purchase_game_name");
            _purchasesFileds.Add("purchase_purchaser_login");
            _purchasesFileds.Add("purchase_price");
            _purchasesFileds.Add("purchase_date");

            _achievementFileds.Add("achievement_id");
            _achievementFileds.Add("achievement_name");
            _achievementFileds.Add("achievement_description");
            _achievementFileds.Add("achievement_game_id");
            _achievementFileds.Add("achievement_image");

            _receiveAchievementsFileds.Add("achievement_id");
            _receiveAchievementsFileds.Add("account_id");
            _receiveAchievementsFileds.Add("receive_achievement_date");
            #endregion
        }
        public ObservableCollection<string> TablesList
        {
            get
            {
                return _tablesUsedInQuery;
            }
        }
        public ObservableCollection<string> CurField
        {
            get { return _curField; }
        }
        public ObservableCollection<string> CriterionList
        {
            get
            {
                return _criterionList;
            }
        }
        public ObservableCollection<string> LinkList
        {
            get
            {
                return _linkList;
            }
        }
        public string Table 
        { 
            get
            {
                return _table;
            }
            set
            {
                _table = value;
                _field = string.Empty;
                UpdateCurFields();
            }
        }
        public string Field
        {
            get
            {
                return _field;
            }
            set
            {
                _field = value;
                UpdateValueType();
            }
        }
        public string Criterion
        {
            get
            {
                return _criterion;
            }
            set
            {
                _criterion = value;
            }
        }
        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                _link = value;
            }
        }
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        private DateTime ValueDate
        {
            get { return _valueDate; }
            set { _valueDate = value; }
        }
        private void UpdateCurFields()
        {
            _curField.Clear();
            switch (_table)
            {
                case "Account":
                    foreach (var s in _accountFileds)
                        _curField.Add(s);
                    break;
                case "Game":
                    foreach (var s in _gameFileds)
                        _curField.Add(s);
                    break;
                case "Games_shop":
                    foreach (var s in _gamesShopFileds)
                        _curField.Add(s);
                    break;
                case "Report":
                    foreach (var s in _reportFileds)
                        _curField.Add(s);
                    break;
                case "Purchases":
                    foreach (var s in _purchasesFileds)
                        _curField.Add(s);
                    break;
                case "Achievement":
                    foreach (var s in _achievementFileds)
                        _curField.Add(s);
                    break;
                case "Receive_achievements":
                    foreach (var s in _receiveAchievementsFileds)
                        _curField.Add(s);
                    break;
            }
            RaisePropertyChanged(nameof(CurField));
        }
        private void UpdateValueType()
        {
            switch (_field)
            {
                case "account_birthdate":
                    _valueType = 1;
                    _parentWindow.ValueTextBox.Visibility = Visibility.Hidden;
                    _parentWindow.ValueDatePicker.Visibility = Visibility.Visible;
                    break;
                case "game_release_date":
                    _valueType = 1;
                    _parentWindow.ValueTextBox.Visibility = Visibility.Hidden;
                    _parentWindow.ValueDatePicker.Visibility = Visibility.Visible;
                    break;
                case "report_receive_date":
                    _valueType = 1;
                    _parentWindow.ValueTextBox.Visibility = Visibility.Hidden;
                    _parentWindow.ValueDatePicker.Visibility = Visibility.Visible;
                    break;
                case "purchase_date":
                    _valueType = 1;
                    _parentWindow.ValueTextBox.Visibility = Visibility.Hidden;
                    _parentWindow.ValueDatePicker.Visibility = Visibility.Visible;
                    break;
                case "receive_achievement_date":
                    _valueType = 1;
                    _parentWindow.ValueTextBox.Visibility = Visibility.Hidden;
                    _parentWindow.ValueDatePicker.Visibility = Visibility.Visible;
                    break;
                default:
                    _valueType = 0;
                    _parentWindow.ValueTextBox.Visibility = Visibility.Visible;
                    _parentWindow.ValueDatePicker.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private RelayCommand _addConditionCommand;
        public RelayCommand AddConditionCommand =>
            _addConditionCommand ?? (_addConditionCommand = new RelayCommand(AddCondition));
        public void AddCondition()
        {
            if (_table==string.Empty || _field==string.Empty || _criterion==string.Empty)
            {
                MessageBox.Show("Fill all fields!");
                return;
            }
            if (_valueType==0 && _value==string.Empty)
            {
                MessageBox.Show("Fill value!");
                return;
            }
            ConditionVM newCondition=null;
            if (_valueType==0)
                newCondition = new ConditionVM(_table,_field,_criterion,_link,_value);
            if (_valueType == 1)
                newCondition = new ConditionVM(_table, _field, _criterion, _link, _valueDate);
            if (newCondition!=null)
            {
                _conditionList.Add(newCondition);
                _parentWindow._customQuaryBuilderVM.UpdateSQLQuery();
                _parentWindow.Close();
            }

        }
    }
}