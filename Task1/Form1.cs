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
        Statistics SystemStatistics = new Statistics();

        public Form1()
        {
            InitializeComponent();
            SimSystem = new SimulationSystem();
        }

        // dynamically generate tables columns based on the number of servers 
        private void GetinputBtn_Click(object sender, EventArgs e)
        {
            InputGrdView.DataSource = null;
            InputGrdView.Rows.Clear();
            OutputGrdView.DataSource = null;
            OutputGrdView.Rows.Clear();
            PriorityRadioBtn.Checked = false;
            RandomRadioBtn.Checked = false;
            LeastUtiRadioBtn.Checked = false;
            MaxNoRadioBtn.Checked = false;
            MaxNoTxtbox.Visible = false;
            SimEndTimeRadioBtn.Checked = false;
            SimTimeTxtbox.Visible = false;

            //get number of servers 
            NoServers = Int32.Parse(NoServersTxt.Text) + 1;//that plus one added because we store the interarrival time dis in server[0]
            // 2 columns for the interarrival time distribution and 2 columns for each server's service time distribution(service time and probability columns)  
            int NoCols = 2 * NoServers;
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
                List<SimualtionCase> result = SimSystem.Simulate(SelectionMethod, StoppingCondition, MaxNoTxtbox.Text);
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

            for (int i = 0; i < Customers.Count; i++)
            {

                DataRow dr = dt.NewRow();
                dr["CustomerNo"] = Customers[i].CustomerNumber;
                dr["Random Digit For Interarrival"] = Customers[i].RandomInterarrivalTime;
                dr["Time Between Arrival"] = Customers[i].InterarrivalTime;
                dr["Clock Time Of Arrival"] = Customers[i].ArrivalTime;
                dr["Random Digit For Service"] = Customers[i].RandomServiceTime;
                dr["Time Service Begin"] = Customers[i].TimeServiceBegins;
                dr["Service Time"] = Customers[i].ServiceTime;
                dr["Time Service End"] = Customers[i].TimeServiceEnds;
                dr["ServerNo"] = Customers[i].AssignedServer.ServerId;
                dr["Time In Queue"] = Customers[i].WaitingTime;
                dt.Rows.Add(dr);
            }
            OutputGrdView.DataSource = dt;

        }

        private void statbutton_Click(object sender, EventArgs e)
        {
            statform new_stat = new statform();
            SystemStatistics = SimSystem.ComputeSystemStatistics();
            new_stat.richTextBox1.Text += "ToTal Simulation Time = " + SystemStatistics.TotalRunTime.ToString() + "\n";
            new_stat.richTextBox1.Text += "Total Number OF Customers = " + SystemStatistics.TotalCustomers.ToString() + "\n";
            new_stat.richTextBox1.Text += "Number Of Waited Customers = " + SystemStatistics.TotalWaitingTime.ToString() + "\n";
            new_stat.richTextBox1.Text += "Probability Of A Customer Wait in Queue = " + SystemStatistics.ProbabilityOfCustomerWait.ToString() + "\n";
            new_stat.richTextBox1.Text += "Average Waiting Time In Queue = " + SystemStatistics.AverageWaitTime.ToString() + "\n";
            new_stat.richTextBox1.Text += "Max Queue Length = " + SystemStatistics.MaximumQueueLength.ToString() + "\n";
            new_stat.richTextBox1.Text += "Total Customers Waiting Time = " + SystemStatistics.TotalWaitingTime.ToString() + "\n";
            new_stat.Show();

        }
        private void showgraph_btn_Click(object sender, EventArgs e)
        {   

             List<int> servtime_per_server;
             List<int> custnum_per_server;
             int total_runtime;
            List<List<int>> graph_data = SimSystem.graph_data(out servtime_per_server ,out custnum_per_server,out total_runtime );
            //iterate on each server and get its busy unit times
            for (int i = 1; i < graph_data.Count; i++)
            {
                //create graph for each server
                graphform g = new graphform();
                g.graph.ChartAreas[0].AxisX.Interval = 1;
                for (int y = 0; y < graph_data[i].Count; y++)
                {
                    int busy = graph_data[i][y];
                    g.graph.Series["status"].Points.AddXY(busy, 1);
                }
                g.Text = "Server " + i.ToString();
                float AVG = (float)servtime_per_server[i] / custnum_per_server[i];
                float Utilization = (float)servtime_per_server[i] / total_runtime;
                g.server_stat.Text += "service time of server " + "= " + servtime_per_server[i].ToString() + "\n";
                g.server_stat.Text += "customer number of server " + "= " + custnum_per_server[i].ToString() + "\n";
                g.server_stat.Text += "average service time of server " + "= " + AVG.ToString() + "\n";
                g.server_stat.Text +="utilization of server " +"= " + Utilization.ToString()+ "\n" ;
                g.Show();
            }
        }
    }
}
