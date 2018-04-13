using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hscCtrl.Script
{
    public class CommandsBatch
    {
        public static readonly short WaitOpCode = 0x01;
        public static readonly short WaitMicroOpCode = 0x02;
        public static readonly short SetPinModeOutputOpCode = 0x03;
        public static readonly short SetPinOnOpCode = 0x04;
        public static readonly short SetPinOffOpCode = 0x05;

        private readonly List<short[]> commands = new List<short[]>();

        public CommandsBatch()
        { }

        public CommandsBatch(IEnumerable<short[]> commands)
        {
            this.commands.AddRange(commands);
        }

        public int Count { get { return commands.Count; } }

        public void Add(short opCode, short param)
        {
            commands.Add(new[] { opCode, param });
        }

        internal short GetOpCode(short loop)
        {
            return commands[loop][0];
        }

        internal short GetParam(short loop)
        {
            return commands[loop][1];
        }

        public override string ToString()
        {
            return string.Join(";", commands.Select(command => "[" + string.Join(",", command) + "]").ToArray());
        }
    }

    public static class CommandsBatchExtensions
    {
        public static string ToCommString(this CommandsBatch @this)
        {
            var stringBuilder = new StringBuilder();
            var commandsCount = @this.Count;
            stringBuilder.Append($"{commandsCount};");
            for(short loop = 0; loop < commandsCount; loop++)
            {
                var opCode = @this.GetOpCode(loop);
                var param = @this.GetParam(loop);
                stringBuilder.Append($"{opCode};{param};");
            }
            return stringBuilder.ToString();
        }
    }
}
