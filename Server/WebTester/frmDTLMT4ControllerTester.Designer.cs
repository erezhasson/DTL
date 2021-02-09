
namespace WebTester
{
    partial class frmDTLMT4ControllerTester
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGet_Status = new System.Windows.Forms.Button();
            this.txtEnergyStarSize = new System.Windows.Forms.TextBox();
            this.btnSet_ResetEnergyStarSize = new System.Windows.Forms.Button();
            this.txtNewEnergyStarSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEnergyStarAnimation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGet_Status
            // 
            this.btnGet_Status.Location = new System.Drawing.Point(1, 3);
            this.btnGet_Status.Name = "btnGet_Status";
            this.btnGet_Status.Size = new System.Drawing.Size(164, 131);
            this.btnGet_Status.TabIndex = 0;
            this.btnGet_Status.Text = "Get_Status";
            this.btnGet_Status.UseVisualStyleBackColor = true;
            this.btnGet_Status.Click += new System.EventHandler(this.btnGet_Status_Click);
            // 
            // txtEnergyStarSize
            // 
            this.txtEnergyStarSize.Enabled = false;
            this.txtEnergyStarSize.Location = new System.Drawing.Point(194, 21);
            this.txtEnergyStarSize.Name = "txtEnergyStarSize";
            this.txtEnergyStarSize.Size = new System.Drawing.Size(237, 23);
            this.txtEnergyStarSize.TabIndex = 1;
            // 
            // btnSet_ResetEnergyStarSize
            // 
            this.btnSet_ResetEnergyStarSize.Location = new System.Drawing.Point(1, 190);
            this.btnSet_ResetEnergyStarSize.Name = "btnSet_ResetEnergyStarSize";
            this.btnSet_ResetEnergyStarSize.Size = new System.Drawing.Size(164, 134);
            this.btnSet_ResetEnergyStarSize.TabIndex = 2;
            this.btnSet_ResetEnergyStarSize.Text = "Set_ResetEnergyStarSize";
            this.btnSet_ResetEnergyStarSize.UseVisualStyleBackColor = true;
            this.btnSet_ResetEnergyStarSize.Click += new System.EventHandler(this.btnSet_ResetEnergyStarSize_Click);
            // 
            // txtNewEnergyStarSize
            // 
            this.txtNewEnergyStarSize.Location = new System.Drawing.Point(194, 247);
            this.txtNewEnergyStarSize.Name = "txtNewEnergyStarSize";
            this.txtNewEnergyStarSize.Size = new System.Drawing.Size(237, 23);
            this.txtNewEnergyStarSize.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Energy Star Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Energy Star Animation";
            // 
            // txtEnergyStarAnimation
            // 
            this.txtEnergyStarAnimation.Enabled = false;
            this.txtEnergyStarAnimation.Location = new System.Drawing.Point(194, 95);
            this.txtEnergyStarAnimation.Name = "txtEnergyStarAnimation";
            this.txtEnergyStarAnimation.Size = new System.Drawing.Size(237, 23);
            this.txtEnergyStarAnimation.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "New Energy Star Size";
            // 
            // frmDTLMT4ControllerTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(489, 336);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEnergyStarAnimation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNewEnergyStarSize);
            this.Controls.Add(this.btnSet_ResetEnergyStarSize);
            this.Controls.Add(this.txtEnergyStarSize);
            this.Controls.Add(this.btnGet_Status);
            this.Name = "frmDTLMT4ControllerTester";
            this.Opacity = 0.95D;
            this.Text = "DTLMT4ControllerTester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGet_Status;
        private System.Windows.Forms.TextBox txtEnergyStarSize;
        private System.Windows.Forms.Button btnSet_ResetEnergyStarSize;
        private System.Windows.Forms.TextBox txtNewEnergyStarSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEnergyStarAnimation;
        private System.Windows.Forms.Label label3;
    }
}

