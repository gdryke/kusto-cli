Finally! Pass through user auth on non-windows.

Helpful tutorials:
- https://github.com/Azure/azure-kusto-samples-dotnet/blob/9ac0485ec7e937be104c651f38eba70add1aae41/client/HelloKusto/Program.cs
- https://docs.microsoft.com/en-us/azure/data-explorer/kusto/api/powershell/powershell

TODO:
- Probably lots of better stuff i can do with all the fun query parameters
- A real deployment process so it's in bin
  - Shell script that calls it?
- integration testing using test cluster, that might be unathed?
  - need a new, optional, auth call probably. https://github.com/gdryke/kusto-cli/runs/1579503747?check_suite_focus=true
- query from stdin
- Adding to brew or something?
- Try to get smaller output
- More tests
- Test multi-line query
- Queries from stdin
- ENV or other config for cluster/db for repeated use
