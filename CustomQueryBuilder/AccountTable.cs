namespace DataBaseIndTask.CustomQueryBuilder
{
    public class AccountTable
    {
        private static bool _account_id;
        private static bool _account_login;
        private static bool _account_password;
        private static bool _account_nickname;
        private static bool _account_email;
        private static bool _account_country;
        private static bool _account_balance;
        private static bool _account_birthdate;
        private static bool _account_role;
        private static bool _account_image;
        private static CustomQuaryBuilderVM _customQuaryBuilderVM;
        public AccountTable(CustomQuaryBuilderVM customQuaryBuilderVM)
        {
            _customQuaryBuilderVM = customQuaryBuilderVM;
        }
        public static bool Account_id
        {
            get
            {
                return _account_id;
            }
            set
            {
                _account_id = value;
                if (_account_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_login
        {
            get
            {
                return _account_login;
            }
            set
            {
                _account_login = value;
                if (_account_login)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_login");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_login");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_password
        {
            get
            {
                return _account_password;
            }
            set
            {
                _account_password = value;
                if (_account_password)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_password");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_password");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_nickname
        {
            get
            {
                return _account_nickname;
            }
            set
            {
                _account_nickname = value;
                if (_account_nickname)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_nickname");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_nickname");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_email
        {
            get
            {
                return _account_email;
            }
            set
            {
                _account_email = value;
                if (_account_email)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_email");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_email");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_country
        {
            get
            {
                return _account_country;
            }
            set
            {
                _account_country = value;
                if (_account_country)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_country");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_country");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_balance
        {
            get
            {
                return _account_balance;
            }
            set
            {
                _account_balance = value;
                if (_account_balance)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_balance");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_balance");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_birthdate
        {
            get
            {
                return _account_birthdate;
            }
            set
            {
                _account_birthdate = value;
                if (_account_birthdate)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_birthdate");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_birthdate");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_role
        {
            get
            {
                return _account_role;
            }
            set
            {
                _account_role = value;
                if (_account_role)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_role");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_role");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Account_image
        {
            get
            {
                return _account_image;
            }
            set
            {
                _account_image = value;
                if (_account_image)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Account.account_image");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Account.account_image");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
    }
}