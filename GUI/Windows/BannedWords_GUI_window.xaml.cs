using Microsoft.Win32;
using Model;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace GUI.Windows
{
    /// <summary>
    /// Логика взаимодействия для BannedWords_GUI_window.xaml
    /// </summary>
    public partial class BannedWords_GUI_window : Window
    {
        private SelectedFiles selectedFiles = new SelectedFiles();
        private Report reporter = new Report();
        private ThreadsClass th;
        private BannedWords bannedWords;
        public BannedWords_GUI_window(ThreadsClass threads)
        {
            InitializeComponent();
            bannedWords = new BannedWords();
            th = threads;
            Closing += BannedWords_GUI_window_Closing;
        }

        private void BannedWords_GUI_window_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            th.Kill();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Текстовый файл|*.txt";
            ofd.Multiselect = true;
            ofd.ShowDialog();
            selectedFiles.pathsToScan = ofd.FileNames;
            CountSelectedFiles_Label.Content = selectedFiles.pathsToScan.Length;
        }

        private void SaveReport(object sender, RoutedEventArgs e)
        {
            if(selectedFiles.pathToReport.Length <= 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Текстовый документ|*.txt";
                sfd.FileName = $"Отчёт о замене слов {DateTime.Now.ToShortDateString()}";
                sfd.ShowDialog();
                selectedFiles.pathToReport = sfd.FileName;
            }
            reporter.PrintReport(selectedFiles.pathToReport);
            MessageBox.Show("Отчёт сохранен успешно!\nПуть к файлу: " + selectedFiles.pathToReport);
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void OpenBannedWordWindow(object sender, RoutedEventArgs e)
        {
            // Не настройки а окно запрещенных слов
            //Settings_GUI_window settings = new Settings_GUI_window();
            //settings.ShowDialog();
        }
        private void OpenReplaceableSymbols(object sender, RoutedEventArgs e)
        {

        }

        private void BeginBan_Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedFiles.pathsToScan.Length <= 0)
            {
                OpenFile(sender, e);
                if (selectedFiles.pathsToScan.Length <= 0)
                {
                    MessageBox.Show("Вы не выбрали файлы для сканирования");
                    return;
                }
            }
            ProgressBar_window scanWindow = new ProgressBar_window(th, selectedFiles, bannedWords, reporter);
            Hide();
            scanWindow.ShowDialog();
            Show();

        }
    }
}
