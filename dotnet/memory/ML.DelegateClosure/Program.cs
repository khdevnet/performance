using System;
using System.Collections.Generic;

namespace ML.DelegateClosure
{
    class Program
    {
        private static Queue<Action> queue = new Queue<Action>();
        static void Main(string[] args)
        {
            for (int i = 0; i < 4000; i++)
            {
                new Samuray(queue).Do();
            }
        }
    }

    class Samuray
    {
        private readonly Queue<Action> _jobQueue;
        private string weapon = "Knife";

        public Samuray(Queue<Action> jobQueue)
        {
            _jobQueue = jobQueue;
        }

        public void Do()
        {
            _jobQueue.Enqueue(() =>
            {
                Console.WriteLine($"Fight with weapon {weapon}");
                // do stuff 
            });
        }
    }
}
