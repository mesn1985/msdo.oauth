# Author: Martin Edwin Schjødt Nielsen
# Build: docker build . -t lightmaze/oauthclient
# Push to docker repo: docker push lightmaze/oauthclient
# Run container: docker run -p 8080:8080 lightmaze/oauthclient

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . msdo.oauth.client/
RUN dotnet restore "./msdo.oauth.client/msdo.oauth.client.csproj"
WORKDIR "/src/msdo.oauth.client"
RUN dotnet build "./msdo.oauth.client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./msdo.oauth.client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:5003
CMD ["dotnet", "msdo.oauth.client.dll"]