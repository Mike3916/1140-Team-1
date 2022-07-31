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

            mOccupancies = new int[151];
            mSwitches = new int[3];
            mSpeeds = new int[151];
            mAuthorities = new int[151];
            mRightLights = new int[151];
            mLeftLights = new int[151];
            mMaintenance = new int[151];
            
        //Open serial port and reset all stored data
        mPi.Open();
            //mPi.WriteLine("?");
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

        public void SendSwitches(int[] mPacket)
        {
            //TODO
            mSwitches = mPacket;
            mSwUpToDate = true;
        }

        public int[] ReceiveSwitches(int mLength)
        {
            //TODO
            if (mSwUpToDate)
            {
                return mSwitches;
            }
            else
            {
                run();
                return mSwitches;
            }
        }

        public void SendOccupancies(int[] mPacket)
        {
            //TODO
            mOccupancies = mPacket;
            mSwUpToDate = false;
            mRLUpToDate = false;
            mLLUpToDate = false;
            mCrUpToDate = false;
        }

        public int[] ReceiveOccupancies(int mLength)
        {
            //todo
            return mOccupancies; 
        }

        public void SendSpeeds(int[] mPacket)
        {
            //todo
            mSpeeds = mPacket;
        }

        public int[] ReceiveSpeeds(int mLength)
        {
            //todo
            return mSpeeds;
        }

        public void SendAuthorities(int[] mPacket)
        {
            //todo
            mAuthorities = mPacket;
        }

        public int[] ReceiveAuthorities(int mLength)
        {
            //todo
            return mAuthorities;
        }

        public void SendCrossings(int[] mPacket)
        {
            //todo
            mCrossings = mPacket;
            mCrUpToDate = true;
        }

        public int[] ReceiveCrossings(int mLength)
        {
            //todo
            if (mCrUpToDate)
            {
                return mCrossings;
            }
            else
            {
                run();
                return mCrossings;
            }
        }

        public void SendLeftLights(int[] mPacket)
        {
            //todo
            mLeftLights = mPacket;
            mLLUpToDate = true;
        }

        public int[] ReceiveLeftLights(int mLength)
        {
            //todo
            if (mLLUpToDate)
            {
                return mLeftLights;
            }
            else
            {
                run();
                return mLeftLights;
            }
        }

        public void SendRightLights(int[] mPacket)
        {
            //todo
            mRightLights = mPacket;
            mRLUpToDate = true;
        }

        public int[] ReceiveRightLights(int mLength)
        {
            //todo
            if (mRLUpToDate)
            {
                return mRightLights;
            }
            else
            {
                run();
                return mRightLights;
            }
        }

        public void SendMaintenance(int[] mPacket)
        {
            //todo
            mMaintenance = mPacket;
            mSwUpToDate = false;
            mRLUpToDate = false;
            mLLUpToDate = false;
            mCrUpToDate = false;
        }

        public int[] ReceiveMaintenance(int mLength)
        {
            //todo
            return mMaintenance;
        }

        public void SendRoute(int[] mPacket)
        {
            //todo
            mRoute = mPacket;
        }

        public int[] ReceiveRoute(int mLength)
        {
            //todo
            return mRoute;
        }

        public void run()
        {
            //todo
            mSentMessage = new byte[11];
            mTempSend = new bool[86];
            for (int i = 0; i < 86; i++)
            {
                if (mOccupancies[i + 57] + mMaintenance[i + 57] >= 1)
                {
                    mSentMessage[i / 8] = Convert.ToByte((mSentMessage[i / 8] | (1<<(i % 8))));
                }
            }

            //for (int i = 0; i < mSentMessage.Length; i++)
            //{
            //    mSentMessage[i] = FileStream.ReadByte(mTempSend, 8 * i);
            //}
            mPi.Write(mSentMessage, 0, 11);
            string mReceivedMessage = "";
            while (mReceivedMessage == "")
            {
                mReceivedMessage = mPi.ReadLine();
            }
            Console.WriteLine(mReceivedMessage);
            // create an array with size as string
            // length and initialize with 0
            int[] temp = new int[mReceivedMessage.Length];


            // Traverse the string
            for (int i = 0; mReceivedMessage[i] != '\0'; i++)
            {
                // subtract str[i] by 48 to convert it to int
                // Generate number by multiplying 10 and adding
                // (int)(str[i])
                //todo
                temp[i] = temp[i] * 10 + (mReceivedMessage[i] - 48);
            }

            mSwUpToDate = true;
            mRLUpToDate = true;
            mLLUpToDate = true;
            mCrUpToDate = true;
        }

        private SerialPort mPi;
        private int[] mOccupancies;
        private int[] mSwitches;
        private int[] mSpeeds;
        private int[] mAuthorities;
        private int[] mRightLights;
        private int[] mLeftLights;
        private int[] mMaintenance;
        private int[] mCrossings;
        private int[] mRoute;
        private bool[] mTempSend;
        private bool mSwUpToDate;
        private bool mRLUpToDate;
        private bool mLLUpToDate;
        private bool mCrUpToDate;
        private byte[] mSentMessage;
    }
}
