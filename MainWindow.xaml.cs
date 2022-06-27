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
        private void UploadCSV_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(System.IO.Path.GetExtension(file));
            if (File.Exists(mfileName) == true && (System.IO.Path.GetExtension(mfileName) == ".csv"))
            {
                string[] file = System.IO.File.ReadAllLines(mfileName);

                string header = file[0];
                string[] lineInfo = new string[file.Length - 1];

                for (int i = 0; i < file.Length - 1; i++)
                    lineInfo[i] = file[i + 1];

                AddLine(lineInfo);

                DataTable lineData = MakeLineDataTable(mLines.Count - 1);

                mLineData.Add(lineData);

                LineDataGrid.ItemsSource = mLineData[mLineData.Count - 1].DefaultView;

            }
            LineCombo.Items.Add(mLines[mLines.Count - 1].getmnameLine());
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.T)
            {
                TrackModelTestWindow testWindow = new TrackModelTestWindow();
                testWindow.Show();
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mfileName = FileText.Text;
        }
        private DataTable MakeLineDataTable(int lineIdx)
        {
            List<string[]> newlineInfo = mLines[lineIdx].getlineInfo();

            DataTable lineData = new DataTable(mLines[lineIdx].getmnameLine());

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

            List<DataRow> blockData = new List<DataRow>();

            for (int blockIdx = 0; blockIdx < mLines[lineIdx].getmnumBlocks(); blockIdx++)
            {
                blockData.Add(lineData.NewRow());
                for (int valueIdx = 0; valueIdx < 10; valueIdx++)
                {
                    blockData[blockIdx][valueIdx] = newlineInfo[blockIdx][valueIdx];
                }
                lineData.Rows.Add(blockData[blockIdx]);
            }


            return lineData;
        }

        private void AddLine(string[] lineInfo)
        {
            string lineName = (lineInfo[0].Split(','))[0];
            Line newLine = new Line(lineName);

            newLine.addSection(lineInfo);
            mLines.Add(newLine);
        }


        /// Line Change Event to add Sections of selected Line
        private void LineCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SectionCombo.Items.Clear();

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
            BlockCombo.Items.Clear(); //removes previous section's block information

            //Idx assumes lines,sections are in the same order as in the list

        }
        private void BlockCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToggleBlockInfo();

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

        private void ToggleBlockInfo()
        {
            SpeedBox.IsReadOnly = !SpeedBox.IsReadOnly;
            LengthBox.IsReadOnly = !LengthBox.IsReadOnly;
            GradeBox.IsReadOnly = !GradeBox.IsReadOnly;
            ElevationBox.IsReadOnly = !ElevationBox.IsReadOnly;
            HeatBox.IsReadOnly = !HeatBox.IsReadOnly;
        }

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



        int mnumLines;
        string mfileName;
        List<Line> mLines = new List<Line>();
        List<DataTable> mLineData = new List<DataTable>();

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
            }
        }

        private void FIleText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UploadCSV_Click(sender, e);
            }
        }

        private void FileText_Click(object sender, MouseButtonEventArgs e)
        {
           (sender as TextBox).SelectAll();
           FileText.SelectAll();
        }

        private void TextBoc_Focus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txt = (sender as TextBox);
            txt.Focus();
            txt.SelectAll();
            if (txt != null)
                txt.SelectAll();
        }
    }
}
