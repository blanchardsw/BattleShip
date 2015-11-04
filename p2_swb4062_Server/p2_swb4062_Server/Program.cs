using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace p2_swb4062_Server
{
    static class Program
    {
        public static TcpListener listener;         //Handles TCP connection requests
        public static TcpClient c;                  //Handles the connection to the Client
        public static NetworkStream n;              //The stream between Server and Client
        public static BinaryReader r;               //Incoming messages
        public static BinaryWriter w;               //Outgoing messages
        public static Form startupForm;             //A reference variable to the initial form
        public static Thread sender;                //Thread to handle all send requests
        public static Thread receiver;              //Thread to handle all receive requests
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(startupForm = new Form1());
        }
    }
    //Class to simulate an individual ship
    public class Ship
    {   //Enum for the ship's orientation
        public enum ShipOrientation
        {
            Horizontal = 0,
            Vertical
        }
        //Name of the current ship (ex: Battleship, Destroyer, etc)
        public string Name
        {
            get;
            set;
        }
        public ShipOrientation Orientation
        {
            get;
            set;
        }
        //The ship's hit points
        public int ExtentUnits
        {
            get;
            set;
        }
        //The anchoring point of the ship
        public int Position_Row
        {
            get;
            set;
        } 
        //the anchoring point of the ship
        public int Position_Col
        {
            get;
            set;
        } 
        //The damage the ship has taken thus far
        public int Hits
        {
            get;
            set;
        }
        //Whether or not the ship is afloat
        public bool Sunken
        {
            get;
            set;
        }
        //Constructor to ensure are newly created ships are not sunken by default
        public Ship()
        {
            Sunken = false;
        }

        // This method allows to determine as to whether the ship hits a given field on the board.
        public bool IsOnField(int Row, int Col)
        {
            if (this.Orientation == ShipOrientation.Horizontal)
                return (Row == this.Position_Row && Col >= Position_Col && Col <= (Position_Col + ExtentUnits - 1));
            else
                // the equivalent of the above, but for vertically oriented ships
                return (Col == this.Position_Col && Row >= Position_Row && Row <= (Position_Row + ExtentUnits - 1));
        }
    }
}
