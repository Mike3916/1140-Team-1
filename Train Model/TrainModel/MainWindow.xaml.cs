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



using Train=TrainObject.Train;
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
        public List<int> TrainIndices = new List<int>();
        public bool actualClose = false;
        public int selectedTrain = 0;
        
       

        public MainWindow()
        {
            InitializeComponent();
           


        }
        
        private void addTrain(int line, int authority=12)
        {
            Trains.Add(new Train(authority, line));
          
                Select_a_Train.Items.Insert(Select_a_Train.Items.Count, "Train " + (Select_a_Train.Items.Count));
            
        }

        private void removeTrain(int index)
        {
            Trains.RemoveAt(index);
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
            physics.Text = "Power:\nCurrent Mass: " + Trains[i].getMass().ToString() + " tons\nForce (P/V): " + Math.Round(Trains[i].getForce(), 2).ToString() + " N\nAcceleration (F/M): " + Trains[i].getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Trains[i].getCurrentSpeedMPH().ToString() + " Mi/h";
            Speed.Text = "Current Speed: " + Math.Round(Trains[i].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[i].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[i].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[i].getTimeTillNextBlock() + "s";
            power.Text = Trains[i].getPowerCmd().ToString() + " kW";
            Beacon.Text = "Beacon: " + Trains[i].getBeacon();

            non_Vitals.Text = "Number of Passengers: " + Trains[i].getPassengers() + "\nNumber of Crew: " + Trains[i].getCrew() + "\nNumber of Cars: " + Trains[i].getCars().ToString() + "\nCapacity: " + Trains[i].getCapacity().ToString() + "\nInterior Lights: " + Trains[i].getInteriorLights() +"Exterior Lights: " + Trains[i].getExteriorLights()+"\nDoors: L-Open\t\tR-Closed\nTemperature: " + Trains[i].getTemperature().ToString() + "F";


            return Trains[i].askForInfo();

        }


       

      

        private void Emergency_Brake(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleEmergencyBrake();
            if (Trains[selectedTrain].getEmergencyBrake()) {
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
            Trains[selectedTrain].toggleServiceBrake();
            if (Trains[selectedTrain].getServiceBrake())
            {
                sBrake.Background = Brushes.Red;
                sBrake.Content = "Service Brake\nOn";
            }
            else
            {
                sBrake.Background = Brushes.Green;
                sBrake.Content = "Service Brake\nOff";
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
            if (e.Key == Key.Enter) {
                double i;
                double.TryParse(power.Text, out i);

                if (i==0 && power.Text.Length>2)
                {
                    
                    double.TryParse(power.Text.Substring(0,power.Text.Length-2), out i);
                }
                
                if ( i> Train.powerMax)
                {
                    power.Text = Train.powerMax.ToString()+" W";
                    Trains[selectedTrain].setPowerCmd(Train.powerMax);
                    
                }
                else
                {
                    Trains[selectedTrain].setPowerCmd(i);
                    power.Text = i + " W";
                }

                power.Text = Trains[selectedTrain].getPowerCmd().ToString() + " W";

            

            physics.Text = "Power:\nCurrent Mass: " + Trains[selectedTrain].getMass().ToString() + " tons\nForce (P/V): " + Math.Round(Trains[selectedTrain].getForce(),2).ToString() + " N\nAcceleration (F/M): " + Trains[selectedTrain].getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Trains[selectedTrain].getCurrentSpeedMPH().ToString() + " Mi/h";
            Speed.Text = "Current Speed: " + Math.Round(Trains[selectedTrain].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[selectedTrain].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[selectedTrain].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: "+Trains[selectedTrain].getTimeTillNextBlock()+"s";

            }
        }

        private void increment_Click(object sender, RoutedEventArgs e)
        {
           
            Trains[selectedTrain].increment();
            physics.Text = "Power:\nCurrent Mass: " + Trains[selectedTrain].getMass().ToString() + " tons\nForce (P/V): " + Math.Round(Trains[selectedTrain].getForce(), 2).ToString() + " N\nAcceleration (F/M): " + Trains[selectedTrain].getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Trains[selectedTrain].getCurrentSpeedMPH().ToString() + " Mi/h";
            Speed.Text = "Current Speed: " + Math.Round(Trains[selectedTrain].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[selectedTrain].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[selectedTrain].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[selectedTrain].getTimeTillNextBlock() + "s";
            power.Text = Trains[selectedTrain].getPowerCmd().ToString() + " W";
            Beacon.Text = "Beacon: " + Trains[selectedTrain].getBeacon();
        }

       private void Lights_Click(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleLights();
            if (Trains[selectedTrain].getLights()) {
                if(!Trains[selectedTrain].getDoorR() && !Trains[selectedTrain].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else if (Trains[selectedTrain].getDoorR() && !Trains[selectedTrain].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Open\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else if(!Trains[selectedTrain].getDoorR() && Trains[selectedTrain].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Closed\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Open\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
            }
            if (!Trains[selectedTrain].getLights())
            {
                if (!Trains[selectedTrain].getDoorR() && !Trains[selectedTrain].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else if (Trains[selectedTrain].getDoorR() && !Trains[selectedTrain].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Open\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else if (!Trains[selectedTrain].getDoorR() && Trains[selectedTrain].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Closed\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Open\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
            }            
        }
      
       
        private void doorL_Click(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleDoorL();
            writeDoors();

        }

        private void doorR_Click(object sender, RoutedEventArgs e)
        {
            Trains[selectedTrain].toggleDoorR();
            writeDoors();
        }

        private void writeDoors()
        {
            if (Trains[selectedTrain].getDoorL() && !Trains[selectedTrain].getDoorR())
            {
                if (!Trains[selectedTrain].getLights())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Closed\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Closed\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
            }
            if (Trains[selectedTrain].getDoorL() && Trains[selectedTrain].getDoorR())
            {
                if (!Trains[selectedTrain].getLights())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Open\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Open\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
            }
            if (!Trains[selectedTrain].getDoorL() && Trains[selectedTrain].getDoorR())
            {
                if (!Trains[selectedTrain].getLights())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Open\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Open\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
            }
            if (!Trains[selectedTrain].getDoorL() && !Trains[selectedTrain].getDoorR()){
                if (!Trains[selectedTrain].getLights())
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";
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
            Select_a_Train.IsEnabled = true;
            Position.Text = "Current Block: 10\nAuthority: " + Trains[selectedTrain].getCmdAuthority().ToString() + " Blocks\nLast Station: Castle Shannon\nNext Station: Dormont";
            Speed.Text = "Current Speed: " + Math.Round(Trains[selectedTrain].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[selectedTrain].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[selectedTrain].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[selectedTrain].getTimeTillNextBlock() + "s";
            Passed_Through_Variables.Text = "Speed Limit: 50Mi/h\nCommanded Authority: 85 Blocks";
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
            power.IsEnabled = true;
            increment.IsEnabled = true;
            lights.IsEnabled = true;
            doorL.IsEnabled = true;
            doorR.IsEnabled = true;

            power.Text = Trains[selectedTrain].getPowerCmd().ToString() + " W";

            physics.Text = "Power:\nCurrent Mass: " + Trains[selectedTrain].getMass().ToString() + " tons\nForce (P/V): " + Math.Round(Trains[selectedTrain].getForce(), 2).ToString() + " N\nAcceleration (F/M): " + Trains[selectedTrain].getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Trains[selectedTrain].getCurrentSpeedMPH().ToString() + " Mi/h";
            Speed.Text = "Current Speed: " + Math.Round(Trains[selectedTrain].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[selectedTrain].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[selectedTrain].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[selectedTrain].getTimeTillNextBlock() + "s";


            non_Vitals.Text = "Number of Passengers: " + Trains[selectedTrain].getPassengers() + "\nNumber of Crew: " + Trains[selectedTrain].getCrew() + "\nNumber of Cars: " + Trains[selectedTrain].getCars().ToString() + "\nCapacity: " + Trains[selectedTrain].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[selectedTrain].getTemperature().ToString() + "F";

            currLine.Text = Trains[selectedTrain].getLineName();
        }
    }
}
