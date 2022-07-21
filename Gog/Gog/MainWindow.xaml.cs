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
        Track_Controller_1._02.Controller mGreenLine1 = new Track_Controller_1._02.Controller(852, false, "127.0.0.1");


        DispatcherTimer mGlobalTimer;
        int mIterationMultiplier = 1;

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
                //trainCtrl.checkUpdatedValues();
                //ctc.SetTrackData(track.);
                //track.GetT

                /*for (int i = 0; i < trains.trainList.Count; i++)
                {
                    newBlock=trains.UpdateValues(trainCtrl.mTrainSetList[i],i);
                    trainCtrl.UpdateValues(trains.Trains[i].cmdauth, trains.Trains[i].curauth, trains.Trains[i].getCommandedSpeed(), trains.Trains[i].getVelocity(), trains.Trains[i].beacon, trains.Trains[i].underground, trains.Trains[i].leftdoors, trains.Trains[i].rightdoors, i);
                                    
                    if(newBlock){
                        trains.updateBlock(trackModel.nextBlock(i)),i); //trackModel.nextBlock(i) moves the train to the next block on it's map and it returns the block info it moved to ***JOE TALK TO HOWARD FOR HELP HERE***
                    
                    }
                }*/

            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            // TODO: Add for every module

            trainCtrl.actualClose = true;
            trainCtrl.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
