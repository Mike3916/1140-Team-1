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
        private double k1;
        private double kP;
        private double mass;
        private double powerCmd;
        private double powerMax = 120000;
        private bool emergencyBrake;
        private bool serviceBrake;
        private int authority;




        private const double maxForce = 18551.9333;
        private const double accelerationLimit = 0.5;
        private const double decelerationLimitService = -1.2;
        private const double decelerationLimitEmergency = -2.73;
        private const double velocityLimit = 19.4444;

        private const double samplePeriod = 1 / 5;


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
            currentSpeed = 10;
            previousAcceleration = 0;


        }


        public void toggleEmergencyBrake()
        {
            emergencyBrake = !emergencyBrake;

        }

        public void toggleServiceBrake()
        {
            serviceBrake = !serviceBrake;
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

        public double getCurrentSpeed()
        {
            return currentSpeed;
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
            try
            {
                return powerCmd / currentSpeed;
            }
            catch (DivideByZeroException)
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




    }

}