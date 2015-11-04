namespace p2_swb4062_Server
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
            this.lStatus = new System.Windows.Forms.Label();
            this.lStatus2 = new System.Windows.Forms.Label();
            this.bStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lStatus
            // 
            this.lStatus.AutoSize = true;
            this.lStatus.Font = new System.Drawing.Font("Impact", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatus.Location = new System.Drawing.Point(12, 9);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(396, 35);
            this.lStatus.TabIndex = 2;
            this.lStatus.Text = "Server listening on port 51111...";
            // 
            // lStatus2
            // 
            this.lStatus2.AutoSize = true;
            this.lStatus2.Font = new System.Drawing.Font("Impact", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatus2.Location = new System.Drawing.Point(12, 100);
            this.lStatus2.Name = "lStatus2";
            this.lStatus2.Size = new System.Drawing.Size(0, 35);
            this.lStatus2.TabIndex = 3;
            // 
            // bStart
            // 
            this.bStart.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bStart.Location = new System.Drawing.Point(463, 263);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(136, 59);
            this.bStart.TabIndex = 4;
            this.bStart.Text = "Start Game";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Aqua;
            this.ClientSize = new System.Drawing.Size(611, 334);
            this.Controls.Add(this.bStart);
            this.Controls.Add(this.lStatus2);
            this.Controls.Add(this.lStatus);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.Label lStatus2;
        private System.Windows.Forms.Button bStart;
    }
}

