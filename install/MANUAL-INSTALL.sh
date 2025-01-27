#!/bin/bash

SCRIPT_DIR="$1"

chown -R "${USER}" "${SCRIPT_DIR}"

chmod -R a+rwx "${SCRIPT_DIR}"

echo -n "${SCRIPT_DIR}" > cs-script.cfg

chmod -R a+rwx cs-script.cfg

./UPDATE-BIN-LINKS.sh

chmod +x "${SCRIPT_DIR}/TestScript.cs"
bash "${SCRIPT_DIR}/TestScript.cs"
