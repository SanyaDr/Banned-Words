using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Interop;

using Model;
using System.Windows.Data;
using System.Windows.Controls;

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
        Binding bind = new Binding("LogInfo");

        public ProgressBar_window(ThreadsClass threads, SelectedFiles selectedfiles, BannedWords banWords, Report reporter)
        {
            InitializeComponent();
            th = threads;
            OpenResultFolder_Button.IsEnabled = false;
            selectedFiles = selectedfiles;
            this.banWords = banWords;
            report = reporter;
            Closing += ProgressBar_window_Closing;
            ScanResult_ProgressBar.Value = 0;            
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
                selectedFiles.pathToFolder += baseFolder;
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

        private void addReport_Click(object sender, RoutedEventArgs e)
        {
            report.AddLineToLog("addReport button pressed!");
        }
    }
}
