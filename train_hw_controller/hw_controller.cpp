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
		bool mAutoMode=false;
		bool mLeftDoors=false;
		bool mRightDoors=false;
		bool mInteriorLights=false;
		bool mExteriorLights=false;
		bool mAnnouncementsStatus=false;
		
		int mTemperature=72;
		int mKp=0;
		int mKi=0;
		
		int mCmdSpeed=0;
		int mSetSpeed=0;
		int mCurSpeed=0;
		
		int mCmdAuthority=0;
		int mCurAuthority=0;
		int fd;
		
		serialConnect()
		{
			if((fd=serialOpen("/dev/ttyS0",115200))<0)
			{
			  fprintf(stderr,"Unable to open serial device: %s\n",strerror(errno));
			}
		}
		
		void displayValues()
		{
			cout << "\n\n-------------------------------------------------------\n"
				 << "mEmergencyBrakeStatus = " << mEmergencyBrakeStatus << "\n"
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
				 << "mCmdAuthority = " << mCmdAuthority << "\n"
				 << "mCurAuthority = " << mCurAuthority << "\n-------------------------------------------------------\n\n";
		}
		
		void resetValues()
		{
			// Vital bools:
			mEmergencyBrakeStatus = false;
			mAutoMode = false;
			
			// Non-vital bools:
			mLeftDoors = false;
			mRightDoors = false;
			mInteriorLights = false;
			mExteriorLights = false;
			mAnnouncementsStatus = false;
			
			// Misc Ints:
			mTemperature = 72;
			mKp = 0;
			mKi = 0;
			
			// Speed:
			mCmdSpeed = 0;
			mSetSpeed = 0;
			mCurSpeed = 0;
			
			// Authority:
			mCmdAuthority = 0;
			mCurAuthority = 0;
		}
		
		const char* toggleBool(bool &status,char identifier)
		{
			status = !status;
			switch(identifier)
			{
				case 'e': 
					return status==false ? "Off\n" : "On\n";
				case 'm':
					return status==true ? "Auto\n" : "Manual\n";
				case 'l':
					return status==true ? "Open\n" : "Closed\n";
				case 'r':
					return status==true ? "Open\n" : "Closed\n";
				case 'i':
					return status==true ? "On\n" : "Closed\n";
				case 'x':
					return status==true ? "On\n" : "Closed\n";
				case 'a':
					return status==true ? "On\n" : "Closed\n";
			}
			return "";
		}
		
		const char* temperature(int &value,char identifier)
		{
			switch(identifier)
			{
				case 'h':
					value++;
					return "Temp_Increased\n";
				case 'c':
					value--;
					return "Temp_Decreased\n";
			}
			return ""; 
		}
		
		int getValue(int fd)
		{
			int numChar; 
			string str;
					
			for (int i=0;i<5;i++)
			{
				numChar = serialGetchar(fd);
				
				if(numChar>47 && numChar <58)
				{
					str += numChar;
				}
			}
				
			return stoi(str);
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
		}
		
		// Accept Ki:
		else if(input == 'k')
		{			
			comm.mKi = comm.getValue(comm.fd);
		}
		
		// Accept Commanded Speed:
		else if(input == 'v')
		{			
			comm.mCmdSpeed = comm.getValue(comm.fd);
		}
		
		// Accept Set Speed:
		else if(input == 'b')
		{			
			comm.mSetSpeed = comm.getValue(comm.fd);
		}
		
		// Accept Current Speed:
		else if(input == 'n')
		{			
			comm.mCurSpeed = comm.getValue(comm.fd);
		}
	}
}
