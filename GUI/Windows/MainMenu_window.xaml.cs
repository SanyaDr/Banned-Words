using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainMenu
{
    /// <summary>
    /// Логика взаимодействия для MainMenu_window.xaml
    /// </summary>
    public partial class MainMenu_window : Window
    {
        public MainMenu_window()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Как запустить только в одном экземпляре:\nhttps://www.cyberforum.ru/csharp-beginners/thread308706.html");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
