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
        BannedWords banned;
        public Settings_GUI_window(SelectedFiles selected, BannedWords bannedWords)
        {
            InitializeComponent();
            selectedFiles = selected;
            banned = bannedWords;
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

        private void AddBanWords_Button_Click(object sender, RoutedEventArgs e)
        {
            BannedWordSettings_Window bannWSett = new BannedWordSettings_Window(banned);
            Hide();
            bannWSett.ShowDialog();
            Show();
            Close();
        }

        private void ChangeChangeableSymbols_Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeChangeableSymbols_Window change = new ChangeChangeableSymbols_Window(banned);
            Hide();
            change.ShowDialog();
            Show();
            Close();
        }
    }
}
