using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Model;

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
        string baseFolder = "\\Запрещенные слова";
        public ProgressBar_window(ThreadsClass threads, SelectedFiles selectedfiles, BannedWords banWords, Report reporter)
        {
            InitializeComponent();
            th = threads;
            OpenResultFolder_Button.IsEnabled = false;
            selectedFiles = selectedfiles;
            this.banWords = banWords;
            report = reporter;
            Closing += ProgressBar_window_Closing;
        }

        private void ProgressBar_window_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            th.Kill();
        }

        private void StartScan(object sender, RoutedEventArgs e)
        {
            th.ResumeThreads();
            if(PathToSaveFiles_TextBox.Text.Length <= 0)
            {
                PathToSaveFiles_TextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + baseFolder;
            }
            selectedFiles.pathToFolder = PathToSaveFiles_TextBox.Text;

            int i = 2;
            if (!Directory.Exists(selectedFiles.pathToFolder))
            {
                Directory.CreateDirectory(selectedFiles.pathToFolder);
            }
            string t = selectedFiles.pathToFolder;
            Thread scanThread = new Thread(() => Filtration.ScanFiles(selectedFiles, banWords, report, th, t));
            scanThread.Start();
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

        }
    }
}
