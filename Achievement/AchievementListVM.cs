using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataBaseIndTask
{
    public class AchievementListVM : ViewModelBase
    {
        enum SearchType
        {
            achievement_name = 0,
            game_name = 1,
        }
        private string _searchFilter;
        private SearchType _searchType;

        public AchievementListVM()
        {
            AchievementList = new ObservableCollection<AchievementVM>();
            FilteredAchievementList = new ObservableCollection<AchievementVM>();
            ReserveAchievementList = new ObservableCollection<AchievementVM>();
            var achievementListVM = DataProvider.Default.GetAchievementList().
                Select(e => new AchievementVM(e, this));
            foreach (var p in achievementListVM)
            { 
                AchievementList.Add(p);
                ReserveAchievementList.Add(p);
            }
            _searchFilter = string.Empty;
            _searchType = SearchType.achievement_name;
            searchTypes = new ObservableCollection<string>();
            searchTypes.Add("Achievement name");
            searchTypes.Add("Game name");
        }
        public ObservableCollection<AchievementVM> AchievementList
        {
            get; set;
        }
        public ObservableCollection<AchievementVM> FilteredAchievementList
        {
            get; set;
        }
        public ObservableCollection<AchievementVM> ReserveAchievementList
        {
            get; set;
        }
        public ObservableCollection<string> searchTypes { get; set; }
        public AchievementVM SelectedAchievement { get; set; }
        
        public string filter
        {
            get
            {
                return _searchFilter;
            }
            set
            {
                if (value == null)
                    _searchFilter = string.Empty;
                else
                    _searchFilter = value;

                if (_searchFilter == "")
                {
                    AchievementList.Clear();
                    foreach (var game in ReserveAchievementList)
                        AchievementList.Add(game);
                }
                else
                {
                    AchievementList.Clear();
                    UpdateFilteredAchievementList();
                    foreach (var game in FilteredAchievementList)
                        AchievementList.Add(game);
                }
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
                _searchType = (SearchType)value;
                if (_searchFilter == "")
                {
                    AchievementList.Clear();
                    foreach (var game in ReserveAchievementList)
                        AchievementList.Add(game);
                }
                else
                {
                    AchievementList.Clear();
                    UpdateFilteredAchievementList();
                    foreach (var game in FilteredAchievementList)
                        AchievementList.Add(game);
                }
            }
        }
        public void UpdateFilteredAchievementList()
        {
            switch (_searchType)
            {
                case SearchType.achievement_name:
                    {
                        FilteredAchievementList.Clear();
                        foreach (var achi in ReserveAchievementList)
                            if (achi.achievement_name.ToLower().Contains(filter.ToLower()))
                                FilteredAchievementList.Add(achi);
                    }
                    break;
                case SearchType.game_name:
                    {
                        FilteredAchievementList.Clear();
                        foreach (var achi in ReserveAchievementList)
                            if (achi.achievement_game_name.ToLower().Contains(filter.ToLower()))
                                FilteredAchievementList.Add(achi);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}