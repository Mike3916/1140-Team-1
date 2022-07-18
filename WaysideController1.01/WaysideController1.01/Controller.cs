using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Controller_1._02
{
    [Serializable]
    internal class Controller
    {
        public Controller(List<Line> mLines = null, int mReference = 0)
        {
          
        }

        private int[] mSections;
        private int[] mBlocks;
        private int mTotalBlocks;
    }
}
