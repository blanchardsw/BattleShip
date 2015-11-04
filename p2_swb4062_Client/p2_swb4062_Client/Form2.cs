using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace p2_swb4062_Client
{
    public partial class Form2 : Form
    {  
        public Form2()
        {
            InitializeComponent();

        }

        private void tbIP_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tbPort_TextChanged(object sender, EventArgs e)
        {

        }
        //When the connect button is clicked, a connection to the server is attempted with the user's
        //given IP address and port number.  If the connection fails, the user is informed as such
        private void bConnect_Click(object sender, EventArgs e)
        {   //try to connect.  if successful, hide the connection form and show the game form
            try 
            {
                Form1.ip = tbIP.Text;
                Form1.port = int.Parse(tbPort.Text);
                Program.client = new TcpClient(Form1.ip, Form1.port);
                if (Program.client.Connected)
                {
                    this.Hide();
                    new Form1().Show();
                }
                
            }
            catch
            {
                lError.Text = "Connection Error!";
            }
           
        }
    }
}
