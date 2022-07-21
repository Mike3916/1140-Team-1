using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TrainController
{
    public class Controller
    {
        // Serial port for connecting to Raspberry Pi:
        SerialPort pi = new SerialPort();

        // Dispatcher timers for power calculation and speed updating:
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer updateSpeed = new DispatcherTimer();

        // Boolean for switching between auto and manual driving modes:
        // 'false' is software controller, 'true' is hardware controller:
        public bool mControlType;
        public bool mSetControlType = false;

        // Boolean for controlling hardware functionality:
        public bool mSerialAccepted = true;

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
        public double mCurSpeed = 0;
        public double mCmdSpeed = 0;
        public double mSetSpeed = 0;
        public int mCmdAuthority = 0;
        public int mCurAuthority = 0;
        public string mBeacon = "-";
        public double mCurPower = 0;

        public const float Pmax = 120000; // 120 kW
        public double Uk = 0;
        public double Ek = 0;
        public double Ek_prev = 0;
        public int T = 250; // 250 ms

        public Controller()
        {

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
            dispatcherTimer.Stop();
            mSetSpeed = mCmdSpeed;
            mAutoMode = true;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("m");
                string output = pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void setManualMode()
        {
            dispatcherTimer.Stop();
            mAutoMode = false;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("m");
                string output = pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void setEmergencyBrake()
        {
            dispatcherTimer.Stop();

            mEmergencyBrakeStatus = true;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("e");
                string output = pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void setServiceBrake()
        {
            dispatcherTimer.Stop();

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

            dispatcherTimer.Start();
        }

        public void setLeftDoors()
        {
            dispatcherTimer.Stop();

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

            dispatcherTimer.Start();
        }

        public void setRightDoors()
        {
            dispatcherTimer.Stop();

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

            dispatcherTimer.Start();
        }

        public void setInteriorLights()
        {
            dispatcherTimer.Stop();

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

            // Hardware controls:
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

            dispatcherTimer.Start();
        }

        public void setExteriorLights()
        {
            dispatcherTimer.Stop();

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

            // Hardware controls:
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

            dispatcherTimer.Start();
        }

        public void setAnnouncements()
        {
            dispatcherTimer.Stop();

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

            // Hardware controls:
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

            dispatcherTimer.Start();
        }

        public void tempIncrease()
        {
            dispatcherTimer.Stop();

            if (!mControlType)
            {
                mTemperature++;
            }

            // Hardware controls:
            else
            {
                pi.WriteLine("h");
                string output = pi.ReadLine();

                if (output == "tempIncreased")
                {
                    mTemperature++;
                }
            }

            dispatcherTimer.Start();
        }

        public void tempDecrease()
        {
            dispatcherTimer.Stop();

            if (!mControlType)
            {
                mTemperature--;
            }

            //Hardware controls:
            else
            {
                pi.WriteLine("c");
                string output = pi.ReadLine();
                MessageBox.Show(output);

                if (output == "tempDecreased")
                {
                    mTemperature--;
                }
            }

            dispatcherTimer.Start();
        }

        public void setKp(int value)
        {
            dispatcherTimer.Stop();
            mKp = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("j");
                pi.WriteLine(mKp.ToString() + "\n");
                pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void setKi(int value)
        {
            dispatcherTimer.Stop();
            mKi = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("k");
                pi.WriteLine(mKi.ToString() + "\n");
                pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void setCmdSpeed(double value)
        {
            dispatcherTimer.Stop();
            mCmdSpeed = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("v");
                pi.WriteLine(mCmdSpeed.ToString() + "\n");
                pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void setSetSpeed(double value)
        {
            dispatcherTimer.Stop();

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

            // Hardware controls:
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

            dispatcherTimer.Start();
        }

        public void setCurSpeed(double value)
        {
            dispatcherTimer.Stop();
            mCurSpeed = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("n");
                pi.WriteLine(mCurSpeed.ToString() + "\n");
                pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void setCmdAuthority(int value)
        {
            dispatcherTimer.Stop();
            mCmdAuthority = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("s");
                pi.WriteLine(mCmdAuthority.ToString() + "\n");
                pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void setCurAuthority(int value)
        {
            dispatcherTimer.Stop();
            mCurAuthority = value;

            // Hardware Controls:
            if (mControlType)
            {
                pi.WriteLine("d");
                pi.WriteLine(mCurAuthority.ToString() + "\n");
                pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void setBeacon(string value)
        {
            dispatcherTimer.Stop();

            // Software Controls:
            if (!mControlType)
            {
                mBeacon = value;
            }

            // Hardware Controls:
            else
            {
                pi.WriteLine("f");
                pi.WriteLine(value + "\n");
                string output = pi.ReadLine();

                mBeacon = output;
            }

            dispatcherTimer.Start();
        }

        public void setPower(int value)
        {
            dispatcherTimer.Stop();
            mCurPower = value;

            mSerialAccepted = false;

            if (mControlType)
            {
                pi.WriteLine("p");
                pi.WriteLine(value + "\n");
                string output = pi.ReadLine();
            }

            dispatcherTimer.Start();
        }

        public void InitTimer()
        {
            if (!mControlType) dispatcherTimer.Tick += new EventHandler(CalculatePowerSW);
            else dispatcherTimer.Tick += new EventHandler(CalculatePowerHW);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, T);
            dispatcherTimer.Start();

            updateSpeed.Tick += new EventHandler(UpdateSpeed);

            updateSpeed.Interval = new TimeSpan(0, 0, 0, 0, T);
            updateSpeed.Start();
        }

        public void UpdateSpeed(object sender, EventArgs e)
        {
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

                    mCurSpeed -= 1;
                }
            }
            else if (mServiceBrakeStatus)
            {
                if (mCurSpeed > 0)
                {
                    mCurSpeed--;
                }
            }
            else if (mAutoMode)
            {
                if (mCurSpeed < mCmdSpeed)
                {
                    mCurSpeed++;
                }
                else if (mCurSpeed > mCmdSpeed)
                {
                    mCurSpeed--;
                }
            }
            else
            {
                if (mCurSpeed < mSetSpeed)
                {
                    mCurSpeed++;
                }
                else if (mCurSpeed > mSetSpeed)
                {
                    mCurSpeed--;
                }
            }
        }

        public void CalculatePowerSW(object sender, EventArgs e)
        {
            double[] powerOutput = new double[3];
            double powerCheck = 0;

            for (int i = 0; i < 3; i++)
            {
                if (mAutoMode)
                {
                    Ek_prev = Ek;
                    Ek = mCmdSpeed - mCurSpeed;

                    if (mCurSpeed < mCmdSpeed)
                    {
                        if (mCurPower < Pmax)
                        {
                            Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                        }
                        else
                        {
                            Uk = Uk;
                        }

                        powerOutput[i] = (mKp * Ek) + (mKi * Uk);
                    }
                    else if (mCurSpeed > mCmdSpeed)
                    {
                        if (mCurPower < Pmax)
                        {
                            Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                        }
                        else
                        {
                            Uk = Uk;
                        }

                        powerOutput[i] = (mKp * Ek) + (mKi * Uk);
                    }
                    else
                    {
                        mCurPower = 0;
                    }
                }
                else
                {
                    Ek_prev = Ek;
                    Ek = mSetSpeed - mCurSpeed;

                    if (mCurSpeed < mSetSpeed)
                    {
                        if (mCurPower < Pmax)
                        {
                            Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                        }
                        else
                        {
                            Uk = Uk;
                        }

                        powerOutput[i] = (mKp * Ek) + (mKi * Uk);
                    }
                    else if (mCurSpeed > mSetSpeed)
                    {
                        if (mCurPower < Pmax)
                        {
                            Uk = Uk + (T / 1000) / 2 * (Ek + Ek_prev);
                        }
                        else
                        {
                            Uk = Uk;
                        }

                        powerOutput[i] = (mKp * Ek) + (mKi * Uk);
                    }
                    else
                    {
                        mCurPower = 0;
                    }
                }
            }

            // Any pair of outputs are equal (Modal calc):
            if (powerOutput[0] == powerOutput[1])
                powerCheck = powerOutput[0];

            else if (powerOutput[0] == powerOutput[2])
                powerCheck = powerOutput[0];

            else if (powerOutput[1] == powerOutput[2])
                powerCheck = powerOutput[1];

            // No outputs match, choose smallest:
            else if (powerOutput[0] <= powerOutput[1] && powerOutput[0] <= powerOutput[2])
                powerCheck = powerOutput[0];

            else if (powerOutput[1] <= powerOutput[0] && powerOutput[1] <= powerOutput[2])
                powerCheck = powerOutput[1];

            else
                powerCheck = powerOutput[2];

            // Check calculate power not above max
            if (powerCheck > Pmax)
            {
                mCurPower = Pmax;
            }
            else if (powerCheck < -Pmax)
            {
                mCurPower = -Pmax;
            }
            else
            {
                mCurPower = powerCheck;
            }
        }

        public void CalculatePowerHW(object sender, EventArgs e)
        {
            string output;
            double[] powerOutput = new double[3];
            double powerCheck = 0;

            for (int i = 0; i < 3; i++)
            {
                // Update current speed in Pi storage:
                pi.WriteLine("n");
                pi.WriteLine(mCurSpeed.ToString() + "\n");
                pi.ReadLine(); //*/

                // Calculate current speed from Pi storage:
                pi.WriteLine("o");
                output = pi.ReadLine();

                powerOutput[i] = Double.Parse(output);
            }

            // Any pair of outputs are equal (Modal calc):
            if (powerOutput[0] == powerOutput[1])
                powerCheck = powerOutput[0];

            else if (powerOutput[0] == powerOutput[2])
                powerCheck = powerOutput[0];

            else if (powerOutput[1] == powerOutput[2])
                powerCheck = powerOutput[1];

            // No outputs match, choose smallest:
            else if (powerOutput[0] <= powerOutput[1] && powerOutput[0] <= powerOutput[2])
                powerCheck = powerOutput[0];

            else if (powerOutput[1] <= powerOutput[0] && powerOutput[1] <= powerOutput[2])
                powerCheck = powerOutput[1];

            else
                powerCheck = powerOutput[2];

            // Check calculate power not above max
            if (powerCheck > Pmax)
            {
                mCurPower = Pmax;
            }
            else if (powerCheck < -Pmax)
            {
                mCurPower = -Pmax;
            }
            else
            {
                mCurPower = powerCheck;
            }
        }
    }
}
