using Model;
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

namespace GUI.Windows.Additional_windows
{
    /// <summary>
    /// Окно изменения заменяемых символов взаместо запрет слова
    /// </summary>
    public partial class ChangeChangeableSymbols_Window : Window
    {
        BannedWords banw;
        public ChangeChangeableSymbols_Window(BannedWords bannedWords)
        {
            InitializeComponent();
            banw = bannedWords;
            old_Label.Text = banw.ReplaceSymbol.ToString();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (New_TextBox.Text.Length == 1)
                {
                    banw.ReplaceSymbol = New_TextBox.Text.ToCharArray()[0];
                }
                else
                {
                    MessageBox.Show("Введите только один символ!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
