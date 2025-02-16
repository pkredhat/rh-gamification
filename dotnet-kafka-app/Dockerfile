# Use .NET SDK as the base image
FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build

# Set working directory
WORKDIR /app

# Clone the repository from GitHub
RUN apt-get update && apt-get install -y git && \
    git clone --depth 1 https://github.com/<your-github-username>/<your-repo>.git .

# Restore dependencies
RUN dotnet restore --no-cache

# Build the application
RUN dotnet publish -c Release -o /app/out

# Use a smaller runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose the application port
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "<YourAppName>.dll"]





# # Use the latest .NET 9 preview SDK image (replace with actual tag when available)
# FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0 AS build
# WORKDIR /app

# # Copy project files and restore dependencies
# COPY *.csproj ./
# RUN dotnet restore --no-cache

# # Copy the entire source and build the application
# COPY . ./
# RUN dotnet publish -r linux-x64 --self-contained -c Release -o ./publish








# # Use a smaller runtime image
# FROM mcr.microsoft.com/dotnet/nightly/aspnet:9.0 AS runtime
# WORKDIR /app
# COPY --from=build /out ./

# # Run the application
# ENTRYPOINT ["dotnet", "dotnet-kafka-app.dll"]