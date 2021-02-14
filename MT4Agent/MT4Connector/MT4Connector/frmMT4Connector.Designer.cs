
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
            this.txtLastPrice = new System.Windows.Forms.TextBox();
            this.txtLastPriceTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "WorkingFolder";
            // 
            // txtWorkingFolder
            // 
            this.txtWorkingFolder.Location = new System.Drawing.Point(85, 39);
            this.txtWorkingFolder.Name = "txtWorkingFolder";
            this.txtWorkingFolder.Size = new System.Drawing.Size(119, 20);
            this.txtWorkingFolder.TabIndex = 1;
            this.txtWorkingFolder.Text = "c:\\temp";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(257, 23);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 62);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start!";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(91, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last";
            // 
            // txtLastPrice
            // 
            this.txtLastPrice.Enabled = false;
            this.txtLastPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtLastPrice.ForeColor = System.Drawing.Color.Maroon;
            this.txtLastPrice.Location = new System.Drawing.Point(85, 117);
            this.txtLastPrice.Name = "txtLastPrice";
            this.txtLastPrice.Size = new System.Drawing.Size(119, 20);
            this.txtLastPrice.TabIndex = 5;
            this.txtLastPrice.Text = "<LastPrice>";
            // 
            // txtLastPriceTime
            // 
            this.txtLastPriceTime.Enabled = false;
            this.txtLastPriceTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtLastPriceTime.ForeColor = System.Drawing.Color.Maroon;
            this.txtLastPriceTime.Location = new System.Drawing.Point(257, 117);
            this.txtLastPriceTime.Name = "txtLastPriceTime";
            this.txtLastPriceTime.Size = new System.Drawing.Size(119, 20);
            this.txtLastPriceTime.TabIndex = 6;
            this.txtLastPriceTime.Text = "<LastPrice>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(285, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Time";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(483, 61);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // frmMT4Connector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLastPriceTime);
            this.Controls.Add(this.txtLastPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtWorkingFolder);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.TextBox txtLastPrice;
        private System.Windows.Forms.TextBox txtLastPriceTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTest;
    }
}

