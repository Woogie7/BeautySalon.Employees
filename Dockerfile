FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["BeautySalon.Employees.Api/BeautySalon.Employees.Api.csproj", "BeautySalon.Employees.Api/"]
COPY ["BeautySalon.Employees.Application/BeautySalon.Employees.Application.csproj", "BeautySalon.Employees.Application/"]
COPY ["BeautySalon.Employees.Domain/BeautySalon.Employees.Domain.csproj", "BeautySalon.Employees.Domain/"]
COPY ["BeautySalon.Employees.Infrastructure/BeautySalon.Employees.Infrastructure.csproj", "BeautySalon.Employees.Infrastructure/"]
COPY ["BeautySalon.Employees.Persistence/BeautySalon.Employees.Persistence.csproj", "BeautySalon.Employees.Persistence/"]
COPY ["BeautySalon.Contracts/BeautySalon.Contracts.csproj", "BeautySalon.Contracts/"]

RUN dotnet restore "BeautySalon.Employees.Api/BeautySalon.Employees.Api.csproj"
COPY . .
WORKDIR "/src/BeautySalon.Employees.Api"
RUN dotnet build "BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BeautySalon.Employees.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeautySalon.Employees.Api.dll"]
