using GUI.Windows.Additional_windows;
using Model;
using System.Windows;

namespace GUI.Windows
{
    /// <summary>
    /// Логика взаимодействия для Settings_GUI_window.xaml
    /// </summary>
    public partial class Settings_GUI_window : Window
    {
        SelectedFiles selectedFiles;
        public Settings_GUI_window(SelectedFiles selected)
        {
            InitializeComponent();
            selectedFiles = selected;
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangePathForReport_Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeReportPath_window changePathWindow = new ChangeReportPath_window(selectedFiles);
            Hide();
            changePathWindow.ShowDialog();
            Show();
            Close();
        }
    }
}
