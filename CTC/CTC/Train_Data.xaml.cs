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
    public partial class Train_Data : Page
    {
        public Train_Data()
        {
            InitializeComponent();

        }


        private void Dest_Loaded(object sender, RoutedEventArgs e) //Display the saved destination data for the selected train
        {
  
            int i = ((MainWindow)Application.Current.MainWindow).SelectTrain.SelectedIndex; //Right now, the combobox index 0 is blank, so the first train (w/ index zero) appears at index 1. Therefore, subtract 1 to get the correct train
            Dest.Text = ((MainWindow)Application.Current.MainWindow).TrainList[i].destination.ToString();
        }

        private void ETA_Loaded(object sender, RoutedEventArgs e) //Display the saved ETA data for the selected train
        {
            int i = ((MainWindow)Application.Current.MainWindow).SelectTrain.SelectedIndex; //Right now, the combobox index 0 is blank, so the first train (w/ index zero) appears at index 1. Therefore, subtract 1 to get the correct train
            ETA.Text = ((MainWindow)Application.Current.MainWindow).TrainList[i].ETA.ToString();
        }

    }
}
