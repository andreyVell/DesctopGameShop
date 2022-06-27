using System.Collections.ObjectModel;
using System.Windows;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для AddAchievementWindow.xaml
    /// </summary>
    public partial class AddAchievementWindow : Window
    {
        public AddAchievementWindow(ObservableCollection<GameVM> gameList, TablesAdminVM tablesAdminVM)
        {
            InitializeComponent();
            DataContext = new AddNewAchievementVM(gameList, tablesAdminVM , this);
        }
    }
}