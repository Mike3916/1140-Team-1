using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TrainController
{
    public class Controller
    {
        // Serial port for connecting to Raspberry Pi:
        SerialPort pi = new SerialPort();

        // Boolean for switching between auto and manual driving modes:
        // 'false' is software controller, 'true' is hardware controller:
        public bool mControlType;

        public bool mAutoMode = false;
        public bool mLeftDoorsStatus = false;
        public bool mRightDoorsStatus = false;
        public bool mInteriorLightsStatus = false;
        public bool mExteriorLightsStatus = false;
        public bool mAnnouncementsStatus = false;
        public bool mServiceBrakeStatus = false;
        public bool mEmergencyBrakeStatus = false;

        public int mTemperature = 72;
        public int mKp = 0;
        public int mKi = 0;
        public int mCurSpeed = 0;
        public int mCmdSpeed = 0;
        public int mSetSpeed = 0;
        public int mCmdAuthority = 0;
        public int mCurAuthority = 0;
        public string mBeacon = "-";
        public float mCurPower = 0;

        public const float Pmax = 120000; // 120 kW
        public float Uk = 0;
        public float Ek = 0,Ek_prev = 0;
        public int T = 250; // 250 ms

        public Controller()
        {
            InitTimer();
        }

        public void setupHardware()
        {
            // Setup serial port information: 
            pi.PortName = "COM3";
            pi.BaudRate = 115200;
            pi.Parity = Parity.None;
            pi.DataBits = 8;
            pi.StopBits = StopBits.One;
            pi.Handshake = Handshake.None;
            pi.WriteTimeout = 500;

            // Open serial port and reset all stored data:
            pi.Open();
            pi.WriteLine("?");
        }

        public void setAutoMode()
        {
            mSetSpeed = mCmdSpeed;
            mAutoMode = true;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("m");
                string output = pi.ReadLine();
            }
        }

        public void setManualMode()
        {
            mAutoMode = false;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("m");
                string output = pi.ReadLine();
            }
        }

        public void setEmergencyBrake()
        {
            mEmergencyBrakeStatus = true;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("e");
                string output = pi.ReadLine();
            }
        }

        public void setServiceBrake()
        {
            if (!mControlType)
            {
                if (!mServiceBrakeStatus)
                {
                    mServiceBrakeStatus = true;
                }
                else
                {
                    mServiceBrakeStatus = false;
                }
            }

            // Hardware  Controls:
            else
            {
                pi.WriteLine("u");
                string output = pi.ReadLine();

                if (output == "On")
                {
                    mServiceBrakeStatus = true;
                }
                else
                {
                    mServiceBrakeStatus = false;
                }
            }
        }

        public void setLeftDoors()
        {
            if (!mControlType)
            {
                if (!mLeftDoorsStatus)
                {
                    mLeftDoorsStatus = true;
                }
                else
                {
                    mLeftDoorsStatus = false;
                }
            }

            //Hardware controls:
            else
            {
                pi.WriteLine("l");
                string output = pi.ReadLine();

                if (output == "Open")
                {
                    mLeftDoorsStatus = true;
                }
                else
                {
                    mLeftDoorsStatus = false;
                }
            }
        }

        public void setRightDoors()
        {
            if (!mControlType)
            {
                if (!mRightDoorsStatus)
                {
                    mRightDoorsStatus = true;
                }
                else
                {
                    mRightDoorsStatus = false;
                }
            }

            // Hardware controls:
            else
            {
                pi.WriteLine("r");
                string output = pi.ReadLine();

                if (output == "Open")
                {
                    mRightDoorsStatus = true;
                }
                else
                {
                    mRightDoorsStatus = false;
                }
            }
        }

        public void setInteriorLights()
        {
            if (!mControlType)
            {
                if (!mInteriorLightsStatus)
                {
                    mInteriorLightsStatus = true;
                }
                else
                {
                    mInteriorLightsStatus = false;
                }
            }
            else
            {
                pi.WriteLine("i");
                string output = pi.ReadLine();

                if (output == "On")
                {
                    mInteriorLightsStatus = true;
                }
                else
                {
                    mInteriorLightsStatus = false;
                }
            }
        }

        public void setExteriorLights()
        {
            if (!mControlType)
            {
                if (!mExteriorLightsStatus)
                {
                    mExteriorLightsStatus = true;
                }
                else
                {
                    mExteriorLightsStatus = false;
                }
            }
            else
            {
                pi.WriteLine("x");
                string output = pi.ReadLine();

                if (output == "On")
                {
                    mExteriorLightsStatus = true;
                }
                else
                {
                    mExteriorLightsStatus = false;
                }
            }
        }

        public void setAnnouncements()
        {
            if (!mControlType)
            {
                if (!mAnnouncementsStatus)
                {
                    mAnnouncementsStatus = true;
                }
                else
                {
                    mAnnouncementsStatus = false;
                }
            }
            else
            {
                pi.WriteLine("a");
                string output = pi.ReadLine();

                if (output == "On")
                {
                    mAnnouncementsStatus = true;
                }
                else
                {
                    mAnnouncementsStatus = false;
                }
            }
        }

        public void tempIncrease()
        {
            if (!mControlType)
            {
                mTemperature++;
            }
            else
            {
                pi.WriteLine("h");
                string output = pi.ReadLine();

                if (output == "tempIncreased")
                {
                    mTemperature++;
                }
            }
        }

        public void tempDecrease()
        {
            if (!mControlType)
            {
                mTemperature--;
            }
            else
            {
                pi.WriteLine("c");
                string output = pi.ReadLine();

                if (output == "tempDecreased")
                {
                    mTemperature--;
                }
            }
        }

        public void setKp(int value)
        {
            mKp = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("j");
                pi.WriteLine(mKp.ToString() + "\n");
            }
        }

        public void setKi(int value)
        {
            mKi = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("k");
                pi.WriteLine(mKi.ToString() + "\n");
            }
        }

        public void setCmdSpeed(int value)
        {
            mCmdSpeed = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("v");
                pi.WriteLine(mCmdSpeed.ToString() + "\n");
            }
        }

        public void setSetSpeed(int value)
        {
            if (!mControlType)
            {
                if (value > mCmdSpeed)
                {
                    MessageBox.Show("Set Speed Shall Not Exceed Commanded Speed");
                }
                else if (value < 0)
                {
                    MessageBox.Show("Set Speed Shall Not Be Less Than Zero");
                }
                else
                {
                    mSetSpeed = value;
                }
            }
            else
            {
                pi.WriteLine("b");
                pi.WriteLine(value.ToString() + "\n");
                string output = pi.ReadLine();

                if (output == "tooHigh")
                {
                    MessageBox.Show("Set Speed Shall Not Exceed Commanded Speed");
                }
                else if (value < 0)
                {
                    MessageBox.Show("Set Speed Shall Not Be Less Than Zero");
                }
                else
                {
                    mSetSpeed = value;
                }
            }
        }

        public void setCurSpeed(int value)
        {
            mCurSpeed = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("n");
                pi.WriteLine(mCurSpeed.ToString() + "\n");
            }
        }

        public void setCmdAuthority(int value)
        {
            mCmdAuthority = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("s");
                pi.WriteLine(mCmdAuthority.ToString() + "\n");
            }
        }

        public void setCurAuthority(int value)
        {
            mCurAuthority = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("d");
                pi.WriteLine(mCurAuthority.ToString() + "\n");
            }
        }

        public void setBeacon(string value)
        {
            // Software Controls:
            if (!mControlType)
            {
                mBeacon = value;
                //Beacon.Text = "Nearest Beacon:\n" + value;
            }

            // Hardware Controls:
            else
            {
                pi.WriteLine("f");
                pi.WriteLine(value + "\n");
                string output = pi.ReadLine();

                mBeacon = output;
                //Beacon.Text = "Nearest Beacon:\n" + output;
            }
        }

        public void setPower(int value)
        {
            mCurPower = value;

            if (mControlType)
            {
                pi.WriteLine("p");
                pi.WriteLine(value + "\n");
                string output = pi.ReadLine();
            }
        }

        public void InitTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();

            if (!mControlType) dispatcherTimer.Tick += new EventHandler(SpeedUpdateSW);
            else dispatcherTimer.Tick += new EventHandler(SpeedUpdateHW);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, T);
            dispatcherTimer.Start();
        }

        public void SpeedUpdateSW(object sender, EventArgs e)
        {
            if (mAutoMode)
            {
                Ek_prev = Ek;
                Ek = mCmdSpeed - mCurSpeed;
            }
            else
            {
                Ek_prev = Ek;
                Ek = mSetSpeed - mCurSpeed;
            }

            if (mEmergencyBrakeStatus)
            {
                if (mCurSpeed == 0)
                {
                    mEmergencyBrakeStatus = false;
                }
                else
                {
                    mCmdSpeed = 0;
                    mSetSpeed = 0;

                    mCurSpeed -= 1; // TODO: Replace with emergency brake deceleration!
                }
            }
            else if (mServiceBrakeStatus)
            {
                if (mCurSpeed > 0)
                {
                    mCurSpeed--;  // TODO: Replace with service brake deceleration!
                }
            }
            else if (mAutoMode)
            {
                if (mCurSpeed < mCmdSpeed)
                {
                    mCurSpeed++;    // TODO: Replace with acceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = (mKp * Ek) + (mKi * Uk);
                }
                else if (mCurSpeed > mCmdSpeed)
                {
                    mCurSpeed--;    // TODO: Replace with deceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = (mKp * Ek) + (mKi * Uk);
                }
                else
                {
                    mCurPower = 0;
                }
            }
            else
            {
                if (mCurSpeed < mSetSpeed)
                {
                    mCurSpeed++;    // TODO: Replace with acceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = (mKp * Ek) + (mKi * Uk);
                }
                else if (mCurSpeed > mSetSpeed)
                {
                    mCurSpeed--;    // TODO: Replace with deceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = (mKp * Ek) + (mKi * Uk);
                }
                else
                {
                    mCurPower = 0;
                }
            }
        }

        public void SpeedUpdateHW(object sender, EventArgs e)
        {
            if(mAutoMode)
            {
                Ek_prev = Ek;
                Ek = mCmdSpeed - mCurSpeed;
            }
            else
            {
                Ek_prev = Ek;
                Ek = mSetSpeed - mCurSpeed;
            }
            

            if (mEmergencyBrakeStatus)
            {
                if (mCurSpeed == 0)
                {
                    mEmergencyBrakeStatus = false;
                }
                else
                {
                    mCmdSpeed = 0;
                    mSetSpeed = 0;

                    mCurSpeed -= 1; // TODO: Replace with emergency brake deceleration!
                }
            }
            else if (mServiceBrakeStatus)
            {
                if (mCurSpeed > 0)
                {
                    mCurSpeed--;  // TODO: Replace with service brake deceleration!
                }
            }
            else if (mAutoMode)
            {
                if (mCurSpeed < mCmdSpeed)
                {
                    mCurSpeed++;    // TODO: Replace with acceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = (mKp * Ek) + (mKi * Uk);
                }
                else if (mCurSpeed > mCmdSpeed)
                {
                    mCurSpeed--;    // TODO: Replace with deceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = (mKp * Ek) + (mKi * Uk);
                }
                else
                {
                    mCurPower = 0;
                }
            }
            else
            {
                if (mCurSpeed < mSetSpeed)
                {
                    mCurSpeed++;    // TODO: Replace with acceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = (mKp * Ek) + (mKi * Uk);
                }
                else if (mCurSpeed > mSetSpeed)
                {
                    mCurSpeed--;    // TODO: Replace with deceleration!

                    if (mCurPower < Pmax)
                    {
                        Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                    }
                    else
                    {
                        Uk = Uk;
                    }

                    mCurPower = (mKp * Ek) + (mKi * Uk);
                }
                else
                {
                    mCurPower = 0;
                }
            }
        }
    }
}
