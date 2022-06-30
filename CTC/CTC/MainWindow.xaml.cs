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

            Default_Page page = new Default_Page();
            Frame.NavigationService.Navigate(page);
        }
       

        private void SelectTrain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///MessageBox.Show(SelectTrain.SelectedItem.ToString());
            Train_Data page = new Train_Data();
            Frame.NavigationService.Navigate(page);
        }

      

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog(); ///This creates OpenFileDialog object that can save a file
            bool? response = openFileDialog.ShowDialog(); ///The ShowDialog() causes file explorer prompt to show up. User navigates to file and double clicks it. It returns a nullable boolean (a bool that can be true, false, or null). To declare a nullable boolean you use: bool? myBool. If the file works, it returns true.

            if (response == true) ///If a file was correctly selected
            {
                string filepath = openFileDialog.FileName; ///Save the filename
                MessageBox.Show(filepath); ///Output the filename
            }
        }

        private void Set_Checked(object sender, RoutedEventArgs e) ///The checkbox to put CTC in manual mode is checked
        {
            
        }

        private void Dispatch_Click(object sender, RoutedEventArgs e) ///Dispatch train button selected, switch to the dispatch page
        {
            Dispatch page = new Dispatch();
            Frame.NavigationService.Navigate(page); ///Set the frame area to go to the dispatch_page
        }

       
    }
}
