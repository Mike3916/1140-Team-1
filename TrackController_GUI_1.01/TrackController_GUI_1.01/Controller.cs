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

        public int[] SendSwitches(int[] mPacket)
        {
            if(mHardPLC == false)
            {
             
                mPacket = mADS.SendSwitches(mPacket);
             
            }
            else
            {
               //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        public int[] SendOccupancies(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.SendOccupancies(mPacket);
                
            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return mPacket;
        }

        public int[] SendSpeeds(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.SendSpeeds(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return mPacket;
        }

        public int[] SendAuthorities(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.SendAuthorities(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return mPacket;
        }

        public int[] SendCrossings(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.SendCrossings(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return mPacket;
        }

        public int[] SendLeftLights(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.SendLeftLights(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return mPacket;
        }

        public int[] SendRightLights(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.SendRightLights(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return mPacket;
        }

        public int[] SendMaintenance(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.SendMaintenance(mPacket);

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
