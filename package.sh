#!/bin/bash

COMPONENT=$(ls *.nuspec | tr -d '\r' | awk -F. '{ print $1 }')
VERSION=$(grep -m1 "<version>" *.nuspec | tr -d '\r' | sed 's/[ ]//g' | awk -F ">" '{ print $2 }' | awk -F "<" '{ print $1 }')
IMAGE1="tsnaveen/${COMPONENT}:${VERSION}-${BUILD_NUMBER-0}"
IMAGE2="tsnaveen/${COMPONENT}:latest"
TAG="v${VERSION}-${BUILD_NUMBER-0}"

# Any subsequent(*) commands which fail will cause the shell scrupt to exit immediately
set -e
set -o pipefail

# Build docker image
docker build -f Dockerfile -t ${IMAGE1} -t ${IMAGE2} .

# Workaround to remove dangling images
docker-compose -f ./docker-compose.yml down

export IMAGE
docker-compose -f ./docker-compose.yml up -d

sleep 1.5

# Test using curl
#curl http://localhost:8080/quotes/get_quotes -X POST -v
wget --post-data="{}" http://localhost:8080/quotes/get_quotes

# Workaround to remove dangling images
docker-compose -f ./docker-compose.yml down

