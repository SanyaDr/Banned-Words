using GUI.Windows;
using Model;
using System.Windows;

namespace MainMenu
{
    /// <summary>
    /// Логика взаимодействия для MainMenu_window.xaml
    /// </summary>
    public partial class MainMenu_window : Window
    {
        public ThreadsClass th;
        public SelectedFiles selected;
        public BannedWords banned;
        public MainMenu_window()
        {
            InitializeComponent();
            th = new ThreadsClass();
            selected = new SelectedFiles();
            banned = new BannedWords();
            Closing += MainMenu_window_Closing;
        }

        private void MainMenu_window_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            th.Kill();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Как запустить только в одном экземпляре:\nhttps://www.cyberforum.ru/csharp-beginners/thread308706.html");
        }

        private void StartApp_Button_Click(object sender, RoutedEventArgs e)
        {
            BannedWords_GUI_window mainWindow = new BannedWords_GUI_window(th, selected, banned);
            mainWindow.ShowDialog();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            th.Kill();
            Close();
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings_GUI_window settings = new Settings_GUI_window(selected, banned);
            settings.ShowDialog();
        }

        private void Author_Button_Click(object sender, RoutedEventArgs e)
        {
            //Сделать отдельное окно
        }
    }
}
