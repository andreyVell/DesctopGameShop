using System;

namespace DataBaseIndTask
{
    public class Account
    {
        public int account_id { get; set; }
        public string account_login { get; set; }
        public string account_password { get; set; }
        public string account_nickname { get; set; }
        public string account_email { get; set; }
        public string account_country { get; set; }
        public decimal account_balance { get; set; }
        public DateTime account_birthdate { get; set; }
        public int account_role { get; set; }
        public string account_image { get; set; }
    }
}