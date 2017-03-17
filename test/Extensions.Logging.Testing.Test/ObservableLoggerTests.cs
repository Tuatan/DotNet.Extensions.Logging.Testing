using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Extensions.Logging.Testing;

namespace Extensions.Logging.Testing.Test
{
    [TestClass]
    public class ObservableLoggerTests
    {
        [TestMethod]
        public void Do()
        {
            var logger = new ObservableLogger("One", (a,b) => true, null);

            //Assert.IsNotNull(logCollector.Logs);
            //Assert.AreEqual(0, logCollector.Logs.Length);
        }
    }
}
