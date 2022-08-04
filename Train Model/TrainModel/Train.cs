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
        static public int nextID = 0;
        private int ID;
        private double currentSpeed;
        private double previousAcceleration;
        private double commandedSpeed;
        private double mass = 56.7 * 907.1850030836;
        private double powerCmd;
        public static double powerMax = 120000;
        private bool emergencyBrake;
        private bool serviceBrake;
        private bool engineFailure;
        private bool signalPickUp;
        private int cmdAuthority;
        private int currAuthority;
        private int passengers=0;
        private int crew=6;
        private bool interiorLights;
        private bool exteriorLights;
        private int cars = 5;
        private const int capacity = 74;
        private bool doorR;
        private bool doorL;
        private int temperature=74;
        private double timeTillNextBlock;
        private bool announcement;
        private string beaconMessage = "No beacon";
        private bool underground;

        private double blockDist;
        private double currDist;
        private double gradient;
        private int RailLine;
        private bool baby;

        private int blockID=0;

        private bool StationRight;
        private bool StationLeft;





        private const double maxForce = 18551.9333;
        private const double accelerationLimit = 0.5;
        private const double decelerationLimitService = -1.2;
        private const double decelerationLimitEmergency = -2.73;
        private const double velocityLimit = 19.4444;

        private const double samplePeriod = 0.001;

        private const double frictionCoefficient = 0.01;

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
            ID = nextID;
            nextID++;
            emergencyBrake = false;
            serviceBrake = false;
            engineFailure = false;
            signalPickUp = false;
            currentSpeed = 0;
            previousAcceleration = 0;
            interiorLights = false;
            exteriorLights = false;
            doorL = false;
            doorR = false;
            announcement = false;
            currAuthority = 0;
            underground = false;
            RailLine = 0; //0=Red
            baby = true;

        }

        public Train(int authority, int line)
        {
            ID = nextID;
            nextID++;
            emergencyBrake = false;
            serviceBrake = false;
            engineFailure = false;
            signalPickUp = false;
            currentSpeed = 0;
            previousAcceleration = 0;
            interiorLights = false;
            exteriorLights = false;
            doorL = false;
            doorR = false;
            announcement = false;
            cmdAuthority = authority;
            underground = false;
            RailLine = line;
            baby = true;
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

            if(power<0)
                toggleServiceBrake();


        }

        public double getPowerCmd()
        {
            return powerCmd;
        }

        public double getMass()
        {
            return mass/ 907.1850030836;
        }

        public void setCommandedSpeed( double s)
        {
            commandedSpeed = s;
        }

        public void setCommandedSpeedMPH(double s)
        {
            commandedSpeed = s / 2.23694;
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

        public void setCmdAuthority(int a)
        {
            cmdAuthority = a;
        }

        public int getCmdAuthority()
        {
            return cmdAuthority;
        }

        public int getCurrAuthority()
        {
            return currAuthority;
        }

        public string getBeacon()
        {
            return beaconMessage;
        }

        public bool getUnderground()
        {
            return underground;
        }

        public int getBlockID()
        {
            return blockID;
        }

        public double getForce()
        {
            double force=0;
            if (engineFailure)
            {
                force = 0;
            }

            else if(currentSpeed!=0)
            {
                force= powerCmd / currentSpeed;
            }
            else if (powerCmd == 0)
            {
                force= 0;
            }
            else
            {
                force= 1000;
            }

            force -= (mass * 9.81 * Math.Sin(Math.Atan(gradient/100)));//gravitational force
            
            if (currentSpeed > 0)
            {
                force -= (mass * 9.81 * Math.Cos(Math.Atan(gradient/100 ))) * frictionCoefficient;//friction force
            }
            
            return force;
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
                powerCmd = 0;
                if (Math.Abs(accelerationCalc) + decelerationLimitService > 0)
                {
                    if (accelerationCalc > 0)
                    {
                        return accelerationCalc + decelerationLimitService;
                    }
                    else
                        return accelerationCalc - decelerationLimitService;
                }
                else return decelerationLimitService;
            }
            else if (emergencyBrake)
            {
                powerCmd = 0;
                if (Math.Abs(accelerationCalc) + decelerationLimitEmergency > 0)
                {
                    if (accelerationCalc > 0)
                    {
                        return accelerationCalc + decelerationLimitEmergency;
                    }
                    else
                        return accelerationCalc - decelerationLimitEmergency;
                }
                else return decelerationLimitEmergency;
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
            double velocityCalc = currentSpeed + ((samplePeriod ) * (getAcceleration() + previousAcceleration));

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
            currDist += currentSpeed * samplePeriod;
            timeTillNextBlock = (blockDist - currDist)/currentSpeed;


        }

        public double getTimeTillNextBlock()
        {
            return timeTillNextBlock;
        }

        public int getAuthority()
        {
            return cmdAuthority;
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

        public void toggleInteriorLights()
        {
            interiorLights = !interiorLights;
        }

        public bool getInteriorLights()
        {
            return interiorLights;
        }

        public void toggleExteriorLights()
        {
            exteriorLights = !exteriorLights;
        }

        public bool getExteriorLights()
        {
            return exteriorLights;
        }

        public bool getLights()
        {
            return false;
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

        public void setAnnouncement(bool a)
        {
            announcement = a;
        }

        public bool getBaby()
        {
            return baby;
        }
        
        public void growUp()
        {
            baby = false;
        }
            
        public void setBlockInfo(TrackModel.Block b, int auth)
        {
            blockDist = b.mLength;
            gradient = b.mGrade*90;
            if (b.mPop > 0)
            {
                int min =  passengers/4;
                int max = 3*passengers/4;
                Random r = new Random();
                int leaveCount = r.Next(min,max);
                passengers -= leaveCount;

            }
            
           
            mass = 56.7 * 907.1850030836 + 65 * passengers + 65 * crew;
            underground = b.mUnderground;
          
            currDist = 0;
            cmdAuthority--;
            currAuthority++;

            if(signalPickUp)
                beaconMessage = b.mBeacon;
            else
                beaconMessage = "";

            if (cmdAuthority == 0)
            {
                cmdAuthority = auth;
                currAuthority = 0;
            }

            blockID = b.mblockNum;

            if (b.mstationSide.Contains("Left"))
            {
                StationLeft = true;
            }
            else
                StationLeft = false;
            if (b.mstationSide.Contains("Right"))
            {
                StationRight = true;
            }
            else
                StationRight = false;

        }

        public bool getStationRight()
        {
            return StationRight;
        }

        public bool getStationLeft()
        {
            return StationLeft;
            return StationLeft;
        }


        public int UpdatePassenger(int p)
        {
            int temp= passengers + p;
            int ret = 0;
            if (temp > capacity)
            {
                passengers = capacity;
                ret = temp - capacity;
            }
            else
                passengers += p;
            return ret;

        }

        public bool askForInfo()
        {
            if (currDist < blockDist)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void setBlockDist(double dist)
        {
            blockDist = dist;
            currDist = 0;

        }

        public void setBlockDistFM(double dist)
        {
            blockDist = dist / 3.280839;
            currDist = 0;

        }

        public double getBlockDist()
        {
            return blockDist;
        }
        
        public double getBlockDistMF(){
            return blockDist * 3.280839;
        }

        public double getRemainingDistMF()
        {
            return (blockDist-currDist) * 3.280839;
        }
        

        public int getLine()
        {
            return RailLine;
        }

        public String getLineName()
        {
            if (RailLine == 1)
                return "Green";
            else
                return "Red";
        }
        public int getID()
        {
            return ID;
        }

        public double getGrade()
        {
            return gradient;
        }

        public void setGrade(double g)
        {
            gradient = g;
        }

    }



    }

