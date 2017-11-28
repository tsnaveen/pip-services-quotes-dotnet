$Component = (Get-Item *.nuspec).BaseName
$Version = (select-string -Path '*.nuspec' -Pattern '<version>\b[0-9.]+\b</version>' -AllMatches | % { $_.Matches } | % { $_.Value }).ToLower().Replace("<version>", "").Replace("</version>", "")
$ImageTag1 = "tsnaveen/${Component}:latest"
if (-not $BUILD_NUMBER) { $BUILD_NUMBER = 0}

$ImageTag2 = "tsnaveen/${Component}:${Version}-${BUILD_NUMBER}"
$TagVersion = "v${Version}-${BUILD_NUMBER}"

# Workaround to remove dangling images
docker-compose -f ./scripts/docker-compose.yml down

docker-compose -f ./scripts/docker-compose.yml up --build -d

# To make sure the uri is up and running and make sure webrequest does not fail
# Test using curl
Start-Sleep -Milliseconds 2000
Invoke-WebRequest -Uri 'http://localhost:8080/quotes/get_quotes' -ContentType 'application/json'  -Method POST -Body '{}'

# Tag the images
docker tag ${ImageTag1} ${ImageTag2}

# Workaround to remove dangling images
docker-compose -f ./scripts/docker-compose.yml down

# Set tag on git repo
git tag ${TagVersion}
git push --tags

# Push production image to docker registry
docker login -u ${DOCKER_USER} -p ${DOCKER_PASS}
docker push ${ImageTag1}
docker push ${ImageTag2}

