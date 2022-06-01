#!/bin/sh

set -euo pipefail

echo "Starting"
ln -s /usr/share/nginx/html /usr/share/nginx/html/docs
echo "Healthy" > /usr/share/nginx/html/health

echo "Validating..."
nginx -t || exit 1
echo "Running..."
nginx -g 'daemon off;'
