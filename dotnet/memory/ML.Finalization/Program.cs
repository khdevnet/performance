using System.Threading;

namespace ML.Finalization
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                File2 f = new File2();
            }
        }
    }

    class File2
    {
        public File2()
        {
            Thread.Sleep(10);
        }
        ~File2()
        {
            Thread.Sleep(20);
        }

        private byte[] data = new byte[1024];
    }
}
