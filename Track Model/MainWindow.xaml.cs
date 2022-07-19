using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace TrackModel_v0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //global var
        TrackModelTestWindow testWindow = new TrackModelTestWindow();
        DispatcherTimer timer = new DispatcherTimer();

        List<Line> mLines = new List<Line>();
        List<DataTable> mLineData = new List<DataTable>();

        int mnumLines;
        int mlineIdx, msectIdx, mblockIdx;
        static int interval = 0;
        string mfileName;

        bool traingo = false;
        int mtrainPos = 0;
        int mauth;
        double mspeed;
        int mdest;

        int trainLine = 0,
            trainSect = 0,
            trainBlock = 0;

        public MainWindow()
        {
            mnumLines = 0;

            InitializeComponent();

            LineDataGrid.IsReadOnly = true;
            ResetBlockInfo();
            ToggleBlockInfo();

            
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += dispatcherTimer_Tick;
            timer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            interval++;
            if (BlockCombo.SelectedIndex == -1)
                return;
            if (interval >= 5)
            {
                double currentTemp = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].getmtrackTemp();
                if (currentTemp < 32)
                {
                    mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackTemp(currentTemp+1);
                    HeatBox.Text = (currentTemp + 1).ToString();
                }
                else if (currentTemp == 32)
                {
                    //do nothing 
                }
                else
                {
                    mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackTemp(currentTemp - 1);
                    HeatBox.Text = (currentTemp - 1).ToString();
                }
                interval = 0;
                if (traingo == true)
                {
                    if (mdest == 0 && mtrainPos == 5)
                    {
                        trainSect = 1;
                        trainBlock = 0;
                    }
                    else if (mdest == 1 && mtrainPos == 5)
                    {
                        trainSect = 2;
                        mtrainPos = 0;
                    }


                    mLines[trainLine].mSections[trainSect].mBlocks[mtrainPos].mOccupied = false;
                    mLines[mlineIdx].mSections[msectIdx].mBlocks[mtrainPos].mOccupied = true;
                    OccupiedBlock.Text = mLines[trainLine].mSections[trainSect].mBlocks[mblockIdx].mOccupied + "";
                }

            }
            if (testWindow.traingo == true)
                sendTrain(testWindow.authority, testWindow.speed, testWindow.destination);
        }



        private DataTable MakeLineDataTable(int lineIdx)
        {
            List<string[]> newlineInfo = mLines[lineIdx].getlineInfo();

            DataTable lineData = new DataTable(mLines[lineIdx].getmnameLine());

            //adds all columms
            lineData.Columns.Add("Line");
            lineData.Columns.Add("Section");
            lineData.Columns.Add("Block Number");
            lineData.Columns.Add("Block Length");
            lineData.Columns.Add("Block Grade");
            lineData.Columns.Add("Speed Limit");
            lineData.Columns.Add("Infrastructure");
            lineData.Columns.Add("Station Side");
            lineData.Columns.Add("Elevation");

            List<DataRow> blockData = new List<DataRow>(); //List of DataRow entries

            //foreach block entry, put data into a DataRow List entry and then add that to the lineData table
            for (int blockIdx = 0; blockIdx < mLines[lineIdx].getmnumBlocks(); blockIdx++)
            {
                blockData.Add(lineData.NewRow());
                for (int valueIdx = 0; valueIdx < 9; valueIdx++)
                {
                    blockData[blockIdx][valueIdx] = newlineInfo[blockIdx][valueIdx]; //add data to row
                }
                lineData.Rows.Add(blockData[blockIdx]); //add to table
            }

            return lineData;
        }

        //adds line to the mLines list
        private void AddLine(string[] lineInfo)
        {
            string lineName = (lineInfo[0].Split(','))[0]; //splits up just to get lineName
            Line newLine = new Line(lineName); 

            newLine.addSections(lineInfo); //pawns off data processing to Line Class
            mLines.Add(newLine);
        }

        //flips readonly state of textboxes
        private void ToggleBlockInfo()
        {
            SpeedBox.IsReadOnly = !SpeedBox.IsReadOnly;
            LengthBox.IsReadOnly = !LengthBox.IsReadOnly;
            GradeBox.IsReadOnly = !GradeBox.IsReadOnly;
            ElevationBox.IsReadOnly = !ElevationBox.IsReadOnly;
            HeatBox.IsReadOnly = !HeatBox.IsReadOnly;
        }

        //places blockInfo into textboxes and makes them writeable
        private void SetBlockInfo(string[] blockInfo)
        {
            SpeedBox.IsReadOnly = false;
            LengthBox.IsReadOnly = false;
            GradeBox.IsReadOnly = false;
            ElevationBox.IsReadOnly = false;
            HeatBox.IsReadOnly = false;

            StationName.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mstationName;
            OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
            InfrastructureBlock.Text = blockInfo[6];
            SpeedBox.Text = blockInfo[5];
            LengthBox.Text = blockInfo[3];
            GradeBox.Text = blockInfo[4];
            ElevationBox.Text = blockInfo[8];

            if (mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mhasCross)
            {
                ToggleCrossbar.Visibility = Visibility.Visible;
                if (mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mcrossDown)
                    ToggleCrossbar.Content = "Down";
                else
                    ToggleCrossbar.Content = "Up";
            }
            else
                ToggleCrossbar.Visibility = Visibility.Collapsed;

            if (mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mPop > 0)
            {
                Population.Visibility = Visibility.Visible;
                Population.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mPop + "";
            }
            else
                Population.Visibility = Visibility.Collapsed;

        }
        private void ResetBlockInfo()
        {
            OccupiedBlock.Text = "N/A";
            InfrastructureBlock.Text = "N/A";
            SpeedBox.Text = "N/A";
            LengthBox.Text = "N/A";
            GradeBox.Text = "N/A";
            ElevationBox.Text = "N/A";
            HeatBox.Text = "32";

            ToggleCrossbar.Visibility = Visibility.Collapsed;
            Population.Visibility = Visibility.Collapsed;
        }

        private void UpdateCurrentRow()
        {
            mLineData[mlineIdx].Select()[mblockIdx].BeginEdit();
            string[] blockinfo = mLines[mlineIdx].getBlockInfo(msectIdx, mblockIdx);

            for (int valueIdx = 0; valueIdx < mLineData[mlineIdx].Columns.Count; valueIdx++)
                mLineData[mlineIdx].Select()[mblockIdx][valueIdx] = blockinfo[valueIdx];

            mLineData[mlineIdx].Select()[mblockIdx].EndEdit();
            mLineData[mlineIdx].Select()[mblockIdx].AcceptChanges();
        }

        private void sendTrain(int authority, double speed, int destination)
        {
            mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied = true;
            OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
            traingo = true;
            mauth = authority;
            mspeed = speed;
            mdest = destination;
        }

        //events

        //looks for a 't' keystroke to open Test Window
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.T)
            {
                testWindow.Show();
            }
        }

        //processes CSV file into DataTable obj and stores it into the mLines list
        private void UploadCSV_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(mfileName) == true && (System.IO.Path.GetExtension(mfileName) == ".csv")) //checks if file exists and .CSV
            {
                string[] file = System.IO.File.ReadAllLines(mfileName); //each block entry in a string

                string[] lineInfo = new string[file.Length - 1]; //creates string[] of csv data discluding the header

                for (int i = 0; i < file.Length - 1; i++)
                    lineInfo[i] = file[i + 1];

                AddLine(lineInfo);

                DataTable lineData = MakeLineDataTable(mLines.Count - 1);

                mLineData.Add(lineData);

                LineDataGrid.ItemsSource = mLineData[mLineData.Count - 1].DefaultView;
                LineCombo.Items.Add(mLines[mLines.Count - 1].getmnameLine());
            }
            else
                MessageBox.Show("File not found :(");
        }

        private void TextBoc_Focus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txt = (sender as TextBox);
            txt.Focus();
            txt.SelectAll();
            if (txt != null)
                txt.SelectAll();
        }

        private void FileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mfileName = FileText.Text;
        }

        private void FileText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UploadCSV_Click(sender, e);
            }
        }

        ///Event when browse button is clicked will launch an openfile dialog
        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog.InitialDirectory = @"c:\";
            if (openFileDialog.ShowDialog() == true)//waits for "okay" to be pressed 
            {
                mfileName = openFileDialog.FileName; //updates mfileName
                FileText.Text = mfileName; //updates textbox
            }
        }

        private void SectionCombo_Close(object sender, EventArgs e)
        {
            if (SectionCombo.Items.Count > 0)
            {
                int lineIdx = LineCombo.SelectedIndex;
                int sectionIdx = SectionCombo.SelectedIndex;
                List<string> blockNumList = mLines[lineIdx].getSectBlockNum(sectionIdx);

                foreach (string blockNum in blockNumList)
                    BlockCombo.Items.Add(blockNum);
            }
        }

        private void BlockCombo_Close(object sender, EventArgs e)
        {
            if (BlockCombo.Items.Count > 0)
            {
                if (BlockCombo.SelectedIndex != -1)
                {
                    int lineIdx = LineCombo.SelectedIndex;
                    int sectionIdx = SectionCombo.SelectedIndex;
                    int blockIdx = BlockCombo.SelectedIndex;
                    string[] blockInfo = mLines[lineIdx].mSections[sectionIdx].mBlocks[blockIdx].mblockInfo;
                    SetBlockInfo(blockInfo);

                    List<int> switchList = mLines[lineIdx].getmblockSwitch(sectionIdx, blockIdx);
                    foreach (int sw in switchList)
                        SwitchCombo.Items.Add(sw);
                    SwitchCombo.SelectedIndex = 0;
                }
            }
        }

        /// Line Change Event to add Sections of selected Line
        private void LineCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SectionCombo.Items.Clear();
            mlineIdx = LineCombo.SelectedIndex;

            int lineIdx = LineCombo.SelectedIndex;
            LineDataGrid.ItemsSource = mLineData[lineIdx].DefaultView;
            List<string> sectNames = mLines[lineIdx].getSectionNames();

            foreach (string name in sectNames)
                SectionCombo.Items.Add(name);
        }

        /// Section Change Event to add Blocks of selected Section
        private void SectionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SectionCombo.SelectedItem == null)
            {
                ResetBlockInfo();
            }
            msectIdx = SectionCombo.SelectedIndex;

            SwitchCombo.Items.Clear();                  //removes previous block's switch information
            BlockCombo.Items.Clear();                   //removes previous section's block information

            //Idx assumes lines,sections are in the same order as in the list
        }
        private void BlockCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToggleBlockInfo();
            mblockIdx = BlockCombo.SelectedIndex;
            SwitchCombo.Items.Clear();
        }

        //param = 0
        private void LengthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LengthBox.IsFocused == true)
            {
                if (double.TryParse(LengthBox.Text, out double info) == true)
                {
                    mLines[mlineIdx].setBlockInfo(msectIdx, mblockIdx, 0, info);
                    UpdateCurrentRow();
                }
                else
                    MessageBox.Show("Only numbers PLEASE");
            }
        }

        //param = 1
        private void GradeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (GradeBox.IsFocused == true)
            {
                if (double.TryParse(GradeBox.Text, out double info) == true)
                {
                    mLines[mlineIdx].setBlockInfo(msectIdx, mblockIdx, 1, info);
                    UpdateCurrentRow();
                }
                else
                    MessageBox.Show("Only numbers PLEASE");
            }
        }

        //param = 2
        private void SpeedBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SpeedBox.IsFocused == true)
            {
                if (double.TryParse(SpeedBox.Text, out double info) == true)
                {
                    mLines[mlineIdx].setBlockInfo(msectIdx, mblockIdx, 2, info);
                    UpdateCurrentRow();
                }
                else
                    MessageBox.Show("Only numbers PLEASE");
            }
        }

        //param = 3
        private void ElevationBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ElevationBox.IsFocused == true)
            {
                if (double.TryParse(ElevationBox.Text, out double info) == true)
                {
                    double currentElevation = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].getmElevation();
                    double dif = Math.Abs(currentElevation - info);
                    if (currentElevation > info)
                        dif *= -1;
                    mLines[mlineIdx].setBlockInfo(msectIdx, mblockIdx, 3, info);
                    UpdateCurrentRow();              
                }
                else
                    MessageBox.Show("Only numbers PLEASE");
            }
        }        

        private void BreakTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            {
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackRail(false);
                BreakTrackButton.Background = Brushes.Red;
                FixTrackButton.Background = Brushes.Gray;
            }
        }

        private void FixTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            { 
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackRail(true);
                FixTrackButton.Background = Brushes.Green;
                BreakTrackButton.Background = Brushes.Gray;
            }
        }

        private void FixPowerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            {
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmPower(true);
                FixPowerButton.Background = Brushes.Green;
                BreakPowerButton.Background = Brushes.Gray;
            }
        }

        private void BreakPowerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            {
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmPower(false);
                BreakPowerButton.Background = Brushes.Red;
                FixPowerButton.Background = Brushes.Gray;
            }
                
        }

        private void HeatBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (mLines.Count == 0)
                return;

            if (HeatBox.IsFocused == true)
            {
                if (double.TryParse(HeatBox.Text, out double info) == true)
                {
                    mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackTemp(Convert.ToDouble(HeatBox.Text));
                }
                else
                    MessageBox.Show("Only numbers PLEASE");
            }
        }

        private void FixCircuitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            {
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackCircuit(true);
                FixCircuitButton.Background = Brushes.Green;
                BreakCircuitButton.Background = Brushes.Gray;
            }
                
        }
        
        private void BreakCircuitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            {
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackCircuit(false);
                BreakCircuitButton.Background = Brushes.Red;
                FixCircuitButton.Background = Brushes.Gray;
            }
        }

       
    }
}
