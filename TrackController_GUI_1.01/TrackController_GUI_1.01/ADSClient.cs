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

        //SendPacket: writes an integer array to an array of global integers at ADS port the client is connected to. Reads the outputs from that 
        //port as an array of integers.
        //<[]mPacket>: contains the block states to be written to the PLC
        //<int[]> returns the block command states to be written to the track
        public int[] SendPacket(int[] mPacket, string mVarName1, string mVarName2 )
        {

            hvar = ads.CreateVariableHandle(mVarName1);
            ads.WriteAny(hvar, mPacket);
            hvar = ads.CreateVariableHandle(mVarName2);
            mPacket = (int[])ads.ReadAny(hvar, typeof(int[]), new int[] { mPacket.Length });

            return mPacket;
        }

        //SendSwitches: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] SendSwitches(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInSwitch", "GVL.mOutSwitch");
            return temp;
        }

        //SendOccupancies: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] SendOccupancies(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInOccupancies", "GVL.mOutOccupancies");
            return temp;
        }

        //SendSpeeds: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] SendSpeeds(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInSpeeds", "GVL.mOutSpeeds");
            return temp;
        }

        //SendAuthorities: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] SendAuthorities(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInAuthorities", "GVL.mOutAuthorities");
            return temp;
        }

        //SendCrossings: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] SendCrossings(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInCrossings", "GVL.mOutCrossings");
            return temp;
        }

        //SendLeftLights: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] SendLeftLights(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInLeftLights", "GVL.mOutLeftLights");
            return temp;
        }
        

        //SendRightLights: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] SendRightLights(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInRightLights", "GVL.mOutRightLights");
            return temp;
        }
        
        //SendMaintenance: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] SendMaintenance(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInMaintenance", "GVL.mOutMaintenance");
            return temp;
        }
    }
}
