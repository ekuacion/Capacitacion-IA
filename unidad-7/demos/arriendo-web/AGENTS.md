# AGENTS.md · ArriendoWeb

Migración a C# del sistema de arriendo de autos `legado/` (Delphi 7 + DBISAM 4). Proyecto de las demos de las Unidades 5, 6 y 7.

## Estructura

- `legado/`: código Delphi original. **Solo lectura**: se lee, no se compila ni se modifica.
- `migracion/tarifas-golden.csv`: golden master de `TarifaBase` (`legado/src/UTarifas.pas`). Es el contrato: **no se edita** para que un test pase.
- `src/ArriendoWeb.Core/Tarifas/Tarifas.cs`: `TarifaBase` migrada (namespace `ArriendoWeb.Core`).
- `tests/ArriendoWeb.Tests/`: tests xUnit (namespace `ArriendoWeb.Tests`). El `.csproj` ya copia `tarifas-golden.csv` a la carpeta de salida (léelo desde `AppContext.BaseDirectory`).

## Reglas

- Montos de dinero en `decimal`, nunca `double` ni `float`.
- Lee los números del CSV con `CultureInfo.InvariantCulture` (usa punto decimal).
- El C# debe calcular igual que el legado, defectos incluidos, hasta que se decida corregirlos en un commit aparte.
- Para entender el Delphi, usa el subagente `analista-legado` (solo lectura).
- Ejecuta `dotnet test` desde la raíz después de cada cambio.
- Nunca edites `.github/workflows/` ni secretos sin que te lo pidan.
