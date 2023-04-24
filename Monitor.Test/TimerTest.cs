using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Monitor.Test
{
    internal class TimerTest : ITimer
    {
        bool enabled = false;
        double interval = 1;
        bool autoreset = false;

        public double Interval { get => interval; set => interval = value; }
        public bool Enabled { get => enabled; set => enabled = value; }
        public bool AutoReset { get => autoreset; set => autoreset = value; }

        public event TimerEvent OnTick;

        public void Dispose()
        {
            
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
        
        }

        public void Tick()
        {
            OnTick();
        }
    }
}
