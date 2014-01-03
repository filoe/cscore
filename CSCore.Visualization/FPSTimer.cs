using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSCore.Visualization
{
    public class FPSTimer
    {
        private Stopwatch _stopWatch;

        private int _interval;

        public int Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _interval = value;
            }
        }

        public FPSTimer(int fps)
        {
            Interval = (int)(1000.0 / fps);
            _stopWatch = new Stopwatch();
        }

        public void Start()
        {
            _stopWatch.Start();
        }

        public bool Update()
        {
            if (_stopWatch.ElapsedMilliseconds < _interval)
            {
                return false;
            }
            else
            {
                _stopWatch.Reset();
                _stopWatch.Start();
                return true;
            }
        }

        public void UpdateSleep()
        {
            if (_stopWatch.ElapsedMilliseconds < _interval)
            {
                System.Threading.Thread.Sleep((int)Math.Max(_interval - _stopWatch.ElapsedMilliseconds, 0));
            }
            _stopWatch.Reset();
            _stopWatch.Start();
        }
    }
}