# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

# copy and build app
COPY src .
RUN dotnet publish -c release "Server\BirToolsApp.Server.csproj" -o /publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "BirToolsApp.Server.dll"]