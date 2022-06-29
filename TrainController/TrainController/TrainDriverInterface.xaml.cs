using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
        public bool mAnnouncementsStatus = false;
        public bool mEmergencyBrakeStatus = false;
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

            InitTimer();

            HW_SW selectType = new HW_SW();
            selectType.Topmost = true;
            selectType.Show();
            selectType.Activate();

            // Setup serial port information: 
            pi.PortName = "COM3";
            pi.BaudRate = 115200;
            pi.Parity = Parity.None;
            pi.DataBits = 8;
            pi.StopBits = StopBits.One;
            pi.Handshake = Handshake.None;

            pi.WriteTimeout = 500;

            pi.Open();
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
                mSetSpeed = mCmdSpeed;
                SetSpeedBox.Text = mSetSpeed.ToString();

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
                    if (mEmergencyBrakeStatus == false)
                    {
                        mEmergencyBrakeStatus = true;
                    }
                }
                else
                {
                    if(mEmergencyBrakeStatus == false)
                    {
                        mEmergencyBrakeStatus = true;
                        pi.WriteLine("E_Brakes from windows");
                        string brakeStatus = pi.ReadLine();
                        MessageBox.Show(brakeStatus);
                    }   
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
                if (!mAnnouncementsStatus)
                {
                    mAnnouncementsStatus = true;
                    Announcements.Content = "Announcements\n        (ON)";
                }
                else
                {
                    mAnnouncementsStatus = false;
                    Announcements.Content = "Announcements\n        (OFF)";
                }
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
                    if (int.Parse(SetSpeedBox.Text) > mCmdSpeed)
                    {
                        SetSpeedBox.Text = mSetSpeed.ToString();
                        MessageBox.Show("Set Speed Shall Not Exceed Commanded Speed");
                    }
                    else
                    {
                        mSetSpeed = int.Parse(SetSpeedBox.Text);
                    }
                }
            }
        }

        private void InitTimer()
        {
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(SpeedUpdate);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,500);
            dispatcherTimer.Start();
        }

        private void SpeedUpdate(object sender, EventArgs e)
        {
            if (mAutoMode)
            {
                if (mCurSpeed < mCmdSpeed)
                {
                    mCurSpeed++;
                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
                else if (mCurSpeed > mCmdSpeed)
                {
                    mCurSpeed--;
                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
            }
            else
            {
                if (mCurSpeed < mSetSpeed)
                {
                    mCurSpeed++;
                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
                else if (mCurSpeed > mSetSpeed)
                {
                    mCurSpeed--;
                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
            }
        }
    }
}
