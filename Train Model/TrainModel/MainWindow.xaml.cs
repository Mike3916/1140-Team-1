using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
       

        public MainWindow()
        {
            InitializeComponent();
            Trains.Add(new Train());
            Trains[0] = new Train();
            Trains[0].setCmdAuthority(35);
            Trains[0].setCommandedSpeed(17.8816);
            Trains[0].setPowerCmd(0);


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
            
            return Trains[i].askForInfo();

        }


        private void Select_a_Line_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Select_a_Train.IsEnabled = true;

        }

        private void Select_a_Train_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Position.Text = "Current Block: 10\nAuthority: " + Trains[0].getCmdAuthority().ToString() + " Blocks\nLast Station: Castle Shannon\nNext Station: Dormont";
            Speed.Text = "Current Speed: " + Math.Round(Trains[0].getCurrentSpeedMPH(),2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[0].getCommandedSpeedMPH(),2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[0].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: "+Trains[0].getTimeTillNextBlock()+"s";
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

            non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
        }

        private void Emergency_Brake(object sender, RoutedEventArgs e)
        {
            Trains[0].toggleEmergencyBrake();
            if (Trains[0].getEmergencyBrake()) {
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
            Trains[0].toggleServiceBrake();
            if (Trains[0].getServiceBrake())
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
            Trains[0].toggleServiceBrake();
            if (Trains[0].getServiceBrake())
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
            Trains[0].toggleEngineFailure();
            if (Trains[0].getEngineFailure())
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
                    power.Text = Train.powerMax.ToString()+" kW";
                    Trains[0].setPowerCmd(Train.powerMax);
                    
                }
                else
                {
                    Trains[0].setPowerCmd(i);
                    power.Text = i + " kW";
                }

                power.Text = Trains[0].getPowerCmd().ToString() + " kW";

            

            physics.Text = "Power:\nCurrent Mass: " + Trains[0].getMass().ToString() + " tons\nForce (P/V): " + Math.Round(Trains[0].getForce(),2).ToString() + " N\nAcceleration (F/M): " + Trains[0].getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Trains[0].getCurrentSpeedMPH().ToString() + " Mi/h";
            Speed.Text = "Current Speed: " + Math.Round(Trains[0].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[0].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[0].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: "+Trains[0].getTimeTillNextBlock()+"s";

            }
        }

        private void increment_Click(object sender, RoutedEventArgs e)
        {

            Trains[0].increment();
            physics.Text = "Power:\nCurrent Mass: " + Trains[0].getMass().ToString() + " tons\nForce (P/V): " + Math.Round(Trains[0].getForce(), 2).ToString() + " N\nAcceleration (F/M): " + Trains[0].getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + Trains[0].getCurrentSpeedMPH().ToString() + " Mi/h";
            Speed.Text = "Current Speed: " + Math.Round(Trains[0].getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(Trains[0].getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + Trains[0].getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: " + Trains[0].getTimeTillNextBlock() + "s";
            power.Text = Trains[0].getPowerCmd().ToString() + " kW";
        }

       private void Lights_Click(object sender, RoutedEventArgs e)
        {
            Trains[0].toggleLights();
            if (Trains[0].getLights()) {
                if(!Trains[0].getDoorR() && !Trains[0].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else if (Trains[0].getDoorR() && !Trains[0].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Open\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else if(!Trains[0].getDoorR() && Trains[0].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Closed\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Open\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
            }
            if (!Trains[0].getLights())
            {
                if (!Trains[0].getDoorR() && !Trains[0].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else if (Trains[0].getDoorR() && !Trains[0].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Open\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else if (!Trains[0].getDoorR() && Trains[0].getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Closed\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Open\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
            }            
        }
      
       
        private void doorL_Click(object sender, RoutedEventArgs e)
        {
            Trains[0].toggleDoorL();
            writeDoors();

        }

        private void doorR_Click(object sender, RoutedEventArgs e)
        {
            Trains[0].toggleDoorR();
            writeDoors();
        }

        private void writeDoors()
        {
            if (Trains[0].getDoorL() && !Trains[0].getDoorR())
            {
                if (!Trains[0].getLights())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Closed\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Closed\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
            }
            if (Trains[0].getDoorL() && Trains[0].getDoorR())
            {
                if (!Trains[0].getLights())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Open\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Open\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
            }
            if (!Trains[0].getDoorL() && Trains[0].getDoorR())
            {
                if (!Trains[0].getLights())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Open\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Open\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
            }
            if (!Trains[0].getDoorL() && !Trains[0].getDoorR()){
                if (!Trains[0].getLights())
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + Trains[0].getPassengers() + "\nNumber of Crew: " + Trains[0].getCrew() + "\nNumber of Cars: " + Trains[0].getCars().ToString() + "\nCapacity: " + Trains[0].getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Closed\nTemperature: " + Trains[0].getTemperature().ToString() + "F";
            }
        }

        
    }
}
