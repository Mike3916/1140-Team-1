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

                ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).AutoMode, e);

                ((MainWindow)Application.Current.MainWindow).AutoMode.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).ManualMode.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).LeftDoors.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).RightDoors.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).InteriorLights.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).ExteriorLights.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).Announcements.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).TempIncrease.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).TempDecrease.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).SetSpeedBox.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0x30, 0, 0, 0));
                ((MainWindow)Application.Current.MainWindow).mAutoMode = true;
            }
            else if (sender == ManualMode)
            {
                ManualMode.Content = "ON";
                ManualMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                AutoMode.Content = "OFF";
                AutoMode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));

                ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).ManualMode, e);

                ((MainWindow)Application.Current.MainWindow).ManualMode.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).AutoMode.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).LeftDoors.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).RightDoors.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).InteriorLights.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).ExteriorLights.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).Announcements.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TempIncrease.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TempDecrease.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).SetSpeedBox.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xDF, 0x20));
                ((MainWindow)Application.Current.MainWindow).mAutoMode = false;
            }

            else if (sender == EmergencyBrake)
            {
                ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).EmergencyBrake, e);
                EmergencyBrake.Content = "ON";
            }

            else if (sender == LeftDoors)
            {
                if (!((MainWindow)Application.Current.MainWindow).mLeftDoorsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).LeftDoors, e);
                    LeftDoors.Content = "OPEN";
                    LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).LeftDoors, e);
                    LeftDoors.Content = "CLOSED";
                    LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }
            else if (sender == RightDoors)
            {
                if (!((MainWindow)Application.Current.MainWindow).mRightDoorsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).RightDoors, e);
                    RightDoors.Content = "OPEN";
                    RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).RightDoors, e);
                    RightDoors.Content = "CLOSED";
                    RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }

            else if (sender == InteriorLights)
            {
                if (!((MainWindow)Application.Current.MainWindow).mInteriorLightsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).InteriorLights, e);
                    InteriorLights.Content = "ON";
                    InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).InteriorLights, e);
                    InteriorLights.Content = "OFF";
                    InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }
            else if (sender == ExteriorLights)
            {
                if (!((MainWindow)Application.Current.MainWindow).mExteriorLightsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).ExteriorLights, e);
                    ExteriorLights.Content = "ON";
                    ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).ExteriorLights, e);
                    ExteriorLights.Content = "OFF";
                    ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }

            else if (sender == TempIncrease)
            {
                ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).TempIncrease, e);
                Temperature.Text = ((MainWindow)Application.Current.MainWindow).mTemperature.ToString();
            }
            else if (sender == TempDecrease)
            {
                ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).TempDecrease, e);
                Temperature.Text = ((MainWindow)Application.Current.MainWindow).mTemperature.ToString();
            }

            else if (sender == Announcements)
            {
                if (!((MainWindow)Application.Current.MainWindow).mAnnouncementsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).Announcements, e);
                    Announcements.Content = "ON";
                    Announcements.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).Button_Click(((MainWindow)Application.Current.MainWindow).Announcements, e);
                    Announcements.Content = "OFF";
                    Announcements.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }
        }

        private void KeyDownButton(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == SetKp)
                {
                    ((MainWindow)Application.Current.MainWindow).setKp(int.Parse(SetKp.Text));
                }
                else if (sender == SetKi)
                {
                    ((MainWindow)Application.Current.MainWindow).setKi(int.Parse(SetKi.Text));
                }
                else if (sender == CmdSpeed)
                {
                    ((MainWindow)Application.Current.MainWindow).setCmdSpeed(int.Parse(CmdSpeed.Text));
                    ((MainWindow)Application.Current.MainWindow).CmdSpeed.Text = "Cmd Speed:\n" + ((MainWindow)Application.Current.MainWindow).mCmdSpeed.ToString() + " mph";
                }
                else if (sender == SetSpeed)
                {
                    ((MainWindow)Application.Current.MainWindow).setSetSpeed(int.Parse(SetSpeed.Text));
                    SetSpeed.Text = ((MainWindow)Application.Current.MainWindow).mSetSpeed.ToString();
                }
                else if (sender == CurSpeed)
                {
                    ((MainWindow)Application.Current.MainWindow).setCurSpeed(int.Parse(CurSpeed.Text));
                    ((MainWindow)Application.Current.MainWindow).CurSpeed.Text = "Current Speed:\n" + ((MainWindow)Application.Current.MainWindow).mCurSpeed.ToString() + " mph";
                }
                else if (sender == CmdAuthority)
                {
                    ((MainWindow)Application.Current.MainWindow).setCmdAuthority(int.Parse(CmdAuthority.Text));
                    ((MainWindow)Application.Current.MainWindow).CmdAuthority.Text = "Commanded Authority:\n" + ((MainWindow)Application.Current.MainWindow).mCmdAuthority + " blocks";
                }
                else if (sender == CurAuthority)
                {
                    ((MainWindow)Application.Current.MainWindow).setCurAuthority(int.Parse(CurAuthority.Text));
                    ((MainWindow)Application.Current.MainWindow).CurAuthority.Text = "Current Authority:\n" + ((MainWindow)Application.Current.MainWindow).mCurAuthority + " blocks";
                }
                else if (sender == CurBeacon)
                {
                    ((MainWindow)Application.Current.MainWindow).setBeacon(CurBeacon.Text);
                }
                else if (sender == CurPower)
                {
                    ((MainWindow)Application.Current.MainWindow).setPower(int.Parse(CurPower.Text));
                    ((MainWindow)Application.Current.MainWindow).CurPower.Text = "Power: " + ((MainWindow)Application.Current.MainWindow).mCurPower + " kW";
                }
            }
        }
    }
}
