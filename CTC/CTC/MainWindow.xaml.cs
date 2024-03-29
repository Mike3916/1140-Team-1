﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Globalization;
using CTCObject= Backend.CTCObject;
using Train = Backend.Train;
using Backend;
using Train_Data = CTC.Train_Data;

namespace CTC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    //MessageBox.Show();
    public partial class MainWindow : Window
    {
        public List<Train> TrainList = new List<Train>(); //Global TrainList
        public int totalTrains; //This will keep track of total number of trains in a day (it does NOT go down even when a train reaches destination). It is also used to assign train names in Dispatch
        public int redThroughput; //This is the number red line tickets sold per hour
        public int greenThroughput; //This is the number of green line tickets sold per hour

        public List<TrackModel.Line> mLines;    //Hold the track model
        public bool trackLoaded = false;        //Keep track of whether the track has been loaded or not
        public DateTime currentTime; //This holds the current time

        Default_Page default_page = new Default_Page(); //Create the center-pane windows that will be switched between
        Dispatch dispatch = new Dispatch();
        Train_Data train_data = new Train_Data(); 
        Block_Data block_data = new Block_Data(); //Don't load at the start, it relies on data that has not been initialized yet (the select Block comboBOxes)

        

        

        //Variables to send to Track Controller
        //Use block indexes starting with 0
        public int[] mRedMaintenanceBlocks = new int[77]; // 0 means not in maintenance, 1 means it is in maintenance
        public int[] mRedOccupancies = new int[77];       // 0 means not occupied, 1 means it is occupied
        public int[] mRedSpeeds = new int[77];
        public int[] mRedAuthorities = new int[77];
        public int[] mRedCrossings = new int[77];
        public int[] mRedSwitches = new int[77]; //The integer stored here is the index of the next block the switch points to (I will send a 1 to indicate the switch is toggled by dispatcher in Block_Data
        public int[] mRedLeftLights = new int[77];
        public int[] mRedRightLights = new int[77];

        public int[] mGreenMaintenanceBlocks = new int[151];
        public int[] mGreenOccupancies = new int[151];
        public int[] mGreenSpeeds = new int[151];
        public int[] mGreenAuthorities = new int[151];
        public int[] mGreenCrossings = new int[151];
        public int[] mGreenSwitches = new int[151];
        public int[] mGreenLeftLights = new int[151];
        public int[] mGreenRightLights = new int[151];

        public bool mRedTrain = false; //This is set to true immediately after dispatching a train on the red line, gog will set it back to false after
        public bool mGreenTrain = false; 

        

        public MainWindow()
        {
            InitializeComponent(); ///Default code
            Frame.NavigationService.Navigate(default_page); //Show default blank page at center of screen

            Application.Current.MainWindow = this;

            SelectTrain.Items.Clear(); //Clear out the default empty spaces in all the comboBoxes
            LineCombo.Items.Clear();
            SectionCombo.Items.Clear();
            BlockCombo.Items.Clear();
        }    

        private void SelectTrain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            train_data.update_Train_Data(); //Update the train data
            Frame.NavigationService.Navigate(train_data);
        }

        

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog(); ///This creates OpenFileDialog object that can save a file
            bool? response = openFileDialog.ShowDialog(); ///The ShowDialog() causes file explorer prompt to show up. User navigates to file and double clicks it. It returns a nullable boolean (a bool that can be true, false, or null). To declare a nullable boolean you use: bool? myBool. If the file works, it returns true.

            if (response == true) ///If a file was correctly selected
            {
                string filepath = openFileDialog.FileName; ///Save the filename
                using (var reader = new StreamReader(filepath))
                {
                    List<int> trackLine = new List<int>();
                    List<string> name = new List<string>();
                    List<int> destination = new List<int>();
                    List<DateTime> ETD = new List<DateTime>();
                    List<DateTime> ETA = new List<DateTime>();

                    int i = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine(); //Reads one line at a time
                        var values = line.Split(','); //Split the single line into 4 columns

                        if (i >= 1) //The first row (line) of the file has the header names, so only start saving the second row onwards
                        {
                            trackLine.Add(Int32.Parse(values[0]));
                            name.Add(values[1]);
                            destination.Add(Int32.Parse(values[2]));
                            ETD.Add(DateTime.Parse(values[3]));
                            ETA.Add(DateTime.Parse(values[4]));
                        }
                        i++; 
                    }
                    
                    totalTrains = trackLine.Count; //The number of trains 
                    Train.numTrains = totalTrains;    //Save to Train static member variable

                    //Create List of trains
                    SelectTrain.Items.Clear(); //Clears the default item pre-loaded into combo-box
                    for (i=0; i < totalTrains; i++)
                    {
                        TrainList.Add(new Train {line = trackLine[i], name = name[i], destination = destination[i], ETD = ETD[i], ETA = ETA[i] });
                        TrainList[i].calcDuration();
                        TrainList[i].calcRoute();
                        SelectTrain.Items.Add(name[i]); //This populates the drop down list of trains with the train names
                    }
                } 

            }
                Track.Visibility = Visibility.Visible;
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e) ///The checkbox to put CTC in manual mode is checked
        {
            if (Mode.IsChecked==true) //This means system is in manual mode and the schedule can be loaded
            {
                LoadSchedule.IsEnabled = true; //enabled the load schedule button
                Dispatch.IsEnabled = true; //enable dispatch new train button

                train_data.DestLineCombo.IsEnabled = true; //on the train_data page, enable editing of the Destination box if the user is in manual mode
                train_data.DestStationCombo.IsEnabled = true;
                train_data.ETA.IsEnabled = true; // on the train_data page, enable editing of the ETA box

                block_data.checkToggle(); //Call checkToggle() to make sure not only the CTC is in maintenance mode, but the block is in maintenance mode too
                block_data.Close.IsEnabled = true; //on block_data page, disabled editing of close/open block status
                block_data.Open.IsEnabled = true;
                train_data.UpdateTrain.IsEnabled = true; //on train_data page, make the update train button available




            }
            else //This means the system is NOT in manual mode so the schedule should not be able to be loaded
            {
                LoadSchedule.IsEnabled = false; //disables the load schedule button
                Dispatch.IsEnabled = false; //disable dispath new train button

                train_data.DestLineCombo.IsEnabled = false; //on the train_data page, disable editing of the Destination box
                train_data.DestStationCombo.IsEnabled = false;
                train_data.ETA.IsEnabled = false; //on the train_data page, disable the editing of the ETA box

                block_data.ToggleButton.IsEnabled = false; //on block_data page, disable editing of toggle switch button
                block_data.Close.IsEnabled = false; //on block_data page, disabled editing of close/open block status
                block_data.Open.IsEnabled = false;
                
                
                train_data.UpdateTrain.IsEnabled = false;
            }

        }

        private void Dispatch_Click(object sender, RoutedEventArgs e) ///Dispatch train button selected, switch to the dispatch page
        {
            Frame.NavigationService.Navigate(dispatch); ///Set the frame area to go to the dispatch_page
        }

        public void GetTrackLayout(List<TrackModel.Line> data)
        {
            if (trackLoaded == false) //This is called every tick right now, but we only want to read it once
            {
                trackLoaded = true;
                mLines = data;
                for (int i = 0; i < mLines.Count; i++)
                {
                    dispatch.LineCombo.Items.Add(mLines[i].GetmnameLine());  //This relies on data being loaded elsewhere for track data, so only do it when the dispatch page has been loaded. This populates the lines comboBox of the Dispatch page
                    LineCombo.Items.Add(mLines[i].GetmnameLine());  //This populates the LineCombo Box on the Main Window Page
                }
            }
        }
        public void getThroughput(int redTP, int greenTP)
        {
            redThroughput = redTP;
            greenThroughput = greenTP;
        }

        public void GetTrackController(int[] RedMaintenanceBlocks, int[] RedOccupancies, int[] RedSpeeds, int[] RedAuthorities, int[] RedCrossings, int[] RedSwitches, int[] RedLeftLights, int[] RedRightLights, int[] GreenMaintenanceBlocks, int[] GreenOccupancies, int[] GreenSpeeds, int[] GreenAuthorities, int[] GreenCrossings, int[] GreenSwitches, int[] GreenLeftLights, int[] GreenRightLights) //Write function in CTC to read in these values
        {
            mRedMaintenanceBlocks = RedMaintenanceBlocks;
            mRedOccupancies = RedOccupancies;
            mRedSpeeds = RedSpeeds;
            mRedAuthorities = RedAuthorities;
            mRedCrossings = RedCrossings;
            mRedSwitches = RedSwitches;
            mRedLeftLights = RedLeftLights;
            mRedRightLights = RedRightLights;

            mGreenMaintenanceBlocks = GreenMaintenanceBlocks;
            mGreenOccupancies = GreenOccupancies;
            mGreenSpeeds = GreenSpeeds;
            mGreenAuthorities = GreenAuthorities;
            mGreenCrossings = GreenCrossings;
            mGreenSwitches = GreenSwitches;
            mGreenLeftLights = GreenLeftLights;
            mGreenRightLights = GreenRightLights;

        }
        public void getTime(DateTime gogNow)
        {
            currentTime = gogNow; //This holds the current system time
        }

        private void LineCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) //Once user selects a line, populate the SectionCombo box with all the sections in that line
        {
            for (int i = 0; i < mLines[LineCombo.SelectedIndex].GetmnumSections(); i++) //Step through each section
                SectionCombo.Items.Add(mLines[LineCombo.SelectedIndex].mSections[i].getmnameSection());
        }
        private void SectionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) //Once user selects a section, populate the BlockCombo box with all the blocks in that section
        {
            for (int i = 0; i < mLines[LineCombo.SelectedIndex].mSections[SectionCombo.SelectedIndex].getmnumBlocks(); i++)
                BlockCombo.Items.Add(mLines[LineCombo.SelectedIndex].mSections[SectionCombo.SelectedIndex].mBlocks[i].GetmblockNum());

        }

        private void Frame_ContentRendered(object sender, EventArgs e) ///Every time the frame changes, it readjusts to fit. Without this code, the page that is sent to the frame gets cut off. Not sure if this is the best implimentation because it causes the entire window to resize a bit, look into later.
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Frame.NavigationService.Navigate(default_page);
        }

        private void SeeBlock_Click(object sender, RoutedEventArgs e)
        {
            block_data.loadBlockInfo(); //This is a function in block_data that loads information into the page
            Frame.NavigationService.Navigate(block_data);
        }
    }
}