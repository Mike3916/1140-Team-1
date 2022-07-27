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

        private TcAdsClient ads = new TcAdsClient();
        private int hvar;
        private int[] var = new int[75];
        int mPort;
        private ITcAdsSymbol sym = null;

        public ADSClient(int mLocalPort)
        {
            mPort = mLocalPort;
           ads.Connect(mPort);//default port of TwinCAT 3 is 851
            if (ads.IsConnected == true)
            {
                MessageBox.Show("Connection OK");
            }
            else
            {
                MessageBox.Show("Not connected");
            }
        }

        //SendPacket: writes an integer array to an array of global integers at ADS port the client is connected to. 
        //<[]mPacket>: contains the block states to be written to the PLC
        public void SendPacket(int[] mPacket, string mVarName1)
        {

            hvar = ads.CreateVariableHandle(mVarName1);
            ads.WriteAny(hvar, mPacket);
            
            return;
        }

        //ReceivePacket: reads an integer array to an array of global integers at ADS port the client is connected to. 
        //<[]mPacket>: contains the block states to be written to the PLC
        //<int[]> returns the block command states to be written to the track
        public int[] ReceivePacket(int[] mPacket, string mVarName2)
        {
            MessageBox.Show(mVarName2 + mPacket[0].ToString() + mPacket[1].ToString());
            hvar = ads.CreateVariableHandle(mVarName2);
            mPacket = (int[])ads.ReadAny(hvar, typeof(int[]), new int[] { 4 });

            MessageBox.Show(mVarName2 + mPacket[0].ToString() + mPacket[1].ToString());

            return mPacket;
        }



        //SendSwitches: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendSwitches(int[] mPacket)
        {
            SendPacket(mPacket, "GVL.mInSwitch");
            return;
        }

        //ReceiveSwitches: Calls the receivepacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveSwitches(int[] mPacket)
        {
            int[] temp = ReceivePacket(mPacket, "GVL.mOutSwitch");
            return temp;
        }

        //SendOccupancies: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        public void SendOccupancies(int[] mPacket)
        {
            SendPacket(mPacket, "GVL.mInOccupancies");
            return;
        }

        //ReceiveOccupancies: Calls the receivepacket function with specified variable names to read
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveOccupancies(int[] mPacket)
        {
            int[] temp = ReceivePacket(mPacket, "GVL.mOutOccupancies");
            return temp;
        }

        //SendSpeeds: Calls the sendpacket function with specified variable names to write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendSpeeds(int[] mPacket)
        {
            SendPacket(mPacket, "GVL.mInSpeeds");
            return;
        }

        //ReceiveSpeeds: Calls the receive packet function with specified variable names to read
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveSpeeds(int[] mPacket)
        {
            int[] temp = ReceivePacket(mPacket, "GVL.mOutSpeeds");
            return temp;
        }

        //SendAuthorities: Calls the sendpacket function with specified variable names to write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendAuthorities(int[] mPacket)
        {
            SendPacket(mPacket, "GVL.mInAuthorities");
            return;
        }

        //ReceiveAuthorities: Calls the sendpacket function with specified variable names to read
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveAuthorities(int[] mPacket)
        {
            int[] temp = ReceivePacket(mPacket, "GVL.mOutAuthorities");
            return temp;
        }

        //SendCrossings: Calls the sendpacket function with specified variable names to write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendCrossings(int[] mPacket)
        {
            SendPacket(mPacket, "GVL.mInCrossings");
            return;
        }

        //ReceiveCrossings: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveCrossings(int[] mPacket)
        {
            int[] temp = ReceivePacket(mPacket, "GVL.mOutCrossings");
            return temp;
        }

        //SendLeftLights: Calls the sendpacket function with specified variable names to write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendLeftLights(int[] mPacket)
        {
            SendPacket(mPacket, "GVL.mInLeftLights");
            return;
        }

        //ReceiveLeftLights: Calls the sendpacket function with specified variable names to read
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveLeftLights(int[] mPacket)
        {
            int[] temp = ReceivePacket(mPacket, "GVL.mOutLeftLights");
            return temp;
        }


        //SendRightLights: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendRightLights(int[] mPacket)
        {
            SendPacket(mPacket, "GVL.mInRightLights");
            return;
        }

        //ReceiveRightLights: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveRightLights(int[] mPacket)
        {
            int[] temp = ReceivePacket(mPacket, "GVL.mOutRightLights");
            return temp;
        }

        //SendMaintenance: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendMaintenance(int[] mPacket)
        {
            SendPacket(mPacket, "GVL.mInMaintenance");
            return;
        }

        //ReceiveMaintenance: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveMaintenance(int[] mPacket)
        {
            int[] temp = ReceivePacket(mPacket, "GVL.mOutMaintenance");
            return temp;
        }

        //SendRoute: Calls the sendpacket function with specified variable names to write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public void SendRoute(int[] mPacket)
        {
            SendPacket(mPacket, "GVL.mInRoute");
            return;
        }

        //ReceiveRoute: Calls the sendpacket function with specified variable names to read
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] ReceiveRoute(int[] mPacket)
        {
            mPacket = ReceivePacket(mPacket, "GVL.mOutRoute");
            return mPacket;
        }
    }
}
