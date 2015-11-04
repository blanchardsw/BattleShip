/*  
Author:         Stephen Blanchard
CLID:           swb4062
Class:          CMPS 358
Assignment:     project_2 - Salvo
assignment­#     1
Due Date:       11:55pm October 13
Description:    Salvo - Networked Game via GUI
*/

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
using System.Collections;

namespace p2_swb4062_Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Listen for any IP address on port 51111
            Program.listener = new TcpListener(IPAddress.Any, 51111);
            Program.listener.Start();
        }
        //The start game button hides the connection window and open up the game window to begin ship placement
        private void bStart_Click(object sender, EventArgs e)
        {
                try
                {
                    Program.c = Program.listener.AcceptTcpClient();
                    if (Program.c.Connected)
                    {
                        lStatus2.Text = "Client successfully connected!";
                        Program.n = Program.c.GetStream();
                        this.Hide();
                        new Form2().Show();
                    }
                }
                catch
                {
                    lStatus2.Text = "Connection Failed!";
                }
            
        }
    }
}
