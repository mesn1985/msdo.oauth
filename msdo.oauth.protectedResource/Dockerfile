#  ATTENTION! The docker image, are only intented for educational use, and should never be used in a production enviroment, or other live enviroments
# Author: Martin Edwin Schjødt Nielsen
# Build: docker build . -t lightmaze/oauthprotectedresourceserver
# Push to docker repo: docker push lightmaze/oauthprotectedresourceserver
# Run container: docker run -p 5002:5002 lightmaze/oauthprotectedresourceserver

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . msdo.oauth.protectedResource/
RUN dotnet restore "./msdo.oauth.protectedResource/msdo.oauth.protectedResource.csproj"
COPY . .
WORKDIR "/src/msdo.oauth.protectedResource"
RUN dotnet build "./msdo.oauth.protectedResource.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./msdo.oauth.protectedResource.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_HTTP_PORTS=5002
ENV ASPNETCORE_URLS=http://0.0.0.0:5002
ENV ASPNETCORE_ENVIROMENT=Development
CMD ["dotnet", "msdo.oauth.protectedResource.dll"]