using GUI.Windows;
using Model;
using System.Threading;
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
            App_Startup();
            InitializeComponent();
            th = new ThreadsClass();
            selected = new SelectedFiles();
            banned = new BannedWords();
            Closing += MainMenu_window_Closing;
        }

        Mutex mutex;

        private void App_Startup()
        {
            bool isCreated;
            string appName = "BannedWordsApplication";
            mutex = new Mutex(true, appName, out isCreated);
            if (!isCreated)
            {
                MessageBox.Show("Приложение уже запущено!");
                Application.Current.Shutdown();
            }
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

        private void Author_Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Выполнил: Дровосеков Александр\n      2023\n\n tg: @SanyaDr");
        }
    }
}
