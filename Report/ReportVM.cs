using GalaSoft.MvvmLight;

namespace DataBaseIndTask
{
    public class ReportVM:ViewModelBase
    {
        private Report _report;
        public Report Report => _report;

        public ReportListVM ParentVM { set; get; }
        public ReportVM(Report report, ReportListVM reportList)
        {
            _report = report;
            ParentVM = reportList;            
        }
        public int report_id
        {
            get
            {
                return _report.report_id;
            }
        }
        public string report_comment 
        { 
            get
            {
                return _report.report_comment;
            }             
        }
        public string report_recievedate
        {
            get
            {
                return _report.report_recievedate.ToString("dd.MM.yyyy HH:mm:ss");
            }
        }
        public string report_receivernickname
        {
            get
            {
                return _report.report_receivernickname;
            }
        }
        public string report_receiverimage
        {
            get
            {
                return _report.report_receiverimage;
            }
        }
        public int report_senderid
        {
            get
            {
                return _report.report_senderid;
            }
        }
        public int report_receiverid
        {
            get
            {
                return _report.report_receiverid;
            }
        }
    }
}