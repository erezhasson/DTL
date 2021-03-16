
namespace DTLExpert
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
            this.btnTrain = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblPB = new System.Windows.Forms.Label();
            this.chkRecalcGains = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(52, 12);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(75, 23);
            this.btnTrain.TabIndex = 0;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(52, 233);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(641, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 1;
            this.progressBar1.Value = 80;
            // 
            // lblPB
            // 
            this.lblPB.AutoSize = true;
            this.lblPB.BackColor = System.Drawing.Color.Lime;
            this.lblPB.Location = new System.Drawing.Point(337, 237);
            this.lblPB.Name = "lblPB";
            this.lblPB.Size = new System.Drawing.Size(36, 13);
            this.lblPB.TabIndex = 2;
            this.lblPB.Text = "1/100";
            // 
            // chkRecalcGains
            // 
            this.chkRecalcGains.AutoSize = true;
            this.chkRecalcGains.Location = new System.Drawing.Point(52, 41);
            this.chkRecalcGains.Name = "chkRecalcGains";
            this.chkRecalcGains.Size = new System.Drawing.Size(105, 17);
            this.chkRecalcGains.TabIndex = 3;
            this.chkRecalcGains.Text = "chkRecalcGains";
            this.chkRecalcGains.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chkRecalcGains);
            this.Controls.Add(this.lblPB);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnTrain);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblPB;
        private System.Windows.Forms.CheckBox chkRecalcGains;
    }
}

