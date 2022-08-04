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
using System.Diagnostics;

using CTC;
using Backend;
using MainWindow = CTC.MainWindow;

namespace CTC
{
    /// <summary>
    /// Interaction logic for Train_Data.xaml
    /// </summary>
    /// 

    public partial class Train_Data : Page
    {
        //Need to add station names to these
        Object[,] redStation = new Object[,] { { 7, "SHADYSIDE" }, { 16, "HERRON AVE" }, { 21, "SWISSVILLE" }, { 25, "PENN STATION" }, { 35, "STEEL PLAZA" }, { 45, "FIRST AVE" }, { 48, "STATION SQUARE" }, { 60, "SOUTH HILLS JUNCTION" } }; //Matches StationCombo index numbers to the block numbers for the red line (index starting at 1)
        Object[,] greenStation = new Object[,] { { 2, "PIONEER" }, { 9, "EDGEBROOK" }, { 16, "STATION 16" }, { 22, "WHITED" }, { 31, "SOUTH BANK" }, { 39, "CENTRAL (1)" }, { 48, "INGLEWOOD (1)" }, { 57, "OVERBROOK (1)" }, { 65, "GLENBURY (1)" }, { 73, "DORMONT (1)" }, { 77, "MT LEBANON" }, { 88, "POPLAR" }, { 96, "CASTLE SHANNON" }, { 105, "DORMONT (2)" }, { 114, "GLENBURY (2)" }, { 123, "OVERBROOK (2)" }, { 132, "INGLEWOOD (2)" }, { 141, "CENTRAL (2)" } };
        String[] lineName = { "Red", "Green" }; 

        public Train_Data()
        {
            InitializeComponent();
        }

        public void update_Train_Data()
        {
            int i;
            int DestStationIndex = 0; //This is just a placeholder, it should change

            DestLineCombo.Items.Clear(); //Clear DestLineCombo Items
            DestStationCombo.Items.Clear(); //Clear DestStation Items

            DestLineCombo.Items.Add(lineName[0]); //Add "RED" to DestLineCombo
            DestLineCombo.Items.Add(lineName[1]); //Add "Green" to DestLineCombo

            //Access the TrainList, and select the Train object with the same index as the selected train, and then check which line this train is dispatched to, this line number is the same as the index that will be displayed (0="red", 1="green")
            DestLineCombo.SelectedIndex = ((MainWindow)Application.Current.MainWindow).TrainList[((MainWindow)Application.Current.MainWindow).SelectTrain.SelectedIndex].line;

            if (DestLineCombo.SelectedIndex == 0) //The red line is selected
            {
                for (i=0; i<redStation.GetLength(0); i++) //redStation.GetLength(0) returns the number of rows in the array
                {
                    DestStationCombo.Items.Add((String)redStation[i,1]); //Add the station names, unbox from Object to String
                    //Check if the redStation block number is equal to the block number of the train destination, if so, save the index i into DestStationIndex so this station will be selected later
                    if ((int)redStation[i, 0] == ((MainWindow)Application.Current.MainWindow).TrainList[((MainWindow)Application.Current.MainWindow).SelectTrain.SelectedIndex].destination)
                        DestStationIndex = i;
                }
            }
            else if (DestLineCombo.SelectedIndex == 1) //The green line is selected
            {
                for (i=0; i<greenStation.GetLength(1); i++)
                {
                    DestStationCombo.Items.Add((String)greenStation[i, 1]); //Add the station names, unbox from Object to String

                    if ((int)greenStation[i, 0] == ((MainWindow)Application.Current.MainWindow).TrainList[((MainWindow)Application.Current.MainWindow).SelectTrain.SelectedIndex].destination)
                        DestStationIndex = i;
                }
            }
            //Set the selected Index of the destination station equal to the selected train
            DestStationCombo.SelectedIndex = DestStationIndex;


            //Display the saved ETA data for the selected train
            i = ((MainWindow)Application.Current.MainWindow).SelectTrain.SelectedIndex; //Right now, the combobox index 0 is blank, so the first train (w/ index zero) appears at index 1. Therefore, subtract 1 to get the correct train
            ETA.Text = ((MainWindow)Application.Current.MainWindow).TrainList[i].ETA.ToString();

            Authority.Text = ((MainWindow)Application.Current.MainWindow).TrainList[i].authority.ToString();
            Speed.Text = ((MainWindow)Application.Current.MainWindow).TrainList[i].speed.ToString();

        }

        private void DestLineCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) //When the DestLineCombo selection is changed, the DestStationCombo needs to be changed to only show the destination for that line
        {
            DestStationCombo.Items.Clear(); //Clear DestStatinCombo so it can be updated without adding to pre-existing

            if (DestLineCombo.SelectedIndex == 0) //red line
            {
                for (int i = 0; i < redStation.GetLength(0); i++) //redStation.GetLength(0) returns the number of rows in the array
                    DestStationCombo.Items.Add((String)redStation[i, 1]); //Add the station names, unbox from Object to String
            }
            else if (DestLineCombo.SelectedIndex == 1) //green line
            {
                for (int i = 0; i < greenStation.GetLength(0); i++) //greenStation.GetLength(0) returns the number of rows in the array
                    DestStationCombo.Items.Add((String)greenStation[i, 1]); //Add the station names, unbox from Object to String
            }
        }



        private void UpdateTrain_Click(object sender, RoutedEventArgs e)
        {
            int i = ((MainWindow)Application.Current.MainWindow).SelectTrain.SelectedIndex;

            if (DestLineCombo.SelectedIndex == 0) //Red line
            {
                ((MainWindow)Application.Current.MainWindow).TrainList[i].destination = (int)redStation[DestStationCombo.SelectedIndex,0];
            }
            else if (DestLineCombo.SelectedIndex == 1) //Green line
            {
                ((MainWindow)Application.Current.MainWindow).TrainList[i].destination = (int)greenStation[DestStationCombo.SelectedIndex, 0];
            }
            
            ((MainWindow)Application.Current.MainWindow).TrainList[i].calcDuration(); //Need to recalculate duration with new destination and ETA
            ((MainWindow)Application.Current.MainWindow).TrainList[i].calcRoute();    //recalculate the route with new destination and ETA

            update_Train_Data(); //Call the update_Train_Data() function so the display for the ETA, Authority, and suggested speed will be updated
        }
    }
}
