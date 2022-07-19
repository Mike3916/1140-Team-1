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

            // Controller enters automatic mode by default:
            ((ControlPanel)Application.Current.MainWindow).ManualMode.IsEnabled = true;
            ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mAutoMode = true;

            // Enable all brake buttons and outer panel buttons:
            ((ControlPanel)Application.Current.MainWindow).ServiceBrake.IsEnabled = true;
            ((ControlPanel)Application.Current.MainWindow).EmergencyBrake.IsEnabled = true;
            ((ControlPanel)Application.Current.MainWindow).EngineerPanel.IsEnabled = true;
            ((ControlPanel)Application.Current.MainWindow).TestPanel.IsEnabled = true;

            // Disable all automatic mode buttons on main window:
            ((ControlPanel)Application.Current.MainWindow).AutoMode.IsEnabled = false;
            ((ControlPanel)Application.Current.MainWindow).SetSpeedBox.IsEnabled = false;
            ((ControlPanel)Application.Current.MainWindow).SetSpeed.Background = new SolidColorBrush(Color.FromArgb(0x30, 0, 0, 0));
            ((ControlPanel)Application.Current.MainWindow).TempIncrease.IsEnabled = false;
            ((ControlPanel)Application.Current.MainWindow).TempDecrease.IsEnabled = false;
            ((ControlPanel)Application.Current.MainWindow).Announcements.IsEnabled = false;
            ((ControlPanel)Application.Current.MainWindow).LeftDoors.IsEnabled = false;
            ((ControlPanel)Application.Current.MainWindow).RightDoors.IsEnabled = false;
            ((ControlPanel)Application.Current.MainWindow).InteriorLights.IsEnabled = false;
            ((ControlPanel)Application.Current.MainWindow).ExteriorLights.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == SoftwareController)
            {
                // Set controller type to software, and show on main window:
                ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mControlType = false;
                ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mSetControlType = true;
                ((ControlPanel)Application.Current.MainWindow).SelectType.Text = "Software Controller";
                ((ControlPanel)Application.Current.MainWindow).SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0xDF, 0x20));

                // Disable both controller type buttons and exit to main window:
                SoftwareController.IsEnabled = false;
                HardwareController.IsEnabled = false;

                // Begin initTimer() for selected train controller:
                ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.InitTimer();

                this.Close();
            }
            else
            {
                // Set controller type to hardware, and show on main window:
                ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mControlType = true;
                ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.mSetControlType = true;
                ((ControlPanel)Application.Current.MainWindow).SelectType.Text = "Hardware Controller";
                ((ControlPanel)Application.Current.MainWindow).SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0x5F, 0xA0));

                // Setup hardware controller port information:
                ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.setupHardware();

                // Disable both controller type buttons and exit to main window:
                SoftwareController.IsEnabled = false;
                HardwareController.IsEnabled = false;

                // Begin initTimer() for selected train controller:
                ((ControlPanel)Application.Current.MainWindow).mSelectedTrain.InitTimer();

                this.Close();
            }
        }
    }
}
