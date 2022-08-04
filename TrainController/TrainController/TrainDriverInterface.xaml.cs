using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ControlPanel : Window
    {
        // Train controller objects/values:
        public List<Controller> mTrainSetList = new List<Controller>();
        public Controller mSelectedTrain;

        // Dispatch timer period (while in testing):
        public int T = 1; // 1 ms

        // Index for train IDs:
        public int mIncreaseID = 0;

        public bool mActualClose = false;

        public ControlPanel()
        {
            InitializeComponent();

            // Disable all buttons on main window until a HW_SW window option is selected:
            ManualMode.IsEnabled = false;
            AutoMode.IsEnabled = false;
            ServiceBrake.IsEnabled = false;
            EmergencyBrake.IsEnabled = false;
            SetSpeedBox.IsEnabled = false;
            SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0x30, 0, 0, 0));
            EngineerPanel.IsEnabled = false;
            TestPanel.IsEnabled = true;
            TempIncrease.IsEnabled = false;
            TempDecrease.IsEnabled = false;
            Announcements.IsEnabled = false;
            LeftDoors.IsEnabled = false;
            RightDoors.IsEnabled = false;
            InteriorLights.IsEnabled = false;
            ExteriorLights.IsEnabled = false;

            InitTimer();
        }

        public void UpdateValues(int cmdAuthority,int curAuthority,double cmdVelocity,double curVelocity,string beacon,bool trainUnderground,bool trainLeftDoors,bool trainRightDoors,bool trainEmergencyBrakes,int i)
        {
            // Update power:
            if (!mTrainSetList[i].mControlType)
            {
                mTrainSetList[i].CalculatePowerSW();
            }
            else
            {
                mTrainSetList[i].CalculatePowerHW();
            }

            // Update emergency brakes:
            if (trainEmergencyBrakes)
            {
                mTrainSetList[i].setEmergencyBrake();
            }

            // Update commanded authority (only at instantiation of train):
            mTrainSetList[i].setCmdAuthority(cmdAuthority);

            // Update current authority:
            mTrainSetList[i].setCurAuthority(curAuthority);

            // Update commanded speed (speed limit):
            mTrainSetList[i].setCmdSpeed(cmdVelocity);

            // Update current speed from train model:
            mTrainSetList[i].setCurSpeed(curVelocity);

            // Update beacon: 
            mTrainSetList[i].setBeacon(beacon);

            // Update doors/lights based on underground/station status:
            if (mTrainSetList[i].mAutoMode)
            {
                if (mTrainSetList[i].mInteriorLightsStatus != trainUnderground) mTrainSetList[i].setInteriorLights();
                if (mTrainSetList[i].mExteriorLightsStatus != trainUnderground) mTrainSetList[i].setExteriorLights();
                if (mTrainSetList[i].mLeftDoorsStatus != trainLeftDoors) mTrainSetList[i].setLeftDoors();
                if (mTrainSetList[i].mRightDoorsStatus != trainRightDoors) mTrainSetList[i].setRightDoors();
            }
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
                mSelectedTrain.mSetSpeed = mSelectedTrain.mCmdSpeed;
                SetSpeedBox.Text = convertToImperial(mSelectedTrain.mSetSpeed).ToString("F2");

                mSelectedTrain.setAutoMode();
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

                mSelectedTrain.setManualMode();
            }

            else if (sender == EmergencyBrake)
            {
                if (!mSelectedTrain.mEmergencyBrakeStatus)
                {
                    EmergencyBrake.Content = "Emergency Brake\n         (ON)";
                    EmergencyBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));

                    mSelectedTrain.setEmergencyBrake();
                }
            }

            else if (sender == ServiceBrake)
            {
                mSelectedTrain.setServiceBrake();

                if (mSelectedTrain.mServiceBrakeStatus)
                {
                    ServiceBrake.Content = "Service Brake\n      (ON)";
                    ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
                }
                else
                {
                    ServiceBrake.Content = "Service Brake\n      (OFF)";
                    ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5A5A"));
                }
            }

            else if (sender == LeftDoors)
            {
                mSelectedTrain.setLeftDoors();

                if (mSelectedTrain.mLeftDoorsStatus)
                {
                    LeftDoors.Content = "Doors - Left\n   (OPEN)";
                }
                else
                {
                    LeftDoors.Content = "Doors - Left\n  (CLOSED)";
                }
            }

            else if (sender == RightDoors)
            {
                mSelectedTrain.setRightDoors();

                if (mSelectedTrain.mRightDoorsStatus)
                {
                    RightDoors.Content = "Doors - Right\n    (OPEN)";
                }
                else
                {
                    RightDoors.Content = "Doors - Right\n   (CLOSED)";
                }
            }

            else if (sender == InteriorLights)
            {
                mSelectedTrain.setInteriorLights();

                if (mSelectedTrain.mInteriorLightsStatus)
                {
                    InteriorLights.Content = "Lights - Interior\n        (ON)";
                }
                else
                {
                    InteriorLights.Content = "Lights - Interior\n        (OFF)";
                }
            }

            else if (sender == ExteriorLights)
            {
                mSelectedTrain.setExteriorLights();

                if (mSelectedTrain.mExteriorLightsStatus)
                {
                    ExteriorLights.Content = "Lights - Exterior\n        (ON)";
                }
                else
                {
                    ExteriorLights.Content = "Lights - Exterior\n        (OFF)";
                }
            }

            else if (sender == Announcements)
            {
                mSelectedTrain.setAnnouncements();

                if (mSelectedTrain.mAnnouncementsStatus)
                {
                    Announcements.Content = "Announcements\n        (ON)";
                }
                else
                {
                    Announcements.Content = "Announcements\n        (OFF)";
                }
            }

            else if (sender == TempIncrease)
            {
                mSelectedTrain.tempIncrease();

                Temperature.Text = "Temperature: " + mSelectedTrain.mTemperature.ToString() + "°F";
            }

            else if (sender == TempDecrease)
            {
                mSelectedTrain.tempDecrease();

                Temperature.Text = "Temperature: " + mSelectedTrain.mTemperature.ToString() + "°F";
            }

            else if (sender == EngineerPanel)
            {
                EngineerPanel ePan = new EngineerPanel(this);
                ePan.Owner = this;

                ePan.Kp.Text = mSelectedTrain.mKp.ToString();
                ePan.Ki.Text = mSelectedTrain.mKi.ToString();

                ePan.Show();
            }

            else if (sender == TestPanel)
            {
                TestPanel tPan = new TestPanel(this);
                tPan.Owner = this;

                if (mTrainSetList.Count > 0)
                {
                    if (mSelectedTrain.mLeftDoorsStatus)
                    {
                        tPan.LeftDoors.Content = "OPEN";
                        tPan.LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                    }
                    else
                    {
                        tPan.LeftDoors.Content = "CLOSED";
                        tPan.LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                    }

                    if (mSelectedTrain.mRightDoorsStatus)
                    {
                        tPan.RightDoors.Content = "OPEN";
                        tPan.RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                    }
                    else
                    {
                        tPan.RightDoors.Content = "CLOSED";
                        tPan.RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                    }

                    if (mSelectedTrain.mInteriorLightsStatus)
                    {
                        tPan.InteriorLights.Content = "ON";
                        tPan.InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                    }
                    else
                    {
                        tPan.InteriorLights.Content = "OFF";
                        tPan.InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                    }

                    if (mSelectedTrain.mExteriorLightsStatus)
                    {
                        tPan.ExteriorLights.Content = "ON";
                        tPan.ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                    }
                    else
                    {
                        tPan.ExteriorLights.Content = "OFF";
                        tPan.ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                    }

                    if (mSelectedTrain.mAnnouncementsStatus)
                    {
                        tPan.Announcements.Content = "ON";
                        tPan.Announcements.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                    }
                    else
                    {
                        tPan.Announcements.Content = "OFF";
                        tPan.Announcements.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                    }

                    tPan.Temperature.Text = mSelectedTrain.mTemperature.ToString();

                    tPan.SetKp.Text = mSelectedTrain.mKp.ToString();
                    tPan.SetKi.Text = mSelectedTrain.mKi.ToString();

                    /*tPan.CurSpeed.Text = mSelectedTrain.mCurSpeed.ToString();
                    tPan.CmdSpeed.Text = mSelectedTrain.mCmdSpeed.ToString();
                    tPan.SetSpeed.Text = mSelectedTrain.mSetSpeed.ToString();*/

                    //tPan.CurSpeed.Text = convertToMetric(mSelectedTrain.mCurSpeed).ToString();
                    //tPan.CmdSpeed.Text = convertToMetric(mSelectedTrain.mCmdSpeed).ToString();
                    //tPan.SetSpeed.Text = convertToMetric(mSelectedTrain.mSetSpeed).ToString();

                    if (mSelectedTrain.mAutoMode)
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

                    if (mSelectedTrain.mEmergencyBrakeStatus)
                    {
                        tPan.EmergencyBrake.Content = "ON";
                        tPan.EmergencyBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                    }
                    else
                    {
                        tPan.EmergencyBrake.Content = "OFF";
                        tPan.EmergencyBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                    }

                    if (mSelectedTrain.mServiceBrakeStatus)
                    {
                        tPan.ServiceBrake.Content = "ON";
                        tPan.ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                    }
                    else
                    {
                        tPan.ServiceBrake.Content = "OFF";
                        tPan.ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                    }

                    tPan.CmdAuthority.Text = mSelectedTrain.mCmdAuthority.ToString();
                    tPan.CurAuthority.Text = mSelectedTrain.mCurAuthority.ToString();

                    tPan.CurBeacon.Text = mSelectedTrain.mBeacon;
                }

                tPan.Show();
            }

            else if (sender == CloseButton)
            {
                mActualClose = true;
                this.Close();
            }
        }

        public void KeyDownButton(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == SetSpeedBox)
                {
                    mSelectedTrain.setSetSpeed(double.Parse(SetSpeedBox.Text));
                    SetSpeedBox.Text = convertToImperial(mSelectedTrain.mSetSpeed).ToString("F2");
                }
            }
        }

        public void InitTimer()
        {
            DispatcherTimer emergencyBrakeStatus = new DispatcherTimer();

            emergencyBrakeStatus.Tick += new EventHandler(checkUpdatedValues);

            emergencyBrakeStatus.Interval = new TimeSpan(0, 0, 0, 0, T);
            emergencyBrakeStatus.Start();
        }

        public void checkUpdatedValues(object sender, EventArgs e)
        {
            if (mTrainSetList.Count > 0)
            {
                // Update Speed display:
                CmdSpeed.Text = "Cmd Speed:\n" + convertToImperial(mSelectedTrain.mCmdSpeed).ToString("F2") + " mph";
                CurSpeed.Text = "Current Speed:\n" + convertToImperial(mSelectedTrain.mCurSpeed).ToString("F2") + " mph";
            
                if (mSelectedTrain.mAutoMode)
                {
                    SetSpeedBox.Text = convertToImperial(mSelectedTrain.mSetSpeed).ToString("F2");
                }            

                // Update Power display:
                CurPower.Text = "Power: " + (mSelectedTrain.mCurPower / 1000).ToString("F4") + " kW";

                // Update Temperature value:
                Temperature.Text = "Temperature: " + mSelectedTrain.mTemperature.ToString() + "°F";

                // Update Commanded and Current Authority values:
                CmdAuthority.Text = "Commanded Authority:\n" + mSelectedTrain.mCmdAuthority + " blocks";
                CurAuthority.Text = "Current Authority:\n" + mSelectedTrain.mCurAuthority + " blocks";

                // Update Service brake/Emergency brake:
                if (mSelectedTrain.mEmergencyBrakeStatus)
                {
                    EmergencyBrake.Content = "Emergency Brake\n         (ON)";
                    EmergencyBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
                }
                else
                {
                    EmergencyBrake.Content = "Emergency Brake\n         (OFF)";
                    EmergencyBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5A5A"));
                }

                // Update Service brake/Emergency brake:
                if (mSelectedTrain.mServiceBrakeStatus)
                {
                    ServiceBrake.Content = "Service Brake\n      (ON)";
                    ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
                }
                else
                {
                    ServiceBrake.Content = "Service Brake\n      (OFF)";
                    ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5A5A"));
                }

                // Update Left Doors status:
                if (mSelectedTrain.mLeftDoorsStatus)
                {
                    LeftDoors.Content = "Doors - Left\n   (OPEN)";
                }
                else
                {
                    LeftDoors.Content = "Doors - Left\n  (CLOSED)";
                }

                // Update Right Doors status:
                if (mSelectedTrain.mRightDoorsStatus)
                {
                    RightDoors.Content = "Doors - Right\n    (OPEN)";
                }
                else
                {
                    RightDoors.Content = "Doors - Right\n   (CLOSED)";
                }

                // Update Interior Lights status:
                if (mSelectedTrain.mInteriorLightsStatus)
                {
                    InteriorLights.Content = "Lights - Interior\n        (ON)";
                }
                else
                {
                    InteriorLights.Content = "Lights - Interior\n        (OFF)";
                }

                // Update Exterior Lights status:
                if (mSelectedTrain.mExteriorLightsStatus)
                {
                    ExteriorLights.Content = "Lights - Exterior\n        (ON)";
                }
                else
                {
                    ExteriorLights.Content = "Lights - Exterior\n        (OFF)";
                }

                // Update Announcements status:
                if (mSelectedTrain.mAnnouncementsStatus)
                {
                    Announcements.Content = "Announcements\n        (ON)";
                }
                else
                {
                    Announcements.Content = "Announcements\n        (OFF)";
                }

                // Update Beacon:
                Beacon.Text = "Nearest Beacon:\n" + mSelectedTrain.mBeacon;

                // Update Line:
                if (!mSelectedTrain.mLine)
                {
                    Line.Content = "Line: Red"; 
                }
                else
                {
                    Line.Content = "Line: Green";
                }

                // Update HW/SW indicator box:
                if (!mSelectedTrain.mControlType && mSelectedTrain.mSetControlType)
                {
                    SelectType.Text = "Software Controller";
                    SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0xDF, 0x20));
                }
                else if (mSelectedTrain.mControlType && mSelectedTrain.mSetControlType)
                {
                    SelectType.Text = "Hardware Controller";
                    SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0x5F, 0xA0));
                }
                else
                {
                    SelectType.Text = "Select Controller Type";
                    SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDA, 0xDA, 0xDA));
                }

                // Enable/Disable controls if selected train is in auto mode or manual mode:
                AutoMode.IsEnabled = !mSelectedTrain.mAutoMode;
                ManualMode.IsEnabled = mSelectedTrain.mAutoMode;
                SetSpeedBox.IsEnabled = !mSelectedTrain.mAutoMode;

                if (mSelectedTrain.mAutoMode) SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0x30, 0, 0, 0));
                else SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xDF, 0x20));

                SetSpeed.IsEnabled = !mSelectedTrain.mAutoMode;
                TempIncrease.IsEnabled = !mSelectedTrain.mAutoMode;
                TempDecrease.IsEnabled = !mSelectedTrain.mAutoMode;
                Announcements.IsEnabled = !mSelectedTrain.mAutoMode;
                LeftDoors.IsEnabled = !mSelectedTrain.mAutoMode;
                RightDoors.IsEnabled = !mSelectedTrain.mAutoMode;
                InteriorLights.IsEnabled = !mSelectedTrain.mAutoMode;
                ExteriorLights.IsEnabled = !mSelectedTrain.mAutoMode;
            }
        }

        public void SelectTrain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mTrainSetList.Count > 0)
            {
                mSelectedTrain = mTrainSetList[mControllerList.SelectedIndex];
            }
        }

        public void addController(bool line)
        {
            mSelectedTrain = new Controller(line);
            mTrainSetList.Add(mSelectedTrain);
            mControllerList.Items.Insert((mTrainSetList.Count - 1), "Train " + mIncreaseID);
            mControllerList.SelectedIndex = (mTrainSetList.Count - 1);

            mIncreaseID++;

            HW_SW selectType = new HW_SW(this);
            selectType.Topmost = true;
            selectType.Show();
            selectType.Activate();
        }

        public void removeController(int index)
        {
            if (mTrainSetList.Count > 0)
            {
                mTrainSetList.RemoveAt(index);
                mControllerList.SelectedIndex = index - 1;
                mControllerList.Items.RemoveAt(index);
            }
        }

        private void TrainControllerActive(object sender, EventArgs e)
        {
            Application.Current.MainWindow = this;
        }

        // Minimize when X is pressed.
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!mActualClose)
            {
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
            }
        }

        private double convertToImperial(double metricSpeed)
        {
            return metricSpeed * 2.236936;
        }

        private double convertToMetric(double imperialSpeed)
        {
            return imperialSpeed / 2.236936;
        }
    }
}
