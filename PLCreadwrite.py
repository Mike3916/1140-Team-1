#!/usr/bin/env python3
try:
	import time
except:
	print("ERROR: Python package 'time' is not installed")
	exit()
try:
	import serial
except:
	print("ERROR: Python package 'serial' is not installed")
	exit()
try:		
	import PLC
except:
	print("ERROR: Could not find PLC.py")
	exit()

serRead = serial.Serial(
	port='/dev/ttyS0',
	baudrate = 230400,
	parity=serial.PARITY_NONE,
	stopbits=serial.STOPBITS_ONE,
	bytesize=serial.EIGHTBITS,
	timeout=1
)

serWrite = serial.Serial(
	port='/dev/ttyS0',
	baudrate = 230400,
	parity=serial.PARITY_NONE,
	stopbits=serial.STOPBITS_ONE,
	bytesize=serial.EIGHTBITS,
	timeout=1
    )

print("Waiting for input...")

while 1:
	received=serRead.read(11)
	
	if received != b'':
		print("\nBytes received:")
		print(received)
		receivedBool=[False]*86
		output = bytearray(13)
	
		for i in range(0,86):
			if (received[int(i / 8)] & (1<<(i % 8))) != 0:
				receivedBool[i] = True
			else:
				receivedBool[i] = False
		print("\nTrack occupancies:")
		print(receivedBool)
		mSwitches, mRightLights, mLeftLights = PLC.PLC(receivedBool)	
		outputBool = mRightLights + mLeftLights + mSwitches
		print("\nOutputs of PLC:")
		print(outputBool)
		for i in range (0,98):
			if outputBool[i]:
				output[int(i / 8)] = (output[int(i / 8)] | (1<<(i % 8)))
		print("\nBytes sent:")
		print(output)
		serWrite.write(bytes(output))
		print("\n\nWaiting for input...")