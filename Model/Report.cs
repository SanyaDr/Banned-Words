using System;
using System.Collections.Generic;
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

        public void FoundBannedWord(string countFound)
        { }

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
