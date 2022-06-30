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
            mspeedLimit = Convert.ToDouble(blockInfo[5]);
            mInfrastructure = blockInfo[6];
            mstationSide = blockInfo[7];
            mElevation = Convert.ToDouble(blockInfo[8]);
            mcumElevation = Convert.ToDouble(blockInfo[9]);

            mblockInfo = blockInfo;

            mtrackTemp = 32;

            readInfrastructure();
        }

        //getters
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

        public double getmElevation()
        {
            return mElevation;
        }

        public double getmcumElevation()
        {
            return mcumElevation;
        }
        public List<int> getmblockSwitch()
        {
            return mblockSwitches;
        }

        public bool getmOccupied()
        {
            return mOccupied;
        }
        public bool getmtrackRail()
        {
            return mtrackRail;
        }
        public bool getmtrackCircuit()
        {
            return mtrackCircuit;
        }
        public bool getmPower()
        {
            return mPower;
        }
        public double getmtrackTemp()
        {
            return mtrackTemp;
        }
        
        //setters
        public void setmLength(double info)
        {
            mLength = info;
            mblockInfo[3] = info + "";
        }

        public void setmGrade(double info)
        {
            mGrade = info;
            mblockInfo[4] = info + "";
        }

        public void setmspeedLimit(double info)
        {
            mspeedLimit = info;
            mblockInfo[5] = info + "";
        }

        public void setmElevation(double info)
        {
            double diff = Math.Abs(mElevation - info);
            if (mElevation > info)
            {
                mcumElevation -= diff;
            }
            else
            {
                mcumElevation += diff;
            }
            mElevation = info;
            mblockInfo[8] = mElevation + "";
            mblockInfo[9] = mcumElevation + "";
        }

        //takes the elevation change as input
        public void UpdateCumElevation(double dif)
        {
            mcumElevation += dif;
            mblockInfo[9] = mcumElevation + "";
        }
        public void setmOccupied(bool info)
        {
            mOccupied = info;
        }
        public void setmtrackRail(bool state)
        {
            mtrackRail = state;
        }
        public void setmtrackCircuit(bool state)
        {
            mtrackCircuit = state;
        }
        public void setmPower(bool state)
        {
            mPower = state;
        }
        public void setNextBlock(int nextBlockNum)
        {
            this.mnextBlockNum = nextBlockNum;
        }
        public void setmtrackTemp(double info)
        {
            mtrackTemp = info;
        }
        public void setmswitchPos(int pos)
        {
            mswitchPos = pos;
        }
        
        //reads infrastructure data
        private void readInfrastructure()
        {
            string[] infraString = mInfrastructure.Split(';');

            foreach (string partInfra in infraString)
            {
                if (partInfra.ToLower().Contains("switch"))
                {
                    string[] switches = partInfra.Substring(8, partInfra.IndexOf(')') - 8).Split(':');

                    foreach(string sw in switches)
                    {
                        string[] connections = sw.Split('-');
                        if (Int32.Parse(connections[0]) != mblockNum)
                        {
                            AddSwitch(Int32.Parse(connections[0]));
                        }
                        else
                        {
                            AddSwitch(Int32.Parse(connections[1]));
                        }
                    }
                }
            }
        }

        //via readInfrastructure adds switches to block obj
        void AddSwitch(int blockNum)
        {
            mblockSwitches.Add(blockNum);

            if (mblockSwitches.Count == 1)
                setNextBlock(blockNum);
        }

        string mlineName;
        string msectionName;
        int mblockNum;
        int mswitchPos;
        int mnextBlockNum; 
        double mLength;         //param = 0
        double mGrade;          //param = 1
        double mspeedLimit;     //param = 2
        string mInfrastructure;
        string mstationSide;
        double mElevation;      //param = 3
        double mcumElevation;   //should NOT be mutable
        double mtrackTemp;

        public bool mOccupied;         //param = 0
        bool mtrackRail;        //param = 1
        bool mtrackCircuit;     //param = 2
        bool mPower;            //param = 3

        string[] mblockInfo;
        List<int> mblockSwitches = new List<int>();

    }
}
