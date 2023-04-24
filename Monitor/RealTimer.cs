using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ProcessHandler
{
    public class RealTimer : System.Timers.Timer, ITimer
    {
        public event TimerEvent OnTick;

        public RealTimer()
        {
            Elapsed += WatchDogTimer_Elapsed;
        }

        private void WatchDogTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            OnTick();
        }
    }

}
