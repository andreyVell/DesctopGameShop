using System.Windows;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для CustomQueryBuilderWindow.xaml
    /// </summary>
    public partial class CustomQueryBuilderWindow : Window
    {
        public CustomQueryBuilderWindow(CustomQuaryBuilderVM customQuaryBuilderVM)
        {
            DataContext = customQuaryBuilderVM;
            InitializeComponent();
        }
    }
}