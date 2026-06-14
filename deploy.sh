#!/bin/bash
set -e

cd "$(dirname "$0")"

# Prepare backend's public folder.
rm -rf ./NATSInternal.Backend/NATSInternal.Api/wwwroot
mkdir -p ./NATSInternal.Backend/NATSInternal.Api/wwwroot
echo "Created backend's public folder (wwwroot)"

# Build frontend.
cd ./NATSInternal.Frontend/NATSInternal.React
rm -rf ./dist
npm run build
echo "Built frontend files"

# Move built files to backend's public folder.
mv ./dist/* ../../NATSInternal.Backend/NATSInternal.Api/wwwroot/
rm -rf ./dist
echo "Moved frontend's built files to backend's public folder"

echo "Finished!"
