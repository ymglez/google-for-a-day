#!/bin/bash

echo 'EXEC => docker build -t googleforaday-backend backend'
docker build -t googleforaday-backend backend

echo 'EXEC => dotnet publish -c Release -o ../../out backend/GoogleForADay.Services.Api/'
dotnet publish -c Release -o ../../out backend/GoogleForADay.Services.Api/

echo 'EXEC => npm run env -s && npm run build -- --prod'
cd frontend
npm run env -s && npm run build -- --prod