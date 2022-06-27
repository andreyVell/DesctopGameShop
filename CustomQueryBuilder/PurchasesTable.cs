namespace DataBaseIndTask.CustomQueryBuilder
{
    public class PurchasesTable
    {
        private static bool _purchase_id;
        private static bool _purchaser_id;
        private static bool _purchase_game_id;
        private static bool _purchase_game_name;
        private static bool _purchase_purchaser_login;
        private static bool _purchase_price;
        private static bool _purchase_date;
        private static CustomQuaryBuilderVM _customQuaryBuilderVM;
        public PurchasesTable(CustomQuaryBuilderVM customQuaryBuilderVM)
        {
            _customQuaryBuilderVM = customQuaryBuilderVM;
        }
        public static bool Purchase_id
        {
            get
            {
                return _purchase_id;
            }
            set
            {
                _purchase_id = value;
                if (_purchase_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Purchases.purchase_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Purchases.purchase_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Purchaser_id
        {
            get
            {
                return _purchaser_id;
            }
            set
            {
                _purchaser_id = value;
                if (_purchaser_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Purchases.purchaser_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Purchases.purchaser_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Purchase_game_id
        {
            get
            {
                return _purchase_game_id;
            }
            set
            {
                _purchase_game_id = value;
                if (_purchase_game_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Purchases.purchase_game_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Purchases.purchase_game_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Purchase_game_name
        {
            get
            {
                return _purchase_game_name;
            }
            set
            {
                _purchase_game_name = value;
                if (_purchase_game_name)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Purchases.purchase_game_name");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Purchases.purchase_game_name");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Purchase_purchaser_login
        {
            get
            {
                return _purchase_purchaser_login;
            }
            set
            {
                _purchase_purchaser_login = value;
                if (_purchase_purchaser_login)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Purchases.purchase_purchaser_login");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Purchases.purchase_purchaser_login");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Purchase_price
        {
            get
            {
                return _purchase_price;
            }
            set
            {
                _purchase_price = value;
                if (_purchase_price)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Purchases.purshase_price");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Purchases.purshase_price");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Purchase_date
        {
            get
            {
                return _purchase_date;
            }
            set
            {
                _purchase_date = value;
                if (_purchase_date)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Purchases.purshase_date");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Purchases.purshase_date");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
    }
}