using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Model;

namespace GUI.Windows.Additional_windows
{
    /// <summary>
    /// Логика взаимодействия для BannedWordSettings_Window.xaml
    /// </summary>
    public partial class BannedWordSettings_Window : Window
    {
        BannedWords banned;
     
        public BannedWordSettings_Window(BannedWords banned)
        {
            InitializeComponent();
            this.banned = banned;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveNewPath_Button_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Path_TextBox.Text))
            {
                string[] inp = FileController.ReadWordsInLines(Path_TextBox.Text);
                if (inp.Length > 0)
                {
                    banned.bannedWords = inp;
                    MessageBox.Show("Запрещенные слова успешно добавлены!\n\n" + $"Найдено: {inp.Length} слов.", "Успешно");
                    Close(); 
                }
                else
                {
                    MessageBox.Show("Добавленный вами файл пуст, или слова не найдены", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Путь не существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenSaveFileDialog_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Текстовый документ|*.txt";
            ofd.Multiselect = false;
            ofd.Title = "Выберите файл для сканирования запрещенных слов";
            ofd.ShowDialog();
            Path_TextBox.Text = ofd.FileName;
        }
    }
}
