namespace Model
{
    /// <summary>
    /// Класс контролирующий завершение всех потоков после закрытия приложения
    /// </summary>
    public class ThreadsClass
    {
        /// <summary>
        /// true - Остановить все потоки; false - работа в штатном режиме
        /// </summary>
        private bool killThreads = false;

        /// <summary>
        /// Остановка потоков
        /// </summary>
        public void Kill()
        {
            killThreads = true;
        }
        /// <summary>
        /// Продолжение работы
        /// </summary>
        public void ResumeThreads()
        {
            killThreads = false;
        }
        /// <summary>
        /// Получение статуса работы
        /// </summary>
        public bool GetStatus()
        {
            return killThreads;
        }
    }
}
