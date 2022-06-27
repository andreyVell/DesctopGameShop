using System;
using System.Windows;

namespace DataBaseIndTask
{
    /// <summary>
    /// Логика взаимодействия для NewReport.xaml
    /// </summary>
    public partial class NewReport : Window
    {        
        private int _new_report_sender_id { get; set; }
        private ReportListVM _parentVM { get; set; }
        public NewReport(int new_report_sender_id, ReportListVM parentVM)
        {
            DataContext = new AccountForReportListVM();
            InitializeComponent();
            _new_report_sender_id = new_report_sender_id;
            _parentVM = parentVM;
        }
        private void ButtonSendReport_Click(object sender, RoutedEventArgs e)
        {
            AccountForReportVM SelectedAccountForReport = (AccountForReportVM)ReportDataGrid.SelectedValue;            
            if (SelectedAccountForReport != null || commentTextBox.Text.Length==0)
            {
                Report report = new Report()
                {
                    report_comment = commentTextBox.Text,
                    report_recievedate = DateTime.Now,
                    report_receiverid = SelectedAccountForReport.account_id,
                    report_senderid = _new_report_sender_id,
                };
                DataProvider.Default.AddReport(report);                
                _parentVM.ReserveReportList.Add(new ReportVM(DataProvider.Default.GetLastReport(), _parentVM));
                _parentVM.UpdateFilteredReportList();
                this.Close();
            }
            else
                MessageBox.Show("Select account and write a comment!");            
        }
    }
}