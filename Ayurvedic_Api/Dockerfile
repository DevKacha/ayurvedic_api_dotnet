# Use official .NET SDK to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.sln .
COPY Ayurvedic_Api/*.csproj ./Ayurvedic_Api/
RUN dotnet restore

# Copy everything and build
COPY . .
WORKDIR /src/Ayurvedic_Api
RUN dotnet publish -c Release -o /app/publish

# Use official .NET runtime image for final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Ayurvedic_Api.dll"]
