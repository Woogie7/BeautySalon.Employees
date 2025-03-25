FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root  # <-- Даем root-права для установки пакетов
RUN apt-get update && apt-get install -y locales && rm -rf /var/lib/apt/lists/*
RUN locale-gen en_US.UTF-8
ENV LANG en_US.UTF-8
ENV LC_ALL en_US.UTF-8

USER app   # <-- Возвращаем обратно к app-пользователю
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BeautySalon.Employees.Api/BeautySalon.Employees.Api.csproj", "BeautySalon.Employees.Api/"]
COPY ["BeautySalon.Employees.Persistence/BeautySalon.Employees.Persistence.csproj", "BeautySalon.Employees.Persistence/"]
COPY ["BeautySalon.Employees.Domain/BeautySalon.Employees.Domain.csproj", "BeautySalon.Employees.Domain/"]
RUN dotnet restore "./BeautySalon.Employees.Api/BeautySalon.Employees.Api.csproj"
COPY . .
WORKDIR "/src/BeautySalon.Employees.Api"
RUN dotnet build "./BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeautySalon.Employees.Api.dll"]