#!/bin/bash

echo "installing kusto-cli"

# install dir, relative to /usr/local
config=Release
project_path=src/kusto-cli.csproj
bin_path=/usr/local/bin/kusto-cli
install_dir_part='personal/kusto-cli'
install_dir=/usr/local/$install_dir_part
link_path="../$install_dir_part/kusto-cli"

echo "Publishing kusto-cli to $install_dir with config: $config"
dotnet publish -c $config -o $install_dir $project_path
echo "Creating symlink at $bin_path to $link_path"
ln -s $link_path $bin_path
