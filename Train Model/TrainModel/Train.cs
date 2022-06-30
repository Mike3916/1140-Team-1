/* Howard Malc
 * 6/26/22
 * Adopted in part from https://github.com/aet37/ECE1140-Project/blob/master/src/TrainModel/TrainCatalogue.py
 * With assistance from github copilot
 */

using System;


namespace TrainObject
{
    public class Train
    {
        private double currentSpeed;
        private double previousAcceleration;
        private double commandedSpeed;
        private double mass = 56.7 * 2000;
        private double powerCmd;
        public static double powerMax = 120000;
        private bool emergencyBrake;
        private bool serviceBrake;
        private bool engineFailure;
        private bool signalPickUp;
        private int authority;
        private int passengers=58;
        private int crew=6;
        private bool lights;
        private int cars = 5;
        private const int capacity = 74;
        private bool doorR;
        private bool doorL;
        private int temperature=74;
   
        




        private const double maxForce = 18551.9333;
        private const double accelerationLimit = 0.5;
        private const double decelerationLimitService = -1.2;
        private const double decelerationLimitEmergency = -2.73;
        private const double velocityLimit = 19.4444;

        private const double samplePeriod = 2;


        /* self.MAX_FORCE = 18551.9333
            self.GRAVITY = 9.8
            self.FRICTION_COEFFICIENT = 0.01
            self.ACCELERATION_LIMIT = 0.5
            self.DECELERATION_LIMIT_SERVICE = -1.2
            self.DECELERATION_LIMIT_EMERGENCY = -2.73
            self.VELOCITY_LIMIT = 19.4444
        */

        //Power=F*V
        //F=Ma

        public Train()
        {
            emergencyBrake = false;
            serviceBrake = false;
            engineFailure = false;
            signalPickUp = false;
            currentSpeed = 0;
            previousAcceleration = 0;
            lights = false;


        }


        public void toggleEmergencyBrake()
        {
            emergencyBrake = !emergencyBrake;

        }

        public bool getEmergencyBrake()
        {
            return emergencyBrake;
        }
        

        public void toggleServiceBrake()
        {
            serviceBrake = !serviceBrake;
        }

        public bool getServiceBrake()
        {
            return serviceBrake;
        }

        public void toggleEngineFailure()
        {
            engineFailure = !engineFailure;
        }

        public bool getEngineFailure()
        {
            return engineFailure;
        }

        public void toggleSignalPickUp()
        {
            signalPickUp = !signalPickUp;
        }

        public bool getSignalPickUp()
        {
            return signalPickUp;
        }

        public void setPowerCmd(double power)
        {
            powerCmd = power;
            if (powerCmd > powerMax)
            {
                powerCmd = powerMax;
            }
          

        }

        public double getPowerCmd()
        {
            return powerCmd;
        }

        public double getMass()
        {
            return mass/2000* 1.10231;
        }

        public void setCommandedSpeed( double s)
        {
            commandedSpeed = s;
        }

        public double getCommandedSpeed() { 
            return commandedSpeed; 
        }

        public double getCommandedSpeedMPH()
        {
            return commandedSpeed * 2.23694;
        }

        public double getCurrentSpeed()
        {
            return currentSpeed;
        }

        public double getCurrentSpeedMPH()
        {
            return Math.Round(currentSpeed * 2.23694,2);
        }

        public void setAuthority(int a)
        {
            authority = a;
        }

        public int getAuthority()
        {
            return authority;
        }

        public double getForce()
        {
            if (engineFailure)
            {
                powerCmd = 0;
            }

            if(currentSpeed!=0)
            {
                return powerCmd / currentSpeed;
            }
            else if (powerCmd == 0)
            {
                return 0;
            }
            else
            {
                return 1000;
            }
        }

        public double getAcceleration()
        {
            double accelerationCalc = getForce() / mass;

            if (accelerationCalc > accelerationLimit && !emergencyBrake && !serviceBrake)
            {//if acceleration greater than limit and no brakes on
                return accelerationLimit;
            }
            else if (serviceBrake && !emergencyBrake)
            {
                return decelerationLimitService;
            }
            else if (emergencyBrake)
            {
                return decelerationLimitEmergency;
            }
            else
            {
                
                return accelerationCalc;
            }

        }

        public double getAccelerationFPS()
        {
            return Math.Round(getAcceleration() * 3.280839,2);
        }

        public double getVelocity()
        {
            double velocityCalc = currentSpeed + ((samplePeriod / 2) * (getAcceleration() + previousAcceleration));

            if (velocityCalc > velocityLimit)
                return velocityLimit;
            else if (velocityCalc <= 0)
                return 0;
            else
                return velocityCalc;
        }
        
        public void increment()
        {
            previousAcceleration = getAcceleration();
            currentSpeed = getVelocity();
        }




        public int getPassengers()
        {
            return passengers;
        }
        public int getCrew()
        {
            return crew;
        }

        public void setPassengers(int p)
        {
            passengers = p;
        }

        public void setCrew(int c)
        {
            crew = c;
        }

        public void toggleLights()
        {
            lights = !lights;
        }

        public bool getLights()
        {
            return lights;
        }

        public int getCars()
        {
            return cars;
        }

        public void setCars(int c)
        {
            cars = c;
        }

        public int getCapacity()
        {
            return capacity;
        }

        public void toggleDoorR()
        {
            doorR = !doorR;
        }

        public bool getDoorR()
        {
            return doorR;
        }

        public void toggleDoorL()
        {
            doorL = !doorL;
        }

        public bool getDoorL()
        {
            return doorL;
        }

        public int getTemperature()
        {
            return temperature;
        }

        public void setTemperature(int t)
        {
            temperature = t;
        }
        
        
    }

}