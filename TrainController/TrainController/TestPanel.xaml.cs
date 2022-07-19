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
using System.Windows.Shapes;

namespace TrainController
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TestPanel : Window
    {
        public TestPanel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == AutoMode)
            {
                AutoMode.Content = "ON";
                AutoMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                ManualMode.Content = "OFF";
                ManualMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));

                ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).AutoMode, e);

                ((ControlPanel)Application.Current.MainWindow).AutoMode.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).ManualMode.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).LeftDoors.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).RightDoors.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).InteriorLights.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).ExteriorLights.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).Announcements.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).TempIncrease.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).TempDecrease.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).SetSpeedBox.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0x30, 0, 0, 0));
                ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setAutoMode();
            }

            else if (sender == ManualMode)
            {
                ManualMode.Content = "ON";
                ManualMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                AutoMode.Content = "OFF";
                AutoMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));

                ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).ManualMode, e);

                ((ControlPanel)Application.Current.MainWindow).ManualMode.IsEnabled = false;
                ((ControlPanel)Application.Current.MainWindow).AutoMode.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).LeftDoors.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).RightDoors.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).InteriorLights.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).ExteriorLights.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).Announcements.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).TempIncrease.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).TempDecrease.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).SetSpeedBox.IsEnabled = true;
                ((ControlPanel)Application.Current.MainWindow).SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xDF, 0x20));
                ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setManualMode();
            }

            else if (sender == EmergencyBrake)
            {
                ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).EmergencyBrake, e);
                EmergencyBrake.Content = "ON";
                EmergencyBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
            }

            else if (sender == LeftDoors)
            {
                if (!((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mLeftDoorsStatus)
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).LeftDoors, e);
                    LeftDoors.Content = "OPEN";
                    LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).LeftDoors, e);
                    LeftDoors.Content = "CLOSED";
                    LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }

            else if (sender == RightDoors)
            {
                if (!((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mRightDoorsStatus)
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).RightDoors, e);
                    RightDoors.Content = "OPEN";
                    RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).RightDoors, e);
                    RightDoors.Content = "CLOSED";
                    RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }

            else if (sender == InteriorLights)
            {
                if (!((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mInteriorLightsStatus)
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).InteriorLights, e);
                    InteriorLights.Content = "ON";
                    InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).InteriorLights, e);
                    InteriorLights.Content = "OFF";
                    InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }

            else if (sender == ExteriorLights)
            {
                if (!((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mExteriorLightsStatus)
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).ExteriorLights, e);
                    ExteriorLights.Content = "ON";
                    ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).ExteriorLights, e);
                    ExteriorLights.Content = "OFF";
                    ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }

            else if (sender == TempIncrease)
            {
                ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).TempIncrease, e);
                Temperature.Text = ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mTemperature.ToString();
            }

            else if (sender == TempDecrease)
            {
                ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).TempDecrease, e);
                Temperature.Text = ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mTemperature.ToString();
            }

            else if (sender == Announcements)
            {
                if (!((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mAnnouncementsStatus)
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).Announcements, e);
                    Announcements.Content = "ON";
                    Announcements.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).Announcements, e);
                    Announcements.Content = "OFF";
                    Announcements.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }

            else if (sender == ServiceBrake)
            {
                if (!((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mServiceBrakeStatus)
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).ServiceBrake, e);
                    ServiceBrake.Content = "ON";
                    ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((ControlPanel)Application.Current.MainWindow).Button_Click(((ControlPanel)Application.Current.MainWindow).ServiceBrake, e);
                    ServiceBrake.Content = "OFF";
                    ServiceBrake.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }

            else if (sender == AddTrain)
            {
                ((ControlPanel)Application.Current.MainWindow).addController();
            }    
        }

        private void KeyDownButton(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == SetKp)
                {
                    ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setKp(int.Parse(SetKp.Text));
                }
                else if (sender == SetKi)
                {
                    ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setKi(int.Parse(SetKi.Text));
                }
                else if (sender == CmdSpeed)
                {
                    ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setCmdSpeed(int.Parse(CmdSpeed.Text));
                    ((ControlPanel)Application.Current.MainWindow).CmdSpeed.Text = "Cmd Speed:\n" + ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mCmdSpeed.ToString() + " mph";
                }
                else if (sender == SetSpeed)
                {
                    ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setSetSpeed(int.Parse(SetSpeed.Text));
                    SetSpeed.Text = ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mSetSpeed.ToString();
                }
                else if (sender == CurSpeed)
                {
                    ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setCurSpeed(int.Parse(CurSpeed.Text));
                    ((ControlPanel)Application.Current.MainWindow).CurSpeed.Text = "Current Speed:\n" + ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mCurSpeed.ToString() + " mph";
                }
                else if (sender == CmdAuthority)
                {
                    ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setCmdAuthority(int.Parse(CmdAuthority.Text));
                    ((ControlPanel)Application.Current.MainWindow).CmdAuthority.Text = "Commanded Authority:\n" + ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mCmdAuthority + " blocks";
                }
                else if (sender == CurAuthority)
                {
                    ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setCurAuthority(int.Parse(CurAuthority.Text));
                    ((ControlPanel)Application.Current.MainWindow).CurAuthority.Text = "Current Authority:\n" + ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mCurAuthority + " blocks";
                }
                else if (sender == CurBeacon)
                {
                    ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setBeacon(CurBeacon.Text);
                }
                else if (sender == CurPower)
                {
                    ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setPower(int.Parse(CurPower.Text));
                    ((ControlPanel)Application.Current.MainWindow).CurPower.Text = "Power: " + ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mCurPower + " kW";
                }
            }
        }
    }
}
