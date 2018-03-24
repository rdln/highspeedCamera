using System.Collections.Generic;

namespace hscCtrl
{
    public abstract class HscProgramBase
    {
        private readonly List<int[]> commands = new List<int[]>();
        private const int WaitOpCode = 0x01;
        private const int SetPinOnOpCode = 0x02;
        private const int SetPinOffOpCode = 0x03;

        public IEnumerable<int[]> GetCommands()
        {
            Run();
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
