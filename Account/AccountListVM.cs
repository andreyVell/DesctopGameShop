using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class AccountListVM:ViewModelBase
    {
        public TablesAdminVM _tablesAdminVM;
        private Window _parentWindow;
        public AccountListVM(TablesAdminVM tablesAdminVM, Window parentWindow)
        {
            _parentWindow = parentWindow;
            _tablesAdminVM = tablesAdminVM;
               AccountList = new ObservableCollection<AccountVM>();

            //search stuff
            ReserveAccountList = new ObservableCollection<AccountVM>();
            VisibleSearchTypes = new ObservableCollection<string>();
            //search stuff

            var accountListVM = DataProvider.Default.GetAccountListPlayer().
                Select(e => new AccountVM(e, this));
            foreach (var p in accountListVM)
            {
                if (p.account_balance > maxPrice)
                    maxPrice = p.account_balance;
                AccountList.Add(p);
                ReserveAccountList.Add(p);
            }
            if (ReserveAccountList.Count() == 1 && ReserveAccountList[0].account_role == "Player")
                SelectedAccount = ReserveAccountList[0];

            //search stuff
            VisibleSearchTypes.Add("ID");
            VisibleSearchTypes.Add("Login");
            VisibleSearchTypes.Add("Password");
            VisibleSearchTypes.Add("Nickname");
            VisibleSearchTypes.Add("E-mail");
            VisibleSearchTypes.Add("Contry");
            VisibleSearchTypes.Add("Balance");
            VisibleSearchTypes.Add("Birth date");
            VisibleSearchTypes.Add("Role");
            _filter = string.Empty;
            _searchType = ESearchType.id;
            _balanceFrom = 0;
            _balanceTo = maxPrice;
            //search stuff

        }
        public ObservableCollection<AccountVM> AccountList { get; set; }
        public AccountVM SelectedAccount { get; set; }

        private RelayCommand _deleteAccountCommand;
        public RelayCommand DeleteAccountCommand =>
            _deleteAccountCommand ?? (_deleteAccountCommand = new RelayCommand(DeleteAccount));
        public void DeleteAccount()
        {
            try
            {
                if (SelectedAccount != null)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this account?(All reports, purchases and received achievements associated with this account will also be deleted)", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes)
                    {
                        return;
                    }                    
                    if (SelectedAccount.account_role == "Admin" && GetAdminAccountsCount()==1)
                    {
                        MessageBox.Show("This is the last admin account, you can't delete it!");
                        return;
                    }
                    bool curAccount = DataProvider.Default.IsAccountCurrent(SelectedAccount.Account);
                    //удалить из БД
                    DataProvider.Default.DeleteAccount(SelectedAccount.Account);
                    if (curAccount)
                    {
                        DataProvider.Default.CloseConnection();
                        _parentWindow.Hide();
                        LoginWindow login = new LoginWindow();
                        login.Show();
                        _parentWindow.Close();
                    }
                    //удалить из UI
                    ReserveAccountList.Remove(SelectedAccount);
                    AccountList.Remove(SelectedAccount);
                    _tablesAdminVM.AccountUpdate();
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private RelayCommand _addAccountCommand;
        public RelayCommand AddAccountCommand =>
            _addAccountCommand ?? (_addAccountCommand = new RelayCommand(AddAccount));
        public void AddAccount()
        {
            AddAccountWindow addAccountWindow = new AddAccountWindow(this);
            addAccountWindow.Show();
        }        
        private int GetAdminAccountsCount()
        {
            int count = 0;
            foreach (var acc in ReserveAccountList)
                if (acc.account_role == "Admin")
                    count++;
            return count;
        }




        //search stuff
        enum ESearchType
        {
            id = 0,
            login = 1,
            password = 2,
            nickname = 3,
            email = 4,
            country = 5,
            balance = 6,
            birthdate = 7,
            role = 8,
        }
        private string _filter;
        private ESearchType _searchType;
        private double _balanceFrom;
        private double _balanceTo;
        private DateTime _dateFilter;
        private double maxPrice = 0;
        public ObservableCollection<string> VisibleSearchTypes { get; set; }
        public ObservableCollection<AccountVM> ReserveAccountList { get; set; }
        public string filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (value == null)
                    _filter = string.Empty;
                else
                    _filter = value;
                UpdateFilteredAccountList();
            }
        }
        public double balanceFromFilter
        {
            get
            {
                return _balanceFrom;
            }
            set
            {
                _balanceFrom = value;
                UpdateFilteredAccountList();
            }
        }
        public double balanceToFilter
        {
            get
            {
                return _balanceTo;
            }
            set
            {
                _balanceTo = value;
                UpdateFilteredAccountList();
            }
        }
        public DateTime dateFilter
        {
            get
            {
                return _dateFilter;
            }
            set
            {

                _dateFilter = value;
                UpdateFilteredAccountList();
            }
        }
        public int searchType
        {
            get
            {
                return ((int)_searchType);
            }
            set
            {
                _searchType = (ESearchType)value;
                filter = string.Empty;
            }
        }
        public void UpdateFilteredAccountList()
        {

            if (_searchType != ESearchType.balance && _searchType != ESearchType.birthdate && _filter == "")
            {
                AccountList.Clear();
                foreach (var game in ReserveAccountList)
                    AccountList.Add(game);
                return;
            }
            switch (_searchType)
            {
                case ESearchType.id:
                    try
                    {
                        AccountList.Clear();
                        foreach (var game in ReserveAccountList)
                            if (game.account_id == Convert.ToInt32(_filter))
                                AccountList.Add(game);
                    }
                    catch { MessageBox.Show("Incorrect ID format!"); }
                    break;                
                case ESearchType.balance:
                    if (_balanceFrom > _balanceTo)
                    {
                        double temp = _balanceFrom;
                        _balanceFrom = _balanceTo;
                        _balanceTo = temp;
                    }
                    AccountList.Clear();
                    foreach (var game in ReserveAccountList)
                        if (_balanceFrom <= game.account_balance && game.account_balance <= _balanceTo)
                            AccountList.Add(game);
                    break;
                case ESearchType.login:
                    AccountList.Clear();
                    foreach (var game in ReserveAccountList)
                        if (game.account_login.ToLower().Contains(_filter.ToLower()))
                            AccountList.Add(game);
                    break;
                case ESearchType.password:
                    AccountList.Clear();
                    foreach (var game in ReserveAccountList)
                        if (game.account_password.ToLower().Contains(_filter.ToLower()))
                            AccountList.Add(game);
                    break;
                case ESearchType.nickname:
                    AccountList.Clear();
                    foreach (var game in ReserveAccountList)
                        if (game.account_nickname.ToLower().Contains(_filter.ToLower()))
                            AccountList.Add(game);
                    break;
                case ESearchType.email:
                    AccountList.Clear();
                    foreach (var game in ReserveAccountList)
                        if (game.account_email.ToLower().Contains(_filter.ToLower()))
                            AccountList.Add(game);
                    break;
                case ESearchType.country:
                    AccountList.Clear();
                    foreach (var game in ReserveAccountList)
                        if (game.account_country.ToLower().Contains(_filter.ToLower()))
                            AccountList.Add(game);
                    break;
                case ESearchType.role:
                    AccountList.Clear();
                    foreach (var game in ReserveAccountList)
                        if (game.account_role.ToLower().Contains(_filter.ToLower()))
                            AccountList.Add(game);
                    break;
                case ESearchType.birthdate:
                    AccountList.Clear();
                    foreach (var game in ReserveAccountList)
                        if (dateFilter.Date == game.account_birthdate.Date)
                            AccountList.Add(game);
                    break;
                default:
                    return;
            }
        }
        //search stuff
    }
}