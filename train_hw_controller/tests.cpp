#include <stdio.h>
#include <string.h>
#include <errno.h>
#include <wiringSerial.h>
#include <string>
#include <iostream>

using namespace std;

int intToAscii(int number) 
{
   return '0' + number;
}

int main()
{
    string input = "\n\n\n\n10";
			
    int mKp = stoi(input);
    
    cout << mKp;
}
