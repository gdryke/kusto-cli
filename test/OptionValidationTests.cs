using Microsoft.VisualStudio.TestTools.UnitTesting;
using kusto_cli;
using System.Threading.Tasks;

namespace kusto_cli.test
{
    [TestClass]
    public class OptionValidationTests
    {
        [TestMethod]
        public async Task AllRequiredSettingsPassesValidation()
        {
            ProgramArguments testArgs = new ProgramArguments ()
            {
                Cluster = "test",
                Database = "test",
                Query = "test"
            };
            Assert.IsTrue(await KustoCli.ValidateArgs(testArgs));
        }

        [TestMethod]
        public async Task MissingClusterIsFalse()
        {
            ProgramArguments testArgs = new ProgramArguments ()
            {
                Database = "test",
                Query = "test"
            };
            Assert.IsFalse(await KustoCli.ValidateArgs(testArgs));
        }

        [TestMethod]
        public async Task MissingDatabaseIsFalse()
        {
            ProgramArguments testArgs = new ProgramArguments ()
            {
                Cluster = "test",
                Query = "test"
            };
            Assert.IsFalse(await KustoCli.ValidateArgs(testArgs));
        }

        [TestMethod]
        public async Task MissingQueryIsFalse()
        {
            ProgramArguments testArgs = new ProgramArguments ()
            {
                Cluster = "test",
                Database = "test"
            };
            Assert.IsFalse(await KustoCli.ValidateArgs(testArgs));
        }
    }
}
