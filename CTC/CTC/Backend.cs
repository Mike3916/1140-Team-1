using System;
using System.IO;
using System.Globalization;


namespace Backend
{
	public class Train
    {
		public static int numTrains;
		public int line;		//0 means red, 1 means green
		public string name;
		public DateTime ETD;    //DateTime saves time in format such that TimeSpan can be found later between ETD and ETA via: Timespan duration = ETA.Subtract(ETD)
		public DateTime ETA;	
		public int destination;
		public TimeSpan duration; //TimeSpan variable to keep track of time duration between ETD and ETA

		public void calcDuration() //Calculates time between ETD and ETA and saves into "duration" variable
        {
			duration = ETA.Subtract(ETD); //This calculates the span of time between departure and arrival
        }
    }

	public class Route
    {
		public Route() { }


    }

	public class CTCObject
	{

		private Route[] routes;

		public CTCObject()
		{
		}


		public void router()
        {

        }

	}
}