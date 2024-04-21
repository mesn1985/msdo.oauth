#  ATTENTION! The docker image, are only intented for educational use, and should never be used in a production enviroment, or other live enviroments
# Author: Martin Edwin Schjødt Nielsen
# Build: docker build . -t lightmaze/oauthauthserver
# Push to docker repo: docker push lightmaze/oauthauthserver
# Run container: docker run -p 5001:5001 lightmaze/oauthauthserver

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . msdo.oauth.identityServer/
RUN dotnet restore "./msdo.oauth.identityServer/msdo.oauth.identityServer.csproj"
WORKDIR "/src/msdo.oauth.identityServer"
RUN dotnet build "./msdo.oauth.identityServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./msdo.oauth.identityServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:5001
ENV ASPNETCORE_ENVIROMENT=Development
CMD ["dotnet", "msdo.oauth.identityServer.dll"]