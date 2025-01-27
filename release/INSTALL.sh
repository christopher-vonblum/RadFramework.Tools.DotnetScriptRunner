#!/bin/sh -v

SCRIPT_DIR=$(readlink -m $0)
TEMP=$(dirname "${SCRIPT_DIR}")
SCRIPT_DIR="${TEMP}"
echo "${SCRIPT_DIR}"

cd ..
sudo chown -R "${USER}" "${SCRIPT_DIR}"
cd "${SCRIPT_DIR}"

sudo chmod -R a+rwx "${SCRIPT_DIR}"

echo -n "${SCRIPT_DIR}" > cs-script.cfg

sudo ln -sfn "${SCRIPT_DIR}/cs-script" /usr/bin/cs-script
sudo ln -sfn "${SCRIPT_DIR}/cs-script.cfg" /usr/bin/cs-script.cfg

chmod +x "${SCRIPT_DIR}/TestScript.cs"
exec "${SCRIPT_DIR}/TestScript.cs"
