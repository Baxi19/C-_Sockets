using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Program
{

    // Incoming data from the client.  
    public static string data = null;

    public static void StartListening()
    {
        //Node Data
        String ip = "localhost";
        int port = 11000;

        // Data buffer for incoming data.  
        byte[] bytes = new Byte[1024];

        // Establish the local endpoint for the socket.  
        // Dns.GetHostName returns the name of the
        // host running the application.  
        //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPHostEntry ipHostInfo = Dns.GetHostEntry(ip);
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

        // Create a TCP/IP socket.  
        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and
        // listen for incoming connections.  
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            // Start listening for connections.  
            while (true)
            {
                Console.WriteLine("SERVER>Waiting for a connection...");
                // Program is suspended while waiting for an incoming connection.  
                Socket handler = listener.Accept();
                data = null;

                // An incoming connection needs to be processed.  
                while (true)
                {
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }
                string[] dataClean = data.Split("<EOF>");

                // Show the data on the console.  
                Console.WriteLine("SERVER>Text received : {0}", dataClean[0]);

                // Echo the data back to the client.  
                //byte[] msg = Encoding.ASCII.GetBytes(data);
                byte[] msg = Encoding.ASCII.GetBytes("Pass");

                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nSERVER>Press ENTER to continue...");
        Console.Read();

    }

    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}