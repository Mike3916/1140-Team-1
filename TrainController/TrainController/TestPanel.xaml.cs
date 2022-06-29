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
            if (sender == EmergencyBrakes)
            {
                /*if ()
                {

                }
                else
                {

                }*/
            }
            else if (sender == LeftDoors)
            {
                if (!((MainWindow)Application.Current.MainWindow).mLeftDoorsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).mLeftDoorsStatus = true;
                    ((MainWindow)Application.Current.MainWindow).LeftDoors.Content = "Doors - Left\n    (OPEN)";
                    LeftDoors.Content = "OPEN";
                    LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).mLeftDoorsStatus = false;
                    ((MainWindow)Application.Current.MainWindow).LeftDoors.Content = "Doors - Left\n  (CLOSED)";
                    LeftDoors.Content = "CLOSED";
                    LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }
            else if (sender == RightDoors)
            {
                if (!((MainWindow)Application.Current.MainWindow).mRightDoorsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).mRightDoorsStatus = true;
                    ((MainWindow)Application.Current.MainWindow).RightDoors.Content = "Doors - Right\n    (OPEN)";
                    RightDoors.Content = "OPEN";
                    RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).mRightDoorsStatus = false;
                    ((MainWindow)Application.Current.MainWindow).RightDoors.Content = "Doors - Right\n  (CLOSED)";
                    RightDoors.Content = "CLOSED";
                    RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
