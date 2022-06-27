using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Windows;

namespace DataBaseIndTask
{
    public class GameVM:ViewModelBase
    {
        private Game _game;
        public Game Game => _game;
        public GameListVM ParentVM { set; get; }
        public GameVM(Game game, GameListVM gameList)
        {
            _game = game;
            ParentVM = gameList;
        }        
        public int game_id
        {
            get
            {
                return _game.game_id;
            }
        }
        public string game_name
        {
            get
            {
                return _game.game_name;
            }
            set
            {
                _game.game_name = value;
                DataProvider.Default.UpdateGame(_game);
                ParentVM._tablesAdminVM?.GameUpdate();
            }
        }
        public string game_description
        {
            get
            {
                return _game.game_description;
            }
            set
            {
                _game.game_description = value;
                DataProvider.Default.UpdateGame(_game);
                ParentVM._tablesAdminVM?.GameUpdate();
            }
        }
        public string game_genre
        {
            get
            {
                return _game.game_genre;
            }
            set
            {
                _game.game_genre = value;
                DataProvider.Default.UpdateGame(_game);
                ParentVM._tablesAdminVM?.GameUpdate();
            }
        }
        public DateTime game_releasedate
        {
            get
            {
                return _game.game_releasedate;
            }
            set
            {
                _game.game_releasedate = value;
                DataProvider.Default.UpdateGame(_game);
                ParentVM._tablesAdminVM?.GameUpdate();
            }
        }
        public string game_developer
        {
            get
            {
                return _game.game_developer;
            }
            set
            {
                _game.game_developer = value;
                DataProvider.Default.UpdateGame(_game);
                ParentVM._tablesAdminVM?.GameUpdate();
            }
        }
        public string game_publisher
        {
            get
            {
                return _game.game_publisher;
            }
            set
            {
                _game.game_publisher = value;
                DataProvider.Default.UpdateGame(_game);
                ParentVM._tablesAdminVM?.GameUpdate();
            }
        }
        public byte game_rating
        {
            get
            {
                return _game.game_rating;
            }
            set
            {
                if (value >= 0 && value <= 10)
                {
                    _game.game_rating = value;
                    DataProvider.Default.UpdateGame(_game);
                    ParentVM._tablesAdminVM?.GameUpdate();
                }
                else MessageBox.Show("Rating must be between 0 and 10");
            }
        }
        public byte game_agelimit
        {
            get
            {
                return _game.game_agelimit;
            }
            set
            {
                if (value >= 0 && value <= 21)
                {
                    _game.game_agelimit = value;
                    DataProvider.Default.UpdateGame(_game);
                    ParentVM._tablesAdminVM?.GameUpdate();
                }
                else MessageBox.Show("Age limit must be between 0 and 21");
            }
        }
        public string game_image
        {
            get
            {
                return _game.game_image;
            }
            set
            {
                _game.game_image = value;
                DataProvider.Default.UpdateGame(_game);
                RaisePropertyChanged(nameof(game_image));
                ParentVM._tablesAdminVM?.GameUpdate();
            }
        }
        public string game_storeimage
        {
            get
            {
                return _game.game_storeimage;
            }
            set
            {
                _game.game_storeimage = value;
                DataProvider.Default.UpdateGame(_game);
                RaisePropertyChanged(nameof(game_storeimage));
                ParentVM._tablesAdminVM?.GameUpdate();
            }
        }


        private RelayCommand _changeImage;
        public RelayCommand ChangeImageCommand =>
            _changeImage ?? (_changeImage = new RelayCommand(ChangeImage));
        public void ChangeImage()
        {
            MessageBox.Show("Recommended image width/height ratio is 16/9");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.PNG)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog.ShowDialog() == true)
                game_image = openFileDialog.FileName;
        }


        private RelayCommand _changeStoreImage;
        public RelayCommand ChangeStoreImageCommand =>
            _changeStoreImage ?? (_changeStoreImage = new RelayCommand(ChangeStoreImage));
        public void ChangeStoreImage()
        {
            MessageBox.Show("Recommended image width/height ratio is 16/9");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.PNG)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog.ShowDialog() == true)
                game_storeimage = openFileDialog.FileName;
        }
    }
}