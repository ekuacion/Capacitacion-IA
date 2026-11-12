# Tasks: Carrito de compras con descuentos

> Plan de origen: `3-plan.md` (revisado) · Spec: `2-spec.md` v1.0

## Resumen

| ID | Tarea | CA cubiertos | Estado |
|---|---|---|---|
| T1 | Modelar el carrito con límite de 10 unidades | CA-01, CA-02, CA-03, CA-04 | pendiente |
| T2 | Calcular subtotal y despacho | CA-05, CA-06, CA-07 | pendiente |
| T3 | Validar existencia, formato y vigencia del cupón | CA-11, CA-13, CA-14 | pendiente |
| T4 | Validar primera compra desde el historial | CA-12 | pendiente |
| T5 | Aplicar la regla del mayor beneficio y el redondeo | CA-08, CA-09, CA-10 | pendiente |
| T6 | Exponer los endpoints con el formato de errores | Ejemplos 1, 2 y 3 | pendiente |

---

## T1 — Modelar el carrito con límite de 10 unidades

- **CA cubiertos:** CA-01, CA-02, CA-03, CA-04
- **Depende de:** ninguna
- **Archivos:** `src/TiendaMini.Api/Carrito/Carrito.cs`, `tests/TiendaMini.Tests/Carrito/CarritoTests.cs`
- **Qué hacer:**
  - `Agregar(producto, cantidad)` suma a la línea existente o crea una nueva.
  - Si la línea quedaría con más de 10 unidades, devuelve error `CANTIDAD_MAXIMA_EXCEDIDA` sin modificar nada.
  - Si `cantidad < 1`, devuelve error `CANTIDAD_INVALIDA`.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~CarritoTests"` → 4 tests en verde.
- **Hecho cuando:** los 4 tests pasan y `dotnet build` no muestra warnings nuevos.

## T2 — Calcular subtotal y despacho

- **CA cubiertos:** CA-05, CA-06, CA-07
- **Depende de:** T1
- **Archivos:** `src/TiendaMini.Api/Carrito/CalculadoraTotales.cs`, `tests/TiendaMini.Tests/Carrito/CalculadoraTotalesTests.cs`
- **Qué hacer:**
  - Subtotal = suma de `precioUnitario × cantidad` (RN-02).
  - Despacho: 0 si el carrito está vacío; 0 si subtotal ≥ 50000; 3990 en otro caso (RN-06).
  - Sin cupón todavía: `descuento` siempre 0.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~CalculadoraTotalesTests.Despacho"` → 3 tests en verde.
- **Hecho cuando:** los 3 tests pasan con los montos exactos de la spec.

## T3 — Validar existencia, formato y vigencia del cupón

- **CA cubiertos:** CA-11, CA-13, CA-14
- **Depende de:** ninguna
- **Archivos:** `src/TiendaMini.Api/Carrito/Cupones/ServicioCupones.cs`, `CuponesOptions.cs`, `tests/TiendaMini.Tests/Carrito/ServicioCuponesTests.cs`
- **Qué hacer:**
  - Normalizar el código: `Trim()` + mayúsculas (RN-09).
  - Código desconocido → `CUPON_INVALIDO` con el mensaje exacto de la sección 6.
  - Fecha de `TimeProvider` en `America/Santiago` posterior a 2026-12-31 23:59:59 → `CUPON_VENCIDO`.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~ServicioCuponesTests"` → 3 tests en verde, usando `FakeTimeProvider`.
- **Hecho cuando:** ningún archivo nuevo usa `DateTime.Now` ni `DateTime.UtcNow` (revisar con búsqueda en el diff).

## T4 — Validar primera compra desde el historial

- **CA cubiertos:** CA-12
- **Depende de:** T3
- **Archivos:** `ServicioCupones.cs`, `tests/TiendaMini.Tests/Carrito/ServicioCuponesTests.cs`
- **Qué hacer:**
  - Inyectar `IHistorialPedidos` en `ServicioCupones`.
  - Si el cliente tiene 1 o más pedidos pagados → `CUPON_NO_APLICABLE`.
  - Confirmar que `AplicarCuponRequest` **no** tiene campo `esPrimeraCompra`.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~ServicioCuponesTests.Aplicar_ClienteConPedidoPagado"` → 1 test en verde.
- **Hecho cuando:** el test pasa y el DTO solo tiene la propiedad `Codigo`.

## T5 — Aplicar la regla del mayor beneficio y el redondeo

- **CA cubiertos:** CA-08, CA-09, CA-10
- **Depende de:** T2, T4
- **Archivos:** `CalculadoraTotales.cs`, `CalculadoraTotalesTests.cs`
- **Qué hacer:**
  - Descuento del cupón = `subtotal * 10 / 100` con aritmética entera (RN-05).
  - Si califica para cupón y despacho gratis, comparar cupón vs. 3990 y aplicar solo el mayor (RN-07).
  - Llenar `beneficioAplicado` con `NINGUNO`, `CUPON_BIENVENIDA10` o `DESPACHO_GRATIS`.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~CalculadoraTotalesTests.Cupon"` → 4 tests en verde (incluye el borde $12.355 → $1.235 pedido en la revisión del plan).
- **Hecho cuando:** los 4 tests pasan y no hay `decimal` ni `Math.Round` en `CalculadoraTotales.cs`.

## T6 — Exponer los endpoints con el formato de errores

- **CA cubiertos:** Ejemplos 1, 2 y 3 de la spec (pruebas de punta a punta)
- **Depende de:** T1 a T5
- **Archivos:** `CarritoEndpoints.cs`, `Dtos.cs`, `Program.cs`, `tests/TiendaMini.Tests/Carrito/CarritoEndpointsTests.cs`
- **Qué hacer:**
  - `GET /api/carrito`, `POST /api/carrito/items`, `POST /api/carrito/cupon`.
  - `clienteId` desde el token. Errores con cuerpo `{ codigo, mensaje }` y el HTTP de la sección 6.
  - Tests con `WebApplicationFactory`, autenticación falsa y el catálogo de prueba.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~CarritoEndpointsTests"` → 3 tests en verde. El JSON de cada respuesta coincide campo a campo con los ejemplos de la spec.
- **Hecho cuando:** `dotnet build` y `dotnet test` completos pasan sin errores.
