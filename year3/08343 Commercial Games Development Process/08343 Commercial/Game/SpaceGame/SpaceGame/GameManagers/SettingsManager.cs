// File Author: Jack Hoyle
// Contributors: Daniel Masterson
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceGame.GameManagers
{
    /// <summary>
    /// Manages game settings
    /// </summary>
    public class SettingsManager : AbstractManager
    {
        static Dictionary<string, string> settings = new Dictionary<string, string>();

        public SettingsManager()
        {
            string[] lines = File.ReadAllLines("settings.cfg");

            foreach(string sLine in lines)
            {
                string line = sLine.Trim();

                if (line == "" || line[0] == '#')
                    continue;

                int eqIndex = line.IndexOf('=');

                if (eqIndex <= 0)
                {
                    Console.WriteLine("Invalid settings line '" + line + "'. Skipping.");
                    continue;
                }

                string[] splLine = line.Split(new char[] { '=' }, 2);

                settings.Add(splLine[0].Trim().ToLower(), splLine[1].Trim());
            }
        }

        public static T GetSetting<T>(string settingName)
        {
            settingName = settingName.ToLower().Replace(' ', '_');

            if(settings.ContainsKey(settingName))
            {
                Type t = typeof(T);
                try
                {
                    TypeConverter tc = TypeDescriptor.GetConverter(t);
                    return (T)tc.ConvertFromString(settings[settingName]);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Could not get setting '" + settingName + "' of type '" + t.ToString() + "'.");
                    Console.WriteLine(e.ToString());
                }
            }

            Console.WriteLine("Unknown setting '" + settingName + "'");
            return default(T);
        }
    }
}
