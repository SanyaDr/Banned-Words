using System;
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
        bool isStarted = false, isFinished = false;
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
                PathToSaveFiles_TextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + baseFolder;
            }

            int i = 2;
            if (!Directory.Exists(PathToSaveFiles_TextBox.Text))
            {
                Directory.CreateDirectory(PathToSaveFiles_TextBox.Text);
            }
            string t = PathToSaveFiles_TextBox.Text;
            Thread scanThread = new Thread(() => Filtration.ScanFiles(selectedFiles, banWords, report, th, t));
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
