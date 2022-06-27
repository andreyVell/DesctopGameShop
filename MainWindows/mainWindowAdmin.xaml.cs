using System.Windows;
namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для mainWindowAdmin.xaml
    /// </summary>
    public partial class mainWindowAdmin : Window
    {
        TablesAdminVM tables;
        public mainWindowAdmin()
        {
            tables = new TablesAdminVM(this);
            DataContext = tables;
            InitializeComponent();            
        }

        private void buttonExitAccount_Click(object sender, RoutedEventArgs e)
        {
            DataProvider.Default.CloseConnection();
            Window loginWindow = new LoginWindow();
            this.Hide();
            this.Close();
            loginWindow.Show();
        }
        private void AllGameComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (AllGameTextSearch == null || AllGameDateSearch == null || AllGameComboBox == null)
                return;
            if (AllGameComboBox.SelectedIndex == 4)
            {
                AllGameTextSearch.Visibility = Visibility.Hidden;
                AllGameDateSearch.Visibility = Visibility.Visible;
            }
            else
            {
                AllGameTextSearch.Visibility = Visibility.Visible;
                AllGameDateSearch.Visibility = Visibility.Hidden;
            }
        }
        private void GamesIsStoreComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GamesIsStoreTextSearch == null || GamesIsStorePriceSearch == null)
                return;
            if (GamesIsStoreComboBox.SelectedIndex == 1)
            {
                GamesIsStoreTextSearch.Visibility = Visibility.Hidden;
                GamesIsStorePriceSearch.Visibility = Visibility.Visible;
            }
            else
            {
                GamesIsStoreTextSearch.Visibility = Visibility.Visible;
                GamesIsStorePriceSearch.Visibility = Visibility.Hidden;
            }
        }
        private void PurchaseComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PurchaseTextSearch == null || PurchasePriceSearch == null || PurchaseDateSearch == null)
                return;
            switch(PurchaseComboBox.SelectedIndex)
            {
                case 5:
                    PurchasePriceSearch.Visibility = Visibility.Visible;
                    PurchaseDateSearch.Visibility = Visibility.Hidden;
                    PurchaseTextSearch.Visibility = Visibility.Hidden;
                    break;
                case 6:
                    PurchaseDateSearch.Visibility = Visibility.Visible;
                    PurchaseTextSearch.Visibility = Visibility.Hidden;
                    PurchasePriceSearch.Visibility = Visibility.Hidden;
                    break;
                default:
                    PurchaseTextSearch.Visibility = Visibility.Visible;
                    PurchaseDateSearch.Visibility = Visibility.Hidden;
                    PurchasePriceSearch.Visibility = Visibility.Hidden;
                    break;
            }
        }
        private void ReceivedAchievementsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ReceivedAchievementsTextSearch == null || ReceivedAchievementsDateSearch == null)
                return;
            switch (ReceivedAchievementsComboBox.SelectedIndex)
            {                
                case 6:
                    ReceivedAchievementsDateSearch.Visibility = Visibility.Visible;
                    ReceivedAchievementsTextSearch.Visibility = Visibility.Hidden;
                    break;
                default:
                    ReceivedAchievementsTextSearch.Visibility = Visibility.Visible;
                    ReceivedAchievementsDateSearch.Visibility = Visibility.Hidden;
                    break;
            }
        }
        private void AccountsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (AccountsTextSearch == null || AccountsPriceSearch == null || AccountsDateSearch == null)
                return;
            switch (AccountsComboBox.SelectedIndex)
            {
                case 6:
                    AccountsPriceSearch.Visibility = Visibility.Visible;
                    AccountsDateSearch.Visibility = Visibility.Hidden;
                    AccountsTextSearch.Visibility = Visibility.Hidden;
                    break;
                case 7:
                    AccountsDateSearch.Visibility = Visibility.Visible;
                    AccountsTextSearch.Visibility = Visibility.Hidden;
                    AccountsPriceSearch.Visibility = Visibility.Hidden;
                    break;
                default:
                    AccountsTextSearch.Visibility = Visibility.Visible;
                    AccountsDateSearch.Visibility = Visibility.Hidden;
                    AccountsPriceSearch.Visibility = Visibility.Hidden;
                    break;
            }
        }
        private void ReportComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ReportTextSearch != null && ReportDateSearch != null)
                switch (tables.ReportListVM.searchType)
                {
                    case 2:
                        ReportDateSearch.Visibility = Visibility.Visible;
                        ReportTextSearch.Visibility = Visibility.Hidden;
                        break;
                    default:
                        ReportDateSearch.Visibility = Visibility.Hidden;
                        ReportTextSearch.Visibility = Visibility.Visible;
                        break;
                }
        }
    }
}