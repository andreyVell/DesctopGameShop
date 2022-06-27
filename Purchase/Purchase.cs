using System;

namespace DataBaseIndTask
{
    public class Purchase
    {
        public int purchase_id { get; set; }
        public int purchaser_id { get; set; }
        public int purchase_game_id { get; set; }
        public string purchase_game_name { get; set; }
        public string purchase_purchaser_login { get; set; }
        public decimal purchase_price { get; set; }
        public DateTime purchase_date { get; set; }
    }
}