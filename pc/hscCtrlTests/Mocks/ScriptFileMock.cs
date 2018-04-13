using hscCtrl.Script;
using System;

namespace hscCtrlTests.Mocks
{
    class ScriptSourceMock : IScriptSource
    {
        public Func<string> OnBackup { get; set; }
        public Func<string> OnGetContent { get; set; }

        public string Backup()
        {
            return OnBackup?.Invoke();
        }

        public string GetContent()
        {
            return OnGetContent?.Invoke();
        }
    }
}
