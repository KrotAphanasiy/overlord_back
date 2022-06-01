#!/bin/bash
# wait-for-postgres.sh
# This script is only needed for local run. It should never be run in production environment or on any staging.

set -e

echo "wait-for-postgresql.sh"
  
until PGPASSWORD=$3 psql -h "$1" -U "$2" -c '\q'; do
  >&2 echo "Postgres is unavailable - sleeping"
  sleep 1
done
  
>&2 echo "Postgres is up - executing command"
