using System;
using System.Collections.Generic;
using System.Text;

namespace TrackModel
{
    public class Block
    {
        public Block()
        {
            mOccupied = false;
        }
        public Block(string[] blockInfo)
        {
            mOccupied = false;
            mhasCross = false;
            mcrossDown = false;
            mUnderground = false;
            mLeft = false;
            mRight = false;
            
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

            mstationName = "";
            mStation = false;

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
        public bool toggleCrossbar()
        {
            return mcrossDown = !mcrossDown;
        }
        public bool crossbarDown()
        {
            return mcrossDown;
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

                    foreach (string sw in switches)
                    {
                        string[] connections = sw.Split('-');

                        if (Int32.Parse(connections[0]) != mblockNum)
                            AddSwitch(Int32.Parse(connections[0]));
                        else
                            AddSwitch(Int32.Parse(connections[1]));
                    }
                }
                else if (partInfra.ToLower().Contains("station"))
                {
                    mstationName = infraString[1];
                    Random r = new Random();
                    mPop = r.Next(0, 222); //randomly initializes station between 1 and 222
                    mStation = true;
                }

                else if (partInfra.ToLower().Contains("railway crossing"))
                    mhasCross = true;

                else if (partInfra.ToLower().Contains("railway crossing"))
                    mUnderground = true;
            }
            if (mstationSide.ToLower().Contains("left"))
                mLeft = true;

            else if (mstationSide.ToLower().Contains("right"))
                mRight = true;
        }

        //via readInfrastructure adds switches to block obj
        void AddSwitch(int blockNum)
        {
            mblockSwitches.Add(blockNum);

            if (mblockSwitches.Count == 1)
                setNextBlock(blockNum);
        }

        void PopulateStation()
        {

        }

        public string mlineName;
        public string msectionName;
        public int mblockNum;
        public string mstationName;
        public int mswitchPos;
        public int mnextBlockNum; 
        public double mLength;         //param = 0
        public double mGrade;          //param = 1
        public double mspeedLimit;     //param = 2
        string mInfrastructure;
        public  string mstationSide;
        public double mElevation;      //param = 3
        public double mcumElevation;   //should NOT be mutable
        public double mtrackTemp;

        public bool mOccupied;         //param = 0
        public bool mtrackRail;        //param = 1
        public bool mtrackCircuit;     //param = 2
        public bool mPower;            //param = 3
        public bool mStation;
        public bool mhasCross;
        public bool mcrossDown;
        public bool mUnderground;
        public bool mLeft;
        public bool mRight;

        public int mPop;

        public string[] mblockInfo;
        public List<int> mblockSwitches = new List<int>();

    }
}
