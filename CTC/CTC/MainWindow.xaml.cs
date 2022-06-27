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
using CTCObject= Backend.CTCObject;

namespace CTC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent(); ///Default code

            SelectTrain.Items.Add("Train_1"); ///Creating placeholder trains
            SelectTrain.Items.Add("Train_2");
            SelectTrain.Items.Add("Train_3");
        }


        private void SelectTrain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(SelectTrain.SelectedItem.ToString());
        }

      

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Click");
        }

        private void LoadFile_Init(object sender, EventArgs e)
        {
            MessageBox.Show("Init!");
        }
    }
}
