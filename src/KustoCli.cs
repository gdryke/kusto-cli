using System;
using System.IO;
using System.Threading.Tasks;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Kusto.Cloud.Platform.Data;
using Microsoft.Azure.Services.AppAuthentication;

namespace kusto_cli
{
    public class KustoCli
    {
        static public ProgramArguments ProgramArgs;

        static private System.Text.Encoding DEFAULT_INPUT_FILE_ENCODING = System.Text.Encoding.UTF8;

        static async Task<int> Main(string[] args)
        {
            ProgramArguments programArgs = await ParseArgs(args);
            if (programArgs == null)
            {
                Console.Error.WriteLine("Exception with arguments.");
                return 1;
            }

            ICslQueryProvider queryProvider = null;

            try
            {
                if (!await ValidateArgs(programArgs))
                {
                    Console.Error.WriteLine("Invalid arguments.");
                    WriteUsage();
                    return -1;
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
                queryProvider?.Dispose();
            }

            return 0;
        }

        static async public Task<ProgramArguments> ParseArgs(string[] args)
        {
            ProgramArguments programArgs = new ProgramArguments();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-c":
                    case "--cluster":
                        programArgs.Cluster = ProcessClusterParameter(args[i + 1]);
                        i++;
                        break;
                    case "-d":
                    case "--database":
                        programArgs.Database = args[i + 1];
                        i++;
                        break;
                    case "-q":
                    case "--query":
                        programArgs.Query = args[i + 1];
                        i++;
                        break;
                    case "-i":
                    case "--query-file":
                        programArgs.QueryFilePath = args[i + 1];
                        i++;
                        break;
                    case "-f":
                    case "--format":
                        if (!OutputFormat.TryParse(args[i + 1], true, out programArgs.Format))
                        {
                            throw new Exception("Incorrect format given");
                        }
                        i++;
                        break;
                    case "-h":
                    case "--help":
                        WriteUsage();
                        System.Environment.Exit(0);
                        break;
                    case "--use-client-id":
                        programArgs.UseClientId = true;
                        break;
                    case "--use-user-assigned-msi":
                        programArgs.UseUserAssignedMSI = true;
                        break;
                    default:
                        Console.WriteLine($"Unknown argument: {args[i]}");
                        WriteUsage();
                        return null;
                }
            }

            // Query can also come from stdin
            if (System.Console.IsInputRedirected)
            {
                programArgs.StdinQuery = await System.Console.In.ReadToEndAsync();
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

            if (string.IsNullOrEmpty(programArgs.Query)
                && string.IsNullOrEmpty(programArgs.QueryFilePath)
                && string.IsNullOrEmpty(programArgs.StdinQuery))
            {
                await Console.Error.WriteLineAsync("No query found. Either use -q for text, -i for a file, or pipe to stdin.");
                return false;
            }

            // TODO add the set of checks to make sure we're not using 2 query inputs.

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
            else if (programArgs.UseUserAssignedMSI)
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider($"RunAs=App;AppId={programArgs.ClientId}");
                kcsb = new KustoConnectionStringBuilder(programArgs.Cluster, programArgs.Database)
                        .WithAadTokenProviderAuthentication(
                            () => azureServiceTokenProvider.GetAccessTokenAsync(programArgs.Cluster));
            }
            else
            {
                kcsb = new KustoConnectionStringBuilder(programArgs.Cluster, programArgs.Database).WithAadUserPromptAuthentication();
            }
            return KustoClientFactory.CreateCslQueryProvider(kcsb);
        }

        static async public Task RunQuery(ICslQueryProvider queryProvider, ProgramArguments programArgs)
        {
            var clientRequestProperties = new ClientRequestProperties() { ClientRequestId = Guid.NewGuid().ToString() };

            string query = await GetQueryContent(programArgs);

            using (var reader = await queryProvider.ExecuteQueryAsync(programArgs.Database, query, clientRequestProperties))
            {
                // other tables have viz and query details
                TextWriter stdout = Console.Out;
                switch (programArgs.Format)
                {
                    case OutputFormat.Text:
                        reader.WriteAsText(null, true, stdout, firstOnly: true, markdown: false, includeWithHeader: "test", includeHeader: true);
                        break;
                    case OutputFormat.Markdown:
                        reader.WriteAsText("results", true, stdout, firstOnly: true, markdown: true, includeWithHeader: "test", includeHeader: true);
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

        static async public Task<string> GetQueryContent(ProgramArguments programArgs)
        {
            if (!string.IsNullOrEmpty(programArgs.Query))
            {
                return programArgs.Query;
            }

            if (!string.IsNullOrEmpty(programArgs.QueryFilePath))
            {
                // TODO This should handle encoding better.
                return await File.ReadAllTextAsync(programArgs.QueryFilePath, DEFAULT_INPUT_FILE_ENCODING);
            }

            if (!string.IsNullOrEmpty(programArgs.StdinQuery))
            {
                return programArgs.StdinQuery;
            }

            throw new ArgumentException("No query found in program arguments.");
        }

        static public string ProcessClusterParameter(string clusterInputParameter)
        {
            if (Uri.TryCreate(clusterInputParameter, UriKind.Absolute, out _))
            {
                return clusterInputParameter;
            }
            else
            {
                return $"https://{clusterInputParameter}.kusto.windows.net";
            }
        }

        // TODO
        // static async public Tuple GetClusterDbInfoFromString(string connectionString) {}
        // Or maybe just another fork in the connection string builder?

        static void BailOut()
        {
            WriteUsage();
            System.Environment.Exit(1);
        }
        static void WriteUsage()
        {
            Console.WriteLine("usage: kusto-cli [-c cluster] [-d database] [-q query] [-i query file] [-f format] [--useClientId] [-h]");
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
