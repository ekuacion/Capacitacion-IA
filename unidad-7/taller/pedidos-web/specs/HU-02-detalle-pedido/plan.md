# Plan: HU-02 · Detalle de un pedido

> Spec de origen: `specs/HU-02-detalle-pedido/spec.md` v1.1 · Autor: Claude Code · Revisado por: Claudio Ortiz Castillo
> Estado: aprobado (Claudio Ortiz Castillo, 2026-11-10) · Ajustado durante el T1 (`null` y negativos en la calculadora, opción A) y el T2 (orden en el servicio y RN-04 con repositorio falso), y ampliado con el T4 (tests HTTP, spec v1.1), aprobados por Claudio

> Formato: `specs/plantillas/plan-plantilla.md`. Este plan responde **cómo** se cumple la spec; no la repite.
> Las correcciones del dev se dejan visibles con el formato `> **[Revisión del dev]** qué está mal, por qué y qué hacer en su lugar.`
> No se implementa hasta que la spec esté «aprobada» (v1.0) y este plan «aprobado» (regla 1 de `AGENTS.md`).

## Enfoque

La lógica vive en `PedidosWeb.Core` y el endpoint solo traduce a HTTP. Se construye en **3 incrementos**, cada uno con
sus tests y su commit. Primero una función pura que suma las líneas (RN-01, RN-02). Luego un
`ServicioDetallePedido`, que recibe `IPedidoRepositorio` e `ILineaPedidoRepositorio`, arma el `DetallePedido` con las
líneas ordenadas (RN-03) y devuelve un `ResultadoDetalle` con el detalle o, si el pedido no existe, un error `{codigo, mensaje}` (RN-05). Al
final, `Program.cs` registra el servicio y expone `GET /api/pedidos/{id:int}/detalle`, respondiendo `200` con el
detalle o `404` con el error.

## Decisiones técnicas

- **El nombre del cliente sale de `Pedido.ClienteNombre`.** El pedido ya lo trae. Descartado: usar `ClientesService`,
  porque está heredado y se refactoriza en la etapa 2: acoplarlos haría que el refactor pudiera romper el detalle.
- **El total se calcula sumando las líneas** (`Cantidad × PrecioUnitario`), no desde `Pedido.Total`. Descartado: copiar
  `Pedido.Total`, porque la spec pide que el total sea la suma de las líneas (RN-01) y así el test lo comprueba.
- **`ResultadoDetalle`: contiene o el detalle o un `ErrorDetalle {Codigo, Mensaje}`**, el mismo patrón de la HU-01
  (RN-05). Core decide qué pasó y la API solo traduce a HTTP; un frontend futuro puede mostrar el mensaje. Descartado:
  `null` con el error armado en `Program.cs`, porque deja una regla de negocio (el mensaje) en la capa equivocada; y
  descartado lanzar excepciones, porque un pedido inexistente es un caso esperado.
- **Código y mensaje en constantes** (`CodigosErrorDetalle`), copiados de la spec. Descartado: escribir los textos
  sueltos en cada `return`, porque se desalinean con la spec.
- **Records nuevos `DetallePedido` y `LineaDetalle`**, con `Subtotal` ya calculado. Descartado: agregar `Subtotal` a
  `LineaPedido`, porque modificaría un módulo con tests que no forma parte de esta historia.
- **El servicio ordena las líneas por `Numero` (RN-03)**, aunque el repositorio en memoria ya las entrega ordenadas. Así
  la regla no depende de cómo se implemente el repositorio cuando llegue PostgreSQL (Unidad 5). Descartado: confiar en
  el orden del repositorio.
- **El caso «pedido sin líneas» (RN-04) se prueba con un repositorio de líneas falso dentro del test**, porque los 45
  pedidos de ejemplo tienen líneas. Descartado: agregar un pedido sin líneas a los datos de ejemplo, que alteraría datos
  compartidos con otras pruebas.
- **La calculadora lanza `ArgumentNullException` si la lista o una línea es `null`**, y no valida negativos. Un `null` es
  un error de programación, no un pedido sin líneas: devolver 0 disfrazaría un fallo de total válido. Los negativos se
  garantizan en la fuente de datos; validarlos aquí sería una regla de negocio nueva (spec v1.1). Descartado: devolver 0
  ante `null` y rechazar negativos con excepción.
- **JSON con la configuración por defecto de System.Text.Json** (camelCase, `DateOnly` como `"2026-10-20"`). Descartado:
  opciones propias de serialización.
- **Sin paquetes nuevos.** Los tests del servicio usan los repositorios en memoria existentes.

## Archivos afectados

| Archivo | Nuevo / Modificado | Incremento | Para qué |
|---|---|---|---|
| `src/PedidosWeb.Core/Pedidos/CalculadoraTotalPedido.cs` | Nuevo | 1 | Suma `Cantidad × PrecioUnitario` de las líneas (RN-01, RN-02); lanza `ArgumentNullException` ante `null` |
| `tests/PedidosWeb.Core.Tests/Pedidos/CalculadoraTotalPedidoTests.cs` | Nuevo | 1 | Pedido 8 = 120100, lista vacía = 0, una línea, cantidad 0, lista `null`, línea `null` y los 45 pedidos coinciden con `Pedido.Total` |
| `src/PedidosWeb.Core/Pedidos/DetallePedido.cs` | Nuevo | 2 | Records `DetallePedido` y `LineaDetalle` |
| `src/PedidosWeb.Core/Pedidos/ResultadoDetalle.cs` | Nuevo | 2 | `ResultadoDetalle` (éxito o error) y `ErrorDetalle(Codigo, Mensaje)` |
| `src/PedidosWeb.Core/Pedidos/CodigosErrorDetalle.cs` | Nuevo | 2 | Código `PEDIDO_NO_ENCONTRADO` y su mensaje (RN-05) |
| `src/PedidosWeb.Core/Pedidos/ServicioDetallePedido.cs` | Nuevo | 2 | Arma el detalle o devuelve el error si no existe (RN-03, RN-05) |
| `tests/PedidosWeb.Core.Tests/Pedidos/ServicioDetallePedidoTests.cs` | Nuevo | 2 | CA-01, CA-02 y CA-03; subtotal, orden de líneas desordenadas, pedido sin líneas (RN-04), total igual a `Pedido.Total` en los pedidos de ejemplo y error con código y mensaje exactos (7 tests) |
| `tests/PedidosWeb.Api.Tests/Pedidos/DetallePedidoEndpointTests.cs` | Nuevo | 4 | Tests HTTP con `WebApplicationFactory<Program>`: CA-02 (JSON exacto del ejemplo), CA-03 (404 con cuerpo) y CA-04 (`GET /api/pedidos/{id}` sin cambios) |
| `src/PedidosWeb.Api/Program.cs` | Modificado | 3 | Registrar `ILineaPedidoRepositorio` y el servicio; `MapGet("/api/pedidos/{id:int}/detalle")`: `200` con el detalle o `404` con `{codigo, mensaje}` |

## Riesgos

| Riesgo | Mitigación |
|---|---|
| La ruta `/{id}/detalle` choque con `/api/pedidos/{id}` o con `/api/pedidos/buscar` | No chocan: `{id:int}` exige un entero y `buscar` no lo es. Se comprueba en el navegador. |
| El incremento sale gigante y se mezcla con otras partes | Cada incremento declara qué toca y qué no (ver `tasks.md`); el diff debe leerse en 5 minutos. |
| El total del detalle difiere de `Pedido.Total` | Los datos de ejemplo cumplen que coinciden; un test compara ambos para el pedido 8. |
| Tocar sin querer `ClientesService` o `GET /api/pedidos/{id}` | Están declarados como «no tocar» en cada tarea y se verifica con `git diff --stat`. |
| Los datos de `Lineas` no calzan con `Pedidos` tras la normalización del repo | Ya verificado: 56 tests en verde tras el paso 0, y un test del T1 compara los 45 totales. |
| Una cantidad o precio negativo en los datos produce un total incorrecto sin avisar | Fuera de alcance de esta historia: se garantiza en la fuente de datos. Si se quiere validar aquí, es una regla nueva y requiere spec v1.1. |

## Pasos

1. Aprobar la spec (v1.0) y este plan. → «aprobada» y «aprobado» en los dos documentos.
2. Incremento 1: calcular el total. → `dotnet test` con los tests nuevos en verde.
3. Incremento 2: obtener el detalle. → `dotnet test` en verde.
4. Incremento 3: publicar la ruta. → `dotnet run --project src/PedidosWeb.Api` y `/api/pedidos/8/detalle` en el navegador.
5. Incremento 4: tests HTTP de la ruta. → `dotnet test` en verde, con los tests de `PedidosWeb.Api.Tests` incluidos.

El detalle de cada paso está en `tasks.md`.

## Fuera del plan

- IVA, descuentos y formato de moneda (spec, fuera de alcance).
- Cambios en `ClientesService`, `GET /api/pedidos/{id}`, `GET /api/pedidos/buscar`, `Pedido`, `LineaPedido` o los repositorios.
