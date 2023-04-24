using NUnit.Framework;
using ProcessHandler;

namespace Monitor.Test
{
    [TestFixture]
    public class MonitorProcessTest
    {
        private MonitorProcess monitor;
        private TimerTest timerTest;
        private TestProcessHandler handler;

        [SetUp]
        public void SetUp()
        {
            handler = new TestProcessHandler();
            timerTest = new TimerTest();
            monitor = new MonitorProcess(timerTest, handler); 
        }

        [TestCase]
        public void Args_ReturnTrue()
        {
            Assert.IsTrue(monitor.ValidateArgs(new string[] { "3", "3", "3" }));
        }

        [TestCase]
        public void ValidateArgs_ReturnFalse()
        {
            Assert.IsFalse(monitor.ValidateArgs(new string[] {"2", "2" }));
            Assert.IsFalse(monitor.ValidateArgs(new string[] {"1" }));
            Assert.IsFalse(monitor.ValidateArgs(new string[] {"4", "4", "4", "4" }));
        }

        [TestCase("notepad", "1", "1")]
        public void ValidateInput_ReturnTrue(String s, String m, String f)
        {
            bool result = monitor.ValidateInput(s, m, f);
            Assert.IsTrue(result, "true");
        }

        [TestCase("", "2", "1")]
        [TestCase("notepad", "61", "1")]
        [TestCase("notepad", "1", "61")]
        [TestCase("notepad", "1", "0")]
        [TestCase("notepad", "0", "1")]
        public void ValidateInput_ReturnFalse(String s, String m, String f)
        {
            bool result = monitor.ValidateInput(s, m, f);
            Assert.IsFalse(result, "false");
        }

        [TestCase("notepad")]
        [TestCase("notepad45")]
        public void Test_ValidateString_ReturnTrue(String s)
        {
            bool result = monitor.ValidStringInput(s);
            Assert.IsTrue(result, "true");
        }

        [TestCase("")]
        [TestCase("!")]
        [TestCase("    ")]
        [TestCase("notepad.")]
        public void ValidateString_ReturnFalse(String s)
        {
            bool result = monitor.ValidStringInput(s);
            Assert.IsFalse(result, "false");
        }

        [TestCase("45")]
        public void ValidateInt_ReturnTrue(String s)
        {
            int num = monitor.ValidIntInput(s);
            bool result = false;
            if (num > 0 || num < 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            Assert.IsTrue(result, "true");
        }

        [TestCase("")]
        public void ValidateInt_ReturnFalse(String s)
        {
            int num = monitor.ValidIntInput(s);
            bool result = false;
            if (num == 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            Assert.IsFalse(result, "false");
        }

        [Test]
        public void Monitor_Correct()
        {
            Assert.That(handler.listRequestedCount, Is.EqualTo(0));
            monitor.ValidateInput("target", "3", "1");
            Assert.That(handler.listRequestedCount, Is.GreaterThan(0));
           

            timerTest.Tick();
         

            handler.addFakeProcess(new ProcessesStruct(1, "target"));

            timerTest.Tick(); // Here it is discovered 0
            timerTest.Tick(); //  1

            timerTest.Tick(); //  2

            Assert.That(handler.lastKilledProcessId, Is.Not.EqualTo(1));

            timerTest.Tick(); // Killed 3

            Assert.That(handler.lastKilledProcessId, Is.EqualTo(1));

            timerTest.Tick(); //  4 Should not be called anymore
        }

        [Test]
        public void Monitor_Frequecy_Is_Bigger()
        {
            monitor.ValidateInput("target", "1", "3");
         
            timerTest.Tick();
      
            handler.addFakeProcess(new ProcessesStruct(1, "target"));

            timerTest.Tick(); // Here it is discovered 0
            Assert.That(handler.lastKilledProcessId, Is.Not.EqualTo(1));
            timerTest.Tick(); // 3 - More than 1 - Killing
            Assert.That(handler.lastKilledProcessId, Is.EqualTo(1));

            timerTest.Tick(); // 6 
            timerTest.Tick(); // 9
        }

        [Test]
        public void Monitor_OpenCloseProcess()
        { 
            monitor.ValidateInput("target", "3", "1");
           
            timerTest.Tick();
      
            handler.addFakeProcess(new ProcessesStruct(1, "target"));// Begin and end process
            handler.removeFakeProcess(new ProcessesStruct(1, "target"));

            timerTest.Tick(); // Here it is discovered 0
            Assert.That(handler.lastKilledProcessId, Is.EqualTo(-1));
        }

        [Test]
        public void Multi_Processes()
        {
            monitor.ValidateInput("target", "3", "1");
           
            timerTest.Tick(); // 0
         
            handler.addFakeProcess(new ProcessesStruct(1, "target")); // AddProcess

            timerTest.Tick(); // 1
            timerTest.Tick(); // 2 

            handler.addFakeProcess(new ProcessesStruct(2, "target")); // AddProcess
            handler.addFakeProcess(new ProcessesStruct(3, "target")); // AddProcess

            timerTest.Tick(); // 3
            timerTest.Tick(); // 4
            Assert.That(handler.lastKilledProcessId, Is.EqualTo(1));

            timerTest.Tick(); // 5
            timerTest.Tick(); // 6
            Assert.That(handler.lastKilledProcessId, Is.EqualTo(3));

            handler.addFakeProcess(new ProcessesStruct(4, "target"));// Begin and end process
         
            timerTest.Tick(); // 7
            timerTest.Tick(); // 8
            
            handler.removeFakeProcess(new ProcessesStruct(4, "target"));
            
            timerTest.Tick(); // 9
            timerTest.Tick(); // 9
            timerTest.Tick(); // 10
            Assert.That(handler.lastKilledProcessId, Is.EqualTo(3));
        }
    }
}