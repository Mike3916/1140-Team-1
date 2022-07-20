using System;
using System.Collections.Generic;
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

        DispatcherTimer globalTimer;

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
            globalTimer = new DispatcherTimer();

            globalTimer.Tick += new EventHandler(updateTick);

            globalTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            globalTimer.Start();
        }

        private void updateTick(object sender, EventArgs e)
        {

        }

        private void CTCButton_Clikc(object sender, RoutedEventArgs e)
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
    }
}
