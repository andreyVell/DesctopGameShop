namespace DataBaseIndTask.CustomQueryBuilder
{
    public class ReceiveAchievementsTable
    {
        private static bool _achievement_id;
        private static bool _account_id;
        private static bool _receive_achievement_date;
        private static CustomQuaryBuilderVM _customQuaryBuilderVM;
        public ReceiveAchievementsTable(CustomQuaryBuilderVM customQuaryBuilderVM)
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
                    _customQuaryBuilderVM.UsedFieldsList.Add("Receive_achievements.achievement_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Receive_achievements.achievement_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_id
        {
            get
            {
                return _account_id;
            }
            set
            {
                _account_id = value;
                if (_account_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Receive_achievements.account_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Receive_achievements.account_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Receive_achievement_date
        {
            get
            {
                return _receive_achievement_date;
            }
            set
            {
                _receive_achievement_date = value;
                if (_receive_achievement_date)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Receive_achievements.receive_achievement_date");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Receive_achievements.receive_achievement_date");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
    }
}