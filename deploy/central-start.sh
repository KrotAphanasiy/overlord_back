#!/bin/bash

# If any of the commands fails, the deploy will fail
set -euo pipefail

chmod +x /app/wait-for-postgresql.sh

if [ "${FLASH_DEBUG_MODE:-false}" = 'true' ]
then
    echo "Waiting for postgres..."
    /app/wait-for-postgresql.sh $FLASH_DB_HOST $FLASH_DB_USER $FLASH_DB_PASSWORD
fi

echo "Running the new version of the app"
dotnet /app/Flash.Central.Api.dll --Migrate=Up || exit 1
