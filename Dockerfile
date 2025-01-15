# Step 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["ColorsManagement/ColorsManagement.csproj", "ColorsManagement/"]
RUN dotnet restore "ColorsManagement/ColorsManagement.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR /src/ColorsManagement

# Publish the application (output will be saved in /app/publish)
RUN dotnet publish "ColorsManagement.csproj" -c Release -o /app/publish

# Step 2: Runtime Stage (for production)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5007

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Define the entry point for the application
ENTRYPOINT ["dotnet", "ColorsManagement.dll"]
