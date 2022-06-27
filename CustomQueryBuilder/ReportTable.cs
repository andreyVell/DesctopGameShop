namespace DataBaseIndTask.CustomQueryBuilder
{
    public class ReportTable
    {
        private static bool _report_id;
        private static bool _report_comment;
        private static bool _report_receive_date;
        private static bool _report_sender_id;
        private static bool _report_receiver_id;
        private static CustomQuaryBuilderVM _customQuaryBuilderVM;
        public ReportTable(CustomQuaryBuilderVM customQuaryBuilderVM)
        {
            _customQuaryBuilderVM = customQuaryBuilderVM;
        }
        public static bool Report_id
        {
            get
            {
                return _report_id;
            }
            set
            {
                _report_id = value;
                if (_report_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Report.report_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Report.report_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Report_comment
        {
            get
            {
                return _report_comment;
            }
            set
            {
                _report_comment = value;
                if (_report_comment)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Report.report_comment");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Report.report_comment");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Report_receive_date
        {
            get
            {
                return _report_receive_date;
            }
            set
            {
                _report_receive_date = value;
                if (_report_receive_date)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Report.report_recieve_date");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Report.report_recieve_date");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Report_sender_id
        {
            get
            {
                return _report_sender_id;
            }
            set
            {
                _report_sender_id = value;
                if (_report_sender_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Report.report_sender_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Report.report_sender_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
        public static bool Report_receiver_id
        {
            get
            {
                return _report_receiver_id;
            }
            set
            {
                _report_receiver_id = value;
                if (_report_receiver_id)
                    _customQuaryBuilderVM.UsedFieldsList.Add("Report.report_receiver_id");
                else
                    _customQuaryBuilderVM.UsedFieldsList.Remove("Report.report_receiver_id");
                _customQuaryBuilderVM.UpdateSQLQuery();
            }
        }
    }
}