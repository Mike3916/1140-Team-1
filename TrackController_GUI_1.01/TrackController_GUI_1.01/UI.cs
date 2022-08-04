using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;

namespace Track_Controller_1._02
{

    //Track_Controller_1._02
    public partial class Form_WC : Form
    {

        //Class variables list for the Test UI
        private OpenFileDialog mOFD;
        private string mFilePath="";
        List<Line> mLines = new List<Line>();
        List<Line> mWriteStream = new List<Line>();  
        List<DataTable> mLineData = new List<DataTable>();
        List<string> mSectNames = new List<string>();
        private int mCurrentLineIndex = -1;
        private int mCurrentSectionIndex = -1;
        private int mCurrentBlockIndex = -1;
        private int mCurrentAuthority = 0;
        private int mCurrentSuggestedSpeed = 0;
        private int mCurrentPLCBlockIndex = 0;
        List<Controller> mPLCs = new List<Controller>();
        List<int> mControllerBlocks = new List<int>();
        List<int> mControllerSections = new List<int>();
        private int mControllerLine = -1;
        private Track_Controller_1._02.Controller mRedline1 = new Track_Controller_1._02.Controller(851, false, "127.0.0.1");
        private Track_Controller_1._02.Controller mRedline2 = new Track_Controller_1._02.Controller(853, false, "127.0.0.1");
        private Track_Controller_1._02.Controller mGreenLine1 = new Track_Controller_1._02.Controller(852, false, "127.0.0.1");

        private int[] mRedMaintenanceBlocks = new int[77];
        private int[] mRedOccupancies = new int[77];
        private int[] mRedSpeeds = new int[77];
        private int[] mRedAuthorities = new int[77];
        private int[] mRedCrossings = new int[77];
        private int[] mRedSwitches = new int[77];
        private int[] mRedLeftLights = new int[77];
        private int[] mRedRightLights = new int[77];

        private int[] mRed1MaintenanceBlocks = new int[44];
        private int[] mRed1Occupancies = new int[44];
        private int[] mRed1Speeds = new int[44];
        private int[] mRed1Authorities = new int[44];
        private int[] mRed1Crossings = new int[44];
        private int[] mRed1Switches = new int[44];
        private int[] mRed1LeftLights = new int[44];
        private int[] mRed1RightLights = new int[44];
        private int[] mRed2MaintenanceBlocks = new int[41];
        private int[] mRed2Occupancies = new int[41];
        private int[] mRed2Speeds = new int[41];
        private int[] mRed2Authorities = new int[41];
        private int[] mRed2Crossings = new int[41];
        private int[] mRed2Switches = new int[41];
        private int[] mRed2LeftLights = new int[41];
        private int[] mRed2RightLights = new int[41];

        private int[] mGreenMaintenanceBlocks = new int[151];
        private int[] mGreenOccupancies = new int[151];
        private int[] mGreenSpeeds = new int[151];
        private int[] mGreenAuthorities = new int[151];
        private int[] mGreenCrossings = new int[151];
        private int[] mGreenSwitches = new int[151];
        private int[] mGreenLeftLights = new int[151];
        private int[] mGreenRightLights = new int[151];


        //Form_WC(): Instantiates new test UI.
        public Form_WC()
        {
            InitializeComponent();
        }

        //****************************************************************************************************************************************
        //Form_WC_Load: Loads the Wayside Controller UI with all form controls
        //<sender>: reference to object that raises the form load event in this case "Track_Controller_<version #>.main()".
        //<e>: Argument containing event data.
        //<void>
        private void Form_WC_Load(object sender, EventArgs e)
        {
            mOFD = new OpenFileDialog();

        }

 


        //****************************************************************************************************************************************
        //Lock_Local_Click: Toggles the class variable private mLocalLock which enables/disables the CTC office ability to put blocks in and
        //out of service and enables the test window to toggle block states.
        //<sender>: reference to object that raises the form load event in this case "Form_WC.Toggle_TestMode()".
        //<e>: Argument containing event data.
        //<void>
        private void mButtonLockLocal_Click(object sender, EventArgs e)
        {
            int localState = 0;
            try
            {
                mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
                localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            }
            catch
            {
                MessageBox.Show("Please select a valid block.");
                return;
            }

            if (this.mButtonLockLocal.BackColor == Color.FromArgb(0,192,0))
            {
                this.mButtonLockLocal.Image = Track_Controller_1._02.Properties.Resources.Lock_Closed;
                this.mButtonLockLocal.BackColor = Color.Red;
                this.mButton_Toggle_Track_Occupancy.Enabled = true;
                this.mButton_Toggle_Track_Occupancy.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Lights_Left.Enabled = true;
                this.mButton_Toggle_Lights_Left.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Lights_Right.Enabled = true;
                this.mButton_Toggle_Lights_Right.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Switch.Enabled = true;
                this.mButton_Toggle_Switch.BackColor = SystemColors.ButtonFace;
                this.button3.Enabled = true;
                this.button3.BackColor = SystemColors.ButtonFace;
                this.mButton_SetSuggestedSpeed.Enabled = true;
                this.mButton_SetSuggestedSpeed.BackColor = SystemColors.ButtonFace;
                this.mText_SuggestedSpeed.Enabled = true;
                this.mTextBox_Authority.Enabled = true;
                this.mButton_SetAuthority.Enabled = true;
                this.mButton_SetAuthority.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Bar.Enabled = true;
                this.mButton_Toggle_Bar.BackColor = SystemColors.ButtonFace;

                localState = localState | 0x00004000;

                if(mCurrentLineIndex == 0)
                {
                    mRedMaintenanceBlocks[mCurrentPLCBlockIndex] = 1;
                    SplitArrays();
                    mRedline1.SendMaintenance(mRed1MaintenanceBlocks);
                    mRedline2.SendMaintenance(mRed2MaintenanceBlocks);
                }
                else
                {
                    mGreenMaintenanceBlocks[mCurrentPLCBlockIndex] = 1;
                    SplitArrays();
                    mGreenLine1.SendMaintenance(mGreenMaintenanceBlocks);
                }

            }
            else
            {
                this.mButtonLockLocal.Image = Track_Controller_1._02.Properties.Resources.Lock_Open;
                this.mButtonLockLocal.BackColor = Color.FromArgb(0, 192, 0);
                this.mButton_Toggle_Track_Occupancy.Enabled = false;
                this.mButton_Toggle_Track_Occupancy.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Lights_Left.Enabled = false;
                this.mButton_Toggle_Lights_Left.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Lights_Right.Enabled = false;
                this.mButton_Toggle_Lights_Right.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Switch.Enabled = false;
                this.mButton_Toggle_Switch.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Bar.Enabled = false;
                this.mButton_Toggle_Bar.BackColor = SystemColors.ControlDark;
                this.button3.Enabled = false;
                this.button3.BackColor = SystemColors.ControlDark;
                this.mButton_SetSuggestedSpeed.Enabled = false;
                this.mButton_SetSuggestedSpeed.BackColor = SystemColors.ControlDark;
                this.mText_SuggestedSpeed.Enabled = false;
                this.mTextBox_Authority.Enabled = false;
                this.mButton_SetAuthority.Enabled = false;
                this.mButton_SetAuthority.BackColor = SystemColors.ControlDark;

                localState = localState & 0x0FFFBFFF;

                if (mCurrentLineIndex == 0)
                {
                    mRedMaintenanceBlocks[mCurrentPLCBlockIndex] = 0;
                    SplitArrays();
                    mRedline1.SendMaintenance(mRed1MaintenanceBlocks);
                    mRedline2.SendMaintenance(mRed2MaintenanceBlocks);
                }
                else
                {
                    mGreenMaintenanceBlocks[mCurrentPLCBlockIndex] = 0;
                    SplitArrays();
                    mGreenLine1.SendMaintenance(mGreenMaintenanceBlocks);
                }
            }

            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
        }


        //mButton_Controller_Connect_Click: Creates a new controller class object of type software so ADS connection can be tested. 
        //<sender>: reference to object that raises the form load event in this case "Form_WC.Toggle_TestMode()".
        //<e>: Argument containing event data.
        //<void>
        private void mButton_Controller_Connect_Click(object sender, EventArgs e)
        {
            int[] routes = new int[45];
            int[] occupancies = new int[45];
            int length = 45;

            for(int i=0; i<45; i++)
            {
                occupancies[i] = 1;
                routes[i] = i;
            }

            Controller mRedLine1 = new Controller(851, false, "127.0.0.1");

            mRedLine1.SendRoute(routes);
            routes = mRedLine1.ReceiveRoute(length);

            mRedLine1.SendOccupancies(occupancies);
            occupancies = mRedLine1.ReceiveOccupancies(length);

        }


        //***************************************************************************************************************************************
        //mButton_Open_CSV_Click: Handles the selection of a track configuration file in .csv format. The method opens a file dialogue, allows the 
        //user to select a file, verifies the file type, parses the file into lines, sections, and blocks, and updates the UI.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Open_CSV_Click(object sender, EventArgs e)
        {
            //Open file dialog
            mOFD.ShowDialog();
            mFilePath = mOFD.FileName;


            //Checks for valid file extension then extracts line data
            if (File.Exists(mFilePath) == true && (System.IO.Path.GetExtension(mFilePath) == ".csv"))
            {
                string[] file = System.IO.File.ReadAllLines(mFilePath);

                string header = file[0];
                string[] lineInfo = new string[file.Length - 1];

                for (int i = 0; i < file.Length - 1; i++)
                    lineInfo[i] = file[i + 1];

                AddLine(lineInfo);

                DataTable lineData = MakeLineDataTable(mLines.Count - 1);

                //adds line data to private variable
                mLineData.Add(lineData);
                mComboBox_Select_Line.Items.Add(mLines[mLines.Count - 1].getmnameLine());
                MessageBox.Show("Line added.");
            }
            
        }


        //*********************************************************************************************************************************************
        //AddLine: Handles the addition of a new line to this UI.
        //<[] lineinfo> holds each line of the .csv as string 
        private void AddLine(string[] lineInfo)
        {
            string lineName = (lineInfo[0].Split(','))[0];
            Line newLine = new Line(lineName);

            newLine.addSection(lineInfo);
            mLines.Add(newLine);
        }


        //**********************************************************************************************************************************************
        //MakeLineDataTable: Creates a data table for the line information uploaded by CSV
        //<lineIdx>: index of the line to make into a data table.
        //<DataTable>: returns the data table object
        private DataTable MakeLineDataTable(int lineIdx)
        {
            List<string[]> newLineInfo = mLines[lineIdx].getlineInfo();

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
                    blockData[blockIdx][valueIdx] = newLineInfo[blockIdx][valueIdx];
                }
                lineData.Rows.Add(blockData[blockIdx]);
            }


            return lineData;
        }

        //*********************************************************************************************************************************************
        //mComboBox_Section_SelectedIndexChanged: Updates the test UI when the selected section changes
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mComboBox_Section_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Empties Block Combo Box
            mComboBox_Select_Block.Items.Clear();
            mComboBox_Select_Block.Items.Add("Select Block to View");


            //sets combobox index
            mCurrentSectionIndex = mComboBox_Section.SelectedIndex - 1;
            mCurrentBlockIndex = -1;

            List<string> blockNumberz = new List<string>();

            blockNumberz = mLines[mCurrentLineIndex].getSectBlockNum(mCurrentSectionIndex);

            for(int i = 0; i < blockNumberz.Count; i++)
            {
                mComboBox_Select_Block.Items.Add(blockNumberz[i]);
            }



        }


        //***************************************************************************************************************************************
        //mComboBox_Select_Line_SelectedIndexChanged: Updates the section combobox when the selected line is changed.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mComboBox_Select_Line_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Resets combo box
            mComboBox_Section.Items.Clear();
            mComboBox_Section.Items.Add("Select Section");
            mComboBox_Select_Block.Items.Clear();
            mComboBox_Select_Block.Items.Add("Select Block to View");
            mCurrentLineIndex = -1;
            mControllerBlocks.Clear();
            mControllerSections.Clear();

            //sets combobox index?
            mCurrentLineIndex = mComboBox_Select_Line.SelectedIndex;
            mCurrentSectionIndex = -1;
            mCurrentBlockIndex = -1;

            //Retrieve number of sections and populate sections in the combobox on the UI
            mSectNames = mLines[mLines.Count() - 1].getSectionNames();

            for (int i = 0; i < mSectNames.Count(); i++)
            {
                mComboBox_Section.Items.Add(mSectNames[i]);
            }

        }

        //*****************************************************************************************************************************************
        //mComboBox_Select_Block_SelectedIndexChanged: Updates the view block panel with current block states
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mComboBox_Select_Block_SelectedIndexChanged(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            mCurrentPLCBlockIndex = Int32.Parse(mComboBox_Select_Block.Text) - 1;

            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mCurrentAuthority = mLines[mCurrentLineIndex].getSuggested(mCurrentSectionIndex, mCurrentBlockIndex);
            mCurrentSuggestedSpeed = mLines[mCurrentLineIndex].getAuthority(mCurrentSectionIndex, mCurrentBlockIndex);



            this.RefreshWindow(mLocalState);

        }



        //*******************************************************************************************************************************************
        //mButton_Toggle_Switch_Click Toggles the switch position for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Switch_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            localState = localState ^ 0x00000001;
            if(mCurrentLineIndex == 0)
            {
                mRedSwitches[mCurrentPLCBlockIndex] = 1;
                SplitArrays();
                try
                {
                    mRedline1.SendOccupancies(mRed1Occupancies);
                    mRedline2.SendOccupancies(mRed2Occupancies);
                }
                catch
                {
                    MessageBox.Show("Red line PLC not connected.");
                }
            }
            else
            {
                mGreenSwitches[mCurrentPLCBlockIndex] = 1;
                try
                {
                    mGreenLine1.SendOccupancies(mGreenOccupancies);
                }
                catch
                {
                    MessageBox.Show("Green line PLC not connected.");
                }
            }
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
            Thread.Sleep(10);
            this.RefreshWindow(localState);
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Track_Occupancy_Click toggles the track occupancy for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Track_Occupancy_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            localState = localState ^ 0x00001000;
            if(mCurrentLineIndex == 0)
            {
                mRedOccupancies[mCurrentPLCBlockIndex] = ((localState & 0x00001000) >> 12);
                SplitArrays();
                try
                {
                    mRedline1.SendOccupancies(mRed1Occupancies);
                    mRedline2.SendOccupancies(mRed2Occupancies);
                }
                catch
                {
                    MessageBox.Show("Red line PLC not connected.");
                }
            }
            else
            {
                mGreenOccupancies[mCurrentPLCBlockIndex] = ((localState & 0x00001000) >> 12);
                try
                {
                    mGreenLine1.SendOccupancies(mGreenOccupancies);
                }
                catch
                {
                    MessageBox.Show("Green line PLC not connected.");
                }
            }
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
            Thread.Sleep(10);
            this.RefreshWindow(localState);            
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Lights_Left_Click toggles the left lights for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Lights_Left_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            localState = localState ^ 0x00100000;
            if (mCurrentLineIndex == 0)
            {
                mRedOccupancies[mCurrentPLCBlockIndex] = ((localState & 0x00100000) >> 20);
                SplitArrays();
                try
                {
                    mRedline1.SendLeftLights(mRed1LeftLights);
                    mRedline2.SendLeftLights(mRed2LeftLights);
                }
                catch
                {
                    MessageBox.Show("Red line PLC not connected.");
                }
            }
            else
            {
                mGreenOccupancies[mCurrentPLCBlockIndex] = ((localState & 0x00100000) >> 20);
                try
                {
                    mGreenLine1.SendLeftLights(mGreenLeftLights);
                }
                catch
                {
                    MessageBox.Show("Green line PLC not connected.");
                }
            }
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
            Thread.Sleep(10);
            this.RefreshWindow(localState);
        }

        //*******************************************************************************************************************************************
        //SetAuthority sets the authority for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mLeft_Green_CheckedChanged(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            localState = localState | 0x00100000;
            localState = localState & 0x0FDFFFFF;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
            this.RefreshWindow(localState);
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Lights_Right_Click toggles the right side transit lights for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Lights_Right_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            localState = localState ^ 0x00010000;
            if (mCurrentLineIndex == 0)
            {
                mRedOccupancies[mCurrentBlockIndex] = ((localState & 0x00010000) >> 16);
                SplitArrays();
                try
                {
                    mRedline1.SendRightLights(mRed1RightLights);
                    mRedline2.SendRightLights(mRed2RightLights);
                }
                catch
                {
                    MessageBox.Show("Red line PLC not connected.");
                }
            }
            else
            {
                mGreenOccupancies[mCurrentBlockIndex] = ((localState & 0x00010000) >> 16);
                try
                {
                    mGreenLine1.SendRightLights(mGreenRightLights);
                }
                catch
                {
                    MessageBox.Show("Green line PLC not connected.");
                }
            }
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
            Thread.Sleep(10);
            this.RefreshWindow(localState);
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Signal_Click toggles the track signal for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Signal_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            localState = localState ^ 0x00000800;
            if (mCurrentLineIndex == 0)
            {
                mRedOccupancies[mCurrentBlockIndex] = ((localState & 0x00000800) >> 11);
                SplitArrays();
                try
                {
                    mRedline1.SendRightLights(mRed1RightLights);
                    mRedline2.SendRightLights(mRed2RightLights);
                }
                catch
                {
                    MessageBox.Show("Red line PLC not connected.");
                }
            }
            else
            {
                mGreenOccupancies[mCurrentBlockIndex] = ((localState & 0x00010000) >> 16);
                try
                {
                    mGreenLine1.SendRightLights(mGreenRightLights);
                }
                catch
                {
                    MessageBox.Show("Green line PLC not connected.");
                }
            }
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
            this.RefreshWindow(localState);
        }

        //*******************************************************************************************************************************************
        //SetAuthority toggles the crossing bar for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Bar_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            localState = localState ^ 0x00000020;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
            this.RefreshWindow(localState);
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Crossing_Lights_Click toggles the crossing lights for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Crossing_Lights_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            localState = localState ^ 0x00000010;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
            this.RefreshWindow(localState);
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Rail_Click toggles broken rail for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Rail_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int localState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            localState = localState ^ 0x00000400;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, localState);
            this.RefreshWindow(localState);
        }

        //*******************************************************************************************************************************************
        //RefreshWindow updates the block states in the view window from the class variables of the selected block
        //<mLocalState> integer holding the state of the current block
        private void RefreshWindow(int mLocalState)
        {
            
            //0x00000002 = contains switch on siding
            //0x00000003 = contains switch on main
            //0x00000004 = contains station
            //0x00000040 = contains crossing, all open
            //0x00000060 = contains crossing, bar closed
            //0x00000050 = contains crossing, lights on
            //0x00100000 = transit light left is green
            //0x00200000 = transit light left is yellow
            //0x00300000 = transit light left is red
            //0x00010000 = transit light right is green
            //0x00020000 = transit light right is Yellow
            //0x00030000 = tranist light right is red
            //0x00000400 = track is broken
            //0x00000800 = no track signal
            //0x00001000 = block not occupied
            //0x00002000 = CTC has not locked out
            //0x00004000 = Wayside controller has locked out

            mImage_Switch_Siding.Visible = (((mLocalState & 0x00000002) == 0x00000002) && ((mLocalState & 0x00000003) != 0x00000003));
            mImage_Switch_Main.Visible = ((mLocalState & 0x00000003) == 0x00000003);
            mImage_Crossing_Closed.Visible = ((mLocalState & 0x00000060) == 0x00000060);
            mImage_Crossing_Open.Visible = ((mLocalState & 0x00000040) == 0x00000040);
            mImage_TransitLeft_Green.Visible = ((mLocalState & 0x00100000) == 0x00100000);
            mImage_TransitLeft_Red.Visible = ((mLocalState & 0x00200000) == 0x00200000);
            mImage_TransitLeft_Yellow.Visible = ((mLocalState & 0x00300000) == 0x00300000);
            mImage_TransitRight_Green.Visible = ((mLocalState & 0x00010000) == 0x00010000);
            mImage_TransitRight_Red.Visible = ((mLocalState & 0x00020000) == 0x00020000);
            mImage_TransitRight_Yellow.Visible = ((mLocalState & 0x00030000) == 0x00030000);
            mImage_Unoccupied_Block.Visible = ((mLocalState & 0x00001000) == 0x00001000);
            mImage_Occupied_Block.Visible = ((mLocalState & 0x00001000) != 0x00001000);
            mLabel_Suggested_Speed.Text = mCurrentSuggestedSpeed.ToString();
            mLabel_Authority.Text = mCurrentAuthority.ToString();



            if ((mLocalState & 0x00002000) == 0x00002000)
            {

            }
            else
            {
            }

            if ((mLocalState & 0x00004000) != 0x00004000)
            {
                mButtonLockLocal.BackColor = Color.FromArgb(0, 192, 0);
                mButtonLockLocal.Image = Track_Controller_1._02.Properties.Resources.Lock_Open;
                this.mButtonLockLocal.Image = Track_Controller_1._02.Properties.Resources.Lock_Open;
                this.mButtonLockLocal.BackColor = Color.FromArgb(0, 192, 0);
                this.mButton_Toggle_Track_Occupancy.Enabled = false;
                this.mButton_Toggle_Track_Occupancy.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Lights_Left.Enabled = false;
                this.mButton_Toggle_Lights_Left.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Lights_Right.Enabled = false;
                this.mButton_Toggle_Lights_Right.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Switch.Enabled = false;
                this.mButton_Toggle_Switch.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Bar.Enabled = false;
                this.button3.Enabled = false;
                this.button3.BackColor = SystemColors.ControlDark;
            }
            else
            {
                mButtonLockLocal.BackColor = Color.Red;
                mButtonLockLocal.Image = Track_Controller_1._02.Properties.Resources.Lock_Closed;
                this.mButtonLockLocal.Image = Track_Controller_1._02.Properties.Resources.Lock_Closed;
                this.mButtonLockLocal.BackColor = Color.Red;
                this.mButton_Toggle_Track_Occupancy.Enabled = true;
                this.mButton_Toggle_Track_Occupancy.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Lights_Left.Enabled = true;
                this.mButton_Toggle_Lights_Left.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Lights_Right.Enabled = true;
                this.mButton_Toggle_Lights_Right.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Switch.Enabled = true;
                this.mButton_Toggle_Switch.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Bar.Enabled = true;
                this.mButton_Toggle_Bar.BackColor = SystemColors.ButtonFace;
                this.button3.Enabled = true;
                this.button3.BackColor = SystemColors.ButtonFace;
            }

            mLabel_Display_Speed.Text = (mLines[mCurrentLineIndex].getmSpeedLimit(mCurrentSectionIndex, mCurrentBlockIndex)).ToString();
        }

        //*******************************************************************************************************************************************
        //mButton_SetSuggestedSpeed_Click sets the suggested speed for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_SetSuggestedSpeed_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            mCurrentSuggestedSpeed = Convert.ToInt32(mText_SuggestedSpeed.Text);
            mLines[mCurrentLineIndex].setSuggested(mCurrentSectionIndex, mCurrentBlockIndex, mCurrentSuggestedSpeed);
            mLabel_Suggested_Speed.Text = (mLines[mCurrentLineIndex].getSuggested(mCurrentSectionIndex, mCurrentBlockIndex)).ToString();
        }

        //*******************************************************************************************************************************************
        //SetAuthority sets the authority for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void SetAuthority(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            mCurrentAuthority = Convert.ToInt32(mTextBox_Authority.Text);
            mLines[mCurrentLineIndex].setAuthority(mCurrentSectionIndex, mCurrentBlockIndex, mCurrentAuthority);
            mLabel_Authority.Text = (mLines[mCurrentLineIndex].getAuthority(mCurrentSectionIndex, mCurrentBlockIndex)).ToString();
        }

        //********************************************************************************************************************************************
        //SplitArrays: This function splits the full line arrays into subarrays for each PLC.
        private void SplitArrays()
        {
            
             Array.Copy(mRedMaintenanceBlocks,0,mRed1MaintenanceBlocks,0,38);
             Array.Copy(mRedOccupancies,0,mRed1Occupancies,0,38);
             Array.Copy(mRedSpeeds,0,mRed1Speeds,0,38);
             Array.Copy(mRedAuthorities,0,mRed1Authorities,0,38);
             Array.Copy(mRedCrossings,0,mRed1Crossings,0,38);
             Array.Copy(mRedSwitches,0,mRed1Switches,0,38);
             Array.Copy(mRedLeftLights,0,mRed1LeftLights,0,38);
             Array.Copy(mRedRightLights,0,mRed1RightLights,0,38);

             Array.Copy(mRedMaintenanceBlocks,71,mRed1MaintenanceBlocks,38,6);
             Array.Copy(mRedOccupancies,71,mRed1Occupancies,38,6);
             Array.Copy(mRedSpeeds,71,mRed1Speeds,38,6);
             Array.Copy(mRedAuthorities,71,mRed1Authorities,38,6);
             Array.Copy(mRedCrossings,71,mRed1Crossings,38,6);
             Array.Copy(mRedSwitches,71,mRed1Switches,38,6);
             Array.Copy(mRedLeftLights,71,mRed1LeftLights,38,6);
             Array.Copy(mRedRightLights,71,mRed1RightLights,38,6);

             Array.Copy(mRedMaintenanceBlocks,32,mRed2MaintenanceBlocks,0,39);
             Array.Copy(mRedOccupancies,32,mRed2Occupancies,0,39);
             Array.Copy(mRedSpeeds,32,mRed2Speeds,0,39);
             Array.Copy(mRedAuthorities,32,mRed2Authorities,0,39);
             Array.Copy(mRedCrossings,32,mRed2Crossings,0,39);
             Array.Copy(mRedSwitches,32,mRed2Switches,0,39);
             Array.Copy(mRedLeftLights,32,mRed2LeftLights,0,39);
             Array.Copy(mRedRightLights,32,mRed2RightLights,0,39);

            return;
        }

        //********************************************************************************************************************************************
        //ArrayMerger: This function merges the subarrays into full line arrays
        private void ArrayMerger()
        {

            Array.Copy(mRed1MaintenanceBlocks, 0, mRedMaintenanceBlocks, 0, 38);
            Array.Copy(mRed1Occupancies, 0, mRedOccupancies, 0, 38);
            Array.Copy(mRed1Speeds, 0, mRedSpeeds, 0, 38);
            Array.Copy(mRed1Authorities, 0, mRedAuthorities, 0, 38);
            Array.Copy(mRed1Crossings, 0, mRedCrossings, 0, 38);
            Array.Copy(mRed1Switches, 0, mRedSwitches, 0, 38);
            Array.Copy(mRed1LeftLights, 0, mRedLeftLights, 0, 38);
            Array.Copy(mRed1RightLights, 0, mRedRightLights, 0, 38);

            Array.Copy(mRed1MaintenanceBlocks, 37, mRedMaintenanceBlocks, 71, 6);
            Array.Copy(mRed1Occupancies, 37, mRedOccupancies, 71, 6);
            Array.Copy(mRed1Speeds, 37, mRedSpeeds, 71, 6);
            Array.Copy(mRed1Authorities, 37, mRedAuthorities, 71, 6);
            Array.Copy(mRed1Crossings, 37, mRedCrossings, 71, 6);
            Array.Copy(mRed1Switches, 37, mRedSwitches, 71, 6);
            Array.Copy(mRed1LeftLights, 37, mRedLeftLights, 71, 6);
            Array.Copy(mRed1RightLights, 37, mRedRightLights, 71, 6);

            Array.Copy(mRed2MaintenanceBlocks, 0, mRedMaintenanceBlocks, 32, 39);
            Array.Copy(mRed2Occupancies, 0, mRedOccupancies, 32, 39);
            Array.Copy(mRed2Speeds, 0, mRedSpeeds, 32, 39);
            Array.Copy(mRed2Authorities, 0, mRedAuthorities, 32, 39);
            Array.Copy(mRed2Crossings, 0, mRedCrossings, 32, 39);
            Array.Copy(mRed2Switches, 0, mRedSwitches, 32, 39);
            Array.Copy(mRed2LeftLights, 0, mRedLeftLights, 32, 39);
            Array.Copy(mRed2RightLights, 0, mRedRightLights, 32, 39);

            return;
        }
    }
}