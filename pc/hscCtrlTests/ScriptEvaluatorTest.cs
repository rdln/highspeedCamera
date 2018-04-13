using hscCtrl.Script;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace hscCtrlTests
{
    [TestClass]
    public class ScriptEvaluatorTest
    {
        [TestMethod]
        public void TranslateCommandTest()
        {
            var iterations = 10;
            var script = $"for (var i = 0; i < {iterations}; i++) {{ Wait(0); }}";
            var commands = script.Evaluate().Result;
            Assert.IsNotNull(commands);
            Assert.AreEqual(commands.Count, iterations);
        }
    }
}
