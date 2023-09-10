namespace Model
{
    public class ThreadsClass
    {
        private bool killThreads = false;

        public void Kill()
        {
            killThreads = true;
        }
        public void ResumeThreads()
        {
            killThreads = false;
        }
        public bool GetStatus()
        {
            return killThreads;
        }
    }
}
