using System;
using System.IO;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Kusto.Cloud.Platform.Data;

namespace kusto_cli
{
    public class KustoCli
    {
        static public string Cluster;
        static public string Database;
        static public string Query;
        static public OutputFormat Format = OutputFormat.Text;

        // TODO better arrangement so this can be mocked and tested.
        static public void RunQuery(string cluster, string database, string query)
        {
            KustoConnectionStringBuilder kcsb;
            if (cluster == "https://help.kusto.windows.net")
            {
                kcsb = new KustoConnectionStringBuilder("https://help.kusto.windows.net/Samples; Fed=true; Accept=true");
            }
            else
            {
                kcsb = new KustoConnectionStringBuilder(cluster, database).WithAadUserPromptAuthentication();
            }
            

            using (var queryProvider = KustoClientFactory.CreateCslQueryProvider(kcsb))
            {
                var clientRequestProperties = new ClientRequestProperties() { ClientRequestId = Guid.NewGuid().ToString() };
                using (var reader = queryProvider.ExecuteQuery(query, clientRequestProperties))
                {
                    // other tables have viz and query details
                    TextWriter stdout = Console.Out;
                    switch (Format)
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
        }

        static public bool ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-c":
                    case "--cluster":
                        Cluster = args[i+1];
                        i++;
                        break;
                    case "-d":
                    case "--database":
                        Database = args[i+1];
                        i++;
                        break;
                    case "-q":
                    case "--query":
                        Query = args[i+1];
                        i++;
                        break;
                    case "-f":
                    case "--format":
                        if (!OutputFormat.TryParse(args[i+1], true, out Format))
                        {
                            throw new Exception("Incorrect format given");
                        }
                        i++;
                        break;
                    default:
                        Console.WriteLine($"Unknown argument: {args[i]}");
                        WriteUsage();
                        return false;
                }
            }
            return true;
        }

        static int Main(string[] args)
        {
            if (!ParseArgs(args))
            {
                return 1;
            }
            try
            {
                RunQuery(Cluster, Database, Query);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception querying Kusto.");
                Console.Error.WriteLine(ex.ToString());
            }
            
            return 0;
        }

        static void WriteUsage()
        {
            Console.WriteLine("usage: kusto-cli [-c cluster] [-d database] [-q query] [-f format]");
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
