#!/bin/bash

echo "installing kusto-cli"

# install dir, relative to /usr/local
config=Release
project_path=src/kusto-cli.csproj
link_path=src/bin/$config/netcoreapp3.1/publish/kusto-cli
bin_path="kusto-cli"

echo "Publishing kusto-cli to $install_dir with config: $config"
dotnet publish -c $config $project_path
echo "Creating symlink at $bin_path to $link_path"
ln -s $link_path $bin_path
