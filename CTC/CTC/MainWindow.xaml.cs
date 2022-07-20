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
    public partial class MainWindow : Window
    {
        public List<Train> TrainList = new List<Train>(); //Global TrainList
                                                          
        Default_Page default_page = new Default_Page(); //Create the center-pane windows that will be switched between
        Dispatch dispatch = new Dispatch();
        Train_Data train_data = new Train_Data();
        Block_Data block_data = new Block_Data();
        public List<TrackModel.Line> mLines;


        public MainWindow()
        {
            MessageBox.Show("poo!");
            InitializeComponent(); ///Default code
            Frame.NavigationService.Navigate(default_page);
        }    

        private void SelectTrain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
                    List<string> trackLine = new List<string>();
                    List<string> name = new List<string>();
                    List<int> destination = new List<int>();
                    List<TimeSpan> ETA = new List<TimeSpan>();

                    int i = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine(); //Reads one line at a time
                        var values = line.Split(','); //Split the single line into 4 columns

                        if (i >= 1) //The first row (line) of the file has the header names, so only start saving the second row onwards
                        {
                            trackLine.Add(values[0]);
                            name.Add(values[1]);
                            destination.Add(Int32.Parse(values[2]));
                            ETA.Add(TimeSpan.Parse(values[3]));
                        }
                        i++; 
                    }
                    
                    int numTrains = trackLine.Count; //The number of trains 
                    Train.numTrains = numTrains;    //Save to Train static member variable
                   
                    //Create List of trains
                    for (i=0; i < numTrains; i++)
                    {
                        TrainList.Add(new Train {line = trackLine[i], name = name[i], destination = destination[i], ETA = ETA[i] });
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

                train_data.Dest.IsEnabled = true; //on the train_data page, enable editing of the Destination box if the user is in manual mode
                train_data.ETA.IsEnabled = true; // on the train_data page, enable editing of the ETA box
                block_data.Left.IsEnabled = true; //on block_data page, enable editing of left/right switch buttons
                block_data.Right.IsEnabled = true;
                block_data.Close.IsEnabled = true; //on block_data page, disabled editing of close/open block status
                block_data.Open.IsEnabled = true;


            }
            else //This means the system is NOT in manual mode so the schedule should not be able to be loaded
            {
                LoadSchedule.IsEnabled = false; //disables the load schedule button
                Dispatch.IsEnabled = false; //disable dispath new train button

                train_data.Dest.IsEnabled = false; //on the train_data page, disable editing of the Destination box
                train_data.ETA.IsEnabled = false; //on the train_data page, disable the editing of the ETA box
                block_data.Left.IsEnabled = false; //on block_data page, disable editing of left/right switch buttons
                block_data.Right.IsEnabled = false;
                block_data.Close.IsEnabled = false; //on block_data page, disabled editing of close/open block status
                block_data.Open.IsEnabled = false;
            }

        }
        public void SetTrackData(List<object> data)
        {

        }
        private void Dispatch_Click(object sender, RoutedEventArgs e) ///Dispatch train button selected, switch to the dispatch page
        {
            
            Frame.NavigationService.Navigate(dispatch); ///Set the frame area to go to the dispatch_page
        }

        public void SetTrackData(List<TrackModel.Line> data)
        {
            mLines = data;
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e) ///After the user types the block code in and hits the enter key
        {
            if (e.Key == Key.Return)
            {
                //enter key is down
                Frame.NavigationService.Navigate(block_data);
            }
        }

        private void Frame_ContentRendered(object sender, EventArgs e) ///Every time the frame changes, it readjusts to fit. Without this code, the page that is sent to the frame gets cut off. Not sure if this is the best implimentation because it causes the entire window to resize a bit, look into later.
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }


        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Frame.NavigationService.Navigate(default_page);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.NavigationService.Navigate(block_data);
        }
    }
}