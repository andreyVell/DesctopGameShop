using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace DataBaseIndTask
{
    public class AddNewAchievementVM:ViewModelBase
    {
        private TablesAdminVM _tablesAdminVM;
        private AddAchievementWindow _parentWindow;
        public AddNewAchievementVM(ObservableCollection<GameVM> gameList, TablesAdminVM tablesAdminVM, AddAchievementWindow parentWindow)        
        {            
            GameList = gameList;
            _tablesAdminVM = tablesAdminVM;
            _parentWindow = parentWindow;
        }
        public string achievement_name { get; set; }
        public string achievement_description { get; set; }
        public int achievement_game_id { get; set; }
        public ObservableCollection<GameVM> GameList { get; set; }


        private RelayCommand _addAchievementCommand;
        public RelayCommand AddAchievementCommand =>
            _addAchievementCommand ?? (_addAchievementCommand = new RelayCommand(AddAchievement));
        public void AddAchievement()
        {   
            AchievementAdmin achievementAdmin = new AchievementAdmin()
            {
                achievement_name = achievement_name,
                achievement_description = achievement_description,
                achievement_game_id = achievement_game_id,
            };
            DataProvider.Default.AddAchievement(achievementAdmin);            
            _tablesAdminVM.AchievementListAdminVM.ReserveAchievementListAdmin.Add(new AchievementAdminVM(DataProvider.Default.GetLastAchievement(), _tablesAdminVM.AchievementListAdminVM));
            _tablesAdminVM.AchievementListAdminVM.UpdateFilteredAchievementListAdmin();
            _parentWindow.Close();
        }
    }
}