# Release Notes

## 0.3

### New
- User Assigned MSI support!
  - `UseUserAssignedMSI` and then the environment variable `KUSTOCLI_CLIENT_ID`
  - I'm being explicit about user vs system here, that way you can have the client ID env present and not use it with a (future) `UseSystemAssignedMSI` flag
  - Move client id/client secret/authority away from ENV into parameters would make this uncessary and allow for extrapolating of intent for MSI auth based on parameters
### Fixed
- You can pass in just a cluster name and it will make it into a URI
### Removed
### Changed
- ClientId now has another distinct, mutually exclusive use, with user assigned MSI

## 0.2
Few little todo list things that I'd already done.
Also added some new query input options!
Also `-h` and a usage page.
### New
- Integration test! You need to build the cluster, the script _might_ work, but you can run a full integration test with `integration-test-full.sh`
- There's a somewhat working install script at `install.sh`. It tries to publish and link things to bin, but may be flaky and is very machine specific at this point
- `-i/--query-file` and standard input are now available for inputting queries!
  - Note: right now there's no validation for using 2 or all 3 query input options, it will queries in the following order
    - `Query Parameter -> Query File Parameter -> Standard In`
  - Query File is just read in with UTF8 encoding. I think Kusto probably has broad unicode support though, so this should be updated.
- New `-h` option to output usage
- Usage page added at [docs/usage.md](usage.md)
### Fixed
- Fixed text in argument validation.
### Removed
### Changed

## 0.1
This version.
Basically, it works for prompted user creds, and some basic app key/secret with `--use-client-id`.
It also has some support for different outputs (those supported by the underlying Kusto library) such as Text, JSON, CSV, TSV.

Not much else is supported, no queries from stdin, no direct out to files, etc.
There's some real hacky support for an "install" process in `install.sh`, but you'll want to tailor it to your machine.
