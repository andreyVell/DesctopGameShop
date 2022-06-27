using GalaSoft.MvvmLight;
using System;

namespace DataBaseIndTask
{
    public class PurchaseVM:ViewModelBase
    {
        private Purchase _purchase;
        public Purchase Purchase => _purchase;
        public PurchaseListVM ParentVM { set; get; }
        public PurchaseVM(Purchase purchase, PurchaseListVM purchaseList)
        {
            _purchase = purchase;
            ParentVM = purchaseList;
        }
        public int purchase_id
        {
            get 
            {
                return _purchase.purchase_id;
            }
        }
        public int purchaser_id
        {
            get
            {
                return _purchase.purchaser_id;
            }
        }
        public int purchase_game_id
        {
            get
            {
                return _purchase.purchase_game_id;
            }
        }
        public string purchase_game_name
        {
            get
            {
                return _purchase.purchase_game_name;
            }
        }
        public string purchase_purchaser_login
        {
            get
            {
                return _purchase.purchase_purchaser_login;
            }
        }
        public double purchase_price
        {
            get
            {
                return Convert.ToDouble(_purchase.purchase_price);
            }
        }
        public string purchase_date
        {
            get
            {
                return _purchase.purchase_date.ToString("dd.MM.yyyy HH:mm:ss");
            }
        }
    }
}