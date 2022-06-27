using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class GameInStoreListVM : ViewModelBase
    {
        enum SearchType
        {            
            game_name = 0,
            game_genre = 1,
            game_price = 2,
        }
        private string _searchFilter;
        private SearchType _searchType;
        private int _priceFrom;
        private int _priceTo;
        private AccountListVM _accountListVM { get; set; }
        private mainWindowPlayer _parentWindow;
        private GameInLibraryListVM _gameInLibraryListVM { get; set; }
        public GameInStoreListVM(AccountListVM accountListVM, GameInLibraryListVM gameInLibraryListVM, mainWindowPlayer window)
        {
            _parentWindow = window;
            _accountListVM = accountListVM;
            _gameInLibraryListVM = gameInLibraryListVM;
            GameInStoreList = new ObservableCollection<GameInStoreVM>();
            FilteredGameInStoreList = new ObservableCollection<GameInStoreVM>();
            ReserveGameInStoreList = new ObservableCollection<GameInStoreVM>();
            var gameInStoreListVM = DataProvider.Default.GetGameInStoreList().
                Select(e => new GameInStoreVM(e, this));
            int maxPrice = 0;
            foreach (var p in gameInStoreListVM)
            {
                if (p.game_price > maxPrice)
                    maxPrice = p.game_price;
                GameInStoreList.Add(p);
                ReserveGameInStoreList.Add(p);
            }
            _searchFilter = string.Empty;
            _searchType = SearchType.game_name;
            searchTypes = new ObservableCollection<string>();
            _priceFrom = 0;
            _priceTo = maxPrice;
            searchTypes.Add("Name");
            searchTypes.Add("Genre");
            searchTypes.Add("Price");
        }
        public ObservableCollection<GameInStoreVM> GameInStoreList
        {
            get; set;
        }
        public ObservableCollection<GameInStoreVM> FilteredGameInStoreList
        {
            get; set;
        }
        public ObservableCollection<GameInStoreVM> ReserveGameInStoreList
        {
            get; set;
        }
        public GameInStoreVM SelectedGameInStore { get; set; }
        public ObservableCollection<string> searchTypes { get; set; }

        public int priceFromFilter
        {
            get
            {
                return _priceFrom;
            }
            set
            {
                _priceFrom = value;                
                GameInStoreList.Clear();
                UpdateFilteredGameInStoreList();
                foreach (var game in FilteredGameInStoreList)
                    GameInStoreList.Add(game);
            }
        }
        public int priceToFilter
        {
            get
            {
                return _priceTo;
            }
            set
            {
                _priceTo = value;                
                GameInStoreList.Clear();
                UpdateFilteredGameInStoreList();
                foreach (var game in FilteredGameInStoreList)
                    GameInStoreList.Add(game);
            }
        }
        public string filter
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
                    GameInStoreList.Clear();
                    foreach (var game in ReserveGameInStoreList)
                        GameInStoreList.Add(game);
                }
                else
                {
                    GameInStoreList.Clear();
                    UpdateFilteredGameInStoreList();
                    foreach (var game in FilteredGameInStoreList)
                        GameInStoreList.Add(game);
                }
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
                _searchType = (SearchType)value;
                if (_searchFilter == "")
                {
                    GameInStoreList.Clear();
                    foreach (var game in ReserveGameInStoreList)
                        GameInStoreList.Add(game);
                }
                else
                {
                    GameInStoreList.Clear();
                    UpdateFilteredGameInStoreList();
                    foreach (var game in FilteredGameInStoreList)
                        GameInStoreList.Add(game);
                }
            }
        }
        public void UpdateFilteredGameInStoreList()
        {
            switch (_searchType)
            {
                case SearchType.game_name:
                    {
                        FilteredGameInStoreList.Clear();
                        foreach (var game in ReserveGameInStoreList)
                            if (game.game_name.ToLower().Contains(filter.ToLower()))
                                FilteredGameInStoreList.Add(game);
                    }
                    break;
                case SearchType.game_genre:
                    {
                        FilteredGameInStoreList.Clear();
                        foreach (var game in ReserveGameInStoreList)
                            if (game.game_genre.ToLower().Contains(filter.ToLower()))
                                FilteredGameInStoreList.Add(game);
                    }
                    break;
                case SearchType.game_price:
                    {
                        if (_priceFrom > _priceTo)
                        {
                            int temp = _priceFrom;
                            _priceFrom = _priceTo;
                            _priceTo = temp;
                        }
                        FilteredGameInStoreList.Clear();
                        foreach (var game in ReserveGameInStoreList)
                            if (_priceFrom <= game.game_price && game.game_price <= _priceTo)
                                FilteredGameInStoreList.Add(game);
                    }
                    break;
                default:
                    break;
            }
        }

        RelayCommand _buyGameCommand;
        public RelayCommand BuyGameCommand =>
            _buyGameCommand ?? (_buyGameCommand = new RelayCommand(BuyGame));
        public void BuyGame()
        {
            //get balance
            double balance = _accountListVM.ReserveAccountList[0].account_balance;
            //get age
            DateTime now = DateTime.Today;
            DateTime birthDate = Convert.ToDateTime(_accountListVM.ReserveAccountList[0].account_birthdate);
            int age = now.Year - birthDate.Year - 1 + ((now.Month > birthDate.Month || now.Month == birthDate.Month && now.Day >= birthDate.Day) ? 1 : 0);
            //get game
            bool isThisGameAlreadyBought = false;
            int store_gameid = SelectedGameInStore.game_id;
            foreach (var game in _gameInLibraryListVM.GameInLibraryList)
                if (game.purchase_game_id == store_gameid)
                    isThisGameAlreadyBought = true;
            //balance check
            if (SelectedGameInStore.game_price > balance)
            {
                MessageBox.Show("Not enough money", "Transaction error");
                return;
            }
            //age check
            if (SelectedGameInStore.game_agelimit > age)
            {
                MessageBox.Show("You're too young", "Age limit error");
                return;
            }
            //game check
            if (isThisGameAlreadyBought)
            {
                MessageBox.Show("You already bought this game", "Transaction error");
                return;
            }
            //decrease the balance
            _accountListVM.ReserveAccountList[0].account_balance -= SelectedGameInStore.game_price;
            //update database
            Purchase purchase = new Purchase()
            {
                purchaser_id = _accountListVM.ReserveAccountList[0].account_id,
                purchase_game_id = SelectedGameInStore.game_id,
                purchase_price = SelectedGameInStore.game_price,
                purchase_date = DateTime.Now,
            };
            DataProvider.Default.AddPurchase(purchase);
            //update UI
            GameInLibrary newGameInLibrary = new GameInLibrary()
            {
                purchase_id = DataProvider.Default.GetLastPurchaseID(),
                purchaser_id = _accountListVM.ReserveAccountList[0].account_id,
                purchase_game_id = SelectedGameInStore.game_id,
                purchase_price = SelectedGameInStore.game_price,
                purchase_date = DateTime.Now,
                game_name = SelectedGameInStore.game_name,
                game_description = SelectedGameInStore.game_description,
                game_genre = SelectedGameInStore.game_genre,
                game_releasedate = SelectedGameInStore.game_releasedate,
                game_developer = SelectedGameInStore.game_developer,
                game_publisher = SelectedGameInStore.game_publisher,
                game_rating = SelectedGameInStore.game_rating,
                game_agelimit = SelectedGameInStore.game_agelimit,
                game_image = SelectedGameInStore.game_image,
            };
            GameInLibraryVM newGameInLibraryVM = new GameInLibraryVM(newGameInLibrary, _gameInLibraryListVM);
            _gameInLibraryListVM.GameInLibraryList.Add(newGameInLibraryVM);
            _gameInLibraryListVM.ReserveGameInLibraryList.Add(newGameInLibraryVM);
            _gameInLibraryListVM.PurchaseList.Add(newGameInLibraryVM);
            _gameInLibraryListVM.ReservePurchaseList.Add(newGameInLibraryVM);
            //updete filtered list in library
            _gameInLibraryListVM.filter = _gameInLibraryListVM.filter;
            //updete filtered list in purchases
            _gameInLibraryListVM.purchaseFilter = _gameInLibraryListVM.purchaseFilter;
            _parentWindow.ShowLibraryContent();
            MessageBox.Show("The game has been added to your library", "Successfully");            
        }
    }
}