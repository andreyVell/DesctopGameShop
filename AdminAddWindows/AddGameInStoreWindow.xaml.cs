using System.Collections.ObjectModel;
using System.Windows;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для AddGameInStoreWindow.xaml
    /// </summary>
    public partial class AddGameInStoreWindow : Window
    {
        public GameInStoreAdminListVM parentClassStore;
        public AddGameInStoreWindow(ObservableCollection<GameVM> _unstoredGamesList, GameInStoreAdminListVM parentClass)
        {            
            InitializeComponent();
            parentClassStore = parentClass;
            DataContext = new AddNewGameInStoreVM(_unstoredGamesList, this);
        }
    }
}