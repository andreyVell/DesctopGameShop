using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class PurchaseListVM:ViewModelBase
    {  
        public TablesAdminVM _tablesAdminVM;        
        public PurchaseListVM(TablesAdminVM tablesAdminVM)
        {
            _tablesAdminVM = tablesAdminVM;
            PurchaseList = new ObservableCollection<PurchaseVM>();

            //search stuff
            ReservePurchaseList = new ObservableCollection<PurchaseVM>();
            VisibleSearchTypes = new ObservableCollection<string>();
            //search stuff

            UpdatePurchaseList();

            //search stuff
            VisibleSearchTypes.Add("Purchase ID");
            VisibleSearchTypes.Add("User ID");
            VisibleSearchTypes.Add("User Login");
            VisibleSearchTypes.Add("Game ID");
            VisibleSearchTypes.Add("Game name");
            VisibleSearchTypes.Add("Price");
            VisibleSearchTypes.Add("Date");
            _filter = string.Empty;
            _searchType = ESearchType.purchase_id;
            _priceFrom = 0;
            _priceTo = maxPrice;
            //search stuff
        }
        public ObservableCollection<PurchaseVM> PurchaseList
        {
            get; set;
        }
        public PurchaseVM SelectedPurchase { get; set; }
        public void UpdatePurchaseList()
        {
            PurchaseList.Clear();
            ReservePurchaseList.Clear();
            var purchaseListVM = DataProvider.Default.GetPurchaseList().
                Select(e => new PurchaseVM(e, this));
            foreach (var p in purchaseListVM)
            {
                if (p.purchase_price > maxPrice)
                    maxPrice = p.purchase_price;
                PurchaseList.Add(p);
                ReservePurchaseList.Add(p);
            }
        }


        //search stuff
        enum ESearchType
        {
            purchase_id = 0,
            user_id = 1,
            user_login = 2,
            game_id = 3,
            game_name = 4,
            price = 5,
            date = 6,
        }
        private string _filter;
        private ESearchType _searchType;
        private double _priceFrom;
        private double _priceTo;
        private DateTime _dateFilter;
        private double maxPrice = 0;
        public ObservableCollection<string> VisibleSearchTypes { get; set; }
        public ObservableCollection<PurchaseVM> ReservePurchaseList { get; set; }
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
                UpdateFilteredPurchaseList();
            }
        }
        public double priceFromFilter
        {
            get
            {
                return _priceFrom;
            }
            set
            {
                _priceFrom = value;
                UpdateFilteredPurchaseList();
            }
        }
        public double priceToFilter
        {
            get
            {
                return _priceTo;
            }
            set
            {
                _priceTo = value;
                UpdateFilteredPurchaseList();
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
                UpdateFilteredPurchaseList();
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
        public void UpdateFilteredPurchaseList()
        {

            if (_searchType != ESearchType.price && _searchType != ESearchType.date && _filter == "")
            {
                PurchaseList.Clear();
                foreach (var game in ReservePurchaseList)
                    PurchaseList.Add(game);
                return;
            }
            switch (_searchType)
            {
                case ESearchType.purchase_id:
                    try
                    {
                        PurchaseList.Clear();
                        foreach (var game in ReservePurchaseList)
                            if (game.purchase_id == Convert.ToInt32(_filter))
                                PurchaseList.Add(game);
                    }
                    catch { MessageBox.Show("Incorrect ID format!"); }
                    break;
                case ESearchType.user_id:
                    try
                    {
                        PurchaseList.Clear();
                        foreach (var game in ReservePurchaseList)
                            if (game.purchaser_id == Convert.ToInt32(_filter))
                                PurchaseList.Add(game);
                    }
                    catch { MessageBox.Show("Incorrect ID format!"); }
                    break;
                case ESearchType.game_id:
                    try
                    {
                        PurchaseList.Clear();
                        foreach (var game in ReservePurchaseList)
                            if (game.purchase_game_id == Convert.ToInt32(_filter))
                                PurchaseList.Add(game);
                    }
                    catch { MessageBox.Show("Incorrect ID format!"); }
                    break;
                case ESearchType.price:
                    if (_priceFrom > _priceTo)
                    {
                        double temp = _priceFrom;
                        _priceFrom = _priceTo;
                        _priceTo = temp;
                    }
                    PurchaseList.Clear();
                    foreach (var game in ReservePurchaseList)
                        if (_priceFrom <= game.purchase_price && game.purchase_price <= _priceTo)
                            PurchaseList.Add(game);
                    break;
                case ESearchType.date:
                    PurchaseList.Clear();
                    foreach (var game in ReservePurchaseList)
                        if (dateFilter.Date == game.Purchase.purchase_date.Date)
                            PurchaseList.Add(game);
                    break;
                case ESearchType.user_login:
                    PurchaseList.Clear();
                    foreach (var game in ReservePurchaseList)
                        if (game.purchase_purchaser_login.ToLower().Contains(_filter.ToLower()))
                            PurchaseList.Add(game);
                    break;
                case ESearchType.game_name:
                    PurchaseList.Clear();
                    foreach (var game in ReservePurchaseList)
                        if (game.purchase_game_name.ToLower().Contains(_filter.ToLower()))
                            PurchaseList.Add(game);
                    break;
                default:
                    return;
            }
        }
        //search stuff
    }
}