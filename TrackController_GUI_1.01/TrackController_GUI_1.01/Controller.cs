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
            }
            else
            {
                mHardPLC = true;
                mPort = mNewPort;
                mIP = mNewIP;
                mHWC = new HWController(mNewPort,230400);
            }
        }

        //SendSwitches: Calls the client class to send an integer array holding the switch positions.
        //<[]packet>: integer holding switch positions
        //<void>
        public void SendSwitches(int[] packet)
        {
            if(mHardPLC == false)
            {
             
                mADS.SendSwitches(packet);
             
            }
            else
            {
               //What to communicate if there is a hardware controller.
               mHWC.SendSwitches(packet);
            }
            return;
        }

        //ReceiveSwitches: Calls the client class to receive an array of switch positions of length[] packet. Failing to specify[] packet will return a 0 element array.
        //<length>: length of return array. This is when the client stops reading.
        //<int[]>: array holding switch positions
        public int[] ReceiveSwitches(int length = 0)
        {
            int[] packet = new int[length];
            if (mHardPLC == false)
            {

                packet = mADS.ReceiveSwitches(length);

            }
            else
            {
                packet = mHWC.ReceiveSwitches(length);

                //What to communicate if there is a hardware controller.
            }
            return packet;
        }

        //SendOccupancies: Calls the client class to send an integer array holding the occupancies.
        //<[]packet>: integer array holding occupancies
        //<void>
        public void SendOccupancies(int[] packet)
        {
            if (mHardPLC == false)
            {

                mADS.SendOccupancies(packet);
                
            }
            else
            {
                //What to communicate if there is a hardware controller
                mHWC.SendOccupancies(packet);
            }
            return;
        }

        //ReceiveOccupancies: Calls the client class to receive an array of occupancies of length[] packet. Failing to specify[] packet will return a 0 element array.
        //<length>: length of return array. This is when the client stops reading.
        //<int[]>: array holding occupancies
        public int[] ReceiveOccupancies(int length = 0)
        {
            int[] packet = new int[length];
            if (mHardPLC == false)
            {

                packet = mADS.ReceiveOccupancies(length);

            }
            else
            {
                packet = mHWC.ReceiveOccupancies(length);
                //What to communicate if there is a hardware controller.
            }
            return packet;
        }

        //SendSpeeds: Calls the client class to send an integer array holding the occupancies.
        //<[]packet>: integer array holding occupancies
        //<void>
        public void SendSpeeds(int[] packet)
        {
            if (mHardPLC == false)
            {

                mADS.SendSpeeds(packet);

            }
            else
            {
                //What to communicate if there is a hardware controller
                mHWC.SendSpeeds(packet);
            }
            return;
        }

        //ReceiveSpeeds: Calls the client class to receive an array of speeds of length[] packet. Failing to specify[] packet will return a 0 element array.
        //<length>: length of return array. This is when the client stops reading.
        //<int[]>: array holding speeds
        public int[] ReceiveSpeeds(int length = 0)
        {
            int[] packet = new int[length];
            if (mHardPLC == false)
            {

                packet = mADS.ReceiveSpeeds(length);

            }
            else
            {
                packet = mHWC.ReceiveSpeeds(length);
                //What to communicate if there is a hardware controller.
            }
            return packet;
        }

        //SendAuthorities: Calls the client class to send an integer array holding the authorities.
        //<[]packet>: integer array holding occupancies
        //<void>
        public void SendAuthorities(int[] packet)
        {
            if (mHardPLC == false)
            {

                mADS.SendAuthorities(packet);

            }
            else
            {
                //What to communicate if there is a hardware controller
                mHWC.SendAuthorities(packet);
            }
            return;
        }

        //ReceiveAuthorities: Calls the client class to receive an array of authorities of length[] packet. Failing to specify[] packet will return a 0 element array.
        //<length>: length of return array. This is when the client stops reading.
        //<int[]>: array holding authorities
        public int[] ReceiveAuthorities(int length = 0)
        {
            int[] packet = new int[length];
            if (mHardPLC == false)
            {

                packet = mADS.ReceiveAuthorities(length);

            }
            else
            {
                packet = mHWC.ReceiveAuthorities(length);
                //What to communicate if there is a hardware controller.
            }
            return packet;
        }

        //SendCrossings: Calls the client class to send an integer array holding the crossing states.
        //<[]packet>: integer array holding occupancies
        //<void>
        public void SendCrossings(int[] packet)
        {
            if (mHardPLC == false)
            {

                mADS.SendCrossings(packet);

            }
            else
            {
                //What to communicate if there is a hardware controller
                mHWC.SendCrossings(packet);
            }
            return;
        }

        //ReceiveCrossings: Calls the client class to receive an array of crossings of length[] packet. Failing to specify[] packet will return a 0 element array.
        //<length>: length of return array. This is when the client stops reading.
        //<int[]>: array holding authorities
        public int[] ReceiveCrossings(int length = 0)
        {
            int[] packet = new int[length];
            if (mHardPLC == false)
            {

                packet = mADS.ReceiveCrossings(length);

            }
            else
            {
                packet = mHWC.ReceiveCrossings(length);
                //What to communicate if there is a hardware controller.
            }
            return packet;
        }

        //SendLeftLights: Calls the client class to send an integer array holding the left transit light states.
        //<[]packet>: integer array holding occupancies
        //<void>
        public void SendLeftLights(int[] packet)
        {
            if (mHardPLC == false)
            {

                mADS.SendLeftLights(packet);

            }
            else
            {
                //What to communicate if there is a hardware controller
                mHWC.SendLeftLights(packet);
            }
            return;
        }

        //ReceiveLeftLights: Calls the client class to receive an array of left light states of length[] packet. Failing to specify[] packet will return a 0 element array.
        //<length>: length of return array. This is when the client stops reading.
        //<int[]>: array holding left light states
        public int[] ReceiveLeftLights(int length = 0)
        {
            int[] packet = new int[length];
            if (mHardPLC == false)
            {

                packet = mADS.ReceiveLeftLights(length);

            }
            else
            {
                packet = mHWC.ReceiveLeftLights(length);
                //What to communicate if there is a hardware controller.
            }
            return packet;
        }

        //SendRightLights: Calls the client class to send an integer array holding the right transit light states.
        //<[]packet>: integer array holding occupancies
        //<void>
        public void SendRightLights(int[] packet)
        {
            if (mHardPLC == false)
            {

                mADS.SendRightLights(packet);

            }
            else
            {
                //What to communicate if there is a hardware controller
                mHWC.SendRightLights(packet);
            }
            return;
        }

        //ReceiveRightLights: Calls the client class to receive an array of right light states of length[] packet. Failing to specify[] packet will return a 0 element array.
        //<length>: length of return array. This is when the client stops reading.
        //<int[]>: array holding left right states
        public int[] ReceiveRightLights(int length = 0)
        {
            int[] packet = new int[length];
            if (mHardPLC == false)
            {

                packet = mADS.ReceiveRightLights(length);

            }
            else
            {
                packet = mHWC.ReceiveRightLights(length);
                //What to communicate if there is a hardware controller.
            }
            return packet;
        }

        //SendMaintenance: Calls the client class to send an integer array holding the maintenance states.
        //<[]packet>: integer array holding occupancies
        //<void>
        public void SendMaintenance(int[] packet)
        {
            if (mHardPLC == false)
            {

                mADS.SendMaintenance(packet);

            }
            else
            {
                //What to communicate if there is a hardware controller
                mHWC.SendMaintenance(packet);
            }
            return;
        }

        //ReceiveMaintenance: Calls the client class to receive an array of maintenance states of length[] packet. Failing to specify[] packet will return a 0 element array.
        //<length>: length of return array. This is when the client stops reading.
        //<int[]>: array holding maintenance states
        public int[] ReceiveMaintenance(int length = 0)
        {
            int[] packet = new int[length];
            if (mHardPLC == false)
            {

                packet = mADS.ReceiveMaintenance(length);

            }
            else
            {
                packet = mHWC.ReceiveMaintenance(length);
                //What to communicate if there is a hardware controller.
            }
            return packet;
        }

        //SendRoute: Calls the client class to send an integer array holding the route.
        //<[]packet>: integer array holding occupancies
        //<void>
        public void SendRoute(int[] packet)
        {
            if (mHardPLC == false)
            {

                mADS.SendRoute(packet);
            }
            else
            {
                //What to communicate if there is a hardware controller
                mHWC.SendRoute(packet);
            }
            return;
        }

        //ReceiveMaintenance: Calls the client class to receive an array of maintenance states of length[] packet. Failing to specify[] packet will return a 0 element array.
        //<length>: length of return array. This is when the client stops reading.
        //<int[]>: array holding maintenance states
        public int[] ReceiveRoute(int length = 0)
        {
            int[] packet = new int[length];
            if (mHardPLC == false)
            {

                packet = mADS.ReceiveRoute(length);

            }
            else
            {
                packet = mHWC.ReceiveRoute(length);
                //What to communicate if there is a hardware controller.
            }
            return packet;
        }

        private int mRouteLength;
        private int mPort;
        private ADSClient mADS;
        private HWController mHWC;
        private bool mHardPLC = false;
        private string mIP;

    }
}
