using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace DataBaseIndTask
{
    public class AchievementAdminVM:ViewModelBase
    {
        private AchievementAdmin _achievement;
        public AchievementAdmin Achievement => _achievement;
        public AchievementListAdminVM ParentVM { set; get; }
        public AchievementAdminVM(AchievementAdmin achievement, AchievementListAdminVM achievementListAdmin)
        {
            _achievement = achievement;
            ParentVM = achievementListAdmin;
            GameList = ParentVM.GameList;
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
            set
            {
                _achievement.achievement_name = value;
                DataProvider.Default.UpdateAchievement(_achievement);
                ParentVM._tablesAdminVM.ReceiveAchievementListVM.UpdateReceiveAchievementList();
            }
        }
        public int achievement_game_id
        {
            get
            {
                return _achievement.achievement_game_id;
            }
            set
            {
                _achievement.achievement_game_id = value;
                DataProvider.Default.UpdateAchievement(_achievement);
                ParentVM._tablesAdminVM.ReceiveAchievementListVM.UpdateReceiveAchievementList();
            }
        }
        public string achievement_description
        {
            get
            {
                return _achievement.achievement_description;
            }
            set
            {
                _achievement.achievement_description = value;
                DataProvider.Default.UpdateAchievement(_achievement);
                ParentVM._tablesAdminVM.ReceiveAchievementListVM.UpdateReceiveAchievementList();
            }
        }
        public string achievement_image
        {
            get
            {
                return _achievement.achievement_image;
            }
            set
            {
                _achievement.achievement_image = value;
                DataProvider.Default.UpdateAchievement(_achievement);
                RaisePropertyChanged(nameof(achievement_image));
            }
        }        
        public ObservableCollection<GameVM> GameList { get; set; }

        private RelayCommand _changeImage;
        public RelayCommand ChangeImageCommand =>
            _changeImage ?? (_changeImage = new RelayCommand(ChangeImage));
        public void ChangeImage()
        {
            MessageBox.Show("Recommended image width/height ratio is 1/1");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.PNG)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog.ShowDialog() == true)
                achievement_image = openFileDialog.FileName;
        }
    }
}