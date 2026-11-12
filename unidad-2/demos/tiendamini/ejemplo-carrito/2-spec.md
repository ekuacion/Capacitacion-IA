# Spec: Carrito de compras con descuentos

| Campo | Valor |
|---|---|
| Estado | aprobada |
| Versión | 1.0 |
| Fecha | 2026-10-27 |
| Autor(es) | Camila Soto (PO), Diego Fuentes (dev) |
| Intent de origen | `1-intent.md` |

---

## 1. Contexto y objetivo

Hoy el cliente ve el costo de despacho recién al pagar, y muchos abandonan la compra.
Queremos que el carrito muestre el total final: subtotal, descuento, despacho y total.
Además, el carrito aplica el cupón de bienvenida y el despacho gratis según las reglas de abajo.

## 2. Historia de usuario

Como **cliente registrado de TiendaMini**, quiero **ver en mi carrito el total final con descuentos y despacho**,
para **saber cuánto voy a pagar antes de llegar al pago**.

## 3. Reglas de negocio

- **RN-01 — Moneda:** todos los montos (precio, subtotal, descuento, despacho, total) son enteros en CLP. No hay decimales.
- **RN-02 — Fórmula:** `total = subtotal − descuento + despacho`. `subtotal` es la suma de `precioUnitario × cantidad` de cada línea.
- **RN-03 — Cupón BIENVENIDA10:** descuento de 10% sobre el subtotal. Solo aplica si el cliente tiene **0 pedidos en estado "pagado"**. Este dato se obtiene del historial de pedidos en el servidor. Nunca viene en el request.
- **RN-04 — Vigencia del cupón:** BIENVENIDA10 es válido hasta el **2026-12-31 23:59:59, hora de Chile (America/Santiago)**, inclusive.
- **RN-05 — Redondeo:** el descuento porcentual se trunca hacia abajo al peso entero. Ejemplo: 10% de $12.345 = $1.234.
- **RN-06 — Despacho:** si el subtotal es **≥ $50.000**, el despacho es gratis. Si es **< $50.000**, cuesta **$3.990**. Si el carrito está vacío, el despacho es $0. El umbral se evalúa sobre el subtotal, antes de descuentos.
- **RN-07 — No acumulables:** el despacho gratis cuenta como un beneficio de $3.990. Si el cliente califica para el cupón y para el despacho gratis, se aplica **solo el beneficio de mayor monto**.
  - *Consecuencia (confirmada por la PO el 2026-10-27):* como 10% de $50.000 = $5.000 > $3.990, con BIENVENIDA10 válido y subtotal ≥ $50.000 **siempre gana el cupón** y se cobra despacho.
- **RN-08 — Límite por producto:** cada línea tiene entre 1 y 10 unidades. Si una operación deja una línea con más de 10, se rechaza completa y el carrito no cambia.
- **RN-09 — Código de cupón:** no distingue mayúsculas de minúsculas e ignora espacios al inicio y al final. Se guarda normalizado en mayúsculas.
- **RN-10 — Un cupón por carrito:** un carrito tiene como máximo un cupón.
- **RN-11 — Errores:** un cupón inexistente, vencido o no aplicable devuelve un error con código y mensaje (ver sección 6). El carrito queda sin cambios.

### Catálogo de prueba
Los criterios usan estos productos. Los `T-xx` existen solo en el entorno de tests.

| productoId | Nombre | Precio (CLP) |
|---|---|---|
| P-001 | Polera | 12990 |
| P-002 | Taza | 7500 |
| P-003 | Polerón | 30000 |
| T-01 | Producto de prueba A | 49999 |
| T-02 | Producto de prueba B | 50000 |
| T-03 | Producto de prueba C | 12345 |

### Endpoints
El cliente está autenticado. El `clienteId` sale del token, no del request.

| Método | Ruta | Cuerpo | Respuesta OK |
|---|---|---|---|
| GET | `/api/carrito` | — | 200 + carrito |
| POST | `/api/carrito/items` | `{ "productoId": "P-001", "cantidad": 2 }` (suma a lo que ya hay) | 200 + carrito |
| POST | `/api/carrito/cupon` | `{ "codigo": "BIENVENIDA10" }` | 200 + carrito |

`beneficioAplicado` puede ser: `NINGUNO`, `CUPON_BIENVENIDA10` o `DESPACHO_GRATIS`.

## 4. Criterios de aceptación

### Cantidades

**CA-01 — Agregar un producto**
- **Dado** un carrito vacío
- **Cuando** el cliente agrega 2 unidades de P-001 (Polera, $12.990)
- **Entonces** el carrito tiene 1 línea con `cantidad` 2 y `totalLinea` 25980, y `subtotal` es 25980.

**CA-02 — Exactamente 10 unidades se aceptan**
- **Dado** un carrito vacío
- **Cuando** el cliente agrega 10 unidades de P-002
- **Entonces** la respuesta es 200 y la línea de P-002 tiene `cantidad` 10.

**CA-03 — 11 unidades se rechazan**
- **Dado** un carrito vacío
- **Cuando** el cliente agrega 11 unidades de P-002
- **Entonces** la respuesta es 422 con `codigo` `CANTIDAD_MAXIMA_EXCEDIDA` y el carrito sigue vacío.

**CA-04 — El límite considera lo que ya está en el carrito**
- **Dado** un carrito con 8 unidades de P-002
- **Cuando** el cliente agrega 3 unidades más de P-002
- **Entonces** la respuesta es 422 con `codigo` `CANTIDAD_MAXIMA_EXCEDIDA` y la línea sigue con `cantidad` 8.

### Despacho

**CA-05 — Bajo el umbral se cobra despacho**
- **Dado** un carrito con 1 unidad de T-01 ($49.999) y sin cupón
- **Cuando** el cliente consulta el carrito
- **Entonces** `subtotal` 49999, `despacho` 3990, `descuento` 0, `total` 53989 y `beneficioAplicado` `NINGUNO`.

**CA-06 — En el umbral exacto el despacho es gratis**
- **Dado** un carrito con 1 unidad de T-02 ($50.000) y sin cupón
- **Cuando** el cliente consulta el carrito
- **Entonces** `despacho` 0, `descuento` 0, `total` 50000 y `beneficioAplicado` `DESPACHO_GRATIS`.

**CA-07 — Carrito vacío no cobra despacho**
- **Dado** un carrito vacío
- **Cuando** el cliente consulta el carrito
- **Entonces** `subtotal` 0, `despacho` 0, `total` 0 y `beneficioAplicado` `NINGUNO`.

### Cupón

**CA-08 — Cupón en primera compra, bajo el umbral**
- **Dado** un cliente con 0 pedidos pagados y un carrito con 2 × P-001 y 1 × P-002 (subtotal $33.480)
- **Cuando** aplica el cupón `BIENVENIDA10`
- **Entonces** `descuento` 3348, `despacho` 3990, `total` 34122 y `beneficioAplicado` `CUPON_BIENVENIDA10`.

**CA-09 — Cupón vs. despacho gratis: gana el mayor**
- **Dado** un cliente con 0 pedidos pagados y un carrito con 2 × P-003 (subtotal $60.000)
- **Cuando** aplica el cupón `BIENVENIDA10`
- **Entonces** `descuento` 6000, `despacho` 3990, `total` 57990 y `beneficioAplicado` `CUPON_BIENVENIDA10`.

**CA-10 — El descuento se trunca hacia abajo**
- **Dado** un cliente con 0 pedidos pagados y un carrito con 1 × T-03 (subtotal $12.345)
- **Cuando** aplica el cupón `BIENVENIDA10`
- **Entonces** `descuento` 1234 y `total` 15101.

**CA-11 — El código ignora mayúsculas y espacios**
- **Dado** el mismo cliente y carrito de CA-08
- **Cuando** aplica el cupón `"  bienvenida10 "`
- **Entonces** la respuesta es 200, `cupon` es `"BIENVENIDA10"` y `total` es 34122.

**CA-12 — Cupón fuera de la primera compra**
- **Dado** un cliente con 1 pedido pagado y un carrito con subtotal $33.480
- **Cuando** aplica el cupón `BIENVENIDA10`
- **Entonces** la respuesta es 422 con `codigo` `CUPON_NO_APLICABLE`, y el carrito sigue con `cupon` null y `total` 37470.

**CA-13 — Cupón inexistente**
- **Dado** cualquier cliente con un carrito con subtotal $33.480
- **Cuando** aplica el cupón `DESCUENTO50`
- **Entonces** la respuesta es 422 con `codigo` `CUPON_INVALIDO` y `mensaje` `"El cupón DESCUENTO50 no existe."`. El carrito no cambia.

**CA-14 — Cupón vencido**
- **Dado** un cliente con 0 pedidos pagados y la fecha actual 2027-01-01 00:00:00 hora de Chile
- **Cuando** aplica el cupón `BIENVENIDA10`
- **Entonces** la respuesta es 422 con `codigo` `CUPON_VENCIDO`. El carrito no cambia.

## 5. Ejemplos de entrada y salida

### Ejemplo 1 — Carrito sin cupón, bajo el umbral

Carrito con 2 × P-001 y 1 × P-002. Request: `GET /api/carrito`

Respuesta `200 OK`:
```json
{
  "items": [
    { "productoId": "P-001", "nombre": "Polera", "precioUnitario": 12990, "cantidad": 2, "totalLinea": 25980 },
    { "productoId": "P-002", "nombre": "Taza", "precioUnitario": 7500, "cantidad": 1, "totalLinea": 7500 }
  ],
  "subtotal": 33480,
  "cupon": null,
  "beneficioAplicado": "NINGUNO",
  "descuento": 0,
  "despacho": 3990,
  "total": 37470
}
```

### Ejemplo 2 — Cupón en primera compra, sobre el umbral

Cliente sin pedidos pagados. Carrito con 2 × P-003. Request: `POST /api/carrito/cupon`
```json
{ "codigo": "BIENVENIDA10" }
```

Respuesta `200 OK`:
```json
{
  "items": [
    { "productoId": "P-003", "nombre": "Polerón", "precioUnitario": 30000, "cantidad": 2, "totalLinea": 60000 }
  ],
  "subtotal": 60000,
  "cupon": "BIENVENIDA10",
  "beneficioAplicado": "CUPON_BIENVENIDA10",
  "descuento": 6000,
  "despacho": 3990,
  "total": 57990
}
```
El cupón ($6.000) gana al despacho gratis ($3.990). Por eso se cobra despacho (RN-07).

### Ejemplo 3 — Cupón inexistente (error)

Request: `POST /api/carrito/cupon`
```json
{ "codigo": "DESCUENTO50" }
```

Respuesta `422 Unprocessable Entity`:
```json
{
  "codigo": "CUPON_INVALIDO",
  "mensaje": "El cupón DESCUENTO50 no existe."
}
```
El carrito queda igual que antes del request.

## 6. Errores esperados

Todos los errores usan el cuerpo `{ "codigo": "...", "mensaje": "..." }`. En todos, el carrito queda sin cambios.

| Código | HTTP | Cuándo ocurre | Mensaje |
|---|---|---|---|
| `CUPON_INVALIDO` | 422 | El código no existe. | `El cupón {CODIGO} no existe.` |
| `CUPON_VENCIDO` | 422 | La fecha actual es posterior a la vigencia (RN-04). | `El cupón {CODIGO} venció el 31-12-2026.` |
| `CUPON_NO_APLICABLE` | 422 | El cliente tiene 1 o más pedidos pagados (RN-03). | `El cupón BIENVENIDA10 es solo para tu primera compra.` |
| `CANTIDAD_MAXIMA_EXCEDIDA` | 422 | La línea quedaría con más de 10 unidades (RN-08). | `Puedes llevar máximo 10 unidades de {nombre}.` |
| `CANTIDAD_INVALIDA` | 400 | `cantidad` es menor que 1 o no es un entero. | `La cantidad debe ser un número entero mayor o igual a 1.` |
| `PRODUCTO_NO_ENCONTRADO` | 404 | El `productoId` no existe en el catálogo. | `El producto {productoId} no existe.` |

`{CODIGO}` es el código ya normalizado (RN-09).

## 7. Fuera de alcance

- Pago, pasarela y creación del pedido.
- Quitar productos o disminuir cantidades (spec separada: CARRITO-02).
- Carritos de usuarios sin cuenta.
- Administración de cupones (crear, editar, desactivar). Solo existe BIENVENIDA10, configurado en el servidor.
- Validación de stock.
- Cálculo de despacho por comuna o peso. El despacho es tarifa única.

## 8. Preguntas abiertas

| ID | Pregunta | Responsable | ¿Bloquea? |
|---|---|---|---|
| P-01 | Un pedido pagado y luego reembolsado, ¿cuenta como "compra previa"? Por ahora sí cuenta (estado "pagado" en el historial). | PO | No |
| P-02 | Si el cupón vence mientras está en un carrito abierto, ¿se quita solo al consultar? Por ahora se mantiene hasta el pago (fuera de alcance). | PO | No |
| P-03 | ¿Se extiende la vigencia de BIENVENIDA10 en 2027? | Marketing | No |

## 9. Historial de cambios

| Versión | Fecha | Cambio | Aprobó |
|---|---|---|---|
| 1.0 | 2026-10-27 | Versión inicial | Camila Soto (PO) |
