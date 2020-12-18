#/bin/bash
res=$(./kusto-cli -c https://help.kusto.windows.net -d Samples -q 'StormEvents | take 1 | project StartTime, State, Source')
echo $res
