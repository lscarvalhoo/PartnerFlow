FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PartnerFlow.sln .
COPY PartnerFlow.Domain/PartnerFlow.Domain.csproj PartnerFlow.Domain/
COPY PartnerFlow.Application/PartnerFlow.Application.csproj PartnerFlow.Application/
COPY PartnerFlow.Infrastructure/PartnerFlow.Infrastructure.csproj PartnerFlow.Infrastructure/
COPY PartnerFlow.API/PartnerFlow.API.csproj PartnerFlow.API/
COPY PartnerFlow.Tests/PartnerFlow.Tests.csproj PartnerFlow.Tests/

RUN dotnet restore PartnerFlow.sln

COPY . .

RUN dotnet publish PartnerFlow.API/PartnerFlow.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PartnerFlow.API.dll"]