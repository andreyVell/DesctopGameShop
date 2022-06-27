using System;

namespace DataBaseIndTask
{
    public class Report
    {
        public int report_id { get; set; }
        public string report_comment { get; set; }
        public DateTime report_recievedate { get; set; }
        public string report_receivernickname { get; set; }
        public string report_receiverimage { get; set; }
        public int report_senderid { get; set; }
        public int report_receiverid { get; set; }
    }
}