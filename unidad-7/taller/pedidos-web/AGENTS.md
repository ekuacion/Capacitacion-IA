# AGENTS.md · Pedidos Web

## Proyecto
Portal web de pedidos de una distribuidora; moderniza Pedidos Legado (Delphi + DBISAM).
API REST en ASP.NET Core (.NET 10, Minimal API) con 45 pedidos de ejemplo en memoria; detalle en `README.md`.

## Comandos
- Compilar: `dotnet build`
- Testear: `dotnet test`
- Ejecutar la API: `dotnet run --project src/PedidosWeb.Api` → http://localhost:5080 (`/salud` responde `{"estado":"ok"}`)

## Estilo de código
- Nombres del dominio en español (`Pedido`, `ObtenerPorId`, `FormatearFolio`).
- Dinero en CLP: `decimal` sin decimales; nunca `double` ni `float`.
- Fechas sin hora: `DateOnly`.
- `src/PedidosWeb.Core/`: modelo, repositorio y lógica de dominio; sin paquetes externos.
- `src/PedidosWeb.Api/`: endpoints Minimal API en `Program.cs`; la lógica vive en Core.

## Tests
- xUnit en `tests/PedidosWeb.Core.Tests/<Área>/` (lógica de Core) y en `tests/PedidosWeb.Api.Tests/<Área>/`
  (tests HTTP de los endpoints), un archivo por clase (`<Clase>Tests.cs`).
- Nombres `Metodo_Escenario_Resultado` (ej.: `ObtenerPorId_IdInexistente_DevuelveNull`).
- Tests HTTP del endpoint con `WebApplicationFactory<Program>` (paquete `Microsoft.AspNetCore.Mvc.Testing`).
- Ejecuta `dotnet test` después de cada cambio y antes de decir que terminaste; siempre antes de un commit.

## Seguridad
- Nunca secretos, claves ni cadenas de conexión en el código ni en el repositorio.

## Reglas de trabajo
1. Toda funcionalidad parte de una spec aprobada en `specs/<historia>/spec.md`.
2. Un test xUnit por criterio de aceptación.
3. No hacer commit sin tests en verde.
4. Responde, comenta y escribe en español de Chile.
5. Puedes ejecutar `dotnet build` y `dotnet test` sin preguntar.
6. Antes de modificar un archivo, explica el cambio y espera aprobación.
7. Nunca hagas commit, push ni merge: lo hace una persona después de leer el diff.
8. No agregues paquetes NuGet sin preguntar. Aprobado: `Microsoft.AspNetCore.Mvc.Testing` (tests HTTP, HU-01).
9. Solo trabajas en `src/`, `tests/`, `specs/` y `docs/`. `.claude/` (settings y subagentes) solo cuando una tarea de la spec lo pide. `.github/` y `REVIEW.md` solo cuando una tarea de TEC-06 lo pide.
10. `legado/` (código Delphi + DBISAM) es **solo lectura**: se lee, se analiza y se cita (`archivo:línea`), pero nunca se edita, se mueve ni se borra.
11. Si una regla de negocio no está escrita en la spec, pregunta; no la inventes. En el diagnóstico del legado, lo que no puedas verificar en el código se marca «por verificar».
