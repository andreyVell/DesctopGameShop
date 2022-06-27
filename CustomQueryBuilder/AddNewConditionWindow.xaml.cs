using System.Collections.ObjectModel;
using System.Windows;

namespace DataBaseIndTask.CustomQueryBuilder
{
    /// <summary>
    /// Логика взаимодействия для AddNewConditionWindow.xaml
    /// </summary>
    public partial class AddNewConditionWindow : Window
    {
        public CustomQuaryBuilderVM _customQuaryBuilderVM;
        public AddNewConditionWindow(ObservableCollection<string> tablesUsedInQuery, ObservableCollection<ConditionVM> ConditionList, CustomQuaryBuilderVM customQuaryBuilderVM)
        {
            _customQuaryBuilderVM = customQuaryBuilderVM;
            InitializeComponent();
            DataContext = new AddNewConditionVM(tablesUsedInQuery, ConditionList, this);
            if (ConditionList.Count == 0)
                LinkComboBox.IsEnabled = false;
        }
    }
}