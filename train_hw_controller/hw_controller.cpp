#include <stdio.h>
#include <string.h>
#include <errno.h>
#include <wiringSerial.h>
#include <string>
#include <iostream>

using namespace std;

class serialConnect
{
	public:
		bool mEmergencyBrakeStatus=false;
		bool mServiceBrakeStatus=false;
		bool mAutoMode=true;
		bool mLeftDoors=false;
		bool mRightDoors=false;
		bool mInteriorLights=false;
		bool mExteriorLights=false;
		bool mAnnouncementsStatus=false;
		
		int mTemperature=72;
		int mKp=10000;
		int mKi=0;
		
		float mCmdSpeed=0;
		float mSetSpeed=0;
		float mCurSpeed=0;
		
		int mCmdAuthority=0;
		int mCurAuthority=0;
		float mCurPower=0;
		int fd;
		
		const float Pmax=120000;
		float Uk=0;
		float Ek=0;
		float Ek_prev=0;
		int T=1; // 1 ms
		
		string mCurBeacon="-";
		
		serialConnect()
		{
			if((fd=serialOpen("/dev/ttyS0",115200))<0)
			{
			  fprintf(stderr,"Unable to open serial device: %s\n",strerror(errno));
			}
		}
		
		const char* displayValues()
		{
			cout << "\n\n-------------------------------------------------------\n"
				 << "mEmergencyBrakeStatus = " << mEmergencyBrakeStatus << "\n"
				 << "mServiceBrakeStatus = " << mServiceBrakeStatus << "\n"
				 << "mAutoMode = " << mAutoMode << "\n"
				 << "mLeftDoors = " << mLeftDoors << "\n"
				 << "mRightDoors = " << mRightDoors << "\n"
				 << "mInteriorLights = " << mInteriorLights << "\n"
				 << "mExteriorLights = " << mExteriorLights << "\n"
				 << "mAnnouncementsStatus = " << mAnnouncementsStatus << "\n\n"
				 << "mTemperature = " << mTemperature << "\n"
				 << "mKp = " << mKp << "\n"
				 << "mKi = " << mKi << "\n"
				 << "mCmdSpeed = " << mCmdSpeed << "\n"
				 << "mSetSpeed = " << mSetSpeed << "\n"
				 << "mCurSpeed = " << mCurSpeed << "\n"
				 << "mCurPower = " << mCurPower << "\n"
				 << "mCmdAuthority = " << mCmdAuthority << "\n"
				 << "mCurAuthority = " << mCurAuthority << "\n"
				 << "mCurBeacon = " << mCurBeacon << "\n-------------------------------------------------------\n\n";
				 
			return "\n";
		}
		
		const char* resetValues()
		{
			// Vital bools:
			mEmergencyBrakeStatus = false;
			mServiceBrakeStatus = false;
			mAutoMode = true;
			
			// Non-vital bools:
			mLeftDoors = false;
			mRightDoors = false;
			mInteriorLights = false;
			mExteriorLights = false;
			mAnnouncementsStatus = false;
			
			// Misc Value:
			mTemperature = 72;
			mKp = 10000;
			mKi = 0;
			Uk = 0;
			Ek = 0;
			Ek_prev = 0;
			
			// Speed:
			mCmdSpeed = 0;
			mSetSpeed = 0;
			mCurSpeed = 0;
			
			// Authority:
			mCmdAuthority = 0;
			mCurAuthority = 0;
			
			// Beacon:
			mCurBeacon = "-";
			
			return "\n";
		}
		
		const char* toggleBool(bool &status,char identifier)
		{
			status = !status;
			switch(identifier)
			{
				case 'e': 
					return status==false ? "Off\n" : "On\n";
				case 'u':
					return status==false ? "Off\n" : "On\n";
				case 'm':
					return status==true ? "Auto\n" : "Manual\n";
				case 'l':
					return status==true ? "Open\n" : "Closed\n";
				case 'r':
					return status==true ? "Open\n" : "Closed\n";
				case 'i':
					return status==true ? "On\n" : "Off\n";
				case 'x':
					return status==true ? "On\n" : "Off\n";
				case 'a':
					return status==true ? "On\n" : "Off\n";
			}
			return "";
		}
		
		const char* temperature(int &value,char identifier)
		{
			switch(identifier)
			{
				case 'h':
					value++;
					return "tempIncreased\n";
				case 'c':
					value--;
					return "tempDecreased\n";
			}
			return ""; 
		}
		
		int getValue(int fd)
		{
			int numChar; 
			string str;
					
			for (int i=0;i<4;i++)
			{
				numChar = serialGetchar(fd);
				
				if((numChar>47 && numChar<58) || numChar == 46)
				{
					str += numChar;
				}
			}
				
			return stoi(str);
		}
		
		string getString(int fd)
		{
			int count=0,numChar;
			string str = "";
			
			// Get first '\n' char to flush stream, then get first char:
			numChar = serialGetchar(fd);
			numChar = serialGetchar(fd);
					
			while(numChar != 10)
			{
				str += numChar;
				
				// Update stream char, and increment count:
				numChar = serialGetchar(fd);
				count++;
			}
			
			/*cout << "\n size: " << str.size();
			cout << "\n string: " << str;*/
				
			return str+"\n";
		}
		
		const char* calculatePower()
		{
			float powerOutput[3];
            float powerCheck = 0;

            // calculate power three times for vitality
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
            
            string power = to_string(mCurPower) + "\n";

            return power.c_str();
		}
};

int main ()
{
	char input;
	const char* output;
	
	serialConnect comm;
	  
	while(1)
	{
		input = serialGetchar(comm.fd);
		putchar(input);
		fflush(stdout);
				
		// Reset all current values:
		if(input == '?')
		{
			comm.resetValues();
		}
				
		// Display all current values:
		else if(input == '`')
		{
			comm.displayValues();
		}		
		
		// Select Emergency Brakes:
		else if(input == 'e')
		{
			output = comm.toggleBool(comm.mEmergencyBrakeStatus,'e');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Toggle Service Brakes:
		else if(input == 'u')
		{
			output = comm.toggleBool(comm.mServiceBrakeStatus,'u');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Toggle Auto/Manual Mode:
		else if(input == 'm')
		{
			output = comm.toggleBool(comm.mAutoMode,'m');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Toggle Left Doors:
		else if(input == 'l')
		{
			output = comm.toggleBool(comm.mLeftDoors,'l');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Toggle Right Doors:
		else if(input == 'r')
		{
			output = comm.toggleBool(comm.mRightDoors,'r');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Toggle Interior Lights:
		else if(input == 'i')
		{
			output = comm.toggleBool(comm.mInteriorLights,'i');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Toggle Exterior Lights:
		else if(input == 'x')
		{
			output = comm.toggleBool(comm.mExteriorLights,'x');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Toggle Announcements:
		else if(input == 'a')
		{
			output = comm.toggleBool(comm.mAnnouncementsStatus,'a');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Increment Temperature:
		else if(input == 'h')
		{
			output = comm.temperature(comm.mTemperature,'h');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Decrement Temperature:
		else if(input == 'c')
		{
			output = comm.temperature(comm.mTemperature,'c');
			serialPrintf(comm.fd,output);
			//printf(output);
		}
		
		// Accept Kp:
		else if(input == 'j')
		{
			comm.mKp = comm.getValue(comm.fd);
			serialPrintf(comm.fd,"valueReceived\n");
			//printf("valueReceived\n");
		}
		
		// Accept Ki:
		else if(input == 'k')
		{			
			comm.mKi = comm.getValue(comm.fd);
			serialPrintf(comm.fd,"valueReceived\n");
			//printf("valueReceived\n");
		}
		
		// Accept Commanded Speed:
		else if(input == 'v')
		{			
			comm.mCmdSpeed = comm.getValue(comm.fd);
			serialPrintf(comm.fd,"valueReceived\n");
			//printf("valueReceived\n");
		}
		
		// Accept Set Speed:
		else if(input == 'b')
		{			
			int compare = comm.getValue(comm.fd);
			
			if(compare>comm.mCmdSpeed)
			{
				serialPrintf(comm.fd,"tooHigh\n");
				//printf("tooHigh\n");
			}
			else
			{
				comm.mSetSpeed = compare;
				serialPrintf(comm.fd,"good\n");
				//printf("good\n");
			}
		}
		
		// Accept Current Speed:
		else if(input == 'n')
		{			
			comm.mCurSpeed = comm.getValue(comm.fd);
			serialPrintf(comm.fd,"valueReceived\n");
			//printf("valueReceived\n");
		}
		
		// Accept Set Power:
		else if(input == 'p')
		{			
			comm.mCurPower = comm.getValue(comm.fd);
			serialPrintf(comm.fd,"valueReceived\n");
			//printf("valueReceived\n");
		}
		
		// Accept Commanded Authority:
		else if(input == 's')
		{			
			comm.mCmdAuthority = comm.getValue(comm.fd);
			serialPrintf(comm.fd,"valueReceived\n");
			//printf("valueReceived\n");
		}
		
		// Accept Current Authority:
		else if(input == 'd')
		{			
			comm.mCurAuthority = comm.getValue(comm.fd);
			serialPrintf(comm.fd,"valueReceived\n");
			//printf("valueReceived\n");
		}
		
		// Accept Current Beacon:
		else if(input == 'f')
		{
			comm.mCurBeacon = comm.getString(comm.fd);
			serialPrintf(comm.fd,comm.mCurBeacon.c_str());
			//printf("comm.mCurBeacon\n");
		}
		
		// Calculate Power:
		else if(input == 'o')
		{
			output = comm.calculatePower();
			serialPrintf(comm.fd,output);
			//printf(output);
		}
	}
}
