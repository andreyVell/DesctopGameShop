using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace DataBaseIndTask
{
    public class GameInStoreAdminVM:ViewModelBase
    {
        private int updateCount = 0;
        private GameInStore _gameInStore;
        public GameInStore GameInStore => _gameInStore;
        public GameInStoreAdminListVM ParentVM { set; get; }
        public GameInStoreAdminVM(GameInStore gameInStore, GameInStoreAdminListVM gameInStoreAdminListVM)
        {
            _gameInStore = gameInStore;
            ParentVM = gameInStoreAdminListVM;

            unstoredGamesList = new ObservableCollection<GameVM>();
            GameVM curGameVM = null;
            foreach (var game in ParentVM._tablesAdminVM.GameListVM.ReserveGameList)
                if (game.game_id == game_id_game)
                {
                    curGameVM = game;
                    break;
                }
            unstoredGamesList.Add(curGameVM);
            
        }
        public void UpdateUnstoredGamesList()
        {
            updateCount++;
            for (int i = unstoredGamesList.Count - 1; i >= 0; i--)
                if (unstoredGamesList[i].game_id != game_id_game)
                    unstoredGamesList.Remove(unstoredGamesList[i]);
            foreach (GameVM game in ParentVM.unstoredGamesList)
                unstoredGamesList.Add(game);
            updateCount = 0;
        }
        public int game_price
        {
            get
            {
                return Convert.ToInt32(_gameInStore.game_price);
            }
            set
            {
                DataProvider.Default.UpdateGameinStore(game_id_game, game_id_game, value);
                _gameInStore.game_price = value;
            }
        }
        public int game_id_game
        {
            get
            {
                return _gameInStore.game_id;
            }
            set
            {
                if (updateCount < 1)
                {                    
                    DataProvider.Default.UpdateGameinStore(_gameInStore.game_id, value, game_price);
                    _gameInStore.game_id = value;
                    ParentVM._tablesAdminVM.UpdateUnstoredGamesList();                         
                }
            }
        }
        public string game_name_game
        {
            get
            {
                return _gameInStore.game_name;
            }
            set
            {
                _gameInStore.game_name = value;
            }
        }
        public ObservableCollection<GameVM> unstoredGamesList { get; set; }        
    }
}