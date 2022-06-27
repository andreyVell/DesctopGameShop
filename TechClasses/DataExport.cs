using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.Linq;
using System.Windows;

namespace DataBaseIndTask
{
    public class DataExport : ViewModelBase
    {
        private enum ETableType
        {
            tt_allgames = 0,
            tt_gamesinstore = 1,
            tt_allpurchases = 2,
            tt_allachievements = 3,
            tt_receivedachievements = 4,
            tt_accounts = 5,
            tt_reports = 6,
        }
        private bool _allGamesChecked;
        private bool _gamesInStoreChecked;
        private bool _allPurchasesChecked;
        private bool _allAchievementsChecked;
        private bool _receivedAvhievementsChecked;
        private bool _accountsChecked;
        private bool _reportsChecked;
        private string _filePath;
        private TablesAdminVM _tablesAdminVM;
        public DataExport(TablesAdminVM tablesAdminVM)
        {
            _tablesAdminVM = tablesAdminVM;
            _filePath = string.Empty;
        }
        public bool AllGamesChecked
        {
            get { return _allGamesChecked; }
            set { _allGamesChecked = value; RaisePropertyChanged(nameof(AllGamesChecked)); }
        }
        public bool GamesInStoreChecked
        {
            get { return _gamesInStoreChecked; }
            set { _gamesInStoreChecked = value; RaisePropertyChanged(nameof(GamesInStoreChecked)); }
        }
        public bool AllPurchasesChecked
        {
            get { return _allPurchasesChecked; }
            set { _allPurchasesChecked = value; RaisePropertyChanged(nameof(AllPurchasesChecked)); }
        }
        public bool AllAchievementsChecked
        {
            get { return _allAchievementsChecked; }
            set { _allAchievementsChecked = value; RaisePropertyChanged(nameof(AllAchievementsChecked)); }
        }
        public bool ReceivedAvhievementsChecked
        {
            get { return _receivedAvhievementsChecked; }
            set { _receivedAvhievementsChecked = value; RaisePropertyChanged(nameof(ReceivedAvhievementsChecked)); }
        }
        public bool AccountsChecked
        {
            get { return _accountsChecked; }
            set { _accountsChecked = value; RaisePropertyChanged(nameof(AccountsChecked)); }
        }
        public bool ReportsChecked
        {
            get { return _reportsChecked; }
            set { _reportsChecked = value; RaisePropertyChanged(nameof(ReportsChecked)); }
        }
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; RaisePropertyChanged(nameof(FilePath)); }
        }

        private RelayCommand _openExportWindowCommand;
        public RelayCommand OpenExportWindowCommand =>
            _openExportWindowCommand ?? (_openExportWindowCommand = new RelayCommand(OpenExportWindow));
        public void OpenExportWindow()
        {
            ExportWindow exportWindow = new ExportWindow(_tablesAdminVM);
            exportWindow.Show();
        }

        private RelayCommand _chooseAllCommand;
        public RelayCommand ChooseAllCommand =>
            _chooseAllCommand ?? (_chooseAllCommand = new RelayCommand(ChooseAll));
        public void ChooseAll()
        {
            AllGamesChecked = true;
            GamesInStoreChecked = true;
            AllPurchasesChecked = true;
            AllAchievementsChecked = true;
            ReceivedAvhievementsChecked = true;
            AccountsChecked = true;
            ReportsChecked = true;
        }

        private RelayCommand _clearAllCommand;
        public RelayCommand ClearAllCommand =>
            _clearAllCommand ?? (_clearAllCommand = new RelayCommand(ClearAll));
        public void ClearAll()
        {
            AllGamesChecked = false;
            GamesInStoreChecked = false;
            AllPurchasesChecked = false;
            AllAchievementsChecked = false;
            ReceivedAvhievementsChecked = false;
            AccountsChecked = false;
            ReportsChecked = false;
        }

        private RelayCommand _browseFilePathCommand;
        public RelayCommand BrowseFilePathCommand =>
            _browseFilePathCommand ?? (_browseFilePathCommand = new RelayCommand(BrowseFilePath));
        public void BrowseFilePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Документ Word(*.docx)|*.docx";
            if (saveFileDialog.ShowDialog() == true && saveFileDialog.FileName != null)
                FilePath = saveFileDialog.FileName;
        }
        private RelayCommand _exportTablesCommand;
        public RelayCommand ExportTablesCommand =>
            _exportTablesCommand ?? (_exportTablesCommand = new RelayCommand(ExportTables));        
        public void ExportTables()
        {
            if (FilePath == string.Empty)
            {
                MessageBox.Show("Enter file path!");
                return;
            }
            using (var package = OpenOrCreate())
            {
                if (package.MainDocumentPart == null)
                {
                    CreateNewMarkup(package);
                }
                if (AllGamesChecked)
                {
                    CreateHeader(package, "All Games");
                    CreateTable(package, ETableType.tt_allgames);
                    FillTable(package, ETableType.tt_allgames);
                }
                if (GamesInStoreChecked)
                {
                    CreateHeader(package, "Games in store");
                    CreateTable(package, ETableType.tt_gamesinstore);
                    FillTable(package, ETableType.tt_gamesinstore);
                }
                if (AllPurchasesChecked)
                {
                    CreateHeader(package, "All purchases");
                    CreateTable(package, ETableType.tt_allpurchases);
                    FillTable(package, ETableType.tt_allpurchases);
                }
                if (AllAchievementsChecked)
                {
                    CreateHeader(package, "All achievements");
                    CreateTable(package, ETableType.tt_allachievements);
                    FillTable(package, ETableType.tt_allachievements);
                }
                if (ReceivedAvhievementsChecked)
                {
                    CreateHeader(package, "Received achievements");
                    CreateTable(package, ETableType.tt_receivedachievements);
                    FillTable(package, ETableType.tt_receivedachievements);
                }
                if (AccountsChecked)
                {
                    CreateHeader(package, "Accounts");
                    CreateTable(package, ETableType.tt_accounts);
                    FillTable(package, ETableType.tt_accounts);
                }
                if (ReportsChecked)
                {
                    CreateHeader(package, "Reports");
                    CreateTable(package, ETableType.tt_reports);
                    FillTable(package, ETableType.tt_reports);
                }
                MessageBox.Show("Done!");                
                package.MainDocumentPart.Document.Save();                
            }
            System.Diagnostics.Process.Start(FilePath);
        }
        public WordprocessingDocument OpenOrCreate()
        {
            if (!File.Exists(FilePath))
                return WordprocessingDocument.Create(FilePath, WordprocessingDocumentType.Document);
            else
                return WordprocessingDocument.Open(FilePath, true);
        }
        private void CreateTable(WordprocessingDocument doc, ETableType tableType)
        {
            // Create an empty table.
            Table table = new Table();
            // Create a TableProperties object and specify its border information.
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder()
                    {
                        Val =
                        new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 10
                    },
                    new BottomBorder()
                    {
                        Val =
                        new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 10
                    },
                    new LeftBorder()
                    {
                        Val =
                        new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 10
                    },
                    new RightBorder()
                    {
                        Val =
                        new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 10
                    },
                    new InsideHorizontalBorder()
                    {
                        Val =
                        new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 10
                    },
                    new InsideVerticalBorder()
                    {
                        Val =
                        new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 10
                    }
                )
            );
            // Append the TableProperties object to the empty table.
            table.AppendChild<TableProperties>(tblProp);
            // Create a row.
            TableRow tableRow = new TableRow();
            int columnsCount = GetColumnsCount(tableType);
            string[] columnHeaders = GetColumnHeaders(tableType);
            for (int i = 0; i < columnsCount; i++)
            {
                // Create a cell.
                TableCell tableCell = new TableCell();
                // Specify the width property of the table cell.
                tableCell.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                // Specify the table cell content.
                tableCell.Append(new Paragraph(new Run(new Text(columnHeaders[i]))));
                // Append the table cell to the table row.
                tableRow.Append(tableCell);
            }
            // Append the table row to the table.
            table.Append(tableRow);
            // Append the table to the document.
            doc.MainDocumentPart.Document.Body.AppendChild(table);
            doc.MainDocumentPart.Document.Body.AppendChild(new Body(new Paragraph(new Run(new Text()))));
        }
        private void CreateHeader(WordprocessingDocument doc, string header)
        {      
            Body body = new Body();
            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run());
            RunProperties runProperties = run.AppendChild(new RunProperties());
            Bold bold = new Bold();
            bold.Val = OnOffValue.FromBoolean(true);   
            runProperties.AppendChild(bold);
            run.AppendChild(new Text(header));
            doc.MainDocumentPart.Document.Body.AppendChild(body);
        }
        private void CreateNewMarkup(WordprocessingDocument doc)
        {
            // Add a main document part. 
            MainDocumentPart mainPart = doc.AddMainDocumentPart();
            // Create the document structure and add some text.
            mainPart.Document = new Document();
            mainPart.Document.Body = new Body();
        }
        private int GetColumnsCount(ETableType tableType)
        {
            switch (tableType)
            {
                case ETableType.tt_allgames:
                    return 9;
                case ETableType.tt_gamesinstore:
                    return 2;
                case ETableType.tt_allpurchases:
                    return 7;
                case ETableType.tt_allachievements:
                    return 4;
                case ETableType.tt_receivedachievements:
                    return 7;
                case ETableType.tt_accounts:
                    return 9;
                case ETableType.tt_reports:
                    return 6;
                default:
                    return 0;
            }
        }
        private string[] GetColumnHeaders(ETableType tableType)
        {
            switch (tableType)
            {
                case ETableType.tt_allgames:
                    return new string[9] {"ID","Name","Description","Genre","Release date","Developer","Publisher","Rating", "Age limit" };
                case ETableType.tt_gamesinstore:
                    return new string[2] { "Name", "Price" };
                case ETableType.tt_allpurchases:
                    return new string[7] { "Purchase ID", "User ID", "User login", "Game ID", "Game name", "Price", "Date"};
                case ETableType.tt_allachievements:
                    return new string[4] { "ID", "Name", "Game ID", "Description" };
                case ETableType.tt_receivedachievements:
                    return new string[7] { "User ID", "User login", "Achievement ID", "Achievement name", "Game ID", "Game name", "Receive date" };
                case ETableType.tt_accounts:
                    return new string[9] { "ID", "Login", "Password", "Nickname", "E-mail", "Country", "Balance", "Birth date", "Role" };
                case ETableType.tt_reports:
                    return new string[6] { "Report ID", "Sender ID", "Receiver ID", "Comment", "Recieve date", "Reciever nickname" };
                default:
                    return null;
            }
        }
        private void FillTable(WordprocessingDocument doc, ETableType tableType)
        {
            var table = doc.MainDocumentPart.Document.Descendants<Table>().Last();
            switch (tableType)
            {
                case ETableType.tt_allgames:
                    foreach (var game in _tablesAdminVM.GameListVM.GameList)
                    {
                        var runProperties = new RunProperties();
                        runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "FF0000" });
                        TableRow tr = new TableRow();
                        var paragraphProperties = new ParagraphProperties(new Justification
                        {
                            Val = JustificationValues.Center
                        });
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_name)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_description)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_genre)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_releasedate.ToString("dd.MM.yyyy"))))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_developer)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_publisher)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_rating.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_agelimit.ToString())))));
                        table.Append(tr);
                    }
                    break;
                case ETableType.tt_gamesinstore:
                    foreach (var game in _tablesAdminVM.GameInStoreAdminListVM.GameInStoreAdminList)
                    {
                        var runProperties = new RunProperties();
                        runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "FF0000" });
                        TableRow tr = new TableRow();
                        var paragraphProperties = new ParagraphProperties(new Justification
                        {
                            Val = JustificationValues.Center
                        });
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_name_game)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.game_price.ToString())))));
                        table.Append(tr);
                    }
                    break;
                case ETableType.tt_allpurchases:
                    foreach (var game in _tablesAdminVM.PurchaseListVM.PurchaseList)
                    {
                        var runProperties = new RunProperties();
                        runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "FF0000" });
                        TableRow tr = new TableRow();
                        var paragraphProperties = new ParagraphProperties(new Justification
                        {
                            Val = JustificationValues.Center
                        });
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.purchase_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.purchaser_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.purchase_purchaser_login)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.purchase_game_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.purchase_game_name)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.purchase_price.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.purchase_date)))));
                        table.Append(tr);
                    }
                    break;
                case ETableType.tt_allachievements:
                    foreach (var game in _tablesAdminVM.AchievementListAdminVM.AchievementListAdmin)
                    {
                        var runProperties = new RunProperties();
                        runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "FF0000" });
                        TableRow tr = new TableRow();
                        var paragraphProperties = new ParagraphProperties(new Justification
                        {
                            Val = JustificationValues.Center
                        });
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.achievement_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.achievement_name)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.achievement_game_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.achievement_description)))));
                        table.Append(tr);
                    }
                    break;
                case ETableType.tt_receivedachievements:
                    foreach (var game in _tablesAdminVM.ReceiveAchievementListVM.ReceiveAchievementList)
                    {
                        var runProperties = new RunProperties();
                        runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "FF0000" });
                        TableRow tr = new TableRow();
                        var paragraphProperties = new ParagraphProperties(new Justification
                        {
                            Val = JustificationValues.Center
                        });
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.receiveAchievement_account_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.receiveAchievement_account_login)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.receiveAchievement_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.receiveAchievement_name)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.receiveAchievement_game_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.receiveAchievement_game_name)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.receiveAchievement_date)))));
                        table.Append(tr);
                    }
                    break;
                case ETableType.tt_accounts:
                    foreach (var game in _tablesAdminVM.AccountListVM.AccountList)
                    {
                        var runProperties = new RunProperties();
                        runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "FF0000" });
                        TableRow tr = new TableRow();
                        var paragraphProperties = new ParagraphProperties(new Justification
                        {
                            Val = JustificationValues.Center
                        });
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.account_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.account_login)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.account_password)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.account_nickname)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.account_email)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.account_country)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.account_balance.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.account_birthdate.ToString("dd.MM.yyyy"))))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.account_role)))));
                        table.Append(tr);
                    }
                    break;
                case ETableType.tt_reports:
                    foreach (var game in _tablesAdminVM.ReportListVM.ReportList)
                    {
                        var runProperties = new RunProperties();
                        runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "FF0000" });
                        TableRow tr = new TableRow();
                        var paragraphProperties = new ParagraphProperties(new Justification
                        {
                            Val = JustificationValues.Center
                        });
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.report_id.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.report_senderid.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.report_receiverid.ToString())))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.report_comment)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.report_recievedate)))));
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(game.report_receivernickname)))));
                        table.Append(tr);
                    }
                    break;
            }
        }
    }
}