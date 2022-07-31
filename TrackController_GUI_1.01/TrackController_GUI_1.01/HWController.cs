using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;
//using System.Windows.Threading;

namespace Track_Controller_1._02
{
    class HWController
    {
        public HWController(int mNewPort = 4, int mBaud = 115200)
        {
            mPi = new SerialPort();

            //Setup serial port information
            mPi.PortName = "COM" + mNewPort.ToString();
            mPi.BaudRate = mBaud;
            mPi.Parity = Parity.None;
            mPi.DataBits = 8;
            mPi.StopBits = StopBits.One;
            mPi.Handshake = Handshake.None;
            mPi.WriteTimeout = 500;


            //Open serial port and reset all stored data
            mPi.Open();
            mPi.WriteLine("?");
        }

        //public int[] TrackSend(int[] mPacket)
        //{
        //    string mSentMessage = "";
        //    for (int i = 0; i < mPacket.Length; i++)
        //    {
        //        mSentMessage = mSentMessage + mPacket[i].ToString();
        //    }

        //    mPi.WriteLine(mSentMessage + "\n");
        //    string mReceivedMessage = "";
        //    while (mReceivedMessage == "")
        //    {
        //        mReceivedMessage = mPi.ReadLine();
        //    }
        //    MessageBox.Show(mReceivedMessage);
        //    // create an array with size as string
        //    // length and initialize with 0
        //    int[] temp = new int[mReceivedMessage.Length];


        //    // Traverse the string
        //    for (int i = 0; mReceivedMessage[i] != '\0'; i++)
        //    {
        //        // subtract str[i] by 48 to convert it to int
        //        // Generate number by multiplying 10 and adding
        //        // (int)(str[i])
        //        temp[i] = temp[i] * 10 + (mReceivedMessage[i] - 48);
        //    }
        //    return temp;
        //}

        //public void SendSwitches(int[] mPacket)
        //{
        //    //TODO
        //}

        //public int[] ReceiveSwitches(int mLength)
        //{
        //    //TODO
            
        //}

        //public void SendOccupancies(int[] mPacket)
        //{
        //    //TODO
        //    string mSentMessage = "";
        //    for (int i = 0; i < mPacket.Length; i++)
        //    {
        //        mSentMessage = mSentMessage + mPacket[i].ToString();
        //    }

        //    mPi.WriteLine(mSentMessage + "\n");
        //    string mReceivedMessage = "";
        //    while (mReceivedMessage == "")
        //    {
        //        mReceivedMessage = mPi.ReadLine();
        //    }
        //    MessageBox.Show(mReceivedMessage);
        //    // create an array with size as string
        //    // length and initialize with 0
        //    int[] temp = new int[mReceivedMessage.Length];


        //    // Traverse the string
        //    for (int i = 0; mReceivedMessage[i] != '\0'; i++)
        //    {
        //        // subtract str[i] by 48 to convert it to int
        //        // Generate number by multiplying 10 and adding
        //        // (int)(str[i])
        //        temp[i] = temp[i] * 10 + (mReceivedMessage[i] - 48);
        //    }
        //}

        //public int[] ReceiveOccupancies(int mLength)
        //{
        //    //todo
        //}

        //public void SendSpeeds(int[] mPacket)
        //{
        //    //todo
        //}

        //public int[] ReceiveSpeeds(int mLength)
        //{
        //    //todo
        //}

        //public void SendAuthorities(int[] mPacket)
        //{
        //    //todo
        //}

        //public int[] ReceiveAuthorities(int mLength)
        //{
        //    //todo
        //}

        //public void SendCrossings(int[] mPacket)
        //{
        //    //todo
        //}

        //public int[] ReceiveCrossings(int mLength)
        //{
        //    //todo
        //}

        //public void SendLeftLights(int[] mPacket)
        //{
        //    //todo
        //}

        //public int[] ReceiveLeftLights(int mLength)
        //{
        //    //todo
        //}

        //public void SendRightLights(int[] mPacket)
        //{
        //    //todo
        //}

        //public int[] ReceiveRightLights(int mLength)
        //{
        //    //todo
        //}

        //public void SendMaintenance(int[] mPacket)
        //{
        //    //todo
        //}

        //public int[] ReceiveMaintenance(int mLength)
        //{
        //    //todo
        //}

        //public void SendRoute(int[] mPacket)
        //{
        //    //todo
        //}

        //public int[] ReceiveRoute(int mLength)
        //{
        //    //todo
        //}


        private SerialPort mPi;
        private int[] mOccupancies;
        private int[] mSwitches;
        private int[] mRightLights;
        private int[] mLeftLights;
        private int[] mCrossings;

    }
}
