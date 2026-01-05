FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["ProducerService/ProducerService.csproj", "ProducerService/"]
RUN dotnet restore "ProducerService/ProducerService.csproj"

# Copy everything else and publish
COPY . .
WORKDIR /src/ProducerService
RUN dotnet publish "ProducerService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProducerService.dll"]
