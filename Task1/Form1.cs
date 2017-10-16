using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiChannelQueueModels;

namespace Task1
{
    public partial class Form1 : Form
    {
        int NoServers = 0;
        SimulationSystem SimSystem;
        public Form1()
        {
            InitializeComponent();
            SimSystem = new SimulationSystem();
        }

        // dynamically generate tables columns based on the number of servers 
        private void GetinputBtn_Click(object sender, EventArgs e)
        {  //get number of servers 
            NoServers = Int32.Parse(NoServersTxt.Text)+1;//that plus one added because we store the interarrival time dis in server[0]
            // 2 columns for the interarrival time distribution and 2 columns for each server's service time distribution(service time and probability columns)  
            int NoCols = 2 * NoServers ;
            InputGrdView.ColumnCount = NoCols;
            InputGrdView.Columns[0].HeaderText = "InterArrival Time";
            InputGrdView.Columns[1].HeaderText = "Probabilty";
            int ServerNo = 1;
            for (int i = 2; i < NoCols; i += 2)
            {
                InputGrdView.Columns[i].HeaderText = "Server" + ServerNo.ToString();
                InputGrdView.Columns[i + 1].HeaderText = "Probabilty";
                ServerNo++;
            }

        }

        //loading input into each server's service time dist table and interarrival time dist table 
        private void SimBtn_Click(object sender, EventArgs e)
        {
            List<Server> Servers = new List<Server>();
            // server[0] contains the time distrubution of interarivaltime throughout the entire program
            int Cols = 0;
            for (int i = 0; i < NoServers; i++)
            {
                Server new_server = new Server();
                Servers.Add(new_server);
                new_server.ServiceTimeDistribution = new List<TimeDistribution>();

                for (int j = 0; j < InputGrdView.RowCount; j++)
                {  //we should check if cells are not empty
                    // add  a new service time dist row in the current server
                    if (InputGrdView.Rows[j].Cells[Cols].Value != null)
                    {
                        TimeDistribution new_td = new TimeDistribution(); //new row
                        Servers[i].ServiceTimeDistribution.Add(new_td);
                        Servers[i].ServiceTimeDistribution[j].Time = Int32.Parse(InputGrdView.Rows[j].Cells[Cols].Value.ToString());
                        Servers[i].ServiceTimeDistribution[j].Probability = double.Parse(InputGrdView.Rows[j].Cells[Cols + 1].Value.ToString());
                    }
                }
                Cols += 2;
            }

            SimSystem.CompleteServiceTimeDistributionData(NoServers, InputGrdView.RowCount, Servers);
            
            Enums.ServerSelectionMethod SelectionMethod = new Enums.ServerSelectionMethod();
            Enums.ServerStoppingCondition StoppingCondition = new Enums.ServerStoppingCondition();
            if (PriorityRadioBtn.Checked)
            {
                SelectionMethod = Enums.ServerSelectionMethod.HighestPriority;
            }
            else if (RandomRadioBtn.Checked)
            {
                SelectionMethod = Enums.ServerSelectionMethod.Random;
            }
            else if (LeastUtiRadioBtn.Checked)
            {
                SelectionMethod = Enums.ServerSelectionMethod.LowestUtilization;
            }


            if (MaxNoRadioBtn.Checked)
            {
                StoppingCondition = Enums.ServerStoppingCondition.NumberOfCustomers;
               List<SimualtionCase> result= SimSystem.Simulate(SelectionMethod, StoppingCondition, MaxNoTxtbox.Text);
               show_results(result);
            }
            else if (SimEndTimeRadioBtn.Checked)
            {
                StoppingCondition = Enums.ServerStoppingCondition.SimulationEndTime;
                List<SimualtionCase> result = SimSystem.Simulate(SelectionMethod, StoppingCondition, SimTimeTxtbox.Text);
                show_results(result);
            }
        }

        private void MaxNoRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            MaxNoTxtbox.Visible = true;
            SimTimeTxtbox.Visible = false;
        }

        private void SimEndTimeRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            MaxNoTxtbox.Visible = false;
            SimTimeTxtbox.Visible = true;
        }

        //printing data to the result table
        void show_results(List<SimualtionCase> Customers)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CustomerNo");
            dt.Columns.Add("Random Digit For Interarrival");
            dt.Columns.Add("Time Between Arrival");
            dt.Columns.Add("Clock Time Of Arrival");
            dt.Columns.Add("Random Digit For Service");
            dt.Columns.Add("Time Service Begin");
            dt.Columns.Add("Service Time");
            dt.Columns.Add("Time Service End");
            dt.Columns.Add("ServerNo");
            dt.Columns.Add("Time In Queue");


            NoServers = Int32.Parse(NoServersTxt.Text) + 1;
            List<int> servtime_per_server = new List<int>();
            List<int> custnum_per_server = new List<int>();
            //total runtime = time service end of the last customer
            int total_runtime = 0;
            total_runtime = Customers[Customers.Count - 1].TimeServiceEnds;

            List<int> queue_length = new List<int>();
            //initializing the queue_length at time 0 to total runtime units

            for (int i = 0; i <= total_runtime; i++)
            {
                queue_length.Add(0);
               
            }

            //initialize service time per server & customers number per server with zero
            for (int d = 0; d < NoServers; d++)
            {
                servtime_per_server.Add(0);
                custnum_per_server.Add(0);
            }

            int total_customers_waiting_time = 0;
            int num_of_waited_customers = 0;


            for (int i = 0; i < Customers.Count; i++)
            {
            
                DataRow dr = dt.NewRow();
                dr["CustomerNo"] = Customers[i].CustomerNumber;
                dr["Random Digit For Interarrival"] = Customers[i].RandomInterarrivalTime;
                dr["Time Between Arrival"] = Customers[i].InterarrivalTime;
                dr["Clock Time Of Arrival"]=Customers[i].ArrivalTime;
                dr["Random Digit For Service"] = Customers[i].RandomServiceTime;
                dr["Time Service Begin"] = Customers[i].TimeServiceBegins;
                dr["Service Time"] = Customers[i].ServiceTime;
                dr["Time Service End"] = Customers[i].TimeServiceEnds;
                dr["ServerNo"] = Customers[i].AssignedServer.ServerId;
                //calculate total service time per server & num of customers assigned to each server
                servtime_per_server[Customers[i].AssignedServer.ServerId] += Customers[i].ServiceTime;
                custnum_per_server[Customers[i].AssignedServer.ServerId]++;


                dr["Time In Queue"] = Customers[i].WaitingTime;
                //calculate num of waited customers & total waiting time
                if(Customers[i].WaitingTime > 0)
                {
                    num_of_waited_customers++;
                }
                total_customers_waiting_time += Customers[i].WaitingTime;

         //calculating length of queue during runtime  (if service begin > arrival time of customer then they waited in queue)
                if (Customers[i].TimeServiceBegins > Customers[i].ArrivalTime)
                {
                    int waiting_units = Customers[i].TimeServiceBegins - Customers[i].ArrivalTime;

                    for (int y = Customers[i].ArrivalTime; y <= waiting_units; y++)
                           queue_length[y]++;
                }


                dt.Rows.Add(dr);
            }
            OutputGrdView.DataSource = dt;

            Console.WriteLine("");
            for (int j = 1; j < NoServers; j++)
            {
                if (custnum_per_server[j] == 0)
                {
                    Console.WriteLine("average service time of server " + j.ToString() + " = " + 0);
                }
                else
                Console.WriteLine("average service time of server " + j.ToString() + " = " + (float)servtime_per_server[j] / custnum_per_server[j]);

                Console.WriteLine("utilization of server " + j.ToString() + " = " + (float)servtime_per_server[j] / total_runtime);

            }

            Console.WriteLine("Probability of a customer wait in queue = "+ (float)num_of_waited_customers/Customers.Count);

            if (num_of_waited_customers == 0)
            {
                Console.WriteLine("Average waiting time in queue = " + 0);
            }
            else
            Console.WriteLine("Average waiting time in queue = "+ (float)total_customers_waiting_time/num_of_waited_customers);


            Console.WriteLine("Max Queue Length = " + queue_length.Max());
            

            
        }

  


    }

}
