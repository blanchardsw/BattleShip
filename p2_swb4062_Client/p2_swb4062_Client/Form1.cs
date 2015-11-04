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
using System.IO;
using System.Diagnostics;

namespace p2_swb4062_Client
{
    public partial class Form1 : Form
    {
        List<Ship> lstShips = new List<Ship>();     //Generic list to be used to store Ships
        string currentShip;                         //Track the current ship being built
        Ship s = new Ship();                        //Ship object to be created and placed into list
        int clickCount = 0;                         //integer to track mouse clicks in ship placement
        static string response = "";                //String to be sent to Client
        static string msg = "";                     //String received from the Client
        bool placingShips;                          //bool to track when ships are still being placed in game field
        DataGridViewCell responseCell;              //tracks the cell that was clicked to attack
        bool isYourTurn = false;                    //tracks if it is currently the Server's turn or not
        int shipsRemaining = 4;                     //track the number of ships remaining to determine game over
        public static string ip;                    //IP Address of the server as entered by the user
        public static int port;                     //Connecting port of the server as entered by the user
        public Form1()
        {
            InitializeComponent();
            DataGridView dgv2 = dataGridView2;
            //This block sets the background of all of the labels to transparent so that the background image will show instead
            lClient.BackColor = System.Drawing.Color.Transparent;
            lServer.BackColor = System.Drawing.Color.Transparent;
            lMiss.BackColor = System.Drawing.Color.Transparent;
            lHit.BackColor = System.Drawing.Color.Transparent;
            lShip.BackColor = System.Drawing.Color.Transparent;
            lInstruct.BackColor = System.Drawing.Color.Transparent;
            lStatus.BackColor = System.Drawing.Color.Transparent;
            //Create 10 rows for each datagridView
            dataGridView1.RowCount = 10;
            dataGridView2.RowCount = 10;
            //This names each row (1-10) and colors in all of the cells an aqua color
            for (int i = 0; i < 10; i++)
            {
                dataGridView1[0, i].Value = (i + 1).ToString();
                dataGridView2[0, i].Value = (i + 1).ToString();

                dataGridView2[10, i].Style.BackColor = Color.Aqua;
                dataGridView1[10, i].Style.BackColor = Color.Aqua;

                for (int j = 0; j < 10; j++)
                {
                    if (i != 0)
                    {
                        dataGridView1[i, j].Style.BackColor = Color.Aqua;
                        dataGridView2[i, j].Style.BackColor = Color.Aqua;
                    }
                }
            }
            //Disable the attack grid to keep the user from causing any trouble when it's not their turn
            dataGridView2.Enabled = false;
            //Assign a unique thread for both sending and receiving data to and from the client
            Thread.CurrentThread.Name = "MainThread";
            Program.sender = new Thread(send);
            Program.sender.Name = "Sender";
            Program.receiver = new Thread(receive);
            Program.receiver.Name = "Receiver";
            Program.hitMiss = new Thread(hitOrMiss);
            Program.n = Program.client.GetStream();
            //then create a binary writer and reader to handle the transactions
            Program.w = new BinaryWriter(Program.n);
            Program.r = new BinaryReader(Program.n);
            //Note that we're now placing ships on the player's grid
            placingShips = true;
            lStatus.Text = "Click a square on the Client grid to palce your Battleship.";
            currentShip = "Battleship";
            s.Name = "Battleship";
            s.ExtentUnits = 5;
            lstShips.Add(s);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //When the "Quit Game" button is clicked, the stream is disposed, connection closed and application exits
        private void bQuit_Click(object sender, EventArgs e)
        {
            Program.n.Dispose();
            Program.client.Close();
            Application.Exit();
        }
        //This event occurs when the Client player clicks on the Client grid
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell;
            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;
            //If the user clicks in an invalid cell, it's corrected
            if (e.ColumnIndex == 0)
                colIndex = 1;
            if (e.RowIndex < 0)
                rowIndex = 0;
            //each time the Client player clicks, we increment the clickCount to determine which action to take
            clickCount++;
            //We grab the cell that the user clicked
            cell = dataGridView1.Rows[rowIndex].Cells[colIndex];
            //If the user is placing ships, we check to see which ship is currently being placed
            if (placingShips)
            {   //If this is the first click for the current ship, we only designate an anchoring position for the ship
                if (currentShip.Equals("Battleship"))
                {
                    if (clickCount == 1)
                    {   //and ask the user to then choose an orientation by clicking a different square
                        lStatus.Text = "Click a different square to choose the orientation.";
                        cell.Style.BackColor = Color.Gray;
                        lstShips[0].Position_Row = cell.RowIndex;
                        lstShips[0].Position_Col = cell.ColumnIndex;
                    }
                    if (clickCount == 2)
                    {   //once the user clicks a different square
                        //We check to see if it's to the left/right of the previous click or if it's above/below
                        if (cell.RowIndex > lstShips[0].Position_Row || cell.RowIndex < lstShips[0].Position_Row)
                        {   //If it's above/below the original click, we place the ship vertically based on its size
                            if (lstShips[0].Position_Row + 4 <= 9)
                            {   //This if statement ensures that the chosen placement will allow the ship to fit on the grid
                                //if it can fit, we place it from top to bottom
                                dataGridView1[lstShips[0].Position_Col, lstShips[0].Position_Row + 1].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col, lstShips[0].Position_Row + 2].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col, lstShips[0].Position_Row + 3].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col, lstShips[0].Position_Row + 4].Style.BackColor = Color.Gray;
                            }
                            else
                            {   //but if that won't fit on the grid, we place the ship bottom to top instead
                                dataGridView1[lstShips[0].Position_Col, lstShips[0].Position_Row - 1].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col, lstShips[0].Position_Row - 2].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col, lstShips[0].Position_Row - 3].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col, lstShips[0].Position_Row - 4].Style.BackColor = Color.Gray;
                            }
                            lstShips[0].Orientation = Ship.ShipOrientation.Vertical;                            
                        }
                        else if (cell.ColumnIndex > lstShips[0].Position_Col || cell.ColumnIndex < lstShips[0].Position_Col)
                        {   //If the second chosen cell is left/right of the original click, we place the ship horizontally
                            if (lstShips[0].Position_Col + 4 <= 10)
                            {
                                dataGridView1[lstShips[0].Position_Col + 1, lstShips[0].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col + 2, lstShips[0].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col + 3, lstShips[0].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col + 4, lstShips[0].Position_Row].Style.BackColor = Color.Gray;
                            }
                            else
                            {   //and if not, we place it right to left
                                dataGridView1[lstShips[0].Position_Col - 1, lstShips[0].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col - 2, lstShips[0].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col - 3, lstShips[0].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[0].Position_Col - 4, lstShips[0].Position_Row].Style.BackColor = Color.Gray;
                            }
                            lstShips[0].Orientation = Ship.ShipOrientation.Horizontal;
                        }
                        //At this point, we change ships and repeat the same process until all ships have been placed
                        currentShip = "Cruiser";
                        clickCount = 0;
                        s = new Ship();
                        s.Name = "Cruiser";
                        s.ExtentUnits = 4;
                        lstShips.Add(s);
                        lStatus.Text = "Click a square on the Client grid to place your Cruiser.";
                    }                   
                }

                if (currentShip.Equals("Cruiser"))
                {
                    if (clickCount == 1)
                    {
                        lStatus.Text = "Click a different square to choose the orientation.";
                        cell.Style.BackColor = Color.Gray;
                        lstShips[1].Position_Row = cell.RowIndex;
                        lstShips[1].Position_Col = cell.ColumnIndex;
                    }
                    if (clickCount == 2)
                    {
                        if (cell.RowIndex > lstShips[1].Position_Row || cell.RowIndex < lstShips[1].Position_Row)
                        {
                            if (lstShips[1].Position_Row + 3 <= 9)
                            {
                                dataGridView1[lstShips[1].Position_Col, lstShips[1].Position_Row + 1].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[1].Position_Col, lstShips[1].Position_Row + 2].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[1].Position_Col, lstShips[1].Position_Row + 3].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                dataGridView1[lstShips[1].Position_Col, lstShips[1].Position_Row - 1].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[1].Position_Col, lstShips[1].Position_Row - 2].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[1].Position_Col, lstShips[1].Position_Row - 3].Style.BackColor = Color.Gray;
                            }
                            lstShips[1].Orientation = Ship.ShipOrientation.Vertical;
                        }
                        else if (cell.ColumnIndex > lstShips[1].Position_Col || cell.ColumnIndex < lstShips[1].Position_Col)
                        {
                            if (lstShips[1].Position_Col + 3 <= 10)
                            {
                                dataGridView1[lstShips[1].Position_Col + 1, lstShips[1].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[1].Position_Col + 2, lstShips[1].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[1].Position_Col + 3, lstShips[1].Position_Row].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                dataGridView1[lstShips[1].Position_Col - 1, lstShips[1].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[1].Position_Col - 2, lstShips[1].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[1].Position_Col - 3, lstShips[1].Position_Row].Style.BackColor = Color.Gray;
                            }
                            lstShips[1].Orientation = Ship.ShipOrientation.Horizontal;
                        }
                        currentShip = "Submarine";
                        clickCount = 0;
                        s = new Ship();
                        s.Name = "Submarine";
                        s.ExtentUnits = 3;
                        lstShips.Add(s);
                        lStatus.Text = "Click a square on the Client grid to place your Submarine.";
                    }
                }
                if (currentShip.Equals("Submarine"))
                {
                    if (clickCount == 1)
                    {
                        lStatus.Text = "Click a different square to choose the orientation.";
                        cell.Style.BackColor = Color.Gray;
                        lstShips[2].Position_Row = cell.RowIndex;
                        lstShips[2].Position_Col = cell.ColumnIndex;
                    }
                    if (clickCount == 2)
                    {
                        if (cell.RowIndex > lstShips[2].Position_Row || cell.RowIndex < lstShips[2].Position_Row)
                        {
                            if (lstShips[2].Position_Row + 2 <= 9)
                            {
                                dataGridView1[lstShips[2].Position_Col, lstShips[2].Position_Row + 1].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[2].Position_Col, lstShips[2].Position_Row + 2].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                dataGridView1[lstShips[2].Position_Col, lstShips[2].Position_Row - 1].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[2].Position_Col, lstShips[2].Position_Row - 2].Style.BackColor = Color.Gray;
                            }
                            lstShips[2].Orientation = Ship.ShipOrientation.Vertical;
                        }
                        else if (cell.ColumnIndex > lstShips[2].Position_Col || cell.ColumnIndex < lstShips[2].Position_Col)
                        {
                            if (lstShips[2].Position_Col + 2 <= 10)
                            {
                                dataGridView1[lstShips[2].Position_Col + 1, lstShips[2].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[2].Position_Col + 2, lstShips[2].Position_Row].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                dataGridView1[lstShips[2].Position_Col - 1, lstShips[2].Position_Row].Style.BackColor = Color.Gray;
                                dataGridView1[lstShips[2].Position_Col - 2, lstShips[2].Position_Row].Style.BackColor = Color.Gray;
                            }
                            lstShips[2].Orientation = Ship.ShipOrientation.Horizontal;
                        }
                        currentShip = "Destroyer";
                        clickCount = 0;
                        s = new Ship();
                        s.Name = "Destroyer";
                        s.ExtentUnits = 2;
                        lstShips.Add(s);
                        lStatus.Text = "Click a square on the Client grid to place your Destroyer.";
                    }
                }
                if (currentShip.Equals("Destroyer"))
                {
                    if (clickCount == 1)
                    {
                        lStatus.Text = "Click a different square to choose the orientation.";
                        cell.Style.BackColor = Color.Gray;
                        lstShips[3].Position_Row = cell.RowIndex;
                        lstShips[3].Position_Col = cell.ColumnIndex;
                    }
                    if (clickCount == 2)
                    {
                        if (cell.RowIndex > lstShips[3].Position_Row || cell.RowIndex < lstShips[3].Position_Row)
                        {
                            if (lstShips[3].Position_Row + 1 <= 9)
                            {
                                dataGridView1[lstShips[3].Position_Col, lstShips[3].Position_Row + 1].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                dataGridView1[lstShips[3].Position_Col, lstShips[3].Position_Row - 1].Style.BackColor = Color.Gray;
                            }
                            lstShips[3].Orientation = Ship.ShipOrientation.Vertical;
                        }
                        else if (cell.ColumnIndex > lstShips[2].Position_Col || cell.ColumnIndex < lstShips[1].Position_Col)
                        {
                            if (lstShips[3].Position_Col + 1 <= 10)
                            {
                                dataGridView1[lstShips[3].Position_Col + 1, lstShips[3].Position_Row].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                dataGridView1[lstShips[3].Position_Col - 1, lstShips[3].Position_Row].Style.BackColor = Color.Gray;
                            }
                            lstShips[3].Orientation = Ship.ShipOrientation.Horizontal;
                        }
                        //once all ships are placed, the user only needs to click anywhere on the grid to finish the placement
                        //then we start the send/receive threads
                        clickCount = 0;
                        currentShip = "";
                        placingShips = false;
                        lStatus.Text = "Click again on the Client grid to finish placing your ships.";
                        isYourTurn = true;
                        Program.receiver.Start();
                        Program.sender.Start();
                    }
                }
            }
            else
            {   //once we're no longer placing ships, we disable the grid used in the ship placement
                if(cell.Style.BackColor == Color.Gray)cell.Style.SelectionBackColor = Color.Gray;
                else cell.Style.SelectionBackColor = Color.Aqua;
                dataGridView1.Enabled = false;
                dataGridView2.Enabled = true;
                lInstruct.Text = "Click a square on the Server's grid to attack!";
                lStatus.Text = "";
            }

        }
        //This handles the attacks on the Server
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {   //if the user clicks the Server's grid to attack and it's currently their turn
            if (!placingShips && isYourTurn)
            {   //we note which cell they clicked, correct any attempt to be a nuisance
                int rowIndex = e.RowIndex;
                int colIndex = e.ColumnIndex;

                if (e.ColumnIndex == 0)
                    colIndex = 1;
                if (e.RowIndex < 0)
                    rowIndex = 0;

                responseCell = dataGridView2.Rows[rowIndex].Cells[colIndex];
                //and prepare to ship that decision off to the Server to see if we hit or missed
                //we also only want to bother sending the attack if the cell hasn't already been attacked
                if (responseCell.Style.BackColor == Color.Aqua)
                {
                    response = responseCell.ColumnIndex.ToString() +" "+ responseCell.RowIndex.ToString();
                    Thread.Sleep(500);
                }
                else
                    lStatus.Text = "That square has already been attacked!";
            }
            else
                lStatus.Text = "It's not your turn!";
        }
        //this function runs on the Sender thread and handles all outgoing messages
        private void send()
        {   //as long as we have a connection to the server
            while (Program.client.Connected)
            {   //we check to see which thread is running this because sometimes the Receiver thread needs to use this function as well
                //we want to break out if the game is over and simply end the cycling of this thread
                if (Thread.CurrentThread.Name.Equals("Sender") && msg.Equals("GameOver"))
                    break;
                //Here, we inform the Server if their most recent attack landed or missed
                if ((response.Equals("Hit") || response.Equals("Miss")) && !isYourTurn)
                {
                    Program.w.Write(response);
                    Program.w.Flush();
                }
                //But if it's the Client's turn, we send our attack over to the Client for evaluation
                if (!response.Equals("Hit") && !response.Equals("Miss") && !response.Equals("") && isYourTurn)
                {
                    Program.w.Write(response);
                    Program.w.Flush();
                    response = "";
                    this.Invoke(new MethodInvoker(() => dataGridView2.Enabled = false));
                    isYourTurn = false;
                    this.Invoke(new MethodInvoker(() => lStatus.Text = "Server's Turn!")); 
                }
                //if the receiver thread is borrowing this function, don't sleep, just break out and return to the receiver
                if (!Thread.CurrentThread.Name.Equals("Receiver"))
                    Thread.Sleep(500);
                else
                    break;
            }
        }
        //This function handles all incoming messages
        private void receive()
        {   //as long as we have a connection to the Server:
            while (Program.client.Connected)
            {   //see if there's a message
                try { msg = Program.r.ReadString(); }
                catch { msg = "";}
                //If the Server is not telling us about our previous attack, then we must be receiving some coordinates
                if (msg.Equals("GameOver")) { gameOver(); break; }

                if (!msg.Equals("Hit") && !msg.Equals("Miss") && !msg.Equals("") && !isYourTurn)
                {   //we break up those coordinates so that we can evaluate them
                    string[] msgSplit = msg.Split(' ');
                    int row = int.Parse(msgSplit[1]);
                    int col = int.Parse(msgSplit[0]);
                    //then we look at the cell that these coordinates belong to
                    DataGridViewCell msgCell = dataGridView1.Rows[row].Cells[col];
                    //if the cell is gray, it means we have a ship there
                    if (msgCell.Style.BackColor == Color.Gray)
                    {   //so we mark it red to indicate that it's been hit and prepare to tell the Server
                        msgCell.Style.BackColor = Color.Red;
                        response = "Hit";
                        //We then check the status of each ship to determine if they should be marked as Sunken
                        if (lstShips[0].IsOnField(row, col))
                        {
                            lstShips[0].Hits++;
                            if (lstShips[0].Hits >= lstShips[0].ExtentUnits && !lstShips[0].Sunken)
                            {
                                lstShips[0].Sunken = true;
                                shipsRemaining--;
                            }
                        } if (lstShips[1].IsOnField(row, col))
                        {
                            lstShips[1].Hits++;
                            if (lstShips[1].Hits >= lstShips[1].ExtentUnits && !lstShips[1].Sunken)
                            {
                                lstShips[1].Sunken = true;
                                shipsRemaining--;
                            }
                        }
                        if (lstShips[2].IsOnField(row, col))
                        {
                            lstShips[2].Hits++;
                            if (lstShips[2].Hits >= lstShips[2].ExtentUnits && !lstShips[2].Sunken)
                            {
                                lstShips[2].Sunken = true;
                                shipsRemaining--;
                            }
                        }
                        if (lstShips[3].IsOnField(row, col))
                        {
                            lstShips[3].Hits++;
                            if (lstShips[3].Hits >= lstShips[3].ExtentUnits && !lstShips[3].Sunken)
                            {
                                lstShips[3].Sunken = true;
                                shipsRemaining--;
                            }
                        }
                        if (shipsRemaining <= 0) { response = "GameOver"; gameOver(); }
                        send();
                    }
                    else
                    {   //If no ship is on the attacked cell, we mark it White for a miss and inform the Client of their failure
                        if(msgCell.Style.BackColor != Color.Red) msgCell.Style.BackColor = Color.White;
                        response = "Miss";
                        send();
                    }
                    isYourTurn = true;
                    this.Invoke(new MethodInvoker(() => dataGridView2.Enabled = true));
                    this.Invoke(new MethodInvoker(() => lStatus.Text = "Your Turn!"));
                }
                else
                    hitOrMiss();
                Thread.Sleep(500);
            }
        }
        //A simple function call to update the grid used for attacking
        private void hitOrMiss()
        {
            try
            {
                this.Invoke(new MethodInvoker(() => lInstruct.Text = "That shot was a " + msg + "!"));
                if (msg.Equals("Hit") || msg.Equals("Miss"))
                {
                    if (msg.Equals("Hit") && responseCell.Style.BackColor == Color.Aqua) responseCell.Style.BackColor = Color.Red;
                    if (msg.Equals("Miss") && responseCell.Style.BackColor == Color.Aqua) responseCell.Style.BackColor = Color.White;
                }
            }
            catch { }
        }
        //Function to handle a game over
        private void gameOver()
        {   //When game over occurs, the user is informed of the game's end status along with a win or lose notification depending on the number of ships on the field
            this.Invoke(new MethodInvoker(() => lStatus.Text = "Game Over!"));
            if (lstShips[0].Sunken == false | lstShips[1].Sunken == false | lstShips[2].Sunken == false | lstShips[3].Sunken == false)
                this.Invoke(new MethodInvoker(() => lInstruct.Text = "You Win!"));
            else
                this.Invoke(new MethodInvoker(() => lInstruct.Text = "You Lose!"));
            this.Invoke(new MethodInvoker(() => dataGridView1.Enabled = false));
            this.Invoke(new MethodInvoker(() => dataGridView2.Enabled = false));
        }
    }
}
