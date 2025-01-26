#!/bin/sh
cd ..
CurrentUser=$USER
sudo chown $CurrentUser RadFramework.Tools.DotnetScriptRunner -R
cd RadFramework.Libraries
echo "$CurrentUser owns the project now."
