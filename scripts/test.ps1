docker-compose -f ./scripts/docker-compose.yml up --build -d
Start-Sleep -Milliseconds 2000
Invoke-WebRequest -Uri 'http://localhost:8080/quotes/get_quotes' -Method POST -Body '{}'
docker-compose -f ./scripts/docker-compose.yml down
