FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Endpoints/MobilePay.Endpoints.WebApi/MobilePay.Endpoints.WebApi.csproj", "src/Endpoints/MobilePay.Endpoints.WebApi/"]
COPY ["src/core/MobilePay.Core.ApplicationService/MobilePay.Core.ApplicationService.csproj", "src/core/MobilePay.Core.ApplicationService/"]
COPY ["src/Core/MobilePay.Core.Domain/MobilePay.Core.Domain.csproj", "src/Core/MobilePay.Core.Domain/"]
COPY ["src/BuildingBlocks/Tools/Framework.Tools/Framework.Tools.csproj", "src/BuildingBlocks/Tools/Framework.Tools/"]
COPY ["src/Infrastructures/MobilePay.Infrastructures.Data/MobilePay.Infrastructures.Data.csproj", "src/Infrastructures/MobilePay.Infrastructures.Data/"]
RUN dotnet restore "src/Endpoints/MobilePay.Endpoints.WebApi/MobilePay.Endpoints.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Endpoints/MobilePay.Endpoints.WebApi"
RUN dotnet build "MobilePay.Endpoints.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MobilePay.Endpoints.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MobilePay.Endpoints.WebApi.dll"]