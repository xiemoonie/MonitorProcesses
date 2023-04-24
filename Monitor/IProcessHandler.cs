using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessHandler
{
    public interface IProcessHandler
    {
        bool KillProcess(int id);
        ProcessesStruct[] GetCurrentProcess(String name);
    }
}
