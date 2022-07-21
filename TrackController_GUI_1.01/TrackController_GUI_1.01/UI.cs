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
    //[Serializable]
    public partial class Form_WC : Form
    {

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
        List<Controller> mPLCs = new List<Controller>();
        List<int> mControllerBlocks = new List<int>();
        List<int> mControllerSections = new List<int>();
        int mControllerLine = -1;


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

            this.FormClosing += new FormClosingEventHandler(Form_WC_FormClosing_1);



        }

        private void Form_WC_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            /*
            if (File.Exists("C:\Users\Michael\Downloads\mPLCs.json") == true)
            {

                mPLCs= JsonConvert.DeserializeObject<Controller>("C:\Users\Michael\Downloads\mPLCs.json");
            }
            */
            /*
            string output = JsonConvert.SerializeObject(mPLCs);
            File.WriteAllText(@"C:\Users\Michael\Downloads\mPLCs.json", output);
            output = JsonConvert.SerializeObject(mLines);
            File.WriteAllText(@"C:\Users\Michael\Downloads\mLines.json", output);
            output = JsonConvert.SerializeObject(mLineData);
            File.WriteAllText(@"C:\Users\Michael\Downloads\mLineData.json", output);
            output = JsonConvert.SerializeObject(mSectNames);
            File.WriteAllText(@"C:\Users\Michael\Downloads\mSectNames.json", output);
            */
           

        
        //Product deserializedProduct = JsonConvert.DeserializeObject<Product>(output);
        
    }

        //****************************************************************************************************************************************
        //Lock_Local_Click: Toggles the class variable private mLocalLock which enables/disables the CTC office ability to put blocks in and
        //out of service and enables the test window to toggle block states.
        //<sender>: reference to object that raises the form load event in this case "Form_WC.Toggle_TestMode()".
        //<e>: Argument containing event data.
        //<void>
        private void mButton_Lock_Local_Click(object sender, EventArgs e)
        {
            int mLocalState = 0;
            try
            {
                mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
                mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            }
            catch
            {
                MessageBox.Show("Please select a valid block.");
                return;
            }

            if((mLocalState & 0x00002000) == 0x00002000)
            {
                MessageBox.Show("CTC must authorize block for maintenance.");
                    return;
            }

            if (this.mButton_Lock_Local.BackColor == Color.FromArgb(0,192,0))
            {
                this.mButton_Lock_Local.Image = Track_Controller_1._02.Properties.Resources.Lock_Closed;
                this.mButton_Lock_Local.BackColor = Color.Red;
                this.mButton_Toggle_Track_Occupancy.Enabled = true;
                this.mButton_Toggle_Track_Occupancy.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Lights_Left.Enabled = true;
                this.mButton_Toggle_Lights_Left.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Lights_Right.Enabled = true;
                this.mButton_Toggle_Lights_Right.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Switch.Enabled = true;
                this.mButton_Toggle_Switch.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Rail.Enabled = true;
                this.mButton_Toggle_Rail.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Signal.Enabled = true;
                this.mButton_Toggle_Signal.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Bar.Enabled = true;
                this.mButton_Toggle_Bar.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Crossing_Lights.Enabled = true;
                this.mButton_Toggle_Crossing_Lights.BackColor = SystemColors.ButtonFace;
                this.mButton_SetBlockSpeed.Enabled = true;
                this.mButton_SetBlockSpeed.BackColor = SystemColors.ButtonFace;
                this.mTextBox_SetBlockSpeed.Enabled = true;
                this.mRichText_Serial_Data.Enabled = true;
                this.button3.Enabled = true;
                this.button3.BackColor = SystemColors.ButtonFace;
                this.mButton_SetSuggestedSpeed.Enabled = true;
                this.mButton_SetSuggestedSpeed.BackColor = SystemColors.ButtonFace;
                this.mText_SuggestedSpeed.Enabled = true;
                this.mTextBox_Authority.Enabled = true;
                this.mButton_SetAuthority.Enabled = true;
                this.mButton_SetAuthority.BackColor = SystemColors.ButtonFace;



                mLocalState = mLocalState | 0x00004000;

            }
            else
            {
                this.mButton_Lock_Local.Image = Track_Controller_1._02.Properties.Resources.Lock_Open;
                this.mButton_Lock_Local.BackColor = Color.FromArgb(0, 192, 0);
                this.mButton_Toggle_Track_Occupancy.Enabled = false;
                this.mButton_Toggle_Track_Occupancy.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Lights_Left.Enabled = false;
                this.mButton_Toggle_Lights_Left.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Lights_Right.Enabled = false;
                this.mButton_Toggle_Lights_Right.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Switch.Enabled = false;
                this.mButton_Toggle_Switch.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Rail.Enabled = false;
                this.mButton_Toggle_Rail.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Signal.Enabled = false;
                this.mButton_Toggle_Signal.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Bar.Enabled = false;
                this.mButton_Toggle_Bar.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Crossing_Lights.Enabled = false;
                this.mButton_Toggle_Crossing_Lights.BackColor = SystemColors.ControlDark;
                this.mButton_SetBlockSpeed.Enabled = false;
                this.mButton_SetBlockSpeed.BackColor = SystemColors.ControlDark;
                this.mTextBox_SetBlockSpeed.Text = "";
                this.mTextBox_SetBlockSpeed.Enabled = false;
                this.mRichText_Serial_Data.Enabled = false;
                this.button3.Enabled = false;
                this.button3.BackColor = SystemColors.ControlDark;
                this.mButton_SetSuggestedSpeed.Enabled = false;
                this.mButton_SetSuggestedSpeed.BackColor = SystemColors.ControlDark;
                this.mText_SuggestedSpeed.Enabled = false;
                this.mTextBox_Authority.Enabled = false;
                this.mButton_SetAuthority.Enabled = false;
                this.mButton_SetAuthority.BackColor = SystemColors.ControlDark;

                mLocalState = mLocalState & 0x0FFFBFFF;
            }

            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
        }


        //mButton_Controller_Connect_Click: Creates a new controller class object of type hardware and another of type software so connection can be tested. 
        //
        private void mButton_Controller_Connect_Click(object sender, EventArgs e)
        {
            //Controller mPLC1 = new Controller(851,false);
            Controller mPLC2 = new Controller(1300, true);

            //int[] test1 = new int[50];
            int[] test2 = new int[50];

            for(int i = 0; i < 50; i++)
            {
               // test1[i] = i;
                test2[i] = i;
            }


            //int[] test3 = mPLC1.SendPacket(test1);
            //int[] test4 = mPLC2.SendTrack(test2);

            //string string3 = "";
            /*string string4 = "";

            foreach(int i in test4)
            {
                //string3 += test1[i].ToString();
                string4 += test2[i].ToString();
            }

            //MessageBox.Show("Software returns " + string3);
            MessageBox.Show("Hardware returns " + string4);
            */
        }

        private void CreateSoftwareController()
        {

            

            string[] mSelectedBlocks = new string[mRichText_BlockInfo.Lines.Count()];
            for (int i=0; i<mRichText_BlockInfo.Lines.Count(); i++)
            {
                mSelectedBlocks[i] = mRichText_BlockInfo.Lines[i];
            }

            Controller mTempPLC = new Controller();

            //mPLCs.Add(mTempPLC);
        }

        private void CreateHardwareController()
        {

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

        private void mComboBox_Section_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Empties Block Combo Box
            mComboBox_Select_Block.Items.Clear();
            mComboBox_Select_Block.Items.Add("Select Block to View");


            //sets combobox index
            mCurrentSectionIndex = mComboBox_Section.SelectedIndex - 1;
            mCurrentBlockIndex = -1;

            List<string> mBlockNumberz = new List<string>();

            mBlockNumberz = mLines[mCurrentLineIndex].getSectBlockNum(mCurrentSectionIndex);

            for(int i = 0; i < mBlockNumberz.Count; i++)
            {
                mComboBox_Select_Block.Items.Add(mBlockNumberz[i]);
            }


            mLabel_Section.Text = "Section: " + mComboBox_Section.Text;
            mLabel_Block.Text = "Block: ";
        }

        //***************************************************************************************************************************************
        //mComboBox_Select_Line_SelectedIndexChanged: Updates the section combobox when the selected line is changed.
        private void mComboBox_Select_Line_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Resets combo box
            mComboBox_Section.Items.Clear();
            mComboBox_Section.Items.Add("Select Section");
            mComboBox_Select_Block.Items.Clear();
            mComboBox_Select_Block.Items.Add("Select Block to View");
            mRichText_BlockInfo.Clear();
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

            mLabel_Line.Text = "Line: " + mLines[mCurrentLineIndex].getmnameLine();
            mLabel_Section.Text = "Section: ";
            mLabel_Block.Text = "Block: ";
        }


        //****************************************************************************************************************************************
        //mComboBox_ControllerSelect_SelectedIndexChanged: Updates the code window with the code associated with selected controller and populates 
        //the block select comboBox.
        private void mComboBox_ControllerSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        //*****************************************************************************************************************************************
        //mComboBox_Select_Block_SelectedIndexChanged: Updates the view block panel with current block states
        private void mComboBox_Select_Block_SelectedIndexChanged(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;

            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mCurrentAuthority = mLines[mCurrentLineIndex].getSuggested(mCurrentSectionIndex, mCurrentBlockIndex);
            mCurrentSuggestedSpeed = mLines[mCurrentLineIndex].getAuthority(mCurrentSectionIndex, mCurrentBlockIndex);

            mLabel_Block.Text = "Block: " + mComboBox_Select_Block.Text;

            this.RefreshWindow(mLocalState);


            /*
            0x00000002 = contains switch on siding
            0x00000003 = contains switch on main
            0x00000004 = contains station
            0x00000040 = contains crossing, all open
            0x00000060 = contains crossing, bar closed
            0x00000050 = contains crossing, lights on
            0x00100000 = transit light left is green
            0x00200000 = transit light left is yellow
            0x00300000 = transit light left is red
            0x00010000 = transit light right is green
            0x00020000 = transit light right is Yellow
            0x00030000 = tranist light right is red
            0x00000400 = track is broken
            0x00000800 = no track signal
            0x00001000 = block occupied
            0x00002000 = CTC has not locked out
            0x00004000 = Wayside controller has locked out
            */
        }



        //*******************************************************************************************************************************************
        //mButton_Toggle_Switch_Click Toggles the switch position for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Switch_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalState = mLocalState ^ 0x00000001;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
            this.RefreshWindow(mLocalState);
            MessageBox.Show(mLocalState.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Track_Occupancy_Click toggles the track occupancy for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Track_Occupancy_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalState = mLocalState ^ 0x00001000;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
            this.RefreshWindow(mLocalState);
            MessageBox.Show(mLocalState.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Lights_Left_Click toggles the left lights for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Lights_Left_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalState = mLocalState ^ 0x00100000;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
            this.RefreshWindow(mLocalState);
            MessageBox.Show(mLocalState.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_SetAuthority_Click sets the authority for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mLeft_Green_CheckedChanged(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalState = mLocalState | 0x00100000;
            mLocalState = mLocalState & 0x0FDFFFFF;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
            this.RefreshWindow(mLocalState);
            MessageBox.Show(mLocalState.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Lights_Right_Click toggles the right side transit lights for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Lights_Right_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalState = mLocalState ^ 0x00010000;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
            this.RefreshWindow(mLocalState);
            MessageBox.Show(mLocalState.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Signal_Click toggles the track signal for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Signal_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalState = mLocalState ^ 0x00000800;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
            this.RefreshWindow(mLocalState);
            MessageBox.Show(mLocalState.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_SetAuthority_Click toggles the crossing bar for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Bar_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalState = mLocalState ^ 0x00000020;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
            this.RefreshWindow(mLocalState);
            MessageBox.Show(mLocalState.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Crossing_Lights_Click toggles the crossing lights for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Crossing_Lights_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalState = mLocalState ^ 0x00000010;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
            this.RefreshWindow(mLocalState);
            MessageBox.Show(mLocalState.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_SetBlockSpeed_Click sets the speed limit for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_SetBlockSpeed_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalSpeed = mLines[mCurrentLineIndex].getmSpeedLimit(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalSpeed = Convert.ToInt32(mTextBox_SetBlockSpeed.Text);
            mLines[mCurrentLineIndex].setmSpeedLimit(mCurrentSectionIndex, mCurrentBlockIndex, mLocalSpeed);
            mLabel_Display_Speed.Text = (mLines[mCurrentLineIndex].getmSpeedLimit(mCurrentSectionIndex, mCurrentBlockIndex)).ToString();
            MessageBox.Show(mLocalSpeed.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_Toggle_Rail_Click toggles broken rail for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_Toggle_Rail_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);
            mLocalState = mLocalState ^ 0x00000400;
            mLines[mCurrentLineIndex].setmBlockState(mCurrentSectionIndex, mCurrentBlockIndex, mLocalState);
            this.RefreshWindow(mLocalState);
            MessageBox.Show(mLocalState.ToString());
        }

        //*******************************************************************************************************************************************
        //RefreshWindow updates the block states in the view window from the class variables of the selected block
        //<mLocalState> integer holding the state of the current block
        private void RefreshWindow(int mLocalState)
        {
            mImage_Switch_Siding.Visible = (((mLocalState & 0x00000002) == 0x00000002) && ((mLocalState & 0x00000003) != 0x00000003));
            mImage_Switch_Main.Visible = ((mLocalState & 0x00000003) == 0x00000003);
            mImage_Crossing_Closed.Visible = ((mLocalState & 0x00000060) == 0x00000060);
            mImage_Crossing_Open.Visible = ((mLocalState & 0x00000040) == 0x00000040);
            mImage_CrossingLights_Off.Visible = ((mLocalState & 0x00000040) == 0x00000040);
            mImage_CrossingLights_On.Visible = ((mLocalState & 0x00000050) == 0x00000050);
            mImage_TransitLeft_Green.Visible = ((mLocalState & 0x00100000) == 0x00100000);
            mImage_TransitLeft_Red.Visible = ((mLocalState & 0x00200000) == 0x00200000);
            mImage_TransitLeft_Yellow.Visible = ((mLocalState & 0x00300000) == 0x00300000);
            mImage_TransitRight_Green.Visible = ((mLocalState & 0x00010000) == 0x00010000);
            mImage_TransitRight_Red.Visible = ((mLocalState & 0x00020000) == 0x00020000);
            mImage_TransitRight_Yellow.Visible = ((mLocalState & 0x00030000) == 0x00030000);
            mImage_BrokenRail.Visible = ((mLocalState & 0x00000400) == 0x00000400);
            mImage_NoTrackSignal.Visible = ((mLocalState & 0x00000800) == 0x00000800);
            mImage_Unoccupied_Block.Visible = ((mLocalState & 0x00001000) != 0x00001000);
            mImage_Occupied_Block.Visible = ((mLocalState & 0x00001000) == 0x00001000);
            mLabel_Suggested_Speed.Text = mCurrentSuggestedSpeed.ToString();
            mLabel_Authority.Text = mCurrentAuthority.ToString();



            if ((mLocalState & 0x00002000) == 0x00002000)
            {
                mButton_CTC_Lockout.BackColor = Color.FromArgb(0, 192, 0);
                mButton_CTC_Lockout.Image = Track_Controller_1._02.Properties.Resources.Lock_Open;
            }
            else
            {
                mButton_CTC_Lockout.BackColor = Color.Red;
                mButton_CTC_Lockout.Image = Track_Controller_1._02.Properties.Resources.Lock_Closed;
            }

            if ((mLocalState & 0x00004000) != 0x00004000)
            {
                mButton_Lock_Local.BackColor = Color.FromArgb(0, 192, 0);
                mButton_Lock_Local.Image = Track_Controller_1._02.Properties.Resources.Lock_Open;
                this.mButton_Lock_Local.Image = Track_Controller_1._02.Properties.Resources.Lock_Open;
                this.mButton_Lock_Local.BackColor = Color.FromArgb(0, 192, 0);
                this.mButton_Toggle_Track_Occupancy.Enabled = false;
                this.mButton_Toggle_Track_Occupancy.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Lights_Left.Enabled = false;
                this.mButton_Toggle_Lights_Left.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Lights_Right.Enabled = false;
                this.mButton_Toggle_Lights_Right.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Switch.Enabled = false;
                this.mButton_Toggle_Switch.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Rail.Enabled = false;
                this.mButton_Toggle_Rail.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Signal.Enabled = false;
                this.mButton_Toggle_Signal.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Bar.Enabled = false;
                this.mButton_Toggle_Bar.BackColor = SystemColors.ControlDark;
                this.mButton_Toggle_Crossing_Lights.Enabled = false;
                this.mButton_Toggle_Crossing_Lights.BackColor = SystemColors.ControlDark;
                this.mButton_SetBlockSpeed.Enabled = false;
                this.mButton_SetBlockSpeed.BackColor = SystemColors.ControlDark;
                this.mTextBox_SetBlockSpeed.Text = "";
                this.mTextBox_SetBlockSpeed.Enabled = false;
                this.mRichText_Serial_Data.Enabled = true;
                this.button3.Enabled = false;
                this.button3.BackColor = SystemColors.ControlDark;
            }
            else
            {
                mButton_Lock_Local.BackColor = Color.Red;
                mButton_Lock_Local.Image = Track_Controller_1._02.Properties.Resources.Lock_Closed;
                this.mButton_Lock_Local.Image = Track_Controller_1._02.Properties.Resources.Lock_Closed;
                this.mButton_Lock_Local.BackColor = Color.Red;
                this.mButton_Toggle_Track_Occupancy.Enabled = true;
                this.mButton_Toggle_Track_Occupancy.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Lights_Left.Enabled = true;
                this.mButton_Toggle_Lights_Left.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Lights_Right.Enabled = true;
                this.mButton_Toggle_Lights_Right.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Switch.Enabled = true;
                this.mButton_Toggle_Switch.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Rail.Enabled = true;
                this.mButton_Toggle_Rail.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Signal.Enabled = true;
                this.mButton_Toggle_Signal.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Bar.Enabled = true;
                this.mButton_Toggle_Bar.BackColor = SystemColors.ButtonFace;
                this.mButton_Toggle_Crossing_Lights.Enabled = true;
                this.mButton_Toggle_Crossing_Lights.BackColor = SystemColors.ButtonFace;
                this.mButton_SetBlockSpeed.Enabled = true;
                this.mButton_SetBlockSpeed.BackColor = SystemColors.ButtonFace;
                this.mTextBox_SetBlockSpeed.Enabled = true;
                this.mRichText_Serial_Data.Enabled = true;
                this.button3.Enabled = true;
                this.button3.BackColor = SystemColors.ButtonFace;
            }

            mLabel_Display_Speed.Text = (mLines[mCurrentLineIndex].getmSpeedLimit(mCurrentSectionIndex, mCurrentBlockIndex)).ToString();
        }

        //This section may become obselete
        //*******************************************************************************************************************************************
        //mbutton3_Click Sends a serial packet to a dummy server to test TCP communications
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        /*
        private void button3_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            int mLocalState = mLines[mCurrentLineIndex].getmBlockState(mCurrentSectionIndex, mCurrentBlockIndex);

            string msg = mLines[mCurrentLineIndex].getmnameLine() + "." + mCurrentSectionIndex + "." + mCurrentBlockIndex.ToString() + "." + mLocalState.ToString();
            string mIP = "127.0.0.1";
            int mPort = 1300;

            CommunicationClient mClient2 = new CommunicationClient();
            string response = mClient2.MessageSender(mIP, mPort, msg);

            mRichText_Serial_Data.AppendText("\r\n" + msg);
            mRichText_Serial_Data.ScrollToCaret();
            mRichText_Serial_Data.AppendText("\r\n" + response);
            mRichText_Serial_Data.ScrollToCaret();

        }*/


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
            MessageBox.Show(mCurrentSuggestedSpeed.ToString());
        }

        //*******************************************************************************************************************************************
        //mButton_SetAuthority_Click sets the authority for a specific block and then shows the change in the block view window.
        //<sender> event sender (form control)
        //<e> event arguments (button click)
        private void mButton_SetAuthority_Click(object sender, EventArgs e)
        {
            mCurrentBlockIndex = mComboBox_Select_Block.SelectedIndex - 1;
            mCurrentAuthority = Convert.ToInt32(mTextBox_Authority.Text);
            mLines[mCurrentLineIndex].setAuthority(mCurrentSectionIndex, mCurrentBlockIndex, mCurrentAuthority);
            mLabel_Authority.Text = (mLines[mCurrentLineIndex].getAuthority(mCurrentSectionIndex, mCurrentBlockIndex)).ToString();
            MessageBox.Show(mCurrentAuthority.ToString());
        }

        private void mButton_Block_Adder_Click(object sender, EventArgs e)
        {

            mRichText_BlockInfo.AppendText("\r\n" + mCurrentLineIndex + "." + mCurrentSectionIndex + "." + mCurrentBlockIndex);
            mRichText_BlockInfo.ScrollToCaret();


            mControllerLine = mCurrentLineIndex;
            mControllerBlocks.Add(mCurrentBlockIndex);
            mControllerSections.Add(mCurrentSectionIndex);


        }

        private void mButton_Clear_Click(object sender, EventArgs e)
        {
            mRichText_BlockInfo.Text = "";
            mText_Port.Text = "";
            mText_Address.Text = "";


        }

    }
}