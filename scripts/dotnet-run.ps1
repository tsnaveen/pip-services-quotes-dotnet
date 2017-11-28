Write-Information "Initiated the process of running application..."

Write-Warning "Assuming MongoDB is running locally.. if not start it by calling 'mongod --dbpath <folder path to database>' and rerun the script"

# Build
Invoke-Expression ./scripts/dotnet-build.ps1

# Run the app
dotnet ./run/bin/debug/netcoreapp2.0/run.dll -c ./config/config.yml
