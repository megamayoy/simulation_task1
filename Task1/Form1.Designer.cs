﻿namespace Task1
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.NoServersTxt = new System.Windows.Forms.TextBox();
            this.InputGrdView = new System.Windows.Forms.DataGridView();
            this.OutputGrdView = new System.Windows.Forms.DataGridView();
            this.PriorityRadioBtn = new System.Windows.Forms.RadioButton();
            this.RandomRadioBtn = new System.Windows.Forms.RadioButton();
            this.LeastUtiRadioBtn = new System.Windows.Forms.RadioButton();
            this.MaxNoRadioBtn = new System.Windows.Forms.RadioButton();
            this.SimEndTimeRadioBtn = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MaxNoTxtbox = new System.Windows.Forms.TextBox();
            this.SimTimeTxtbox = new System.Windows.Forms.TextBox();
            this.SimBtn = new System.Windows.Forms.Button();
            this.GetinputBtn = new System.Windows.Forms.Button();
            this.statbutton = new System.Windows.Forms.Button();
            this.showgraph_btn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.InputGrdView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputGrdView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Num of Servers";
            // 
            // NoServersTxt
            // 
            this.NoServersTxt.Location = new System.Drawing.Point(98, 6);
            this.NoServersTxt.Name = "NoServersTxt";
            this.NoServersTxt.Size = new System.Drawing.Size(37, 20);
            this.NoServersTxt.TabIndex = 1;
            // 
            // InputGrdView
            // 
            this.InputGrdView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InputGrdView.Location = new System.Drawing.Point(190, 6);
            this.InputGrdView.Name = "InputGrdView";
            this.InputGrdView.Size = new System.Drawing.Size(1044, 298);
            this.InputGrdView.TabIndex = 2;
            // 
            // OutputGrdView
            // 
            this.OutputGrdView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OutputGrdView.Location = new System.Drawing.Point(190, 310);
            this.OutputGrdView.Name = "OutputGrdView";
            this.OutputGrdView.Size = new System.Drawing.Size(1044, 362);
            this.OutputGrdView.TabIndex = 3;
            // 
            // PriorityRadioBtn
            // 
            this.PriorityRadioBtn.AutoSize = true;
            this.PriorityRadioBtn.Location = new System.Drawing.Point(3, 3);
            this.PriorityRadioBtn.Name = "PriorityRadioBtn";
            this.PriorityRadioBtn.Size = new System.Drawing.Size(56, 17);
            this.PriorityRadioBtn.TabIndex = 4;
            this.PriorityRadioBtn.TabStop = true;
            this.PriorityRadioBtn.Text = "Priority";
            this.PriorityRadioBtn.UseVisualStyleBackColor = true;
            // 
            // RandomRadioBtn
            // 
            this.RandomRadioBtn.AutoSize = true;
            this.RandomRadioBtn.Location = new System.Drawing.Point(3, 26);
            this.RandomRadioBtn.Name = "RandomRadioBtn";
            this.RandomRadioBtn.Size = new System.Drawing.Size(65, 17);
            this.RandomRadioBtn.TabIndex = 5;
            this.RandomRadioBtn.TabStop = true;
            this.RandomRadioBtn.Text = "Random";
            this.RandomRadioBtn.UseVisualStyleBackColor = true;
            // 
            // LeastUtiRadioBtn
            // 
            this.LeastUtiRadioBtn.AutoSize = true;
            this.LeastUtiRadioBtn.Location = new System.Drawing.Point(3, 49);
            this.LeastUtiRadioBtn.Name = "LeastUtiRadioBtn";
            this.LeastUtiRadioBtn.Size = new System.Drawing.Size(99, 17);
            this.LeastUtiRadioBtn.TabIndex = 6;
            this.LeastUtiRadioBtn.TabStop = true;
            this.LeastUtiRadioBtn.Text = "Least Utilization";
            this.LeastUtiRadioBtn.UseVisualStyleBackColor = true;
            // 
            // MaxNoRadioBtn
            // 
            this.MaxNoRadioBtn.AutoSize = true;
            this.MaxNoRadioBtn.Location = new System.Drawing.Point(3, 3);
            this.MaxNoRadioBtn.Name = "MaxNoRadioBtn";
            this.MaxNoRadioBtn.Size = new System.Drawing.Size(152, 17);
            this.MaxNoRadioBtn.TabIndex = 7;
            this.MaxNoRadioBtn.TabStop = true;
            this.MaxNoRadioBtn.Text = "Maximum No Of Customers";
            this.MaxNoRadioBtn.UseVisualStyleBackColor = true;
            this.MaxNoRadioBtn.CheckedChanged += new System.EventHandler(this.MaxNoRadioBtn_CheckedChanged);
            // 
            // SimEndTimeRadioBtn
            // 
            this.SimEndTimeRadioBtn.AutoSize = true;
            this.SimEndTimeRadioBtn.Location = new System.Drawing.Point(3, 54);
            this.SimEndTimeRadioBtn.Name = "SimEndTimeRadioBtn";
            this.SimEndTimeRadioBtn.Size = new System.Drawing.Size(121, 17);
            this.SimEndTimeRadioBtn.TabIndex = 8;
            this.SimEndTimeRadioBtn.TabStop = true;
            this.SimEndTimeRadioBtn.Text = "Simulation End Time";
            this.SimEndTimeRadioBtn.UseVisualStyleBackColor = true;
            this.SimEndTimeRadioBtn.CheckedChanged += new System.EventHandler(this.SimEndTimeRadioBtn_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Server Selection Method";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Stopping Condition";
            // 
            // MaxNoTxtbox
            // 
            this.MaxNoTxtbox.Location = new System.Drawing.Point(24, 26);
            this.MaxNoTxtbox.Name = "MaxNoTxtbox";
            this.MaxNoTxtbox.Size = new System.Drawing.Size(44, 20);
            this.MaxNoTxtbox.TabIndex = 11;
            this.MaxNoTxtbox.Visible = false;
            // 
            // SimTimeTxtbox
            // 
            this.SimTimeTxtbox.Location = new System.Drawing.Point(24, 77);
            this.SimTimeTxtbox.Name = "SimTimeTxtbox";
            this.SimTimeTxtbox.Size = new System.Drawing.Size(44, 20);
            this.SimTimeTxtbox.TabIndex = 12;
            this.SimTimeTxtbox.Visible = false;
            // 
            // SimBtn
            // 
            this.SimBtn.Location = new System.Drawing.Point(41, 288);
            this.SimBtn.Name = "SimBtn";
            this.SimBtn.Size = new System.Drawing.Size(75, 23);
            this.SimBtn.TabIndex = 13;
            this.SimBtn.Text = "Simulate";
            this.SimBtn.UseVisualStyleBackColor = true;
            this.SimBtn.Click += new System.EventHandler(this.SimBtn_Click);
            // 
            // GetinputBtn
            // 
            this.GetinputBtn.Location = new System.Drawing.Point(41, 32);
            this.GetinputBtn.Name = "GetinputBtn";
            this.GetinputBtn.Size = new System.Drawing.Size(75, 23);
            this.GetinputBtn.TabIndex = 14;
            this.GetinputBtn.Text = "Get Input";
            this.GetinputBtn.UseVisualStyleBackColor = true;
            this.GetinputBtn.Click += new System.EventHandler(this.GetinputBtn_Click);
            // 
            // statbutton
            // 
            this.statbutton.Location = new System.Drawing.Point(31, 368);
            this.statbutton.Name = "statbutton";
            this.statbutton.Size = new System.Drawing.Size(104, 42);
            this.statbutton.TabIndex = 15;
            this.statbutton.Text = "Show System Statistics ";
            this.statbutton.UseVisualStyleBackColor = true;
            this.statbutton.Click += new System.EventHandler(this.statbutton_Click);
            // 
            // showgraph_btn
            // 
            this.showgraph_btn.Location = new System.Drawing.Point(31, 317);
            this.showgraph_btn.Name = "showgraph_btn";
            this.showgraph_btn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.showgraph_btn.Size = new System.Drawing.Size(99, 45);
            this.showgraph_btn.TabIndex = 16;
            this.showgraph_btn.Text = "Show Server Statistics";
            this.showgraph_btn.UseVisualStyleBackColor = true;
            this.showgraph_btn.Click += new System.EventHandler(this.showgraph_btn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PriorityRadioBtn);
            this.panel1.Controls.Add(this.LeastUtiRadioBtn);
            this.panel1.Controls.Add(this.RandomRadioBtn);
            this.panel1.Location = new System.Drawing.Point(29, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(101, 75);
            this.panel1.TabIndex = 17;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.MaxNoRadioBtn);
            this.panel2.Controls.Add(this.SimEndTimeRadioBtn);
            this.panel2.Controls.Add(this.MaxNoTxtbox);
            this.panel2.Controls.Add(this.SimTimeTxtbox);
            this.panel2.Location = new System.Drawing.Point(29, 182);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(156, 100);
            this.panel2.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 691);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.showgraph_btn);
            this.Controls.Add(this.statbutton);
            this.Controls.Add(this.GetinputBtn);
            this.Controls.Add(this.SimBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OutputGrdView);
            this.Controls.Add(this.InputGrdView);
            this.Controls.Add(this.NoServersTxt);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Servers Simulation";
            ((System.ComponentModel.ISupportInitialize)(this.InputGrdView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputGrdView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NoServersTxt;
        private System.Windows.Forms.DataGridView InputGrdView;
        private System.Windows.Forms.DataGridView OutputGrdView;
        private System.Windows.Forms.RadioButton PriorityRadioBtn;
        private System.Windows.Forms.RadioButton RandomRadioBtn;
        private System.Windows.Forms.RadioButton LeastUtiRadioBtn;
        private System.Windows.Forms.RadioButton MaxNoRadioBtn;
        private System.Windows.Forms.RadioButton SimEndTimeRadioBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MaxNoTxtbox;
        private System.Windows.Forms.TextBox SimTimeTxtbox;
        private System.Windows.Forms.Button SimBtn;
        private System.Windows.Forms.Button GetinputBtn;
        private System.Windows.Forms.Button statbutton;
        private System.Windows.Forms.Button showgraph_btn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}

