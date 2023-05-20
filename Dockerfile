FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS http://*:5001
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore 
WORKDIR "/src/CAEVSYNC.Api"
RUN dotnet build "CAEVSYNC.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CAEVSYNC.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CAEVSYNC.API.dll"]