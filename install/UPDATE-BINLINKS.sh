#!/bin/bash

installationPath=`cat cs-script.cfg`
echo $installationPath

sudo rm /usr/bin/cs-script
sudo rm /usr/bin/cs-script.cfg

sudo ln -sfn "${installationPath}/cs-script" /usr/bin/cs-script
sudo ln -sfn "${installationPath}/cs-script.cfg" /usr/bin/cs-script.cfg
