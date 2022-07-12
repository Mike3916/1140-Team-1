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
                ((MainWindow)Application.Current.MainWindow).mSelectedTrain.mControlType = false;
                ((MainWindow)Application.Current.MainWindow).SelectType.Text = "Software Controller";
                ((MainWindow)Application.Current.MainWindow).SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0xDF, 0x20));

                // Controller enters automatic mode by default:
                ((MainWindow)Application.Current.MainWindow).ManualMode.IsEnabled = true;

                // Enable all brake buttons and outer panel buttons:
                ((MainWindow)Application.Current.MainWindow).ServiceBrake.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).EmergencyBrake.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).EngineerPanel.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TestPanel.IsEnabled = true;

                // Disable all automatic mode buttons on main window:
                ((MainWindow)Application.Current.MainWindow).AutoMode.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).SetSpeedBox.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).TempIncrease.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).TempDecrease.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).Announcements.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).LeftDoors.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).RightDoors.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).InteriorLights.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).ExteriorLights.IsEnabled = false;

                // Disable both controller type buttons and exit to main window:
                SoftwareController.IsEnabled = false;
                HardwareController.IsEnabled = false;
                this.Close();
            }
            else
            {
                // Set controller type to hardware, and show on main window:
                ((MainWindow)Application.Current.MainWindow).mSelectedTrain.mControlType = true;
                ((MainWindow)Application.Current.MainWindow).SelectType.Text = "Hardware Controller";
                ((MainWindow)Application.Current.MainWindow).SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0x5F, 0xA0));

                // Controller enters automatic mode by default:
                ((MainWindow)Application.Current.MainWindow).ManualMode.IsEnabled = true;

                // Enable all brake buttons and outer panel buttons:
                ((MainWindow)Application.Current.MainWindow).ServiceBrake.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).EmergencyBrake.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).EngineerPanel.IsEnabled = true;
                ((MainWindow)Application.Current.MainWindow).TestPanel.IsEnabled = true;

                // Disable all automatic mode buttons on main window:
                ((MainWindow)Application.Current.MainWindow).AutoMode.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).SetSpeedBox.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).TempIncrease.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).TempDecrease.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).Announcements.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).LeftDoors.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).RightDoors.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).InteriorLights.IsEnabled = false;
                ((MainWindow)Application.Current.MainWindow).ExteriorLights.IsEnabled = false;

                // Setup hardware controller port information:
                ((MainWindow)Application.Current.MainWindow).mSelectedTrain.setupHardware();

                // Disable both controller type buttons and exit to main window:
                SoftwareController.IsEnabled = false;
                HardwareController.IsEnabled = false;
                this.Close();
            }
        }
    }
}
