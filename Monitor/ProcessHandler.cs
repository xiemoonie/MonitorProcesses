 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessHandler
{
    public struct ProcessesStruct
    {
        public string name;
        public int id;

        public ProcessesStruct(int id, string name)
        {
            this.name = name;
            this.id = id;
        }
    }
    public class ProcessesHandler : IProcessHandler
    {
        ProcessesStruct[] listProcesses;
        

        public ProcessesStruct[] GetCurrentProcess(String name)
        {
            int i = 0;
            Process[] p = Process.GetProcessesByName(name);
            listProcesses = new ProcessesStruct[p.Length];  
            foreach (Process process in p)
            {
                ProcessesStruct processS = new ProcessesStruct(process.Id, process.ProcessName);
                listProcesses[i] = processS;
                i++;

            }
            return listProcesses;
        }

        public bool KillProcess(int id)
        {
            Process p = Process.GetProcessById(id);
            if (p != null)
            {
                p.Kill();
                p.WaitForExit();
                return true;
            }
            else
            {
                return false;
            }
           
        }
        public int getIdProcess(String n)
        {
            int num = 0;

            foreach (ProcessesStruct l in listProcesses)
            {
                if (l.name == n)
                {
                    num = l.id;
                }
            }

            return num;

        }
    }
}
