# Tasks: HU-02 · Detalle de un pedido

> Plan de origen: `specs/HU-02-detalle-pedido/plan.md` (aprobado) · Spec: `specs/HU-02-detalle-pedido/spec.md` v1.1 (aprobada)

> Formato: `specs/plantillas/tasks-plantilla.md`. Una tarea = un cambio pequeño con su verificación y su commit.
> Las tareas de abajo pueden depender de las de arriba, nunca al revés.

## Resumen

| ID | Tarea | CA cubiertos | Estado |
|---|---|---|---|
| T0 | Aprobar la spec v1.0 y el plan | — | hecha |
| T1 | Incremento 1: calcular el total desde las líneas | CA-01 | hecha |
| T2 | Incremento 2: obtener el detalle | CA-01, CA-02, CA-03 | hecha |
| T3 | Incremento 3: publicar `GET /api/pedidos/{id}/detalle` | CA-03 | hecha |
| T4 | Incremento 4: tests HTTP de la ruta | CA-02, CA-03, CA-04 | hecha |

---

## T0 — Aprobar la spec v1.0 y el plan

- **CA cubiertos:** ninguno.
- **Depende de:** ninguna.
- **Archivos:** `specs/HU-02-detalle-pedido/spec.md`, `specs/HU-02-detalle-pedido/plan.md`.
- **Qué hacer:**
  - Claudio deja la spec en «aprobada», v1.0 (las decisiones **[Decidir]**, RN-04 y el texto del error, ya están resueltas en la v0.3).
  - Claudio revisa el plan, deja sus correcciones como `> **[Revisión del dev]**` y lo deja en «aprobado».
- **Verificación:** `grep -n "Estado" specs/HU-02-detalle-pedido/spec.md specs/HU-02-detalle-pedido/plan.md` → «aprobada» y «aprobado».
- **Hecho cuando:** los dos documentos están aprobados por una persona.

## T1 — Incremento 1: calcular el total desde las líneas

- **CA cubiertos:** CA-01 (la parte del total).
- **Depende de:** T0.
- **Archivos:** `src/PedidosWeb.Core/Pedidos/CalculadoraTotalPedido.cs`, `tests/PedidosWeb.Core.Tests/Pedidos/CalculadoraTotalPedidoTests.cs`.
- **No tocar:** `Program.cs`, `Clientes/`, `Lineas/` y `Pedido.cs`.
- **Qué hacer:**
  - Función pura que recibe las líneas de un pedido y devuelve la suma de `Cantidad × PrecioUnitario` (RN-01, RN-02).
  - Lista o línea `null` lanzan `ArgumentNullException`; los negativos no se validan (ver `plan.md`).
  - Tests: pedido 8 = 120100, lista vacía = 0, una sola línea, cantidad 0, lista `null`, línea `null` y los 45 pedidos coinciden con `Pedido.Total`.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~CalculadoraTotalPedidoTests"` → tests nuevos en verde; `dotnet test` → 56 previos + nuevos, todos en verde.
- **Hecho cuando:** el diff se lee en 5 minutos y solo aparecen los 2 archivos de arriba.
- **Verificación extra:** romper el cálculo a propósito y comprobar que los tests fallan (hecho en el T1).
- **Commit:** `feat(u3): total del pedido desde sus líneas`

## T2 — Incremento 2: obtener el detalle

- **CA cubiertos:** CA-01, CA-02.
- **Depende de:** T1.
- **Archivos:** `src/PedidosWeb.Core/Pedidos/DetallePedido.cs`, `ResultadoDetalle.cs`, `CodigosErrorDetalle.cs`, `ServicioDetallePedido.cs`, y `tests/PedidosWeb.Core.Tests/Pedidos/ServicioDetallePedidoTests.cs`.
- **No tocar:** `Program.cs`, `Clientes/` y los repositorios existentes.
- **Qué hacer:**
  - `DetallePedido` y `LineaDetalle` como records, con `Subtotal` calculado.
  - `ResultadoDetalle` (el detalle o un `ErrorDetalle {Codigo, Mensaje}`) y `CodigosErrorDetalle` con `PEDIDO_NO_ENCONTRADO` y su mensaje (RN-05).
  - `ServicioDetallePedido(IPedidoRepositorio, ILineaPedidoRepositorio).Obtener(id)`: arma el detalle con las líneas ordenadas por número (el orden lo aplica el servicio) y el total de T1; si el pedido no existe, devuelve el error.
  - Tests (7): pedido 8 completo (folio, estado, cliente, 3 líneas ordenadas, total 120100), subtotal = cantidad × precio, total igual a `Pedido.Total` en los pedidos de ejemplo, líneas desordenadas, pedido sin líneas (repositorio falso) e id 999 = error con código y mensaje exactos.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~ServicioDetallePedidoTests"` → en verde; `dotnet test` → todo en verde.
- **Hecho cuando:** el diff se lee en 5 minutos y no hay cambios fuera de los 5 archivos.
- **Verificación extra:** romper el código de varias formas (quitar el orden, tomar el total de `Pedido.Total`, quitar el id del mensaje, vaciar el cliente) y comprobar que los tests fallan (hecho en el T2).
- **Commit:** `feat(u3): detalle del pedido en Core`

## T3 — Incremento 3: publicar `GET /api/pedidos/{id}/detalle`

- **CA cubiertos:** CA-03 (y CA-01 y CA-02 vistos por HTTP).
- **Depende de:** T2.
- **Archivos:** `src/PedidosWeb.Api/Program.cs`.
- **No tocar:** `Core`, `GET /api/pedidos/{id}`, `GET /api/pedidos/buscar` y `GET /salud`.
- **Qué hacer:**
  - Registrar `ILineaPedidoRepositorio` y `ServicioDetallePedido` como singletons.
  - `MapGet("/api/pedidos/{id:int}/detalle")`: `200` con el detalle o `404` con el cuerpo `{codigo, mensaje}` del error.
- **Verificación:**
  - `dotnet test` → todo en verde.
  - `dotnet run --project src/PedidosWeb.Api`, abrir `http://localhost:5080/api/pedidos/8/detalle` (total 120100) y `/api/pedidos/999/detalle` (404 con `PEDIDO_NO_ENCONTRADO`). Comprobar que `/api/pedidos/8` y `/api/pedidos/buscar` siguen igual.
- **Hecho cuando:** el navegador muestra el ejemplo de la spec y `git diff --stat` solo muestra `Program.cs`.
- **Commit:** `feat(u3): ruta GET /api/pedidos/{id}/detalle`

## T4 — Incremento 4: tests HTTP de la ruta

- **CA cubiertos:** CA-02, CA-03, CA-04.
- **Depende de:** T3.
- **Archivos:** `tests/PedidosWeb.Api.Tests/Pedidos/DetallePedidoEndpointTests.cs`.
- **No tocar:** `Program.cs`, `Core` y los tests existentes.
- **Qué hacer:**
  - Tests con `WebApplicationFactory<Program>`, en el mismo estilo de `BuscarPedidosEndpointTests`.
  - `GET /api/pedidos/8/detalle`: 200 y el JSON exacto del ejemplo de la spec (CA-02).
  - `GET /api/pedidos/999/detalle`: 404 y el cuerpo `{codigo, mensaje}` exacto (CA-03).
  - `GET /api/pedidos/8`: 200 y el pedido sin `lineas`, igual que antes (CA-04).
- **Verificación:** `dotnet test --filter "FullyQualifiedName~DetallePedidoEndpointTests"` → 3 tests en verde; `dotnet test` → todo en verde.
- **Hecho cuando:** el diff se lee en 5 minutos y solo aparece el archivo de tests.
- **Verificación extra:** romper la ruta a propósito (por ejemplo, devolver 200 en vez de 404) y comprobar que los tests fallan.
- **Commit:** `test(u3): tests HTTP de la ruta /detalle`
