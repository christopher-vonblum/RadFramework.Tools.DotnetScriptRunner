#!/bin/bash

SCRIPT_DIR=$(readlink -n $0)
TEMP=$(dirname "${SCRIPT_DIR}")
SCRIPT_DIR="${TEMP}"
echo "${SCRIPT_DIR}"

chmod +x MANUAL-INSTALL.sh
bash ./MANUAL-INSTALL.sh "${SCRIPT_DIR}"
