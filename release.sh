#!/bin/sh

cp doc/README.txt release
cp install/INSTALL.sh release
cp install/MANUAL-INSTALL.sh release
cp install/UNINSTALL.sh release
cp install/UPDATE-BINLINKS.sh release
cp src/cs-script/TestScript.csx release/TestScript.cs
rm -R release/cs-script-data
mkdir release/cs-script-data
cp src/cs-script/cs-script-data/script-project.xml release/cs-script-data
cp -r release cs-script-1.0
zip -r cs-script-1.0.zip cs-script-1.0
rm -R cs-script-1.0
rm download/cs-script-1.0.zip
mv cs-script-1.0.zip download
