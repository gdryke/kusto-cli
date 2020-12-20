using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using kusto_cli;
using Moq;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Kusto.Cloud.Platform.Data;
using System.Threading.Tasks;
using System.IO;

namespace kusto_cli.test
{
    [TestClass]
    public class RunQueryTests
    {
        // TODO its an extension method, so this doesn't work :/
        //[TestMethod]
        public async Task EmptyArgsParsesOkay()
        {
            ProgramArguments testArgs = new ProgramArguments ()
            {
                Cluster = "test",
                Database = "test",
                Query = "test"
            };
            var expectedRequestProps = new ClientRequestProperties() { ClientRequestId = Guid.NewGuid().ToString() };

            Mock<IDataReader> mockReader = new Mock<IDataReader>();
            mockReader.Setup(reader => reader.WriteAsText("results",
                                                            true,
                                                            System.Console.Out,
                                                            true,
                                                            true,
                                                            25,
                                                            "test",
                                                            true,
                                                            false));

            Mock<ICslQueryProvider> mockProvider = new Mock<ICslQueryProvider>();
            mockProvider.Setup(provider => provider.ExecuteQueryAsync(testArgs.Database, testArgs.Query, expectedRequestProps))
                        .Returns(Task.FromResult(mockReader.Object));

            await KustoCli.RunQuery(mockProvider.Object, testArgs);

            mockProvider.Verify(provider => provider.ExecuteQueryAsync(testArgs.Database, testArgs.Query, expectedRequestProps), 
                                Times.Once());

            mockReader.Verify(reader => reader.WriteAsText("results",
                                        true,
                                        System.Console.Out,
                                        true,
                                        true,
                                        25,
                                        "test",
                                        true,
                                        false), Times.Once());
        }

        
    }
}
