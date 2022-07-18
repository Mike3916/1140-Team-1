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

 

        private void Dest_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i = ((MainWindow)Application.Current.MainWindow).SelectTrain.SelectedIndex;
            Dest.Text = ((MainWindow)Application.Current.MainWindow).TrainList[i].destination.ToString();
        }
    }
}
