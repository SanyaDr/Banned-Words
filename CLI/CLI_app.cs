//CLI интерфейс для Banned Words

using Model;
using System.Diagnostics;

SelectedFiles selected = new SelectedFiles();   //Класс выбранных файлов
Report logger = new Report();                   //Класс куда записывается отчет о работе программы
BannedWords bannedWords = new BannedWords();
ThreadsClass threadClass = new ThreadsClass();
bool started = true;

void prog()
{
    Console.WriteLine("Введите путь к файлу:");
    {
        bool succes = false;
        string pathToFile = string.Empty;
        while (!succes)
        {
            pathToFile = Console.ReadLine();
            if (File.Exists(pathToFile))
            {
                succes = true;
            }
            else
            {
                Console.WriteLine("Ошибка пути! Проверьте путь");
            }
        }
        {
            string[] strs = { pathToFile };
            selected.pathsToScan = strs;
        }
    }
    Console.WriteLine("Принято!");

    Console.WriteLine("Введите путь для сохранения результата:");
    {
        bool succes = false;
        while (!succes)
        {
            succes = selected.SelectResultDirectory(Console.ReadLine());
            
            if (!succes)
            {
                Console.WriteLine("Ошибка пути! Проверьте путь");
            }
        }
    }
    Console.WriteLine("Принято!");

    Filtration filter = new Filtration();
    filter.ScanFiles(selected.pathsToScan, bannedWords.GetBannedWords(), "*******", logger, threadClass, selected.pathToFolder);

    logger.PrintReport(selected.pathToFolder, logger.GetLog());

    if (started)
    {
        Process.Start("explorer.exe", selected.pathToFolder);
        started = false;
    }

}


Console.WriteLine("Введите запрещенные слова. Каждое запрещенное слово с новой строки.\nВведите 0 для прекращения записи");
string inp;
do
{
    inp = Console.ReadLine();
    if (inp != "0")
    {
        bannedWords.AppendNewBanWord(inp);
    }
}
while (inp != "0" || bannedWords.GetBannedWords().Length <= 0);

while (true)
{
    threadClass.ResumeThreads();
    prog();
    Console.WriteLine("Запустить программу еще раз? (y/n)");
    if(Console.ReadLine() == "n")
    {
        break;
    }
}

Thread.Sleep(1000);
Console.WriteLine("Работа завершена");
Thread.Sleep(1000);