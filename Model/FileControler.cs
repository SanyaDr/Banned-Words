using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;

namespace Model
{
    public static class FileController
    {
        /// <summary>
        /// Записывает данные в файл
        /// </summary>
        /// <param name="path">Путь для записи</param>
        /// <param name="data">Данные</param>
        /// <param name="append">Добавить или перезаписать</param>
        /// <returns></returns>
        public static bool WriteLineToFile(string path, string data, bool append = false)
        {
            string[] strs = { data };
            return WriteLinesToFile(path, strs, append);
        }
        public static bool WriteLinesToFile(string path, IEnumerable<string> data, bool append = false)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, append))
                {
                    foreach (var line in data)
                    {
                        sw.WriteLine(line);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Считывает слова построчно из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Возвращает массив слов</returns>
        public static string[] ReadWordsInLines(string path)
        {
            List<string> words = new List<string>();
            using(StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    words.Add(sr.ReadLine());
                }
            }
            return words.ToArray();
        }
    }
}
