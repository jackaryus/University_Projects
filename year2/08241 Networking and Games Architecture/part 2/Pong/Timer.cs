using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
	public static class Timer
    {
        static DateTime mLastTime;

        static public void Start()
        {
            mLastTime = DateTime.Now;
        }

        static public float GetElapsedSeconds()
        {
            DateTime now = DateTime.Now;
            TimeSpan elasped = now - mLastTime;
            mLastTime = now;
            return (float)elasped.Ticks / TimeSpan.TicksPerSecond;
        }
    }
}


