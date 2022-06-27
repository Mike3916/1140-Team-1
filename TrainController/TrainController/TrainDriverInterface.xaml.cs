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

namespace TrainController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Boolean for switching between auto and manual driving modes:
        private bool mAutoMode = false;

        public MainWindow()
        {
            InitializeComponent();

            ManualMode.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == AutoMode)
            {
                AutoMode.IsEnabled = false;
                ManualMode.IsEnabled = true;

                LeftDoors.IsEnabled = false;
                RightDoors.IsEnabled = false;
                LightsInterior.IsEnabled = false;
                LightsExterior.IsEnabled = false;
                Announcements.IsEnabled = false;
                TempIncrease.IsEnabled = false;
                TempDecrease.IsEnabled = false;

                SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0x30, 0, 0, 0));
                
                mAutoMode = true;
            }
            else if (sender == ManualMode)
            {
                ManualMode.IsEnabled = false;
                AutoMode.IsEnabled = true;

                LeftDoors.IsEnabled = true;
                RightDoors.IsEnabled = true;
                LightsInterior.IsEnabled = true;
                LightsExterior.IsEnabled = true;
                Announcements.IsEnabled = true;
                TempIncrease.IsEnabled = true;
                TempDecrease.IsEnabled = true;

                SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xDF, 0x20));

                mAutoMode = false;
            }
            else if (sender == EmergencyBrake)
            {
                MessageBox.Show("Emergency Brakes engaged!");
            }
            else if (sender == LeftDoors)
            {
                MessageBox.Show("Left Doors open");
            }
            else if (sender == RightDoors)
            {
                MessageBox.Show("Right Doors open");
            }
            else if (sender == LightsInterior)
            {
                MessageBox.Show("Interior Lights on");
            }
            else if (sender == LightsExterior)
            {
                MessageBox.Show("Exterior Lights on");
            }
            else if (sender == Announcements)
            {
                MessageBox.Show("Announcements off");
            }
            else if (sender == TempIncrease)
            {
                MessageBox.Show("Temperature Increased");
            }
            else if (sender == TempDecrease)
            {
                MessageBox.Show("Temperature Decreased");
            }
            else if (sender == EngineerPanel)
            {
                EngineerPanel ePan = new EngineerPanel();
                ePan.Show();
            }
            else if (sender == TestPanel)
            {
                TestPanel tPan = new TestPanel();
                tPan.Show();
            }
        }
    }
}
