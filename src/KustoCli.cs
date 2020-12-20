using System;
using System.IO;
using System.Threading.Tasks;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Kusto.Cloud.Platform.Data;

namespace kusto_cli
{
    public class KustoCli
    {
        static public ProgramArguments ProgramArgs;

        static async public Task RunQuery(ICslQueryProvider queryProvider, ProgramArguments programArgs)
        {   
            var clientRequestProperties = new ClientRequestProperties() { ClientRequestId = Guid.NewGuid().ToString() };
            using (var reader = await queryProvider.ExecuteQueryAsync(programArgs.Database, programArgs.Query, clientRequestProperties))
            {
                // other tables have viz and query details
                TextWriter stdout = Console.Out;
                switch (programArgs.Format)
                {
                    case OutputFormat.Text:
                        reader.WriteAsText(null, true, stdout, firstOnly: true, markdown: false , includeWithHeader: "test", includeHeader:true);
                        break;
                    case OutputFormat.Markdown:
                        reader.WriteAsText("results", true, stdout, firstOnly: true, markdown: true , includeWithHeader: "test", includeHeader:true);
                        break;
                    case OutputFormat.Csv:
                        reader.WriteAsCsv(true, stdout);
                        break;
                    case OutputFormat.Json:
                        reader.WriteAsJson(stdout, out long bytes);
                        break;
                    case OutputFormat.Tsv:
                        reader.WriteAsTsv(true, stdout);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

// TODO update this to instead return ProgramArgs, just has test implications
        static public ProgramArguments ParseArgs(string[] args)
        {
            ProgramArguments programArgs = new ProgramArguments();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-c":
                    case "--cluster":
                        programArgs.Cluster = args[i+1];
                        i++;
                        break;
                    case "-d":
                    case "--database":
                        programArgs.Database = args[i+1];
                        i++;
                        break;
                    case "-q":
                    case "--query":
                        programArgs.Query = args[i+1];
                        i++;
                        break;
                    case "-f":
                    case "--format":
                        if (!OutputFormat.TryParse(args[i+1], true, out programArgs.Format))
                        {
                            throw new Exception("Incorrect format given");
                        }
                        i++;
                        break;
                    case "--use-client-id":
                        programArgs.UseClientId = true;
                        break;
                    default:
                        Console.WriteLine($"Unknown argument: {args[i]}");
                        WriteUsage();
                        return null;
                }
            }

            return programArgs;
        }

        static async public Task<bool> ValidateArgs(ProgramArguments programArgs)
        {

            if (string.IsNullOrEmpty(programArgs.Cluster))
            {
                await Console.Error.WriteLineAsync("Required parameter cluster (-c) not set.");
                return false;
            }
            
            if (string.IsNullOrEmpty(programArgs.Database))
            {
                await Console.Error.WriteLineAsync("Required parameter database (-d) not set.");
                return false;
            }

            if (string.IsNullOrEmpty(programArgs.Query))
            {
                await Console.Error.WriteLineAsync("Required parameter database (-d) not set.");
                return false;
            }

            return true;
        }

        public static ICslQueryProvider GetQueryProvider(ProgramArguments programArgs)
        {
            KustoConnectionStringBuilder kcsb = null;
            if (programArgs.UseClientId)
            {
                kcsb = new KustoConnectionStringBuilder(programArgs.Cluster, programArgs.Database)
                            .WithAadApplicationKeyAuthentication(programArgs.ClientId, programArgs.ClientKey, programArgs.Authority);
            }
            else
            {
                kcsb = new KustoConnectionStringBuilder(programArgs.Cluster, programArgs.Database).WithAadUserPromptAuthentication();
            }
            return KustoClientFactory.CreateCslQueryProvider(kcsb);
        }

        static async Task<int> Main(string[] args)
        {
            ProgramArguments programArgs = ParseArgs(args);
            if (programArgs == null)
            {
                Console.Error.WriteLine("Exception with arguments.");
                return 1;
            }

            ICslQueryProvider queryProvider = null;

            try
            {
                // update all of this to use an Args or Options class for args
                if (!await ValidateArgs(programArgs))
                {
                    Console.Error.WriteLine("Invalid arguments.");
                }
                queryProvider = GetQueryProvider(programArgs);
                await RunQuery(queryProvider, programArgs);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception querying Kusto.");
                Console.Error.WriteLine(ex.ToString());
            }
            finally 
            {
                queryProvider.Dispose();
            }
            
            return 0;
        }

        static void WriteUsage()
        {
            Console.WriteLine("usage: kusto-cli [-c cluster] [-d database] [-q query] [-f format] [--useClientId]");
        }
    }

    public enum OutputFormat
    {
        Text,
        Json,
        Csv,
        Tsv,
        Html,
        Markdown
    }
}
