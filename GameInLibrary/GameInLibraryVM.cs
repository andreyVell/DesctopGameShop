using GalaSoft.MvvmLight;
using System;

namespace DataBaseIndTask
{
    public class GameInLibraryVM:ViewModelBase
    {
        private GameInLibrary _gameInLibrary;
        public GameInLibrary GameInLibrary => _gameInLibrary;

        public GameInLibraryListVM ParentVM { set; get; }
        public GameInLibraryVM(GameInLibrary gameInLibrary, GameInLibraryListVM gameInLibraryList)
        {
            _gameInLibrary = gameInLibrary;
            ParentVM = gameInLibraryList;
        }
        public int purchase_id 
        { 
            get
            {
                return _gameInLibrary.purchase_id;
            }
        }
        public int purchaser_id
        {
            get
            {
                return _gameInLibrary.purchaser_id;
            }
        }
        public int purchase_game_id
        {
            get
            {
                return _gameInLibrary.purchase_game_id;
            }
        }
        public double purchase_price
        {
            get
            {
                return Convert.ToDouble(_gameInLibrary.purchase_price);
            }
        }
        public string purchase_date
        {
            get
            {
                return _gameInLibrary.purchase_date.ToString("dd.MM.yyyy HH:mm:ss");
            }
        }
        public string game_name
        {
            get
            {
                return _gameInLibrary.game_name;
            }            
        }
        public string game_description
        {
            get
            {
                return _gameInLibrary.game_description;
            }
        }
        public string game_genre
        {
            get
            {
                return _gameInLibrary.game_genre;
            }
        }
        public DateTime game_releasedate
        {
            get
            {
                return _gameInLibrary.game_releasedate;
            }
        }
        public string game_developer
        {
            get
            {
                return _gameInLibrary.game_developer;
            }
        }
        public string game_publisher
        {
            get
            {
                return _gameInLibrary.game_publisher;
            }
        }
        public byte game_rating
        {
            get
            {
                return _gameInLibrary.game_rating;
            }
        }
        public byte game_agelimit
        {
            get
            {
                return _gameInLibrary.game_agelimit;
            }
        }
        public string game_image
        {
            get
            {
                return _gameInLibrary.game_image;
            }            
        }
    }
}