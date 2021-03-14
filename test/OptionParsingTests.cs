using Microsoft.VisualStudio.TestTools.UnitTesting;
using kusto_cli;
using System.Threading.Tasks;

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
        public async Task UnknownArgFailsToParse()
        {
            string[] args = new string[] { "-yy" };
            Assert.IsNull(await KustoCli.ParseArgs(args));
        }

        [TestMethod]
        public async Task FullArgsParsesSuccessfully()
        {
            string[] args = new string[] { "-c", "https://cluster.kusto.windows.net", "-d", "database", "-q", "query" };
            var processedArgs = await KustoCli.ParseArgs(args);
            Assert.IsNotNull(processedArgs);
            Assert.AreEqual(processedArgs.Cluster, args[1]);
            Assert.AreEqual(processedArgs.Database, args[3]);
            Assert.AreEqual(processedArgs.Query, args[5]);
        }

        [TestMethod]
        public async Task FormatDefaultsToText()
        {
            string[] args = new string[0];
            var processedArgs = await KustoCli.ParseArgs(args);
            Assert.IsNotNull(processedArgs);
            Assert.AreEqual(processedArgs.Format, OutputFormat.Text);
        }

        [TestMethod]
        public async Task UseClientIdDefaultsFalse()
        {
            string[] args = new string[0];
            var processedArgs = await KustoCli.ParseArgs(args);
            Assert.IsNotNull(processedArgs);
            Assert.IsFalse(processedArgs.UseClientId);
        }

        [TestMethod]
        public async Task UseClientIdDGetsSetToTrue()
        {
            string[] args = new string[] { "--use-client-id" };
            var processedArgs = await KustoCli.ParseArgs(args);
            Assert.IsNotNull(processedArgs);
            Assert.IsTrue(processedArgs.UseClientId);
        }

        [TestMethod]
        public void ClusterUriIsReturned()
        {
            string clusterInput = "help";
            var clusterValue = KustoCli.ProcessClusterParameter(clusterInput);
            Assert.IsNotNull(clusterValue);
            Assert.AreEqual("https://help.kusto.windows.net", clusterValue);
        }

        [TestMethod]
        public void ClusterNameBecomesUri()
        {
            string clusterInput = "https://help.kusto.windows.net";
            var clusterValue = KustoCli.ProcessClusterParameter(clusterInput);
            Assert.IsNotNull(clusterValue);
            Assert.AreEqual("https://help.kusto.windows.net", clusterValue);
        }
    }
}
