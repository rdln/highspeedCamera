using hscCtrl;
using hscCtrlTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace hscCtrlTests
{
    [TestClass]
    public class ProcessScriptTest
    {
        private ScriptSourceMock scriptMock;
        private SerialCommMock serialPortMock;
        bool fileBackedUp = false;
        string messageSentToArduino = string.Empty;

        [TestMethod]
        public void TestScriptIsProcessed()
        {
            var scriptProcessorTask = ScriptProcessor.GetProcessScriptTask(scriptMock, () => { return serialPortMock; });
            var task = scriptProcessorTask();
            task.Start();
            task.Wait();

            Assert.IsTrue(fileBackedUp);
            Assert.IsTrue(messageSentToArduino.Equals("1;1;0;"));
        }

        [TestInitialize]
        public void Initialize()
        {
            scriptMock = new ScriptSourceMock()
            {
                OnBackup = () => {
                    fileBackedUp = true;
                    return string.Empty;
                },
                OnGetContent = () => {
                    return "Wait(0);";
                }
            };
            serialPortMock = new SerialCommMock()
            {
                OnWrite = (message) => {
                    messageSentToArduino = messageSentToArduino + message;
                }
            };
            serialPortMock.OnOpen = () => {
                Task.Run(() => { SimulateArduinoMessages(); });
            };
        }

        private void SimulateArduinoMessages()
        {
            Thread.Sleep(50);
            serialPortMock.SimulateMessageReceived("ready");
            Thread.Sleep(50);
            serialPortMock.SimulateMessageReceived("done");
        }
    }
}
