using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;

namespace Track_Controller_1._02
{
    class CommunicationServer
    {
        //Server class variables
        private TcpListener mListener;
        private TcpClient mClient;
        private NetworkStream mStream;
        private StreamReader mSr;
        private StreamWriter mSw;

        //***********************************************************************************************************************************
        //Constructor: Establishes a TCP listener that accepts one client only. For additional server client relationships, additional server
        //objects must be created.
        //<mPort>: Defines the port to receive communications from. This must be provided when the server class is instantiated.
        //</string>: return type string verifies the connection has been made.
       /* public CommunicationServer(int mPort)
        {
            //creates new TCPlistener, stores in the listener variable, and starts listening for clients.
            mListener = new TcpListener(System.Net.IPAddress.Any, mPort);
            mListener.Start();

            //This loop makes ten attempts to connect to client using try/catch. On a successful connection, confirmation is sent to client and String returned.
            //On a failed connection, string is returned.
            for (int i = 0; i < 10; i++)
            {
                mClient = mListener.AcceptTcpClient();
                mStream = mClient.GetStream();
                mSr = new StreamReader(mClient.GetStream());
                mSw = new StreamWriter(mClient.GetStream());
                try
                {
                    byte[] buffer = new byte[1024];
                    mStream.Read(buffer, 0, buffer.Length);
                    int recv = 0;
                    foreach (byte b in buffer)
                    {
                        if (b != 0)
                        {
                            recv++;
                        }
                    }
                    string mRequest = Encoding.UTF8.GetString(buffer, 0, recv);
                    mSw.WriteLine(mRequest + " received.");
                    mSw.Flush();
                    return "Connected to client.";
                }
                catch (Exception e)
                {
                    mSw.WriteLine(e.ToString());
                }
            }
            return "Failed to connect.";
        }

        public string ReceiveRequest()
        {
            return "Sample";
        }

       */ 
    }
}
