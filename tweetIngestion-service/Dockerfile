FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["tweetIngestion-service/tweetIngestion-service.csproj", "tweetIngestion-service/"]
RUN dotnet restore "tweetIngestion-service/tweetIngestion-service.csproj"
COPY . .
WORKDIR "/src/tweetIngestion-service"
RUN dotnet build "tweetIngestion-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tweetIngestion-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tweetIngestion-service.dll"]
