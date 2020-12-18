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
            Assert.IsTrue(KustoCli.ParseArgs(args));
        }

        [TestMethod]
        public void UnknownArgFailsToParse()
        {
            string[] args = new string[] {"-yy"};
            Assert.IsFalse(KustoCli.ParseArgs(args));
        }

        [TestMethod]
        public void FullArgsParsesSuccessfully()
        {
            string[] args = new string[] {"-c", "cluster", "-d", "database", "-q", "query"};
            Assert.IsTrue(KustoCli.ParseArgs(args));
            Assert.AreEqual(KustoCli.Cluster, args[1]);
            Assert.AreEqual(KustoCli.Database, args[3]);
            Assert.AreEqual(KustoCli.Query, args[5]);
        }

        [TestMethod]
        public void FormatDefaultsToText()
        {
            string[] args = new string[0];
            Assert.IsTrue(KustoCli.ParseArgs(args));
            Assert.AreEqual(KustoCli.Format, OutputFormat.Text);
        }
    }
}
