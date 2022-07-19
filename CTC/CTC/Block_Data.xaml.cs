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
            Right.Background = Brushes.Green;
            Left.Background = Brushes.Gray;
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            Left.Background = Brushes.Green;
            Right.Background = Brushes.Gray;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Open.Background = Brushes.Green;
            Close.Background = Brushes.Gray;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close.Background = Brushes.Green;
            Open.Background = Brushes.Gray;
        }
    }
}
