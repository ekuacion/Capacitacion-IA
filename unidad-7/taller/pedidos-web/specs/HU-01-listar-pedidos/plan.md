# Plan: HU-01 · Listar pedidos con filtros

> Spec de origen: `specs/HU-01-listar-pedidos/spec.md` v1.1 · Autor: Claude Code (plan mode) · Revisado por: Claudio Ortiz Castillo
> Estado: aprobado (Claudio Ortiz Castillo, 2026-11-10)

> Formato: `specs/plantillas/plan-plantilla.md`. Este plan responde **cómo** se cumple la spec; no la repite.
> Las correcciones del dev se dejan visibles con el formato `> **[Revisión del dev]** qué está mal, por qué y qué hacer en su lugar.`
> No se implementa hasta que la spec esté en estado «aprobada» (v1.0) y este plan en «aprobado» (regla 1 de `AGENTS.md`).

## Enfoque

La lógica vive en `PedidosWeb.Core` y el endpoint solo traduce a HTTP. En Core, una clase `BuscadorPedidos` recibe el
`IPedidoRepositorio` y expone `Buscar(ConsultaPedidos)`. Ese método trata los filtros vacíos como no enviados (RN-02),
valida en el orden fijo de RN-11, filtra con Y sin distinguir mayúsculas (RN-03, RN-04, RN-07, RN-08), ordena por
`Fecha` y luego `Id` (RN-05) y pagina (RN-09). Devuelve un `ResultadoBusqueda`, que contiene o una `PaginaPedidos` o un
`ErrorBusqueda` con `{codigo, mensaje}` (RN-10). En `PedidosWeb.Api`, un `MapGet("/api/pedidos/buscar")` recibe los
7 parámetros como texto, llama al buscador y responde `200` con la página o `400` con el error (RN-01). Los criterios
de lógica se prueban contra Core; los dos criterios HTTP (CA-21, CA-22) se prueban con `WebApplicationFactory<Program>`
en un proyecto de tests aparte.

## Decisiones técnicas

- **Resultado en vez de excepciones.** Un error de validación es un caso esperado, no excepcional, y el test queda
  `Assert.Equal("FECHAS_INVERTIDAS", resultado.Error!.Codigo)`. Descartado: lanzar excepciones y traducirlas con un middleware.
- **Los 7 parámetros llegan a Core como `string?`.** Así Core decide `FECHA_INVALIDA` y `PAGINACION_INVALIDA` con nuestro
  formato (RN-06, RN-09, CA-24) y esas reglas se prueban en Core. Descartado: enlazarlos como `DateOnly?`/`int?`, porque
  ASP.NET respondería su 400 genérico sin `{codigo, mensaje}` y rompería RN-10.
- **Fechas con `DateOnly.TryParseExact(valor, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out …)`.**
  Rechaza `31-10-2026` y fechas que no existen, como `2026-02-30`. Descartado: `DateOnly.TryParse`, que acepta otros formatos según la cultura.
- **Comparación con `StringComparison.OrdinalIgnoreCase`** para cliente, estado y sucursal. Los estados válidos (RN-07) y
  las sucursales válidas (RN-08) quedan en listas fijas de Core. Descartado: `ToUpper()`/`ToLower()`, que depende de la cultura.
- **Códigos y mensajes en constantes** (`CodigosErrorBusqueda`), copiados tal cual de §6 de la spec. Descartado: escribir
  los textos sueltos en cada `return`, porque es fácil que se desalineen con la spec.
- **`BuscadorPedidos` recibe `IPedidoRepositorio` por constructor.** Permite el repositorio de test de CA-23 (dos pedidos
  con la misma fecha). Descartado: un método estático sobre la lista fija del repositorio en memoria.
- **La respuesta usa el record `Pedido` completo.** El Ejemplo 1 muestra todos sus campos. Descartado: un DTO aparte, que hoy no aporta nada.
- **JSON con la configuración por defecto de System.Text.Json.** En Minimal API ya entrega camelCase y `DateOnly` como
  `"2026-09-09"`, que es lo que piden el Ejemplo 1 y CA-21. Descartado: configurar opciones propias de serialización.
- **Proyecto nuevo `tests/PedidosWeb.Api.Tests`** con xUnit (mismas versiones que `PedidosWeb.Core.Tests`) y
  `Microsoft.AspNetCore.Mvc.Testing` 10.0.14 (coincide con el runtime ASP.NET 10.0.14 instalado; paquete aprobado en la
  regla 8 de `AGENTS.md`). Descartado: agregar el paquete y la referencia a Api en `PedidosWeb.Core.Tests`, porque los tests
  de Core no deben depender de ASP.NET.
- **`Program.cs` ya expone `public partial class Program { }`**, que es lo que necesita `WebApplicationFactory<Program>`; no hay que cambiarlo para eso.

## Archivos afectados

| Archivo | Nuevo / Modificado | Para qué |
|---|---|---|
| `src/PedidosWeb.Core/Pedidos/ConsultaPedidos.cs` | Nuevo | Record con los 7 parámetros tal como llegan (`string?`): `ClienteId`, `Estado`, `Sucursal`, `Desde`, `Hasta`, `Pagina`, `TamanoPagina` |
| `src/PedidosWeb.Core/Pedidos/ResultadoBusqueda.cs` | Nuevo | `ResultadoBusqueda` (éxito o error), `PaginaPedidos(Pedidos, Total, Pagina, TamanoPagina)` y `ErrorBusqueda(Codigo, Mensaje)` |
| `src/PedidosWeb.Core/Pedidos/CodigosErrorBusqueda.cs` | Nuevo | Los 6 códigos de error y sus mensajes (§6 de la spec) |
| `src/PedidosWeb.Core/Pedidos/BuscadorPedidos.cs` | Nuevo | Normalizar, validar, filtrar, ordenar y paginar (RN-02 a RN-11) |
| `src/PedidosWeb.Api/Program.cs` | Modificado | Registrar `BuscadorPedidos` como singleton y agregar `MapGet("/api/pedidos/buscar")` |
| `tests/PedidosWeb.Core.Tests/Pedidos/BuscadorPedidosTests.cs` | Nuevo | Tests de CA-01 a CA-20 y CA-23 a CA-26 |
| `tests/PedidosWeb.Api.Tests/PedidosWeb.Api.Tests.csproj` | Nuevo | Proyecto de tests HTTP |
| `tests/PedidosWeb.Api.Tests/Pedidos/BuscarPedidosEndpointTests.cs` | Nuevo | Tests de CA-21 y CA-22 |
| `PedidosWeb.sln` | Modificado | Agregar `PedidosWeb.Api.Tests` dentro de la carpeta de solución `tests` |
| `AGENTS.md` | Modificado (lo hace o aprueba Claudio, regla 9) | Sección Tests: los tests HTTP viven en `tests/PedidosWeb.Api.Tests/<Área>/` |

## Riesgos

| Riesgo | Mitigación |
|---|---|
| `/api/pedidos/buscar` podría chocar con la ruta existente `/api/pedidos/{id}` | No choca: esa ruta exige `{id:int}`. CA-21 lo comprueba por HTTP. |
| El restore del paquete NuGet falla sin red | El trabajo de Core (T1 a T4) no depende del paquete; el proyecto HTTP queda al final (T5). |
| El JSON real no calza con el Ejemplo 1 (nombres, fecha o montos) | CA-21 compara campos y valores exactos de la respuesta. |
| Tests débiles que pasan con código roto | Cada test compara ids, `total` y `codigo` exactos; en la revisión (T6) se rompe el código a propósito para ver que fallen. |
| `AGENTS.md` dice que los tests viven solo en `PedidosWeb.Core.Tests` | Se actualiza en T5, con la aprobación de Claudio (regla 9). |
| Negocio: los mensajes de error de §6 los redactó Claude | El PO los revisa y aprueba junto con la spec v1.0. |
| Implementar con la spec sin aprobar | T0 exige la spec v1.0 «aprobada» y este plan «aprobado» antes de T1. |

## Pasos

1. Aprobar la spec (v1.0) y este plan. → Estado «aprobada» y «aprobado» en los dos documentos.
2. Crear los tipos de Core. → `dotnet build` sin errores ni advertencias nuevas.
3. Implementar el camino feliz (filtrar, ordenar y paginar). → Tests de T2 en verde.
4. Implementar las validaciones en el orden de RN-11. → Tests de T3 en verde.
5. Agregar el endpoint en Api. → `dotnet run --project src/PedidosWeb.Api` y los Ejemplos 1 y 3 en `http://localhost:5080`.
6. Crear el proyecto de tests HTTP. → `dotnet test` con todo en verde (21 tests previos + los nuevos).
7. Revisar el diff con las 5 preguntas del paso 5 de la guía y hacer el commit (persona).

El detalle de cada paso está en `tasks.md`.

## Fuera del plan

- Interfaz web, autenticación y base de datos (spec §7).
- Búsqueda por nombre de cliente, otros filtros (folio, monto) y otros órdenes (spec §7).
- Cambios en `FolioFormatter`, en `GET /salud` o en `GET /api/pedidos/{id}`.
- Cambios en `IPedidoRepositorio` o en `PedidoRepositorioEnMemoria`: se usan tal como están.
