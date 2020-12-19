#/bin/bash

id="clitest" # something from date
tenant_id='778f31df-53d5-450a-8cf0-c35547574e75'
sub_id='2149dbf7-dca9-47ee-8374-33bdb4ff0ee6'
rg="projects" # maybe fresh per run?

# az account set -s $sub_id
cluster_name=$id"cluster"
region="eastus"
# az kusto cluster create -n $cluster_name -l $region -g $rg --sku name='Dev(No SLA)_Standard_E2a_v4' capacity=1 tier='Basic'
cluster_uri="https://$cluster_name.$region.kusto.windows.net"
db_name=$id"_db"
# az kusto database create --cluster-name $cluster_name --database-name $db_name --resource-group $rg
app_name=$id"_app"
client_key=$(dd if=/dev/urandom bs=1 count=32 2>/dev/null | base64  | rev | cut -b 2- | rev)
#res=$(az ad app create --display-name $app_name --password "$client_key")
client_id=$(echo $res | jq -r .appId)
# make SP on that app?
echo "creating SP on app: $client_id"
#res=$(az ad sp create --id $client_id)
sp_id=$(echo $res | jq -r .objectId)
echo "created SP with objectId: $sp_id. Adding it to cluster"
# Then the principal ID down here is the SP object id, not app
# az kusto cluster-principal-assignment create --cluster-name "kustoclitest"  --principal-id "$sp_id" --principal-type "App" --role  "AllDatabasesAdmin" --tenant-id "$tenant_id" --principal-assignment-name "CLI Test Suite" --resource-group "$rg"
echo "added SP to cluster"


echo "Setting ENV"
export KUSTOCLI_CLIENT_ID=$client_id
export KUSTOCLI_CLIENT_KEY=$client_key
export KUSTOCLI_TENANT_ID=$tenant_id

echo "running cli"
res=$(./kusto-cli -c "$cluster_uri" -d $db_name -q '.show tables' --use-client-id)
echo $res
