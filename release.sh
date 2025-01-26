#!/bin/sh
cp src/cs-script/INSTALL.sh release
cp src/cs-script/UNINSTALL.sh release
cp src/cs-script/TestScript.csx release
mkdir release/cs-script-data
cp src/cs-script/cs-script-data/script-project.xml release/cs-script-data
