using System;
using System.Collections.Generic;
using System.Text;

namespace TrackModel_v0._1
{
    internal class Section
    {
        public Section()
        {
            mBlocks = new List<Block>();
        }
        public Section(string newName)
        {
            mnameSection = newName;
            mBlocks = new List<Block>();
        }

        //getters
        public int getmnumBlocks()
        {
            return mnumBlocks;
        }
        public string getmnameSection()
        {
            return mnameSection;
        }
        public string[] getmblockInfo(int blockIdx)
        {
            return mBlocks[blockIdx].getmblockInfo();
        }
        public List<string> getBlockNum()
        {
            List<string> blockNum = new List<string>();

            foreach(Block block in mBlocks)
            {
                blockNum.Add("" + block.getmblockNum());
            }    
            return blockNum;
        }
        public List<int> getmblockSwitch(int blockIdx)
        {
            return mBlocks[blockIdx].getmblockSwitch();
        }

        //setters
        public void setmnameSection(string newName)
        {
            mnameSection = newName;
        }

        //gets Idx of block to change, the index of the parameter to change, and the new info
        //for param with double dataypes
        public void setBlockInfo(int blockIdx, int param, double info)
        {
            switch (param)
            {
                case 0:         //length
                    mBlocks[blockIdx].setmLength(info);
                    break;
                case 1:         //grade
                    mBlocks[blockIdx].setmGrade(info);
                    break;
                case 2:         //speed limit
                    mBlocks[blockIdx].setmspeedLimit(info);
                    break;
                case 3:         //elevation
                    mBlocks[blockIdx].setmElevation(info);
                    break;
                default:
                    break;
            }
        }
        //for param with bool dataypes
        public void setBlockInfo(int blockIdx, int param, bool info)
        {
            switch (param)
            {
                case 0:         //occupied
                    mBlocks[blockIdx].setmOccupied(info);
                    break;
                case 1:         //track rail
                    mBlocks[blockIdx].setmtrackRail(info);
                    break;
                case 2:         //track circuit
                    mBlocks[blockIdx].setmtrackCircuit(info);
                    break;
                case 3:         //power
                    mBlocks[blockIdx].setmPower(info);
                    break;
                default:
                    break;
            }
        }  

         //                                                                 //
        //  there are many answers to this problem, but this one is mine.  //
       //                                                                 //

        //add a block to the section
        public void addBlock(string[] blockInfo)
        {
            Block newBlock = new Block(blockInfo);
            mBlocks.Add(newBlock);
            mnumBlocks++;
        }

        

        int mnumBlocks;
        string mnameSection;
        List<Block> mBlocks;
    }
}
