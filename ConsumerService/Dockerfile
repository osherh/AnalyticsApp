FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["ConsumerService/ConsumerService.csproj", "ConsumerService/"]
RUN dotnet restore "ConsumerService/ConsumerService.csproj"

# Copy everything else and publish
COPY . .
WORKDIR /src/ConsumerService
RUN dotnet publish "ConsumerService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ConsumerService.dll"]
