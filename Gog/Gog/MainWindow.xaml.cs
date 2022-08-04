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


namespace Gog
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
        int[] mRedRoute = new int[77];
        bool mRedTrain = false;

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
        int[] mGreenRoute = new int[151];
        bool mGreenTrain = false;

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
        int mIterationMultiplier = 1, numTrains = 0, iter = 0;
        bool newBlock;
        bool gotTrack = false;
        bool paused = true;

        int hour, minute, second;
        string hourString, minuteString, secondString;

        public MainWindow()
        {
            InitializeComponent();

            DateTime now = DateTime.Now;
            LiveTimeLabel.Content = now.ToString("HH:mm:ss");

            hour = now.Hour;
            minute = now.Minute;
            second = now.Second;

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

        public void SpeedControl(object sender, RoutedEventArgs e)
        {
            if (mIterationMultiplier == 1)
            {
                mIterationMultiplier = 10;
                SpeedMultiplier.Content = "Clock speed (10x)";
            }
            else
            {
                mIterationMultiplier = 1;
                SpeedMultiplier.Content = "Clock speed (1x)";
            }
        }

        public void PauseControl(object sender, RoutedEventArgs e)
        {
            if (paused)
            {
                paused = false;
                mGlobalTimer.Start();
                Pause.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF28C347"));
                Pause.Content = "Running";
            }
            else
            {
                paused = true;
                mGlobalTimer.Stop();
                Pause.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                Pause.Content = "Paused";
            }
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
        }

        private void updateTick(object sender, EventArgs e)
        {
            for (int i = 0; i < mIterationMultiplier; i++)
            {
                if (iter++ == 63)
                {
                    iter = 0;

                    if (second++ == 59)
                    {
                        second = 0;
                        
                        if (minute++ == 59)
                        {
                            minute = 0;

                            if (hour++ == 23)
                            {
                                hour = 0;
                            }
                        }
                    }

                    if (second < 10)
                    {
                        secondString = "0" + second.ToString();
                    }
                    else
                    {
                        secondString = second.ToString();
                    }

                    if (minute < 10)
                    {
                        minuteString = "0" + minute.ToString();
                    }
                    else
                    {
                        minuteString = minute.ToString();
                    }

                    if (hour < 10)
                    {
                        hourString = "0" + hour.ToString();
                    }
                    else
                    {
                        hourString = hour.ToString();
                    }

                    LiveTimeLabel.Content = hourString + ":" + minuteString + ":" + secondString;
                }

                if (track != null && ctc != null && track.mLines.Count == 2)    //As long as track and ctc both exist, and the track has not been sent to the CTC yet,
                {
                    //Update Track Controller variables with values from CTC
                    mRedMaintenanceBlocks = ctc.mRedMaintenanceBlocks;
                    mRedOccupancies = ctc.mRedOccupancies;
                    mRedSpeeds = ctc.mRedSpeeds;
                    mRedAuthorities = ctc.mRedAuthorities;
                    mRedCrossings = ctc.mRedCrossings;
                    mRedSwitches = ctc.mRedSwitches;
                    mRedLeftLights = ctc.mRedLeftLights;
                    mRedRightLights = ctc.mRedRightLights;
                    mRedTrain = ctc.mRedTrain;
                    if(ctc.mRedTrain == true)
                    {
                        MessageBox.Show(mRedTrain.ToString() + " " + ctc.mRedTrain.ToString());
                    }
                    mGreenMaintenanceBlocks = ctc.mGreenMaintenanceBlocks;
                    mGreenOccupancies = ctc.mGreenOccupancies;
                    mGreenSpeeds = ctc.mGreenSpeeds;
                    mGreenAuthorities = ctc.mGreenAuthorities;
                    mGreenCrossings = ctc.mGreenCrossings;
                    mGreenSwitches = ctc.mGreenSwitches;
                    mGreenLeftLights = ctc.mGreenLeftLights;
                    mGreenRightLights = ctc.mGreenRightLights;
                    mGreenTrain = ctc.mGreenTrain;

                    PLCgetCTC();
                    PLCgetTrack();
                }

                if (track != null && ctc != null && track.mLines.Count == 2 && iter % 20 == 0)
                {
                    PLCsetTrack();
                    PLCsetCTC();
                    ctc.GetTrackController(mRedMaintenanceBlocks, mRedOccupancies, mRedSpeeds, mRedAuthorities, mRedCrossings, mRedSwitches, mRedLeftLights, mRedRightLights, mGreenMaintenanceBlocks, mGreenOccupancies, mGreenSpeeds, mGreenAuthorities, mGreenCrossings, mGreenSwitches, mGreenLeftLights, mGreenRightLights); //Write function in CTC to read in these values 
                }
                if (ctc != null) //This sends current simulation time to ctc (needed for ETD for dispatching a train)
                {
                    var now = new DateTime(1,1,1, hour, minute, second);
                    ctc.getTime(now);
                }
                    

                if (track != null && ctc != null && gotTrack==false && track.mLines.Count == 2)    //As long as track and ctc both exist, and the track has not been sent to the CTC yet,
                {                                                       
                    if (track.mLines.Count > 0)                         //Make sure the track files are loaded into the TrackModel module BEFORE the CTC model (button) is pressed to make sure the CTC will always get the full track
                    {
                        ctc.GetTrackLayout(track.mLines);                 //Send the track data to the CTC
                        gotTrack = true;                                //Set boolean to mark that the track data has been read by CTC
                    }
                }

                if (track != null && ctc != null &&  track.mLines.Count == 2)    //As long as track and ctc both exist, and the track has not been sent to the CTC yet,
                {
                    
                                                                                                                                                                                                                                                                                                                                  
                    
                }

                if (mRedTrain == true)
                {
                    MessageBox.Show("Authority Passed : " + mRedAuthorities[76].ToString());
                    track.AddTrain(0, mRedAuthorities[76]);
                    trains.addTrain(0, mRedAuthorities[76]);
                    trainCtrl.addController((false));
                    numTrains++;
                    
                }
                if(mGreenTrain == true)
                {
                    track.AddTrain(1, mGreenAuthorities[150]);
                    trains.addTrain(1, mGreenAuthorities[150]);
                    trainCtrl.addController((true));
                    numTrains++;
                   
                }
               
                mRedline1.SendTrain(false);
                mGreenLine1.SendTrain(false);
                ctc.mRedTrain = false;
                ctc.mGreenTrain = false;





                for (int j = 0; j < numTrains; j++)
                {
                    newBlock = trains.UpdateValues(trainCtrl.mTrainSetList[j],j);
                    trainCtrl.UpdateValues(trains.Trains[j].getCmdAuthority(), trains.Trains[j].getCurrAuthority(), trains.Trains[j].getCommandedSpeedMPH(), trains.Trains[j].getCurrentSpeedMPH(), trains.Trains[j].getBeacon(), trains.Trains[j].getUnderground(), trains.Trains[j].getDoorL(), trains.Trains[j].getDoorR(), j);

                    if (newBlock)                  //if train at j enters a new block
                    { 
                        TrackModel.Block bl = track.UpdateTrain(j); //get next block
                        if (bl != null)                             //if that block exists
                        {
                            MessageBox.Show("I am in the block");
                            trains.UpdateBlock(bl, track.mtrainList[j].commAuthority , j);              //update the train pos
                            if (bl.mStation)
                            {
                                track.SetPopulation(trains.Trains[j].UpdatePassenger(bl.mPop), bl);
                            }
                        }
                        else
                        {                                         //if that block doesn't exist ...
                            trains.RemoveTrain(j);             //delete the train
                            track.RemoveTrain(j);              //remove the train from the track
                            numTrains--;
                        }
                    }
                }
            }
        }

        private void PLCgetCTC()
        {

            ArraySplitter();
            if(mRedTrain == true)
            {
                MessageBox.Show(mRedAuthorities[76].ToString() + " " + mRed1Authorities[43].ToString());
            }
            mRedline1.SendMaintenance(mRed1MaintenanceBlocks);
            mRedline1.SendSpeeds(mRed1Speeds);
            mRedline1.SendAuthorities(mRed1Authorities);
            mRedline1.SendSwitches(mRed1Switches);
            mRedline2.SendMaintenance(mRed2MaintenanceBlocks);
            mRedline2.SendSpeeds(mRed2Speeds);
            mRedline2.SendAuthorities(mRed2Authorities);
            mRedline2.SendSwitches(mRed2Switches);
            mRedline1.SendRoute(mRedRoute);
            mRedline1.SendTrain(mRedTrain);
            Array.Clear(mRedRoute,0,mRedRoute.Length);

            mGreenLine1.SendMaintenance(mGreen1MaintenanceBlocks);
            mGreenLine1.SendSpeeds(mGreen1Speeds);
            mGreenLine1.SendAuthorities(mGreen1Authorities);
            mGreenLine1.SendSwitches(mGreen1Switches);
            mGreenLine1.SendRoute(mGreenRoute);
            mGreenLine1.SendTrain(mGreenTrain);

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

            ctc.mRedTrain = false;
            ctc.mGreenTrain = false;

            ArrayMerger();
           
        }

        private void PLCgetTrack()
        {
            //77 ELEMENT array for red
            mRedOccupancies = track.OccupiedBlocks(0).ToArray();

            //151 ELEMENT array for red
            mGreenOccupancies = track.OccupiedBlocks(1).ToArray();

            ArraySplitter();

            mRedline1.SendOccupancies(mRed1Occupancies);
            mRedline2.SendOccupancies(mRed2Occupancies);
            mGreenLine1.SendOccupancies(mGreen1Occupancies);

        }

        private void PLCsetTrack()
        {
            mRed1Speeds = mRedline1.ReceiveSpeeds(mRed1Speeds.Length);
            mRed1Authorities = mRedline1.ReceiveAuthorities(mRed1Authorities.Length);
            mRed1Crossings = mRedline1.ReceiveCrossings(mRed1Crossings.Length);
            mRed1Switches = mRedline1.ReceiveSwitches(mRed1Switches.Length);
            mRed1RightLights = mRedline1.ReceiveRightLights(mRed1RightLights.Length);
            mRed1LeftLights = mRedline1.ReceiveLeftLights(mRed1LeftLights.Length);
            mRed2Speeds = mRedline2.ReceiveSpeeds(mRed2Speeds.Length);
            mRed2Authorities = mRedline2.ReceiveAuthorities(mRed2Authorities.Length);
            mRed2Crossings = mRedline2.ReceiveCrossings(mRed2Crossings.Length);
            mRed2Switches = mRedline2.ReceiveSwitches(mRed2Switches.Length);
            mRed2RightLights = mRedline2.ReceiveRightLights(mRed2RightLights.Length);
            mRed2LeftLights = mRedline2.ReceiveLeftLights(mRed2LeftLights.Length);
            mRedRoute = mRedline1.ReceiveRoute(mRedRoute.Length);
            mRedTrain = mRedline1.ReceiveTrain();

            mGreenSpeeds = mGreenLine1.ReceiveSpeeds(mGreenSpeeds.Length);
            mGreenAuthorities = mGreenLine1.ReceiveAuthorities(mGreenAuthorities.Length);
            mGreenCrossings = mGreenLine1.ReceiveCrossings(mGreenCrossings.Length);
            mGreenSwitches = mGreenLine1.ReceiveSwitches(mGreenSwitches.Length);
            mGreenRightLights = mGreenLine1.ReceiveRightLights(mGreenRightLights.Length);
            mGreenLeftLights = mGreenLine1.ReceiveLeftLights(mGreenLeftLights.Length);
            mGreenTrain = mGreenLine1.ReceiveTrain();

            ArrayMerger();

            track.SetSpeeds(mRedSpeeds, 0);
            track.SetAuthorities(mRedAuthorities, 0);
            track.SetCrossings(mRedCrossings, 0);
            track.SetSwitches(mRedSwitches, 0);

            track.SetSpeeds(mGreenSpeeds, 1);
            track.SetAuthorities(mGreenAuthorities, 1);
            track.SetCrossings(mGreenCrossings, 1);
            track.SetSwitches(mGreenSwitches, 1);




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

            Array.Copy(mRed1MaintenanceBlocks, 38, mRedMaintenanceBlocks, 71, 6);
            Array.Copy(mRed1Occupancies, 38, mRedOccupancies, 71, 6);
            Array.Copy(mRed1Speeds, 38, mRedSpeeds, 71, 6);
            Array.Copy(mRed1Authorities, 38, mRedAuthorities, 71, 6);
            Array.Copy(mRed1Crossings, 38, mRedCrossings, 71, 6);
            Array.Copy(mRed1Switches, 38, mRedSwitches, 71, 6);
            Array.Copy(mRed1LeftLights, 38, mRedLeftLights, 71, 6);
            Array.Copy(mRed1RightLights, 38, mRedRightLights, 71, 6);

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
                trainCtrl.mActualClose = true;
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
