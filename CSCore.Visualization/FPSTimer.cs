using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSCore.Visualization
{
    public class FPSTimer
    {
        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint timeGetTime();

        int _interval;
        uint msSafe = 0;
        public int Interval { get { return _interval; } }
        public FPSTimer(int interval)
        {
            _interval = interval;
        }

        public void Start()
        {
            msSafe = timeGetTime();
        }

        public bool Update()
        {
            uint time = timeGetTime();
            if (time - msSafe < _interval)
            {
                return false;
            }
            else
            {
                msSafe = time;
                return true;
            }
        }

        public void UpdateSleep()
        {
            uint time = timeGetTime();
            if (time - msSafe < _interval)
            {
                System.Threading.Thread.Sleep(_interval - (int)(time - msSafe));
            }
            msSafe = timeGetTime();
        }
    }
}
