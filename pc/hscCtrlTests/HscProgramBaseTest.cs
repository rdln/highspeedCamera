using hscCtrl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace hscCtrlTests
{
    [TestClass]
    public class HscProgramBaseTest
    {
        [TestMethod]
        public void TestCommandsAreGenerated()
        {
            var commands = new HscProgram()
                .GetCommands();
            Assert.IsNotNull(commands);
            Assert.IsTrue(commands.Count() == 3);
        }

        class HscProgram : HscProgramBase
        {
            protected override void Run()
            {
                Wait(0);
                SetPinOn(1);
                SetPinOff(2);
            }
        }
    }
}
