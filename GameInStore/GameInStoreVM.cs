using GalaSoft.MvvmLight;
using System;

namespace DataBaseIndTask
{
    public class GameInStoreVM:ViewModelBase
    {
        private GameInStore _gameInStore;
        public GameInStore GameInStore => _gameInStore;
        public GameInStoreListVM ParentVM { set; get; }
        public GameInStoreVM(GameInStore gameInStore, GameInStoreListVM gameInStoreList)
        {
            _gameInStore = gameInStore;
            ParentVM = gameInStoreList;
        }        
        public int game_price
        {
            get
            {
                return Convert.ToInt32(_gameInStore.game_price);
            }
        }
        public int game_id
        {
            get
            {
                return _gameInStore.game_id;
            }
        }
        public string game_name
        {
            get
            {
                return _gameInStore.game_name;
            }
        }
        public string game_description
        {
            get
            {
                return _gameInStore.game_description;
            }
        }
        public string game_genre
        {
            get
            {
                return _gameInStore.game_genre;
            }
        }
        public DateTime game_releasedate
        {
            get
            {
                return _gameInStore.game_releasedate;
            }
        }
        public string game_developer
        {
            get
            {
                return _gameInStore.game_developer;
            }
        }
        public string game_publisher
        {
            get
            {
                return _gameInStore.game_publisher;
            }
        }
        public byte game_rating
        {
            get
            {
                return _gameInStore.game_rating;
            }
        }
        public byte game_agelimit
        {
            get
            {
                return _gameInStore.game_agelimit;
            }
        }
        public string game_image
        {
            get
            {
                return _gameInStore.game_image;
            }
        }
        public string game_storeimage
        {
            get
            {
                return _gameInStore.game_storeimage;
            }
        }
    }
}