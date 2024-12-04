# Use the official .NET image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published app from the build environment
COPY --from=build-env /app/out ./

# Expose ports
EXPOSE 80
EXPOSE 443

# Run the application
ENTRYPOINT ["dotnet", "Social-Media-Server.dll"]
