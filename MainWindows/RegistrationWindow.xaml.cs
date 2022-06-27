using System;
using System.Windows;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void ButtonRegisterComplete_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text.Length != 0 && PasswordTextBox.Password.Length != 0 && NicknameTextBox.Text.Length != 0 && BirthDatePicker.DisplayDate != null)
            {
                if (PasswordTextBox.Password == PasswordRepeatTextBox.Password)
                {
                    try
                    {
                        DataProvider.SetLoginData("sysa","123");
                        DataProvider.Default.AddAccount(
                            new Account()
                            {
                                account_login = LoginTextBox.Text,
                                account_password = PasswordTextBox.Password,
                                account_nickname = NicknameTextBox.Text,
                                account_birthdate = BirthDatePicker.DisplayDate,
                                account_role=1,
                            });
                        DataProvider.Default.CloseConnection();
                        MessageBox.Show("Registration is successful!");                        
                        ShowLogin();
                    }
                    catch(Exception ex) 
                    { 
                        if (ex.Message.Contains("UNIQUE KEY"))
                            MessageBox.Show("This login already used");
                        else
                            MessageBox.Show(ex.Message); 
                    }
                }
                else
                    MessageBox.Show("Password mismatch!");
            }
            else
                MessageBox.Show("Please enter all fields!");
        }
        private void ButtonBackLogin_Click(object sender, RoutedEventArgs e)
        {
            ShowLogin();
        }
        private void ShowLogin()
        {
            Window loginWindow = new LoginWindow();
            this.Hide();
            this.Close();
            loginWindow.Show();
        }
    }
}