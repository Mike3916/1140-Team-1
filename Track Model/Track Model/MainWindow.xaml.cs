using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace TrackModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //global var
        public TrackModelTestWindow testWindow = new TrackModelTestWindow();
        DispatcherTimer timer = new DispatcherTimer();

        public List<Line> mLines = new List<Line>(); //red will always be 0, green will always be 1
        public List<DataTable> mLineData = new List<DataTable>();
        public List<Train> mtrainList = new List<Train>();

        int mnumLines;
        int mlineIdx, msectIdx, mblockIdx;
        static int interval = 0;
        string mfileName;

        //bool traingo = false;
        //int mtrainPos = 0;
        //int mauth;
        //double mspeed;
        //int mdest;
        //static int pos = 0;
        //int trainLine = 0,
        //    trainSect = 0,
        //    trainBlock = 0;

        public bool actualClose = false;

        int[] mredRoute = {77, 9, 8, 7, 6, 5, 4, 3, 2, 1, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 76, 75,
                                74, 73, 72, 33, 34, 35, 36, 37, 38, 71, 70, 69, 68, 67, 44, 45, 46, 47, 48, 49, 50, 51,
                                52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 52, 51, 50, 49, 48, 47, 46,
                                45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24,
                                23, 22, 21, 20, 19, 18, 17, 16, 1, 2, 3, 4, 5, 6, 7, 8, 9, 77 };

        int[] mgreenRoute = {151, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
                                82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 85, 84,
                                83, 82, 81, 80, 79, 78, 77, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111,
                                112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128,
                                129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145,
                                146, 147, 148, 149, 150, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15,
                                14, 13, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21,
                                22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42,
                                43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 151 };

        public struct Train
        {
            public int blockIdx;
            public int lineIdx;
            public int commAuthority;
            
            public Train()
            {
                blockIdx = 0;
                lineIdx = 0;
                commAuthority = 0;
            }
            public Train(int bidx, int lidx, int auth)
            {
                blockIdx = bidx;
                lineIdx = lidx;
                commAuthority = auth;
            }
        }


        public MainWindow()
        {
            mnumLines = 0;

            InitializeComponent();

            Application.Current.MainWindow = this;
            
            LineDataGrid.IsReadOnly = true;
            ResetBlockInfo();
            ToggleBlockInfo();

            
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += dispatcherTimer_Tick;
            //timer.Start();
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

                if (testWindow.traingo)
                {
                    for (int i = 0; i < mtrainList.Count; i++)
                        UpdateTrain(i);
                }

                interval = 0;
            }
        }
        public void UpdateTick(int mult)
        {
            for (int i = 0; i < mult; i++)
            {
                //update train
                interval++;

                if (interval >= 5000)
                {
                    if (testWindow.traingo)
                    {
                        for (int j = 0; j < mtrainList.Count; j++)
                            UpdateTrain(j);
                    }
                    if (mblockIdx != -1)
                    {
                        double currentTemp = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].getmtrackTemp();
                        if (currentTemp < 32)
                        {
                            mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackTemp(currentTemp + 1);
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
                    }
                    interval = 0;
                }
            }
        }

        public List<TrackModel.Line> GetTrackData()
        {
            if (mnumLines != 2)
                return null;
            return mLines;
        }

        public List<int> OccupiedBlocks(int idx)
        {
            List<int> occblocks = new List<int>();
            for (int i = 0; i < mLines[idx].GetmnumSections(); i++)
            {
                for (int j = 0; j < mLines[idx].mSections[i].getmnumBlocks(); j++)
                {
                    if (mLines[idx].mSections[i].mBlocks[j].mOccupied == true)
                        occblocks.Add(1);
                    else
                        occblocks.Add(0);
                }
            }

            return occblocks;
        }

        public void SetPopulation(int pop, TrackModel.Block bl)
        {
            bl.mPop = pop;
            if (bl.mlineName == "Red")
                mLines[0].SetBlock(bl);
            else
                mLines[1].SetBlock(bl);
            SetBlockInfo();
        }

        public void SetSpeeds(int[] speeds, int lIdx)
        {
            for (int blocknum = 1; blocknum <= mLines[lIdx].GetmnumBlocks(); blocknum++)
            {
                TrackModel.Block newSpeed = mLines[lIdx].GetBlock(blocknum);
                newSpeed.mspeedLimit = speeds[blocknum-1];
                mLines[lIdx].SetBlock(newSpeed);
            }
            SetBlockInfo();
        }

        public void SetAuthorities(int[] auth, int lIdx)
        {
            for (int trIdx = 0; trIdx < mtrainList.Count; trIdx++)
            {
                Train tr = mtrainList[trIdx];
                for (int i = 0; i < auth.Length; i++)
                {
                    if (tr.blockIdx - 1 == i && tr.lineIdx == lIdx)
                        tr.commAuthority = auth[i];
                }
            }
            SetBlockInfo();
        }

        public void SetCrossings(int[] crossings, int lIdx)
        {
            for (int cr = 0; cr < crossings.Length; cr++)
            {
                if (cr == 1)
                {
                    TrackModel.Block bl = mLines[lIdx].GetBlock(cr+1);
                    bl.mcrossDown = true;
                    mLines[lIdx].SetBlock(bl);
                }
                else
                {
                    TrackModel.Block bl = mLines[lIdx].GetBlock(cr + 1);
                    bl.mcrossDown = false;
                    mLines[lIdx].SetBlock(bl);
                }
            }
            SetBlockInfo();
        }

        public void SetSwitches(int[] switches, int lIdx)
        {
            for (int sw = 0; sw < switches.Length; sw++)
            {
                TrackModel.Block bl = mLines[lIdx].GetBlock(sw + 1);
                bl.setmswitchPos(switches[sw]);
                mLines[lIdx].SetBlock(bl);
            }
            SetBlockInfo();
        }
        

        private DataTable MakeLineDataTable(int lineIdx)
        {
            List<string[]> newlineInfo = mLines[lineIdx].GetlineInfo();

            DataTable lineData = new DataTable(mLines[lineIdx].GetmnameLine());

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
            lineData.Columns.Add("Occupied");

            List<DataRow> blockData = new List<DataRow>(); //List of DataRow entries

            //foreach block entry, put data into a DataRow List entry and then add that to the lineData table
            for (int blockIdx = 0; blockIdx < mLines[lineIdx].GetmnumBlocks(); blockIdx++)
            {
                blockData.Add(lineData.NewRow());
                for (int valueIdx = 0; valueIdx < 9; valueIdx++)
                {
                    blockData[blockIdx][valueIdx] = newlineInfo[blockIdx][valueIdx]; //add data to row
                }
                lineData.Rows.Add(blockData[blockIdx]); //add to table

                blockData[blockIdx][9] = mLines[lineIdx].GetBlock(blockIdx+1).mOccupied + "";  
            }

            return lineData;
        }

        //adds line to the mLines list
        private void AddLine(string[] lineInfo)
        {
            string lineName = (lineInfo[0].Split(','))[0]; //splits up just to get lineName
            Line newLine = new Line(lineName); 

            newLine.AddSections(lineInfo); //pawns off data processing to Line Class
            mLines.Add(newLine);
        }
        public bool AddTrain(int blockIdx, int lineIdx, int auth)
        {
            mtrainList.Add(new Train(blockIdx, lineIdx, auth));
            mLines[lineIdx].OccupyBlock(blockIdx);
            OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
            UpdateSelectRow(lineIdx, blockIdx);
            return true;
        }
        public bool AddTrain(int lineIdx, int auth)
        {
            int blockIdx = 1;
            if (lineIdx == 0)
                blockIdx = 77;
            else
                blockIdx = 151;

            mtrainList.Add(new Train(blockIdx, lineIdx, auth));
            mLines[lineIdx].OccupyBlock(blockIdx);
            OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
            UpdateSelectRow(lineIdx, blockIdx);
            return true;
        }
        public void RemoveTrain(int trainIDX)
        {
            mtrainList.RemoveAt(trainIDX);
        }
        public void RemoveTrainAt(int blockIdx, int lineIdx)
        {
            int idx = 0;
            foreach (Train tr in mtrainList)
            {
                if (tr.blockIdx == blockIdx)
                    break;
                idx++;
            }
            mtrainList.RemoveAt(idx);
        }

        //takes in 
        public TrackModel.Block UpdateTrain(int IDX)
        {
            Train tr = mtrainList[IDX];
            mLines[tr.lineIdx].UnOccupyBlock(tr.blockIdx);
            UpdateSelectRow(mlineIdx, tr.blockIdx);
            tr.blockIdx = NextBlock(tr);
            mLines[tr.lineIdx].OccupyBlock(tr.blockIdx);
            UpdateSelectRow(mlineIdx, tr.blockIdx);

            mtrainList[IDX] = tr;
            OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";

            return mLines[tr.lineIdx].GetBlock(tr.blockIdx);
        }

        private int NextBlock(Train tr)
        {
            int[] route;
            if (tr.lineIdx == 0)
                route = mredRoute;
            else
                route = mgreenRoute;
            for (int idx = 0; idx < route.Length; idx++)
            {
                if (route[idx] == tr.blockIdx)
                    return route[idx+1]; //returns the next blockIdx
            }
            return route.Length; //returns the end of the route
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

        private void ReadOnly()
        {
            SpeedBox.IsReadOnly = true;
            LengthBox.IsReadOnly = true;
            GradeBox.IsReadOnly = true;
            ElevationBox.IsReadOnly = true;
            HeatBox.IsReadOnly = true;
        }
        //places blockInfo into textboxes and makes them writeable
        private void SetBlockInfo()
        {
            if (mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied)
                ReadOnly();
            else
            {
                SpeedBox.IsReadOnly = false;
                LengthBox.IsReadOnly = false;
                GradeBox.IsReadOnly = false;
                ElevationBox.IsReadOnly = false;
                HeatBox.IsReadOnly = false;
            }

            StationName.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mstationName;
            OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
            InfrastructureBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mInfrastructure;
            SpeedBox.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mspeedLimit + "";
            LengthBox.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mLength + "";
            GradeBox.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mGrade + "";
            ElevationBox.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mElevation + "";

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

            SetMurphy();

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
            StationName.Text = "";

            ToggleCrossbar.Visibility = Visibility.Collapsed;
            Population.Visibility = Visibility.Collapsed;
            AddTrainButton.Visibility = Visibility.Collapsed;
        }

        private void UpdateCurrentRow()
        {
            mLineData[mlineIdx].Select()[mblockIdx].BeginEdit();
            string[] blockinfo = mLines[mlineIdx].GetBlockInfo(msectIdx, mblockIdx);

            //for (int valueIdx = 0; valueIdx < mLineData[mlineIdx].Columns.Count; valueIdx++)
            //    mLineData[mlineIdx].Select()[mblockIdx][valueIdx] = blockinfo[valueIdx];

            mLineData[mlineIdx].Select()[mblockIdx][0] = blockinfo[0];
            mLineData[mlineIdx].Select()[mblockIdx][1] = blockinfo[1];
            mLineData[mlineIdx].Select()[mblockIdx][2] = blockinfo[2];
            mLineData[mlineIdx].Select()[mblockIdx][3] = blockinfo[3];
            mLineData[mlineIdx].Select()[mblockIdx][4] = blockinfo[4];
            mLineData[mlineIdx].Select()[mblockIdx][5] = blockinfo[5];
            mLineData[mlineIdx].Select()[mblockIdx][6] = blockinfo[6];
            mLineData[mlineIdx].Select()[mblockIdx][7] = blockinfo[7];
            mLineData[mlineIdx].Select()[mblockIdx][8] = blockinfo[8];
            mLineData[mlineIdx].Select()[mblockIdx][9] = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied;

            mLineData[mlineIdx].Select()[mblockIdx].EndEdit();
            mLineData[mlineIdx].Select()[mblockIdx].AcceptChanges();
        }
        public void UpdateSelectRow(int lIdx, int bIdx)
        {
            mLineData[lIdx].Select()[bIdx-1].BeginEdit();
            TrackModel.Block bl = mLines[lIdx].GetBlock(bIdx);
            string[] blockinfo = bl.mblockInfo;

            mLineData[lIdx].Select()[bIdx-1][1] = blockinfo[1];
            mLineData[lIdx].Select()[bIdx-1][0] = blockinfo[0];
            mLineData[lIdx].Select()[bIdx-1][2] = blockinfo[2];
            mLineData[lIdx].Select()[bIdx-1][3] = blockinfo[3];
            mLineData[lIdx].Select()[bIdx-1][4] = blockinfo[4];
            mLineData[lIdx].Select()[bIdx-1][5] = blockinfo[5];
            mLineData[lIdx].Select()[bIdx-1][6] = blockinfo[6];
            mLineData[lIdx].Select()[bIdx-1][7] = blockinfo[7];
            mLineData[lIdx].Select()[bIdx-1][8] = blockinfo[8];
            mLineData[lIdx].Select()[bIdx-1][9] = bl.mOccupied;
                                 
            mLineData[lIdx].Select()[bIdx-1].EndEdit();
            mLineData[lIdx].Select()[bIdx-1].AcceptChanges();
        }
        private void UpdateRows()
        {
            for (int bIdx = 0; bIdx < mLines[mlineIdx].GetmnumBlocks(); bIdx++)
            {
                UpdateSelectRow(mlineIdx, bIdx);
            }
        }
        
        public void SetMurphy()
        {
            if (mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mtrackCircuit == true)
            {
                FixCircuitButton.Background = Brushes.Green;
                BreakCircuitButton.Background = Brushes.Gray;
            }
            else
            {
                BreakCircuitButton.Background = Brushes.Red;
                FixCircuitButton.Background = Brushes.Gray;
            }

            if (mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mtrackRail == true)
            {
                FixTrackButton.Background = Brushes.Green;
                BreakTrackButton.Background = Brushes.Gray;
            }
            else
            {
                BreakTrackButton.Background = Brushes.Red;
                FixTrackButton.Background = Brushes.Gray;
            }

            if (mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mPower == true)
            {
                FixPowerButton.Background = Brushes.Green;
                BreakPowerButton.Background = Brushes.Gray;
            }
            else
            {
                BreakPowerButton.Background = Brushes.Red;
                FixPowerButton.Background = Brushes.Gray;
            }
        }

        //private void sendTrain(int authority, double speed, int destination)
        //{
        //    mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied = true;
        //    OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
        //    traingo = true;
        //    mauth = authority;
        //    mspeed = speed;
        //    mdest = destination;
        //}

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
                LineCombo.Items.Add(mLines[mLines.Count - 1].GetmnameLine());

                mLines[mLines.Count - 1].SetBeacon();
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
                List<string> blockNumList = mLines[lineIdx].GetSectBlockNum(sectionIdx);

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
                    SetBlockInfo();

                    List<int> switchList = mLines[lineIdx].GetmblockSwitch(sectionIdx, blockIdx);
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
            List<string> sectNames = mLines[lineIdx].GetSectionNames();

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
                    mLines[mlineIdx].SetBlockInfo(msectIdx, mblockIdx, 0, info);
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
                    mLines[mlineIdx].SetBlockInfo(msectIdx, mblockIdx, 1, info);
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
                    mLines[mlineIdx].SetBlockInfo(msectIdx, mblockIdx, 2, info);
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
                    double currentElevation = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].GetmElevation();
                    double dif = Math.Abs(currentElevation - info);
                    if (currentElevation > info)
                        dif *= -1;
                    mLines[mlineIdx].SetBlockInfo(msectIdx, mblockIdx, 3, info);
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
                OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
                UpdateCurrentRow();
            }
        }

        private void FixTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            { 
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackRail(true);
                FixTrackButton.Background = Brushes.Green;
                BreakTrackButton.Background = Brushes.Gray;
                OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
                UpdateCurrentRow();
            }
        }

        private void FixPowerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            {
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmPower(true);
                FixPowerButton.Background = Brushes.Green;
                BreakPowerButton.Background = Brushes.Gray;
                OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
                UpdateCurrentRow();
            }
        }

        private void TrackModelClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
            //this.Visibility = Visibility.Visible;

        }

        private void BreakPowerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            {
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmPower(false);
                BreakPowerButton.Background = Brushes.Red;
                FixPowerButton.Background = Brushes.Gray;
                OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
                UpdateCurrentRow();
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
                OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
                UpdateCurrentRow();
            }
                
        }
        

        private void AddTrain_Click(object sender, RoutedEventArgs e)
        {
            AddTrain(mblockIdx+1, mlineIdx, 0);
        }

        private void ToggleCrossbar_Click(object sender, RoutedEventArgs e)
        {
            bool crossDown = !mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mcrossDown;
            mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mcrossDown = crossDown;

            if (crossDown)
                ToggleCrossbar.Content = "Down";
            else
                ToggleCrossbar.Content = "Up";

        }

        private void BreakCircuitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(mblockIdx == -1) && mLines.Count != 0)
            {
                mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].setmtrackCircuit(false);
                BreakCircuitButton.Background = Brushes.Red;
                FixCircuitButton.Background = Brushes.Gray;
                OccupiedBlock.Text = mLines[mlineIdx].mSections[msectIdx].mBlocks[mblockIdx].mOccupied + "";
                UpdateCurrentRow();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!actualClose)
            {
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
                testWindow.WindowState = WindowState.Minimized;
            }
        }


    }
}
