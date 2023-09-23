using System.Text;
using System.Text.RegularExpressions;

namespace Model
{
    public class Filtration
    {
        public object locker = new();
        //private char[] separators = { ',', '.', '!', '?', '(', ')', ' ', '<', '>', '\"', '\'', '|' };
        private string separatorPattern = "([ !?,.()\'\"|])";
        public bool filtStarted = false;
        public bool isFiltering = true;
        public double filterStatus = 0;    // 0 - 100%
        public int filesToScanLeft = 0;
        /// <summary>
        /// Сканирует приложение на наличие запрещенных слов и заменяет их на символы
        /// </summary>
        /// <param name="banWords">Объект с запрещенными словами и заменяемыми символами</param>
        /// <param name="sentence">Предложение которое надо просканировать</param>
        /// <param name="logger">Объект для создания лога</param>
        /// <returns></returns>
        public string CheckLineForBannedWords(string[] banWords, string symbols, string sentence, Report logger, int strNum, out bool foundWord)
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

        public void ScanFiles(string[] selectedFiles, string[] bannedWords, string replaceableSymbols, Report logger, ThreadsClass thC, string resPath)
        {
           
            filesToScanLeft = selectedFiles.Length;
            filtStarted = true;
            isFiltering = true;
            filterStatus = 0;

            try
            {
                foreach (var file in selectedFiles)
                {
                    int lvlForEachFile = 100 / selectedFiles.Length;
                    int countlines = 0;
                    Thread.Sleep(150);
                    using (StreamReader sr = new StreamReader(file))
                    {
                        while (sr.ReadLine() != null && !thC.GetStatus())
                        {
                            countlines++;
                        }
                    }
                    lvlForEachFile /= countlines;

                    if (thC.GetStatus())
                    {
                        return;
                    }
                    bool banWordFound = false;
                    string fileName = Path.GetFileName(file);
                    logger.PrintStartedScanFile(fileName);
                    
                    if(Path.Exists(resPath))
                    {
                        File.Copy(file, resPath + "\\" + fileName, true);

                    }

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
                            string output = CheckLineForBannedWords(bannedWords, replaceableSymbols, line, logger, strNum, out banWordFound);
                            strNum++;
                            new Thread(() =>
                            {
                                lock (locker)
                                {
                                    FileController.WriteLineToFile(newPath, output, true);
                                }
                            }).Start();
                            filterStatus += lvlForEachFile;
                        }
                    }
                    filesToScanLeft--;
                    if(!banWordFound)
                    {
                        logger.PrintNoOneBanWordFound();
                    }
                    logger.PrintFinishedScanFile(file, fileName);
                }
                filterStatus = 100;
                isFiltering = false;
                filtStarted = false;
            }
            catch (Exception ex)
            {
                logger.AddLineToLog("Произошла ошибка!!");
                logger.AddLineToLog(ex.Message);
            }
        }
    }
}
