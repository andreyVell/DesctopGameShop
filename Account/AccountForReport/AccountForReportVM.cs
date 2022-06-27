using GalaSoft.MvvmLight;

namespace DataBaseIndTask
{
    public class AccountForReportVM : ViewModelBase
    {
        private AccountForReport _accountForReport;
        public AccountForReport AccountForReport => _accountForReport;        

        public AccountForReportListVM ParentVM { set; get; }
        public AccountForReportVM(AccountForReport accountForReport, AccountForReportListVM accountForReportList)
        {
            _accountForReport = accountForReport;
            ParentVM = accountForReportList;
        }
        public int account_id
        {
            get
            {
                return _accountForReport.account_id;
            }
        } 
        public string account_nickname
        {
            get
            {
                return _accountForReport.account_nickname;
            }            
        }        
        public string account_country
        {
            get
            {
                return _accountForReport.account_country;
            }            
        }        
        public string account_birthdate
        {
            get
            {
                return _accountForReport.account_birthdate;
            }
        }        
        public string account_image
        {
            get
            {
                return _accountForReport.account_image;
            }            
        }
    }
}