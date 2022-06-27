namespace DataBaseIndTask.CustomQueryBuilder
{
    public class GameTable
    {
        private static bool _game_id;
        private static bool _game_name;
        private static bool _game_description;
        private static bool _game_genre;
        private static bool _game_release_date;
        private static bool _game_developer;
        private static bool _game_publisher;
        private static bool _game_reting;
        private static bool _game_agelimit;
        private static bool _game_image;
        private static bool _game_storeimage;
        private static CustomQuaryBuilderVM _customQuaryBuilderVM;
        public GameTable(CustomQuaryBuilderVM customQuaryBuilderVM)
        {
            _customQuaryBuilderVM = customQuaryBuilderVM;
        }
        public static bool Game_id
        {
            get
            {
                return _game_id;
            }
            set
            {
                _game_id = value;
                if (_game_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_name
        {
            get
            {
                return _game_name;
            }
            set
            {
                _game_name = value;
                if (_game_name)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_name");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_name");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_description
        {
            get
            {
                return _game_description;
            }
            set
            {
                _game_description = value;
                if (_game_description)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_description");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_description");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_genre
        {
            get
            {
                return _game_genre;
            }
            set
            {
                _game_genre = value;
                if (_game_genre)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_genre");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_genre");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_release_date
        {
            get
            {
                return _game_release_date;
            }
            set
            {
                _game_release_date = value;
                if (_game_release_date)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_release_date");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_release_date");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_developer
        {
            get
            {
                return _game_developer;
            }
            set
            {
                _game_developer = value;
                if (_game_developer)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_developer");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_developer");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_publisher
        {
            get
            {
                return _game_publisher;
            }
            set
            {
                _game_publisher = value;
                if (_game_publisher)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_publisher");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_publisher");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_rating
        {
            get
            {
                return _game_reting;
            }
            set
            {
                _game_reting = value;
                if (_game_reting)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_rating");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_rating");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_agelimit
        {
            get
            {
                return _game_agelimit;
            }
            set
            {
                _game_agelimit = value;
                if (_game_agelimit)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_agelimit");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_agelimit");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_image
        {
            get
            {
                return _game_image;
            }
            set
            {
                _game_image = value;
                if (_game_image)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_image");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_image");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_storeimage
        {
            get
            {
                return _game_storeimage;
            }
            set
            {
                _game_storeimage = value;
                if (_game_storeimage)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Game.game_storeimage");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Game.game_storeimage");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
    }
}