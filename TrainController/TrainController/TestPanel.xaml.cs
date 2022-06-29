﻿using System;
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
using System.Windows.Shapes;

namespace TrainController
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TestPanel : Window
    {
        public TestPanel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == EmergencyBrakes)
            {
                /*if ()
                {

                }
                else
                {

                }*/
            }
            else if (sender == LeftDoors)
            {
                if (!((MainWindow)Application.Current.MainWindow).mLeftDoorsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).mLeftDoorsStatus = true;
                    ((MainWindow)Application.Current.MainWindow).LeftDoors.Content = "Doors - Left\n    (OPEN)";
                    LeftDoors.Content = "OPEN";
                    LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).mLeftDoorsStatus = false;
                    ((MainWindow)Application.Current.MainWindow).LeftDoors.Content = "Doors - Left\n   (CLOSED)";
                    LeftDoors.Content = "CLOSED";
                    LeftDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }
            else if (sender == RightDoors)
            {
                if (!((MainWindow)Application.Current.MainWindow).mRightDoorsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).mRightDoorsStatus = true;
                    ((MainWindow)Application.Current.MainWindow).RightDoors.Content = "Doors - Right\n     (OPEN)";
                    RightDoors.Content = "OPEN";
                    RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).mRightDoorsStatus = false;
                    ((MainWindow)Application.Current.MainWindow).RightDoors.Content = "Doors - Right\n   (CLOSED)";
                    RightDoors.Content = "CLOSED";
                    RightDoors.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }
            else if (sender == InteriorLights)
            {
                if (!((MainWindow)Application.Current.MainWindow).mInteriorLightsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).mInteriorLightsStatus = true;
                    ((MainWindow)Application.Current.MainWindow).InteriorLights.Content = "Lights - Interior\n        (ON)";
                    InteriorLights.Content = "ON";
                    InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).mInteriorLightsStatus = false;
                    ((MainWindow)Application.Current.MainWindow).InteriorLights.Content = "Lights - Interior\n        (OFF)";
                    InteriorLights.Content = "OFF";
                    InteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }
            else if (sender == ExteriorLights)
            {
                if (!((MainWindow)Application.Current.MainWindow).mExteriorLightsStatus)
                {
                    ((MainWindow)Application.Current.MainWindow).mExteriorLightsStatus = true;
                    ((MainWindow)Application.Current.MainWindow).ExteriorLights.Content = "Lights - Exterior\n        (ON)";
                    ExteriorLights.Content = "ON";
                    ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70D060"));
                }
                else
                {
                    ((MainWindow)Application.Current.MainWindow).mExteriorLightsStatus = false;
                    ((MainWindow)Application.Current.MainWindow).ExteriorLights.Content = "Lights - Exterior\n        (OFF)";
                    ExteriorLights.Content = "OFF";
                    ExteriorLights.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF5050"));
                }
            }
        }

        private void KeyDownButton(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == Temperature)
                {
                    ((MainWindow)Application.Current.MainWindow).mTemperature = int.Parse(Temperature.Text);
                    ((MainWindow)Application.Current.MainWindow).Temperature.Text = "Temperature: " + ((MainWindow)Application.Current.MainWindow).mTemperature.ToString() + "°F";
                }
                else if (sender == SetKp)
                {
                    ((MainWindow)Application.Current.MainWindow).mKp = int.Parse(SetKp.Text);
                }
                else if (sender == SetKi)
                {
                    ((MainWindow)Application.Current.MainWindow).mKi = int.Parse(SetKi.Text);
                }
            }
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
