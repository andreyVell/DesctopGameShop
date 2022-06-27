using GalaSoft.MvvmLight;

namespace DataBaseIndTask
{
    public class ReceiveAchievementVM : ViewModelBase
    {
        private ReceiveAchievement _receiveAchievement;
        public ReceiveAchievement ReceiveAchievement => _receiveAchievement;

        public ReceiveAchievementListVM ParentVM { set; get; }
        public ReceiveAchievementVM(ReceiveAchievement receiveAchievement, ReceiveAchievementListVM receiveAchievementList)
        {
            _receiveAchievement = receiveAchievement;
            ParentVM = receiveAchievementList;
        }
        public int receiveAchievement_account_id
        {
            get
            {
                return _receiveAchievement.receiveAchievement_account_id;
            }
        }
        public string receiveAchievement_account_login
        {
            get
            {
                return _receiveAchievement.receiveAchievement_account_login;
            }
        }
        public int receiveAchievement_id
        {
            get
            {
                return _receiveAchievement.receiveAchievement_id;
            }
        }
        public string receiveAchievement_name
        {
            get
            {
                return _receiveAchievement.receiveAchievement_name;
            }
        }
        public string receiveAchievement_description
        {
            get
            {
                return _receiveAchievement.receiveAchievement_description;
            }
        }
        public int receiveAchievement_game_id
        {
            get
            {
                return _receiveAchievement.receiveAchievement_game_id;
            }
        }
        public string receiveAchievement_game_name
        {
            get
            {
                return _receiveAchievement.receiveAchievement_game_name;
            }
        }
        public string receiveAchievement_image
        {
            get
            {
                return _receiveAchievement.receiveAchievement_image;
            }
        }
        public string receiveAchievement_date
        {
            get
            {
                return _receiveAchievement.receiveAchievement_date.ToString("dd.MM.yyyy HH:mm:ss");
            }
        }
    }    
}