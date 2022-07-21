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

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            LineCombo.Items.Add(((MainWindow)Application.Current.MainWindow).mLines[((MainWindow)Application.Current.MainWindow).mLines.Count - 1].getmnameLine());  //Don't do this in public Dispatch() above. This relies on data being loaded elsewhere for track data, so only do it when the dispatch page has been loaded
        }

        private void LineCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> stations = new List<string>();
            for (int i = 0; i < ((MainWindow)Application.Current.MainWindow).mLines[LineCombo.SelectedIndex].getmnumSections(); i++)
            {
                for (int j = 0; j < ((MainWindow)Application.Current.MainWindow).mLines[LineCombo.SelectedIndex].getmnumBlocks(); j++)
                {
                    if (((MainWindow)Application.Current.MainWindow).mLines[LineCombo.SelectedIndex].mSections[i].mBlocks[j].mStation == true) //This block has a station if true
                    {
                        stations.Add(((MainWindow)Application.Current.MainWindow).mLines[LineCombo.SelectedIndex].mSections[i].mBlocks[j].mstationName);
                    }
                }
            }
            StationCombo.Items.Clear(); //Clear empty space from StationCombo
            StationCombo.Items.Add(stations);

        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            ///Here, save the Destination and ETA, then return to the default page
            this.NavigationService.GoBack();
        }

        
    }
}
