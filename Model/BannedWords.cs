using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Model
{
    public class BannedWords
    {
        /// <summary>
        /// Указывает кол-во символов, на которое запрещенное слово будет заменятся
        /// </summary>
        public int countSymbols = 7;
        /// <summary>
        /// Какие слова запрещены
        /// </summary>
        private string[] bannedWords = { "фак", "fuck" };
        /// <summary>
        /// На какой символ заменяется слово
        /// </summary>
        public char ReplaceSymbol = '*';

        public string[] GetBannedWords()
        {
            return bannedWords;
        }
        public void AppendNewBanWord(string banWord)
        {
            bannedWords.Append(banWord.ToLower());
        }
        public string GetReplaceString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < countSymbols; i++)
            {
                sb.Append(ReplaceSymbol);
            }
            return sb.ToString();
        }
    }
}
