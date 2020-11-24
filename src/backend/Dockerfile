FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore GoogleForADay.Services.Api
RUN dotnet publish GoogleForADay.Services.Api -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim
WORKDIR /app
COPY --from=build-env /app/GoogleForADay.Services.Api/out .

ENTRYPOINT ["dotnet", "GoogleForADay.Services.Api.dll"]