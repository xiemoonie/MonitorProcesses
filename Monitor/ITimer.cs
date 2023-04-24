using System.Timers;

public interface ITimer
{
    event TimerEvent OnTick;

    double Interval { get; set; }
    bool Enabled { get; set; }
    bool AutoReset { get; set; }

    void Dispose();

    void Start();

    void Stop();
}
public delegate void TimerEvent();