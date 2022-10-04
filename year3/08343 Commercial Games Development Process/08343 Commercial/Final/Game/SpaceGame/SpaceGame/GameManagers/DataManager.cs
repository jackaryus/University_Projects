// File Author: Daniel Masterson
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Manages data logging
    /// </summary>
    public class DataManager : AbstractManager
    {
        static StringBuilder dataBuffer = new StringBuilder();
        static float timeSinceLastOutput = 0;
        const float timeBeforeWrite = 0.2f;
        string outputFile = "";
        static bool isLogging = false;

        public DataManager()
        {
            Directory.CreateDirectory("DataOutput");
            outputFile = "DataOutput\\" + DateTime.Now.ToString("dd-MM-yyy_HH-mm-ss") + ".log";
        }

        public override void OnManagerUpdate(float delta)
        {
            timeSinceLastOutput += delta;
            if (dataBuffer.Length > 0 && timeSinceLastOutput > timeBeforeWrite)
            {
                File.AppendAllText(outputFile, dataBuffer.ToString());
                dataBuffer.Clear();
                timeSinceLastOutput = 0;
            }
        }

        /// <summary>
        /// Output data to the data log (Won't log if BeginLogging has not been called)
        /// </summary>
        /// <param name="data">The data to log</param>
        public static void Output(string data)
        {
            if (!isLogging)
                return;

            string toLog = DateTime.Now.ToString("dd-MM-yyy HH:mm:ss") + " - " + data;
            dataBuffer.AppendLine(toLog);
            Console.WriteLine(toLog);
            timeSinceLastOutput = 0;
        }

        /// <summary>Starts logging</summary>
        public static void BeginLogging()
        {
            isLogging = true;
        }

        /// <summary>Stops logging</summary>
        public static void EndLogging()
        {
            isLogging = false;
        }
    }
}
