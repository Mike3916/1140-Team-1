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
using System.Diagnostics; //Added to be able to quickly write output 
using Train = Backend.Train;
using Backend;

namespace CTC
{
    /// <summary>
    /// Interaction logic for Block_Data.xaml
    /// </summary>
    public partial class Block_Data : Page
    {
        
        int line = ((MainWindow)Application.Current.MainWindow).LineCombo.SelectedIndex; //This hold the line, 0=red, 1=green
        int block = ((MainWindow)Application.Current.MainWindow).LineCombo.SelectedIndex; //This holds the block index (to get BlockID, do this + 1)
        int blockType; //This will indicate the type of block selected (0 = normal, 1 = switch, 2 = crossing);
        public Block_Data()
        {
            InitializeComponent(); 
            //Here the CTC loads only the necessary items depending on the type of block
            if (((MainWindow)Application.Current.MainWindow).mLines[line].GetBlock(block + 1).mInfrastructure)


            if ( line == 0 ) //The selected line is red
            {

            }
            else if ( line == 1 ) //The selected line is greenj
            {

            }
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
