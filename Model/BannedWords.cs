using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Model
{
    /// <summary>
    /// Класс хранящий запрещенные слова
    /// </summary>
    public class BannedWords
    {
        /// <summary>
        /// Указывает кол-во символов, на которое запрещенное слово будет заменятся
        /// </summary>
        public int countSymbols = 7;
        /// <summary>
        /// Какие слова запрещены
        /// </summary>
        public string[] bannedWords = { "фак", "fuck" };
        /// <summary>
        /// На какой символ заменяется слово
        /// </summary>
        public char ReplaceSymbol = '*';

        /// <summary>
        /// Получение слов
        /// </summary>
        public string[] GetBannedWords()
        {
            return bannedWords;
        }
        /// <summary>
        /// Добавить новое запрещенное слово
        /// </summary>
        /// <param name="banWord">Слово, без знаков пунктуации и пр.</param>
        public void AppendNewBanWord(string banWord)
        {
            if (!bannedWords.Contains(banWord))
            {
                bannedWords = bannedWords.Append(banWord.ToLower()).ToArray();
            }
        }
        /// <summary>
        /// Получение строки, чем заменить запрещенное слово
        /// </summary>
        /// <returns></returns>
        public string GetReplaceString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < countSymbols; i++)
            {
                sb.Append(ReplaceSymbol);
            }
            return sb.ToString();
        }
        ///// <summary>
        ///// Загружает запрещенные слова из файла
        ///// </summary>
        ///// <param name="path">Путь к файлу</param>
        ///// <returns>true - загрузка успешна. false - загрузка завершена с ошибками</returns>
        //public bool LoadBanWordsFromFile(string path)
        //{
        //    if (Path.Exists(path))
        //    {

        //    }
        //    return false;
        //}
        ///// <summary>
        ///// Сохраняет запрещенные слова в файл
        ///// </summary>
        ///// <param name="path">Путь к файлу</param>
        ///// <returns>true - загрузка успешна. false - загрузка завершена с ошибками</returns>
        //public bool SaveBanWordsIntoFile(string path)
        //{
        //    return false;
        //}

        /// <summary>
        /// Восстановление значений по умолчанию
        /// </summary>
        public void RestoreToDefault()
        {
            countSymbols = 7;
            ReplaceSymbol = '*';
        }
    }
}
