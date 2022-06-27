using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class AchievementListAdminVM:ViewModelBase
    {
        public TablesAdminVM _tablesAdminVM;
        public AchievementListAdminVM(TablesAdminVM tablesAdminVM, GameListVM gameListVM)
        {
            _tablesAdminVM = tablesAdminVM;
            GameList = gameListVM.ReserveGameList;
            AchievementListAdmin = new ObservableCollection<AchievementAdminVM>();
            //search stuff
            ReserveAchievementListAdmin = new ObservableCollection<AchievementAdminVM>();
            VisibleSearchTypes = new ObservableCollection<string>();
            //search stuff
            UpdateAchievementListAdmin();
            //search stuff
            VisibleSearchTypes.Add("ID");
            VisibleSearchTypes.Add("Name");
            VisibleSearchTypes.Add("Game name");
            VisibleSearchTypes.Add("Description");
            _filter = string.Empty;
            _searchType = ESearchType.id;
            //search stuff
        }
        public ObservableCollection<AchievementAdminVM> AchievementListAdmin { get; set; }
        public AchievementAdminVM SelectedAchievementAdmin { get; set; }
        public ObservableCollection<GameVM> GameList { get; set; }
        public void UpdateAchievementListAdmin()
        {
            ReserveAchievementListAdmin.Clear();
            AchievementListAdmin.Clear();
            var achievementListVM = DataProvider.Default.GetAchievementListAdmin().
                Select(e => new AchievementAdminVM(e, this));
            foreach (var p in achievementListVM)
            {
                ReserveAchievementListAdmin.Add(p);
                AchievementListAdmin.Add(p);
            }
        }

        private RelayCommand _deleteAchievementCommand;
        public RelayCommand DeleteAchievementCommand =>
            _deleteAchievementCommand ?? (_deleteAchievementCommand = new RelayCommand(DeleteAchievement));
        public void DeleteAchievement()
        {
            try
            {
                if (SelectedAchievementAdmin != null)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this achievement?(All information about receiving this achievement will also be deleted)", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes)
                    {
                        return;
                    }
                    DataProvider.Default.DeleteAchievement(SelectedAchievementAdmin.Achievement);
                    ReserveAchievementListAdmin.Remove(SelectedAchievementAdmin);
                    AchievementListAdmin.Remove(SelectedAchievementAdmin);
                    //соответствующие обновления таблиц после удаления ачивки
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        private RelayCommand _addAchievementCommand;
        public RelayCommand AddAchievementCommand =>
            _addAchievementCommand ?? (_addAchievementCommand = new RelayCommand(AddAchievement));
        public void AddAchievement()
        {
            AddAchievementWindow addAchievementWindow = new AddAchievementWindow(GameList, _tablesAdminVM);
            addAchievementWindow.Show();
        }



        //search stuff
        enum ESearchType
        {
            id = 0,
            name = 1,
            game_name = 2,
            description = 3,
        }
        private string _filter;
        private ESearchType _searchType;
        public ObservableCollection<string> VisibleSearchTypes { get; set; }
        public ObservableCollection<AchievementAdminVM> ReserveAchievementListAdmin { get; set; }
        public string filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (value == null)
                    _filter = string.Empty;
                else
                    _filter = value;
                UpdateFilteredAchievementListAdmin();
            }
        }
        public int searchType
        {
            get
            {
                return ((int)_searchType);
            }
            set
            {
                _searchType = (ESearchType)value;
                filter = string.Empty;
            }
        }
        public void UpdateFilteredAchievementListAdmin()
        {

            if (_filter == "")
            {
                AchievementListAdmin.Clear();
                foreach (var game in ReserveAchievementListAdmin)
                    AchievementListAdmin.Add(game);
                return;
            }
            switch (_searchType)
            {
                case ESearchType.id:
                    try
                    {
                        AchievementListAdmin.Clear();
                        foreach (var game in ReserveAchievementListAdmin)
                            if (game.achievement_id == Convert.ToInt32(_filter))
                                AchievementListAdmin.Add(game);
                    }
                    catch { MessageBox.Show("Incorrect ID format!"); }
                    break;                
                case ESearchType.name:
                    AchievementListAdmin.Clear();
                    foreach (var game in ReserveAchievementListAdmin)
                        if (game.achievement_name.ToLower().Contains(_filter.ToLower()))
                            AchievementListAdmin.Add(game);
                    break;
                case ESearchType.game_name:
                    AchievementListAdmin.Clear();
                    foreach (var game in ReserveAchievementListAdmin)
                        if (GetGameNameFromId(game.achievement_game_id).ToLower().Contains(_filter.ToLower()))
                            AchievementListAdmin.Add(game);
                    break;
                case ESearchType.description:
                    AchievementListAdmin.Clear();
                    foreach (var game in ReserveAchievementListAdmin)
                        if (game.achievement_description.ToLower().Contains(_filter.ToLower()))
                            AchievementListAdmin.Add(game);
                    break;
                default:
                    return;
            }
        }
        private string GetGameNameFromId(int gameID)
        {
            foreach (var game in _tablesAdminVM.GameListVM.GameList)
                if (game.game_id == gameID)
                    return game.game_name;
            return "";
        }
        //search stuff
    }
}