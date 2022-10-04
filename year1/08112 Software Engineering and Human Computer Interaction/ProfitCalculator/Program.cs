using System;
using System.IO;

namespace ProfitCalculator
{

    public class RangeFinder
    {
        /// <summary> 
        /// Performs a search to find the start and end of the "best" period and the 
        /// total amount of sales over that period. 
        /// </summary> 
        /// <param name="data">the data to be examined</param> 
        /// <param name="bestStart">the start point found by the search</param> 
        /// <param name="bestEnd">the end point found by the search</param> 
        /// <param name="bestTotal">the total sales over that period</param> 
        /// <param name="loops">the number of executions of the inner loop</param> 
        public static void Search1(double[] data, out int bestStart, out int bestEnd, out double bestTotal, out int loops)
        {
            bestTotal = 0;
            bestStart = 0;
            bestEnd = 0;
            loops = 0;
            double subtotal;
            /// TODO - put your search code here
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    subtotal = 0;
                    for (int k = i; k <= j; k++)
                    {
                        loops++;
                        subtotal += data[k];
                    }

                    if (subtotal > bestTotal)
                    {
                        bestTotal = subtotal;
                        bestStart = i;
                        bestEnd = j;
                    }
                }
            }
        }

        public static void Search2(double[] data, out int bestStart, out int bestEnd, out double bestTotal, out int loops)
        {
            bestTotal = 0;
            bestStart = 0;
            bestEnd = 0;
            loops = 0;
            double subtotal;
            /// TODO - put your search code here
            for (int i = 0; i < data.Length; i++)
            {
                subtotal = 0;
                for (int j = i; j < data.Length; j++)
                {
                    loops++;
                    subtotal += data[j];

                    if (subtotal > bestTotal)
                    {
                        bestTotal = subtotal;
                        bestStart = i;
                        bestEnd = j;
                    }
                }
            }
        }

        public static void Search3(double[] data, out int bestStart, out int bestEnd, out double bestTotal, out int loops)
        {
            bestTotal = 0;
            bestStart = 0;
            bestEnd = 0;
            loops = 0;
            double subtotal;
            int start = 0;
            /// TODO - put your search code here
            subtotal = 0;
            for (int i = 0; i < data.Length; i++)
            {
                loops++;
                subtotal += data[i];

                if (subtotal > bestTotal)
                {
                    bestTotal = subtotal;
                    bestStart = start;
                    bestEnd = i;
                }
                if (subtotal < 0)
                {
                    start = i + 1;
                    subtotal = 0;
                }
            }            
        }
    }

    

    /// <summary> 
    /// Tests the Profits Calculator 
    /// </summary> 
    class Test
    {
        /// <summary> 
        /// The main entry point for the application. 
        /// </summary> 
        static void Main()
        {
            double[] data;
            int bestStart, bestEnd;
            double bestTotal;
            int loops;

            /// name of the file and the number of readings 
            string filename = "week208.txt";
            int items = 208;

            data = new double[items];

            try
            {
                TextReader textIn = new StreamReader(filename);
                for (int i = 0; i < items; i++)
                {
                    string line = textIn.ReadLine();
                    data[i] = double.Parse(line);
                }
                textIn.Close();
            }
            catch
            {
                Console.WriteLine("File Read Failed");
                return;
            }

            RangeFinder.Search1(data, out bestStart, out bestEnd, out bestTotal, out loops);
            Console.WriteLine("Start : {0} End : {1} Total {2} Loops {3}", bestStart, bestEnd, bestTotal, loops);
            RangeFinder.Search2(data, out bestStart, out bestEnd, out bestTotal, out loops);
            Console.WriteLine("Start : {0} End : {1} Total {2} Loops {3}", bestStart, bestEnd, bestTotal, loops);
            RangeFinder.Search3(data, out bestStart, out bestEnd, out bestTotal, out loops);
            Console.WriteLine("Start : {0} End : {1} Total {2} Loops {3}", bestStart, bestEnd, bestTotal, loops);
            Console.ReadLine();
        }
    }
}
