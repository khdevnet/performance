using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DataProvider.Web.Services
{
    public class DataInfrastructure
    {
        private readonly Random random = new Random();
        private readonly IReadOnlyCollection<int> DeadLockChance = new ReadOnlyCollection<int>(Enumerable.Range(1, 8).ToList());
        public Task<int> GetDataAsync()
        {
            var value = random.Next(1, 10);
            if (DeadLockChance.Contains(value))
            {
                throw new DeadLockException();
            }

            return Task.FromResult(value);
        }
    }
}
