#!/bin/sh
cp doc/README.txt release
cp src/cs-script/INSTALL.sh release
cp src/cs-script/UNINSTALL.sh release
cp src/cs-script/TestScript.csx release/TestScript.cs
mkdir release/cs-script-data
cp src/cs-script/cs-script-data/script-project.xml release/cs-script-data
cp -r release cs-script-1.0
zip -r cs-script-1.0.zip cs-script-1.0
rm -R cs-script-1.0
mv cs-script-1.0.zip download
