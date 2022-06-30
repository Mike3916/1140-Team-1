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

        Train train;
        public MainWindow()
        {
            InitializeComponent();
            train = new Train();
            train.setAuthority(35);
            train.setCommandedSpeed(17.8816);
            train.setPowerCmd(0);


        }

      



        private void Select_a_Line_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Select_a_Train.IsEnabled = true;

        }

        private void Select_a_Train_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Position.Text = "Current Block: 10\nAuthority: " + train.getAuthority().ToString() + " Blocks\nLast Station: Castle Shannon\nNext Station: Dormont";
            Speed.Text = "Current Speed: " + Math.Round(train.getCurrentSpeedMPH(),2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(train.getCommandedSpeedMPH(),2).ToString() + "Mi/h\nCurrent Acceleration: " + train.getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: 7s";
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

            non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Closed\nTemperature: " + train.getTemperature().ToString() + "F";
        }

        private void Emergency_Brake(object sender, RoutedEventArgs e)
        {
            train.toggleEmergencyBrake();
            if (train.getEmergencyBrake()) {
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
            train.toggleServiceBrake();
            if (train.getServiceBrake())
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
            train.toggleServiceBrake();
            if (train.getServiceBrake())
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
            train.toggleEngineFailure();
            if (train.getEngineFailure())
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
                    train.setPowerCmd(Train.powerMax);
                    
                }
                else
                {
                    train.setPowerCmd(i);
                    power.Text = i + " kW";
                }

                power.Text = train.getPowerCmd().ToString() + " kW";

            

            physics.Text = "Power:\nCurrent Mass: " + train.getMass().ToString() + " tons\nForce (P/V): " + Math.Round(train.getForce(),2).ToString() + " N\nAcceleration (F/M): " + train.getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + train.getCurrentSpeedMPH().ToString() + " Mi/h";
            Speed.Text = "Current Speed: " + Math.Round(train.getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(train.getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + train.getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: 7s";

            }
        }

        private void increment_Click(object sender, RoutedEventArgs e)
        {

            train.increment();
            physics.Text = "Power:\nCurrent Mass: " + train.getMass().ToString() + " tons\nForce (P/V): " + Math.Round(train.getForce(), 2).ToString() + " N\nAcceleration (F/M): " + train.getAccelerationFPS().ToString() + " ft/s^2\nVelocity(V_(n - 1) + T / 2(A_n + A_(n - 1)): " + train.getCurrentSpeedMPH().ToString() + " Mi/h";
            Speed.Text = "Current Speed: " + Math.Round(train.getCurrentSpeedMPH(), 2).ToString() + "Mi/h\nCommanded Speed: " + Math.Round(train.getCommandedSpeedMPH(), 2).ToString() + "Mi/h\nCurrent Acceleration: " + train.getAccelerationFPS().ToString() + "ft/s^2\nTime to Next Block: 7s";
            power.Text = train.getPowerCmd().ToString() + " kW";
        }

        private void Lights_Click(object sender, RoutedEventArgs e)
        {
            train.toggleLights();
            if (train.getLights()) {
                if(!train.getDoorR() && !train.getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Closed\nTemperature: " + train.getTemperature().ToString() + "F";
                else if (train.getDoorR() && !train.getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Open\nTemperature: " + train.getTemperature().ToString() + "F";
                else if(!train.getDoorR() && train.getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Closed\nTemperature: " + train.getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Open\nTemperature: " + train.getTemperature().ToString() + "F";
            }
            if (!train.getLights())
            {
                if (!train.getDoorR() && !train.getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Closed\nTemperature: " + train.getTemperature().ToString() + "F";
                else if (train.getDoorR() && !train.getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Open\nTemperature: " + train.getTemperature().ToString() + "F";
                else if (!train.getDoorR() && train.getDoorL())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Closed\nTemperature: " + train.getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Open\nTemperature: " + train.getTemperature().ToString() + "F";
            }            
        }

        private void doorL_Click(object sender, RoutedEventArgs e)
        {
            train.toggleDoorL();
            writeDoors();

        }

        private void doorR_Click(object sender, RoutedEventArgs e)
        {
            train.toggleDoorR();
            writeDoors();
        }

        private void writeDoors()
        {
            if (train.getDoorL() && !train.getDoorR())
            {
                if (!train.getLights())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Closed\nTemperature: " + train.getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Closed\nTemperature: " + train.getTemperature().ToString() + "F";
            }
            if (train.getDoorL() && train.getDoorR())
            {
                if (!train.getLights())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: Off\nDoors: L-Open\t\tR-Open\nTemperature: " + train.getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: On\nDoors: L-Open\t\tR-Open\nTemperature: " + train.getTemperature().ToString() + "F";
            }
            if (!train.getDoorL() && train.getDoorR())
            {
                if (!train.getLights())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Open\nTemperature: " + train.getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Open\nTemperature: " + train.getTemperature().ToString() + "F";
            }
            if (!train.getDoorL() && !train.getDoorR()){
                if (!train.getLights())
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: Off\nDoors: L-Closed\t\tR-Closed\nTemperature: " + train.getTemperature().ToString() + "F";
                else
                    non_Vitals.Text = "Number of Passengers: " + train.getPassengers() + "\nNumber of Crew: " + train.getCrew() + "\nNumber of Cars: " + train.getCars().ToString() + "\nCapacity: " + train.getCapacity().ToString() + "\nLights: On\nDoors: L-Closed\t\tR-Closed\nTemperature: " + train.getTemperature().ToString() + "F";
            }
        }

        
    }
}
