# # Stage 1: Build
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src

# # Copy solution file
# COPY Dorfo_Server.sln ./

# # Copy csproj cho từng project
# COPY Dorfo.API/Dorfo.API.csproj Dorfo.API/
# COPY Dorfo.Application/Dorfo.Application.csproj Dorfo.Application/
# COPY Dorfo.Domain/Dorfo.Domain.csproj Dorfo.Domain/
# COPY Dorfo.Infrastructure/Dorfo.Infrastructure.csproj Dorfo.Infrastructure/
# COPY Dorfo.Shared/Dorfo.Shared.csproj Dorfo.Shared/

# # Restore dependencies
# RUN dotnet restore Dorfo_Server.sln

# # Copy toàn bộ source code
# COPY . .

# # Publish project API
# WORKDIR /src/Dorfo.API
# RUN dotnet publish -c Release -o /app/publish

# # Stage 2: Runtime
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
# WORKDIR /app
# COPY --from=build /app/publish .
# ENTRYPOINT ["dotnet", "Dorfo.API.dll"]


# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution và file project để tận dụng cache
COPY Dorfo_Server.sln ./
COPY Dorfo.API/Dorfo.API.csproj Dorfo.API/
COPY Dorfo.Application/Dorfo.Application.csproj Dorfo.Application/
COPY Dorfo.Domain/Dorfo.Domain.csproj Dorfo.Domain/
COPY Dorfo.Infrastructure/Dorfo.Infrastructure.csproj Dorfo.Infrastructure/
COPY Dorfo.Shared/Dorfo.Shared.csproj Dorfo.Shared/

# Restore packages (tận dụng cache)
RUN dotnet restore

# Copy toàn bộ source code
COPY . .

# Build và publish
WORKDIR /src/Dorfo.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime (nhẹ hơn, chỉ cần aspnet)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Dorfo.API.dll"]
