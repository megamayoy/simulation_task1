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
        //this function calculates the cumlative probability on each server (note:server[0] contains the interaarival time dist table)
        public void CompleteServiceTimeDistributionData(int NoOfServers, int NoOfRows, List<Server> SystemServers)
        {
            NoServers = NoOfServers;
            Servers = SystemServers;
            int Cols = 0;
            for (int i = 0; i < NoServers; i++)
            {
                Servers[i].ServerId = i ;
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
                        // min range is the equal to the max range of the previous row + 1
                        Servers[i].ServiceTimeDistribution[j].MinRange = Servers[i].ServiceTimeDistribution[j-1].MaxRange + 1;
                    }

                    Servers[i].ServiceTimeDistribution[j].CummProbability = CummProbability;

                    if (j == NoOfRows - 1)
                        Servers[i].ServiceTimeDistribution[j].MaxRange = 0;
                    else
                        Servers[i].ServiceTimeDistribution[j].MaxRange = Servers[i].ServiceTimeDistribution[j].CummProbability * 100.0f;
                    Servers[i].Status = false;
                    Servers[i].ServerEndTime = 0;
                }
                Cols += 2;
            }

        }

        private int GetTimeFromRange(Server CurrenentServer, SimualtionCase CurrentCustomer)
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
            if (SelectionMethod == Enums.ServerSelectionMethod.HighestPriority && StoppingCondition == Enums.ServerStoppingCondition.NumberOfCustomers)
            {
                int NoOfCustomers = Int32.Parse(TextBoxData.ToString());
                
                for (int i = 0; i < NoOfCustomers; i++)
                {
                    SimualtionCase new_sc = new SimualtionCase();
                    Customers.Add(new_sc);
                    Customers[i].CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        Customers[i].RandomInterarrivalTime = Rnd.Next(100);
                        Customers[i].InterarrivalTime = this.GetTimeFromRange(Servers[0], Customers[i]);
                        Customers[i].ArrivalTime = Customers[i - 1].ArrivalTime + Customers[i].InterarrivalTime;
                    }
                    else
                    {
                        Customers[i].RandomInterarrivalTime = -1;
                        Customers[i].InterarrivalTime = -1;
                        Customers[i].ArrivalTime = 0;
                    }
                    Customers[i].RandomServiceTime = Rnd.Next(100);

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
                        Customers[i].WaitingTime = (Servers[FirstIdleID].ServerEndTime-Customers[i].ArrivalTime );


                    Customers[i].AssignedServer = Servers[FirstIdleID];
                    Customers[i].TimeServiceBegins = Customers[i].ArrivalTime + Customers[i].WaitingTime;
                    Customers[i].ServiceTime = this.GetTimeFromRange(Customers[i].AssignedServer, Customers[i]);
                    Customers[i].TimeServiceEnds = Customers[i].TimeServiceBegins + Customers[i].ServiceTime;
                    Servers[FirstIdleID].ServerEndTime = Customers[i].TimeServiceEnds;
                }
                return Customers;
            }

            else if (SelectionMethod == Enums.ServerSelectionMethod.HighestPriority && StoppingCondition == Enums.ServerStoppingCondition.SimulationEndTime)
            {
                
                int SimulationEndTime = Int32.Parse(TextBoxData), i = 0;
                while (true)
                {
                    SimualtionCase CurrentCustomer = new SimualtionCase();
                    CurrentCustomer.CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        CurrentCustomer.RandomInterarrivalTime = Rnd.Next(100);
                        CurrentCustomer.InterarrivalTime = this.GetTimeFromRange(Servers[0], CurrentCustomer);
                        CurrentCustomer.ArrivalTime = Customers[i - 1].ArrivalTime + CurrentCustomer.InterarrivalTime;
                    }
                    else
                    {
                        CurrentCustomer.RandomInterarrivalTime = -1;
                        CurrentCustomer.InterarrivalTime = -1;
                        CurrentCustomer.ArrivalTime = 0;
                    }
                    CurrentCustomer.RandomServiceTime = Rnd.Next(100);
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
                        CurrentCustomer.WaitingTime = CurrentCustomer.ArrivalTime - Servers[FirstIdleID].ServerEndTime;
                    CurrentCustomer.AssignedServer = Servers[FirstIdleID];
                    CurrentCustomer.TimeServiceBegins = CurrentCustomer.ArrivalTime + CurrentCustomer.WaitingTime;
                    CurrentCustomer.ServiceTime = this.GetTimeFromRange(CurrentCustomer.AssignedServer, CurrentCustomer);

                    CurrentCustomer.TimeServiceEnds = CurrentCustomer.TimeServiceBegins + CurrentCustomer.ServiceTime;
                    Servers[FirstIdleID].ServerEndTime = CurrentCustomer.TimeServiceEnds;
                    if (Servers[FirstIdleID].ServerEndTime > SimulationEndTime)
                        break;
                    Customers.Add(CurrentCustomer);
                    i++;
                }
                return Customers;
            }

            else if (SelectionMethod == Enums.ServerSelectionMethod.Random && StoppingCondition == Enums.ServerStoppingCondition.NumberOfCustomers)
            {
                int NoOfCustomers = Int32.Parse(TextBoxData);
                int AssignedServer = -1;
                for (int i = 0; i < NoOfCustomers; i++)
                {   
                    SimualtionCase new_sc = new SimualtionCase();
                    Customers.Add(new_sc);
                    Customers[i].CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        Customers[i].RandomInterarrivalTime = Rnd.Next(100);
                        Customers[i].InterarrivalTime = this.GetTimeFromRange(Servers[0], Customers[i]);
                        Customers[i].ArrivalTime = Customers[i - 1].ArrivalTime + Customers[i].InterarrivalTime;
                    }
                    else
                    {
                        Customers[i].RandomInterarrivalTime = -1;
                        Customers[i].InterarrivalTime = -1;
                        Customers[i].ArrivalTime = 0;
                    }
                    Customers[i].RandomServiceTime = Rnd.Next(100);
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
                            if (Servers[q].ServerEndTime <= Customers[i].ArrivalTime)
                            {
                                FirstIdle = Servers[q];
                                AssignedServer = q;
                                break;
                            }
                            else if (Servers[q].ServerEndTime < FirstIdle.ServerEndTime)
                            {
                                FirstIdle = Servers[q];
                                AssignedServer = q;
                            }
                        }
                        Customers[i].WaitingTime = Customers[i].ArrivalTime - Servers[AssignedServer].ServerEndTime;
                        Customers[i].AssignedServer = Servers[AssignedServer];
                    }

                    Customers[i].TimeServiceBegins = Customers[i].ArrivalTime + Customers[i].WaitingTime;
                    Customers[i].ServiceTime = this.GetTimeFromRange(Customers[i].AssignedServer, Customers[i]);
                    Customers[i].TimeServiceEnds = Customers[i].TimeServiceBegins + Customers[i].ServiceTime;
                    Servers[AssignedServer].ServerEndTime = Customers[i].TimeServiceEnds;
                }
                return Customers;
            }

            else if (SelectionMethod == Enums.ServerSelectionMethod.Random && StoppingCondition == Enums.ServerStoppingCondition.SimulationEndTime)
            {
                int AssignedServer = -1;
                
                int SimulationEndTime = Int32.Parse(TextBoxData.ToString()), i = 0;
                while (true)
                {
                    SimualtionCase CurrentCustomer = new SimualtionCase();
                    CurrentCustomer.CustomerNumber = i + 1;
                    if (i != 0)
                    {
                        CurrentCustomer.RandomInterarrivalTime = Rnd.Next(100);
                        CurrentCustomer.InterarrivalTime = this.GetTimeFromRange(Servers[0], CurrentCustomer);
                        CurrentCustomer.ArrivalTime = Customers[i - 1].ArrivalTime + CurrentCustomer.InterarrivalTime;
                    }
                    else
                    {
                        CurrentCustomer.RandomInterarrivalTime = -1;
                        CurrentCustomer.InterarrivalTime = -1;
                        CurrentCustomer.ArrivalTime = 0;
                    }
                    CurrentCustomer.RandomServiceTime = Rnd.Next(100);
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
                            if (Servers[q].ServerEndTime <= Customers[i].ArrivalTime)
                            {
                                FirstIdle = Servers[q];
                                AssignedServer = q;
                                break;
                            }
                            else if (Servers[q].ServerEndTime < FirstIdle.ServerEndTime)
                            {
                                FirstIdle = Servers[q];
                                AssignedServer = q;
                            }
                        }
                        CurrentCustomer.WaitingTime = CurrentCustomer.ArrivalTime - Servers[AssignedServer].ServerEndTime;
                        CurrentCustomer.AssignedServer = Servers[AssignedServer];
                    }

                    CurrentCustomer.TimeServiceBegins = CurrentCustomer.ArrivalTime + CurrentCustomer.WaitingTime;
                    CurrentCustomer.ServiceTime = this.GetTimeFromRange(CurrentCustomer.AssignedServer, CurrentCustomer);
                    CurrentCustomer.TimeServiceEnds = CurrentCustomer.TimeServiceBegins + CurrentCustomer.ServiceTime;
                    Servers[AssignedServer].ServerEndTime = CurrentCustomer.TimeServiceEnds;
                   // Customers.Add(CurrentCustomer);
                }
                return Customers;
            }
            else if (SelectionMethod == Enums.ServerSelectionMethod.LowestUtilization && StoppingCondition == Enums.ServerStoppingCondition.NumberOfCustomers)
            {
                return new List<SimualtionCase>();
            }

            else if (SelectionMethod == Enums.ServerSelectionMethod.LowestUtilization && StoppingCondition == Enums.ServerStoppingCondition.SimulationEndTime)
            {
                return new List<SimualtionCase>();
            }
            else
            {
                return new List<SimualtionCase>();
            }
        }
    }
}


