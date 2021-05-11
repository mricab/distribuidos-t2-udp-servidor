using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace srv2
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Variables */
            Int32 size;     //Data size in bytes.

            /* Socket creation */
            Socket skt = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            /* Socket binding */
            IPAddress srvAddress = IPAddress.Parse("127.0.0.1");
            Int32 prtNumber = 8080;
            IPEndPoint srvEndPoint = new IPEndPoint(srvAddress, prtNumber);
            skt.Bind(srvEndPoint);

            /* Receiving data */
            int choice = 0;
            while(true) {

                /* Option */
                byte[] optByte = new byte[1];
                Console.WriteLine("Waiting for data type.");    
                size = skt.Receive(optByte);
                string opt = Encoding.ASCII.GetString(optByte);
                Console.WriteLine("Option Selected: {0}", opt);

                /* Data */
                int.TryParse(opt, out choice);
                switch (choice) {

                    case 1:
                        /* Text message */
                        byte[] msgBytes = new byte[100];
                        Console.WriteLine("Waiting for message.");
                        size = skt.Receive(msgBytes);
                        string msg = Encoding.ASCII.GetString(msgBytes);
                        Console.WriteLine("Message: {0}", msg);
                        Console.WriteLine("{0} byte(s) succesfully received.", size);
                        break;

                    case 2:
                        /* Text file */
                        byte[] fileNameBytes = new byte[100];
                        byte[] fileDataBytes = new byte[65536];
                        Console.WriteLine("Waiting for file.");

                        // Receiving filename.
                        size = skt.Receive(fileNameBytes);                     
                        string fileName = Encoding.UTF8.GetString(fileNameBytes);
                        fileName = fileName.Substring(0,size);                        
                        Console.WriteLine(fileName);
                        Console.WriteLine("Reveiving: {0}", fileName);
                        
                        // Receiving data.
                        size = skt.Receive(fileDataBytes);
                        Console.WriteLine("File received succesfully.", size);

                        // Writing data.
                        string dateString = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        using (FileStream fs = File.OpenWrite("./files/"+dateString+"_"+fileName))
                            { fs.Write(fileDataBytes, 0, fileDataBytes.Length); }
                        Console.WriteLine("File stored.", size);
                        break;

                    default:
                        Console.WriteLine("Wrong selection.");
                        break;
                }
            }
        }
    }
}
