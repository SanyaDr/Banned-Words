using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Report
    {
        public List<string> log = new List<string> { };
        public void AddLineToLog(string data)
        {
            log.Add(data + "\n");
        }

        public void PrintStartedScanFile(string fileName)
        { }

        public void PrintFinishedScanFile(string oldFilePath, string fileName)
        {
            AddLineToLog($"Завершена проверка файла {fileName}! ->\nЕго изначальный путь:{oldFilePath}");
        }

        /// <summary>
        /// Добавление в отчёт уведомления о том, что найдено запрещенное слово
        /// </summary>
        /// <param name="count">Количество запрещенных слов в предложении</param>
        /// <param name="strNum">Номер строки</param>
        public void FoundBannedWord(int count, int strNum)
        {
            AddLineToLog($"Найдено запрещенных слов: {count}, в строке {strNum}!");
        }

        public void PrintReport(string path)
        {
            using(StreamWriter sw = new StreamWriter(path, false))
            {
                sw.WriteLine("Отчёт работы программы \"Запрещенные слова\"");
                sw.WriteLine("Создан: " + DateTime.Now);
                sw.WriteLine("Дровосеков Александр, 2023");
                sw.WriteLine("---------------------------------------------\n");
                
                foreach(var line in log)
                {
                    sw.WriteLine(line);
                }
            }
        }

    }
}
