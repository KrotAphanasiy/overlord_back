#!/bin/bash

# If any of the commands fails, the deploy will fail
set -euo pipefail

echo "Running the new version of the app"
dotnet /app/Flash.Central.AdminApi.dll
