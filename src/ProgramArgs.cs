namespace kusto_cli
{
    public class ProgramArguments
    {
        public string Cluster;
        public string Database;
        public string Query;
        public string StdinQuery;
        public string QueryFilePath;
        public OutputFormat Format = OutputFormat.Text;
        public bool UseClientId;
        public string ClientId => System.Environment.GetEnvironmentVariable("KUSTOCLI_CLIENT_ID");
        public string ClientKey => System.Environment.GetEnvironmentVariable("KUSTOCLI_CLIENT_KEY").Trim();
        public string Authority => System.Environment.GetEnvironmentVariable("KUSTOCLI_TENANT_ID");
    }
}