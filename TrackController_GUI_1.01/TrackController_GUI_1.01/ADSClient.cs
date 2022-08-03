using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwinCAT.Ads;


namespace Track_Controller_1._02
{
    public class ADSClient
    {

        private TcAdsClient mAds = new TcAdsClient();
        private int mHvar;
        private int mPort;

        public ADSClient(int LocalPort)
        {
            mPort = LocalPort;
           mAds.Connect(mPort);//default port of TwinCAT 3 is 851
            if (mAds.IsConnected == true)
            {
                Console.WriteLine(mPort.ToString() + " Connection OK");
            }
            else
            {
                Console.WriteLine(mPort.ToString() + " Not connected");
            }
        }

        //SendPacket: writes an integer array to an array of global integers at ADS port the client is connected to. 
        //<[]packet>: contains the block states to be written to the PLC
        public void SendPacket(int[] packet, string varName1)
        {
            
            try
            {
                mHvar = mAds.CreateVariableHandle(varName1);
            }
            catch
            {
                Console.WriteLine("You are attempting to connect to a server that does not exist. Please check your server port number.");
            }

            try
            {
                mAds.WriteAny(mHvar, packet);
            }
            catch
            {
                Console.WriteLine("Failed to send. Check that Port " + mPort.ToString() + " is connected and the array size matches at the sender and receiver.");
            }
            return;
        }

        //ReceivePacket: reads an integer array to an array of global integers at ADS port the client is connected to. 
        //<[]packet>: contains the block states to be written to the PLC
        //<int[]> returns the block command states to be written to the track
        public int[] ReceivePacket(int length, string varName2)
        {
            try
            {
            mHvar = mAds.CreateVariableHandle(varName2);
            int[] Packet = (int[])mAds.ReadAny(mHvar, typeof(int[]), new int[] { length });;
            return Packet;
            }
            catch
            {
                Console.WriteLine("You are attempting to connect to a server that does not exist. Please check your server port number.");
                int[] temp = new int[0];
                return temp;
            }
           
        }



        //SendSwitches: Calls the sendpacket function with specified variable names to read/write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendSwitches(int[] packet)
        {
            SendPacket(packet, "GVL.mInSwitch");
            return;
        }

        //ReceiveSwitches: Calls the receivepacket function with specified variable names to read/write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveSwitches(int length = 0)
        {
            int[] temp = ReceivePacket(length, "GVL.mOutSwitch");
            return temp;
        }

        //SendOccupancies: Calls the sendpacket function with specified variable names to read/write
        //<packet>: integer array of values to send to PLC
        public void SendOccupancies(int[] packet)
        {
            SendPacket(packet, "GVL.mInOccupancies");
            return;
        }

        //ReceiveOccupancies: Calls the receivepacket function with specified variable names to read
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveOccupancies(int length = 0)
        {
            int[] temp = ReceivePacket(length, "GVL.mOutOccupancies");
            return temp;
        }

        //SendSpeeds: Calls the sendpacket function with specified variable names to write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendSpeeds(int[] packet)
        {
            SendPacket(packet, "GVL.mInSpeeds");
            return;
        }

        //ReceiveSpeeds: Calls the receive packet function with specified variable names to read
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveSpeeds(int length = 0)
        {
            int[] temp = ReceivePacket(length, "GVL.mOutSpeeds");
            return temp;
        }

        //SendAuthorities: Calls the sendpacket function with specified variable names to write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendAuthorities(int[] packet)
        {
            SendPacket(packet, "GVL.mInAuthorities");
            return;
        }

        //ReceiveAuthorities: Calls the sendpacket function with specified variable names to read
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveAuthorities(int length = 0)
        {
            int[] temp = ReceivePacket(length, "GVL.mOutAuthorities");
            return temp;
        }

        //SendCrossings: Calls the sendpacket function with specified variable names to write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendCrossings(int[] packet)
        {
            SendPacket(packet, "GVL.mInCrossings");
            return;
        }

        //ReceiveCrossings: Calls the sendpacket function with specified variable names to read/write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveCrossings(int length = 0)
        {
            int[] temp = ReceivePacket(length, "GVL.mOutCrossings");
            return temp;
        }

        //SendLeftLights: Calls the sendpacket function with specified variable names to write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendLeftLights(int[] packet)
        {
            SendPacket(packet, "GVL.mInLeftLights");
            return;
        }

        //ReceiveLeftLights: Calls the sendpacket function with specified variable names to read
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveLeftLights(int length = 0)
        {
            int[] temp = ReceivePacket(length, "GVL.mOutLeftLights");
            return temp;
        }


        //SendRightLights: Calls the sendpacket function with specified variable names to read/write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendRightLights(int[] packet)
        {
            SendPacket(packet, "GVL.mInRightLights");
            return;
        }

        //ReceiveRightLights: Calls the sendpacket function with specified variable names to read/write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveRightLights(int length = 0)
        {
            int[] temp = ReceivePacket(length, "GVL.mOutRightLights");
            return temp;
        }

        //SendMaintenance: Calls the sendpacket function with specified variable names to read/write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendMaintenance(int[] packet)
        {
            SendPacket(packet, "GVL.mInMaintenance");
            return;
        }

        //ReceiveMaintenance: Calls the sendpacket function with specified variable names to read/write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveMaintenance(int length = 0)
        {
            int[] temp = ReceivePacket(length, "GVL.mOutMaintenance");
            return temp;
        }

        //SendRoute: Calls the sendpacket function with specified variable names to write
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendRoute(int[] packet)
        {
            SendPacket(packet, "GVL.mInRoute");
            return;
        }

        //ReceiveRoute: Calls the sendpacket function with specified variable names to read
        //<packet>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveRoute(int length = 0)
        {
            int[] packet = ReceivePacket(length, "GVL.mOutRoute");
            return packet;
        }
    }
}
