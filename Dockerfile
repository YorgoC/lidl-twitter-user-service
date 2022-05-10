#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["lidl-twitter-user-service.csproj", "."]
RUN dotnet restore "./lidl-twitter-user-service.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "lidl-twitter-user-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lidl-twitter-user-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lidl-twitter-user-service.dll"]
