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
            Closing += ProgressBar_window_Closing;
        }

        public void CheckFilterStatus()
        {
            try
            {
                bool isFilter = true;
                Thread.Sleep(150);
                {
                    bool waiting = true;
                    int tick = 0;
                    while (tick < 150 && waiting)
                    {
                        Thread.Sleep(100);
                        disp.Invoke(() => { waiting = !filter.filtStarted; });
                        tick++;
                    }
                }

                while (isFilter)
                {
                    Thread.Sleep(50);
                    disp.Invoke(() =>
                    {

                        isFilter = filter.isFiltering;

                        ScanResult_ProgressBar.Value = filter.filterStatus;
                        FilesLeftToScan_Label.Content = filter.filesToScanLeft.ToString();
                    });
                }
            }
            catch (Exception ex)
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
            if(PathToSaveFiles_TextBox.Text.Length <= 0)
            {
                PathToSaveFiles_TextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + selectedFiles.baseFolder ;
            }

            if (PathToSaveFiles_TextBox.Text.EndsWith('\\'))
            {
                PathToSaveFiles_TextBox.Text.Remove(PathToSaveFiles_TextBox.Text.Length - 1, 1);
            }
            selectedFiles.pathToFolder = PathToSaveFiles_TextBox.Text;
           
            if (!Directory.Exists(selectedFiles.pathToFolder))
            {
                Directory.CreateDirectory(selectedFiles.pathToFolder);
            }
            else
            {
                selectedFiles.pathToFolder += selectedFiles.baseFolder;
                Directory.CreateDirectory(selectedFiles.pathToFolder);
            }
            string t = selectedFiles.pathToFolder;
            Thread scanThread = new Thread(() => { filter.ScanFiles(selectedFiles.pathsToScan, banWords.GetBannedWords(), banWords.GetReplaceString(), report, th, t); MessageBox.Show("Фильтрация файлов завершена!"); });
            scanThread.Start();
            new Thread(CheckFilterStatus).Start();
            OpenResultFolder_Button.IsEnabled = true;
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
