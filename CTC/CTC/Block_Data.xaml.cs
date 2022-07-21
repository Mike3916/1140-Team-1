using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CTC
{
    /// <summary>
    /// Interaction logic for Block_Data.xaml
    /// </summary>
    public partial class Block_Data : Page
    {
        public Block_Data()
        {
            InitializeComponent();
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            Right.Background = Brushes.LightGreen;
            Left.Background = Brushes.LightGray;
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            Left.Background = Brushes.LightGreen;
            Right.Background = Brushes.LightGray;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Open.Background = Brushes.LightGreen;
            Close.Background = Brushes.LightGray;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close.Background = Brushes.LightGreen;
            Open.Background = Brushes.LightGray;
        }
    }
}
