using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("hscCtrlTests")]

namespace hscCtrl
{
    static class Program
    {
        static void Main(string[] args)
        {
            SetupLogging();

            var file = args.Length > 0 ? args[0] : "test.script"; //This is the file which we'll watch for changes and [1]
            file = Path.GetFullPath(file);

            if (!File.Exists(file))
            {
                Log.Error($"File '{file}' not found, exiting.");
                return;
            }

            MonitorFile(file);
            Log.Information($"hscCtrl exiting.");
        }

        private static void MonitorFile(string file)
        {
            Log.Information($"Monitoring file '{file}'.");

            using (var fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(file), Path.GetFileName(file))) // the path parameter needs to be the folder of the file and the filename is used as a filter...
            {
                fileWatcher.Changed += FileChanged; //[1] when changed we'll start acting upon its content.
                fileWatcher.EnableRaisingEvents = true;
                Console.ReadLine();
                fileWatcher.EnableRaisingEvents = false;
                Log.Information($"File monitoring stopped.");
            }
        }

        private static void FileChanged(object sender, FileSystemEventArgs eventArgs)
        {
            Log.Information("file changed.");
            //ok, now we know the file is changed but, depending on the editor you're using you'd might notice there are more than one events triggered by each save.
        }

        private static void SetupLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
