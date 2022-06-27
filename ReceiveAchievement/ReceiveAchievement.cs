using System;

namespace DataBaseIndTask
{
    public class ReceiveAchievement
    {
        public int receiveAchievement_account_id { get; set; }
        public string receiveAchievement_account_login { get; set; }
        public int receiveAchievement_id { get; set; }
        public string receiveAchievement_name { get; set; }
        public string receiveAchievement_description { get; set; }
        public int receiveAchievement_game_id { get; set; }
        public string receiveAchievement_game_name { get; set; }
        public string receiveAchievement_image { get; set; }
        public DateTime receiveAchievement_date { get; set; }
    }
}