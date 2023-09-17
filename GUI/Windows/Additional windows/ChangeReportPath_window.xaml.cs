using Microsoft.Win32;
using Model;
using System;
using System.IO;
using System.Windows;

namespace GUI.Windows.Additional_windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeReportPath_window.xaml
    /// </summary>
    public partial class ChangeReportPath_window : Window
    {
        private SelectedFiles selectedFiles;
        private string newPath = string.Empty;
        public ChangeReportPath_window(SelectedFiles selectedFiles)
        {
            InitializeComponent();
            this.selectedFiles = selectedFiles;
            if(selectedFiles.pathToReport.Length > 0 )
            {
                oldPath_Label.Text = selectedFiles.pathToReport;
            }
        }

        private void OpenSaveFileDialog_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовый файл|*.txt";
            sfd.FileName = $"Отчёт о замене слов {DateTime.Now.ToShortDateString()}";
            sfd.ShowDialog();
            newPath = sfd.FileName;
        }

        private void SaveNewPath_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Path.GetDirectoryName(newPath)))
            {
                selectedFiles.pathToReport = newPath;
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
