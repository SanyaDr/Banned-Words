using GUI.Windows;
using Microsoft.Win32;
using System.Windows;
using ViewModel;

namespace MainMenu
{
    /// <summary>
    /// Логика взаимодействия для MainMenu_window.xaml
    /// </summary>
    public partial class MainMenu_window : Window
    {
        public ThreadsClass th;

        public MainMenu_window()
        {
            InitializeComponent();
            th = new ThreadsClass();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Как запустить только в одном экземпляре:\nhttps://www.cyberforum.ru/csharp-beginners/thread308706.html");
        }

        private void StartApp_Button_Click(object sender, RoutedEventArgs e)
        {
            th.ResumeThreads();
            BannedWords_GUI_window mainWindow = new BannedWords_GUI_window();
            Hide();
            mainWindow.ShowDialog();
            Show();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            th.Kill();
            Close();
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings_GUI_window settings = new Settings_GUI_window();
            Hide();
            settings.ShowDialog();
            Show();
        }

        private void Author_Button_Click(object sender, RoutedEventArgs e)
        {
            //Сделать отдельное окно
        }
    }
}
