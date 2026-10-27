FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TransitDesk.Core/TransitDesk.Core.csproj", "TransitDesk.Core/"]
COPY ["TransitDesk.Web/TransitDesk.Web.csproj", "TransitDesk.Web/"]
COPY ["TransitDesk.Tests/TransitDesk.Tests.csproj", "TransitDesk.Tests/"]
COPY ["TransitDesk.sln", "./"]

RUN dotnet restore "TransitDesk.sln"

COPY . .
WORKDIR "/src/TransitDesk.Web"
RUN dotnet build "TransitDesk.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransitDesk.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TransitDesk.Web.dll"]