using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class ReceiveAchievementListVM : ViewModelBase
    {
        enum ESearchType
        {
            achievement_name = 0,
            game_name = 1,
            user_id=2,
            user_login=3,
            achievement_id=4,
            game_id=5,
            date=6,
        }
        private string _filter;
        private ESearchType _searchType;
        private DateTime _dateFilter;
        public ReceiveAchievementListVM(string mode)
        {
            ReceiveAchievementList = new ObservableCollection<ReceiveAchievementVM>();            
            ReserveReceiveAchievementList = new ObservableCollection<ReceiveAchievementVM>();
            UpdateReceiveAchievementList();
            _filter = string.Empty;
            _searchType = ESearchType.achievement_name;
            searchTypes = new ObservableCollection<string>();            
            searchTypes.Add("Achievement name");
            searchTypes.Add("Game name");
            if (mode =="admin")
            {
                searchTypes.Add("User ID");
                searchTypes.Add("User login");
                searchTypes.Add("Achievement ID");
                searchTypes.Add("Game ID");
                searchTypes.Add("Receive date");
            }
        }
        public ObservableCollection<ReceiveAchievementVM> ReceiveAchievementList
        {
            get; set;
        }
        public ObservableCollection<ReceiveAchievementVM> ReserveReceiveAchievementList
        {
            get; set;
        }
        public ObservableCollection<string> searchTypes { get; set; }
        public ReceiveAchievementVM SelectedReceiveAchievement { get; set; }
        public void UpdateReceiveAchievementList()
        {
            ReceiveAchievementList.Clear();
            var receiveAchievementListVM = DataProvider.Default.GetReceiveAchievementList().
                Select(e => new ReceiveAchievementVM(e, this));
            foreach (var p in receiveAchievementListVM)
            {
                ReceiveAchievementList.Add(p);
                ReserveReceiveAchievementList.Add(p);
            }
        }
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
                UpdateFilteredAchievementList();
            }     
        }
        public DateTime dateFilter
        {
            get
            {
                return _dateFilter;
            }
            set
            {

                _dateFilter = value;
                UpdateFilteredAchievementList();
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
        public void UpdateFilteredAchievementList()
        {
            if (_searchType != ESearchType.date && _filter == "")
            {
                ReceiveAchievementList.Clear();
                foreach (var game in ReserveReceiveAchievementList)
                    ReceiveAchievementList.Add(game);
                return;
            }
            switch (_searchType)
            {
                case ESearchType.achievement_name:
                    {
                        ReceiveAchievementList.Clear();
                        foreach (var achi in ReserveReceiveAchievementList)
                            if (achi.receiveAchievement_name.ToLower().Contains(filter.ToLower()))
                                ReceiveAchievementList.Add(achi);
                    }
                    break;
                case ESearchType.game_name:
                    {
                        ReceiveAchievementList.Clear();
                        foreach (var achi in ReserveReceiveAchievementList)
                            if (achi.receiveAchievement_game_name.ToLower().Contains(filter.ToLower()))
                                ReceiveAchievementList.Add(achi);
                    }
                    break;
                case ESearchType.user_id:
                    try
                    {
                        ReceiveAchievementList.Clear();
                        foreach (var game in ReserveReceiveAchievementList)
                            if (game.receiveAchievement_account_id == Convert.ToInt32(_filter))
                                ReceiveAchievementList.Add(game);
                    }
                    catch { MessageBox.Show("Incorrect ID format!"); }
                    break;
                case ESearchType.achievement_id:
                    try
                    {
                        ReceiveAchievementList.Clear();
                        foreach (var game in ReserveReceiveAchievementList)
                            if (game.receiveAchievement_id == Convert.ToInt32(_filter))
                                ReceiveAchievementList.Add(game);
                    }
                    catch { MessageBox.Show("Incorrect ID format!"); }
                    break;
                case ESearchType.game_id:
                    try
                    {
                        ReceiveAchievementList.Clear();
                        foreach (var game in ReserveReceiveAchievementList)
                            if (game.receiveAchievement_game_id == Convert.ToInt32(_filter))
                                ReceiveAchievementList.Add(game);
                    }
                    catch { MessageBox.Show("Incorrect ID format!"); }
                    break;
                case ESearchType.user_login:
                    {
                        ReceiveAchievementList.Clear();
                        foreach (var achi in ReserveReceiveAchievementList)
                            if (achi.receiveAchievement_account_login.ToLower().Contains(filter.ToLower()))
                                ReceiveAchievementList.Add(achi);
                    }
                    break;
                case ESearchType.date:
                    ReceiveAchievementList.Clear();
                    foreach (var game in ReserveReceiveAchievementList)
                        if (dateFilter.Date == game.ReceiveAchievement.receiveAchievement_date.Date)
                            ReceiveAchievementList.Add(game);
                    break;
                default:
                    break;
            }
        }
    }
}