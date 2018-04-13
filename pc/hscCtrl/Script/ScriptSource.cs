using System;
using System.IO;

namespace hscCtrl.Script
{
    internal interface IScriptSource
    {
        string GetContent();
        string Backup();
    }

    class ScriptSource : IScriptSource
    {
        public string File { get; private set; }

        public ScriptSource(string file)
        {
            this.File = file;
        }

        public string GetContent()
        {
            return System.IO.File.ReadAllText(File);
        }

        public string Backup()
        {
            var backupFile = GetBackupFilename(File);
            System.IO.File.Copy(File, backupFile);
            return backupFile;
        }

        private static string GetBackupFilename(string file)
        {
            var dir = Path.GetDirectoryName(file);
            var filename = Path.GetFileNameWithoutExtension(file);
            var fileExtension = Path.GetExtension(filename);
            var currentDate = DateTime.Now.ToString("yyMMddHHmmss");
            var backupFile = Path.Combine(dir, $"{filename}.{currentDate}.{fileExtension}");
            return backupFile;
        }
    }
}
