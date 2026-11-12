# Pedidos Web · CI local y despliegue (TEC-05). Sin secretos en la imagen.
# Etapas: build -> test (compuerta) -> publish -> final. publish parte de test: si un test falla, no hay imagen.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# Primero solo la solución y los .csproj: la capa de restore se cachea mientras no cambien las dependencias.
COPY PedidosWeb.sln ./
COPY src/PedidosWeb.Core/PedidosWeb.Core.csproj src/PedidosWeb.Core/
COPY src/PedidosWeb.Api/PedidosWeb.Api.csproj src/PedidosWeb.Api/
COPY tests/PedidosWeb.Core.Tests/PedidosWeb.Core.Tests.csproj tests/PedidosWeb.Core.Tests/
COPY tests/PedidosWeb.Api.Tests/PedidosWeb.Api.Tests.csproj tests/PedidosWeb.Api.Tests/
RUN dotnet restore
# migracion/ trae el golden master que leen los tests.
COPY src/ src/
COPY tests/ tests/
COPY migracion/ migracion/
RUN dotnet build -c Release --no-restore

FROM build AS test
RUN dotnet test -c Release --no-build

FROM test AS publish
RUN dotnet publish src/PedidosWeb.Api/PedidosWeb.Api.csproj -c Release --no-build -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER app
ENTRYPOINT ["dotnet", "PedidosWeb.Api.dll"]
