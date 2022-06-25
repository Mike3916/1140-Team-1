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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == AutoMode)
            {
                MessageBox.Show("Automatic Mode");
            }
            else if (sender == ManualMode)
            {
                MessageBox.Show("Manual Mode");
            }
            else if (sender == EmergencyBrake)
            {
                MessageBox.Show("Emergency Brakes engaged!");
            }
            else if (sender == LeftDoors)
            {
                MessageBox.Show("Left Doors open");
            }
            else if (sender == RightDoors)
            {
                MessageBox.Show("Right Doors open");
            }
            else if (sender == LightsInterior)
            {
                MessageBox.Show("Interior Lights on");
            }
            else if (sender == LightsExterior)
            {
                MessageBox.Show("Exterior Lights on");
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
        }
    }
}
