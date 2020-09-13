using System;
using System.Collections.Generic;

namespace ML.StaticList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            for (int i = 0; i < 10000; i++)
            {
                new Samuray();
            }
            Console.WriteLine("Finish");
        }
    }

    class Samuray
    {
        private static readonly List<Samuray> Instances = new List<Samuray>();
        public Samuray()
        {
            Instances.Add(this);
        }
    }
}
