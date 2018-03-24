using Serilog;
using System;
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

            var script = "Wait(0); SetPinOn(1); SetPinOff(2);";

            var commands = script.Evaluate().Result;
            Log.Information($"generated commands: {string.Join(";", commands.Select(command => "[" + string.Join(",", command) + "]").ToArray())}.");

            Console.ReadLine();
        }

        private static void SetupLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
