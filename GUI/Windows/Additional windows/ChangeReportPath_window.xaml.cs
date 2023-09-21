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
            FolderPicker fp = new FolderPicker();
            fp.Title = "Выберите папку для сохранения результатов";
            fp.Multiselect = false;
            if (fp.ShowDialog() == true)
            {
                NewPathReport_TextBox.Text = fp.ResultPath;
            }
            else
            {
                NewPathReport_TextBox.Clear();
            }
        }

        private void SaveNewPath_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Path.GetDirectoryName(NewPathReport_TextBox.Text)))
            {
                selectedFiles.pathToReport = NewPathReport_TextBox.Text;
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка сохранения: Путь не существует!", "Ошибка выбора пути сохранения",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
