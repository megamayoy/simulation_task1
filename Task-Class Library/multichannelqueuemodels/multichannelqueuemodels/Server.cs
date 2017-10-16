using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChannelQueueModels
{
    public class Server
    {
        public Server()
        {
            ServiceTimeDistribution = new List<TimeDistribution>();
        }
        public int ServerId { get; set; }

        public string Name { get; set; }

        public double ServiceEfficiency { get; set; }

        public bool Status { get; set; }

        public int ServerEndTime {get; set;}

        public List<TimeDistribution> ServiceTimeDistribution { get; set; }

        public int TotalServiceTime { get; set; }

        public int TotalNumberOfCustomers { get; set; }
    }
}
