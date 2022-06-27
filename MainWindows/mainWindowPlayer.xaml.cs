using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для mainWindowPlayer.xaml
    /// </summary>
    public partial class mainWindowPlayer : Window
    {
        TablesPlayerVM tables;
        public mainWindowPlayer()
        {
            tables = new TablesPlayerVM(this);
            DataContext = tables;
            InitializeComponent();
            if (GamesLibraryDataGrid.Items.Count > 0)
                GamesLibraryDataGrid.SelectedIndex = -1;
            else
                HideLibraryContent();
            if (StoreDataGrid.Items.Count > 0)
                StoreDataGrid.SelectedIndex = 0;
            else
                HideStoreContent();
        }

        private void buttonExitAccount_Click(object sender, RoutedEventArgs e)
        {
            DataProvider.Default.CloseConnection();
            Window loginWindow = new LoginWindow();
            this.Hide();
            this.Close();
            loginWindow.Show();
        }
        private void GamesLibraryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            UpdateTargetLibrary();
            if (GamesLibraryDataGrid.SelectedIndex == -1)
                HideLibraryContent();
            else
                ShowLibraryContent();
        }        
        private void HideLibraryContent()
        {
            libraryBorderAll.Visibility = Visibility.Hidden;
            libraryBorderImage.Visibility = Visibility.Hidden;
            gameImage.Visibility = Visibility.Hidden;
            buttonPlayGame.Visibility = Visibility.Hidden;
            gameNameTextBox.Visibility = Visibility.Hidden;
            GameNameCard.Visibility = Visibility.Hidden;
            GameInfoCard.Visibility = Visibility.Hidden;
        }
        public void ShowLibraryContent()
        {
            libraryBorderAll.Visibility = Visibility.Visible;
            libraryBorderImage.Visibility = Visibility.Visible;
            gameImage.Visibility = Visibility.Visible;
            gameNameTextBox.Visibility = Visibility.Visible;
            buttonPlayGame.Visibility = Visibility.Visible;
            GameNameCard.Visibility = Visibility.Visible;
            GameInfoCard.Visibility = Visibility.Visible;
        }        
        public void HideStoreContent()
        {
            storeBuyButton.Visibility = Visibility.Hidden;
            storeBorderLeft.Visibility = Visibility.Hidden;
            GameStoreCard.Visibility = Visibility.Hidden;
        }
        public void ShowStoreContent()
        {
            storeBuyButton.Visibility = Visibility.Visible;
            storeBorderLeft.Visibility = Visibility.Visible;
            GameStoreCard.Visibility = Visibility.Visible;
        }
        private void UpdateTargetLibrary()
        {
            gameImage.GetBindingExpression(Image.SourceProperty).UpdateTarget();
            gameNameTextBox.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameDescriptionTextBox.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameGenreTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameReleaseDateTextBlockRun1.GetBindingExpression(Run.TextProperty).UpdateTarget();
            gameReleaseDateTextBlockRun2.GetBindingExpression(Run.TextProperty).UpdateTarget();
            gameReleaseDateTextBlockRun3.GetBindingExpression(Run.TextProperty).UpdateTarget();
            gamePublisherTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameDeveloperTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameAgeLimitTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameRatingTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();            
        }
        private void UpdateTargetStore()
        {
            storeBuyButtonContentTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameNameStoreTextBox.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameDescriptionStoreTextBox.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameStoreImage.GetBindingExpression(Image.SourceProperty).UpdateTarget();
            gameGenreStoreTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameReleaseDateStoreTextBlockRun1.GetBindingExpression(Run.TextProperty).UpdateTarget();
            gameReleaseDateStoreTextBlockRun2.GetBindingExpression(Run.TextProperty).UpdateTarget();
            gameReleaseDateStoreTextBlockRun3.GetBindingExpression(Run.TextProperty).UpdateTarget();
            gamePublisherStoreTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameDeveloperStoreTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameAgeLimitStoreTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            gameRatingStoreTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
        private void StoreDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StoreDataGrid.SelectedIndex == -1)
                HideStoreContent();
            else
            {
                UpdateTargetStore();
                ShowStoreContent();
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StoreTextSearch!=null && StorePriceSearch!=null)
                if (tables.GameInStoreListVM.searchType==2)
                {
                    StorePriceSearch.Visibility = Visibility.Visible;
                    StoreTextSearch.Visibility = Visibility.Hidden;
                }
                else
                {
                    StorePriceSearch.Visibility = Visibility.Hidden;
                    StoreTextSearch.Visibility = Visibility.Visible;
                }
        }
        private void PurchaseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PurchaseTextSearch != null && PurchasePriceSearch != null && PurchaseDateSearch!=null)
                switch (tables.GameInLibraryListVM.searchPurchaseType)
                {
                    case 2:
                        PurchasePriceSearch.Visibility = Visibility.Visible;
                        PurchaseTextSearch.Visibility = Visibility.Hidden;
                        PurchaseDateSearch.Visibility = Visibility.Hidden;
                        break;
                    case 3:
                        PurchaseDateSearch.Visibility = Visibility.Visible;
                        PurchaseTextSearch.Visibility = Visibility.Hidden;
                        PurchasePriceSearch.Visibility = Visibility.Hidden;
                        break;
                    default:
                        PurchaseDateSearch.Visibility = Visibility.Hidden;
                        PurchasePriceSearch.Visibility = Visibility.Hidden;
                        PurchaseTextSearch.Visibility = Visibility.Visible;
                        break;
                            
                }                
        }
        private void ReportComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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