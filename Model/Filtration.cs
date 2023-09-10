using ViewModel;

namespace Model
{
    public static class Filtration
    {
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
                    for (int j = 0; j < banWords.countSymbols; j++)
                    {
                        words[i] += banWords.ReplaceSymbol.ToString();
                    }
                }
            }
            return words.ToString();
        }

        public static async void ScanFiles(SelectedFiles selF, BannedWords banW, Report logger, ThreadsClass thC, string resPath)
        {
            try
            {
                foreach (var file in selF.pathsToScan)
                {
                    List<string> output = new List<string>();
                    string fileName = Path.GetFileName(file);
                    File.Copy(file, resPath, true);
                    string newPath = Path.GetDirectoryName(file) + $"БЕЗОПАСНО {fileName}";

                    using (StreamReader sr = new StreamReader(file))
                    {
                        string? line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (thC.GetStatus())
                            {
                                return;
                            }

                            await File.WriteAllLinesAsync(newPath, output);
                            //output.Add(Filtration.CheckLineForBannedWords(banW, line, logger, thC));

                        }
                    }
                    //using (StreamWriter sw = new StreamWriter(newPath, false))
                    //{
                    //    foreach (var line in output)
                    //    {
                    //        if (thC.GetStatus())
                    //        {
                    //            return;
                    //        }
                    //        sw.WriteLine(line);
                    //    }
                    //}

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
