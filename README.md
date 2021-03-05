# Overview
Finally! Pass through user auth on non-windows.

This is mostly just a small hack to enable [https://github.com/gdryke/vim-kql], but I'll try to make it as much of a thing as I can.

# Installation
Try out `install.sh`, results very much may vary.

# Usage
See [docs/usage.md]

# Release Notes
https://github.com/gdryke/kusto-cli/tree/main/docs/release_notes.md

## Current Version
0.2

# TODO
- [x] query from stdin
- [ ] Add to brew, even if just local for the learning
- [ ] Try to get smaller output
- [ ] More tests
- [ ] Add test multi-line query
- [ ] Probably lots of better stuff i can do with all the fun Kusto query parameters
- [ ] Any way to get charts/timeseries?
- [ ] ENV or other config for cluster/db for repeated use
- [ ] Better encoding for a query file

# DONE

- A real deployment process so it's in bin
  - Shell script that calls it? Kind of have this.
- integration testing using test cluster, that might be unauthed?
  - This is actually kinda working with the `integration.sh` and `integraton.yml` workflow. See `docs/integration-testing.md` for more details.

# Links
Helpful tutorials:
- https://github.com/Azure/azure-kusto-samples-dotnet/blob/9ac0485ec7e937be104c651f38eba70add1aae41/client/HelloKusto/Program.cs
- https://docs.microsoft.com/en-us/azure/data-explorer/kusto/api/powershell/powershell

