#/bin/bash

tenant_id='778f31df-53d5-450a-8cf0-c35547574e75'
cluster_name="kustoclitest"
region="eastus"
cluster_uri="https://$cluster_name.$region.kusto.windows.net"
db_name="sample"
echo "running cli"
res=$(./kusto-cli -c "$cluster_uri" -d $db_name -q '.show tables' --use-client-id)
echo $res
