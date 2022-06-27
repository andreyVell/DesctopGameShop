namespace DataBaseIndTask.CustomQueryBuilder
{
    public class GamesShopTable
    {
        private static bool _game_id;
        private static bool _game_price;
        private static CustomQuaryBuilderVM _customQuaryBuilderVM;
        public GamesShopTable(CustomQuaryBuilderVM customQuaryBuilderVM)
        {
            _customQuaryBuilderVM = customQuaryBuilderVM;
        }
        public static bool Game_id
        {
            get
            {
                return _game_id;
            }
            set
            {
                _game_id = value;
                if (_game_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Games_shop.game_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Games_shop.game_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Game_price
        {
            get
            {
                return _game_price;
            }
            set
            {
                _game_price = value;
                if (_game_price)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Games_shop.game_price");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Games_shop.game_price");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
    }
}