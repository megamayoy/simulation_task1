using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiChannelQueueModels;

namespace Task1
{
    class SimulationSystem
    {
        List<Server> Servers;
        List<SimualtionCase> Customers;
        int NoServers;
        List<int> FreeServers = new List<int>();
        Statistics SystemStatistics = new Statistics();

        public void CompleteServiceTimeDistributionData(int NoOfServers, int NoOfRows, List<Server> SystemServers)
        {
            NoServers = NoOfServers;
            Servers = SystemServers;
            int Cols = 0;
            for (int i = 0; i < NoServers; i++)
            {
                Servers[i].ServerId = i;
                double CummProbability = 0.0;
                for (int j = 0; j < Servers[i].ServiceTimeDistribution.Count; j++)
                {
                    if (j == 0)
                    {
                        CummProbability = Servers[i].ServiceTimeDistribution[j].Probability;
                        Servers[i].ServiceTimeDistribution[j].MinRange = 1;
                    }
                    else
                    {
                        CummProbability += Servers[i].ServiceTimeDistribution[j].Probability;
                        Servers[i].ServiceTimeDistribution[j].MinRange = Servers[i].ServiceTimeDistribution[j - 1].MaxRange + 1;
                    }

                    Servers[i].ServiceTimeDistribution[j].CummProbability = CummProbability;
                    Servers[i].ServiceTimeDistribution[j].MaxRange = Math.Floor(Servers[i].ServiceTimeDistribution[j].CummProbability * 100.0f);
                    Servers[i].Status = false;
                    Servers[i].ServerEndTime = 0;
                    Servers[i].TotalNumberOfCustomers = 0;
                    Servers[i].TotalServiceTime = 0;
                }
                Cols += 2;
            }

        }

        private int GetServiceTimeFromRange(Server CurrenentServer, SimualtionCase CurrentCustomer)
        {
            int LoopCount = CurrenentServer.ServiceTimeDistribution.Count;
            for (int i = 0; i < LoopCount; i++)
            {
                if (CurrentCustomer.RandomServiceTime >= CurrenentServer.ServiceTimeDistribution[i].MinRange &&
                               CurrentCustomer.RandomServiceTime <= CurrenentServer.ServiceTimeDistribution[i].MaxRange)
                {
                    return CurrenentServer.ServiceTimeDistribution[i].Time;
                }
            }
            return -1;
        }

        private int GetInterarrivalTimeFromRange(Server CurrenentServer, SimualtionCase CurrentCustomer)
        {
            int LoopCount = CurrenentServer.ServiceTimeDistribution.Count;
            for (int i = 0; i < LoopCount; i++)
            {
                if (CurrentCustomer.RandomInterarrivalTime >= CurrenentServer.ServiceTimeDistribution[i].MinRange &&
                               CurrentCustomer.RandomInterarrivalTime <= CurrenentServer.ServiceTimeDistribution[i].MaxRange)
                {
                    return CurrenentServer.ServiceTimeDistribution[i].Time;
                }
            }
            return -1;
        }

        private void FillFreeServers(int Time)
        {
            FreeServers.Clear();
            for (int i = 1; i < NoServers; i++)
            {
                if (Servers[i].ServerEndTime <= Time)
                    FreeServers.Add(i);
            }
        }

        public List<SimualtionCase> Simulate(Enums.ServerSelectionMethod SelectionMethod, Enums.ServerStoppingCondition StoppingCondition, string TextBoxData)
        {
            Random Rnd = new Random();
            Customers = new List<SimualtionCase>(); //each selection method and stopping condition need a list of customers
            SystemStatistics.NoOfCustomersWait = 0;
            if (SelectionMethod == Enums.ServerSelectionMethod.HighestPriority && StoppingCondition == Enums.ServerStoppingCondition.NumberOfCustomers)
            {
                int NoOfCustomers = Int32.Parse(TextBoxData.ToString());
                Customers = new List<SimualtionCase>(NoOfCustomers);
                for (int i = 0; i < NoOfCustomers; i++)
                {
                    SimualtionCase NewCustomer = new SimualtionCase();
                    Customers.Add(NewCustomer);
                    Customers[i].CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        Customers[i].RandomInterarrivalTime = Rnd.Next(1, 101);
                        Customers[i].InterarrivalTime = this.GetInterarrivalTimeFromRange(Servers[0], Customers[i]);
                        if (Customers[i].RandomInterarrivalTime == 100)
                            Customers[i].RandomInterarrivalTime = 0;

                        SystemStatistics.AppendNewUnitTimes(Customers[i].InterarrivalTime);
                        Customers[i].ArrivalTime = Customers[i - 1].ArrivalTime + Customers[i].InterarrivalTime;
                    }
                    else
                    {
                        Customers[i].RandomInterarrivalTime = -1;
                        Customers[i].InterarrivalTime = -1;
                        Customers[i].ArrivalTime = 0;
                        SystemStatistics.AppendNewUnitTimes(1);
                    }
                    Customers[i].RandomServiceTime = Rnd.Next(1, 101);
                    Server FirstIdle = new Server();
                    FirstIdle.ServerEndTime = 100000;
                    int FirstIdleID = -1;
                    for (int q = 1; q < NoServers; q++)
                    {   //get the first idle server in order
                        if (Servers[q].ServerEndTime <= Customers[i].ArrivalTime)
                        {
                            FirstIdle = Servers[q];
                            FirstIdleID = q;
                            break;
                        }
                        //if that current server end time is greater that customers arrival time, pick the one that will finish early
                        else if (Servers[q].ServerEndTime < FirstIdle.ServerEndTime)
                        {
                            FirstIdle = Servers[q];
                            FirstIdleID = q;
                        }
                    }
                    // if all servers are busy and customer has to wait then calculate wait time
                    if (Servers[FirstIdleID].ServerEndTime > Customers[i].ArrivalTime)
                    {
                        Customers[i].WaitingTime = (Servers[FirstIdleID].ServerEndTime - Customers[i].ArrivalTime);
                        SystemStatistics.NoOfCustomersWait++;
                        SystemStatistics.TotalWaitingTime += Customers[i].WaitingTime;
                        for (int x = Customers[i].ArrivalTime; x < Servers[FirstIdleID].ServerEndTime; x++)
                            SystemStatistics.QueueLength[x]++;
                    }
                    Customers[i].AssignedServer = Servers[FirstIdleID];
                    Customers[i].TimeServiceBegins = Customers[i].ArrivalTime + Customers[i].WaitingTime;
                    Customers[i].ServiceTime = this.GetServiceTimeFromRange(Customers[i].AssignedServer, Customers[i]);
                    Customers[i].TimeServiceEnds = Customers[i].TimeServiceBegins + Customers[i].ServiceTime;
                    Servers[FirstIdleID].TotalServiceTime += Customers[i].ServiceTime;
                    Servers[FirstIdleID].TotalNumberOfCustomers++;
                    Servers[FirstIdleID].ServerEndTime = Customers[i].TimeServiceEnds;
                    if (Customers[i].RandomServiceTime == 100)
                        Customers[i].RandomServiceTime = 0;
                }
                return Customers;
            }

            else if (SelectionMethod == Enums.ServerSelectionMethod.HighestPriority && StoppingCondition == Enums.ServerStoppingCondition.SimulationEndTime)
            {
                int SimulationEndTime = Int32.Parse(TextBoxData), i = 0;
                SystemStatistics.InitializeQueueLength(SimulationEndTime);
                while (true)
                {
                    SimualtionCase CurrentCustomer = new SimualtionCase();
                    CurrentCustomer.CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        CurrentCustomer.RandomInterarrivalTime = Rnd.Next(1, 101);
                        CurrentCustomer.InterarrivalTime = this.GetInterarrivalTimeFromRange(Servers[0], CurrentCustomer);
                        CurrentCustomer.ArrivalTime = Customers[i - 1].ArrivalTime + CurrentCustomer.InterarrivalTime;
                        if (CurrentCustomer.RandomInterarrivalTime == 100)
                            CurrentCustomer.RandomInterarrivalTime = 0;
                    }
                    else
                    {
                        CurrentCustomer.RandomInterarrivalTime = -1;
                        CurrentCustomer.InterarrivalTime = -1;
                        CurrentCustomer.ArrivalTime = 0;
                    }
                    CurrentCustomer.RandomServiceTime = Rnd.Next(1, 101);
                    Server FirstIdle = new Server();
                    FirstIdle.ServerEndTime = 100000;
                    int FirstIdleID = -1;
                    for (int q = 1; q < NoServers; q++)
                    {
                        if (Servers[q].ServerEndTime <= CurrentCustomer.ArrivalTime)
                        {
                            FirstIdle = Servers[q];
                            FirstIdleID = q;
                            break;
                        }
                        else if (Servers[q].ServerEndTime < FirstIdle.ServerEndTime)
                        {
                            FirstIdle = Servers[q];
                            FirstIdleID = q;
                        }
                    }
                    if (Servers[FirstIdleID].ServerEndTime > CurrentCustomer.ArrivalTime)
                    {
                        CurrentCustomer.WaitingTime = CurrentCustomer.ArrivalTime - Servers[FirstIdleID].ServerEndTime;
                        SystemStatistics.NoOfCustomersWait++;
                        SystemStatistics.TotalWaitingTime += CurrentCustomer.WaitingTime;
                        for (int x = Customers[i].ArrivalTime; x < Servers[FirstIdleID].ServerEndTime; x++)
                            SystemStatistics.QueueLength[x]++;
                    }
                    CurrentCustomer.AssignedServer = Servers[FirstIdleID];
                    CurrentCustomer.TimeServiceBegins = CurrentCustomer.ArrivalTime + CurrentCustomer.WaitingTime;
                    CurrentCustomer.ServiceTime = this.GetServiceTimeFromRange(CurrentCustomer.AssignedServer, CurrentCustomer);

                    CurrentCustomer.TimeServiceEnds = CurrentCustomer.TimeServiceBegins + CurrentCustomer.ServiceTime;
                    Servers[FirstIdleID].ServerEndTime = CurrentCustomer.TimeServiceEnds;
                    if (CurrentCustomer.TimeServiceEnds > SimulationEndTime)
                        break;
                    if (CurrentCustomer.RandomServiceTime == 100)
                        CurrentCustomer.RandomServiceTime = 0;
                    Customers.Add(CurrentCustomer);
                    Servers[FirstIdleID].TotalServiceTime += CurrentCustomer.ServiceTime;
                    Servers[FirstIdleID].TotalNumberOfCustomers++;
                    i++;
                }
                return Customers;
            }

            else if (SelectionMethod == Enums.ServerSelectionMethod.Random && StoppingCondition == Enums.ServerStoppingCondition.NumberOfCustomers)
            {
                int NoOfCustomers = Int32.Parse(TextBoxData);
                int AssignedServer = -1;
                Customers = new List<SimualtionCase>(NoOfCustomers);
                for (int i = 0; i < NoOfCustomers; i++)
                {
                    SimualtionCase NewCustomer = new SimualtionCase();
                    Customers.Add(NewCustomer);
                    Customers[i].CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        Customers[i].RandomInterarrivalTime = Rnd.Next(1, 101);
                        Customers[i].InterarrivalTime = this.GetInterarrivalTimeFromRange(Servers[0], Customers[i]);
                        Customers[i].ArrivalTime = Customers[i - 1].ArrivalTime + Customers[i].InterarrivalTime;
                        if (Customers[i].RandomInterarrivalTime == 100)
                            Customers[i].RandomInterarrivalTime = 0;
                        SystemStatistics.AppendNewUnitTimes(Customers[i].InterarrivalTime);
                    }
                    else
                    {
                        Customers[i].RandomInterarrivalTime = -1;
                        Customers[i].InterarrivalTime = -1;
                        Customers[i].ArrivalTime = 0;
                        SystemStatistics.AppendNewUnitTimes(1);
                    }
                    Customers[i].RandomServiceTime = Rnd.Next(1, 101);
                    this.FillFreeServers(Customers[i].ArrivalTime);
                    if (FreeServers.Count > 0)
                    {
                        AssignedServer = Rnd.Next(FreeServers.Count);
                        Customers[i].AssignedServer = Servers[AssignedServer];
                    }
                    else
                    {
                        Server FirstIdle = new Server();
                        FirstIdle.ServerEndTime = 100000;

                        for (int q = 1; q < NoServers; q++)
                        {
                            if (Servers[q].ServerEndTime < FirstIdle.ServerEndTime)
                            {
                                FirstIdle = Servers[q];
                                AssignedServer = q;
                            }
                        }
                        Customers[i].WaitingTime = Customers[i].ArrivalTime - Servers[AssignedServer].ServerEndTime;
                        SystemStatistics.NoOfCustomersWait++;
                        SystemStatistics.TotalWaitingTime += Customers[i].WaitingTime;
                        for (int x = Customers[i].ArrivalTime; x < Servers[AssignedServer].ServerEndTime; x++)
                            SystemStatistics.QueueLength[x]++;
                        Customers[i].AssignedServer = Servers[AssignedServer];
                        Servers[AssignedServer].TotalNumberOfCustomers++;
                        Servers[AssignedServer].TotalServiceTime += Customers[i].ServiceTime;
                    }

                    Customers[i].TimeServiceBegins = Customers[i].ArrivalTime + Customers[i].WaitingTime;
                    Customers[i].ServiceTime = this.GetServiceTimeFromRange(Customers[i].AssignedServer, Customers[i]);
                    Customers[i].TimeServiceEnds = Customers[i].TimeServiceBegins + Customers[i].ServiceTime;
                    Servers[AssignedServer].ServerEndTime = Customers[i].TimeServiceEnds;
                    if (Customers[i].RandomServiceTime == 100)
                        Customers[i].RandomServiceTime = 0;
                }
                return Customers;
            }

            else if (SelectionMethod == Enums.ServerSelectionMethod.Random && StoppingCondition == Enums.ServerStoppingCondition.SimulationEndTime)
            {
                int AssignedServer = -1;
                int SimulationEndTime = Int32.Parse(TextBoxData.ToString()), i = 0;
                SystemStatistics.InitializeQueueLength(SimulationEndTime);
                while (true)
                {
                    SimualtionCase CurrentCustomer = new SimualtionCase();
                    CurrentCustomer.CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        CurrentCustomer.RandomInterarrivalTime = Rnd.Next(1, 101);
                        CurrentCustomer.InterarrivalTime = this.GetInterarrivalTimeFromRange(Servers[0], CurrentCustomer);
                        CurrentCustomer.ArrivalTime = Customers[i - 1].ArrivalTime + CurrentCustomer.InterarrivalTime;
                        if (CurrentCustomer.RandomInterarrivalTime == 100)
                            CurrentCustomer.RandomInterarrivalTime = 0;
                    }
                    else
                    {
                        CurrentCustomer.RandomInterarrivalTime = -1;
                        CurrentCustomer.InterarrivalTime = -1;
                        CurrentCustomer.ArrivalTime = 0;
                    }
                    CurrentCustomer.RandomServiceTime = Rnd.Next(1, 101);
                    this.FillFreeServers(Customers[i].ArrivalTime);
                    if (FreeServers.Count > 0)
                    {
                        AssignedServer = Rnd.Next(FreeServers.Count);
                        Customers[i].AssignedServer = Servers[AssignedServer];
                    }
                    else
                    {
                        Server FirstIdle = new Server();
                        FirstIdle.ServerEndTime = 100000;

                        for (int q = 1; q < NoServers; q++)
                        {
                            if (Servers[q].ServerEndTime < FirstIdle.ServerEndTime)
                            {
                                FirstIdle = Servers[q];
                                AssignedServer = q;
                            }
                        }
                        CurrentCustomer.WaitingTime = CurrentCustomer.ArrivalTime - Servers[AssignedServer].ServerEndTime;
                        SystemStatistics.TotalWaitingTime += CurrentCustomer.WaitingTime;
                        SystemStatistics.NoOfCustomersWait++;
                        for (int x = Customers[i].ArrivalTime; x < Servers[AssignedServer].ServerEndTime; x++)
                            SystemStatistics.QueueLength[x]++;
                        CurrentCustomer.AssignedServer = Servers[AssignedServer];
                    }

                    CurrentCustomer.TimeServiceBegins = CurrentCustomer.ArrivalTime + CurrentCustomer.WaitingTime;
                    CurrentCustomer.ServiceTime = this.GetServiceTimeFromRange(CurrentCustomer.AssignedServer, CurrentCustomer);
                    CurrentCustomer.TimeServiceEnds = CurrentCustomer.TimeServiceBegins + CurrentCustomer.ServiceTime;
                    Servers[AssignedServer].ServerEndTime = CurrentCustomer.TimeServiceEnds;
                    if (CurrentCustomer.TimeServiceEnds > SimulationEndTime)
                        break;
                    if (CurrentCustomer.RandomServiceTime == 100)
                        CurrentCustomer.RandomServiceTime = 0;
                    Customers.Add(CurrentCustomer);
                    Servers[AssignedServer].TotalNumberOfCustomers++;
                    Servers[AssignedServer].TotalServiceTime += CurrentCustomer.ServiceTime;
                }
                return Customers;
            }
            else if (SelectionMethod == Enums.ServerSelectionMethod.LowestUtilization && StoppingCondition == Enums.ServerStoppingCondition.NumberOfCustomers)
            {
               
                int NoOfCustomers = Int32.Parse(TextBoxData.ToString());
                Customers = new List<SimualtionCase>(NoOfCustomers);
                for (int i = 0; i < NoOfCustomers; i++)
                {
                    SimualtionCase NewCustomer = new SimualtionCase();
                    Customers.Add(NewCustomer);
                    Customers[i].CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        Customers[i].RandomInterarrivalTime = Rnd.Next(1, 101);
                        Customers[i].InterarrivalTime = this.GetInterarrivalTimeFromRange(Servers[0], Customers[i]);
                        if (Customers[i].RandomInterarrivalTime == 100)
                            Customers[i].RandomInterarrivalTime = 0;

                        SystemStatistics.AppendNewUnitTimes(Customers[i].InterarrivalTime);
                        Customers[i].ArrivalTime = Customers[i - 1].ArrivalTime + Customers[i].InterarrivalTime;
                    }
                    else
                    {
                        Customers[i].RandomInterarrivalTime = -1;
                        Customers[i].InterarrivalTime = -1;
                        Customers[i].ArrivalTime = 0;
                        SystemStatistics.AppendNewUnitTimes(1);
                    }
                    Customers[i].RandomServiceTime = Rnd.Next(1, 101);
                    Server FirstIdle = new Server();
                    FirstIdle.ServerEndTime = 100000;
                    int FirstIdleID = -1;
                    float utilization = 3;
                    bool free_found = false;
                    

                    for (int q = 1; q < NoServers; q++)
                    {   //get the first idle server in order

                        float server_utilization = Servers[q].TotalServiceTime / Customers[i - 1].TimeServiceEnds;
                       // if server is free and least utilization , update global utiization 
                        if (Servers[q].ServerEndTime <= Customers[i].ArrivalTime && (server_utilization < utilization))
                        {  
                            FirstIdle = Servers[q];
                            FirstIdleID = q;
                            utilization = server_utilization;
                            free_found = true;
                        }
                        //if that current server end time is greater that customers arrival time, pick the one that will finish early
                        else if (Servers[q].ServerEndTime < FirstIdle.ServerEndTime && free_found == false)
                        {
                            FirstIdle = Servers[q];
                            FirstIdleID = q;
                        }
                    }
                    // if all servers are busy and customer has to wait then calculate wait time
                    
                    if (Servers[FirstIdleID].ServerEndTime > Customers[i].ArrivalTime)
                    {
                        Customers[i].WaitingTime = (Servers[FirstIdleID].ServerEndTime - Customers[i].ArrivalTime);
                        SystemStatistics.NoOfCustomersWait++;
                        SystemStatistics.TotalWaitingTime += Customers[i].WaitingTime;
                        for (int x = Customers[i].ArrivalTime; x < Servers[FirstIdleID].ServerEndTime; x++)
                            SystemStatistics.QueueLength[x]++;
                    }
                    Customers[i].AssignedServer = Servers[FirstIdleID];
                    Customers[i].TimeServiceBegins = Customers[i].ArrivalTime + Customers[i].WaitingTime;
                    Customers[i].ServiceTime = this.GetServiceTimeFromRange(Customers[i].AssignedServer, Customers[i]);
                    Customers[i].TimeServiceEnds = Customers[i].TimeServiceBegins + Customers[i].ServiceTime;
                    Servers[FirstIdleID].TotalServiceTime += Customers[i].ServiceTime;
                    Servers[FirstIdleID].TotalNumberOfCustomers++;
                    Servers[FirstIdleID].ServerEndTime = Customers[i].TimeServiceEnds;
                    if (Customers[i].RandomServiceTime == 100)
                        Customers[i].RandomServiceTime = 0;
                }
                return Customers;
            }

            else if (SelectionMethod == Enums.ServerSelectionMethod.LowestUtilization && StoppingCondition == Enums.ServerStoppingCondition.SimulationEndTime)
            {
                int SimulationEndTime = Int32.Parse(TextBoxData), i = 0;
                SystemStatistics.InitializeQueueLength(SimulationEndTime);
                while (true)
                {
                    SimualtionCase CurrentCustomer = new SimualtionCase();
                    CurrentCustomer.CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        CurrentCustomer.RandomInterarrivalTime = Rnd.Next(1, 101);
                        CurrentCustomer.InterarrivalTime = this.GetInterarrivalTimeFromRange(Servers[0], CurrentCustomer);
                        CurrentCustomer.ArrivalTime = Customers[i - 1].ArrivalTime + CurrentCustomer.InterarrivalTime;
                        if (CurrentCustomer.RandomInterarrivalTime == 100)
                            CurrentCustomer.RandomInterarrivalTime = 0;
                    }
                    else
                    {
                        CurrentCustomer.RandomInterarrivalTime = -1;
                        CurrentCustomer.InterarrivalTime = -1;
                        CurrentCustomer.ArrivalTime = 0;
                    }
                    CurrentCustomer.RandomServiceTime = Rnd.Next(1, 101);
                    Server FirstIdle = new Server();
                    FirstIdle.ServerEndTime = 100000;
                    int FirstIdleID = -1;
                    float utilization = 3;
                    bool free_found = false;


                    for (int q = 1; q < NoServers; q++)
                    {   //get the first idle server in order

                        float server_utilization = Servers[q].TotalServiceTime / Customers[i - 1].TimeServiceEnds;
                        // if server is free and least utilization , update global utiization 
                        if (Servers[q].ServerEndTime <= Customers[i].ArrivalTime && (server_utilization < utilization))
                        {
                            FirstIdle = Servers[q];
                            FirstIdleID = q;
                            utilization = server_utilization;
                            free_found = true;
                        }
                        else if (Servers[q].ServerEndTime < FirstIdle.ServerEndTime && free_found == false)
                        {
                            FirstIdle = Servers[q];
                            FirstIdleID = q;
                        }
                    }
                    if (Servers[FirstIdleID].ServerEndTime > CurrentCustomer.ArrivalTime )
                    {
                        CurrentCustomer.WaitingTime = CurrentCustomer.ArrivalTime - Servers[FirstIdleID].ServerEndTime;
                        SystemStatistics.NoOfCustomersWait++;
                        SystemStatistics.TotalWaitingTime += CurrentCustomer.WaitingTime;
                        for (int x = Customers[i].ArrivalTime; x < Servers[FirstIdleID].ServerEndTime; x++)
                            SystemStatistics.QueueLength[x]++;
                    }
                    CurrentCustomer.AssignedServer = Servers[FirstIdleID];
                    CurrentCustomer.TimeServiceBegins = CurrentCustomer.ArrivalTime + CurrentCustomer.WaitingTime;
                    CurrentCustomer.ServiceTime = this.GetServiceTimeFromRange(CurrentCustomer.AssignedServer, CurrentCustomer);

                    CurrentCustomer.TimeServiceEnds = CurrentCustomer.TimeServiceBegins + CurrentCustomer.ServiceTime;
                    Servers[FirstIdleID].ServerEndTime = CurrentCustomer.TimeServiceEnds;
                    if (CurrentCustomer.TimeServiceEnds > SimulationEndTime)
                        break;
                    if (CurrentCustomer.RandomServiceTime == 100)
                        CurrentCustomer.RandomServiceTime = 0;
                    Customers.Add(CurrentCustomer);
                    Servers[FirstIdleID].TotalServiceTime += CurrentCustomer.ServiceTime;
                    Servers[FirstIdleID].TotalNumberOfCustomers++;
                    i++;
                }
                return Customers;
            }
            else
            {
                return new List<SimualtionCase>();
            }
        }

        public Statistics ComputeSystemStatistics()
        {
            SystemStatistics.TotalRunTime = Customers[Customers.Count - 1].TimeServiceEnds;
            SystemStatistics.TotalCustomers= Customers.Count;
            SystemStatistics.NoOfServers = NoServers;
            SystemStatistics.ProbabilityOfCustomerWait = (float)SystemStatistics.NoOfCustomersWait / SystemStatistics.TotalCustomers;
            if (SystemStatistics.NoOfCustomersWait == 0)
            {
                SystemStatistics.AverageWaitTime = 0;
            }
            else
                SystemStatistics.AverageWaitTime = (float)SystemStatistics.TotalWaitingTime / SystemStatistics.NoOfCustomersWait;
            SystemStatistics.MaximumQueueLength = SystemStatistics.QueueLength.Max();
            return SystemStatistics;
        }
        public List<List<int>> graph_data(out List<int> servtime_per_server, out List<int> custnum_per_server,out int total_runtime)
        {
             servtime_per_server = new List<int>();
             custnum_per_server = new List<int>();
            List<List<int>> data = new List<List<int>>();


            //initializing the graph data list so that it contains lists equal to the number of servers 
            //initialize service time per server & customers number per server with zero
            for (int y = 1; y <= NoServers; y++)
            {
                data.Add(new List<int>());
                servtime_per_server.Add(0);
                custnum_per_server.Add(0);
            }

           total_runtime = Customers[Customers.Count - 1].TimeServiceEnds;

            for (int i = 0; i < Customers.Count; i++)
            {
                //get server 
                int server_id = Customers[i].AssignedServer.ServerId;
                //get service time start
                int service_start = Customers[i].TimeServiceBegins;
                int service_end = Customers[i].TimeServiceEnds;
                //get busy units of time of current server
                for (int j = service_start; j <= service_end; j++)
                {
                    data[server_id].Add(j);
                }

                servtime_per_server[Customers[i].AssignedServer.ServerId] += Customers[i].ServiceTime;
                custnum_per_server[Customers[i].AssignedServer.ServerId]++;
            }
            return data;
        }
    }
}
