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
        int line =0;  //This hold the line, 0=red, 1=green
        int blockNum; //This holds the block number (to get BlockIdx do this - 1)
        int blockIdx =0;
        int blockType = 0; //This will indicate the type of block selected (0 = normal, 1 = switch, 2 = crossing);

        public Block_Data()
        {
            InitializeComponent();

        }
        public void loadBlockInfo()
        {
            line = ((MainWindow)Application.Current.MainWindow).LineCombo.SelectedIndex; //This hold the line, 0=red, 1=green
            blockNum = Int32.Parse(((MainWindow)Application.Current.MainWindow).BlockCombo.SelectedValue.ToString()); //This holds the block number (starts at 1) (to get BlockID, do this - 1)
            blockIdx = blockNum - 1;
            blockType = 0; //This will indicate the type of block selected (0 = normal, 1 = switch, 2 = crossing);




            //Here the CTC loads only the necessary items depending on the type of block
            blockType = 0; //Start by assuming block is normal (not switch or crossing)
            if (((MainWindow)Application.Current.MainWindow).mLines[line].GetBlock(blockNum).mSwitch == true) //set blockType to 1 if it a switch block //send GetBlock(blockNum) because GetBlock recieves index 1, and "blockNum" starts at index 1
                blockType = 1;
            else if (((MainWindow)Application.Current.MainWindow).mLines[line].GetBlock(blockNum).mhasCross == true) //set blockType to 2 if it is a crossing block (need to add 1 to go from block index to block number)
                blockType = 2;


            if (blockType == 0) //If the selecte block is just a normal block, it doesn't have a switch or crossing light, so hide these properties
            {
                ToggleButton.Visibility = Visibility.Collapsed; //Hide switching and crossing info
                SwitchText.Visibility = Visibility.Collapsed;
                SwitchNum.Visibility = Visibility.Collapsed;

                CrossingRect.Visibility = Visibility.Collapsed;
                CrossingText.Visibility = Visibility.Collapsed;

            }
            else if (blockType == 1) //The selected block is a switch block, hide crossing info
            {
                ToggleButton.Visibility = Visibility.Visible; //Show switch info
                SwitchText.Visibility = Visibility.Visible;
                SwitchNum.Visibility = Visibility.Visible;

                CrossingRect.Visibility = Visibility.Collapsed; //Hide crossing info
                CrossingText.Visibility = Visibility.Collapsed;


                if (line == 0)
                {
                    SwitchNum.Text = ((MainWindow)Application.Current.MainWindow).mRedSwitches[blockIdx].ToString();
                    // User can only toggle switch block if the CTC is in maintenance mode, AND the block is in maintenance mode
                    if (((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[blockIdx] == 0)
                        ToggleButton.IsEnabled = false;
                    else if (((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[blockIdx] == 1 && ((MainWindow)Application.Current.MainWindow).Mode.IsChecked == true)
                        ToggleButton.IsEnabled = true;
                }

                else if (line == 1)
                {
                    SwitchNum.Text = ((MainWindow)Application.Current.MainWindow).mGreenSwitches[blockIdx].ToString();
                    // User can only toggle switch block if the CTC is in maintenance mode, AND the block is in maintenance mode
                    if (((MainWindow)Application.Current.MainWindow).mGreenMaintenanceBlocks[blockIdx] == 0)
                        ToggleButton.IsEnabled = false;
                    else if (((MainWindow)Application.Current.MainWindow).mGreenMaintenanceBlocks[blockIdx] == 1 && ((MainWindow)Application.Current.MainWindow).Mode.IsChecked == true)
                        ToggleButton.IsEnabled = true;
                }
            }
            else if (blockType == 2) //The block is a crossing block
            {
                ToggleButton.Visibility = Visibility.Collapsed; //Hide switch info
                SwitchText.Visibility = Visibility.Collapsed;
                SwitchNum.Visibility = Visibility.Collapsed;

                CrossingRect.Visibility = Visibility.Visible; //Show crossing info
                CrossingText.Visibility = Visibility.Visible;

                if (line == 0) //red line
                    if (((MainWindow)Application.Current.MainWindow).mRedCrossings[blockIdx] == 0) //no crossing light
                        CrossingRect.Fill = System.Windows.Media.Brushes.LightGray;
                    else if (((MainWindow)Application.Current.MainWindow).mRedCrossings[blockIdx] == 1) //crossing light on
                        CrossingRect.Fill = System.Windows.Media.Brushes.LightGreen;
                    else if (line == 1) //green line
                        if (((MainWindow)Application.Current.MainWindow).mGreenCrossings[blockIdx] == 0) //no crossing light
                            CrossingRect.Fill = System.Windows.Media.Brushes.LightGray;
                        else if (((MainWindow)Application.Current.MainWindow).mGreenCrossings[blockIdx] == 1) //crossing light on
                            CrossingRect.Fill = System.Windows.Media.Brushes.LightGreen;

            }

            /////////////////////Info about all trains
            if (line == 0) //The selected line is red, put the info here that should always show up for every type of block (occupancy, maintenance, throughput, and signal light)
            {
                //Check occupancy
                if (((MainWindow)Application.Current.MainWindow).mRedOccupancies[blockIdx] == 0) //the block is unoccupied
                {
                    UnoccupiedRect.Fill = System.Windows.Media.Brushes.LightGreen;
                    OccupiedRect.Fill = System.Windows.Media.Brushes.LightGray;
                }
                else if (((MainWindow)Application.Current.MainWindow).mRedOccupancies[blockIdx] == 1)
                {
                    UnoccupiedRect.Fill = System.Windows.Media.Brushes.LightGray;
                    OccupiedRect.Fill = System.Windows.Media.Brushes.LightGreen;
                }
                //////////////////////// Check maintenance
                if (((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[blockIdx] == 0) //the block is not in maintenance (open)
                {
                    Close.Background = System.Windows.Media.Brushes.LightGray;
                    Open.Background = System.Windows.Media.Brushes.LightGreen;
                }
                else if (((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[blockIdx] == 1) //The block is in maintenance (closed)
                {
                    Close.Background = System.Windows.Media.Brushes.LightGreen;
                    Open.Background = System.Windows.Media.Brushes.LightGray;
                }
                //////////////////////// Check signal light color
                if (((MainWindow)Application.Current.MainWindow).mRedLeftLights[blockIdx] == 0) //Check left lights
                    SignalLeft.Fill = OccupiedRect.Fill = System.Windows.Media.Brushes.LightGray;
                else if (((MainWindow)Application.Current.MainWindow).mRedLeftLights[blockIdx] == 1)
                    SignalLeft.Fill = OccupiedRect.Fill = System.Windows.Media.Brushes.LightGreen;

                if (((MainWindow)Application.Current.MainWindow).mRedRightLights[blockIdx] == 0) //Check right lights
                    SignalRight.Fill = OccupiedRect.Fill = System.Windows.Media.Brushes.LightGray;
                else if (((MainWindow)Application.Current.MainWindow).mRedRightLights[blockIdx] == 1)
                    SignalRight.Fill = OccupiedRect.Fill = System.Windows.Media.Brushes.LightGreen;


                ////////////////////////Check throughput for the line


            }
            else if (line == 1) //The selected line is green, put the info here that should always show up for every type of block
            {
                //Copy above but for greeninfo insead of red
            }


        }

        public void checkToggle() //If the main CTC maintenance mode is changed, this function is called to see if the toggleButton should be enabled
        {
            if (line== 0) //Red line
                if (blockType == 1 && ((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[blockIdx] == 0) //mRedMaintenanceBlocks = 0 means it is NOT in maintenance
                    ToggleButton.IsEnabled = false;
                else if (blockType ==1 && ((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[blockIdx] == 1)
                    ToggleButton.IsEnabled = true;
            else if (line == 1) //Green line
                if (blockType == 1 && ((MainWindow)Application.Current.MainWindow).mGreenMaintenanceBlocks[blockIdx] == 0)
                    ToggleButton.IsEnabled = false;
                else if (blockType == 1 && ((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[blockIdx] == 1)
                        ToggleButton.IsEnabled = true;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Open.Background = Brushes.LightGreen;
            Close.Background = Brushes.LightGray;


            if (line==0)
                ((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[blockIdx] = 0; //Block is out of maintenance, send 0
            else if(line==1)
                ((MainWindow)Application.Current.MainWindow).mGreenMaintenanceBlocks[blockIdx] = 0; //Block is out of maintenance, send 0

            checkToggle(); //Changing the maintenance status to closed may allow the toggle button to be shown, so call this function
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close.Background = Brushes.LightGreen;
            Open.Background = Brushes.LightGray;


            if (line == 0)
                ((MainWindow)Application.Current.MainWindow).mRedMaintenanceBlocks[blockIdx] = 1; //Block is out of maintenance, send 1
            else if (line == 1)
                ((MainWindow)Application.Current.MainWindow).mGreenMaintenanceBlocks[blockIdx] = 1; //Block is out of maintenance, send 1

            checkToggle(); //Changing the maintenance status to closed may allow the toggle button to be shown, so call this function
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e) //Change the switch to it's opposite position
        {
            if (line==0) //red line
                ((MainWindow)Application.Current.MainWindow).mRedSwitches[blockIdx] = 1; //To indicate that the switch should be toggled, set the switch to 1
            else if (line==1) //green line
                ((MainWindow)Application.Current.MainWindow).mGreenSwitches[blockIdx] = 1;
        }
    }
}