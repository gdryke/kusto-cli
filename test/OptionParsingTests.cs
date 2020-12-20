using Microsoft.VisualStudio.TestTools.UnitTesting;
using kusto_cli;

namespace kusto_cli.test
{
    [TestClass]
    public class OptionParsingTests
    {
        [TestMethod]
        public void EmptyArgsParsesOkay()
        {
            string[] args = new string[0];
            Assert.IsNotNull(KustoCli.ParseArgs(args));
        }

        [TestMethod]
        public void UnknownArgFailsToParse()
        {
            string[] args = new string[] {"-yy"};
            Assert.IsNull(KustoCli.ParseArgs(args));
        }

        [TestMethod]
        public void FullArgsParsesSuccessfully()
        {
            string[] args = new string[] {"-c", "cluster", "-d", "database", "-q", "query"};
            var processedArgs = KustoCli.ParseArgs(args);
            Assert.IsNotNull(processedArgs);
            Assert.AreEqual(processedArgs.Cluster, args[1]);
            Assert.AreEqual(processedArgs.Database, args[3]);
            Assert.AreEqual(processedArgs.Query, args[5]);
        }

        [TestMethod]
        public void FormatDefaultsToText()
        {
            string[] args = new string[0];
            var processedArgs = KustoCli.ParseArgs(args);
            Assert.IsNotNull(processedArgs);
            Assert.AreEqual(processedArgs.Format, OutputFormat.Text);
        }

        [TestMethod]
        public void UseClientIdDefaultsFalse()
        {
            string[] args = new string[0];
            var processedArgs = KustoCli.ParseArgs(args);
            Assert.IsNotNull(processedArgs);
            Assert.IsFalse(processedArgs.UseClientId);
        }

        [TestMethod]
        public void UseClientIdDGetsSetToTrue()
        {
            string[] args = new string[] {"--use-client-id"};
            var processedArgs = KustoCli.ParseArgs(args);
            Assert.IsNotNull(processedArgs);
            Assert.IsTrue(processedArgs.UseClientId);
        }
    }
}
