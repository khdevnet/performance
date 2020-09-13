using System;

namespace ML.EventSubscriptions
{
    class Program
    {
        static void Main(string[] args)
        {
            var captain = new SamurayCaptain();
            MemoryLeakVersion(captain);
            //FixedVersion(captain);
        }

        private static void FixedVersion(SamurayCaptain captain)
        {
            for (int i = 0; i < 20000; i++)
            {
                using (var s = new Samuray(captain, "Samuray_" + i))
                {

                }
            }
        }

        private static void MemoryLeakVersion(SamurayCaptain captain)
        {
            for (int i = 0; i < 20000; i++)
            {
                var s = new Samuray(captain, "Samuray_" + i);
            }
        }
    }

    public class Samuray : IDisposable
    {
        private readonly SamurayCaptain samurayCaptain;
        private readonly string name;
        public Samuray(SamurayCaptain samurayCaptain, string name)
        {
            samurayCaptain.CommandGived += OnCommandGived;
            this.samurayCaptain = samurayCaptain;
            this.name = name;
        }

        public void Dispose()
        {
            samurayCaptain.CommandGived -= OnCommandGived;
        }

        private void OnCommandGived(object sender, SamurayActionEventArgs e)
        {
            Console.WriteLine(name + " begin " + e.Action);
        }
    }

    public class SamurayCaptain
    {
        public event EventHandler<SamurayActionEventArgs> CommandGived;

        public void GiveCommand(string action)
        {
            SamurayActionEventArgs args = new SamurayActionEventArgs();
            args.Action = action;
            args.TimeReached = DateTime.Now;
            OnCommandGived(args);
        }

        protected virtual void OnCommandGived(SamurayActionEventArgs e)
        {
            EventHandler<SamurayActionEventArgs> handler = CommandGived;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }


    public class SamurayActionEventArgs : EventArgs
    {
        public string Action { get; set; }
        public DateTime TimeReached { get; set; }
    }
}
