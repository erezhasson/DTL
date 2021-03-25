
namespace DTLExpert
{
    partial class FormTesting
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
            this.btnAdvise = new System.Windows.Forms.Button();
            this.Dir = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAdvisedDir = new System.Windows.Forms.TextBox();
            this.txtAdvisedAbort = new System.Windows.Forms.TextBox();
            this.txtAdvisedReturn = new System.Windows.Forms.TextBox();
            this.btnEvaluate = new System.Windows.Forms.Button();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(130, 12);
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
            this.chkRecalcGains.Location = new System.Drawing.Point(123, 42);
            this.chkRecalcGains.Name = "chkRecalcGains";
            this.chkRecalcGains.Size = new System.Drawing.Size(105, 17);
            this.chkRecalcGains.TabIndex = 3;
            this.chkRecalcGains.Text = "chkRecalcGains";
            this.chkRecalcGains.UseVisualStyleBackColor = true;
            // 
            // btnAdvise
            // 
            this.btnAdvise.Location = new System.Drawing.Point(237, 12);
            this.btnAdvise.Name = "btnAdvise";
            this.btnAdvise.Size = new System.Drawing.Size(75, 23);
            this.btnAdvise.TabIndex = 4;
            this.btnAdvise.Text = "Advise";
            this.btnAdvise.UseVisualStyleBackColor = true;
            this.btnAdvise.Click += new System.EventHandler(this.btnAdvise_Click);
            // 
            // Dir
            // 
            this.Dir.AutoSize = true;
            this.Dir.Location = new System.Drawing.Point(234, 38);
            this.Dir.Name = "Dir";
            this.Dir.Size = new System.Drawing.Size(20, 13);
            this.Dir.TabIndex = 5;
            this.Dir.Text = "Dir";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(234, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Position";
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(316, 38);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(100, 20);
            this.txtDir.TabIndex = 7;
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(316, 54);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(100, 20);
            this.txtPosition.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(234, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Return";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(234, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Abort";
            // 
            // txtAdvisedDir
            // 
            this.txtAdvisedDir.Location = new System.Drawing.Point(435, 39);
            this.txtAdvisedDir.Name = "txtAdvisedDir";
            this.txtAdvisedDir.Size = new System.Drawing.Size(100, 20);
            this.txtAdvisedDir.TabIndex = 11;
            // 
            // txtAdvisedAbort
            // 
            this.txtAdvisedAbort.Location = new System.Drawing.Point(435, 88);
            this.txtAdvisedAbort.Name = "txtAdvisedAbort";
            this.txtAdvisedAbort.Size = new System.Drawing.Size(100, 20);
            this.txtAdvisedAbort.TabIndex = 14;
            // 
            // txtAdvisedReturn
            // 
            this.txtAdvisedReturn.Location = new System.Drawing.Point(435, 72);
            this.txtAdvisedReturn.Name = "txtAdvisedReturn";
            this.txtAdvisedReturn.Size = new System.Drawing.Size(100, 20);
            this.txtAdvisedReturn.TabIndex = 13;
            // 
            // btnEvaluate
            // 
            this.btnEvaluate.Location = new System.Drawing.Point(618, 12);
            this.btnEvaluate.Name = "btnEvaluate";
            this.btnEvaluate.Size = new System.Drawing.Size(75, 23);
            this.btnEvaluate.TabIndex = 15;
            this.btnEvaluate.Text = "Evaluate";
            this.btnEvaluate.UseVisualStyleBackColor = true;
            this.btnEvaluate.Click += new System.EventHandler(this.btnEvaluate_Click);
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(12, 12);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(75, 23);
            this.btnLoadData.TabIndex = 16;
            this.btnLoadData.Text = "LoadData";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // FormTesting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLoadData);
            this.Controls.Add(this.btnEvaluate);
            this.Controls.Add(this.txtAdvisedAbort);
            this.Controls.Add(this.txtAdvisedReturn);
            this.Controls.Add(this.txtAdvisedDir);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Dir);
            this.Controls.Add(this.btnAdvise);
            this.Controls.Add(this.chkRecalcGains);
            this.Controls.Add(this.lblPB);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnTrain);
            this.Name = "FormTesting";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblPB;
        private System.Windows.Forms.CheckBox chkRecalcGains;
        private System.Windows.Forms.Button btnAdvise;
        private System.Windows.Forms.Label Dir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAdvisedDir;
        private System.Windows.Forms.TextBox txtAdvisedAbort;
        private System.Windows.Forms.TextBox txtAdvisedReturn;
        private System.Windows.Forms.Button btnEvaluate;
        private System.Windows.Forms.Button btnLoadData;
    }
}

