using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Logging.Testing.Test
{
    [TestClass]
    public class EmptyLoggerTest
    {
        [TestMethod]
        public void EmptyLogget_Construct()
        {
            var logger = new EmptyLogger();

            Assert.IsNotNull(logger);
        }
    }
}
