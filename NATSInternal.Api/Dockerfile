# ========= BUILD STAGE =========
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore dependencies
COPY ["NATSInternal.Api/NATSInternal.Api.csproj", "NATSInternal.Api/"]
COPY ["NATSInternal.Application/NATSInternal.Application.csproj", "NATSInternal.Application/"]
COPY ["NATSInternal.Infrastructure/NATSInternal.Infrastructure.csproj", "NATSInternal.Infrastructure/"]
RUN dotnet restore "NATSInternal.Api/NATSInternal.Api.csproj"
RUN dotnet restore "NATSInternal.Application/NATSInternal.Application.csproj"
RUN dotnet restore "NATSInternal.Infrastructure/NATSInternal.Infrastructure.csproj"

# Copy all source code and publish
COPY . .
WORKDIR "/src/NATSInternal.Api"
RUN dotnet publish -c Release -o /app/publish

# ========= RUNTIME STAGE =========
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Optional: set ASP.NET environment
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "NATSInternal.Api.dll"]
