# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ["pd311_web_api/pd311_web_api.csproj", "pd311_web_api/"]
COPY ["pd311_web_api.BLL/pd311_web_api.BLL.csproj", "pd311_web_api.BLL/"]
COPY ["pd311_web_api.DAL/pd311_web_api.DAL.csproj", "pd311_web_api.DAL/"]
RUN dotnet restore "pd311_web_api/pd311_web_api.csproj"

# copy everything else and build app
COPY . .
WORKDIR /app/pd311_web_api
RUN dotnet publish -o /app/out


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "pd311_web_api.dll"]