using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class ReportListVM : ViewModelBase
    {
        enum ESearchType
        {
            report_id = 0,
            receiver_nickname = 1,
            date = 2,
            sender_id = 3,
            receiver_id = 4,
            comment = 5,
        }
        private ESearchType _searchType;
        private string _searchFilter;
        private DateTime _dateFilter;

        public TablesPlayerVM _tablesPlayerVM;
        public ReportListVM(TablesPlayerVM tablesPlayerVM)
        {
            _tablesPlayerVM = tablesPlayerVM;
            ReportList = new ObservableCollection<ReportVM>();
            ReserveReportList = new ObservableCollection<ReportVM>();
            var reportListVM = DataProvider.Default.GetReportListPlayer().
                Select(e => new ReportVM(e, this));
            foreach (var p in reportListVM)
            {
                ReportList.Add(p);
                ReserveReportList.Add(p);
            }
            _searchFilter = string.Empty;
            _searchType = ESearchType.report_id;
            searchTypes = new ObservableCollection<string>();
            searchTypes.Add("ID");
            searchTypes.Add("Nickname");
            searchTypes.Add("Date");
            if (tablesPlayerVM==null)
            {
                searchTypes.Add("Sender ID");
                searchTypes.Add("Receiver ID");
                searchTypes.Add("Comment");
            }
        }
        public ObservableCollection<ReportVM> ReportList { get; set; }
        public ObservableCollection<ReportVM> ReserveReportList { get; set; }
        public ObservableCollection<string> searchTypes { get; set; }
        public ReportVM SelectedReport { get; set; }

        RelayCommand _sendNewReportCommand;
        public RelayCommand SendNewReportCommand =>
            _sendNewReportCommand ?? (_sendNewReportCommand = new RelayCommand(SendNewReport));
        public void SendNewReport()
        {
            NewReport newReport = new NewReport(_tablesPlayerVM.AccountListVM.ReserveAccountList[0].account_id, this);            
            newReport.Show();
        }

        RelayCommand _deleteReportCommand;
        public RelayCommand DeleteReportCommand =>
            _deleteReportCommand ?? (_deleteReportCommand = new RelayCommand(DeleteReport));
        public void DeleteReport()
        {
            DataProvider.Default.DeleteReport(SelectedReport.Report);
            ReserveReportList.Remove(SelectedReport);
            ReportList.Remove(SelectedReport);
        }
        public void UpdateList()
        {         
            
            ReserveReportList.Clear();
            var NewReportListVM = DataProvider.Default.GetReportListPlayer().
                Select(e => new ReportVM(e, this));
            foreach (var p in NewReportListVM)
                ReserveReportList.Add(p);
        }
        public string searchFilter
        {
            get
            {
                return _searchFilter;
            }
            set
            {
                if (value == null)
                    _searchFilter = string.Empty;
                else
                    _searchFilter = value;                
                UpdateFilteredReportList();
            }
        }
        public int searchType
        {
            get
            {
                return ((int)_searchType);
            }
            set
            {
                _searchType = (ESearchType)value;
                searchFilter = string.Empty;
            }
        }
        public DateTime dateFilter
        {
            get
            {
                return _dateFilter;
            }
            set
            {
                _dateFilter = value;
                UpdateFilteredReportList();
            }
        }
        public void UpdateFilteredReportList()
        {
            if (_searchType != ESearchType.date && _searchFilter == "")
            {
                ReportList.Clear();
                foreach (var game in ReserveReportList)
                    ReportList.Add(game);
                return;
            }
            switch (_searchType)
            {
                case ESearchType.report_id:
                    {
                        try
                        {
                            ReportList.Clear();
                            foreach (var game in ReserveReportList)
                                if (game.report_id == Convert.ToInt32(searchFilter))
                                    ReportList.Add(game);
                        }
                        catch (Exception ex) { MessageBox.Show("Incorrect ID format!"); }
                    }
                    break;
                case ESearchType.sender_id:
                    {
                        try
                        {
                            ReportList.Clear();
                            foreach (var game in ReserveReportList)
                                if (game.report_senderid == Convert.ToInt32(searchFilter))
                                    ReportList.Add(game);
                        }
                        catch (Exception ex) { MessageBox.Show("Incorrect ID format!"); }
                    }
                    break;
                case ESearchType.receiver_id:
                    {
                        try
                        {
                            ReportList.Clear();
                            foreach (var game in ReserveReportList)
                                if (game.report_receiverid == Convert.ToInt32(searchFilter))
                                    ReportList.Add(game);
                        }
                        catch (Exception ex) { MessageBox.Show("Incorrect ID format!"); }
                    }
                    break;
                case ESearchType.receiver_nickname:
                    {
                        ReportList.Clear();
                        foreach (var game in ReserveReportList)
                            if (game.report_receivernickname.ToLower().Contains(searchFilter.ToLower()))
                                ReportList.Add(game);
                    }
                    break;                
                case ESearchType.date:
                    {
                        ReportList.Clear();
                        foreach (var game in ReserveReportList)
                            if (dateFilter.ToString("dd.MM.yyyy") == game.report_recievedate.Substring(0, 10))
                                ReportList.Add(game);
                    }
                    break;
                case ESearchType.comment:
                    {
                        ReportList.Clear();
                        foreach (var game in ReserveReportList)
                            if (game.report_comment.ToLower().Contains(searchFilter.ToLower()))
                                ReportList.Add(game);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}