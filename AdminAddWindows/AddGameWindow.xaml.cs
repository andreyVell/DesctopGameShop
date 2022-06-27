using System;
using System.Windows;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для AddGameWindow.xaml
    /// </summary>
    public partial class AddGameWindow : Window
    {
        private TablesAdminVM _tablesAdminVM;
        public AddGameWindow(TablesAdminVM tablesAdminVM)
        {
            InitializeComponent();
            _tablesAdminVM = tablesAdminVM;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            byte rating= 0;
            byte ageLimit = 0;
            if (NameGameTextBox.Text.Length == 0)
            { MessageBox.Show("Please enter the name of the game"); return; }
            if (DescriptionGameTextBox.Text.Length == 0)
            { MessageBox.Show("Please enter the description of the game"); return; }
            if (GenreGameTextBox.Text.Length == 0)
            { MessageBox.Show("Please enter the genre of the game"); return; }
            if (ReleaseDateGamePicker.DisplayDate.ToString() == "")
            { MessageBox.Show("Please enter the release date of the game"); return; }
            if (DeveloperGameTextBox.Text.Length == 0)
            { MessageBox.Show("Please enter the developer of the game"); return; }
            if (PublisherGameTextBox.Text.Length == 0)
            { MessageBox.Show("Please enter the publisher of the game"); return; }
            if (!Byte.TryParse(RatingGameTextBox.Text, out rating) || !(rating >= 0 && rating <= 10)) 
            { MessageBox.Show("Please enter the rating of the game (0-10)"); return; }
            if (!Byte.TryParse(AgelimitGameTextBox.Text, out ageLimit) || !(ageLimit >= 0 && ageLimit <= 21)) 
            { MessageBox.Show("Please enter the Age limit of the game (0-21)"); return; }
            Game game = new Game()
            {
                game_name = NameGameTextBox.Text,
                game_description = DescriptionGameTextBox.Text,
                game_genre = GenreGameTextBox.Text,
                game_releasedate = ReleaseDateGamePicker.DisplayDate,
                game_developer = DeveloperGameTextBox.Text,
                game_publisher = PublisherGameTextBox.Text,
                game_rating = rating,
                game_agelimit = ageLimit,
            };
            try
            {
                DataProvider.Default.AddGame(game);
                _tablesAdminVM.GameListVM.ReserveGameList.Add(new GameVM(DataProvider.Default.GetLastGame(), _tablesAdminVM.GameListVM));
                _tablesAdminVM.GameUpdate();
                _tablesAdminVM.GameListVM.UpdateFilteredGameList();
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}