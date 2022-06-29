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
        public bool mControlType;
        private bool mAutoMode = false;
        public bool mLeftDoorsStatus = false;
        public bool mRightDoorsStatus = false;
        private bool mInteriorLightsStatus = false;
        private bool mExteriorLightsStatus = false;

        public MainWindow()
        {
            InitializeComponent();

            // Disable all buttons on main window until a HW_SW window option is selected:
            ManualMode.IsEnabled = false;
            AutoMode.IsEnabled = false;
            ServiceBrake.IsEnabled = false;
            EmergencyBrake.IsEnabled = false;
            SetSpeedBox.IsEnabled = false;
            EngineerPanel.IsEnabled = false;
            TestPanel.IsEnabled = false;
            TempIncrease.IsEnabled = false;
            TempDecrease.IsEnabled = false;
            Announcements.IsEnabled = false;
            LeftDoors.IsEnabled = false;
            RightDoors.IsEnabled = false;
            LightsInterior.IsEnabled = false;
            LightsExterior.IsEnabled = false;

            HW_SW selectType = new HW_SW();
            selectType.Show();
            selectType.Activate();
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
                if (!mLeftDoorsStatus)
                {
                    mLeftDoorsStatus = true;
                    LeftDoors.Content = "Doors - Left\n    (OPEN)";
                }
                else
                {
                    mLeftDoorsStatus = false;
                    LeftDoors.Content = "Doors - Left\n  (CLOSED)";
                }
            }
            else if (sender == RightDoors)
            {
                if (!mRightDoorsStatus)
                {
                    mRightDoorsStatus = true;
                    RightDoors.Content = "Doors - Right\n     (OPEN)";
                }
                else
                {
                    mRightDoorsStatus = false;
                    RightDoors.Content = "Doors - Right\n   (CLOSED)";
                }
            }
            else if (sender == LightsInterior)
            {
                if (!mInteriorLightsStatus)
                {
                    mInteriorLightsStatus = true;
                    LightsInterior.Content = "Lights - Interior\n        (ON)";
                }
                else
                {
                    mInteriorLightsStatus = false;
                    LightsInterior.Content = "Lights - Interior\n        (OFF)";
                }
            }
            else if (sender == LightsExterior)
            {
                if (!mExteriorLightsStatus)
                {
                    mExteriorLightsStatus = true;
                    LightsExterior.Content = "Lights - Exterior\n        (ON)";
                }
                else
                {
                    mExteriorLightsStatus = false;
                    LightsExterior.Content = "Lights - Exterior\n        (OFF)";
                }
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
                ePan.Owner = this;
                ePan.Show();
            }
            else if (sender == TestPanel)
            {
                TestPanel tPan = new TestPanel();
                tPan.Owner = this;

                if (mLeftDoorsStatus)
                {
                    tPan.LeftDoors.Content = "OPEN";
                    tPan.LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    tPan.LeftDoors.Content = "CLOSED";
                    tPan.LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }

                tPan.Show();
            }
        }
    }
}
