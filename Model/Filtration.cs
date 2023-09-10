using Model;

namespace Model
{
    public static class Filtration
    {
        public static object locker = new();
        /// <summary>
        /// Сканирует приложение на наличие запрещенных слов и заменяет их на символы
        /// </summary>
        /// <param name="banWords">Объект с запрещенными словами и заменяемыми символами</param>
        /// <param name="sentence">Предложение которое надо просканировать</param>
        /// <param name="logger">Объект для создания лога</param>
        /// <returns></returns>
        public static string CheckLineForBannedWords(BannedWords banWords, string sentence, Report logger, ThreadsClass th)
        {
            string[] words = sentence.Split(' ');
            for(int i = 0; i < words.Length && !th.GetStatus(); i++)
            {
                if (banWords.GetBannedWords().Contains(words[i].ToLower()))
                {
                    words[i] = string.Empty;
                    for (int j = 0; j < banWords.countSymbols; j++)
                    {
                        words[i] += banWords.ReplaceSymbol.ToString();
                    }
                }
            }
            string res = string.Empty;
            for(int i = 0; i < words.Length; i++)
            {
                res += words[i];
                if(i != words.Length-1)
                {
                    res += " ";
                }
            }
            return res;
        }

        public static async void ScanFiles(SelectedFiles selF, BannedWords banW, Report logger, ThreadsClass thC, string resPath)
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
                        while ((line = sr.ReadLine()) != null && !thC.GetStatus())
                        {

                            //КОСТЫЛЬ
                            //УБЕРИ ЭТО
                            //Добавить в логер в какой строке найдено слово

                            string output = Filtration.CheckLineForBannedWords(banW, line, logger, thC);
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
