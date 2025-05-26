# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ["pv311_web_api/pv311_web_api.csproj", "pv311_web_api/"]
COPY ["pv311_web_api.BLL/pv311_web_api.BLL.csproj", "pv311_web_api.BLL/"]
COPY ["pv311_web_api.DAL/pv311_web_api.DAL.csproj", "pv311_web_api.DAL/"]
RUN dotnet restore "pv311_web_api/pv311_web_api.csproj"

# copy everything else and build app
COPY . .
WORKDIR /app/pv311_web_api
RUN dotnet publish -o /app/out


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "pv311_web_api.dll"]