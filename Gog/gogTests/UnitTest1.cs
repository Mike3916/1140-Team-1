

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
        public void TrainController_ToggleLeftDoors()
        {
            TrainController.Controller train = new TrainController.Controller(true);
            Assert.AreEqual(train.mLeftDoorsStatus, false); // closed by default
            train.setLeftDoors();
            Assert.AreEqual(train.mLeftDoorsStatus, true);  // open
            train.setLeftDoors();
            Assert.AreEqual(train.mLeftDoorsStatus, false); // closed
        }

        [TestMethod]
        //Checks if program will crash when attempting to connect to an unconfigured port number
        public void ConnectToBadPort()
        {
            Track_Controller_1._02.Controller RedlinePLC = new Track_Controller_1._02.Controller(855, false, "127.0.0.1");

            int[] crossings = new int[45];
            int[] returns = new int[45];

            RedlinePLC.SendCrossings(crossings);
            (RedlinePLC.ReceiveCrossings(45)).CopyTo(returns,0);
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
                authorities[i] = i+1;
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
            for(int i=16; i<=26; i++)
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
            for(int i = 0; i< occupancies.Length; i++)
            {
                Assert.AreEqual(returns1[i], 0);
                Assert.AreEqual(returns2[i], 0);
            }

            //Switch all blocks to unoccupied (high) from occupied (low).
            for(int i=0; i< returns1.Length; i++)
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

        //[TestMethod]
        //public void TrackReceiveOccupanciesGreen()
        //{
        //    Track_Controller_1._02.Controller GreenLinePLC = new Track_Controller_1._02.Controller(4, true, "127.0.0.1");

            //int[] occupancies = new int[151];
            //int[] returns = new int[151];

            //GreenLinePLC.SendOccupancies(occupancies);
            //returns = GreenLinePLC.ReceiveOccupancies(151);

            //for (int i = 0; i < returns.Length; i++)
            //{
            //    Assert.AreEqual(occupancies[i], returns[i]);
            //}

        //    for (int i = 0; i < returns.Length; i++)
        //    {
        //        occupancies[i] = 1;
        //    }
            //for (int i = 0; i < returns.Length; i++)
            //{
            //    occupancies[i] = 1;
            //    Assert.AreEqual(occupancies[i],returns[i]);
            //}

            //GreenLinePLC.SendOccupancies(occupancies);
            //returns = GreenLinePLC.ReceiveOccupancies(151);

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
    }
