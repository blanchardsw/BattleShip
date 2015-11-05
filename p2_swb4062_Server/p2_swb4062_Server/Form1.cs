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
        private int timeCount = 0;
        //The start game button hides the connection window and open up the game window to begin ship placement
        private void bStart_Click(object sender, EventArgs e)
        {
            int timeoutCount = 0;
                try
                {
                    //multi-threading so gui will remain responsive while connection is attempted
                    //thread 1 is where the program actually listens for the client
                    Thread t1 = new Thread(() =>
                    {
                        using (Program.c = Program.listener.AcceptTcpClient()) ;
                    });

                    //thread 2 is the portion of code that is executed if the is successful
                    Thread t2 = new Thread(() =>
                    {
                        lStatus2.Text = "Client successfully connected!";
                        Program.n = Program.c.GetStream();
                        this.Hide();
                        new Form2().Show();
                    });
                    t1.Start();
                    if (Program.c.Connected)
                    {
                        t2.Start();
                    }
                }
                catch
                {
                    lStatus2.Text = "Connection Failed!";
                }
            
        }

    }
}
