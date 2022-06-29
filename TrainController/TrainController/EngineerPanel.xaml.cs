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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void KeyDownButton(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == DisplayKp)
                {
                    ((MainWindow)Application.Current.MainWindow).mKp = int.Parse(DisplayKp.Text);
                }
                else if (sender == DisplayKi)
                {
                    ((MainWindow)Application.Current.MainWindow).mKi = int.Parse(DisplayKi.Text);
                }
            }
        }
    }
}
