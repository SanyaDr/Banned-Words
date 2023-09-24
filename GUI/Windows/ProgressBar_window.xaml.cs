using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Model;
using System.Windows.Threading;

namespace GUI.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProgressBar_window.xaml
    /// </summary>
    public partial class ProgressBar_window : Window
    {
        ThreadsClass th;
        SelectedFiles selectedFiles;
        BannedWords banWords;
        Report report;
        Filtration filter = new Filtration();
        public Dispatcher disp = Dispatcher.CurrentDispatcher;
        object locker = new();

        public ProgressBar_window(ThreadsClass threads, SelectedFiles selectedfiles, BannedWords banWords, Report reporter)
        {
            InitializeComponent();
            th = threads;
            OpenResultFolder_Button.IsEnabled = false;
            selectedFiles = selectedfiles;
            this.banWords = banWords;
            report = reporter;
            ScanResult_ProgressBar.Value = 0;
            AllCountFiles_Label.Content = selectedfiles.pathsToScan.Length.ToString();
            FilesLeftToScan_Label.Content = selectedfiles.pathsToScan.Length.ToString();
            PathToSaveFiles_TextBox.Text = selectedfiles.pathToReport.ToString();
            Closing += ProgressBar_window_Closing;
        }

        public void CheckFilterStatus()
        {
            try
            {
                bool isFilter = true;
                Thread.Sleep(10);

                while (isFilter)
                {
                    Thread.Sleep(50);
                    disp.Invoke(() =>
                    {
                        lock (locker)
                        {
                            isFilter = filter.isFiltering;

                            ScanResult_ProgressBar.Value = filter.filterStatus;
                            FilesLeftToScan_Label.Content = filter.filesToScanLeft.ToString();

                            var log = report.lastProg.ToArray();
                            Logger_TextBox.Clear();
                            foreach (var line in log)
                            {
                                Logger_TextBox.Text += line;
                                Logger_TextBox.ScrollToEnd();
                            }
                        }
                    });
                }
            }
            catch
            {
                return;
            }
        }

        private void ProgressBar_window_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            th.Kill();
        }

        private void StartScan(object sender, RoutedEventArgs e)
        {
            th.ResumeThreads();
            ScanResult_ProgressBar.Value = 0;
            if (PathToSaveFiles_TextBox.Text.Length <= 0)
            {
                selectedFiles.SelectResultDirectory(string.Empty);
                PathToSaveFiles_TextBox.Text = selectedFiles.pathToFolder;
            }
            else
            {
                selectedFiles.pathToFolder = PathToSaveFiles_TextBox.Text;
            }

            if (!Directory.Exists(selectedFiles.pathToFolder))
            {
                Directory.CreateDirectory(selectedFiles.pathToFolder);
            }
            string _pathToFolder = selectedFiles.pathToFolder;
            Thread scanThread = new Thread(() => { filter.ScanFiles(selectedFiles.pathsToScan, banWords.GetBannedWords(), banWords.GetReplaceString(), report, th, _pathToFolder); MessageBox.Show("Фильтрация файлов завершена!"); });
            scanThread.Start();
            new Thread(CheckFilterStatus).Start();
            OpenResultFolder_Button.IsEnabled = true;
            report.ClearLastLog();
        }

        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", selectedFiles.pathToFolder);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenSaveFileDialog_Button_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker sfd = new FolderPicker();
            if (sfd.ShowDialog() == true)
            {
                //Если путь выбран успешно
                PathToSaveFiles_TextBox.Text = sfd.ResultPath;
            }
            else
            {
                //если путь не выбран
                PathToSaveFiles_TextBox.Text = string.Empty;
            }
        }
    }
}