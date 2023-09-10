using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using ViewModel;

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
        bool isStarted = false, isFinished = false;
        string resultPath = string.Empty;
        public ProgressBar_window(ThreadsClass threads, SelectedFiles selectedfiles, BannedWords banWords, Report reporter)
        {
            InitializeComponent();
            th = threads;
            OpenResultFolder_Button.IsEnabled = false;
            PauseResume_Button.IsEnabled = false;
            selectedFiles = selectedfiles;
            this.banWords = banWords;
            report = reporter;
        }

        private void StartScan(object sender, RoutedEventArgs e)
        {
            th.ResumeThreads();
            isStarted = true;
            if(PathToSaveFiles_TextBox.Text.Length <= 0)
            {
                PathToSaveFiles_TextBox.Text = resultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Запрещенные слова";
            }

            Thread scanThread = new Thread(() => Filtration.ScanFiles(selectedFiles, banWords, report, th, resultPath));
            scanThread.Start();
        }

        private void PauseAndResume(object sender, RoutedEventArgs e)
        {

        }
        private void OpenFolder(object sender, RoutedEventArgs e)
        {

        }

        private void Exit(object sender, RoutedEventArgs e)
        {

        }

        private void OpenSaveFileDialog_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
