using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class GameInStoreAdminListVM:ViewModelBase
    {
        enum ESearchType
        {
            name = 0,
            price = 1,
        }        
        private string _filter;
        private ESearchType _searchType;
        private double _priceFrom;
        private double _priceTo;
        public TablesAdminVM _tablesAdminVM;
        private double maxPrice = 0;
        public GameInStoreAdminListVM(TablesAdminVM tablesAdminVM)
        {            
            _tablesAdminVM = tablesAdminVM;
            unstoredGamesList = _tablesAdminVM.unstoredGamesList;
            GameInStoreAdminList = new ObservableCollection<GameInStoreAdminVM>();
            ReserveGameInStoreAdminList = new ObservableCollection<GameInStoreAdminVM>();
            UpdateGamesInStoreList();
            _filter = string.Empty;
            _searchType = ESearchType.name;

            VisibleSearchTypes = new ObservableCollection<string>();
            VisibleSearchTypes.Add("Name");
            VisibleSearchTypes.Add("Price");
            _priceFrom = 0;
            _priceTo = maxPrice;
        }
        public ObservableCollection<GameInStoreAdminVM> GameInStoreAdminList
        {
            get; set;
        }
        public ObservableCollection<GameInStoreAdminVM> ReserveGameInStoreAdminList
        {
            get; set;
        }
        public ObservableCollection<string> VisibleSearchTypes { get; set; }
        public GameInStoreAdminVM SelectedGameInStoreAdmin { get; set; }
        public ObservableCollection<GameVM> unstoredGamesList
        {
            get; set;
        }
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
                UpdateFilteredGameInStoreAdminList();
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
                UpdateFilteredGameInStoreAdminList();
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
                UpdateFilteredGameInStoreAdminList();
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
                if (filter != "")
                    UpdateFilteredGameInStoreAdminList();
            }
        }
        public void UpdateFilteredGameInStoreAdminList()
        {

            if (_searchType != ESearchType.price && _filter == "")
            {
                GameInStoreAdminList.Clear();
                foreach (var game in ReserveGameInStoreAdminList)
                    GameInStoreAdminList.Add(game);
                return;
            }
            switch (_searchType)
            {
                case ESearchType.name:                    
                    GameInStoreAdminList.Clear();
                    foreach (var game in ReserveGameInStoreAdminList)
                        if (game.game_name_game.ToLower().Contains(_filter.ToLower()))
                            GameInStoreAdminList.Add(game);
                    break;
                case ESearchType.price:
                    if (_priceFrom > _priceTo)
                    {
                        double temp = _priceFrom;
                        _priceFrom = _priceTo;
                        _priceTo = temp;
                    }
                    GameInStoreAdminList.Clear();
                    foreach (var game in ReserveGameInStoreAdminList)
                        if (_priceFrom <= game.game_price && game.game_price <= _priceTo)
                            GameInStoreAdminList.Add(game);
                    break;                
                default:
                    return;
            }
        }
        public void UpdateUnstoredGamesList()
        {
            foreach (var game in ReserveGameInStoreAdminList)
                game.UpdateUnstoredGamesList();
        }

        private RelayCommand _deleteGameFromStoreCommand;
        public RelayCommand DeleteGameFromStoreCommand =>
            _deleteGameFromStoreCommand ?? (_deleteGameFromStoreCommand = new RelayCommand(DeleteGameFromStore));
        public void DeleteGameFromStore()
        {
            try
            {
                if (SelectedGameInStoreAdmin!=null)
                {
                    DataProvider.Default.DeleteGameFromStore(SelectedGameInStoreAdmin.GameInStore);
                    ReserveGameInStoreAdminList.Remove(SelectedGameInStoreAdmin);
                    GameInStoreAdminList.Remove(SelectedGameInStoreAdmin);                    
                    _tablesAdminVM.UpdateUnstoredGamesList();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private RelayCommand _addGameInStoreCommand;
        public RelayCommand AddGameInStoreCommand =>
            _addGameInStoreCommand ?? (_addGameInStoreCommand = new RelayCommand(AddGameInStore));
        public void AddGameInStore()
        {
            try
            {
                AddGameInStoreWindow addGameInStoreWindow = new AddGameInStoreWindow(_tablesAdminVM.unstoredGamesList,this);
                addGameInStoreWindow.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void UpdateGamesInStoreList()
        {
            ReserveGameInStoreAdminList.Clear();
            var gameInStoreAdminListVM = DataProvider.Default.GetGameInStoreList().
                Select(e => new GameInStoreAdminVM(e, this));
            foreach (var p in gameInStoreAdminListVM)
            {
                if (p.game_price > maxPrice)
                    maxPrice = p.game_price;
                GameInStoreAdminList.Add(p);
                ReserveGameInStoreAdminList.Add(p);
            }
        }
    }
}