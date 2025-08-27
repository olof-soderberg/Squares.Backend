FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Squares.sln", "./"]
COPY ["Squares.Api/*.csproj", "./Squares.Api/"]
COPY ["Squares.Domain/*.csproj", "./Squares.Domain/"]
COPY ["Squares.Infrastructure/*.csproj", "./Squares.Infrastructure/"]
COPY ["Squares.AppHost/*.csproj", "./Squares.AppHost/"]
COPY ["Squares.ServiceDefaults/*.csproj", "./Squares.ServiceDefaults/"]
COPY ["Squares.StreamTestApplication/*.csproj", "./Squares.StreamTestApplication/"]
COPY ["Squares.StressTestApplication/*.csproj", "./Squares.StressTestApplication/"]
COPY ["Squares.Domain.UnitTests/*.csproj", "./Squares.Domain.UnitTests/"]

RUN dotnet restore Squares.sln

COPY . .

WORKDIR /src/Squares.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNETCORE_HTTP_PORTS=7280
EXPOSE 7280
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Squares.Api.dll"]