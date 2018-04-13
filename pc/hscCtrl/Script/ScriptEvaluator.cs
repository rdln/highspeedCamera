using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Serilog;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace hscCtrl.Script
{
    internal static class ScriptEvaluator
    {
        private const string Template = @"public class HscProgram : hscCtrl.Script.HscProgramBase {{ protected override void Run() {{ {0} }} }} return new HscProgram().GetCommands();";

        private static readonly Assembly[] References = new[] {
            typeof(System.Object).Assembly,
            typeof(IEnumerable<int[]>).Assembly,
            typeof(HscProgramBase).Assembly
        };

        public static async Task<CommandsBatch> Evaluate(this string script)
        {
            var executableScript = string.Format(Template, script);
            Log.Information($"Executing the script.");
            var result = await Execute(executableScript);
            Log.Information($"Script generated {result.Count} commands: {result}.");
            return result;
        }

        private static async Task<CommandsBatch> Execute(string script)
        {
            var state = await CSharpScript.RunAsync<CommandsBatch>(script,
                    ScriptOptions.Default.WithReferences(References));
            return state.ReturnValue;
        }
    }
}
