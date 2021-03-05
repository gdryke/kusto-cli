# Usage of kusto-cli

## Basic Usage
The output from the program with bad options or `-h`
```
usage: kusto-cli [-c cluster] [-d database] [-q query] [-i query file] [-f format] [--useClientId]
```

## More Details

### -h/--help
Print usage and exit

### -c/--cluster
Cluster to query, full URL. Example: `https://help.kusto.windows.net`

### -d/--database
Database to query. Example: `Samples`

### -q/--query
Query string to run. Example: `StormEvents | project StartTime, EventId, EpisodeNarrative, EventNarrative | take 10`

### -i/--query-file
File containing a query to run. Example: `docs/samples/sample_db_query.kql`

### -f/--format
Output format. Defaults to Text. Example: `Text`

Full List:
- Text
- Json
- Csv
- Tsv
- Html
- Markdown

### --useClientId
This allows you to use an external client (ie an AAD SP or App) to authenticate to Kusto.
Currently only an Client ID + Client ID are supported, no MSI or certificates.

You'll need to set the following environment variables when using this option:
- `KUSTOCLI_CLIENT_ID`: the client ID
- `KUSTOCLI_CLIENT_KEY`: the client key/secret
- `KUSTOCLI_TENANT_ID`: the GUID id of the tenant your ADD entity and cluster are in