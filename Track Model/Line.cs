using System;
using System.Collections.Generic;
using System.Text;

namespace TrackModel_v0._1
{
    internal class Line
    {
        public Line()
        {
            mnumSections = 0;
            mSections = new List<Section>();
            BuildRoute();
        }
        public Line(string newName)
        {
            mnameLine = newName;
            mnumSections = 0;
            mSections = new List<Section>();
            BuildRoute();
        }

        public void BuildRoute()
        {
            routeB.Push(9);
            routeB.Push(8);
            routeB.Push(7);
            routeB.Push(6);
            routeB.Push(5);
            routeB.Push(4);
            routeB.Push(3);
            routeB.Push(2);
            routeB.Push(1);
            routeB.Push(0);

            routeC.Push(14);
            routeC.Push(13);
            routeC.Push(12);
            routeC.Push(11);
            routeC.Push(10);
            routeC.Push(4);
            routeC.Push(3);
            routeC.Push(2);
            routeC.Push(1);
            routeC.Push(0);
        }

        //getters

        public int getmnumSections()
        {
            return mnumSections;
        }

        public int getmnumBlocks()
        {
            int blockSum = 0;
            foreach (Section sect in mSections)
            {
                blockSum += sect.getmnumBlocks();
            }
            mnumBlocks = blockSum;
            return mnumBlocks;
        }

        public string getmnameLine()
        {
            return mnameLine;
        }

        public List<string[]> getlineInfo()
        {
            List<string[]> lineInfo = new List<string[]>();
            for (int sectIdx = 0; sectIdx < getmnumSections(); sectIdx++)
            {
                for (int blockIdx = 0; blockIdx < mSections[sectIdx].getmnumBlocks(); blockIdx++)
                {
                    lineInfo.Add(mSections[sectIdx].getmblockInfo(blockIdx));
                }
            }

            return lineInfo;
        }

        public string[] getBlockInfo(int sectIdx, int blockIdx)
        {
            return mSections[sectIdx].getmblockInfo(blockIdx);
        }
        public List<int> getmblockSwitch(int sectIdx, int blockIdx)
        {
            return mSections[sectIdx].getmblockSwitch(blockIdx);
        }

        public List<string> getSectionNames()
        {
            List<string> sectNames = new List<string>();
            foreach(Section sect in mSections)
            {
                sectNames.Add(sect.getmnameSection());
            }
            return sectNames;
        }

        public List<string> getSectBlockNum(int sectIdx)
        {
            return mSections[sectIdx].getBlockNum();
        }

        public void setmnameLine(string newName)
        {
            mnameLine = newName;
        }

        //setters
        //sets info of param with type double
        public void setBlockInfo(int sectIdx, int blockIdx, int param, double info)
        {
            mSections[sectIdx].setBlockInfo(blockIdx, param, info);
        }
        //sets info of param with type bool
        public void setBlockInfo(int sectIdx, int blockIdx, int param, bool info)
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

        public int MoveTrain(int authority, double speed, int destination)
        {
            if (destination == 0)
            {
                return routeB.Pop();
            }
                //mSections[0].mBlocks[4].set
            else
            {
                return routeC.Pop();
            }
        }

        //adds a section to the line
        public void addSections(string[] lineInfo)
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

        Stack<int> routeB = new Stack<int>();
        Stack<int> routeC = new Stack<int>();
        int mnumSections;
        int mnumBlocks;
        string mnameLine;
        public List<Section> mSections;
    }
}
