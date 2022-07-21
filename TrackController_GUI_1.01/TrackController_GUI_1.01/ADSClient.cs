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

        //CTCSend: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] CTCSend(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInFromCTC", "GVL.mOutToCTC");
            return temp;
        }

        //TrackSend: Calls the sendpacket function with specified variable names to read/write
        //<mPacket>: integer array of values to send to PLC
        //<int[]>: integer array of values returned from the PLC
        public int[] TrackSend(int[] mPacket)
        {
            int[] temp = SendPacket(mPacket, "GVL.mInFromTrack", "GVL.mOutToTrack");
            return temp;
        }

    }
}
