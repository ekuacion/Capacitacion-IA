# Trazabilidad: criterio → tarea → test

> Spec: `2-spec.md` v1.0 · Tasks: `4-tasks.md`

Cada criterio de aceptación tiene al menos un test. Cada test lleva el ID del criterio en un comentario
(por ejemplo `// CA-09`). Así, si un test falla, sabes qué regla de negocio se rompió.

Convención de nombres: `Metodo_Escenario_ResultadoEsperado` (ver `AGENTS.md`).

## Criterios de aceptación

| CA | Qué verifica | Tarea | Test xUnit (`Clase.Método`) |
|---|---|---|---|
| CA-01 | Agregar 2 Poleras → subtotal 25980 | T1 | `CarritoTests.Agregar_DosPolerasEnCarritoVacio_SubtotalEs25980` |
| CA-02 | 10 unidades se aceptan | T1 | `CarritoTests.Agregar_DiezUnidades_LineaQuedaConDiez` |
| CA-03 | 11 unidades se rechazan | T1 | `CarritoTests.Agregar_OnceUnidades_DevuelveCantidadMaximaExcedida` |
| CA-04 | 8 + 3 unidades se rechaza y queda en 8 | T1 | `CarritoTests.Agregar_TresSobreOchoExistentes_RechazaYMantieneOcho` |
| CA-05 | Subtotal 49999 → despacho 3990 | T2 | `CalculadoraTotalesTests.Despacho_Subtotal49999_Cobra3990` |
| CA-06 | Subtotal 50000 → despacho gratis | T2 | `CalculadoraTotalesTests.Despacho_Subtotal50000_EsGratis` |
| CA-07 | Carrito vacío → todo en 0 | T2 | `CalculadoraTotalesTests.Despacho_CarritoVacio_TotalEsCero` |
| CA-08 | Cupón con subtotal 33480 → total 34122 | T5 | `CalculadoraTotalesTests.Cupon_PrimeraCompraBajoUmbral_Total34122` |
| CA-09 | Cupón gana al despacho gratis → total 57990 | T5 | `CalculadoraTotalesTests.Cupon_Subtotal60000_GanaCuponYCobraDespacho` |
| CA-10 | 10% de 12345 → 1234 (trunca) | T5 | `CalculadoraTotalesTests.Cupon_Subtotal12345_DescuentoSeTrunca` |
| CA-11 | `"  bienvenida10 "` se acepta | T3 | `ServicioCuponesTests.Aplicar_CodigoEnMinusculasConEspacios_SeNormaliza` |
| CA-12 | Cliente con pedido pagado → `CUPON_NO_APLICABLE` | T4 | `ServicioCuponesTests.Aplicar_ClienteConPedidoPagado_DevuelveCuponNoAplicable` |
| CA-13 | `DESCUENTO50` → `CUPON_INVALIDO` | T3 | `ServicioCuponesTests.Aplicar_CodigoInexistente_DevuelveCuponInvalido` |
| CA-14 | Fecha 2027-01-01 → `CUPON_VENCIDO` | T3 | `ServicioCuponesTests.Aplicar_FechaPosteriorAVigencia_DevuelveCuponVencido` |

## Tests adicionales

| Origen | Qué verifica | Tarea | Test xUnit (`Clase.Método`) |
|---|---|---|---|
| Revisión del plan (RN-05) | 10% de 12355 → 1235, no 1236 | T5 | `CalculadoraTotalesTests.Cupon_Subtotal12355_NoUsaRedondeoAlPar` |
| Ejemplo 1 | `GET /api/carrito` sin cupón → JSON exacto | T6 | `CarritoEndpointsTests.GetCarrito_SinCupon_DevuelveTotal37470` |
| Ejemplo 2 | `POST /api/carrito/cupon` → JSON exacto | T6 | `CarritoEndpointsTests.PostCupon_PrimeraCompraSobreUmbral_DevuelveTotal57990` |
| Ejemplo 3 | `POST /api/carrito/cupon` inválido → 422 | T6 | `CarritoEndpointsTests.PostCupon_CodigoInexistente_Devuelve422ConCodigo` |

## Chequeo de cobertura

- [x] Los 14 criterios tienen un test.
- [x] Los 3 ejemplos de la spec tienen un test de integración.
- [x] Cada tarea tiene al menos un test asociado.
- [x] Las 2 correcciones del plan tienen un test que las protege (redondeo: `Cupon_Subtotal12355_...`; primera compra: `Aplicar_ClienteConPedidoPagado_...`).

**Para clase:** elige una fila y recórrela hacia atrás: test → tarea → criterio → regla de negocio → intent.
Si en algún paso no puedes explicar "por qué existe", hay un hueco en la spec.
