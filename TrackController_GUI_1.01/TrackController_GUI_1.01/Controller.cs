using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Controller_1._02
{
    [Serializable]
    internal class Controller
    {
        public Controller(int mNewPort = 851, bool mPLCSelect = false, string mNewIP="127.0.0.1")
        {
            if(mPLCSelect == false)
            {
                mHardPLC = false;
                mPort = mNewPort;
                mADS = new ADSClient(mPort);
            }
            else
            {
                mHardPLC = true;
                mPort = mNewPort;
                mIP = mNewIP;
                mTCP = new CommunicationClient(mIP, mPort);

            }
        }

        public int[] SendPacket(int[] mPacket)
        {
            if(mHardPLC == false)
            {
                MessageBox.Show("Sending Software");
                string string1 = "";

                mPacket = mADS.SendPacket(mPacket);

                foreach (int i in mPacket)
                {
                    string1 += mPacket[i].ToString();
                }
                MessageBox.Show(string1);
            }
            else
            {
                MessageBox.Show("Sending Hardware");
                mPacket = mTCP.MessageSender(mIP,mPort,mPacket);
            }
            return mPacket;
        }


        private int mPort;
        private ADSClient mADS;
        private CommunicationClient mTCP;
        private bool mHardPLC = false;
        private string mIP;

    }
}
