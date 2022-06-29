using System;
using System.Collections.Generic;
using System.IO.Ports;
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
        // Serial port for connecting to Raspberry Pi:
        SerialPort pi = new SerialPort();

        // Boolean for switching between auto and manual driving modes:
        // 'false' is software controller, 'true' is hardware controller:
        public bool mControlType;

        public bool mAutoMode = false;
        public bool mLeftDoorsStatus = false;
        public bool mRightDoorsStatus = false;
        public bool mInteriorLightsStatus = false;
        public bool mExteriorLightsStatus = false;
        public int mTemperature = 72;
        public int mKp = 0;
        public int mKi = 0;
        public int mCurSpeed = 0;
        public int mCmdSpeed = 0;
        public int mSetSpeed = 0;
        public int mCmdAuthority = 0;
        public int mCurAuthority = 0;
        public string mBeacon = "-";

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
            InteriorLights.IsEnabled = false;
            ExteriorLights.IsEnabled = false;

            mAutoMode = false;

            HW_SW selectType = new HW_SW();
            selectType.Topmost = true;
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
                InteriorLights.IsEnabled = false;
                ExteriorLights.IsEnabled = false;
                Announcements.IsEnabled = false;
                TempIncrease.IsEnabled = false;
                TempDecrease.IsEnabled = false;
                SetSpeedBox.IsEnabled = false;

                SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0x30, 0, 0, 0));

                mAutoMode = true;
            }
            else if (sender == ManualMode)
            {
                ManualMode.IsEnabled = false;
                AutoMode.IsEnabled = true;

                LeftDoors.IsEnabled = true;
                RightDoors.IsEnabled = true;
                InteriorLights.IsEnabled = true;
                ExteriorLights.IsEnabled = true;
                Announcements.IsEnabled = true;
                TempIncrease.IsEnabled = true;
                TempDecrease.IsEnabled = true;
                SetSpeedBox.IsEnabled = true;

                SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xDF, 0x20));

                mAutoMode = false;
            }
            else if (sender == EmergencyBrake)
            {
                if (mControlType == false)
                {
                    MessageBox.Show("Emergency Brakes engaged!");
                }
                else
                {
                    MessageBox.Show(SerialPort.GetPortNames()[1]);
                }
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
                    LeftDoors.Content = "Doors - Left\n   (CLOSED)";
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
            else if (sender == InteriorLights)
            {
                if (!mInteriorLightsStatus)
                {
                    mInteriorLightsStatus = true;
                    InteriorLights.Content = "Lights - Interior\n        (ON)";
                }
                else
                {
                    mInteriorLightsStatus = false;
                    InteriorLights.Content = "Lights - Interior\n        (OFF)";
                }
            }
            else if (sender == ExteriorLights)
            {
                if (!mExteriorLightsStatus)
                {
                    mExteriorLightsStatus = true;
                    ExteriorLights.Content = "Lights - Exterior\n        (ON)";
                }
                else
                {
                    mExteriorLightsStatus = false;
                    ExteriorLights.Content = "Lights - Exterior\n        (OFF)";
                }
            }
            else if (sender == Announcements)
            {
                MessageBox.Show("Making an Announcement");
            }
            else if (sender == TempIncrease)
            {
                mTemperature++;
                Temperature.Text = "Temperature: " + mTemperature.ToString() + "°F";
            }
            else if (sender == TempDecrease)
            {
                mTemperature--;
                Temperature.Text = "Temperature: " + mTemperature.ToString() + "°F";
            }
            else if (sender == EngineerPanel)
            {
                EngineerPanel ePan = new EngineerPanel();
                ePan.Owner = this;

                ePan.DisplayKp.Text = mKp.ToString();
                ePan.DisplayKi.Text = mKi.ToString();

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

                if (mRightDoorsStatus)
                {
                    tPan.RightDoors.Content = "OPEN";
                    tPan.RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    tPan.RightDoors.Content = "CLOSED";
                    tPan.RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }

                if (mInteriorLightsStatus)
                {
                    tPan.InteriorLights.Content = "ON";
                    tPan.InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    tPan.InteriorLights.Content = "OFF";
                    tPan.InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }

                if (mExteriorLightsStatus)
                {
                    tPan.ExteriorLights.Content = "ON";
                    tPan.ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    tPan.ExteriorLights.Content = "OFF";
                    tPan.ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }

                tPan.Temperature.Text = mTemperature.ToString();

                tPan.SetKp.Text = mKp.ToString();
                tPan.SetKi.Text = mKi.ToString();

                tPan.CurSpeed.Text = mCurSpeed.ToString();
                tPan.CmdSpeed.Text = mCmdSpeed.ToString();
                tPan.SetSpeed.Text = mSetSpeed.ToString();

                if (mAutoMode)
                {
                    tPan.AutoMode.Content = "ON";
                    tPan.AutoMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                    tPan.ManualMode.Content = "OFF";
                    tPan.ManualMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
                else
                {
                    tPan.ManualMode.Content = "ON";
                    tPan.ManualMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                    tPan.AutoMode.Content = "OFF";
                    tPan.AutoMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }

                tPan.CmdAuthority.Text = mCmdAuthority.ToString();
                tPan.CurAuthority.Text = mCurAuthority.ToString();

                tPan.CurBeacon.Text = mBeacon;

                tPan.Show();
            }
        }

        private void KeyDownButton(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == SetSpeedBox)
                {
                    mSetSpeed = int.Parse(SetSpeedBox.Text);
                }
            }
        }
    }
}
