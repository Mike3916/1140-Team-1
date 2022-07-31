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
        int blockType = 0; //This will indicate the type of block selected (0 = normal, 1 = switch, 2 = crossing);


        public Block_Data()
        {
            InitializeComponent();
       
            //Here the CTC loads only the necessary items depending on the type of block
            blockType = 0; //Start by assuming block is normal (not switch or crossing)
            if (((MainWindow)Application.Current.MainWindow).mLines[line].GetBlock(block + 1).mSwitch == true) //set blockType to 1 if it a switch block
                blockType = 1;
            else if (((MainWindow)Application.Current.MainWindow).mLines[line].GetBlock(block + 1).mhasCross == true) //set blockType to 2 if it is a crossing block
                blockType = 2;


            if (blockType == 0) //If the selecte block is just a normal block, it doesn't have a switch or crossing light, so hide these properties
            {
                Left.Visibility = Visibility.Collapsed;
                Right.Visibility = Visibility.Collapsed;
                CrossingRect.Visibility = Visibility.Collapsed;
                CrossingText.Visibility = Visibility.Collapsed;

            }
            else if (blockType ==1) //The selected block is a switch block, hide crossing info
            {
                CrossingRect.Visibility = Visibility.Collapsed;
                CrossingText.Visibility = Visibility.Collapsed;

    
            }
            else if (blockType ==2)
            {
                Left.Visibility = Visibility.Collapsed;
                Right.Visibility = Visibility.Collapsed;
            }


            if ( line == 0 ) //The selected line is red, put the info here that should always show up for every type of block (occupancy, maintenance, throughput, and signal light)
            {
                //Check occupancy
                if (((MainWindow)Application.Current.MainWindow).mRedOccupancies[block] == 0) //the block is unoccupied
                {
                    UnoccupiedRect.Fill = System.Windows.Media.Brushes.LightGreen;
                    OccupiedRect.Fill = System.Windows.Media.Brushes.LightGray;
                }
                else if (((MainWindow)Application.Current.MainWindow).mRedOccupancies[block] == 1)
                {
                    UnoccupiedRect.Fill = System.Windows.Media.Brushes.LightGray;
                    OccupiedRect.Fill = System.Windows.Media.Brushes.LightGreen;
                }
                //////////////////////// Check maintenance
                if (((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[block] == 0) //the block is not in maintenance (open)
                {
                    Close.Background = System.Windows.Media.Brushes.LightGray;
                    Open.Background = System.Windows.Media.Brushes.LightGreen;
                }
                else if (((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[block] == 1) //The block is in maintenance (closed)
                {
                    Close.Background = System.Windows.Media.Brushes.LightGreen;
                    Open.Background = System.Windows.Media.Brushes.LightGray;
                }
                //////////////////////// Check signal light color
                

                ////////////////////////Check throughput for the line


            }
            else if ( line == 1 ) //The selected line is green, put the info here that should always show up for every type of block
            {
                //Copy above but for greeninfo insead of red
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
