# Overview
Finally! Pass through user auth on non-windows.

This is mostly just a small hack to enable [https://github.com/gdryke/vim-kql], but I'll try to make it as much of a thing as I can.

# Release Notes

## 0.2
### New
### Fixed
### Removed
### Changed

## 0.1
This version.
Basically, it works for prompted user creds, and some basic app key/secret with `--use-client-id`.
It also has some support for different outputs (those supported by the underlying Kusto library) such as Text, JSON, CSV, TSV.

Not much else is supported, no queries from stdin, no direct out to files, etc.
There's some real hacky support for an "install" process in `install.sh`, but you'll want to tailor it to your machine.

Helpful tutorials:
- https://github.com/Azure/azure-kusto-samples-dotnet/blob/9ac0485ec7e937be104c651f38eba70add1aae41/client/HelloKusto/Program.cs
- https://docs.microsoft.com/en-us/azure/data-explorer/kusto/api/powershell/powershell

TODO:
- Probably lots of better stuff i can do with all the fun query parameters
- A real deployment process so it's in bin
  - Shell script that calls it?
- integration testing using test cluster, that might be unathed?
  - This is actually kinda working with the `integration.sh` and `integraton.yml` workflow. See `docs/integration-testing.md` for more details.
- query from stdin
- Adding to brew or something?
- Try to get smaller output
- More tests
- Test multi-line query
- Queries from stdin
- ENV or other config for cluster/db for repeated use
