using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SerialPort mySerialPort = new SerialPort("COM3");
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.ReadTimeout = 500;
            mySerialPort.WriteTimeout = 500;
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(Form1.DataReceivedHandler);
            mySerialPort.Open();

            //Console.WriteLine("Press any key to continue...");
            //Console.WriteLine();
            //Console.ReadKey();
            //mySerialPort.Close();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}
