using hscCtrl.Script;
using hscCtrl.Serial;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hscCtrl
{
    internal class ScriptProcessor
    {
        public static Func<IScriptSource, Func<ISerialPortComm>, Func<Task>> GetProcessScriptTask =
            (scriptSource, serialCommFactory) =>
            {
                return () => ProcessScript(scriptSource, serialCommFactory);
            };

        public static Task ProcessScript(IScriptSource scriptSource, Func<ISerialPortComm> serialCommFactory)
        {
            return new Task(async () =>
            {
                try
                {
                    Log.Information("Processing the script.");

                    scriptSource.Backup();
                    var script = scriptSource.GetContent();
                    var commands = await script.Evaluate();

                    using (var serialComm = serialCommFactory())
                    {
                        var state = InitStateMachineBuilder(serialComm, commands)
                            .BuildStateMachine();

                        serialComm.OnNewMessage += (message) =>
                        {
                            state = state.Handle(message);
                        };

                        serialComm.Open();

                        while (state != null)
                        {
                            Thread.Sleep(100);
                        }
                    }

                    Log.Information("Script processing done.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error encountered while processing the script.");
                }
            });
        }

        private static StateMachineBuilder InitStateMachineBuilder(ISerialPortComm serialComm, CommandsBatch commands)
        {
            return new StateMachineBuilder()
            {
                WriteInstructionsFn = () => { serialComm.Write(commands.ToCommString()); },
                DoneFn = () => { serialComm.Close(); }
            };
        }
    }
}
