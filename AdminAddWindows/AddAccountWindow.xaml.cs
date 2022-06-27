using System.Windows;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для AddAccountWindow.xaml
    /// </summary>
    public partial class AddAccountWindow : Window
    {
        private AccountListVM _parentVM { get; set; }
        public AddAccountWindow(AccountListVM parentVM)
        {
            _parentVM = parentVM;
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text.Length != 0 && PasswordTextBox.Text.Length != 0 && NicknameTextBox.Text.Length != 0 && BirthDatePicker.DisplayDate != null && (RoleComboBox.SelectedIndex==0 || RoleComboBox.SelectedIndex == 1))
            {
                Account newAccount = new Account()
                {
                    account_login = LoginTextBox.Text,
                    account_password = PasswordTextBox.Text,
                    account_nickname = NicknameTextBox.Text,
                    account_birthdate = BirthDatePicker.DisplayDate,
                    account_role = RoleComboBox.SelectedIndex,
                };
                DataProvider.Default.AddAccount(newAccount);
                _parentVM.ReserveAccountList.Add(new AccountVM(DataProvider.Default.GetLastAccount(), _parentVM));
                _parentVM.UpdateFilteredAccountList();
                this.Close();
            }
            else
                MessageBox.Show("Please enter all fields!");
        }
    }
}