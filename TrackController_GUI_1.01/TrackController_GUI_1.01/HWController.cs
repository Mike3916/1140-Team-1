﻿using System;
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
            mSwitches = new int[151];
            mSpeeds = new int[151];
            mAuthorities = new int[151];
            mRightLights = new int[151];
            mLeftLights = new int[151];
            mMaintenance = new int[151];
            
        //Open serial port and reset all stored data
        mPi.Open();
            //mPi.WriteLine("?");
        }

        public void SendSwitches(int[] mPacket)
        {
            //TODO
            mSwitches = mPacket;
            mSwUpToDate = true;
        }

        public int[] ReceiveSwitches(int mLength)
        {
            //TODO
            if (!mSwUpToDate)
            {
                run();
            }
            
            
            return mSwitches;
            
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
            mReceivedMessage = new byte[12];
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
            mPi.Read(mReceivedMessage, 0, 12);
            


            // Traverse the string
            for (int i = 0; i < 86; i++)
            {
                if ((mReceivedMessage[i / 8] & (1<<(i % 8))) != 0)
                {
                    mRightLights[i + 57] = 1;
                    mLeftLights[i + 57] = 1;
                }
                else
                {
                    mRightLights[i + 57] = 0;
                    mLeftLights[i + 57] = 0;
                }
            }


            mSwitches[61] = ((mReceivedMessage[86 / 8] & (1 << (86 % 8))) != 0) ? 62 : 150;
            mSwitches[76] = ((mReceivedMessage[87 / 8] & (1 << (87 % 8))) != 0) ? 100 : 75;
            mSwitches[84] = ((mReceivedMessage[88 / 8] & (1 << (88 % 8))) != 0) ? 85 : 99;


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
        private bool mSwUpToDate;
        private bool mRLUpToDate;
        private bool mLLUpToDate;
        private bool mCrUpToDate;
        private byte[] mSentMessage;
        private byte[] mReceivedMessage;
    }
}
