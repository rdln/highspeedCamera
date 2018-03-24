using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hscCtrl
{
    internal class SingleTaskRunner
    {
        volatile Task task;
        private static readonly object sync = new object();
        private static readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

        public void Add(Func<Task> taskAction)
        {
            if (task != null)
            {
                //A task is already being processed, nothing to do for now.
                return;
            }
            else
            {
                lock (sync)
                {
                    if (task == null)
                    {
                        task = Task.Factory.StartNew(WrapTask(this, taskAction));
                    }
                }
            }
        }

        private static Func<SingleTaskRunner, Func<Task>, Func<Task>> WrapTask =
            (taskRunner, taskAction) =>
            async () =>
            {
                Log.Information("Running script update task.");
                Thread.Sleep(Delay);

                try
                {
                    await taskAction();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error encountered.");
                }
                finally
                {
                    taskRunner.task = null;
                }
                Log.Information("Finished running script update task.");
            };
    }
}
