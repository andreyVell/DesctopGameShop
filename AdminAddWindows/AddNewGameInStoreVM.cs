using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace DataBaseIndTask
{
    class AddNewGameInStoreVM:ViewModelBase
    {
        AddGameInStoreWindow _ParentWindow;
        public ObservableCollection<GameVM> UnstoredGamesList { get; set; }
        public AddNewGameInStoreVM(ObservableCollection<GameVM> _unsoredGameList, AddGameInStoreWindow addGameInStoreWindow)
        {
            _ParentWindow = addGameInStoreWindow;
            UnstoredGamesList = _unsoredGameList;
        }
        public int game_id { get; set; }

        private RelayCommand _addGameInStoreCommand;
        public RelayCommand AddGameInStoreCommand =>
            _addGameInStoreCommand ?? (_addGameInStoreCommand = new RelayCommand(AddGameInStore));
        public void AddGameInStore()
        {
            int game_price_over;            
            if (!Int32.TryParse(_ParentWindow.PriceTextBox.Text, out game_price_over))
            {
                MessageBox.Show("Enter a correct price (integer)");
                return;
            }
            DataProvider.Default.AddGameInStore(game_id, game_price_over);
            GameInStore newGameInStore=null;
            foreach (var gameinstore in _ParentWindow.parentClassStore._tablesAdminVM.GameListVM.ReserveGameList)
                if (gameinstore.game_id == game_id)
                {
                    newGameInStore = new GameInStore()
                    {
                        game_id = gameinstore.game_id,
                        game_agelimit = gameinstore.game_agelimit,
                        game_description = gameinstore.game_description,
                        game_developer = gameinstore.game_developer,
                        game_genre = gameinstore.game_genre,
                        game_image = gameinstore.game_image,
                        game_name = gameinstore.game_name,
                        game_price = game_price_over,
                        game_publisher = gameinstore.game_publisher,
                        game_rating = gameinstore.game_rating,
                        game_releasedate = gameinstore.game_releasedate,
                        game_storeimage = gameinstore.game_storeimage,
                    };
                    break;
                }            
            _ParentWindow.parentClassStore.ReserveGameInStoreAdminList.Add(new GameInStoreAdminVM(newGameInStore, _ParentWindow.parentClassStore));
            _ParentWindow.parentClassStore.UpdateFilteredGameInStoreAdminList();
            _ParentWindow.parentClassStore._tablesAdminVM.UpdateUnstoredGamesList();
            _ParentWindow.Close();
        }
    }
}