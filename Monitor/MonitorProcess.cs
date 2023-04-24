using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Timers;
using ProcessHandler;
using System.Linq;

namespace ProcessHandler
{
    public class MonitorProcess
    {
        ITimer t;
        IProcessHandler p;

        IDictionary<int, int> dicProcesses = new Dictionary<int, int>();

        public MonitorProcess(ITimer t, IProcessHandler p)
        {
            this.t = t;
            this.p = p;
        }

        public void UpdatedLifeProcess(int frecuency)
        {
            foreach (var process in dicProcesses.Keys.ToList())
            {
                if (dicProcesses[process] <= 0)
                {
                    p.KillProcess(process);
                    dicProcesses.Remove(process);
                }
                else
                {
                    dicProcesses[process] = dicProcesses[process] - frecuency;
                }
            }
        }

        public void TimeUp(int idToKill, DateTime signalTime)
        {
            Console.WriteLine("Process killed at {0:HH:mm:ss.fff}",
                              signalTime);
            p.KillProcess(idToKill);
        }

        public void AddtoDictionary(int id, int life)
        {
            if (!dicProcesses.ContainsKey(id))
            {
                dicProcesses.Add(id, life);
            }
        }

        public void FindsProcess(String name, int time)
        {
            ProcessesStruct[] processes = p.GetCurrentProcess(name);

            if (processes.Length > 0)
            {
                foreach (ProcessesStruct p in processes)
                {
                    AddtoDictionary(p.id, time);
                }
            }
        }

        public void UpdateDictionary(String name)
        {
            ProcessesStruct[] processes = p.GetCurrentProcess(name);

            if (processes.Length < dicProcesses.Count)
            {
                if (processes.Length == 0)
                {
                    dicProcesses.Clear();
                }
                else
                {
                    foreach (ProcessesStruct p in processes)
                    {
                        if (dicProcesses.ContainsKey(p.id) == false)
                        {
                            dicProcesses.Remove(p.id);

                        }
                    }
                }
            }
        }

        public void RunMonitor(String name, int life, int frecuency)
        {
            t.OnTick += () =>
            {
                CheckForPreviousProcesses(name, life, frecuency);
            };
            t.Interval = frecuency * 60000;

            t.AutoReset = true;

            t.Start();
            CheckForPreviousProcesses(name, life, frecuency);
        }

        public void CheckForPreviousProcesses(String name, int life, int frecuency)
        {

            Console.WriteLine("Timer");
            UpdateDictionary(name);
            FindsProcess(name, life);
            UpdateDictionary(name);
            UpdatedLifeProcess(frecuency);
            //t.Start();
        }

        public bool ValidStringInput(string processName)
        {

            if (Regex.Match(processName, "^[a-zA-Z0-9-_]+$").Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public int ValidIntInput(string minutesTime)
        {
            int number = 0;

            if (int.TryParse(minutesTime, out number))
            {
                // Console.WriteLine(number);
            }
            else
            {
                //  Console.WriteLine("Unable to parse string.");
            }

            return number;
        }

        public bool ValidateInput(String n, String m, String f)
        {
            int maxLife = ValidIntInput(m);
            int frecuency = ValidIntInput(f);
            if (ValidStringInput(n) && ( maxLife > 0 && maxLife <= 60) && ( frecuency > 0 && frecuency <= 60))
            {
                RunMonitor(n, maxLife, frecuency);
                return true;

            }
            else
            {
                return false;
                //  Console.WriteLine("Error");
            }
        }
        public bool ValidateArgs(string[] args)
        {

            if (args.Length == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
