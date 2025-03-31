FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root


RUN apt-get update && apt-get install -y locales \
    && echo "ru_RU.UTF-8 UTF-8" > /etc/locale.gen \
    && locale-gen

ENV LANG=ru_RU.UTF-8
ENV LC_ALL=ru_RU.UTF-8
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

USER app
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


USER app 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BeautySalon.Employees.Api.dll"]
