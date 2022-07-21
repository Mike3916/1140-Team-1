using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Controller_1._02
{
    [Serializable]
    public class Controller
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

        public int[] SendCTC(int[] mPacket)
        {
            if(mHardPLC == false)
            {
             
                mPacket = mADS.CTCSend(mPacket);
             
            }
            else
            {
               //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        public int[] SendTrack(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.TrackSend(mPacket);
                
            }
            else
            {
                //What to communicate if there is a hardware controller
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
