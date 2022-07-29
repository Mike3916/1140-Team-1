using GogNS;
using Train=TrainObject.Train;
using TrainController;
using TrackModel;

namespace gogTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void maxPowerLimited()
        {
            Train chooChoo = new Train(35, 1);
            chooChoo.setPowerCmd(500000000);
            Assert.AreEqual(120000, chooChoo.getPowerCmd(), "Account not debited correctly");
            
        }


        [TestMethod]
        public void TrainController_ToggleLeftDoors()
        {
            TrainController.Controller train = new TrainController.Controller();
            Assert.AreEqual(train.mLeftDoorsStatus, true); // closed by default
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

        //[TestMethod]
        //public void TrackReceiveOccupanciesGreen()
        //{
        //    Track_Controller_1._02.Controller GreenLinePLC = new Track_Controller_1._02.Controller(4, true, "127.0.0.1");

        //    int[] occupancies = new int[151];
        //    int[] returns = new int[151];

        //    GreenLinePLC.SendOccupancies(occupancies);
        //    returns = GreenLinePLC.ReceiveOccupancies(151);

        //    for (int i = 0; i < returns.Length; i++)
        //    {
        //        Assert.AreEqual(occupancies[i],returns[i]);
        //    }

        //    for (int i = 0; i < returns.Length; i++)
        //    {
        //        occupancies[i] = 1;
        //    }
            for (int i = 0; i < returns.Length; i++)
            {
                occupancies[i] = 1;
                Assert.AreEqual(occupancies[i],returns[i]);
            }

        //    GreenLinePLC.SendOccupancies(occupancies);
        //    returns = GreenLinePLC.ReceiveOccupancies(151);

        //    for (int i = 0; i < returns.Length; i++)
        //    {
        //        Assert.AreEqual(occupancies[i],returns[i]);
        //    }


        //}
        }
        [TestMethod]
        public void TrackModel_AddTrain()
        {
            int blockIdx = 0;
            int lineIdx = 0;
            int authority = 0;

            TrackModel.MainWindow track = new TrackModel.MainWindow();
            track.AddTrain(blockIdx, lineIdx, authority);

            Assert.AreEqual(track.mtrainList.Count, 1);
            Assert.AreEqual(track.mtrainList[0].blockIdx, 0);
            Assert.AreEqual(track.mtrainList[0].lineIdx, 0);
            Assert.AreEqual(track.mtrainList[0].commAuthority, 0);
        }
    }
}