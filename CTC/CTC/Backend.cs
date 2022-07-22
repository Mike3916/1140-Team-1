using System;
using System.IO;
using System.Globalization;


namespace Backend
{
	public class Train
    {
		public static int numTrains;
		public string line { get; set; }
		public string name { get; set; }
		public TimeSpan ETA { get; set; } //Timespan saves time in form: "02:45:13"
		public int destination { get; set; }

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