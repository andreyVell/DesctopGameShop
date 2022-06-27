using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class GameListVM:ViewModelBase
    {
        enum ESearchType
        {
            id = 0,
            name = 1,
            description = 2,
            genre = 3,
            date = 4,
            developer = 5,
            publisher = 6,
            rating = 7,
            agelimit = 8,
        }
        private string _filter;
        private DateTime _dateFilter;
        private ESearchType _searchType;
        private static object _lock = new object();

        public TablesAdminVM _tablesAdminVM;
        public GameListVM(TablesAdminVM tablesAdminVM)
        {
            _tablesAdminVM = tablesAdminVM;
            GameList = new ObservableCollection<GameVM>();            
            ReserveGameList = new ObservableCollection<GameVM>();
            var gameListVM = DataProvider.Default.GetGameList().
                Select(e => new GameVM(e, this));
            foreach (var p in gameListVM)
            { 
                GameList.Add(p);
                ReserveGameList.Add(p);
            }
            _filter = string.Empty;
            _searchType = ESearchType.id;

            VisibleSearchTypes = new ObservableCollection<string>();
            VisibleSearchTypes.Add("ID");
            VisibleSearchTypes.Add("Name");
            VisibleSearchTypes.Add("Description");
            VisibleSearchTypes.Add("Genre");
            VisibleSearchTypes.Add("Release date");
            VisibleSearchTypes.Add("Developer");
            VisibleSearchTypes.Add("Publisher");
            VisibleSearchTypes.Add("Rating");
            VisibleSearchTypes.Add("Age limit");            
        }
        public ObservableCollection<GameVM> GameList
        {
            get; set;
        }        
        public ObservableCollection<GameVM> ReserveGameList
        {
            get; set;
        }
        public ObservableCollection<string> VisibleSearchTypes { get; set; }
        public GameVM SelectedGame { get; set; }

        private RelayCommand _deleteGameCommand;
        public RelayCommand DeleteGameCommand =>
            _deleteGameCommand ?? (_deleteGameCommand = new RelayCommand(DeleteGame));
        public void DeleteGame()
        {
            try
            {
                if (SelectedGame != null)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this game?(All achievements associated with this game will also be deleted)", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes)
                    {
                        return;
                    }
                    DataProvider.Default.DeleteGame(SelectedGame.Game);
                    ReserveGameList.Remove(SelectedGame);
                    GameList.Remove(SelectedGame);
                    _tablesAdminVM.GameUpdate();
                }
            } catch (Exception ex) { MessageBox.Show(ex.Message); }
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
                UpdateFilteredGameList();
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
                UpdateFilteredGameList();
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
                if (_searchType != ESearchType.date)
                    filter = string.Empty;
            }
        }        
        public void UpdateFilteredGameList()
        {           
            
            if (_searchType != ESearchType.date && _filter == "")
            {                    
                GameList.Clear();
                foreach (var game in ReserveGameList)                        
                    GameList.Add(game);
                return;
            }
            switch (_searchType)
            {
                case ESearchType.id:
                    try
                    {
                        GameList.Clear();                            
                        foreach (var game in ReserveGameList)
                            if (game.game_id == Convert.ToInt32(_filter))                                    
                                GameList.Add(game);                            
                    }
                    catch (Exception ex) { MessageBox.Show("Incorrect ID format!"); }
                    break;
                case ESearchType.name:
                    GameList.Clear();
                    foreach (var game in ReserveGameList)
                        if (game.game_name.ToLower().Contains(_filter.ToLower()))
                            GameList.Add(game);
                    break;
                case ESearchType.description:
                    GameList.Clear();
                    foreach (var game in ReserveGameList)
                        if (game.game_description.ToLower().Contains(_filter.ToLower()))
                            GameList.Add(game);
                    break;
                case ESearchType.genre:
                    GameList.Clear();
                    foreach (var game in ReserveGameList)
                        if (game.game_genre.ToLower().Contains(_filter.ToLower()))
                            GameList.Add(game);
                    break;
                case ESearchType.date:
                    GameList.Clear();
                    foreach (var game in ReserveGameList)
                        if (game.game_releasedate.Date == _dateFilter.Date)
                            GameList.Add(game);
                    break;
                case ESearchType.developer:
                    GameList.Clear();
                    foreach (var game in ReserveGameList)
                        if (game.game_developer.ToLower().Contains(_filter.ToLower()))
                            GameList.Add(game);
                    break;
                case ESearchType.publisher:
                    GameList.Clear();
                    foreach (var game in ReserveGameList)
                        if (game.game_publisher.ToLower().Contains(_filter.ToLower()))
                            GameList.Add(game);
                    break;
                case ESearchType.rating:
                    try
                    {
                        GameList.Clear();
                        foreach (var game in ReserveGameList)
                            if (game.game_rating == Convert.ToByte(_filter))
                                GameList.Add(game);
                    }
                    catch (Exception ex) { MessageBox.Show("Incorrect Rating format!"); }
                    break;
                case ESearchType.agelimit:
                    try
                    {
                        GameList.Clear();
                        foreach (var game in ReserveGameList)
                            if (game.game_agelimit == Convert.ToByte(_filter))
                                GameList.Add(game);
                    }
                    catch (Exception ex) { MessageBox.Show("Incorrect Age limit format!"); }
                    break;
                default:
                    return;
            }
        }        
        
        private RelayCommand _addGameCommand;
        public RelayCommand AddGameCommand =>
            _addGameCommand ?? (_addGameCommand = new RelayCommand(AddGame));
        public void AddGame()
        {
            AddGameWindow addGameWindow = new AddGameWindow(_tablesAdminVM);
            addGameWindow.Show();
        }
    }
}