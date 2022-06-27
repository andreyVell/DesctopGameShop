using System;
using System.Windows;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Password.Length !=0 && LoginTextBox.Text.Length != 0)
            {
                string login = LoginTextBox.Text;
                string password = PasswordTextBox.Password;
                try
                {
                    DataProvider.SetLoginData(login, password);
                    int userRole = DataProvider.Default.GetAccountListPlayer()[0].account_role;
                    Window nextWindow = null;
                    if (userRole == 1)
                        nextWindow = new mainWindowPlayer();
                    else if (userRole == 0)
                        nextWindow = new mainWindowAdmin();
                    this.Hide();
                    this.Close();
                    nextWindow.Show();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("При входе в систему пользователя"))
                        MessageBox.Show("User does not exist!", "Error!");
                    else
                        MessageBox.Show(ex.Message, "Error!");
                }
            }
            else               
                MessageBox.Show("Please enter your Login and Password","Error!");
        }
        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            { DataProvider.Default.CloseConnection(); }
            catch { }
            Window registersrationWindow = new RegistrationWindow();
            this.Hide();
            this.Close();
            registersrationWindow.Show();
        }
    }
}