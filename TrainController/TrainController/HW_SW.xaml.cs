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
    public partial class HW_SW : Window
    {
        public HW_SW()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == SoftwareController)
            {
                // Set controller type to software, and show on main window:
                ((MainWindow)Application.Current.MainWindow).mControlType = false;
                ((MainWindow)Application.Current.MainWindow).SelectType.Text = "Software Controller";
                ((MainWindow)Application.Current.MainWindow).SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0xDF, 0x20));

                // Enable all manual mode buttons on main window:
                // Controller enters manual mode by default
                ((MainWindow)Application.Current.MainWindow).ManualMode.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).AutoMode.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).ServiceBrake.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).EmergencyBrake.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).SetSpeedBox.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).EngineerPanel.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TestPanel.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TempIncrease.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TempDecrease.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).Announcements.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).LeftDoors.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).RightDoors.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).LightsInterior.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).LightsExterior.IsEnabled = true;

                // Disable both controller type buttons and exit to main window:
                SoftwareController.IsEnabled = false;
                HardwareController.IsEnabled = false;
                this.Close();
            }
            else
            {
                // Set controller type to hardware, and show on main window:
                ((MainWindow)Application.Current.MainWindow).mControlType = true;
                ((MainWindow)Application.Current.MainWindow).SelectType.Text = "Hardware Controller";
                ((MainWindow)Application.Current.MainWindow).SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0x5F, 0xA0));

                // Enable all manual mode buttons on main window:
                // Controller enters manual mode by default
                ((MainWindow)Application.Current.MainWindow).ManualMode.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).AutoMode.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).ServiceBrake.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).EmergencyBrake.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).SetSpeedBox.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).EngineerPanel.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TestPanel.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TempIncrease.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TempDecrease.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).Announcements.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).LeftDoors.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).RightDoors.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).LightsInterior.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).LightsExterior.IsEnabled = true;

                // Disable both controller type buttons and exit to main window:
                SoftwareController.IsEnabled = false;
                HardwareController.IsEnabled = false;
                this.Close();
            }
        }
    }
}
