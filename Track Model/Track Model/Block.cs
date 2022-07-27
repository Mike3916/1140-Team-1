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
            mOccupied = false; //is occupied
            mhasCross = false; //has rail crossing
            mcrossDown = false; //rail crossing is down
            mUnderground = false; //is underground
            mLeft = false; //has station on left
            mRight = false; //has station on right

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

            mtrackRail = true;
            mtrackCircuit = true;
            mPower = true;

            mblockInfo = blockInfo;

            mtrackTemp = 32;

            mstationName = "";
            mStation = false; // has station

            readInfrastructure();
        }

        //getters
        int GetNextBlock()
        {
            return 0;  
        }

        public string[] GetmblockInfo()
        {
            return mblockInfo;
        }

        public int GetmblockNum()
        {
            return mblockNum;
        }

        public double GetmElevation()
        {
            return mElevation;
        }

        public double GetmcumElevation()
        {
            return mcumElevation;
        }
        public List<int> GetmblockSwitch()
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
            if (mtrackRail == false)
                mOccupied = true;
            else
                mOccupied = false;
        }
        public void setmtrackCircuit(bool state)
        {
            mtrackCircuit = state;
            if (mtrackCircuit == false)
                mOccupied = true;
            else
                mOccupied = false;
        }
        public void setmPower(bool state)
        {
            mPower = state;
            if (mPower == false)
                mOccupied = true;
            else
                mOccupied = false;
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

        public bool mOccupied;         //param = 0 is occupied
        public bool mtrackRail;        //param = 1 track rail working
        public bool mtrackCircuit;     //param = 2 track circuit working
        public bool mPower;            //param = 3 power working
        public bool mStation;   //has station
        public bool mhasCross;  //has rail crossing
        public bool mcrossDown; //crossbar down
        public bool mUnderground;//is underground
        public bool mLeft;      //station on left
        public bool mRight;     //station on right

        public int mPop;        //population at station

        public string[] mblockInfo;
        public List<int> mblockSwitches = new List<int>();

    }
}
