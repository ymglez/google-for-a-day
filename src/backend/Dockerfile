FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build-env
WORKDIR /app

#Compile

COPY . ./
RUN dotnet restore GoogleForADay.Services.Api
RUN dotnet publish GoogleForADay.Services.Api -c Release -o out


#Runtime
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine

WORKDIR /app
COPY --from=build-env /app/GoogleForADay.Services.Api/out .
## Add lmdb lib 
RUN apk update
RUN apk add lmdb
RUN cp /usr/lib/liblmdb.so.0 ./lmdb.so

ENTRYPOINT ["dotnet", "GoogleForADay.Services.Api.dll"]