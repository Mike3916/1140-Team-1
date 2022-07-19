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
    public partial class EngineerPanel : Window
    {
        public EngineerPanel()
        {
            InitializeComponent();
        }

        private void KeyDownButton(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == Kp)
                {
                    ((MainWindow)Application.Current.MainWindow).mSelectedTrain.setKp(int.Parse(Kp.Text));
                }
                else if (sender == Ki)
                {
                    ((MainWindow)Application.Current.MainWindow).mSelectedTrain.setKi(int.Parse(Ki.Text));
                }
            }
        }
    }
}
