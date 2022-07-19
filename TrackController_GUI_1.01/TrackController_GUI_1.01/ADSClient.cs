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
    internal class ADSClient
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
        public int[] SendPacket(int[] mPacket )
        {

            hvar = ads.CreateVariableHandle("GVL.mInputArray");
            ads.WriteAny(hvar, mPacket);
            hvar = ads.CreateVariableHandle("GVL.mOutputArray");
            mPacket = (int[])ads.ReadAny(hvar, typeof(int[]), new int[] { 75 });

            return mPacket;
        }

       
    }
}
