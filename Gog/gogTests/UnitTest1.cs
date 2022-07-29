using GogNS;
using TrainController;

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
        public void TrainController_ToggleLeftDoors()
        {
            TrainController.Controller train = new TrainController.Controller();
            Assert.AreEqual(train.mLeftDoorsStatus, false); // closed by default
            train.setLeftDoors();
            Assert.AreEqual(train.mLeftDoorsStatus, true);  // open
            train.setLeftDoors();
            Assert.AreEqual(train.mLeftDoorsStatus, false); // closed
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
                Assert.AreEqual(occupancies[i],returns[i]);
            }

            for (int i = 0; i < returns.Length; i++)
            {
                occupancies[i] = 1;
            }

            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreNotEqual(occupancies[i], returns[i]);
            }

            RedlinePLC.SendOccupancies(occupancies);
            returns = RedlinePLC.ReceiveOccupancies(77);

            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(occupancies[i],returns[i]);
            }


        }

        [TestMethod]
        public void TrackReceiveOccupanciesGreen()
        {
            Track_Controller_1._02.Controller GreenLinePLC = new Track_Controller_1._02.Controller(4, true, "127.0.0.1");

            int[] occupancies = new int[151];
            int[] returns = new int[151];

            GreenLinePLC.SendOccupancies(occupancies);
            returns = GreenLinePLC.ReceiveOccupancies(151);

            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(occupancies[i],returns[i]);
            }

            for (int i = 0; i < returns.Length; i++)
            {
                occupancies[i] = 1;
            }

            GreenLinePLC.SendOccupancies(occupancies);
            returns = GreenLinePLC.ReceiveOccupancies(151);

            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(occupancies[i],returns[i]);
            }


        }

    }
}