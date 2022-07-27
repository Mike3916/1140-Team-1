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
        //Constructor: Creates a new controller object that holds an ADS client, a Port number, and a boolean indicating the type of PLC.
        //<mNewPort>: integer holding the port number of the PLC to be connected to.
        //<mPLCSelect>: Boolean value indicating soft or hard PLC
        //<mNewIP>: IP address of the new controller. Default is the loop IP.
        public Controller(int mNewPort = 851, bool mPLCSelect = false, string mNewIP="127.0.0.1")
        {
            if(mPLCSelect == false)
            {
                mHardPLC = false;
                mPort = mNewPort;
                mIP = mNewIP;
                mADS = new ADSClient(mPort);
                //mTCP = new CommunicationClient(mIP, mPort);
            }
            else
            {
                mHardPLC = true;
                mPort = mNewPort;
                mIP = mNewIP;
                mTCP = new CommunicationClient(mIP, mPort);
                mADS = new ADSClient(mPort);
            }
        }

        //SendSwitches: Calls the client class to send an integer array holding the switch positions.
        //<[]mPacket>: integer holding switch positions
        //<void>
        public void SendSwitches(int[] mPacket)
        {
            if(mHardPLC == false)
            {
             
                mADS.SendSwitches(mPacket);
             
            }
            else
            {
               //What to communicate if there is a hardware controller.
            }
            return;
        }

        //ReceiveSwitches: Calls the client class to receive an array of switch positions of length[] mPacket. Failing to specify[] mPacket will return a 0 element array.
        //<mLength>: length of return array. This is when the client stops reading.
        //<int[]>: array holding switch positions
        public int[] ReceiveSwitches(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.ReceiveSwitches(mPacket);

            }
            else
            {
       
                //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        //SendOccupancies: Calls the client class to send an integer array holding the occupancies.
        //<[]mPacket>: integer array holding occupancies
        //<void>
        public void SendOccupancies(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mADS.SendOccupancies(mPacket);
                
            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return;
        }

        //ReceiveOccupancies: Calls the client class to receive an array of occupancies of length[] mPacket. Failing to specify[] mPacket will return a 0 element array.
        //<mLength>: length of return array. This is when the client stops reading.
        //<int[]>: array holding occupancies
        public int[] ReceiveOccupancies(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.ReceiveOccupancies(mPacket);

            }
            else
            {
        
                //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        //SendSpeeds: Calls the client class to send an integer array holding the occupancies.
        //<[]mPacket>: integer array holding occupancies
        //<void>
        public void SendSpeeds(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mADS.SendSpeeds(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return;
        }

        //ReceiveSpeeds: Calls the client class to receive an array of speeds of length[] mPacket. Failing to specify[] mPacket will return a 0 element array.
        //<mLength>: length of return array. This is when the client stops reading.
        //<int[]>: array holding speeds
        public int[] ReceiveSpeeds(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.ReceiveSpeeds(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        //SendAuthorities: Calls the client class to send an integer array holding the authorities.
        //<[]mPacket>: integer array holding occupancies
        //<void>
        public void SendAuthorities(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mADS.SendAuthorities(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return;
        }

        //ReceiveAuthorities: Calls the client class to receive an array of authorities of length[] mPacket. Failing to specify[] mPacket will return a 0 element array.
        //<mLength>: length of return array. This is when the client stops reading.
        //<int[]>: array holding authorities
        public int[] ReceiveAuthorities(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.ReceiveAuthorities(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        //SendCrossings: Calls the client class to send an integer array holding the crossing states.
        //<[]mPacket>: integer array holding occupancies
        //<void>
        public void SendCrossings(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mADS.SendCrossings(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return;
        }

        //ReceiveCrossings: Calls the client class to receive an array of crossings of length[] mPacket. Failing to specify[] mPacket will return a 0 element array.
        //<mLength>: length of return array. This is when the client stops reading.
        //<int[]>: array holding authorities
        public int[] ReceiveCrossings(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.ReceiveCrossings(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        //SendLeftLights: Calls the client class to send an integer array holding the left transit light states.
        //<[]mPacket>: integer array holding occupancies
        //<void>
        public void SendLeftLights(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mADS.SendLeftLights(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return;
        }

        //ReceiveLeftLights: Calls the client class to receive an array of left light states of length[] mPacket. Failing to specify[] mPacket will return a 0 element array.
        //<mLength>: length of return array. This is when the client stops reading.
        //<int[]>: array holding left light states
        public int[] ReceiveLeftLights(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.ReceiveLeftLights(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        //SendRightLights: Calls the client class to send an integer array holding the right transit light states.
        //<[]mPacket>: integer array holding occupancies
        //<void>
        public void SendRightLights(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mADS.SendRightLights(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return;
        }

        //ReceiveRightLights: Calls the client class to receive an array of right light states of length[] mPacket. Failing to specify[] mPacket will return a 0 element array.
        //<mLength>: length of return array. This is when the client stops reading.
        //<int[]>: array holding left right states
        public int[] ReceiveRightLights(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.ReceiveRightLights(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        //SendMaintenance: Calls the client class to send an integer array holding the maintenance states.
        //<[]mPacket>: integer array holding occupancies
        //<void>
        public void SendMaintenance(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mADS.SendMaintenance(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return;
        }

        //ReceiveMaintenance: Calls the client class to receive an array of maintenance states of length[] mPacket. Failing to specify[] mPacket will return a 0 element array.
        //<mLength>: length of return array. This is when the client stops reading.
        //<int[]>: array holding maintenance states
        public int[] ReceiveMaintenance(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.ReceiveMaintenance(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        //SendRoute: Calls the client class to send an integer array holding the route.
        //<[]mPacket>: integer array holding occupancies
        //<void>
        public void SendRoute(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mADS.SendRoute(mPacket);
            }
            else
            {
                //What to communicate if there is a hardware controller
            }
            return;
        }

        //ReceiveMaintenance: Calls the client class to receive an array of maintenance states of length[] mPacket. Failing to specify[] mPacket will return a 0 element array.
        //<mLength>: length of return array. This is when the client stops reading.
        //<int[]>: array holding maintenance states
        public int[] ReceiveRoute(int[] mPacket)
        {
            if (mHardPLC == false)
            {

                mPacket = mADS.ReceiveRoute(mPacket);

            }
            else
            {
                //What to communicate if there is a hardware controller.
            }
            return mPacket;
        }

        private int mRouteLength;
        private int mPort;
        private ADSClient mADS;
        private CommunicationClient mTCP;
        private bool mHardPLC = false;
        private string mIP;

    }
}
