using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class GameInLibraryListVM : ViewModelBase
    {
        enum SearchPurchaseType
        {
            id = 0,
            product = 1,
            price = 2,
            date = 3,
        }
        private AchievementListVM _achievementListVM;
        private ReceiveAchievementListVM _receiveAchievementListVM;
        private string _searchfilter;
        private string _searchPurchaseFilter;
        private SearchPurchaseType _searchPurchaseType;
        private double _pricePurchaseFrom;
        private double _pricePurchaseTo;
        private DateTime _purchaseDateFilter;
        public GameInLibraryListVM(AchievementListVM achievementListVM, ReceiveAchievementListVM receiveAchievementListVM)
        {
            _achievementListVM = achievementListVM;
            _receiveAchievementListVM = receiveAchievementListVM;
            GameInLibraryList = new ObservableCollection<GameInLibraryVM>();
            FilteredGameInLibraryList = new ObservableCollection<GameInLibraryVM>();
            ReserveGameInLibraryList = new ObservableCollection<GameInLibraryVM>();
            PurchaseList = new ObservableCollection<GameInLibraryVM>();
            FilteredPurchaseList = new ObservableCollection<GameInLibraryVM>();
            ReservePurchaseList = new ObservableCollection<GameInLibraryVM>();
            var gameInLibraryListVM = DataProvider.Default.GetGameInLibraryList().Select(e => new GameInLibraryVM(e, this));
            double maxPrice = 0;
            foreach (var p in gameInLibraryListVM)
            {
                if (p.purchase_price > maxPrice)
                    maxPrice = p.purchase_price;
                GameInLibraryList.Add(p);
                ReserveGameInLibraryList.Add(p);
                PurchaseList.Add(p);
                ReservePurchaseList.Add(p);
            }
            _searchPurchaseFilter = string.Empty;
            _searchPurchaseType = SearchPurchaseType.id;
            _searchfilter = string.Empty;
            searchPurchaseTypes = new ObservableCollection<string>();
            searchPurchaseTypes.Add("ID");
            searchPurchaseTypes.Add("Product");
            searchPurchaseTypes.Add("Price");
            searchPurchaseTypes.Add("Date");
            _pricePurchaseFrom = 0;
            _pricePurchaseTo = maxPrice;
        }
        public ObservableCollection<GameInLibraryVM> GameInLibraryList
        {
            get; set;
        }
        public ObservableCollection<GameInLibraryVM> FilteredGameInLibraryList
        {
            get; set;
        }
        public ObservableCollection<GameInLibraryVM> ReserveGameInLibraryList
        {
            get; set;
        }
        public ObservableCollection<GameInLibraryVM> PurchaseList { get; set; }
        public ObservableCollection<GameInLibraryVM> FilteredPurchaseList { get; set; }
        public ObservableCollection<GameInLibraryVM> ReservePurchaseList { get; set; }
        public ObservableCollection<string> searchPurchaseTypes { get; set; }
        public GameInLibraryVM SelectedGameInLibrary { get; set; }
        
        RelayCommand _playGameCommand;
        public RelayCommand PlayGameCommand =>
            _playGameCommand ?? (_playGameCommand = new RelayCommand(PlayGame));

        private void PlayGame()
        {
            MessageBox.Show("Game is playing!", SelectedGameInLibrary.GameInLibrary.game_name);
            List<int> unreceivedAchievementIDList = GetUnreceivedAchievementIDList();
            if (unreceivedAchievementIDList.Count() == 0)
                return;
            //выбрать случайную неполученную ачивку в выбранной игре
            int unreceivedAchievementID = unreceivedAchievementIDList[new Random().Next(unreceivedAchievementIDList.Count())];
            //добавить выбранную ачивку в список полученных в БД
            DataProvider.Default.AddReceivedAchievement(unreceivedAchievementID);
            //добавить выбранную ачивку в список полученных в UI
            ReceiveAchievement receiveAchievement = DataProvider.Default.GetReceiveAchievement(unreceivedAchievementID);
            _receiveAchievementListVM.ReceiveAchievementList.Add(new ReceiveAchievementVM(receiveAchievement, _receiveAchievementListVM));
            _receiveAchievementListVM.ReserveReceiveAchievementList.Add(new ReceiveAchievementVM(receiveAchievement, _receiveAchievementListVM));
            //обновляем фильтр
            _receiveAchievementListVM.filter = _receiveAchievementListVM.filter;
            MessageBox.Show("Сongrats! You just got the achievement: " + receiveAchievement.receiveAchievement_name, "New Achievement");
        }
        private List<int> GetUnreceivedAchievementIDList()
        {
            IEnumerable<int> unreceivedAchievementIDsTEMP;
            List<int> receivedAchievementIDs = GetReceivedAchievementID();
            List<int> allAchievementIDs = GetAllAchievementID();
            unreceivedAchievementIDsTEMP = allAchievementIDs.Except(receivedAchievementIDs);
            List<int> unreceivedAchievementIDs = unreceivedAchievementIDsTEMP.ToList();
            return unreceivedAchievementIDs;
        }
        private List<int> GetReceivedAchievementID()
        {
            List<int> receivedAchievementIDs = new List<int>();
            foreach (var achi in _receiveAchievementListVM.ReceiveAchievementList)
                if (achi.receiveAchievement_game_name==SelectedGameInLibrary.game_name)
                    receivedAchievementIDs.Add(achi.receiveAchievement_id);
            return receivedAchievementIDs;
        }
        private List<int> GetAllAchievementID()
        {
            List<int> allAchievementIDs = new List<int>();
            foreach (var achi in _achievementListVM.AchievementList)
                if (achi.achievement_game_name==SelectedGameInLibrary.game_name)
                    allAchievementIDs.Add(achi.achievement_id);
            return allAchievementIDs;
        }
        public string filter
        {
            get
            {
                return _searchfilter;
            }
            set
            {
                if (value == null)
                    _searchfilter = string.Empty;
                else
                    _searchfilter = value;
                if (_searchfilter == "")
                {                    
                    GameInLibraryList.Clear();
                    foreach (var game in ReserveGameInLibraryList)
                        GameInLibraryList.Add(game);
                }
                else
                {                    
                    GameInLibraryList.Clear();
                    UpdateFilteredGameInLibraryList();
                    foreach (var game in FilteredGameInLibraryList)
                        GameInLibraryList.Add(game);
                }
            }
        }
        private void UpdateFilteredGameInLibraryList()
        {
            FilteredGameInLibraryList.Clear();
            foreach (var game in ReserveGameInLibraryList)
                if (game.game_name.ToLower().Contains(filter.ToLower()))
                    FilteredGameInLibraryList.Add(game);
        }

        public string purchaseFilter
        {
            get
            {
                return _searchPurchaseFilter;
            }
            set
            {
                if (value == null)
                    _searchPurchaseFilter = string.Empty;
                else
                    _searchPurchaseFilter = value;
                if (_searchPurchaseFilter == "")
                {
                    PurchaseList.Clear();
                    foreach (var game in ReservePurchaseList)
                        PurchaseList.Add(game);
                }
                else
                {
                    PurchaseList.Clear();
                    UpdateFilteredPurchaseList();
                    foreach (var game in FilteredPurchaseList)
                        PurchaseList.Add(game);
                }
            }
        }
        public int searchPurchaseType
        {
            get
            {
                return ((int)_searchPurchaseType);
            }
            set
            {
                _searchPurchaseType = (SearchPurchaseType)value;
                if (_searchPurchaseFilter == "")
                {
                    PurchaseList.Clear();
                    foreach (var game in ReservePurchaseList)
                        PurchaseList.Add(game);
                }
                else
                {
                    PurchaseList.Clear();
                    UpdateFilteredPurchaseList();
                    foreach (var game in FilteredPurchaseList)
                        PurchaseList.Add(game);
                }
            }
        }
        public double pricePurchaseFromFilter
        {
            get
            {
                return _pricePurchaseFrom;
            }
            set
            {
                _pricePurchaseFrom = value;
                PurchaseList.Clear();
                UpdateFilteredPurchaseList();
                foreach (var game in FilteredPurchaseList)
                    PurchaseList.Add(game);
            }
        }
        public double pricePurchaseToFilter
        {
            get
            {
                return _pricePurchaseTo;
            }
            set
            {
                _pricePurchaseTo = value;
                PurchaseList.Clear();
                UpdateFilteredPurchaseList();
                foreach (var game in FilteredPurchaseList)
                    PurchaseList.Add(game);
            }
        }
        public DateTime purchaseDateFilter
        {
            get
            {
                return _purchaseDateFilter;
            }
            set
            {                
                _purchaseDateFilter = value;   
                PurchaseList.Clear();
                UpdateFilteredPurchaseList();
                foreach (var game in FilteredPurchaseList)
                    PurchaseList.Add(game);                
            }
        }
        public void UpdateFilteredPurchaseList()
        {
            switch (_searchPurchaseType)
            {
                case SearchPurchaseType.id:
                    {
                        try
                        {
                            FilteredPurchaseList.Clear();
                            foreach (var game in ReservePurchaseList)
                                if (game.purchase_id == Convert.ToInt32(purchaseFilter))
                                    FilteredPurchaseList.Add(game);
                        }
                        catch (Exception ex) { MessageBox.Show("Incorrect ID format!"); }
                    }
                    break;
                case SearchPurchaseType.product:
                    {
                        FilteredPurchaseList.Clear();
                        foreach (var game in ReservePurchaseList)
                            if (game.game_name.ToLower().Contains(purchaseFilter.ToLower()))
                                FilteredPurchaseList.Add(game);
                    }
                    break;
                case SearchPurchaseType.price:
                    {
                        if (_pricePurchaseFrom > _pricePurchaseTo)
                        {
                            double temp = _pricePurchaseFrom;
                            _pricePurchaseFrom = _pricePurchaseTo;
                            _pricePurchaseTo = temp;
                        }
                        FilteredPurchaseList.Clear();
                        foreach (var game in ReservePurchaseList)
                            if (_pricePurchaseFrom <= game.purchase_price && game.purchase_price <= _pricePurchaseTo)
                                FilteredPurchaseList.Add(game);
                    }
                    break;
                case SearchPurchaseType.date:
                    {                        
                        FilteredPurchaseList.Clear();
                        foreach (var game in ReservePurchaseList)
                            if (purchaseDateFilter.ToString("dd.MM.yyyy")==game.purchase_date.Substring(0,10))
                                FilteredPurchaseList.Add(game);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}