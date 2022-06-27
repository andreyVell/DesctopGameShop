namespace DataBaseIndTask.CustomQueryBuilder
{
    public class AchievementTable
    {
        private static bool _achievement_id;
        private static bool _achievement_name;
        private static bool _achievement_description;
        private static bool _achievement_game_id;
        private static bool _achievement_image;
        private static CustomQuaryBuilderVM _customQuaryBuilderVM;
        public AchievementTable(CustomQuaryBuilderVM customQuaryBuilderVM)
        {
            _customQuaryBuilderVM = customQuaryBuilderVM;
        }
        public static bool Achievement_id
        {
            get
            {
                return _achievement_id;
            }
            set
            {
                _achievement_id = value;
                if (_achievement_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Achievement.achievement_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Achievement.achievement_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Achievement_name
        {
            get
            {
                return _achievement_name;
            }
            set
            {
                _achievement_name = value;
                if (_achievement_name)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Achievement.achievement_name");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Achievement.achievement_name");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Achievement_description
        {
            get
            {
                return _achievement_description;
            }
            set
            {
                _achievement_description = value;
                if (_achievement_description)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Achievement.achievement_description");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Achievement.achievement_description");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Achievement_game_id
        {
            get
            {
                return _achievement_game_id;
            }
            set
            {
                _achievement_game_id = value;
                if (_achievement_game_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Achievement.achievement_game_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Achievement.achievement_game_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Achievement_image
        {
            get
            {
                return _achievement_image;
            }
            set
            {
                _achievement_image = value;
                if (_achievement_image)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Achievement.achievement_image");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Achievement.achievement_image");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
    }
}