using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Windows;

namespace DataBaseIndTask
{
    public class AccountVM:ViewModelBase
    {
        private Account _account;
        public Account Account => _account;
        public AccountListVM ParentVM { set; get; }
        public AccountVM(Account account, AccountListVM accountList)
        {
            _account = account;
            ParentVM = accountList;
        }

        public int account_id
        {
            get
            {
                return _account.account_id;
            }                
        }
        public string account_login
        {
            get
            {
                return _account.account_login;
            }            
        }
        public string account_password
        {
            get
            {
                return _account.account_password;
            }
            set
            {                
                _account.account_password = value;
                DataProvider.Default.UpdateAccount(_account);
                ParentVM._tablesAdminVM?.AccountUpdate();
            }
        }
        public string account_nickname
        {
            get
            {
                return _account.account_nickname;
            }
            set
            {
                _account.account_nickname = value;
                DataProvider.Default.UpdateAccount(_account);
                ParentVM._tablesAdminVM?.AccountUpdate();
            }
        }
        public string account_email
        {
            get
            {
                return _account.account_email;
            }
            set
            {
                _account.account_email = value;
                DataProvider.Default.UpdateAccount(_account);
                ParentVM._tablesAdminVM?.AccountUpdate();
            }
        }
        public string account_country
        {
            get
            {
                return _account.account_country;
            }
            set
            {
                _account.account_country = value;
                DataProvider.Default.UpdateAccount(_account);
                ParentVM._tablesAdminVM?.AccountUpdate();
            }
        }
        public double account_balance
        {
            get
            {
                return Convert.ToDouble(_account.account_balance);
            }
            set
            {
                _account.account_balance = Convert.ToDecimal(value);
                DataProvider.Default.UpdateAccount(_account);
                RaisePropertyChanged(nameof(account_balance));
                ParentVM._tablesAdminVM?.AccountUpdate();
            }
        }
        public DateTime account_birthdate
        {
            get
            {
                return _account.account_birthdate;
            }
            set
            {
                if (value == null)
                    MessageBox.Show("Date of birth required");
                else
                {
                    _account.account_birthdate = value;
                    DataProvider.Default.UpdateAccount(_account);
                    ParentVM._tablesAdminVM?.AccountUpdate();
                }
            }
        }
        public string account_role
        {
            get
            {
                if (_account.account_role == 0)
                    return "Admin";
                else return "Player";
            }            
        }
        public string account_image
        {
            get
            {
                return _account.account_image;
            }
            set
            {
                _account.account_image = value;
                DataProvider.Default.UpdateAccount(_account);
                RaisePropertyChanged(nameof(account_image));
                ParentVM._tablesAdminVM?.AccountUpdate();
            }
        }

        private RelayCommand _increaseBalance;
        public RelayCommand IncreaseBalanceCommand =>
            _increaseBalance ?? (_increaseBalance = new RelayCommand(IncreaseBalance));
        public void IncreaseBalance()
        {
            int sum = 150;
            account_balance += sum;
        }

        private RelayCommand _changeImage;
        public RelayCommand ChangeImageCommand =>
            _changeImage ?? (_changeImage = new RelayCommand(ChangeImage));
        public void ChangeImage()
        {
            MessageBox.Show("Recommended image width/height ratio is 1/1");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.PNG)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName != null)
                account_image = openFileDialog.FileName;
        }

        private RelayCommand _browseImageCommand;
        public RelayCommand BrowseImageCommand =>
            _browseImageCommand ?? (_browseImageCommand = new RelayCommand(BrowseImage));
        public void BrowseImage()
        {
            MessageBox.Show("Recommended image width/height ratio is 1/1");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName!=null)
                account_image = openFileDialog.FileName;            
        }
    }
}