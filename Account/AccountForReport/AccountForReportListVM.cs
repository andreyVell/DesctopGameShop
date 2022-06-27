using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataBaseIndTask
{
    public class AccountForReportListVM : ViewModelBase
    {
        private string _searchFilter;
        public AccountForReportListVM()
        {
            AccountForReportList = new ObservableCollection<AccountForReportVM>();
            FilteredAccountForReportList = new ObservableCollection<AccountForReportVM>();
            ReserveAccountForReportList = new ObservableCollection<AccountForReportVM>();
            var accountForReportListVM = DataProvider.Default.GetAccountForReport().
                Select(e => new AccountForReportVM(e, this));
            foreach (var p in accountForReportListVM)
            { 
                AccountForReportList.Add(p);
                ReserveAccountForReportList.Add(p);
            }
        }
        public ObservableCollection<AccountForReportVM> AccountForReportList
        {
            get; set;
        }
        public ObservableCollection<AccountForReportVM> FilteredAccountForReportList
        {
            get; set;
        }
        public ObservableCollection<AccountForReportVM> ReserveAccountForReportList
        {
            get; set;
        }
        public string searchFilter
        {
            get
            {
                return _searchFilter;
            }
            set
            {
                if (value == null)
                    _searchFilter = string.Empty;
                else
                    _searchFilter = value;
                if (_searchFilter == "")
                {
                    AccountForReportList.Clear();
                    foreach (var game in ReserveAccountForReportList)
                        AccountForReportList.Add(game);
                }
                else
                {
                    AccountForReportList.Clear();
                    UpdateFilteredAccountForReportList();
                    foreach (var game in FilteredAccountForReportList)
                        AccountForReportList.Add(game);
                }
            }
        }
        public void UpdateFilteredAccountForReportList()
        {
            FilteredAccountForReportList.Clear();
            foreach (var game in ReserveAccountForReportList)
                if (game.account_nickname.ToLower().Contains(searchFilter.ToLower()))
                    FilteredAccountForReportList.Add(game);
        }
    }
}