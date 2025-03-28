# Базовый образ для финального контейнера
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Устанавливаем локали вручную
RUN apt-get update && apt-get install -y locales \
    && echo "ru_RU.UTF-8 UTF-8" > /etc/locale.gen \
    && locale-gen

ENV LANG=ru_RU.UTF-8
ENV LC_ALL=ru_RU.UTF-8

USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Образ для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем и восстанавливаем зависимости
COPY ["BeautySalon.Employees.Api/BeautySalon.Employees.Api.csproj", "BeautySalon.Employees.Api/"]
COPY ["BeautySalon.Employees.Persistence/BeautySalon.Employees.Persistence.csproj", "BeautySalon.Employees.Persistence/"]
COPY ["BeautySalon.Employees.Domain/BeautySalon.Employees.Domain.csproj", "BeautySalon.Employees.Domain/"]
RUN dotnet restore "./BeautySalon.Employees.Api/BeautySalon.Employees.Api.csproj"

# Копируем остальные файлы и собираем проект
COPY . .
WORKDIR "/src/BeautySalon.Employees.Api"
RUN dotnet build "./BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикуем приложение
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BeautySalon.Employees.Api.dll"]
