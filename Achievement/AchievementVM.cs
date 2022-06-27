using GalaSoft.MvvmLight;

namespace DataBaseIndTask
{
    public class AchievementVM : ViewModelBase
    {
        private Achievement _achievement;
        public Achievement Achievement => _achievement;
        public AchievementListVM ParentVM { set; get; }
        public AchievementVM(Achievement achievement, AchievementListVM achievementList)
        {
            _achievement = achievement;
            ParentVM = achievementList;
        }
        public int achievement_id
        {
            get
            {
                return _achievement.achievement_id;
            }
        }
        public string achievement_name
        {
            get
            {
                return _achievement.achievement_name;
            }
        }
        public int achievement_game_id
        {
            get
            {
                return _achievement.achievement_game_id;
            }
        }
        public string achievement_game_name
        {
            get
            {
                return _achievement.achievement_game_name;
            }
        }
        public string achievement_image
        {
            get
            {
                return _achievement.achievement_image;
            }
        }
    }
}