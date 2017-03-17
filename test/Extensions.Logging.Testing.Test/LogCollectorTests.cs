using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Extensions.Logging.Testing;

namespace Extensions.Logging.Testing.Test
{
    [TestClass]
    public class LogCollectorTests
    {
        [TestMethod]
        public void Document()
        {
            var logCollector = new LogCollector();

            using (logCollector.CollectLogs())
            {
            }

            Assert.IsNotNull(logCollector.Logs);
            Assert.AreEqual(0, logCollector.Logs.Length);
        }
    }
}
