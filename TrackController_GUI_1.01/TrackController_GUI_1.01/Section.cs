using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Controller_1._02
{
    public class Section
    {
        //Constructor
        public Section()
        {
            mBlocks = new List<Block>();
        }

        //constructor ovverride newName
        public Section(string newName)
        {
            mnameSection = newName;
            mBlocks = new List<Block>();
        }

        //Joe's accessor and mutator methods
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

            foreach (Block block in mBlocks)
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
        //end Joe's accessor and mutator functions

        //**********************************************************************************************************************************
        //Mike's Accessor and Mutator functions for the block subclass
      
        public int getmBlockState(int blockIdx)
        {
            try
            {
                return mBlocks[blockIdx].getmBlockState();
            }
            catch { return 0; }
        }

        public void setmBlockState(int blockIdx, int mNewState)
        {
            mBlocks[blockIdx].setmBlockState(mNewState);
        }

        public int getmSpeedLimit(int blockIdx)
        {
            try
            {
                return mBlocks[blockIdx].getmSpeedLimit();
            }
            catch { return 0; }
            
        }

        public void setmSpeedLimit(int blockIdx, int mNewSpeed)
        {
            mBlocks[blockIdx].setmSpeedLimit(mNewSpeed);
        }

        public void setAuthority(int blockIdx, int mNewAuthority)
        {
            mBlocks[blockIdx].setAuthority(mNewAuthority);
        }

        public int getAuthority(int blockIdx)
        {
            return mBlocks[blockIdx].getAuthority();
        }
        public void setSuggested(int blockIdx, int mNewSuggested)
        {
            mBlocks[blockIdx].setSuggested(mNewSuggested);
        }

        public int getSuggested(int blockIdx)
        {
            return mBlocks[blockIdx].getSuggested();
        }


        //*****************************************************************************************************************************************
        //End Mike's Accessors and Mutators


        int mnumBlocks;
        string mnameSection;
        List<Block> mBlocks;
    }
}
