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

        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            th.Kill();
            Close();
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Author_Button_Click(object sender, RoutedEventArgs e)
        {
            //Сделать отдельное окно
        }
    }
}
