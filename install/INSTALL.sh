#!/bin/bash

SCRIPT_DIR=$(readlink -n $0)
TEMP=$(dirname "${SCRIPT_DIR}")
SCRIPT_DIR="${TEMP}"
SCRIPT_DIR=$(realpath "${SCRIPT_DIR}")
echo "${SCRIPT_DIR}"

sudo chmod +x MANUAL-INSTALL.sh
sudo ./MANUAL-INSTALL.sh "${SCRIPT_DIR}"
