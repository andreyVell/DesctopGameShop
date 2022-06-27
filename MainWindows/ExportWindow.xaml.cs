using System.Windows;
using System.Windows.Controls;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        private TablesAdminVM _tablesAdminVM;
        public ExportWindow(TablesAdminVM tablesAdminVM)
        {
            _tablesAdminVM = tablesAdminVM;
            InitializeComponent();
            DataContext = _tablesAdminVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeViewItem item1 in TreeViewSection.Items)            
                item1.IsExpanded = true;      
        }
    }
}