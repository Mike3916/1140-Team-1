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

namespace CTC
{
    /// <summary>
    /// Interaction logic for Dispatch.xaml
    /// </summary>
    public partial class Dispatch : Page
    {
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
        }

        
    }
}
