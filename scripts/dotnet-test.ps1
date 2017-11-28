Write-Information "Initiated the process of testing application..."

Write-Warning "Assuming MongoDB is running locally.. if not start it by calling 'mongod --dbpath <folder path to database>' and rerun the script"

# Build
Invoke-Expression ./scripts/dotnet-build.ps1

# Run the tests
dotnet test test/test.csproj
