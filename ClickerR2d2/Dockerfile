FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClickerR2d2/ClickerR2d2.csproj", "ClickerR2d2/"]
RUN dotnet restore "ClickerR2d2/ClickerR2d2.csproj"
COPY . .
WORKDIR "/src/ClickerR2d2"
RUN dotnet build "ClickerR2d2.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ClickerR2d2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClickerR2d2.dll"]
