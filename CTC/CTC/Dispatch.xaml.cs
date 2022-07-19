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
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            ///Here, save the Destination and ETA, then return to the default page
            this.NavigationService.GoBack();
        }
    }
}
