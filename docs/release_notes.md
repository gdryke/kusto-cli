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
