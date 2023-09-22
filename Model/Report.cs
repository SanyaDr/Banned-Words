using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Model
{
    public class Report
    {
        public List<string> log = new List<string> { };
        public List<string> lastProg = new List<string>();

        public void AddLineToLog(string data)
        {
            log.Add(data + "\n");
            lastProg.Add(data + "\n");
        }

        public void PrintStartedScanFile(string fileName)
        {
            AddLineToLog($"Началось сканирование файла: \"{fileName}\"");
        }

        public void PrintFinishedScanFile(string oldFilePath, string fileName)
        {
            AddLineToLog($"Завершена проверка файла \"{fileName}\"! ->\nЕго изначальный путь:\"{oldFilePath}\"\n");
        }

        public List<string> GetLog()
        {
            return log;
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

        public void PrintReport(string path, IEnumerable<string> log)
        {
            AddLineToLog("Отчёт работы программы \"Запрещенные слова\"");
            AddLineToLog("Создан: " + DateTime.Now);
            AddLineToLog("Дровосеков Александр, 2023");
            AddLineToLog("---------------------------------------------\n");

            FileController.WriteLinesToFile(path, log);
            ClearLog();
            ClearLastLog();
        }

        public void PrintNoOneBanWordFound()
        {
            AddLineToLog("Не найдено ни одного запрещенного слова!");
        }

        public void ClearLog()
        {
            log.Clear();
        }

        public void ClearLastLog()
        {
            lastProg.Clear();
        }
    }
}
