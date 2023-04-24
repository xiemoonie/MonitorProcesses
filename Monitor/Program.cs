using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Timers;
using ProcessHandler;


namespace ProcessHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            MonitorProcess m = new MonitorProcess(new RealTimer(), new ProcessesHandler());
            if (m.ValidateArgs(args))
            {
                m.ValidateInput(args[0], args[1], args[2]);
            }
            else
            {
                Console.WriteLine("Invalid Arguments");
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}







