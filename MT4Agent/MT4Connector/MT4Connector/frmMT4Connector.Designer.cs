
namespace MT4Connector
{
    partial class frmMT4Connector
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
            this.txtWorkingFolder = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLastBidPrice = new System.Windows.Forms.TextBox();
            this.txtLastPriceTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.chkToServer = new System.Windows.Forms.CheckBox();
            this.chkToLog = new System.Windows.Forms.CheckBox();
            this.btnTestLogCreation = new System.Windows.Forms.Button();
            this.txtLastAskPrice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "WorkingFolder";
            // 
            // txtWorkingFolder
            // 
            this.txtWorkingFolder.Location = new System.Drawing.Point(99, 19);
            this.txtWorkingFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtWorkingFolder.Multiline = true;
            this.txtWorkingFolder.Name = "txtWorkingFolder";
            this.txtWorkingFolder.Size = new System.Drawing.Size(196, 67);
            this.txtWorkingFolder.TabIndex = 1;
            this.txtWorkingFolder.Text = "C:\\Users\\remote\\AppData\\Roaming\\MetaQuotes\\Terminal\\62B8A0282B3745E01673EF4E1C377" +
    "8AA\\MQL4\\Files";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(665, 17);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(127, 72);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start!";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(106, 120);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last Bid";
            // 
            // txtLastBidPrice
            // 
            this.txtLastBidPrice.Enabled = false;
            this.txtLastBidPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtLastBidPrice.ForeColor = System.Drawing.Color.Maroon;
            this.txtLastBidPrice.Location = new System.Drawing.Point(99, 138);
            this.txtLastBidPrice.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLastBidPrice.Name = "txtLastBidPrice";
            this.txtLastBidPrice.Size = new System.Drawing.Size(138, 20);
            this.txtLastBidPrice.TabIndex = 5;
            this.txtLastBidPrice.Text = "<LastBidPrice>";
            // 
            // txtLastPriceTime
            // 
            this.txtLastPriceTime.Enabled = false;
            this.txtLastPriceTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtLastPriceTime.ForeColor = System.Drawing.Color.Maroon;
            this.txtLastPriceTime.Location = new System.Drawing.Point(99, 193);
            this.txtLastPriceTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLastPriceTime.Name = "txtLastPriceTime";
            this.txtLastPriceTime.Size = new System.Drawing.Size(138, 20);
            this.txtLastPriceTime.TabIndex = 6;
            this.txtLastPriceTime.Text = "<LastTime>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(131, 175);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Time";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(350, 15);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(124, 72);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "Test DTL Server connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // chkToServer
            // 
            this.chkToServer.AutoSize = true;
            this.chkToServer.Location = new System.Drawing.Point(665, 95);
            this.chkToServer.Name = "chkToServer";
            this.chkToServer.Size = new System.Drawing.Size(89, 19);
            this.chkToServer.TabIndex = 10;
            this.chkToServer.Text = "ToDTLServer";
            this.chkToServer.UseVisualStyleBackColor = true;
            // 
            // chkToLog
            // 
            this.chkToLog.AutoSize = true;
            this.chkToLog.Location = new System.Drawing.Point(665, 114);
            this.chkToLog.Name = "chkToLog";
            this.chkToLog.Size = new System.Drawing.Size(58, 19);
            this.chkToLog.TabIndex = 11;
            this.chkToLog.Text = "ToLog";
            this.chkToLog.UseVisualStyleBackColor = true;
            // 
            // btnTestLogCreation
            // 
            this.btnTestLogCreation.Location = new System.Drawing.Point(482, 17);
            this.btnTestLogCreation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnTestLogCreation.Name = "btnTestLogCreation";
            this.btnTestLogCreation.Size = new System.Drawing.Size(124, 72);
            this.btnTestLogCreation.TabIndex = 12;
            this.btnTestLogCreation.Text = "Test Log creation";
            this.btnTestLogCreation.UseVisualStyleBackColor = true;
            this.btnTestLogCreation.Click += new System.EventHandler(this.btnTestLogCreation_Click);
            // 
            // txtLastAskPrice
            // 
            this.txtLastAskPrice.Enabled = false;
            this.txtLastAskPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtLastAskPrice.ForeColor = System.Drawing.Color.Maroon;
            this.txtLastAskPrice.Location = new System.Drawing.Point(308, 138);
            this.txtLastAskPrice.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLastAskPrice.Name = "txtLastAskPrice";
            this.txtLastAskPrice.Size = new System.Drawing.Size(138, 20);
            this.txtLastAskPrice.TabIndex = 14;
            this.txtLastAskPrice.Text = "<LastAskPrice>";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label4.Location = new System.Drawing.Point(315, 120);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Last Ask";
            // 
            // frmMT4Connector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.txtLastAskPrice);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnTestLogCreation);
            this.Controls.Add(this.chkToLog);
            this.Controls.Add(this.chkToServer);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLastPriceTime);
            this.Controls.Add(this.txtLastBidPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtWorkingFolder);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmMT4Connector";
            this.ShowIcon = false;
            this.Text = "MT4Connector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWorkingFolder;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLastBidPrice;
        private System.Windows.Forms.TextBox txtLastPriceTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.CheckBox chkToServer;
        private System.Windows.Forms.CheckBox chkToLog;
        private System.Windows.Forms.Button btnTestLogCreation;
        private System.Windows.Forms.TextBox txtLastAskPrice;
        private System.Windows.Forms.Label label4;
    }
}

