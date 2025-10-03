FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["What2Gift.Apis/What2Gift.Apis.csproj", "What2Gift.Apis/"]
COPY ["What2Gift.Application/What2Gift.Application.csproj", "What2Gift.Application/"]
COPY ["What2Gift.Domain/What2Gift.Domain.csproj", "What2Gift.Domain/"]
COPY ["What2Gift.Infrastructure/What2Gift.Infrastructure.csproj", "What2Gift.Infrastructure/"]

RUN dotnet restore "What2Gift.Apis/What2Gift.Apis.csproj"

COPY . .

WORKDIR "/src/What2Gift.Apis"
RUN dotnet publish "What2Gift.Apis.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "What2Gift.Apis.dll"]
