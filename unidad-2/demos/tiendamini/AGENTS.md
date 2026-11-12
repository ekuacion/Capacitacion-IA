# AGENTS.md — TiendaMini

## Proyecto
API REST de TiendaMini, tienda online pequeña. ASP.NET Core (.NET 10), tests con xUnit.
Código en `src/TiendaMini.Api`. Tests en `tests/TiendaMini.Tests`.

## Comandos
- Compilar: `dotnet build`
- Tests: `dotnet test`
- Un grupo de tests: `dotnet test --filter "FullyQualifiedName~Carrito"`
- Ejecutar la API: `dotnet run --project src/TiendaMini.Api`

## Convenciones
- Código, rutas y mensajes en español. Montos en CLP como `int`. Nunca `decimal` ni `double` para dinero.
- Nombres de test: `Metodo_Escenario_ResultadoEsperado`.
- Errores de negocio: HTTP 422 con cuerpo `{ "codigo": "...", "mensaje": "..." }`.
- Fecha y hora siempre desde `TimeProvider` inyectado. Nunca `DateTime.Now`.

## Reglas SDD
1. No escribas código sin una spec aprobada en `specs/<funcionalidad>/spec.md`.
2. Empieza en plan mode. Propón `plan.md` y espera la revisión del dev.
3. Si la spec es ambigua, detente y pregunta. No inventes reglas de negocio.
4. Trabaja una tarea de `tasks.md` a la vez. Cierra cada una con su verificación.
5. Cada criterio de aceptación (CA-xx) debe tener al menos un test con su ID en un comentario.
6. Si cambias el comportamiento, actualiza primero la spec y luego el código.
7. Antes de terminar: `dotnet build` y `dotnet test` sin errores ni warnings nuevos.
