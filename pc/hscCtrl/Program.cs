using hscCtrl.Script;
using hscCtrl.Serial;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("hscCtrlTests")]

namespace hscCtrl
{
    static class Program
    {
        static SingleTaskRunner taskRunner = new SingleTaskRunner();

        static void Main(string[] args)
        {
            SetupLogging();

            var file = args.Length > 0 ? args[0] : "test.script";
            file = Path.GetFullPath(file);

            if (!File.Exists(file))
            {
                Log.Error($"File '{file}' not found, exiting.");
                return;
            }
            var scriptFile = new ScriptSource(file);

            var port = GetPort(args);
            Log.Information($"Using port: {port}.");
            ISerialPortComm serialPortBuilder() => new SerialPortComm(port);

            MonitorFile(scriptFile.File, BuildFileChangedHandler(scriptFile, serialPortBuilder));
            Log.Information($"hscCtrl exiting.");
        }

        private static string GetPort(string[] args)
        {
            var availablePorts = SerialPortComm.GetPorts();
            Log.Information($"Found {string.Join(", ", availablePorts)}.");
            var argPort = args.Length > 1 ? args[1] : null;
            var port = !string.IsNullOrEmpty(argPort) && availablePorts.Any(p => p.Equals(argPort))
                ? argPort
                : availablePorts.FirstOrDefault();
            return port;
        }

        private static void MonitorFile(string file, FileSystemEventHandler handler)
        {
            Log.Information($"Monitoring file '{file}'.");

            using (var fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(file), Path.GetFileName(file)))
            {
                fileWatcher.Changed += handler;
                fileWatcher.EnableRaisingEvents = true;
                Console.ReadLine();
                fileWatcher.EnableRaisingEvents = false;
                fileWatcher.Changed -= handler;
                Log.Information($"File monitoring stopped.");
            }
        }

        private static Func<IScriptSource, Func<ISerialPortComm>, FileSystemEventHandler> BuildFileChangedHandler = (scriptSource, serialPortFactory) =>
        {
            return (sender, args) =>
            {
                Log.Information("Script file changed");
                var scriptProcessingTask = ScriptProcessor.GetProcessScriptTask(scriptSource, serialPortFactory);
                taskRunner.Add(scriptProcessingTask);
            };
        };


        private static void SetupLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
