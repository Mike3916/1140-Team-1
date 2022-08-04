

namespace gogTests
{
    [TestClass]
    public class UnitTest1
    {
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

        [TestMethod]
        public void maxPowerLimited()
        {
            TrainObject.Train chooChoo = new TrainObject.Train(35, 1);
            chooChoo.setPowerCmd(500000000);
            Assert.AreEqual(120000, chooChoo.getPowerCmd(), "Account not debited correctly");

        }

        [TestMethod]
        //Checks if program will crash when attempting to connect to an unconfigured port number
        public void ConnectToBadPort()
        {
            Track_Controller_1._02.Controller RedlinePLC = new Track_Controller_1._02.Controller(855, false, "127.0.0.1");

            int[] crossings = new int[45];
            int[] returns = new int[45];

            RedlinePLC.SendCrossings(crossings);
            (RedlinePLC.ReceiveCrossings(45)).CopyTo(returns, 0);
        }

        [TestMethod]
        //Test to see if PLC receives and sends the occupancies correctly
        public void TrackControllerReceiveOccupancies()
        {
            Track_Controller_1._02.Controller RedlinePLC = new Track_Controller_1._02.Controller(851, false, "127.0.0.1");

            int[] occupancies = new int[45];
            int[] returns = new int[45];

            //Send and receive zero arrays with wait in between
            RedlinePLC.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC.ReceiveOccupancies(45)).CopyTo(returns, 0);

            //Change values of array
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(occupancies[i], returns[i]);
            }

            //Change occupancies to 1s
            for (int i = 0; i < returns.Length; i++)
            {
                occupancies[i] = 1;
            }

            //check that arrays no longer match
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreNotEqual(occupancies[i], returns[i]);
            }

            //Send. Wait. Receive
            RedlinePLC.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC.ReceiveOccupancies(45)).CopyTo(returns, 0);

            //Check for match
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(occupancies[i], returns[i]);
            }

            //Test mismatch size.
            int[] sizemismatch = new int[99];

            RedlinePLC.SendOccupancies(sizemismatch);
            (RedlinePLC.ReceiveOccupancies(99)).CopyTo(sizemismatch, 0);
            (RedlinePLC.ReceiveOccupancies(45)).CopyTo(sizemismatch, 0);
        }

        [TestMethod]
        //Test to see if PLC receives and sends the speeds correctly
        public void TrackControllerReceiveSpeeds()
        {
            Track_Controller_1._02.Controller RedlinePLC = new Track_Controller_1._02.Controller(851, false, "127.0.0.1");

            int[] speeds = new int[45];
            int[] returns = new int[45];

            //Send and receive zero arrays with wait in between
            RedlinePLC.SendSpeeds(speeds);
            Thread.Sleep(10);
            (RedlinePLC.ReceiveSpeeds(45)).CopyTo(returns, 0);

            //Change values of array
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(speeds[i], returns[i]);
            }

            //Change speeds to 1s
            for (int i = 0; i < returns.Length; i++)
            {
                speeds[i] = 5;
            }

            //check that arrays no longer match
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreNotEqual(speeds[i], returns[i]);
            }

            //Send. Wait. Receive
            RedlinePLC.SendSpeeds(speeds);
            Thread.Sleep(10);
            (RedlinePLC.ReceiveSpeeds(45)).CopyTo(returns, 0);

            //Check for match
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(speeds[i], returns[i]);
            }

            //Set speeds above speed limit
            for (int i = 0; i < returns.Length; i++)
            {
                speeds[i] = 150;
            }

            RedlinePLC.SendSpeeds(speeds);
            Thread.Sleep(10);
            (RedlinePLC.ReceiveSpeeds(45)).CopyTo(returns, 0);

            //Speeds should not match
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreNotEqual(speeds[i], returns[i]);
            }

            //Sample a few test blocks with known speed limits
            Assert.AreEqual(speeds[0], 40);
            Assert.AreEqual(speeds[16], 55);
            Assert.AreEqual(speeds[17], 70);

            //Test mismatch size.
            int[] sizemismatch = new int[99];

            RedlinePLC.SendSpeeds(sizemismatch);
            (RedlinePLC.ReceiveSpeeds(99)).CopyTo(sizemismatch, 0);
            (RedlinePLC.ReceiveSpeeds(45)).CopyTo(sizemismatch, 0);
        }

        [TestMethod]
        //Test to see if PLC receives the authorities correctly
        public void TrackControllerReceiveAuthorities()
        {
            Track_Controller_1._02.Controller RedlinePLC = new Track_Controller_1._02.Controller(851, false, "127.0.0.1");

            int[] authorities = new int[45];
            int[] returns = new int[45];

            //Send and receive zero arrays with wait in between
            RedlinePLC.SendAuthorities(authorities);
            Thread.Sleep(10);
            (RedlinePLC.ReceiveAuthorities(45)).CopyTo(returns, 0);

            //Change values of array
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(authorities[i], returns[i]);
            }

            //Change speeds to 1s
            for (int i = 0; i < returns.Length; i++)
            {
                authorities[i] = i + 1;
            }

            //check that arrays no longer match
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreNotEqual(authorities[i], returns[i]);
            }

            //Send. Wait. Receive
            RedlinePLC.SendAuthorities(authorities);
            Thread.Sleep(10);
            (RedlinePLC.ReceiveAuthorities(45)).CopyTo(returns, 0);

            //Check for match
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(authorities[i], returns[i]);
            }

            //Test mismatch size.
            int[] sizemismatch = new int[99];

            RedlinePLC.SendAuthorities(sizemismatch);
            (RedlinePLC.ReceiveAuthorities(99)).CopyTo(sizemismatch, 0);
            (RedlinePLC.ReceiveAuthorities(45)).CopyTo(sizemismatch, 0);
        }

        [TestMethod]
        //Test to see if PLC changes the crossing value when occupied
        public void TrackControllerChangeCrossing()
        {
            Track_Controller_1._02.Controller RedlinePLC2 = new Track_Controller_1._02.Controller(853, false, "127.0.0.1");

            //arrays for handling send receive values
            int[] crossings = new int[39];
            int[] occupancies = new int[39];
            int[] returns1 = new int[39];
            int[] returns2 = new int[39];

            //Send and receive zero arrays with wait in between
            RedlinePLC2.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC2.ReceiveCrossings(39)).CopyTo(returns1, 0);
            occupancies[14] = 1;
            RedlinePLC2.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC2.ReceiveCrossings(39)).CopyTo(returns2, 0);

            //Check that Crossing changed state.
            Assert.AreNotEqual(returns1[14], returns2[14]);

            //Test mismatch size.
            int[] sizemismatch = new int[99];
            RedlinePLC2.SendCrossings(sizemismatch);
            (RedlinePLC2.ReceiveCrossings(99)).CopyTo(sizemismatch, 0);
            (RedlinePLC2.ReceiveCrossings(45)).CopyTo(sizemismatch, 0);
        }

        [TestMethod]
        //Test to see if switch changes when in maintenance mode and commanded change given
        //Test to see if switch does not change when not in maintenance mode and commanded change given
        //Test to see if PLC code changes the switch value when some blocks occupied
        public void TrackControllerChangeSwitch()
        {
            Track_Controller_1._02.Controller RedlinePLC1 = new Track_Controller_1._02.Controller(851, false, "127.0.0.1");

            //arrays for handling send receive values
            int[] switches = new int[45];
            int[] occupancies = new int[45];
            int[] maintenance = new int[45];
            int[] returns1 = new int[45];
            int[] returns2 = new int[45];

            switches[8] = 99;
            switches[15] = 99;
            switches[26] = 99;
            switches[32] = 99;

            //Not in maintenance mode. Send switch command
            RedlinePLC1.SendMaintenance(maintenance);
            RedlinePLC1.SendSwitches(switches);
            Thread.Sleep(10);
            (RedlinePLC1.ReceiveSwitches(45)).CopyTo(returns1, 0);

            //Check that switch did not change in PLC code
            Assert.AreNotEqual(switches[8], returns1[8]);
            Assert.AreNotEqual(switches[15], returns1[15]);
            Assert.AreNotEqual(switches[26], returns1[26]);
            Assert.AreNotEqual(switches[32], returns1[32]);

            maintenance[8] = 1;
            maintenance[15] = 1;
            maintenance[26] = 1;
            maintenance[32] = 1;

            //In maintenance mode. Send switch command
            RedlinePLC1.SendMaintenance(maintenance);
            RedlinePLC1.SendSwitches(switches);
            Thread.Sleep(10);
            (RedlinePLC1.ReceiveSwitches(45)).CopyTo(returns1, 0);

            //Check that switch did change in PLC
            Assert.AreEqual(switches[8], returns1[8]);
            Assert.AreEqual(switches[15], returns1[15]);
            Assert.AreEqual(switches[26], returns1[26]);
            Assert.AreEqual(switches[32], returns1[32]);


            maintenance[8] = 0;
            maintenance[15] = 0;
            maintenance[26] = 0;
            maintenance[32] = 0;

            //block 76 (yard) to unoccupied (high)
            occupancies[44] = 1;
            //two way track unoccupied
            for (int i = 16; i <= 26; i++)
            {
                occupancies[i] = 0;
            }
            //block 15 to  occupied (high)
            occupancies[14] = 1;
            //block 76 to occupied (high)
            occupancies[43] = 1;
            //two way track unoccupied
            for (int i = 32; i <= 37; i++)
            {
                occupancies[i] = 0;
            }
            //block 33 to unoccupied (high)
            occupancies[31] = 1;

            //Not in maintenance mode. Test switch position with occupancy change
            RedlinePLC1.SendMaintenance(maintenance);
            RedlinePLC1.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC1.ReceiveSwitches(45)).CopyTo(returns1, 0);

            //Checks each switch position
            Assert.AreEqual(returns1[8], 9);
            Assert.AreEqual(returns1[15], 0);
            Assert.AreEqual(returns1[26], 27);
            Assert.AreEqual(returns1[32], 71);

            //branches occupied
            occupancies[44] = 0;
            occupancies[14] = 0;
            occupancies[43] = 0;
            occupancies[31] = 0;

            //Not in maintenance mode. Test switch position with occupancy change
            RedlinePLC1.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC1.ReceiveSwitches(45)).CopyTo(returns1, 0);

            //Checks each switch position
            Assert.AreEqual(returns1[8], 76);
            Assert.AreEqual(returns1[15], 14);
            Assert.AreEqual(returns1[26], 35);
            Assert.AreEqual(returns1[32], 31);

            //Set two way track to unoccupied
            for (int i = 16; i <= 26; i++)
            {
                occupancies[i] = 1;
            }
            for (int i = 32; i <= 37; i++)
            {
                occupancies[i] = 1;
            }

            //Not in maintenance mode. Test switch position with occupancy change
            RedlinePLC1.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC1.ReceiveSwitches(45)).CopyTo(returns1, 0);

            //Checks each switch position
            Assert.AreEqual(returns1[8], 76);
            Assert.AreEqual(returns1[15], 0);
            Assert.AreEqual(returns1[26], 27);
            Assert.AreEqual(returns1[32], 71);


            //Test mismatch size.
            int[] sizemismatch = new int[99];
            RedlinePLC1.SendSwitches(sizemismatch);
            (RedlinePLC1.ReceiveSwitches(99)).CopyTo(sizemismatch, 0);
            (RedlinePLC1.ReceiveSwitches(45)).CopyTo(sizemismatch, 0);
        }

        [TestMethod]
        //Test to see if PLC changes the transit lights
        public void TrackControllerChangeLights()
        {
            Track_Controller_1._02.Controller RedlinePLC1 = new Track_Controller_1._02.Controller(851, false, "127.0.0.1");

            //arrays for handling send receive values
            int[] occupancies = new int[45];
            int[] returns1 = new int[45];
            int[] returns2 = new int[45];

            //Send all blocks occupied and pull light values
            RedlinePLC1.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC1.ReceiveLeftLights(45)).CopyTo(returns1, 0);
            (RedlinePLC1.ReceiveRightLights(45)).CopyTo(returns2, 0);

            //Lights should be logical low when red/occupied
            for (int i = 0; i < occupancies.Length; i++)
            {
                Assert.AreEqual(returns1[i], 0);
                Assert.AreEqual(returns2[i], 0);
            }

            //Switch all blocks to unoccupied (high) from occupied (low).
            for (int i = 0; i < returns1.Length; i++)
            {
                occupancies[i] = 1;
            }

            //Send all blocks occupied and pull light values
            RedlinePLC1.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC1.ReceiveLeftLights(45)).CopyTo(returns1, 0);
            (RedlinePLC1.ReceiveRightLights(45)).CopyTo(returns2, 0);

            //Lights should be 2 for green
            for (int i = 0; i < occupancies.Length; i++)
            {
                Assert.AreEqual(returns1[i], 2);
                Assert.AreEqual(returns2[i], 2);
            }

            occupancies[16] = 0;

            RedlinePLC1.SendOccupancies(occupancies);
            Thread.Sleep(10);
            (RedlinePLC1.ReceiveLeftLights(45)).CopyTo(returns1, 0);
            (RedlinePLC1.ReceiveRightLights(45)).CopyTo(returns2, 0);

            Assert.AreEqual(returns1[15], 1);
            Assert.AreEqual(returns2[17], 1);


        }

        
    }
    //[TestMethod]
    //public void TrackModel_AddTrain()
    //{
    //    int blockIdx = 0;
    //    int lineIdx = 0;
    //    int authority = 0;

    //    TrackModel.MainWindow track = new TrackModel.MainWindow();
    //    track.AddTrain(blockIdx, lineIdx, authority);

    //    Assert.AreEqual(track.mtrainList.Count, 1);
    //    Assert.AreEqual(track.mtrainList[0].blockIdx, 0);
    //    Assert.AreEqual(track.mtrainList[0].lineIdx, 0);
    //    Assert.AreEqual(track.mtrainList[0].commAuthority, 0);
    //}
    [TestClass]
    public class TrackControllerHW
    {
        [TestMethod]
        public void TrackReceiveOccupanciesGreen()
        {
            Track_Controller_1._02.Controller GreenLinePLC = new Track_Controller_1._02.Controller(4, true, "127.0.0.1");

            int[] occupancies = new int[151];
            int[] returns = new int[151];
            Random rnd = new Random();

            GreenLinePLC.SendOccupancies(occupancies);
            returns = GreenLinePLC.ReceiveOccupancies(151);

            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(occupancies[i], returns[i]);
            }

            for (int i = 0; i < returns.Length; i++)
            {
                occupancies[i] = rnd.Next(1);
            }


            GreenLinePLC.SendOccupancies(occupancies);
            returns = GreenLinePLC.ReceiveOccupancies(151);
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(occupancies[i], returns[i]);
            }


        }

        [TestMethod]
        public void ReceiveAuthoritiesGreen()
        {
            //Test valid constructor
            Track_Controller_1._02.Controller green = new Track_Controller_1._02.Controller(4, true);
            int[] mAuthorities = new int[151];
            int[] returns = new int[151];
            Random rnd = new Random();

            green.SendAuthorities(mAuthorities);
            returns = green.ReceiveAuthorities(151);

            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(mAuthorities[i], returns[i]);
            }

            for (int i = 0; i < returns.Length; i++)
            {
                mAuthorities[i] = rnd.Next(1);
            }


            green.SendAuthorities(mAuthorities);
            returns = green.ReceiveAuthorities(151);
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(mAuthorities[i], returns[i]);
            }
        }

        [TestMethod]
        public void ReceiveSpeedsGreen()
        {
            //Test valid constructor
            Track_Controller_1._02.Controller green = new Track_Controller_1._02.Controller(4, true);
            int[] mSpeeds = new int[151];
            int[] returns = new int[151];
            Random rnd = new Random();

            green.SendSpeeds(mSpeeds);
            returns = green.ReceiveSpeeds(151);

            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(mSpeeds[i], returns[i]);
            }

            for (int i = 0; i < returns.Length; i++)
            {
                mSpeeds[i] = rnd.Next(1);
            }


            green.SendSpeeds(mSpeeds);
            returns = green.ReceiveSpeeds(151);
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(mSpeeds[i], returns[i]);
            }
        }

        [TestMethod]
        public void ReceiveSwitches()
        {
            Track_Controller_1._02.Controller green = new Track_Controller_1._02.Controller(4, true);
            int[] mOcc = new int[151];
            green.SendOccupancies(mOcc);
            int[] mSwitches = green.ReceiveSwitches(151);

            
            Assert.IsTrue(mSwitches[62] == 151);
            Assert.IsTrue(mSwitches[76] == 76);
            Assert.IsTrue(mSwitches[84] == 100);

            mOcc[61] = 1;
            green.SendOccupancies(mOcc);
            mSwitches = green.ReceiveSwitches(151);
            Assert.IsTrue(mSwitches[62] == 62);
            Assert.IsTrue(mSwitches[76] == 76);
            Assert.IsTrue(mSwitches[84] == 100);

            mOcc[61] = 0;
            mOcc[76] = 1;
            green.SendOccupancies(mOcc);
            mSwitches = green.ReceiveSwitches(151);
            Assert.IsTrue(mSwitches[62] == 151);
            Assert.IsTrue(mSwitches[76] == 101);
            Assert.IsTrue(mSwitches[84] == 86);

            mOcc[76] = 0;
            mOcc[84] = 1;
            green.SendOccupancies(mOcc);
            mSwitches = green.ReceiveSwitches(151);
            Assert.IsTrue(mSwitches[62] == 151);
            Assert.IsTrue(mSwitches[76] == 101);
            Assert.IsTrue(mSwitches[84] == 86);
        }

        [TestMethod]
        public void Maintenance()
        {
            Track_Controller_1._02.Controller green = new Track_Controller_1._02.Controller(4, true);
            int[] mOcc = new int[151];
            int[] mMaint = new int[151];
            green.SendOccupancies(mOcc);
            green.SendMaintenance(mMaint);
            int[] mSwitches = green.ReceiveSwitches(151);


            Assert.IsTrue(mSwitches[62] == 151);
            Assert.IsTrue(mSwitches[76] == 76);
            Assert.IsTrue(mSwitches[84] == 100);

            mMaint[61] = 1;
            green.SendMaintenance(mMaint);
            mSwitches = green.ReceiveSwitches(151);
            Assert.IsTrue(mSwitches[62] == 62);
            Assert.IsTrue(mSwitches[76] == 76);
            Assert.IsTrue(mSwitches[84] == 100);

            mOcc[61] = 1;
            mOcc[76] = 1;
            green.SendOccupancies(mOcc);
            mSwitches = green.ReceiveSwitches(151);
            Assert.IsTrue(mSwitches[62] == 62);
            Assert.IsTrue(mSwitches[76] == 101);
            Assert.IsTrue(mSwitches[84] == 86);

            mOcc[61] = 0;
            mMaint[61] = 0;
            mMaint[76] = 0;
            mMaint[84] = 1;
            green.SendMaintenance(mMaint);
            green.SendOccupancies(mOcc);
            mSwitches = green.ReceiveSwitches(151);
            Assert.IsTrue(mSwitches[62] == 151);
            Assert.IsTrue(mSwitches[76] == 101);
            Assert.IsTrue(mSwitches[84] == 86);
        }

        [TestMethod]
        public void OverrideSwitches()
        {
            //Test valid constructor
            Track_Controller_1._02.Controller green = new Track_Controller_1._02.Controller(4, true);
            int[] mSwitches = new int[151];
            int[] returns = new int[151];
            Random rnd = new Random();

            green.SendSwitches(mSwitches);
            returns = green.ReceiveSwitches(151);

            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(mSwitches[i], returns[i]);
            }

            for (int i = 0; i < returns.Length; i++)
            {
                mSwitches[i] = rnd.Next(1);
            }
            mSwitches[62] = 0;

            green.SendSwitches(mSwitches);
            returns = green.ReceiveSwitches(151);
            for (int i = 0; i < returns.Length; i++)
            {
                Assert.AreEqual(mSwitches[i], returns[i]);
            }

            int[] mMaint = new int[151];
            for (int i = 1; i < mMaint.Length; i++)
            {
                mMaint[i] = 0;
            }
            green.SendMaintenance(mMaint);
            returns = green.ReceiveSwitches(151);
            Assert.IsFalse(0 == returns[62]);

        }

        [TestMethod]
        public void TransitLights()
        {
            Track_Controller_1._02.Controller green = new Track_Controller_1._02.Controller(4, true);
            int[] mOcc = new int[151];
            int[] mMaint = new int[151];
            green.SendOccupancies(mOcc);
            green.SendMaintenance(mMaint);
            int[] mRightLights = new int[151];
            mRightLights = green.ReceiveRightLights(151);
            int[] mLeftLights = new int[151];
            mLeftLights =  green.ReceiveLeftLights(151);
            for (int i = 58; i < 142; i++)
            {
                Assert.AreEqual(mRightLights[i], 1);
                
            }
            for (int i = 76; i < 85; i++)
            {
                Assert.AreEqual(mLeftLights[i], 1);
            }

            mOcc[98] = 1;
            mMaint[79] = 1;
            green.SendOccupancies(mOcc);
            green.SendMaintenance(mMaint);
            mRightLights = green.ReceiveRightLights(151);
            mLeftLights = green.ReceiveLeftLights(151);
            Assert.AreEqual(mRightLights[97], 0);
            Assert.AreEqual(mLeftLights[80], 0);
            Assert.AreEqual(mRightLights[99], 0);
            Assert.AreEqual(mRightLights[75], 0);
            mOcc[100] = 1;
            green.SendOccupancies(mOcc);
            mLeftLights = green.ReceiveLeftLights(151);
            Assert.AreEqual(mLeftLights[76], 0);
        }
    }
    [TestClass]
    public class TrainControllerSW
    {
        [TestMethod]
        public void ToggleLeftDoors()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;

            Assert.AreEqual(train.mLeftDoorsStatus, false); // closed by default
            train.setLeftDoors();
            Assert.AreEqual(train.mLeftDoorsStatus, true);  // open
            train.setLeftDoors();
            Assert.AreEqual(train.mLeftDoorsStatus, false); // closed
        }

        [TestMethod]
        public void ToggleRightDoors()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;

            Assert.AreEqual(train.mRightDoorsStatus, false); // closed by default
            train.setRightDoors();
            Assert.AreEqual(train.mRightDoorsStatus, true);  // open
            train.setRightDoors();
            Assert.AreEqual(train.mRightDoorsStatus, false); // closed
        }

        [TestMethod]
        public void ToggleInteriorLights()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;

            Assert.AreEqual(train.mInteriorLightsStatus, false); // off by default
            train.setInteriorLights();
            Assert.AreEqual(train.mInteriorLightsStatus, true);  // on
            train.setInteriorLights();
            Assert.AreEqual(train.mInteriorLightsStatus, false); // off
        }

        [TestMethod]
        public void ToggleExteriorLights()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;

            Assert.AreEqual(train.mExteriorLightsStatus, false); // off by default
            train.setExteriorLights();
            Assert.AreEqual(train.mExteriorLightsStatus, true);  // on
            train.setExteriorLights();
            Assert.AreEqual(train.mExteriorLightsStatus, false); // off
        }

        [TestMethod]
        public void ToggleAnnouncements()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;

            Assert.AreEqual(train.mAnnouncementsStatus, false); // off by default
            train.setAnnouncements();
            Assert.AreEqual(train.mAnnouncementsStatus, true);  // on
            train.setAnnouncements();
            Assert.AreEqual(train.mAnnouncementsStatus, false); // off
        }

        [TestMethod]
        public void DecrementTemperature()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;

            Assert.AreEqual(train.mTemperature, 72);  // 72 degrees F by default
            train.tempDecrease();
            Assert.AreEqual(train.mTemperature, 71);  // 71 degrees F
            for (int i = 0; i < 9; i++)
            {
                train.tempDecrease();
            }
            Assert.AreEqual(train.mTemperature, 62);  // 62 degrees F
        }

        [TestMethod]
        public void IncrementTemperature()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;

            Assert.AreEqual(train.mTemperature, 72);  // 72 degrees F by default
            train.tempIncrease();
            Assert.AreEqual(train.mTemperature, 73);  // 73 degrees F
            for (int i=0; i<9; i++)
            {
                train.tempIncrease();
            }
            Assert.AreEqual(train.mTemperature, 82);  // 82 degrees F
        }

        [TestMethod]
        public void SetKp()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;

            Assert.AreEqual(train.mKp, 10000); // default 10000 Kp
            train.setKp(500);
            Assert.AreEqual(train.mKp, 500);   // 500 Kp
            train.setKp(0);
            Assert.AreEqual(train.mKp, 0);     // 0 Kp
        }

        [TestMethod]
        public void SetKi()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;

            Assert.AreEqual(train.mKi, 0);     // default 0 Ki
            train.setKi(500);
            Assert.AreEqual(train.mKi, 500);   // 500 Kp
            train.setKi(10000);
            Assert.AreEqual(train.mKi, 10000); // 10000 Ki
        }

        [TestMethod]
        public void TrainMovesTowardsCommandedSpeedInAutomaticMode()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = true;              // automatic mode

            Assert.AreEqual(train.mCurSpeed, 0); // train initially not moving, has no speed
            train.setCmdSpeed(5 * 2.236936);     // set to 5 m/s (imperial input)

            for (int i=0; i<100; i++)
            {
                train.UpdateSpeed();
            }
            Assert.IsTrue(train.mCurSpeed == 5); // train reaches commanded speed
        }

        [TestMethod]
        public void TrainMovesTowardsSetSpeedInManualMode()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = false;             // manual mode

            Assert.AreEqual(train.mCurSpeed, 0); // train initially not moving, has no speed
            train.setCmdSpeed(5 * 2.236936);     // set to 5 m/s (imperial input)
            train.setSetSpeed(3 * 2.236936);
            
            for (int i=0; i<100; i++)
            {
                train.UpdateSpeed();
            }
            Assert.IsTrue(train.mCurSpeed == 3); // train reaches set speed
        }

        [TestMethod]
        public void UnableToSetSetSpeedAboveCommandedSpeed()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = false;

            Assert.AreEqual(train.mSetSpeed, 0); // set speed initiialy zero
            train.setCmdSpeed(5);
            train.setSetSpeed(10);
            Assert.AreEqual(train.mSetSpeed, 0); // set speed does not change because input is above commanded speed
        }

        [TestMethod]
        public void TrainSpeedIncrements()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = true;

            Assert.AreEqual(train.mCurSpeed, 0); // train initially not moving, has no speed
            train.setCmdSpeed(5);                // set commanded speed to 5
            train.UpdateSpeed();
            Assert.IsTrue(train.mCurSpeed > 0);  // train speed increments
        }

        [TestMethod]
        public void TrainSpeedDecrements()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = true;

            Assert.AreEqual(train.mCurSpeed, 0); // train initially not moving, has no speed
            train.setCurSpeed(5 * 2.236936);     // set current speed to 5 (imperial input)
            train.UpdateSpeed();
            Assert.IsTrue(train.mCurSpeed < 5);  // train speed decrements
        }

        [TestMethod]
        public void TrainCalculatesPositivePower()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = true;

            Assert.AreEqual(train.mCurPower, 0); // train initially not moving, has no power
            train.setCmdSpeed(100 * 2.236936);
            train.CalculatePowerSW();
            Assert.IsTrue(train.mCurPower > 0);  // train accelerates towards commanded speed, calculates positive power
        }

        [TestMethod]
        public void TrainCalculatesNegativePower()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = true;

            Assert.AreEqual(train.mCurPower, 0); // train initially not moving, has no power
            train.setCurSpeed(100 * 2.236936);
            train.CalculatePowerSW();
            Assert.IsTrue(train.mCurPower < 0);  // train decelerates towards commanded speed, calculates negative power
        }

        [TestMethod]
        public void TrainCalculatesMaxPower()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = true;

            Assert.AreEqual(train.mCurPower, 0);          // train initially not moving, has no power
            train.setCmdSpeed(1000000000000000000);       // train receives very large commanded speed
            train.CalculatePowerSW();                     // train calculates power
            Assert.IsTrue(train.mCurPower == 120000); // power set to max power
        }

        [TestMethod]
        public void TrainDeceleratesWhileEmergencyBrakeActivated()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = true;

            Assert.AreEqual(train.mCurSpeed, 0); // train initially not moving, has no speed
            train.setCmdSpeed(5 * 2.236936);     // set to 5 m/s (imperial input)
            
            // train accelerates to commanded speed
            for (int i=0; i<100; i++)
            {
                train.UpdateSpeed();
            }
            Assert.IsTrue(train.mCurSpeed == 5);

            train.setEmergencyBrake();
            train.UpdateSpeed();
            Assert.IsTrue(train.mCurSpeed < 5); // train decelerates from emergency brake
        }

        [TestMethod]
        public void TrainDeceleratesWhileServiceBrakeActivated()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = false;
            train.mAutoMode = true;

            Assert.AreEqual(train.mCurSpeed, 0); // train initially not moving, has no speed
            train.setCmdSpeed(5 * 2.236936);     // set to 5 m/s (imperial input)

            // train accelerates to commanded speed
            for (int i=0; i<100; i++)
            {
                train.UpdateSpeed();
            }
            Assert.IsTrue(train.mCurSpeed == 5);

            train.setServiceBrake();
            train.UpdateSpeed();
            Assert.IsTrue(train.mCurSpeed < 5); // train decelerates from emergency brake
        }
    }

    [TestClass]
    public class TrainControllerHW
    {
        [TestMethod]
        public void ToggleLeftDoors()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = true;

            Assert.AreEqual(train.mLeftDoorsStatus, false); // closed by default
            train.setLeftDoors();
            Assert.AreEqual(train.mLeftDoorsStatus, true);  // open
            train.setLeftDoors();
            Assert.AreEqual(train.mLeftDoorsStatus, false); // closed
        }

        [TestMethod]
        public void ToggleRightDoors()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = true;

            Assert.AreEqual(train.mRightDoorsStatus, false); // closed by default
            train.setRightDoors();
            Assert.AreEqual(train.mRightDoorsStatus, true);  // open
            train.setRightDoors();
            Assert.AreEqual(train.mRightDoorsStatus, false); // closed
        }

        [TestMethod]
        public void ToggleInteriorLights()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = true;

            Assert.AreEqual(train.mInteriorLightsStatus, false); // closed by default
            train.setInteriorLights();
            Assert.AreEqual(train.mInteriorLightsStatus, true);  // open
            train.setInteriorLights();
            Assert.AreEqual(train.mInteriorLightsStatus, false); // closed
        }

        [TestMethod]
        public void ToggleExteriorLights()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = true;

            Assert.AreEqual(train.mExteriorLightsStatus, false); // closed by default
            train.setExteriorLights();
            Assert.AreEqual(train.mExteriorLightsStatus, true);  // open
            train.setExteriorLights();
            Assert.AreEqual(train.mExteriorLightsStatus, false); // closed
        }

        [TestMethod]
        public void ToggleAnnouncements()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = true;

            Assert.AreEqual(train.mAnnouncementsStatus, false); // closed by default
            train.setAnnouncements();
            Assert.AreEqual(train.mAnnouncementsStatus, true);  // open
            train.setAnnouncements();
            Assert.AreEqual(train.mAnnouncementsStatus, false); // closed
        }

        [TestMethod]
        public void DecrementTemperature()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = true;

            Assert.AreEqual(train.mTemperature, 72); // 72 degrees F by default
            train.tempDecrease();
            Assert.AreEqual(train.mTemperature, 71);  // 71 degrees F
            for (int i = 0; i < 9; i++)
            {
                train.tempDecrease();
            }
            Assert.AreEqual(train.mTemperature, 62); // 62 degrees F
        }

        [TestMethod]
        public void IncrementTemperature()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = true;

            Assert.AreEqual(train.mTemperature, 72); // 72 degrees F by default
            train.tempIncrease();
            Assert.AreEqual(train.mTemperature, 73);  // 73 degrees F
            for (int i = 0; i < 9; i++)
            {
                train.tempIncrease();
            }
            Assert.AreEqual(train.mTemperature, 82); // 82 degrees F
        }

        [TestMethod]
        public void SetKp()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = true;

            Assert.AreEqual(train.mKp, 10000); // default 10000 Kp
            train.setKp(500);
            Assert.AreEqual(train.mKp, 500); // 500 Kp
            train.setKp(0);
            Assert.AreEqual(train.mKp, 0); // 0 Kp
        }

        [TestMethod]
        public void SetKi()
        {
            TrainController.Controller train = new TrainController.Controller(false);
            train.mControlType = true;

            Assert.AreEqual(train.mKi, 0); // default 0 Ki
            train.setKi(500);
            Assert.AreEqual(train.mKi, 500); // 500 Kp
            train.setKi(10000);
            Assert.AreEqual(train.mKi, 10000); // 10000 Ki
        }
    }
}
