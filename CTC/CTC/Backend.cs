using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Globalization;

using CTC;
namespace Backend

{
	public class Train
    {
		public static int numTrains;
		public int line;		//0 means red, 1 means green
		public string name;
		public DateTime ETD;    //DateTime saves time in format such that TimeSpan can be found later between ETD and ETA via: Timespan duration = ETA.Subtract(ETD)
		public DateTime ETA;	
		public int destination; //holds the block number of the destination
		public TimeSpan duration; //TimeSpan variable to keep track of time duration between ETD and ETA
        public double length = 0;  //The length of the route
        //public int authority;
        public double speed;
        public List<int> route = new List<int>(); //holds list of blockNumbers that the train passes through


        //red and green routes are set so they start at index 1 (my destinations are saved starting at index 0)
        int[] mredRoute = {77, 9, 8, 7, 6, 5, 4, 3, 2, 1, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 76, 75,
                                74, 73, 72, 33, 34, 35, 36, 37, 38, 71, 70, 69, 68, 67, 44, 45, 46, 47, 48, 49, 50, 51,
                                52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 52, 51, 50, 49, 48, 47, 46,
                                45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24,
                                23, 22, 21, 20, 19, 18, 17, 16, 1, 2, 3, 4, 5, 6, 7, 8, 9, 77 };


        int[] mgreenRoute = {151, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
                                82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 85, 84,
                                83, 82, 81, 80, 79, 78, 77, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111,
                                112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128,
                                129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145,
                                146, 147, 148, 149, 150, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15,
                                14, 13, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21,
                                22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42,
                                43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 151 };


        public void calcDuration() //Calculates time between ETD and ETA and saves into "duration" variable
        {
            duration = ETA.Subtract(ETD); //This calculates the span of time between departure and arrival
        }
        public void calcRoute()
        {
            int maxAuthority;
            int tempAuthority;

            int i = 0; //Start at 0
            if (line == 0) //The red line   [mRedAuthorities is 0 through 76]
            {
                while (true)
                {
                    route.Add(mredRoute[i]);
                    length += ((MainWindow)Application.Current.MainWindow).mLines[line].GetBlock(mredRoute[i]).mLength; //Add the length of the block. GetBlock() is sent the Block ID (starts at 1), not block index
                    if (destination == mredRoute[i])
                        break;
                    i++; //increment i and add info to document, so it's already added by the time the while loop ends. (i=-1 before the while loop starts)
                }

                maxAuthority = route.Count - 1; //If there are 5 blocks in the route, we want the authority to be 0 on the fifth block, so the max authority should be 4, or 1 less than the number of block in the route
                tempAuthority = maxAuthority;  //This is the temporary authority, it will decrease by 1 as authorities are consecutively assigned
                
                for (i=0; i<= maxAuthority; i++)
                {
                    ((MainWindow)Application.Current.MainWindow).mRedAuthorities[route[i] - 1] = tempAuthority; //The route values are 1-indexed, but the redAuthorities are 0-indexed, so subtract one
                    MessageBox.Show(((MainWindow)Application.Current.MainWindow).mRedAuthorities[route[i] - 1].ToString());
                    tempAuthority--; //Decrease the authority for each iteration
                }
            }
            else if (line == 1) //The green line
            {
                while (true)
                {
                    route.Add(mgreenRoute[i]);
                    length += ((MainWindow)Application.Current.MainWindow).mLines[line].GetBlock(mgreenRoute[i]).mLength; //Add the length of the block. GetBlock() is sent the Block ID (starts at 1), not block index
                    if (destination == mgreenRoute[i])
                        break;
                    i++;
                }
                maxAuthority = route.Count - 1; //If there are 5 blocks in the route, we want the authority to be 0 on the fifth block, so the max authority should be 4, or 1 less than the number of block in the route
                tempAuthority = maxAuthority;  //This is the temporary authority, it will decrease by 1 as authorities are consecutively assigned

                for (i = 0; i <= maxAuthority; i++)
                {
                    ((MainWindow)Application.Current.MainWindow).mGreenAuthorities[route[i] - 1] = tempAuthority; //The route values are 1-indexed, but the redAuthorities are 0-indexed, so subtract one
                    tempAuthority--; //Decrease the authority for each iteration
                }
            }

            speed = length / duration.TotalSeconds; //This will give speed in meters per second
        }
    }

	public class CTCObject
	{

		public CTCObject()
		{
		}


	}
}