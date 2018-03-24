using Serilog;

namespace hscCtrl
{
    static class Program
    {
        static void Main(string[] args)
        {
            SetupLogging();
        }

        private static void SetupLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
