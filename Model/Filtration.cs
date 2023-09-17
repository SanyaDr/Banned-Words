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
        public static string CheckLineForBannedWords(BannedWords banWords, string sentence, Report logger, int strNum)
        {
            //3.0

            //Проверка отделения слов от разделителей
            //string test = "У лукоморья, дуб, зеленый! И кот ходит там!";
            //string[] res2 = test.Split(separators, StringSplitOptions.None);
            string[] words = Regex.Split(sentence, separatorPattern);

            int count = 0;
            
            for(int i = 0; i < words.Length; i++)
            {
                
                if (banWords.GetBannedWords().Contains(words[i].ToLower()))
                {
                    count++;
                    words[i] = banWords.GetReplaceString();
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
            }
            return sb.ToString();
            /*
             * 1.0
            //string[] words = sentence.Split(separators);

            //for(int i = 0; i < words.Length && !th.GetStatus(); i++)
            //{
            //    logger.FoundBannedWord(1, strNum);
            //    if (banWords.GetBannedWords().Contains(words[i].ToLower()))
            //    {
            //        words[i] = string.Empty;
            //        for (int j = 0; j < banWords.countSymbols; j++)
            //        {
            //            words[i] += banWords.ReplaceSymbol.ToString();
            //        }
            //    }
            //}
            //string res = string.Empty;
            //for(int i = 0; i < words.Length; i++)
            //{
            //    res += words[i];
            //    if(i != words.Length-1)
            //    {
            //        res += " ";
            //    }
            //}
            */

            /*
             * 2.0
                        int count = 0;
            foreach(var ban in  banWords.GetBannedWords())
            {
                if (sentence.ToLower().Contains(ban))
                {
                    count++;
                    //int index = sentence.IndexOf(ban);
                    sentence = sentence.ToLower().Replace(ban, banWords.GetReplaceString());
                }
            }

            if (count > 0)
            {
                logger.FoundBannedWord(count, strNum);
            }
            return sentence;
             */

        }

        public static void ScanFiles(SelectedFiles selF, BannedWords banW, Report logger, ThreadsClass thC, string resPath)
        {
            try
            {
                foreach (var file in selF.pathsToScan)
                {
                    string fileName = Path.GetFileName(file);
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

                            string output = Filtration.CheckLineForBannedWords(banW, line, logger, strNum);
                            strNum++;
                            new Thread(() =>
                            {
                                lock (locker)
                                {
                                    using (StreamWriter sw = new StreamWriter(newPath, true))
                                    {
                                        sw.WriteLine(output);
                                    }
                                }
                            }).Start();
                        }
                    }
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
