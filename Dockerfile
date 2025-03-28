FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base


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


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

RUN apt-get update && apt-get install -y locales && \
    locale-gen ru_RU.UTF-8 && \
    update-locale LANG=ru_RU.UTF-8
ENV LANG=ru_RU.UTF-8
ENV LC_ALL=ru_RU.UTF-8

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BeautySalon.Employees.Api.dll"]
