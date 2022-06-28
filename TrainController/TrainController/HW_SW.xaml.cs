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
                ((MainWindow)Application.Current.MainWindow).mControlType = false;
                ((MainWindow)Application.Current.MainWindow).SelectType.Text = "Software Controller";
                ((MainWindow)Application.Current.MainWindow).SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0xDF, 0x20)); ;
                SoftwareController.IsEnabled = false;
                HardwareController.IsEnabled = false;
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).mControlType = true;
                ((MainWindow)Application.Current.MainWindow).SelectType.Text = "Hardware Controller";
                ((MainWindow)Application.Current.MainWindow).SelectType.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8F, 0x5F, 0xA0)); ;
                SoftwareController.IsEnabled = false;
                HardwareController.IsEnabled = false;
            }
        }
    }
}
