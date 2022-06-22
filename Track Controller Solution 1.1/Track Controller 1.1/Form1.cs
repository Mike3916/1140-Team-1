
namespace Track_Controller_1._02
{
    public partial class Form_WC : Form
    {

        private OpenFileDialog mOFD;
        
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
        //Toggle_TestMode_Click: Enables/Disables the test window on the left side of the UI form
        //<sender>: reference to object that raises the form load event in this case "Form_WC.Toggle_TestMode()".
        //<e>: Argument containing event data.
        //<void>
        private void Toggle_TestMode_Click(object sender, EventArgs e)
        {

        }

        //****************************************************************************************************************************************
        //Lock_Local_Click: Toggles the class variable private mLocalLock which enables/disables the CTC offfice ability to put blocks in and
        //out of service.
        //<sender>: reference to object that raises the form load event in this case "Form_WC.Toggle_TestMode()".
        //<e>: Argument containing event data.
        //<void>
        private void Lock_Local_Click(object sender, EventArgs e)
        {

        }

        private void mButton_Browse_Files_Click(object sender, EventArgs e)
        {
            mOFD.ShowDialog();
        }

        private void mButton_Commit_File_Click(object sender, EventArgs e)
        {

        }

        private void mButton_Discard_File_Click(object sender, EventArgs e)
        {

        }
    }
}