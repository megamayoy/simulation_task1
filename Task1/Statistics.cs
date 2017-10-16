using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    class Statistics
    {
        public int TotalRunTime;
        public List<int> QueueLength = new List<int>();
        public float ProbabilityOfCustomerWait;
        public int NoOfCustomersWait;
        public float AverageWaitTime = 0;
        public float TotalWaitingTime = 0;
        public int MaximumQueueLength;
        public int TotalCustomers;
        public int NoOfServers;

        public void InitializeQueueLength(int Length)
        {
            for (int i = 0; i < Length; i++)
            {
                QueueLength.Add(0);
            }
        }

        public void AppendNewUnitTimes(int Units)
        {
            for (int i = QueueLength.Count; i < QueueLength.Count + Units; i++)
            {
                QueueLength.Add(0);
            }
        }

    }
}
