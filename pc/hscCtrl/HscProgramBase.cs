using System.Collections.Generic;

namespace hscCtrl
{
    /// <summary>
    /// Base class used for translating the script
    /// </summary>
    public abstract class HscProgramBase
    {
        private readonly List<int[]> commands = new List<int[]>();
        private const int WaitOpCode = 0x01;
        private const int SetPinOnOpCode = 0x02;
        private const int SetPinOffOpCode = 0x03;

        public IEnumerable<int[]> GetCommands()
        {
            Run(); //this will end up running the script and generating the list of commands which are sent to microcontroller
            return commands;
        }

        protected abstract void Run();

        protected void Wait(int delay)
        {
            commands.Add(new int[2] { WaitOpCode, delay });
        }

        protected void SetPinOn(int pin)
        {
            commands.Add(new int[2] { SetPinOnOpCode, pin });
        }

        protected void SetPinOff(int pin)
        {
            commands.Add(new int[2] { SetPinOffOpCode, pin });
        }
    }
}
