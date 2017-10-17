namespace Task1
{
    partial class graphform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.graph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.server_stat = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.graph)).BeginInit();
            this.SuspendLayout();
            // 
            // graph
            // 
            chartArea2.Name = "ChartArea1";
            this.graph.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.graph.Legends.Add(legend2);
            this.graph.Location = new System.Drawing.Point(3, 12);
            this.graph.Name = "graph";
            series2.ChartArea = "ChartArea1";
            series2.CustomProperties = "PointWidth=1";
            series2.Legend = "Legend1";
            series2.Name = "status";
            this.graph.Series.Add(series2);
            this.graph.Size = new System.Drawing.Size(1234, 317);
            this.graph.TabIndex = 0;
            this.graph.Text = "graph";
            // 
            // server_stat
            // 
            this.server_stat.Location = new System.Drawing.Point(374, 361);
            this.server_stat.Name = "server_stat";
            this.server_stat.Size = new System.Drawing.Size(413, 96);
            this.server_stat.TabIndex = 1;
            this.server_stat.Text = "";
            // 
            // graphform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 492);
            this.Controls.Add(this.server_stat);
            this.Controls.Add(this.graph);
            this.Name = "graphform";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.graphform_Load);
            ((System.ComponentModel.ISupportInitialize)(this.graph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart graph;
        public System.Windows.Forms.RichTextBox server_stat;

    }
}