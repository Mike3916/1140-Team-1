﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;


namespace Track_Controller_1._02
{
    public class CommunicationClient
    {
        //****************************************************************************************************************************************
        //Constructor: Initializes connection with the server and sends a test message.
        //<mIP>: String representing the IP address of the connection to be made. The default 127.0.0.1 is the local loop address (itself)
        //<mPort>: integer representing the TCP port to be used to communicate with the server. The default is port 1300
        public CommunicationClient(string mIP= "127.0.0.1", int mPort= 1300)
        {
            connection:
            try
            {
                //establish connection with the server, set to class variable mClient, and send sample message
                mClient = new TcpClient(mIP, mPort);
                string mMessageToSend = "Connected to " + mIP + " through port " + mPort.ToString();

                //Creates a buffer (byte array) and encodes into Bytes
                int mByteCount = Encoding.ASCII.GetByteCount(mMessageToSend + 1);
                byte[] mSendData = new byte[mByteCount];
                mSendData = Encoding.ASCII.GetBytes(mMessageToSend);
              
                //Writes to the data server
                mStream = mClient.GetStream();
                mStream.Write(mSendData, 0, mSendData.Length);
             
                //Reads from the server
                mSR = new StreamReader(mStream);
                string mResponse = mSR.ReadLine();
               

            }
            catch (Exception e)
            {
                //loops back to try the connection again.
                MessageBox.Show("Cannot connect to server.");
                return;
            }
        }

        //****************************************************************************************************************************************
        //Destructor closes stream freeing the memory
        ~CommunicationClient()
        {
            mStream.Close();
            mClient.Close();
        }

        //****************************************************************************************************************************************
        //MessageSender: Sends a message to the class instantiated server and receives a response
        //<mIP>: String representing the IP address of the connection to be made. The default 127.0.0.1 is the local loop address (itself)
        //<mPort>: integer representing the TCP port to be used to communicate with the server
        //<mMessage>: integer array of data to send.
        //</int[]>: returns server response
        public string MessageSender(string mIP, int mPort, string mMessage)
        {
        connection:
            try
            {
             
                //recreating the TCP client because it doesn't persist for some reason....
                mClient = new TcpClient(mIP, mPort);

                //Creates a buffer (byte array) and encodes int array into Bytes
                int mByteCount = mMessage.Length * sizeof(int);
                byte[] mSendData = new byte[mByteCount];
                mSendData = Encoding.ASCII.GetBytes(mMessage);


                //Writes data to the server
                mStream = mClient.GetStream();
                mStream.Write(mSendData, 0, mSendData.Length);

                //Reads data from the server
                StreamReader sr = new StreamReader(mStream);
                string response = sr.ReadLine();

                //returns integer response
                return response;
            }
            catch (Exception e)
            {
                //returns a single element array with 0 value on connection failure

                return "";
            }
        }

        //Class variables for client and network stream
        private BinaryReader mBReader;
        private NetworkStream mStream;
        private StreamReader mSR;
        private TcpClient mClient;
    }
}
