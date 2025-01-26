#!/bin/sh
cd ..
CurrentUser=$USER
sudo chown $CurrentUser cs-script -R
cd cs-script
echo "$CurrentUser owns the project now."
