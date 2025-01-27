#!/bin/bash

installationPath < cs-script.cfg

sudo ln -sfn "${installationPath}/cs-script" /usr/bin/cs-script
sudo ln -sfn "${installationPath}/cs-script.cfg" /usr/bin/cs-script.cfg
