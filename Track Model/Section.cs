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
        public void setmnameSection(string newName)
        {
            mnameSection = newName;
        }
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
