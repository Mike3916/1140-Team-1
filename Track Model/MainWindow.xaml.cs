using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TrackModel_v0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            mnumLines = 0;

            InitializeComponent();

            LineDataGrid.IsReadOnly = true;
            ResetBlockInfo();
            ToggleBlockInfo();
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
            lineData.Columns.Add("Cumalative Elevation");

            List<DataRow> blockData = new List<DataRow>(); //List of DataRow entries

            //foreach block entry, put data into a DataRow List entry and then add that to the lineData table
            for (int blockIdx = 0; blockIdx < mLines[lineIdx].getmnumBlocks(); blockIdx++)
            {
                blockData.Add(lineData.NewRow());
                for (int valueIdx = 0; valueIdx < 10; valueIdx++)
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

            InfrastructureBlock.Text = blockInfo[6];
            SpeedBox.Text = blockInfo[5];
            LengthBox.Text = blockInfo[3];
            GradeBox.Text = blockInfo[4];
            ElevationBox.Text = blockInfo[8];
            CumElevationBlock.Text = blockInfo[9];

        }
        private void ResetBlockInfo()
        {
            OccupiedBlock.Text = "N/A";
            InfrastructureBlock.Text = "N/A";
            SpeedBox.Text = "N/A";
            LengthBox.Text = "N/A";
            GradeBox.Text = "N/A";
            ElevationBox.Text = "N/A";
            CumElevationBlock.Text = "N/A";
            HeatBox.Text = "N/A";
        }

        //events

        //looks for a 't' keystroke to open Test Window
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.T)
            {
                TrackModelTestWindow testWindow = new TrackModelTestWindow();
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

            }
            LineCombo.Items.Add(mLines[mLines.Count - 1].getmnameLine());
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
                int lineIdx = LineCombo.SelectedIndex;
                int sectionIdx = SectionCombo.SelectedIndex;
                int blockIdx = BlockCombo.SelectedIndex;
                string[] blockInfo = mLines[lineIdx].getBlockInfo(sectionIdx, blockIdx);
                SetBlockInfo(blockInfo);

                List<int> switchList = mLines[lineIdx].getmblockSwitch(sectionIdx, blockIdx);
                foreach (int sw in switchList)
                    SwitchCombo.Items.Add(sw);
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



        //global var
        int mnumLines;
        int mlineIdx, msectIdx, mblockIdx;
        string mfileName;

        //param = 0
        private void LengthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LengthBox.IsFocused == true)
            {
                if (double.TryParse(LengthBox.Text, out double info) == true)
                    mLines[mlineIdx].setBlockInfo(msectIdx, mblockIdx, 0, info);
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
                    mLines[mlineIdx].setBlockInfo(msectIdx, mblockIdx, 1, info);
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
                    mLines[mlineIdx].setBlockInfo(msectIdx, mblockIdx, 2, info);
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
                    mLines[mlineIdx].setBlockInfo(msectIdx, mblockIdx, 3, info);
                else
                    MessageBox.Show("Only numbers PLEASE");
            }
        }

        

        List<Line> mLines = new List<Line>();
        List<DataTable> mLineData = new List<DataTable>();
    }
}
