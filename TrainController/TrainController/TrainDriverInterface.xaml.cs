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
        public bool mServiceBrakeStatus = false;
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
        public float mCurPower = 0;

        public const float Pmax = 120000; // 120 kW
        public float Uk = 0;
        public int T = 250; // 250 ms

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

            InitTimer();

            HW_SW selectType = new HW_SW();
            selectType.Topmost = true;
            selectType.Show();
            selectType.Activate();
        }

        public void setupHardware()
        {
            // Setup serial port information: 
            pi.PortName = "COM4";
            pi.BaudRate = 115200;
            pi.Parity = Parity.None;
            pi.DataBits = 8;
            pi.StopBits = StopBits.One;
            pi.Handshake = Handshake.None;
            pi.WriteTimeout = 500;

            // Open serial port and reset all stored data:
            pi.Open();
            //pi.WriteLine("?");
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == AutoMode)
            {
                //UI Element Controls:
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
                SetSpeedBox.Text = mSetSpeed.ToString();

                mSetSpeed = mCmdSpeed;
                mAutoMode = true;

                // Hardware Controls:
                if (mControlType)
                {
                    pi.WriteLine("m");
                    string output = pi.ReadLine();
                }
            }
            else if (sender == ManualMode)
            {
                // UI Element Controls:
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

                // Hardware Controls:
                if (mControlType)
                {
                    pi.WriteLine("m");
                    string output = pi.ReadLine();
                }
            }

            else if (sender == EmergencyBrake)
            {
                if (!mEmergencyBrakeStatus)
                {
                    mEmergencyBrakeStatus = true;
                    EmergencyBrake.Content = "Emergency Brake\n         (ON)";
                    EmergencyBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
                    MessageBox.Show("Emergency Brake Activated");
                }

                // Hardware Controls:
                if (mControlType)
                {
                    pi.WriteLine("e");
                    string output = pi.ReadLine();
                }
            }

            else if (sender == ServiceBrake)
            {
                if (!mServiceBrakeStatus)
                {
                    mServiceBrakeStatus = true;
                    ServiceBrake.Content = "Service Brake\n      (ON)";
                    ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
                }
                else
                {
                    mServiceBrakeStatus = false;
                    ServiceBrake.Content = "Service Brake\n      (OFF)";
                    ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5A5A"));
                }

                // Hardware  Controls:
                if (mControlType)
                {
                    // Thomas stuff
                }
            }

            else if (sender == LeftDoors)
            {
                if (!mControlType)
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
                else
                {
                    pi.WriteLine("l");
                    string output = pi.ReadLine();

                    if (output == "Open")
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
            }
            else if (sender == RightDoors)
            {
                if (!mControlType)
                {
                    if (!mRightDoorsStatus)
                    {
                        mRightDoorsStatus = true;
                        RightDoors.Content = "Doors - Right\n    (OPEN)";
                    }
                    else
                    {
                        mRightDoorsStatus = false;
                        RightDoors.Content = "Doors - Right\n   (CLOSED)";
                    }
                }
                else
                {
                    pi.WriteLine("r");
                    string output = pi.ReadLine();

                    if (output == "Open")
                    {
                        mRightDoorsStatus = true;
                        RightDoors.Content = "Doors - Right\n    (OPEN)";
                    }
                    else
                    {
                        mRightDoorsStatus = false;
                        RightDoors.Content = "Doors - Right\n   (CLOSED)";
                    }
                }
            }

            else if (sender == InteriorLights)
            {
                if (!mControlType)
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
                else
                {
                    pi.WriteLine("i");
                    string output = pi.ReadLine();

                    if (output == "On")
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
            }
            else if (sender == ExteriorLights)
            {
                if (!mControlType)
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
                else
                {
                    pi.WriteLine("x");
                    string output = pi.ReadLine();

                    if (output == "On")
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
            }

            else if (sender == Announcements)
            {
                if (!mControlType)
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
                else
                {
                    pi.WriteLine("a");
                    string output = pi.ReadLine();

                    if (output == "On")
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
            }

            else if (sender == TempIncrease)
            {
                if (!mControlType)
                {
                    mTemperature++;
                }
                else
                {
                    pi.WriteLine("h");
                    string output = pi.ReadLine();

                    if(output == "tempIncreased")
                    {
                        mTemperature++;
                    }
                }

                Temperature.Text = "Temperature: " + mTemperature.ToString() + "°F";
            }
            else if (sender == TempDecrease)
            {
                if (!mControlType)
                {
                    mTemperature--;
                }
                else
                {
                    pi.WriteLine("c");
                    string output = pi.ReadLine();

                    if (output == "tempDecreased")
                    {
                        mTemperature--;
                    }
                }

                Temperature.Text = "Temperature: " + mTemperature.ToString() + "°F";
            }

            else if (sender == EngineerPanel)
            {
                EngineerPanel ePan = new EngineerPanel();
                ePan.Owner = this;

                ePan.Kp.Text = mKp.ToString();
                ePan.Ki.Text = mKi.ToString();

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

                if (mAnnouncementsStatus)
                {
                    tPan.Announcements.Content = "ON";
                    tPan.Announcements.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    tPan.Announcements.Content = "OFF";
                    tPan.Announcements.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
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

        public void setKp(int value)
        {
            mKp = value;

            // Hardware Controls:
            if(mControlType)
            {
                pi.WriteLine("j");
                pi.WriteLine(mKp.ToString()+"\n");
            }
        }

        public void setKi(int value)
        {
            mKi = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("k");
                pi.WriteLine(mKi.ToString()+"\n");
            }
        }

        public void setCmdSpeed(int value)
        {
            mCmdSpeed = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("v");
                pi.WriteLine(mCmdSpeed.ToString()+"\n");
            }
        }

        public void setSetSpeed(int value)
        {
            if (!mControlType)
            {
                if (value > mCmdSpeed)
                {
                    MessageBox.Show("Set Speed Shall Not Exceed Commanded Speed");
                }
                else
                {
                    mSetSpeed = value;
                }
            }
            else
            {
                pi.WriteLine("b");
                pi.WriteLine(value.ToString()+"\n");
                string output = pi.ReadLine();

                if (output == "tooHigh")
                {
                    MessageBox.Show("Set Speed Shall Not Exceed Commanded Speed");
                }
                else
                {
                    mSetSpeed = value;
                }
            }

            SetSpeedBox.Text = mSetSpeed.ToString();
        }

        public void setCurSpeed(int value)
        {
            mCurSpeed = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("n");
                pi.WriteLine(mCurSpeed.ToString()+"\n");
            }
        }

        public void setCmdAuthority(int value)
        {
            mCmdAuthority = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("s");
                pi.WriteLine(mCmdAuthority.ToString()+"\n");
            }
        }

        public void setCurAuthority(int value)
        {
            mCurAuthority = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("d");
                pi.WriteLine(mCurAuthority.ToString()+"\n");
            }
        }

        public void setBeacon(string value)
        {
            // Software Controls:
            if (!mControlType)
            {
                mBeacon = value;
                Beacon.Text = "Nearest Beacon:\n" + value;
            }

            // Hard Controls:
            else
            {
                pi.WriteLine("f");
                pi.WriteLine(value+"\n");
                string output = pi.ReadLine();

                mBeacon = output;
                Beacon.Text = "Nearest Beacon:\n" + output;
            }
        }

        public void setPower(int value)
        {
            mCurPower = value;

            if (mControlType)
            {
                pi.WriteLine("p");
                pi.WriteLine(value + "\n");
                string output = pi.ReadLine();
            }
        }

        public void KeyDownButton(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == SetSpeedBox)
                {
                    setSetSpeed(int.Parse(SetSpeedBox.Text));
                }
            }
        }

        public void InitTimer()
        {
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(SpeedUpdate);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,T);
            dispatcherTimer.Start();
        }

        public void SpeedUpdate(object sender, EventArgs e)
        {
            if (mEmergencyBrakeStatus)
            {
                if (mCurSpeed == 0)
                {
                    mEmergencyBrakeStatus = false;
                    EmergencyBrake.Content = "Emergency Brake\n         (OFF)";
                    EmergencyBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5A5A"));
                }
                else
                {
                    mCmdSpeed = 0;
                    CmdSpeed.Text = "Cmd Speed:\n" + mCmdSpeed + " mph";
                    mSetSpeed = 0;
                    SetSpeedBox.Text = mSetSpeed.ToString();

                    mCurSpeed -= 1; // TODO: Replace with emergency brake deceleration!
                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
            }
            else if (mServiceBrakeStatus)
            {
                if (mCurSpeed > 0)
                {
                    mCurSpeed--;  // TODO: Replace with service brake deceleration!
                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
            }
            else if (mAutoMode)
            {
                if (mCurSpeed < mCmdSpeed)
                {
                    mCurSpeed++;    // TODO: Replace with acceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (mCmdSpeed + mCurSpeed);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = mKp * mCmdSpeed + mKi * Uk;
                    CurPower.Text = "Power: " + mCurPower/1000 + " kW";

                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
                else if (mCurSpeed > mCmdSpeed)
                {
                    mCurSpeed--;    // TODO: Replace with deceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (mCmdSpeed + mCurSpeed);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = -1 * (mKp * mCmdSpeed + mKi * Uk);
                    CurPower.Text = "Power: " + mCurPower / 1000 + " kW";

                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
                else
                {
                    mCurPower = 0;
                    CurPower.Text = "Power: " + mCurPower / 1000 + " kW";
                }
            }
            else
            {
                if (mCurSpeed < mSetSpeed)
                {
                    mCurSpeed++;    // TODO: Replace with acceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (mSetSpeed + mCurSpeed);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = mKp * mSetSpeed + mKi * Uk;
                    CurPower.Text = "Power: " + mCurPower / 1000 + " kW";

                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
                else if (mCurSpeed > mSetSpeed)
                {
                    mCurSpeed--;    // TODO: Replace with deceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (mSetSpeed + mCurSpeed);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = -1 * (mKp * mSetSpeed + mKi * Uk);
                    CurPower.Text = "Power: " + mCurPower / 1000 + " kW";

                    CurSpeed.Text = "Current Speed:\n" + mCurSpeed + " mph";
                }
                else
                {
                    mCurPower = 0;
                    CurPower.Text = "Power: " + mCurPower / 1000 + " kW";
                }
            }
        }
    }
}
