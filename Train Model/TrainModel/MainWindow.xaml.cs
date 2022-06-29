using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Train=TrainObject.Train;

namespace TrainModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    class railLines : ObservableCollection<string>
    {
        public railLines()
        {

            Add("Green");
            Add("Red");
            Add("Blue");
        }
    }
    public partial class MainWindow : Window
    {

        Train train;
        public MainWindow()
        {
            InitializeComponent();
            train = new Train();
            

            
        }

      

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            if (HelloButton.IsChecked == true)
            {
                double speed = train.getCurrentSpeed();
                MessageBox.Show(speed.ToString());
            }
            else if (GoodbyeButton.IsChecked == true)
            {
                MessageBox.Show("Goodbye.");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
