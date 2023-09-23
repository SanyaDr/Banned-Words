using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SelectedFiles
    {
        public readonly string baseFolder = "\\Запрещенные слова";

        public string[] pathsToScan = Array.Empty<string>();
        public string pathToReport = string.Empty;
        public string pathToFolder = string.Empty;

        /// <summary>
        /// Создает папку для сохранения результата. Если существует сохраняет в нее, иначе создаёт новую
        /// </summary>
        /// <param name="path">Путь к папке</param>
        /// <returns>true - сохранение успешно, false - завершено с ошибкой</returns>
        public bool SelectResultDirectory(string path)
        {
            try
            {
                if (path == null || path.Length <= 0)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                while (path.EndsWith(" "))
                {
                    path = path.Remove(path.Length - 1, 1);
                }
                //Если пустой путь то выбираем рабочий стол
                //Если путь кончается на \, то приводим к виду без \ на конце
                if (path.EndsWith('\\'))
                {
                   path = path.Remove(path.Length - 1, 1);
                }
                path += baseFolder;
                if (!Path.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pathToFolder = path;
                //Process.Start("explorer.exe", path);
                return true;
            }
            catch
            {
                //Завершено с ошибкой
                return false;
            }
        }

        public string[] GetPathsToFile()
        {
            return pathsToScan.ToArray();
        }

        public void RestoreToDefault()
        {
            pathToFolder = string.Empty;
            pathToReport = string.Empty;
            pathsToScan = Array.Empty<string>();
        }
    }
}
