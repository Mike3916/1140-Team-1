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
    /// Interaction logic for Dispatch.xaml
    /// </summary>
    public partial class Dispatch : Page
    {
        int[] redStation = { 7, 16, 21, 25, 35, 45, 48, 60 }; //Matches StationCombo index numbers to the block numbers for the red line (index starting at 1)
        int[] greenStation = { 2, 9, 16, 22, 31, 39, 48, 57, 65, 73, 77, 88, 96, 105, 114, 123, 132, 141 };
        public Dispatch()
        {
            InitializeComponent();
            LineCombo.Items.Clear();
            StationCombo.Items.Clear();

        }

        //The LineCombo data is loaded in the SetTrackData function in MainWindow
        private void LineCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            List<string> stations = new List<string>(); //List of strings to hold station names

            for (int i = 0; i < ((MainWindow)Application.Current.MainWindow).mLines[LineCombo.SelectedIndex].GetmnumSections(); i++) //Step through each section
            {
                for (int j = 0; j < ((MainWindow)Application.Current.MainWindow).mLines[LineCombo.SelectedIndex].mSections[i].getmnumBlocks(); j++) //Step through each block
                {
                    if (((MainWindow)Application.Current.MainWindow).mLines[LineCombo.SelectedIndex].mSections[i].mBlocks[j].mStation == true) //This block has a station if true
                    {
                        stations.Add(((MainWindow)Application.Current.MainWindow).mLines[LineCombo.SelectedIndex].mSections[i].mBlocks[j].mstationName); //Add the station name to the list
                    }
                }
            }
            StationCombo.Items.Clear(); //Clear empty space from StationCombo
            foreach(string station in stations) //Add each station name to the station comboBox
                StationCombo.Items.Add(station);

        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            //Here, save the Destination and ETA, then return to the default page
            this.NavigationService.GoBack();

            ((MainWindow)Application.Current.MainWindow).totalTrains++; //Increment totalTrains (this never decreases)

            string tempName = "train_" + (((MainWindow)Application.Current.MainWindow).totalTrains - 1).ToString(); //Create new train name train_<train#>

            int destNum = 0; //This will hold the number of the destination block
            if (LineCombo.SelectedIndex == 0) //This means the red line was selected, pick appropriate red line station number
                destNum = redStation[StationCombo.SelectedIndex];
            else if(LineCombo.SelectedIndex==1)                     //This means the green line was selected, pick appropriate green line station number
                destNum = greenStation[StationCombo.SelectedIndex];


            ((MainWindow)Application.Current.MainWindow).TrainList.Add(new Train { line = LineCombo.SelectedIndex, name = tempName, destination = destNum, ETD = ((MainWindow)Application.Current.MainWindow).currentTime, ETA = DateTime.Parse(ETABox.Text) }); //NEED TO ADD ETD (which should be set to current simulation time)
            ((MainWindow)Application.Current.MainWindow).SelectTrain.Items.Add(tempName); //Add the new train's name to the SelectTrain ComboBox so the user will be able to see data about it.

            ((MainWindow)Application.Current.MainWindow).TrainList[((MainWindow)Application.Current.MainWindow).TrainList.Count - 1].calcDuration();  //This is the function call to set duration.
            ((MainWindow)Application.Current.MainWindow).TrainList[((MainWindow)Application.Current.MainWindow).TrainList.Count - 1].calcRoute();



            //Set boolean to indicate that a new train was dispatched on the red line
            if (LineCombo.SelectedIndex == 0)
                ((MainWindow)Application.Current.MainWindow).mRedTrain = true;
            else if (LineCombo.SelectedIndex == 1)
                ((MainWindow)Application.Current.MainWindow).mGreenTrain = true;
        }

        
    }
}
