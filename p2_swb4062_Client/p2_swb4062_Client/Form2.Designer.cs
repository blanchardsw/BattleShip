namespace p2_swb4062_Client
{
    partial class Form2
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
            this.bConnect = new System.Windows.Forms.Button();
            this.lIP = new System.Windows.Forms.Label();
            this.lPort = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bConnect
            // 
            this.bConnect.Location = new System.Drawing.Point(363, 234);
            this.bConnect.Name = "bConnect";
            this.bConnect.Size = new System.Drawing.Size(141, 48);
            this.bConnect.TabIndex = 0;
            this.bConnect.Text = "Connect";
            this.bConnect.UseVisualStyleBackColor = true;
            this.bConnect.Click += new System.EventHandler(this.bConnect_Click);
            // 
            // lIP
            // 
            this.lIP.AutoSize = true;
            this.lIP.Font = new System.Drawing.Font("Impact", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lIP.Location = new System.Drawing.Point(32, 57);
            this.lIP.Name = "lIP";
            this.lIP.Size = new System.Drawing.Size(153, 35);
            this.lIP.TabIndex = 1;
            this.lIP.Text = "IP Address:";
            // 
            // lPort
            // 
            this.lPort.AutoSize = true;
            this.lPort.Font = new System.Drawing.Font("Impact", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPort.Location = new System.Drawing.Point(32, 145);
            this.lPort.Name = "lPort";
            this.lPort.Size = new System.Drawing.Size(73, 35);
            this.lPort.TabIndex = 2;
            this.lPort.Text = "Port:";
            // 
            // tbIP
            // 
            this.tbIP.Font = new System.Drawing.Font("Impact", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbIP.Location = new System.Drawing.Point(206, 56);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(298, 40);
            this.tbIP.TabIndex = 3;
            this.tbIP.TextChanged += new System.EventHandler(this.tbIP_TextChanged);
            // 
            // tbPort
            // 
            this.tbPort.Font = new System.Drawing.Font("Impact", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPort.Location = new System.Drawing.Point(206, 145);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(298, 40);
            this.tbPort.TabIndex = 4;
            this.tbPort.TextChanged += new System.EventHandler(this.tbPort_TextChanged);
            // 
            // lError
            // 
            this.lError.AutoSize = true;
            this.lError.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lError.Location = new System.Drawing.Point(33, 257);
            this.lError.Name = "lError";
            this.lError.Size = new System.Drawing.Size(0, 25);
            this.lError.TabIndex = 5;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Aqua;
            this.ClientSize = new System.Drawing.Size(544, 307);
            this.Controls.Add(this.lError);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.lPort);
            this.Controls.Add(this.lIP);
            this.Controls.Add(this.bConnect);
            this.Name = "Form2";
            this.Text = "Connect to Server...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bConnect;
        private System.Windows.Forms.Label lIP;
        private System.Windows.Forms.Label lPort;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lError;
    }
}