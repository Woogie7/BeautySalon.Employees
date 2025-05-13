FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Копируем только NuGet.Config сначала
COPY NuGet.Config .

# 2. Обновляем учетные данные источника (вместо добавления нового)
RUN sed -i 's/%GITHUB_TOKEN%/'"$GITHUB_TOKEN"'/g' NuGet.Config

# 3. Копируем остальные файлы
COPY . .

# 4. Восстанавливаем зависимости
RUN dotnet restore "BeautySalon.Employees.Api/BeautySalon.Employees.Api.csproj"

# 5. Сборка
WORKDIR "/src/BeautySalon.Employees.Api"
RUN dotnet build "BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeautySalon.Employees.Api.dll"]