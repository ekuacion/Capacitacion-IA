# Plan: Carrito de compras con descuentos

> Spec de origen: `2-spec.md` v1.0 · Autor: agente (plan mode) · Revisado por: Diego Fuentes (dev)
> Estado: revisado. Se aprueba con las 2 correcciones marcadas como **[Revisión del dev]**.

<!-- Así lo propuso el agente. Las revisiones del dev quedan visibles para aprender de ellas. -->

## Enfoque

Voy a separar la lógica de negocio de la capa HTTP:

1. **Dominio (`Carrito`)**: guarda las líneas y aplica el límite de 10 unidades (RN-08).
2. **`CalculadoraTotales`**: función pura que recibe el carrito y devuelve subtotal, descuento, despacho, total y beneficio aplicado (RN-02, RN-05, RN-06, RN-07). Sin base de datos ni HTTP: fácil de testear.
3. **`ServicioCupones`**: valida el código (existe, vigente, primera compra) y devuelve un resultado de éxito o error con código (RN-03, RN-04, RN-09, RN-11).
4. **Endpoints** (Minimal API): traducen resultados a 200, 400, 404 o 422 con el cuerpo `{ codigo, mensaje }`.

Para calcular el 10% usaré `decimal` y `Math.Round(subtotal * 0.10m)`, y luego convertiré a `int`.

> **[Revisión del dev]** Esto rompe RN-05 y CA-10. `Math.Round` en .NET usa redondeo "al par" por defecto:
> no trunca. Para $12.345 da $1.234 por casualidad, pero para $12.355 daría $1.236 y la spec pide $1.235.
> Además, no hace falta `decimal`: todo es `int` (RN-01).
> **Corrección:** calcular `descuento = subtotal * 10 / 100` con aritmética entera (trunca hacia abajo en positivos).
> Agregar un test extra con $12.355 → $1.235 para dejar el borde cubierto.

## Decisiones técnicas

- Montos como `int`. Descartado `decimal`: la spec dice enteros en CLP. (Ajustado tras la revisión.)
- Fecha actual desde `TimeProvider` inyectado, con zona `America/Santiago`. Descartado `DateTime.Now`: CA-14 necesita simular el 2027-01-01.
- Errores como `Resultado<T>` (éxito o error con código). Descartadas las excepciones para reglas de negocio: son flujo normal, no fallas.
- Carrito en memoria por `clienteId` (`ConcurrentDictionary`). La persistencia real queda para otra spec.

## Archivos afectados

| Archivo | Nuevo / Modificado | Para qué |
|---|---|---|
| `src/TiendaMini.Api/Carrito/Carrito.cs` | Nuevo | Líneas del carrito y límite de 10 unidades |
| `src/TiendaMini.Api/Carrito/CalculadoraTotales.cs` | Nuevo | Subtotal, regla del mayor beneficio, despacho y total |
| `src/TiendaMini.Api/Carrito/Cupones/ServicioCupones.cs` | Nuevo | Validación de BIENVENIDA10 |
| `src/TiendaMini.Api/Carrito/Cupones/CuponesOptions.cs` | Nuevo | Vigencia del cupón desde `appsettings.json` |
| `src/TiendaMini.Api/Carrito/CarritoEndpoints.cs` | Nuevo | GET carrito, POST items, POST cupón |
| `src/TiendaMini.Api/Carrito/Dtos.cs` | Nuevo | `AgregarItemRequest`, `AplicarCuponRequest`, `CarritoResponse`, `ErrorResponse` |
| `src/TiendaMini.Api/Program.cs` | Modificado | Registrar servicios, `TimeProvider` y endpoints |
| `tests/TiendaMini.Tests/Carrito/*.cs` | Nuevo | Tests unitarios y de integración |

`AplicarCuponRequest` tendrá `codigo` y `esPrimeraCompra`. El frontend ya sabe si el cliente compró antes,
así evitamos consultar el historial de pedidos y el servicio queda más simple.

> **[Revisión del dev]** No. RN-03 dice que ese dato **nunca** viene en el request. Si el cliente lo manda,
> cualquiera puede enviar `esPrimeraCompra: true` y usar el cupón infinitas veces. Es una falla de seguridad.
> **Corrección:** `AplicarCuponRequest` solo tiene `codigo`. `ServicioCupones` recibe un `IHistorialPedidos`
> (ya existe en el proyecto) y cuenta los pedidos pagados del `clienteId` que viene en el token.
> En los tests se usa un `IHistorialPedidos` falso. Agregar `Pedidos/IHistorialPedidos.cs` a la tabla como "Usado, no modificado".

## Riesgos

| Riesgo | Mitigación |
|---|---|
| Diferencias de cálculo entre front y back. | El back es la única fuente del total. El front solo muestra `CarritoResponse`. |
| Tests de vigencia que fallan según la fecha real. | `FakeTimeProvider` en todos los tests que usan fecha. |
| Desfase de zona horaria en el borde del 31-12. | Convertir a `America/Santiago` antes de comparar con la vigencia. |
| Clientes molestos porque con cupón se cobra despacho sobre $50.000. | Comportamiento confirmado por la PO (RN-07). Se informa en el texto de la UI (fuera de este plan). |

## Pasos

1. Modelo `Carrito` con límite de unidades y sus tests.
2. `CalculadoraTotales` con despacho y sus tests.
3. `ServicioCupones` con existencia, normalización y vigencia.
4. Regla de primera compra con `IHistorialPedidos`.
5. Regla del mayor beneficio y redondeo en `CalculadoraTotales`.
6. Endpoints, formato de errores y tests de integración con los 3 ejemplos de la spec.

## Fuera del plan

Pago, quitar productos, stock, cupones administrables y carritos de invitados (sección 7 de la spec).
