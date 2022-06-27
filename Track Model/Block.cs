using System;
using System.Collections.Generic;
using System.Text;

namespace TrackModel_v0._1
{
    class Block
    {
        public Block()
        {
            mOccupied = false;
        }
        public Block(string[] blockInfo)
        {
            mOccupied = false;

            mlineName = blockInfo[0];
            msectionName = blockInfo[1];
            mblockNum = Int32.Parse(blockInfo[2]);
            mLength = Convert.ToDouble(blockInfo[3]);
            mGrade = Convert.ToDouble(blockInfo[4]);
            mspeedLimit = Int32.Parse(blockInfo[5]);
            mInfrastructure = blockInfo[6];
            mstationSide = blockInfo[7];
            mElevation = Convert.ToDouble(blockInfo[8]);
            mcumElevation = Convert.ToDouble(blockInfo[9]);

            mblockInfo = blockInfo;
        }

        int getNextBlock()
        {
            return 0;  
        }

        public string[] getmblockInfo()
        {
            return mblockInfo;
        }

        public int getmblockNum()
        {
            return mblockNum;
        }

        private void readInfrastructure()
        {
            //if (mInfrastructure.Contains("switch"))
        }

        string mlineName;
        string msectionName;
        int mblockNum;
        double mLength;
        double mGrade;
        int mspeedLimit;
        string mInfrastructure;
        string mstationSide;
        double mElevation;
        double mcumElevation;

        bool mOccupied;

        string[] mblockInfo;
    }
}
