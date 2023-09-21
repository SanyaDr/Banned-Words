using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Model;

namespace Model
{
    public static class Filtration
    {
        public static object locker = new();
        private static char[] separators = { ',', '.', '!', '?', '(', ')', ' ', '<', '>', '\"', '\'', '|' };
        private static string separatorPattern = "([ !?,.()\'\"|])";
        /// <summary>
        /// Сканирует приложение на наличие запрещенных слов и заменяет их на символы
        /// </summary>
        /// <param name="banWords">Объект с запрещенными словами и заменяемыми символами</param>
        /// <param name="sentence">Предложение которое надо просканировать</param>
        /// <param name="logger">Объект для создания лога</param>
        /// <returns></returns>
        public static string CheckLineForBannedWords(string[] banWords, string symbols, string sentence, Report logger, int strNum, out bool foundWord)
        {
            //3.0

            //Проверка отделения слов от разделителей
            //string test = "У лукоморья, дуб, зеленый! И кот ходит там!";
            //string[] res2 = test.Split(separators, StringSplitOptions.None);
            string[] words = Regex.Split(sentence, separatorPattern);

            int count = 0;
            
            for(int i = 0; i < words.Length; i++)
            {
                
                if (banWords.Contains(words[i].ToLower()))
                {
                    count++;
                    words[i] = symbols;
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach(string word in words)
            {
                sb.Append(word);
            }

            if (count > 0)
            {
                logger.FoundBannedWord(count, strNum);
                foundWord = true;
            }
            else
            {
                foundWord = false;
            }
            return sb.ToString();

        }

        public static void ScanFiles(string[] selectedFiles, string[] bannedWords, string replaceableSymbols, Report logger, ThreadsClass thC, string resPath)
        {
            int countFiles = selectedFiles.Length;
            int lvlForEachFile = 100 / countFiles;
            try
            {
                foreach (var file in selectedFiles)
                {
                    bool banWordFound = false;
                    string fileName = Path.GetFileName(file);
                    logger.PrintStartedScanFile(fileName);
                    File.Copy(file, resPath + "\\" + fileName, true);
                    string newPath = resPath + $"\\БЕЗОПАСНО {fileName}";
                    if(File.Exists(newPath))
                    {
                        File.Delete(newPath);
                    }

                    using (StreamReader sr = new StreamReader(file))
                    {
                        string? line;
                        int strNum = 1;
                        while ((line = sr.ReadLine()) != null && !thC.GetStatus())
                        {

                            //КОСТЫЛЬ
                            //УБЕРИ ЭТО
                            //Добавить в логер в какой строке найдено слово

                            string output = Filtration.CheckLineForBannedWords(bannedWords, replaceableSymbols, line, logger, strNum, out banWordFound);
                            strNum++;
                            new Thread(() =>
                            {
                                lock (locker)
                                {
                                    FileController.WriteLineToFile(newPath, output, true);
                                }
                            }).Start();
                        }
                    }
                    if(!banWordFound)
                    {
                        logger.PrintNoOneBanWordFound();
                    }
                    logger.PrintFinishedScanFile(file, fileName);
                }
            }
            catch (Exception ex)
            {
                logger.AddLineToLog("Произошла ошибка!!");
                logger.AddLineToLog(ex.Message);
            }
        }
    }
}
