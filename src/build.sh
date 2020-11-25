#!/bin/bash

echo 'EXEC => docker build -t googleforaday-backend backend'
docker build -t googleforaday-backend backend

echo 'EXEC => dotnet publish -c Release -o ../../out backend/GoogleForADay.Services.Api/'
dotnet publish -c Release -o ../../out backend/GoogleForADay.Services.Api/

echo 'EXEC => npm run env -s && npm run build -- --prod'
  
 #BASH
 docker run --name dotnet-app-container -d -p 44340:80 -p 44341:443 -e ASPNETCORE_URLS="https://+:443;http://+:80" -e ASPNETCORE_HTTPS_PORT=44341 -e ASPNETCORE_Kestrel__Certificates__Default__Password=password -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v %USERPROFILE%/.aspnet/https:/https:ro googleforaday-backend

 # CMD
 docker run --name dotnet-app-alpine-container -d -p 44340:80 -p 44341:443 -e ASPNETCORE_URLS="https://+:443;http://+:80" -e ASPNETCORE_HTTPS_PORT=44341 -e ASPNETCORE_Kestrel__Certificates__Default__Password=password -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v %USERPROFILE%/.aspnet/https:/https:ro googleforaday-backend-alpine 