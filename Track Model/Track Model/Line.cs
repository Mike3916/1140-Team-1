using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TrackModel
{
    public class Line
    {
        public Line()
        {
            mnumSections = 0;
            mSections = new List<Section>();
        }
        public Line(string newName)
        {
            mnameLine = newName;
            mnumSections = 0;
            mSections = new List<Section>();

            List<int> redRoute = mredRoute.ToList();
            List<int> greenRoute = mgreenRoute.ToList();

            mRoute.Add(redRoute);
            mRoute.Add(greenRoute);
        }

       


        //getters

        public int GetmnumSections()
        {
            return mnumSections;
        }

        public int GetmnumBlocks()
        {
            int blockSum = 0;
            foreach (Section sect in mSections)
            {
                blockSum += sect.getmnumBlocks();
            }
            mnumBlocks = blockSum;
            return mnumBlocks;
        }

        public string GetmnameLine()
        {
            return mnameLine;
        }

        public List<string[]> GetlineInfo()
        {
            List<string[]> lineInfo = new List<string[]>();
            for (int sectIdx = 0; sectIdx < GetmnumSections(); sectIdx++)
            {
                for (int blockIdx = 0; blockIdx < mSections[sectIdx].getmnumBlocks(); blockIdx++)
                {
                    lineInfo.Add(mSections[sectIdx].getmblockInfo(blockIdx));
                }
            }

            return lineInfo;
        }

        public string[] GetBlockInfo(int sectIdx, int blockIdx)
        {
            return mSections[sectIdx].getmblockInfo(blockIdx);
        }
        public List<int> GetmblockSwitch(int sectIdx, int blockIdx)
        {
            return mSections[sectIdx].getmblockSwitch(blockIdx);
        }

        //gets to 1-index block with ID blockNum
        public TrackModel.Block GetBlock(int blockNum)
        {
            for (int idx = 0; idx < mnumSections; idx++)
            {
                for (int jdx = 0; jdx < mSections[idx].getmnumBlocks(); jdx++)
                {
                    if (blockNum == this.mSections[idx].mBlocks[jdx].mblockNum)
                        return this.mSections[idx].mBlocks[jdx];
                }
            }
            return null;
        }

        public void SetBlock(TrackModel.Block bl)
        {
            int blockNum = bl.mblockNum;
            for (int idx = 0; idx < mnumSections; idx++)
            {
                for (int jdx = 0; jdx < mSections[idx].getmnumBlocks(); jdx++)
                {
                    if (blockNum == this.mSections[idx].mBlocks[jdx].mblockNum)
                        this.mSections[idx].mBlocks[jdx] = bl;
                }
            }
        }
        public void SetBlock(TrackModel.Block bl, int blockNum)
        {
            for (int idx = 0; idx < mnumSections; idx++)
            {
                for (int jdx = 0; jdx < mSections[idx].getmnumBlocks(); jdx++)
                {
                    if (blockNum == this.mSections[idx].mBlocks[jdx].mblockNum)
                        this.mSections[idx].mBlocks[jdx] = bl;
                }
            }
        }
        public List<string> GetSectionNames()
        {
            List<string> sectNames = new List<string>();
            foreach(Section sect in mSections)
            {
                sectNames.Add(sect.getmnameSection());
            }
            return sectNames;
        }

        public List<string> GetSectBlockNum(int sectIdx)
        {
            return mSections[sectIdx].getBlockNum();
        }

        public void SetmnameLine(string newName)
        {
            mnameLine = newName;
        }
        public void UpdateSignal()
        {
            for (int blockNum = 1; blockNum <= GetmnumBlocks(); blockNum++)
            {
                TrackModel.Block bl = GetBlock(blockNum);
                if (bl.mOccupied && bl.mblockNum > 2)
                {
                    TrackModel.Block newYellow = GetBlock(blockNum - 2);
                    newYellow.mSignal = "Yellow";
                    SetBlock(newYellow);

                    TrackModel.Block newRed = GetBlock(blockNum - 1);
                    newRed.mSignal = "Red";
                    SetBlock(newRed);
                }
            }
        }
        //setters
        //sets info of param with type double
        public void SetBlockInfo(int sectIdx, int blockIdx, int param, double info)
        {
            mSections[sectIdx].setBlockInfo(blockIdx, param, info);
        }
        //sets info of param with type bool
        public void SetBlockInfo(int sectIdx, int blockIdx, int param, bool info)
        {
            mSections[sectIdx].setBlockInfo(blockIdx, param, info);
        }
        //if elevation is changed, call this
        public void UpdateCumElevation(int sectIdx, int blockIdx, double dif)
        {

            for (int idx = 0; idx < mSections.Count; idx++)
            {
                for (int jdx = 0; jdx < mSections[idx].getmnumBlocks(); jdx++)
                {
                    if (sectIdx == idx && blockIdx == jdx) //if it's the block we just updated, don't even bother.
                        continue;
                    mSections[sectIdx].mBlocks[idx].UpdateCumElevation(dif);
                }
            }
        }

        //adds beacons 3 blocks ahead of station en route
        public void SetBeacon()
        {
            //add beacons to Block obj
            List<int> Stations = new List<int>();
            for (int i = 1; i <= mnumBlocks; i++)
            {
                if (GetBlock(i).mStation)
                    Stations.Add(i);
            }

            for (int i = 0; i < Stations.Count; i++)
            {
                List<int> route = new List<int>();
                if (mnameLine == "Red")
                    route = mRoute[0];
                else
                    route = mRoute[1];

                int rIdx = route.IndexOf(Stations[i]);

                TrackModel.Block newBeacon = GetBlock(route[rIdx-3]);
                newBeacon.mBeacon = "Station 3 blocks Ahead!";
                SetBlock(newBeacon);

                newBeacon = GetBlock(route[rIdx - 2]);
                newBeacon.mBeacon = "Station 2 blocks Ahead!";
                SetBlock(newBeacon);

                newBeacon = GetBlock(route[rIdx - 1]);
                newBeacon.mBeacon = "Station 1 blocks Ahead!";
                SetBlock(newBeacon);

                newBeacon = GetBlock(route[rIdx]);
                newBeacon.mBeacon = "Station: " + newBeacon.mstationName;
                SetBlock(newBeacon);
            }
        }

        //adds a section to the line
        public void AddSections(string[] lineInfo)
        {
            //foreach block entry in lineInfo:
            //  1.check if the block's section exists:
            //      if yes -> add block to section
            //      else -> add a section w/ the block in it

            for (int i = 0; i < lineInfo.Length; i++)
            {
                string[] blockInfo = lineInfo[i].Split(',');
                string newSectName = blockInfo[1];

                //gets Idx of section
                int sectIdx = -1;
                for (int j = 0; j < mSections.Count; j++)
                {
                    if (mSections[j].getmnameSection() == newSectName)
                    {
                        sectIdx = j;
                        break;
                    }
                }

                if (sectIdx == -1) //a section with Idx = -1 does not exist
                {
                    Section newSection = new Section(newSectName);
                    newSection.addBlock(blockInfo);
                    mSections.Add(newSection);
                    mnumSections++;
                }
                else //if the section exists
                {
                    mSections[sectIdx].addBlock(blockInfo);
                }
            }
        }

        public bool OccupyBlock(int blockIdx)
        {
            for (int i = 0; i < this.GetmnumSections(); i++)
            {
                for (int j = 0; j < this.mSections[i].getmnumBlocks(); j++)
                {
                    if (this.mSections[i].mBlocks[j].mblockNum == blockIdx)
                    {
                        if (this.mSections[i].mBlocks[j].mOccupied == true)
                            return false; //failed to occupy
                        else if (this.mSections[i].mBlocks[j].mOccupied == false)
                        {
                            this.mSections[i].mBlocks[j].mOccupied = true;
                            return this.mSections[i].mBlocks[j].mOccupied; //successful occupy
                        }
                    }
                }
            }

            return false; //failed to find 
        }

        public bool UnOccupyBlock(int blockIdx)
        {
            for (int i = 0; i < this.GetmnumSections(); i++)
            {
                for (int j = 0; j < this.mSections[i].getmnumBlocks(); j++)
                {
                    if (this.mSections[i].mBlocks[j].mblockNum == blockIdx)
                    {
                        if (this.mSections[i].mBlocks[j].mOccupied == false)
                            return false; //failed to unoccupy
                        else if (this.mSections[i].mBlocks[j].mOccupied == true)
                        {
                            this.mSections[i].mBlocks[j].mOccupied = false;
                            return this.mSections[i].mBlocks[j].mOccupied; //successful unoccupy
                        }
                    }
                }
            }

            return false; //failed to find 
        }

        int[] mredRoute = {77, 9, 8, 7, 6, 5, 4, 3, 2, 1, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 76, 75,
                                74, 73, 72, 33, 34, 35, 36, 37, 38, 71, 70, 69, 68, 67, 44, 45, 46, 47, 48, 49, 50, 51,
                                52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 52, 51, 50, 49, 48, 47, 46,
                                45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24,
                                23, 22, 21, 20, 19, 18, 17, 16, 1, 2, 3, 4, 5, 6, 7, 8, 9, 77 };

        int[] mgreenRoute = {151, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
                                82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 85, 84,
                                83, 82, 81, 80, 79, 78, 77, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111,
                                112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128,
                                129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145,
                                146, 147, 148, 149, 150, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15,
                                14, 13, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21,
                                22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42,
                                43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 151 };

       

        int mnumSections;
        public int mnumBlocks;
        string mnameLine;
        public List<Section> mSections;
        List<List<int>> mRoute = new List<List<int>>();
    }
}
