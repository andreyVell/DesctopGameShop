using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace DataBaseIndTask
{
    public class TablesAdminVM : ViewModelBase
    {        
        public mainWindowAdmin ParentWindow { get; set; }
        public AccountListVM AccountListVM { get; set; }
        public ReportListVM ReportListVM { get; set; }
        public GameListVM GameListVM { get; set; }
        public GameInStoreAdminListVM GameInStoreAdminListVM { get; set; }
        public PurchaseListVM PurchaseListVM { get; set; }
        public AchievementListAdminVM AchievementListAdminVM { get; set; }
        public ReceiveAchievementListVM ReceiveAchievementListVM { get; set; }
        public DataExport DataExport { get; set; }
        public CustomQuaryBuilderVM CustomQuaryBuilderVM { get; set; }
        public TablesAdminVM(mainWindowAdmin mainWindowAdmin)
        {
            ParentWindow = mainWindowAdmin;
            unstoredGamesList = new ObservableCollection<GameVM>();            
            AccountListVM = new AccountListVM(this, ParentWindow);            
            ReportListVM = new ReportListVM(null);            
            GameListVM = new GameListVM(this);            
            GameInStoreAdminListVM = new GameInStoreAdminListVM(this);
            PurchaseListVM = new PurchaseListVM(this);
            AchievementListAdminVM = new AchievementListAdminVM(this, GameListVM);
            ReceiveAchievementListVM = new ReceiveAchievementListVM("admin");
            UpdateUnstoredGamesList();
            DataExport = new DataExport(this);
            CustomQuaryBuilderVM = new CustomQuaryBuilderVM();
        }
        public void AccountUpdate()
        {
            ReportListVM.UpdateList();
            PurchaseListVM.UpdatePurchaseList();
        }
        public void GameUpdate()
        {            
            GameInStoreAdminListVM.UpdateGamesInStoreList();         //обновляем информацию в games in store
            UpdateUnstoredGamesList();

            PurchaseListVM.UpdatePurchaseList();                     //обновляем информацию в purchases            
            AchievementListAdminVM.UpdateAchievementListAdmin();     //обновляем информацию в available achievements
            ReceiveAchievementListVM.UpdateReceiveAchievementList(); //обновить информацию в received achievements
        }        
        public ObservableCollection<GameVM> unstoredGamesList;
        public void UpdateUnstoredGamesList()
        {    
            unstoredGamesList.Clear();
            foreach (var game in GameListVM.ReserveGameList)
                unstoredGamesList.Add(game);         
            foreach (var game in GameListVM.ReserveGameList)
                foreach (var gameInStore in GameInStoreAdminListVM.ReserveGameInStoreAdminList)
                    if (game.game_id == gameInStore.game_id_game)
                        unstoredGamesList.Remove(game);
            GameInStoreAdminListVM.UpdateUnstoredGamesList();
        }
    }
}