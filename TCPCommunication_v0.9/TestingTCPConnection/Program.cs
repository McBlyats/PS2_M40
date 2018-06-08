using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestingTCPConnection
{
    class Program
    {
        public static System.Timers.Timer timy = new System.Timers.Timer(1800); 
        static byte[] buffers;
        static object state = 0;
        private static TcpClient client;
        static IAsyncResult result;

        static void Main(string[] args)
        {
            tcp_listener();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            client.Close();
        }

        public static void tcp_listener()
        {
            try
            {
                Int32 port = 2000;
                IPAddress localAddr = IPAddress.Parse("192.168.0.190");

                TcpListener server = new TcpListener(localAddr, port);

                
                server.Start();

                Byte[] bytes = new Byte[16];
                String data = null;

                Boolean inaltime = false;
                

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    //timy.Enabled = true;
                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                    Restart:
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);       
                        Console.WriteLine(String.Format("Received: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",bytes[0],
                        bytes[1],bytes[2],bytes[3],bytes[4],bytes[5],bytes[6],bytes[7],bytes[8],bytes[9]));
                        byte[] msg = new byte[16];
                        msg[0] = bytes[0];
                        msg[1] = bytes[8];                       
                        
                        stream.Write(msg, 0, msg.Length);                       
                    
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }          

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        } 
          public static int  Opreste_Banda ()
            {
                return 0;
            }
    }
}
