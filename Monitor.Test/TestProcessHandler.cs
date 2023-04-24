using ProcessHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Test
{
    internal class TestProcessHandler : IProcessHandler
    {
        List<ProcessesStruct> fakeProcesses = new List<ProcessesStruct>();
        public int lastKilledProcessId = -1;
        public int listRequestedCount = 0;

        public ProcessesStruct[] GetCurrentProcess(string name)
        {
            listRequestedCount++;
            return fakeProcesses.ToArray();
        }
        public bool KillProcess(int id)
        {
            Console.WriteLine($"Killing process with ID: {id}");
            fakeProcesses.Remove(GetProcessByID(id));
            lastKilledProcessId = id;
            return true;
        }

        private ProcessesStruct GetProcessByID(int id)
        {
            int index = -1;
            for (int i = 0; i < fakeProcesses.Count; i++)
            {
                if (fakeProcesses[i].id == id)
                {
                    index = i;
                }
            }
            if (index == -1) throw new KeyNotFoundException($"Process with id {id} was not found");

            return fakeProcesses[index];
        }

        public void addFakeProcess(ProcessesStruct process)
        {
            fakeProcesses.Add(process);
        }

        public void removeFakeProcess(ProcessesStruct process)
        {
            fakeProcesses.Remove(process);
        }
    }
}
