using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace p2_swb4062_Client
{
    static class Program
    {
        public static NetworkStream n;
        public static BinaryReader r;
        public static BinaryWriter w;
        public static TcpClient client;
        public static Form startupForm;
        public static Thread receiver;
        public static Thread sender;
        public static Thread hitMiss;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);          
            Application.Run(startupForm = new Form2());  
        }
    }

    public class Ship
    {
        public enum ShipOrientation
        {
            Horizontal = 0,
            Vertical
        }
        public string Name 
        { 
            get; set; 
        }
        public ShipOrientation Orientation 
        { 
            get; set; 
        }
        public int ExtentUnits 
        { 
            get; set; 
        } // The number of fields the ship occupies
        public int Position_Row 
        { 
            get; set; 
        } // The (zero-based) row index where the ship starts out
        public int Position_Col 
        { 
            get; set; 
        } // The (zero-based) column index where the ship starts out

        public int Hits
        {
            get; set;
        }
        public bool Sunken
        {
            get;
            set;
        }
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
