using System;

namespace hscCtrl.Script
{
    /// <summary>
    /// Base class used for translating the script
    /// </summary>
    public abstract class HscProgramBase
    {
        private readonly CommandsBatch commands = new CommandsBatch();

        public CommandsBatch GetCommands()
        {
            Run(); //this will end up running the script and generating the list of commands which are sent to microcontroller
            return commands;
        }

        protected abstract void Run();

        protected void Wait(short delay)
        {
            commands.Add(CommandsBatch.WaitOpCode, delay);
        }

        protected void WaitMicro(short delay)
        {
            if (delay > 16383)
            {
                throw new Exception("Keep micro delays under 16383us.");
            }
            commands.Add(CommandsBatch.WaitMicroOpCode, delay);
        }

        protected void SetPinModeOutput(short pin)
        {
            commands.Add(CommandsBatch.SetPinModeOutputOpCode, pin);
        }

        protected void SetPinOn(short pin)
        {
            commands.Add(CommandsBatch.SetPinOnOpCode, pin);
        }

        protected void SetPinOff(short pin)
        {
            commands.Add(CommandsBatch.SetPinOffOpCode, pin);
        }
    }
}
