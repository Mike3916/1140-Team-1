using System.Collections.Generic;

namespace TrackModel_v0._1
{
    internal class Line
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

        public void addSection(string[] lineInfo)
        {
            for (int i = 0; i < lineInfo.Length; i++)
            {
                string[] blockInfo = lineInfo[i].Split(',');
                string newSectName = blockInfo[1];

                int sectionIDX = -1;
                for (int j = 0; j < mSections.Count; j++)
                {
                    if (mSections[j].getmnameSection() == newSectName)
                    {
                        sectionIDX = j;
                        break;
                    }
                }

                if (sectionIDX == -1)
                {
                    Section newSection = new Section(newSectName);
                    newSection.addBlock(blockInfo);
                    mSections.Add(newSection);
                    mnumSections++;
                }
                else
                {
                    mSections[sectionIDX].addBlock(blockInfo);
                }
            }
        }

        int mnumSections;
        int mnumBlocks;
        string mnameLine;
        List<Section> mSections;
    }
}
