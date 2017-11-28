Write-Information "Initiated the process of building application..."

# Remove build files
if(Test-Path ./obj) { Remove-Item ./obj -Force -Recurse; }
if(Test-Path ./bin) { Remove-Item ./bin -Force -Recurse; }

# Build 
dotnet restore run/run.csproj
dotnet restore src/src.csproj
dotnet restore test/test.csproj
dotnet build .