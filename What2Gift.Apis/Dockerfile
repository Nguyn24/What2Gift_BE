FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["What2Gift.Apis/What2Gift.Apis.csproj", "What2Gift.Apis/"]
RUN dotnet restore "What2Gift.Apis/What2Gift.Apis.csproj"
COPY . .
WORKDIR "/src/What2Gift.Apis"
RUN dotnet build "./What2Gift.Apis.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./What2Gift.Apis.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "What2Gift.Apis.dll"]
