using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Track_Controller_1;


namespace GogNS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TrackModel.MainWindow track;
        TrainController.ControlPanel trainCtrl;
        TrainModel.MainWindow trains;
        CTC.MainWindow ctc;
        Track_Controller_1._02.Controller mRedline1 = new Track_Controller_1._02.Controller(851, false, "127.0.0.1");
        Track_Controller_1._02.Controller mRedline2 = new Track_Controller_1._02.Controller(853, false, "127.0.0.1");
        Track_Controller_1._02.Controller mGreenLine1 = new Track_Controller_1._02.Controller(852, false, "127.0.0.1");

        int[] mRedMaintenanceBlocks = new int[77];
        int[] mRedOccupancies = new int[77];
        int[] mRedSpeeds = new int[77];
        int[] mRedAuthorities = new int[77];
        int[] mRedCrossings = new int[77];
        int[] mRedSwitches = new int[77];
        int[] mRedLeftLights = new int[77];
        int[] mRedRightLights = new int[77];

        int[] mRed1MaintenanceBlocks = new int[44];
        int[] mRed1Occupancies = new int[44];
        int[] mRed1Speeds = new int[44];
        int[] mRed1Authorities = new int[44];
        int[] mRed1Crossings = new int[44];
        int[] mRed1Switches = new int[44];
        int[] mRed1LeftLights = new int[44];
        int[] mRed1RightLights = new int[44];
        int[] mRed2MaintenanceBlocks = new int[41];
        int[] mRed2Occupancies = new int[41];
        int[] mRed2Speeds = new int[41];
        int[] mRed2Authorities = new int[41];
        int[] mRed2Crossings = new int[41];
        int[] mRed2Switches = new int[41];
        int[] mRed2LeftLights = new int[41];
        int[] mRed2RightLights = new int[41];

        int[] mGreenMaintenanceBlocks = new int[151];
        int[] mGreenOccupancies = new int[151];
        int[] mGreenSpeeds = new int[151];
        int[] mGreenAuthorities = new int[151];
        int[] mGreenCrossings = new int[151];
        int[] mGreenSwitches = new int[151];
        int[] mGreenLeftLights = new int[151];
        int[] mGreenRightLights = new int[151];

        int[] mGreen1MaintenanceBlocks = new int[140];
        int[] mGreen1Occupancies = new int[140];
        int[] mGreen1Speeds = new int[140];
        int[] mGreen1Authorities = new int[140];
        int[] mGreen1Crossings = new int[140];
        int[] mGreen1Switches = new int[140];
        int[] mGreen1LeftLights = new int[140];
        int[] mGreen1RightLights = new int[140];
        /*int[] mGreen2MaintenanceBlocks = new int[151];
        int[] mGreen2Occupancies = new int[151];
        int[] mGreen2Speeds = new int[151];
        int[] mGreen2Authorities = new int[151];
        int[] mGreen2Crossings = new int[151];
        int[] mGreen2Switches = new int[151];
        int[] mGreen2LeftLights = new int[151];
        int[] mGreen2RightLights = new int[151];
        */

        DispatcherTimer mGlobalTimer;
        int mIterationMultiplier = 10, numTrains = 0, numTrainCtrls = 0, iter = 0;
        bool newBlock;
        bool gotTrack = false;

        public MainWindow()
        {
            InitializeComponent();
            InitTimer();
        }

        //events 
        private void TrackModelButton_Click(object sender, RoutedEventArgs e)
        {
            if (track == null)
            {
                Application.Current.MainWindow = track;
                track = new TrackModel.MainWindow();
                track.Show();
            }
            else
                track.Activate();
        }


        private void TrainModelButton_Click(object sender, RoutedEventArgs e)
        {
            if (trains == null)
            {
                Application.Current.MainWindow = trains;
                trains = new TrainModel.MainWindow();
                trains.Show();
                numTrains++;
            }
            else
                trains.Activate();
        }

        private void TrainCtrlButton_Click(object sender, RoutedEventArgs e)
        {
            if (trainCtrl == null)
            {
                Application.Current.MainWindow = trainCtrl;
                trainCtrl = new TrainController.ControlPanel();
                trainCtrl.Show();
                numTrainCtrls++;
            }
            else
                trainCtrl.Activate();
        }

        private void CTC_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ctc == null)
            {
                Application.Current.MainWindow = ctc;
                ctc = new CTC.MainWindow();
                ctc.Show();
            }
            else
                ctc.Activate();
        }

        private void StartUpActivated(object sender, EventArgs e)
        {
            Application.Current.MainWindow = trainCtrl;
            Application.Current.MainWindow = trains;
            Application.Current.MainWindow = track;
            Application.Current.MainWindow = ctc;
        }

        private void StartUpInactive(object sender, EventArgs e)
        {
            if (trainCtrl != null && trainCtrl.IsActive)
            {
                Application.Current.MainWindow = trainCtrl;
            }
        }

        private void InitTimer()    
        {
            mGlobalTimer = new DispatcherTimer();

            mGlobalTimer.Tick += new EventHandler(updateTick);

            mGlobalTimer.Interval = new TimeSpan(0, 0, 0, 0, 10); //1 millisecond
            mGlobalTimer.Start();

        }

        private void updateTick(object sender, EventArgs e)
        {
            for (int i = 0; i < mIterationMultiplier; i++)
            {
                if (iter++ % 10 == 0) TimeBox.Text = (iter).ToString();

                PLCgetCTC();
                PLCgetTrack();

                System.Threading.Thread.Sleep(10);

                PLCsetCTC();
                PLCsetTrack(); 

           
                


                if (track != null && ctc != null && gotTrack==false)    //As long as track and ctc both exist, and the track has not been sent to the CTC yet,
                {                                                       
                    if (track.mLines.Count > 0)                         //Make sure the track files are loaded into the TrackModel module BEFORE the CTC model (button) is pressed to make sure the CTC will always get the full track
                    {
                        ctc.GetTrackLayout(track.mLines);                 //Send the track data to the CTC
                        gotTrack = true;                                //Set boolean to mark that the track data has been read by CTC
                    }
                }
                //ctc.GetTrackController(mRedMaintenanceBlocks, mRedOccupancies, mRedSpeeds, mRedAuthorities, mRedCrossings, mRedSwitches, mRedLeftLights, mRedRightLights, mGreenMaintenanceBlocks, mGreenOccupancies, mGreenSpeeds, mGreenAuthorities, mGreenCrossings, mGreenSwitches, mGreenLeftLights, mGreenRightLights); //Write function in CTC to read in these values
                /* Update Track Controller variables with values from CTC
                mRedMaintenanceBlocks = ctc.mRedMaintenanceBlocks;
                mRedOccupancies = ctc.mRedOccupancies;
                mRedSpeeds = ctc.mRedSpeeds;
                mRedAuthorities = ctc.mRedAuthorities;
                mRedCrossings = ctc.mRedCrossings;
                mRedSwitches = ctc.mRedSwitches;
                mRedLeftLights = ctc.mRedLeftLights;
                mRedRightLights = ctc.mRedRightLights

                mGreenMaintenanceBlocks = ctc.mGreenMaintenanceBlocks;
                mGreenOccupancies = ctc.mGreenOccupancies;
                mGreenSpeeds = ctc.mGreenSpeeds;
                mGreenAuthorities = ctc.mGreenAuthorities;
                mGreenCrossings = ctc.mGreenCrossings;
                mGreenSwitches = ctc.mGreenSwitches;
                mGreenLeftLights = ctc.mGreenLeftLights;
                mGreenRightLights = ctc.mGreenRightLights;
                */
                
                /*
                ctc.SetTrackData(track.mLines);
                track.AddTrain(151, 1, 1, 12);
                trainCtrl.checkUpdatedValues();
                ctc.SetTrackData(track.);
                track.GetT
                */

                for (int j = 0; j < numTrains && j < numTrainCtrls; j++)
                {
                    newBlock = trains.UpdateValues(trainCtrl.mTrainSetList[j],j);
                    trainCtrl.UpdateValues(trains.Trains[j].getCmdAuthority(), trains.Trains[j].getCurrAuthority(), trains.Trains[j].getCommandedSpeedMPH(), trains.Trains[j].getCurrentSpeedMPH(), trains.Trains[j].getBeacon(), trains.Trains[j].getUnderground(), trains.Trains[j].getDoorL(), trains.Trains[j].getDoorR(), j);

                   /* if (trains.Trains[j].newBlock)                  //if train at j enters a new block
                    { 
                        TrackModel.Block bl = track.UpdateTrain(j); //get next block
                        if (bl != null)                             //if that block exists
                            trains.UpdateBlock(bl, j);              //update the train pos
                        else                                        //if that block doesn't exist ...
                            trains.Trains.RemoveAt(j);              //delete the train

                    }*/
                }



            }
        }

        private void PLCgetCTC()
        {
            /*77 element integer arrays for red line
            mRedMaintenanceBlocks = CTCReturnMaintenanceFunction;
            mRedSpeeds = CTCReturnSpeedsFunction;
            mRedAuthorities = CTCRetrunAuthoritiesFunction;
            mRedSwitches = CTCReturnSwitchesFunction;
            */

            /*151 element integer arrays for red line
            mGreenMaintenanceBlocks = CTCReturnMaintenanceFunction;
            mGreenSpeeds = CTCReturnSpeedsFunction;
            mGreenAuthorities = CTCReturnAuhtorityFunction;
            mGreenSwitches = CTCReturnSwitchesFunction;
            */

            ArraySplitter();

            mRedline1.SendMaintenance(mRed1MaintenanceBlocks);
            mRedline1.SendSpeeds(mRed1Speeds);
            mRedline1.SendAuthorities(mRed1Authorities);
            mRedline1.SendSwitches(mRed1Switches);
            mRedline2.SendMaintenance(mRed2MaintenanceBlocks);
            mRedline2.SendSpeeds(mRed2Speeds);
            mRedline2.SendAuthorities(mRed2Authorities);
            mRedline2.SendSwitches(mRed2Switches);

            mGreenLine1.SendMaintenance(mGreen1MaintenanceBlocks);
            mGreenLine1.SendSpeeds(mGreen1Speeds);
            mGreenLine1.SendAuthorities(mGreen1Authorities);
            mGreenLine1.SendSwitches(mGreen1Switches);

        }

        private void PLCsetCTC()
        {
            mRedline1.ReceiveOccupancies(mRed1Occupancies.Length);
            mRedline1.ReceiveCrossings(mRed1Crossings.Length);
            mRedline1.ReceiveSwitches(mRed1Switches.Length);
            mRedline1.ReceiveRightLights(mRed1RightLights.Length);
            mRedline1.ReceiveLeftLights(mRed1LeftLights.Length);
            mRedline2.ReceiveOccupancies(mRed2Occupancies.Length);
            mRedline2.ReceiveCrossings(mRed2Crossings.Length);
            mRedline2.ReceiveSwitches(mRed2Switches.Length);
            mRedline2.ReceiveRightLights(mRed2RightLights.Length);
            mRedline2.ReceiveLeftLights(mRed2LeftLights.Length);
            mGreenLine1.ReceiveOccupancies(mGreen1Occupancies.Length);
            mGreenLine1.ReceiveCrossings(mGreen1Crossings.Length);
            mGreenLine1.ReceiveSwitches(mGreen1Switches.Length);
            mGreenLine1.ReceiveRightLights(mGreen1RightLights.Length);
            mGreenLine1.ReceiveLeftLights(mGreen1LeftLights.Length);

            ArrayMerger();

            /*77 element integer arrays for red line
            
            = mRedOccupancies;
            = mRedCrossings;
            = mRedSwitches;
         */
            /*151 element integer arrays for green line
             = mGreenOccupancies;
             = mGreenCrossings;
             = mGreenSwitches;
       */
           
        }

        private void PLCgetTrack()
        {
            //77 ELEMENT array for red
            //mRedOccupancies = ;

            //151 ELEMENT array for red
            //mGreenOccupancies = ;

            ArraySplitter();

            mRedline1.SendOccupancies(mRed1Occupancies);
            mRedline2.SendOccupancies(mRed2Occupancies);
            mGreenLine1.SendOccupancies(mGreen1Occupancies);

        }

        private void PLCsetTrack()
        {
            mRedline1.ReceiveSpeeds(mRed1Speeds.Length);
            mRedline1.ReceiveAuthorities(mRed1Authorities.Length);
            mRedline1.ReceiveCrossings(mRed1Crossings.Length);
            mRedline1.ReceiveSwitches(mRed1Switches.Length);
            mRedline1.ReceiveRightLights(mRed1RightLights.Length);
            mRedline1.ReceiveLeftLights(mRed1LeftLights.Length);
            mRedline2.ReceiveSpeeds(mRed2Speeds.Length);
            mRedline2.ReceiveAuthorities(mRed2Authorities.Length);
            mRedline2.ReceiveCrossings(mRed2Crossings.Length);
            mRedline2.ReceiveSwitches(mRed2Switches.Length);
            mRedline2.ReceiveRightLights(mRed2RightLights.Length);
            mRedline2.ReceiveLeftLights(mRed2LeftLights.Length);

            mGreenLine1.ReceiveSpeeds(mGreen1Speeds.Length);
            mGreenLine1.ReceiveAuthorities(mGreen1Authorities.Length);
            mGreenLine1.ReceiveCrossings(mGreen1Crossings.Length);
            mGreenLine1.ReceiveSwitches(mGreen1Switches.Length);
            mGreenLine1.ReceiveRightLights(mGreen1RightLights.Length);
            mGreenLine1.ReceiveLeftLights(mGreen1LeftLights.Length);

            ArrayMerger();

            /*77 element array for red
            = mRedSpeeds;
            = mRedAuthorities;
            = mRedCrossings;
            = mRedSwitches;
            = mRedLeftLights;
            = mRedRightLights;
            */

            //151 element array for green
            /*
            = mRedSpeeds;
            = mRedAuthorities;
            = mRedCrossings;
            = mRedSwitches;
            = mRedLeftLights;
            = mRedRightLights;
            */
        }

        private void ArraySplitter()
        {
            
             Array.Copy(mRedMaintenanceBlocks,0,mRed1MaintenanceBlocks,0,38);
             Array.Copy(mRedOccupancies,0,mRed1Occupancies,0,38);
             Array.Copy(mRedSpeeds,0,mRed1Speeds,0,38);
             Array.Copy(mRedAuthorities,0,mRed1Authorities,0,38);
             Array.Copy(mRedCrossings,0,mRed1Crossings,0,38);
             Array.Copy(mRedSwitches,0,mRed1Switches,0,38);
             Array.Copy(mRedLeftLights,0,mRed1LeftLights,0,38);
             Array.Copy(mRedRightLights,0,mRed1RightLights,0,38);

             Array.Copy(mRedMaintenanceBlocks,71,mRed1MaintenanceBlocks,38,6);
             Array.Copy(mRedOccupancies,71,mRed1Occupancies,38,6);
             Array.Copy(mRedSpeeds,71,mRed1Speeds,38,6);
             Array.Copy(mRedAuthorities,71,mRed1Authorities,38,6);
             Array.Copy(mRedCrossings,71,mRed1Crossings,38,6);
             Array.Copy(mRedSwitches,71,mRed1Switches,38,6);
             Array.Copy(mRedLeftLights,71,mRed1LeftLights,38,6);
             Array.Copy(mRedRightLights,71,mRed1RightLights,38,6);

             Array.Copy(mRedMaintenanceBlocks,32,mRed2MaintenanceBlocks,0,39);
             Array.Copy(mRedOccupancies,32,mRed2Occupancies,0,39);
             Array.Copy(mRedSpeeds,32,mRed2Speeds,0,39);
             Array.Copy(mRedAuthorities,32,mRed2Authorities,0,39);
             Array.Copy(mRedCrossings,32,mRed2Crossings,0,39);
             Array.Copy(mRedSwitches,32,mRed2Switches,0,39);
             Array.Copy(mRedLeftLights,32,mRed2LeftLights,0,39);
             Array.Copy(mRedRightLights,32,mRed2RightLights,0,39);
            
            return;
        }

        private void ArrayMerger()
        {

            Array.Copy(mRed1MaintenanceBlocks, 0, mRedMaintenanceBlocks, 0, 38);
            Array.Copy(mRed1Occupancies, 0, mRedOccupancies, 0, 38);
            Array.Copy(mRed1Speeds, 0, mRedSpeeds, 0, 38);
            Array.Copy(mRed1Authorities, 0, mRedAuthorities, 0, 38);
            Array.Copy(mRed1Crossings, 0, mRedCrossings, 0, 38);
            Array.Copy(mRed1Switches, 0, mRedSwitches, 0, 38);
            Array.Copy(mRed1LeftLights, 0, mRedLeftLights, 0, 38);
            Array.Copy(mRed1RightLights, 0, mRedRightLights, 0, 38);

            Array.Copy(mRedMaintenanceBlocks, 38, mRed1MaintenanceBlocks, 71, 6);
            Array.Copy(mRedOccupancies, 38, mRed1Occupancies, 71, 6);
            Array.Copy(mRedSpeeds, 38, mRed1Speeds, 71, 6);
            Array.Copy(mRedAuthorities, 38, mRed1Authorities, 71, 6);
            Array.Copy(mRedCrossings, 38, mRed1Crossings, 71, 6);
            Array.Copy(mRedSwitches, 38, mRed1Switches, 71, 6);
            Array.Copy(mRedLeftLights, 38, mRed1LeftLights, 71, 6);
            Array.Copy(mRedRightLights, 38, mRed1RightLights, 71, 6);

            Array.Copy(mRed2MaintenanceBlocks, 0, mRedMaintenanceBlocks, 32, 39);
            Array.Copy(mRed2Occupancies, 0, mRedOccupancies, 32, 39);
            Array.Copy(mRed2Speeds, 0, mRedSpeeds, 32, 39);
            Array.Copy(mRed2Authorities, 0, mRedAuthorities, 32, 39);
            Array.Copy(mRed2Crossings, 0, mRedCrossings, 32, 39);
            Array.Copy(mRed2Switches, 0, mRedSwitches, 32, 39);
            Array.Copy(mRed2LeftLights, 0, mRedLeftLights, 32, 39);
            Array.Copy(mRed2RightLights, 0, mRedRightLights, 32, 39);

            return;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // TODO: Add for every module
            if (track != null)
            {
                track.actualClose = true;
                track.Close();
            }
            if (trainCtrl != null)
            {
                trainCtrl.actualClose = true;
                trainCtrl.Close();
            }
            if (trains != null)
            {
                trains.actualClose = true;
                trains.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
