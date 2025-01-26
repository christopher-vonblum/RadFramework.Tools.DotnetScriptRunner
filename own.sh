#!/bin/sh
cd ..
CurrentUser=$USER
sudo chown $CurrentUser RadFramework.Tools.DotnetScriptRunner -R
cd RadFramework.Tools.DotnetScriptRunner
echo "$CurrentUser owns the project now."
