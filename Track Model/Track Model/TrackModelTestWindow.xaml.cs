using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TrackModel
{
    /// <summary>
    /// Interaction logic for TrackModelTestWindow.xaml
    /// </summary>
    public partial class TrackModelTestWindow : Window
    {
        //MainWindow main = (MainWindow)Application.Current.MainWindow;
        public int authority;
        public int mlineIdx;
        public double speed;
        public bool traingo;
        public bool actualClose;
        public TrackModelTestWindow()
        {
            InitializeComponent();
            DestinationBox.Items.Clear();
            DestinationBox.Items.Add("Red Line");
            DestinationBox.Items.Add("Green Line");
            AuthorityBox.Text = 0 + "";
            SpeedBox.Text = 0 + "";
            actualClose = false;
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).AddTrain(((MainWindow)Application.Current.MainWindow).mLines[mlineIdx].mnumBlocks,
                                                                    mlineIdx, authority);
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
            mlineIdx = DestinationBox.SelectedIndex;
        }

        private void TestWindowClosed(object sender, EventArgs e)
        {
            //this.Visibility = Visibility.Collapsed;
            
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!actualClose)
            {
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
            }
        }
    }
}
