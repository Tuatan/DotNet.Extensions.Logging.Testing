using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Extensions.Logging.Testing;

namespace Extensions.Logging.Testing.Test
{
    [TestClass]
    public class EmptyLoggerTests
    {
        [TestMethod]
        public void EmptyLogget_Construct()
        {
            var logger = new EmptyLogger();

            Assert.IsNotNull(logger);
        }
    }
}
