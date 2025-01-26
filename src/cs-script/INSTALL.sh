#!/bin/sh -v

SCRIPT_DIR="$(dirname ${0})"
cd ..
sudo chown "${USER}" "${SCRIPT_DIR}" -R
cd "${SCRIPT_DIR}"

echo -n "${SCRIPT_DIR}" > cs-script.cfg

sudo ln -sfn "${SCRIPT_DIR}/cs-script" /usr/bin/cs-script
sudo ln -sfn "${SCRIPT_DIR}/cs-script.cfg" /usr/bin/cs-script.cfg

chmod +x "${SCRIPT_DIR}/TestScript.csx
.${SCRIPT_DIR}/TestScript.csx
