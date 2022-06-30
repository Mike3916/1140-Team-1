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
using System.Windows.Shapes;

namespace TrackModel_v0._1
{
    /// <summary>
    /// Interaction logic for TrackModelTestWindow.xaml
    /// </summary>
    public partial class TrackModelTestWindow : Window
    {
        //MainWindow main = (MainWindow)Application.Current.MainWindow;
        public int authority;
        public int destination;
        public double speed;
        public bool traingo;
        public TrackModelTestWindow()
        {
            InitializeComponent();
            DestinationBox.Items.Add("Station B");
            DestinationBox.Items.Add("Station C");
            AuthorityBox.Text = 0 + "";
            SpeedBox.Text = 0 + "";
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            traingo = true;
        }

        private void AuthorityBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AuthorityBox.IsFocused == true)
            {
                if (int.TryParse(AuthorityBox.Text, out int info) == true)
                {
                    authority = info;
                }
                else
                    MessageBox.Show("Only numbers PLEASE");
            }
        }

        private void SpeedBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SpeedBox.IsFocused == true)
            {
                if (double.TryParse(SpeedBox.Text, out double info) == true)
                {
                    speed = info;
                }
                else
                    MessageBox.Show("Only numbers PLEASE");
            }
        }

        private void DestinationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            destination = DestinationBox.SelectedIndex;
        }
    }
}
