using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Controller_1._02
{
    class Block
    {
        public Block()
        {
            mBlockState = 0;
        }
        public Block(string[] blockInfo)
        {

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

            if (mInfrastructure.ToUpper().Contains("SWITCH"))
                mBlockState = mBlockState | 0x00000002;
            if (mInfrastructure.ToUpper().Contains("STATION"))
                mBlockState = mBlockState | 0x00000004;
            if (mInfrastructure.ToUpper().Contains("CROSSING"))
                mBlockState = mBlockState | 0x00000040;

            mAuthority = 0;
            /*
            0x00000002 = contains switch on siding
            0x00000003 = contains switch on main
            0x00000004 = contains station
            0x00000040 = contains crossing, all open
            0x00000060 = contains crossing, bar closed
            0x00000050 = contains crossing, lights on
            0x00100000 = transit light left is on
            0x00010000 = transit light right is on
            0x00000400 = track is broken
            0x00000800 = no track signal
            0x00001000 = block occupied
            0x00002000 = CTC has locked out
            0x00004000 = Wayside controller has locked out
            */
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

        public int getmBlockState()
        {
            return mBlockState;
        }

        public void setmBlockState(int mNewState)
        {
            mBlockState = mNewState;
        }

        public int getmSpeedLimit()
        {
            return mspeedLimit;
        }

        public void setmSpeedLimit(int mNewSpeed)
        {
            mspeedLimit = mNewSpeed;
        }

        public void setAuthority(int mNewAuthority)
        {
            mAuthority = mNewAuthority;
        }

        public int getAuthority()
        {
            int mCurrentAuthority = mAuthority;
            mAuthority = 0;
            return mCurrentAuthority;
        }
        public void setSuggested(int mNewSuggested)
        {
            mSuggestedSpeed = mNewSuggested;
        }

        public int getSuggested()
        {
            int mLocalSuggested = mSuggestedSpeed;
            mSuggestedSpeed = 0;
            return mLocalSuggested;
        }

        string mlineName;
        string msectionName;
        string mInfrastructure;
        string mstationSide;

        int mBlockState;
        int mAuthority = 0;
        int mSuggestedSpeed = 0;
        int mspeedLimit;
        int mblockNum;

        double mLength;
        double mGrade;
        double mElevation;
        double mcumElevation;

        string[] mblockInfo;
    }

}
