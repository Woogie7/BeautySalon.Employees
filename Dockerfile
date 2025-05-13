FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Сначала копируем только файлы конфигурации
COPY NuGet.Config .
COPY BeautySalon.Employees.sln .

# 2. Добавляем аутентификацию для GitHub Packages
RUN dotnet nuget add source https://nuget.pkg.github.com/Woogie7/index.json \
    -n github -u Woogie7 -p $GITHUB_TOKEN --store-password-in-clear-text

# 3. Копируем остальные файлы
COPY . .

# 4. Восстанавливаем зависимости
RUN dotnet restore "BeautySalon.Employees.Api/BeautySalon.Employees.Api.csproj"

# 5. Собираем проект
WORKDIR "/src/BeautySalon.Employees.Api"
RUN dotnet build "BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeautySalon.Employees.Api.dll"]