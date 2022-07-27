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
        /*Track_Controller_1._02.Controller mRedline1 = new Track_Controller_1._02.Controller(851, false, "127.0.0.1");
        Track_Controller_1._02.Controller mGreenLine1 = new Track_Controller_1._02.Controller(852, false, "127.0.0.1");

        int[] mRedMaintenanceBlocks = new int[77];
        int[] mRedOccupancies = new int[77];
        int[] mRedSpeeds = new int[77];
        int[] mRedAuthorities = new int[77];
        int[] mRedCrossings = new int[77];
        int[] mRedSwitches = new int[77];
        int[] mRedLeftLights = new int[77];
        int[] mRedRightLights = new int[77];

        int[] mGreenMaintenanceBlocks = new int[151];
        int[] mGreenOccupancies = new int[151];
        int[] mGreenSpeeds = new int[151];
        int[] mGreenAuthorities = new int[151];
        int[] mGreenCrossings = new int[151];
        int[] mGreenSwitches = new int[151];
        int[] mGreenLeftLights = new int[151];
        int[] mGreenRightLights = new int[151];*/


        DispatcherTimer mGlobalTimer;
        int mIterationMultiplier = 100, numTrains = 0, numTrainCtrls = 0, iter = 0;
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

            mGlobalTimer.Interval = new TimeSpan(0, 0, 0, 0, 1); //1 millisecond
            mGlobalTimer.Start();

        }

        private void updateTick(object sender, EventArgs e)
        {
            for (int i = 0; i < mIterationMultiplier; i++)
            {
                if (iter++ % 10 == 0) TimeBox.Text = (iter).ToString();
                /*Mike's edits
                 * How this works is you will send arrays of all the blocks
                 * Track_Controller_1.SendMaintenance sends all the current block maintenance requests
                 * and returns all the blocks placed under maintenance.
                 * SendOccupancies sends all of the occupancies and returns the states of all the occupancies
                 * so on and so forth.
                 * */
                /*try
                {
                    mRedline1.SendMaintenance(mRedMaintenanceBlocks);
                    mRedMaintenanceBlocks = mRedline1.ReceiveMaintenance();
                    mRedOccupancies = mRedline1.SendOccupancies(mRedOccupancies);
                    mRedSpeeds = mRedline1.SendSpeeds(mRedSpeeds);
                    mRedAuthorities = mRedline1.SendAuthorities(mRedAuthorities);
                    mRedCrossings = mRedline1.SendCrossings(mRedCrossings);
                    mRedSwitches = mRedline1.SendSwitches(mRedSwitches);
                    mRedLeftLights = mRedline1.SendLeftLights(mRedLeftLights);
                    mRedRightLights = mRedline1.SendRightLights(mRedRightLights);

                    mGreenMaintenanceBlocks = mGreenLine1.SendMaintenance(mGreenMaintenanceBlocks);
                    mGreenOccupancies = mGreenLine1.SendOccupancies(mGreenOccupancies);
                    mGreenSpeeds = mGreenLine1.SendSpeeds(mGreenSpeeds);
                    mGreenAuthorities = mGreenLine1.SendAuthorities(mGreenAuthorities);
                    mGreenCrossings = mGreenLine1.SendCrossings(mGreenCrossings);
                    mGreenSwitches = mGreenLine1.SendSwitches(mGreenSwitches);
                    mGreenLeftLights = mGreenLine1.SendLeftLights(mGreenLeftLights);
                    mGreenRightLights = mGreenLine1.SendRightLights(mGreenRightLights);
                }
                catch
                {

                }*/

                
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
                    newBlock=trains.UpdateValues(trainCtrl.mTrainSetList[j],j);
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
