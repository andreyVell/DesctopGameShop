using GalaSoft.MvvmLight;

namespace DataBaseIndTask
{
    public class TablesPlayerVM : ViewModelBase
    {
        private mainWindowPlayer _parentWindow;
        public AccountListVM AccountListVM { get; set; }
        public ReportListVM ReportListVM { get; set; }
        public GameInLibraryListVM GameInLibraryListVM { get; set; }
        public GameInStoreListVM GameInStoreListVM { get; set; }
        public AchievementListVM AchievementListVM { get; set; }
        public ReceiveAchievementListVM ReceiveAchievementListVM { get; set; }
        public TablesPlayerVM(mainWindowPlayer window)
        {
            _parentWindow = window;
            AccountListVM = new AccountListVM(null, _parentWindow);
            ReportListVM = new ReportListVM(this);
            AchievementListVM = new AchievementListVM();
            ReceiveAchievementListVM = new ReceiveAchievementListVM("player");
            GameInLibraryListVM = new GameInLibraryListVM(AchievementListVM, ReceiveAchievementListVM);
            GameInStoreListVM = new GameInStoreListVM(AccountListVM, GameInLibraryListVM,_parentWindow);            
        }
    }
}