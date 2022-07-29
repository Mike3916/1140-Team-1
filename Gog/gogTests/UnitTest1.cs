using GogNS;

namespace gogTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TrackReceiveOccupancies()
        {
            Track_Controller_1._02.Controller RedlinePLC = new Track_Controller_1._02.Controller(851, false, "127.0.0.1");

            int[] occupancies = new int[77];
            int[] returns = new int[77];

            RedlinePLC.SendOccupancies(occupancies);
            returns = RedlinePLC.ReceiveOccupancies(77);

            for(int i = 0; i < returns.Length; i++)
            {
                Assert(occupancies[i] == returns[i]);
            }

            for (int i = 0; i < returns.Length; i++)
            {
                occupancies[i] = 1;
            }

            RedlinePLC.SendOccupancies(occupancies);
            returns = RedlinePLC.ReceiveOccupancies(77);

            for (int i = 0; i < returns.Length; i++)
            {
                Assert(occupancies[i] == returns[i]);
            }


        }



    }
}