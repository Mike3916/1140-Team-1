﻿using System;
using System.Collections.Generic;
using System.Text;

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


        int mnumSections;
        public int mnumBlocks;
        string mnameLine;
        public List<Section> mSections;
    }
}
