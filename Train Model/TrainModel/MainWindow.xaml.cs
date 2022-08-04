using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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



using Train = TrainObject.Train;
using TrainCtrl = TrainController.Controller;

namespace TrainModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    class railLines : ObservableCollection<string>
    {
        public railLines()
        {

            Add("Green");
            Add("Red");
            Add("Blue");
        }
    }
    public partial class MainWindow : Window
    {

        public List<Train> Trains = new List<Train>();

        public bool actualClose = false;
        public int selectedTrain = 0;
        public bool testModeSet = false;
        public int click = 0;



        public MainWindow()
        {
            InitializeComponent();



        }

        public void addTrain(int line, int authority = 12)
        {
            Trains.Add(new Train(authority, line));
            string name = "Train " + Trains[Trains.Count - 1].getID().ToString();
            Select_a_Train.Items.Insert(Trains.Count - 1, name);
            Select_a_Train.SelectedIndex = Trains.Count - 1;

        }

        public void RemoveTrain(int index)
        {
            for (int i = 0; i < Trains.Count; i++)
                System.Diagnostics.Debug.WriteLine(Trains[i].ToString() + " " + i.ToString() + " " + Trains.Count.ToString());
            Trains.RemoveAt(index);
            for (int i = 0; i < Trains.Count; i++)
                System.Diagnostics.Debug.WriteLine(Trains[i].ToString() + " " + i.ToString() + " " + Trains.Count.ToString());
            Select_a_Train.Items.RemoveAt(index);

        }


        public bool UpdateValues(TrainCtrl ctrl, int i)
        {
            if (ctrl.mLeftDoorsStatus != Trains[i].getDoorL())
            {
                Trains[i].toggleDoorL();
            }
            if (ctrl.mRightDoorsStatus != Trains[i].getDoorR())
            {
                Trains[i].toggleDoorR();
            }
            if (ctrl.mInteriorLightsStatus != Trains[i].getInteriorLights())
            {
                Trains[i].toggleInteriorLights();
            }
            if (ctrl.mExteriorLightsStatus != Trains[i].getExteriorLights())
            {
                Trains[i].toggleExteriorLights();
            }
            Trains[i].setAnnouncement(ctrl.mAnnouncementsStatus);
            if (ctrl.mServiceBrakeStatus != Trains[i].getServiceBrake())
            {
                Trains[i].toggleServiceBrake();
            }
            if (ctrl.mEmergencyBrakeStatus != Trains[i].getEmergencyBrake())
            {
                Trains[i].toggleEmergencyBrake();
            }
            Trains[i].setPowerCmd(ctrl.mCurPower);
            Trains[i].setTemperature(ctrl.mTemperature);
            Trains[i].increment();
            physics.Text = "Power:\nCurrent Mass: " + Math.Round(Trains[selectedTrain].getMass(),2).ToString() + " tons\nForce (P/V): " + Math.Round(Trains[selectedTrain].getForce(), 2).ToString() + " N\nAcceleration (F/M): " + Trains[selectedTrain].getAcceleration().ToString() + " m/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Trains[selectedTrain].getCurrentSpeed().ToString() + " m/s";
            Speed.Text = "Current Speed: " + Math.Round(Trains[selectedTrain].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: \nCurrent Acceleration: " + Trains[selectedTrain].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[selectedTrain].getTimeTillNextBlock() + "s";
            power.Text = Trains[selectedTrain].getPowerCmd().ToString() + " W";
            Beacon.Text = "Beacon: " + Trains[selectedTrain].getBeacon();
            non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nInterior Lights:\t\tExterior Lights: \nLeft Doors:\t\tRight Doors: \nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
            Position.Text = "Current Block: " + Trains[selectedTrain].getBlockID() + "\nAuthority: \nLength of Block:\nDistance left on block:" + Trains[selectedTrain].getRemainingDistMF() + " Ft\nGrade:";

            Authority.Text = Trains[selectedTrain].getCmdAuthority().ToString() + " Blocks";
            Grade.Text = Trains[selectedTrain].getGrade().ToString();
            cmdSpeed.Text = Math.Round(Trains[selectedTrain].getCommandedSpeedMPH(),2).ToString() + " mph";

            if (Trains[selectedTrain].getInteriorLights())
                InteriorLight.Text = "On";
            else
            {
                InteriorLight.Text = "Off";
            }

            if (Trains[selectedTrain].getExteriorLights())
                ExteriorLight.Text = "On";
            else
            {
                ExteriorLight.Text = "Off";
            }
            if (Trains[selectedTrain].getDoorL())
            {

                LDoor.Text = "Open";
            }
            else
            {
                LDoor.Text = "Closed";
            }
            if (Trains[selectedTrain].getDoorR())
            {

                RDoor.Text = "Open";
            }
            else
            {
                RDoor.Text = "Closed";
            }

            if (Trains[i].getBaby())
            {
                Trains[i].growUp();
                return true;
            }

            return Trains[i].askForInfo();



        }


        public void UpdateBlock(TrackModel.Block block, int auth, int i)
        {
            Trains[i].setBlockInfo(block, auth);
            length.Text = block.mLength.ToString() + "ft";

        }





        private void Emergency_Brake(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleEmergencyBrake();
            if (Trains[selectedTrain].getEmergencyBrake())
            {
                eBrake.Background = Brushes.Red;
                eBrake.Content = "Emegency Brake\nOn";
            }
            else
            {
                eBrake.Background = Brushes.Green;
                eBrake.Content = "Emegency Brake\nOff";
            }

        }

        private void Signal_Pick_Up(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleSignalPickUp();
            if (Trains[selectedTrain].getSignalPickUp())
            {
                signalPickUp.Background = Brushes.Red;
                signalPickUp.Content = "Signal Pick-up\nNot Working";
            }
            else
            {
                signalPickUp.Background = Brushes.Green;
                signalPickUp.Content = "Signal Pick-up\nWorking";
            }

        }

        private void Service_Brake(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleBrakeFailure();
            if (Trains[selectedTrain].getBrakeFailure())
            {
                sBrake.Background = Brushes.Red;
                sBrake.Content = "Brake Failure\nOn";
            }
            else
            {
                sBrake.Background = Brushes.Green;
                sBrake.Content = "Brake Failure\nOff";
            }

        }

        private void Engine_Failure(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleEngineFailure();
            if (Trains[selectedTrain].getEngineFailure())
            {
                engineFailure.Background = Brushes.Red;
                engineFailure.Content = "Engine Failure\nOn";
            }
            else
            {
                engineFailure.Background = Brushes.Green;
                engineFailure.Content = "Engine Failure\nOff";
            }

        }

        private void Power_Changed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double i;
                double.TryParse(power.Text, out i);

                if (i == 0 && power.Text.Length > 2)
                {

                    double.TryParse(power.Text.Substring(0, power.Text.Length - 2), out i);
                }

                if (i > Train.powerMax)
                {
                    power.Text = Train.powerMax.ToString() + " W";
                    Trains[selectedTrain].setPowerCmd(Train.powerMax);

                }
                else
                {
                    Trains[selectedTrain].setPowerCmd(i);
                    power.Text = i + " W";
                }

                power.Text = Trains[selectedTrain].getPowerCmd().ToString() + " W";



                physics.Text = "Power:\nCurrent Mass: " + Trains[selectedTrain].getMass().ToString() + " tons\nForce (P/V): " + Math.Round(Trains[selectedTrain].getForce(), 2).ToString() + " N\nAcceleration (F/M): " + Trains[selectedTrain].getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Trains[selectedTrain].getCurrentSpeedMPH().ToString() + " Mi/h";
                Speed.Text = "Current Speed: " + Math.Round(Trains[selectedTrain].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[selectedTrain].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[selectedTrain].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[selectedTrain].getTimeTillNextBlock() + "s";

            }
        }

        private void increment_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
                Trains[selectedTrain].increment();


            Position.Text = "Current Block: " + Trains[selectedTrain].getBlockID() + "\nAuthority: " + Trains[selectedTrain].getCmdAuthority().ToString() + " Blocks\nLength of Block:\nDistance left on block:" + Trains[selectedTrain].getRemainingDistMF() + " Ft\nGrade:";
            physics.Text = "Power:\nCurrent Mass: " + Trains[selectedTrain].getMass().ToString() + " tons\nForce (P/V): " + Math.Round(Trains[selectedTrain].getForce(), 2).ToString() + " N\nAcceleration (F/M): " + Math.Round(Trains[selectedTrain].getAcceleration(), 2).ToString() + " m/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Math.Round(Trains[selectedTrain].getCurrentSpeed(), 2).ToString() + " m/s^2";
            Speed.Text = "Current Speed: " + Math.Round(Trains[selectedTrain].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[selectedTrain].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[selectedTrain].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[selectedTrain].getTimeTillNextBlock() + "s";
            power.Text = Trains[selectedTrain].getPowerCmd().ToString() + " W";
            Beacon.Text = "Beacon: " + Trains[selectedTrain].getBeacon();
            Authority.Text = Trains[selectedTrain].getCmdAuthority().ToString() + " Blocks";
            Grade.Text = Trains[selectedTrain].getGrade().ToString();
            non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nInterior Lights:\t\tExterior Lights: \nLeft Doors:\t\tRight Doors: \nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";

            cmdSpeed.Text = Trains[selectedTrain].getCommandedSpeedMPH().ToString() + " mph";


            click++;


        }

        private void Internal_Lights_Click(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleInteriorLights();
            if (Trains[selectedTrain].getInteriorLights())
                InteriorLight.Text = "On";
            else
            {
                InteriorLight.Text = "Off";
            }

        }

        private void Exterior_Lights_Click(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleExteriorLights();
            if (Trains[selectedTrain].getExteriorLights())
                ExteriorLight.Text = "On";
            else
            {
                ExteriorLight.Text = "Off";
            }
        }


        private void doorL_Click(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleDoorL();
            if (Trains[selectedTrain].getDoorL())
            {

                LDoor.Text = "Open";
            }
            else
            {
                LDoor.Text = "Closed";
            }
        }

        private void doorR_Click(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleDoorR();
            if (Trains[selectedTrain].getDoorR())
            {

                RDoor.Text = "Open";
            }
            else
            {
                RDoor.Text = "Closed";
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (!actualClose)
            {
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addTrain(1, 56);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            actualClose = true;
            this.Close();
            this.Close();
        }

        private void Select_a_Train_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            selectedTrain = Select_a_Train.SelectedIndex;
            if (selectedTrain != -1)
            {

                Position.Text = "Current Block: " + Trains[selectedTrain].getBlockID() + "\nAuthority: " + Trains[selectedTrain].getCmdAuthority().ToString() + " Blocks\nLength of Block:\nDistance left on block:" + Trains[selectedTrain].getRemainingDistMF() + " Ft\nGrade:";
                Speed.Text = "Current Speed: " + Math.Round(Trains[selectedTrain].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[selectedTrain].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[selectedTrain].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[selectedTrain].getTimeTillNextBlock() + "s";
                eBrake.Foreground = Brushes.White;
                eBrake.Background = Brushes.Green;
                eBrake.IsEnabled = true;
                signalPickUp.Foreground = Brushes.White;
                signalPickUp.Background = Brushes.Green;
                signalPickUp.IsEnabled = true;
                sBrake.Foreground = Brushes.White;
                sBrake.Background = Brushes.Green;
                sBrake.IsEnabled = true;
                engineFailure.Foreground = Brushes.White;
                engineFailure.Background = Brushes.Green;
                engineFailure.IsEnabled = true;
                testMode.IsEnabled = true;


                power.Text = Trains[selectedTrain].getPowerCmd().ToString() + " W";

                physics.Text = "Power:\nCurrent Mass: " + Trains[selectedTrain].getMass().ToString() + " tons\nForce (P/V): " + Math.Round(Trains[selectedTrain].getForce(), 2).ToString() + " N\nAcceleration (F/M): " + Trains[selectedTrain].getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Trains[selectedTrain].getCurrentSpeedMPH().ToString() + " Mi/h";
                Speed.Text = "Current Speed: " + Math.Round(Trains[selectedTrain].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[selectedTrain].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[selectedTrain].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[selectedTrain].getTimeTillNextBlock() + "s";


                non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nIndoor Lights: \t\tExternal Lights: \nRight Door: \t\tLeft Door: \nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                if (Trains[selectedTrain].getInteriorLights())
                    InteriorLight.Text = "On";
                else
                {
                    InteriorLight.Text = "Off";
                }

                if (Trains[selectedTrain].getExteriorLights())
                    ExteriorLight.Text = "On";
                else
                {
                    ExteriorLight.Text = "Off";
                }
                if (Trains[selectedTrain].getDoorL())
                {

                    LDoor.Text = "Open";
                }
                else
                {
                    LDoor.Text = "Closed";
                }
                if (Trains[selectedTrain].getDoorR())
                {

                    RDoor.Text = "Open";
                }
                else
                {
                    RDoor.Text = "Closed";
                }

                currLine.Text = Trains[selectedTrain].getLineName();

                Authority.Text = Trains[selectedTrain].getCmdAuthority().ToString() + " Blocks";
                length.Text = Trains[selectedTrain].getBlockDistMF().ToString() + "Ft";
                cmdSpeed.Text = Trains[selectedTrain].getCommandedSpeedMPH().ToString()+ " mph";

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        { RemoveTrain(selectedTrain); }

        private void testMode_Click(object sender, RoutedEventArgs e)
        {
            if (!testModeSet)
            {

                cmdSpeed.IsEnabled = true;
                Authority.IsEnabled = true;
                power.IsEnabled = true;
                increment.IsEnabled = true;
                addTrainButton.IsEnabled = true;
                removeTrainButton.IsEnabled = true;
                interiorLights.IsEnabled = true;
                exteriorLights.IsEnabled = true;
                doorL.IsEnabled = true;
                doorR.IsEnabled = true;
                testModeSet = true;
                length.IsEnabled = true;
                Grade.IsEnabled = true;
                cmdSpeed.IsEnabled = true;
            }
            else
            {
                cmdSpeed.IsEnabled = false;
                Authority.IsEnabled = false;
                power.IsEnabled = false;
                increment.IsEnabled = false;
                addTrainButton.IsEnabled = false;
                removeTrainButton.IsEnabled = false;
                interiorLights.IsEnabled = false;
                exteriorLights.IsEnabled = false;
                doorL.IsEnabled = false;
                doorR.IsEnabled = false;
                testModeSet = false;
                length.IsEnabled = false;
                Grade.IsEnabled = false;
                cmdSpeed.IsEnabled = false;
            }
        }

        private void length_TextChanged(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string l = length.Text;
                if (length.Text.Contains("Ft"))
                    l = length.Text.Substring(0, length.Text.Length - 3);
                int L;
                bool worked = int.TryParse(l, out L);
                if (worked)
                    Trains[selectedTrain].setBlockDistFM(L);

                Position.Text = "Current Block: " + Trains[selectedTrain].getBlockID() + "\nAuthority: " + Trains[selectedTrain].getCmdAuthority().ToString() + " Blocks\nLength of Block:\nDistance left on block:" + Trains[selectedTrain].getRemainingDistMF() + " Ft\nGrade:";
                length.Text = L.ToString() + " Ft";

            }


        }

        private void Authority_TextChanged(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int i;
                int.TryParse(Authority.Text, out i);
                if (i == 0 && Authority.Text.Length > 6)
                {

                    int.TryParse(Authority.Text.Substring(0, Authority.Text.Length - 7), out i);
                }

                Trains[selectedTrain].setCmdAuthority(i);

                Authority.Text = Trains[selectedTrain].getCmdAuthority().ToString() + " Blocks";


            }
        }

        private void Grade_Changed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double i;
                double.TryParse(Grade.Text, out i);


                Trains[selectedTrain].setGrade(i);
            }
        }

        private void cmdSpeed_Changed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                double i;
                double.TryParse(cmdSpeed.Text, out i);
                if (i == 0 && cmdSpeed.Text.Length > 3)
                {

                    double.TryParse(cmdSpeed.Text.Substring(0, cmdSpeed.Text.Length - 3), out i);
                }

                Trains[selectedTrain].setCommandedSpeedMPH(i);

                cmdSpeed.Text = Trains[selectedTrain].getCommandedSpeedMPH().ToString() + " mph";
            }
        }
    }
}

