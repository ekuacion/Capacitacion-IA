# Spec: HU-01 · Listar pedidos con filtros

> Formato: `specs/plantillas/spec-plantilla-comentada.md`. Regla de oro: si alguien tiene que
> preguntarte algo para escribir un test, falta información en la spec.

| Campo | Valor |
|---|---|
| Estado | aprobada |
| Versión | 1.1 |
| Fecha | 2026-11-10 |
| Autor(es) | Claudio Ortiz Castillo (PO y Dev) · borrador asistido por Claude Code |
| Intent de origen | `specs/HU-01-listar-pedidos/intent.md` |

---

## 1. Historia de usuario

Como vendedor, quiero listar pedidos filtrando por cliente, estado y rango de fechas, para responder las
consultas de mis clientes sobre el estado de sus pedidos.

## 2. Objetivo

Los vendedores necesitan consultar pedidos desde el navegador sin entrar al sistema Delphi. Esta historia
entrega un endpoint que lista los pedidos de ejemplo (en memoria) acotados por cliente, estado, sucursal y
rango de fechas, en un orden fijo, paginados y con errores explícitos, para que el mismo filtro devuelva
siempre el mismo resultado.

## 3. Reglas de negocio

- **RN-01:** El listado se expone en `GET /api/pedidos/buscar`. Filtros (query): `clienteId`, `estado`, `sucursal`,
  `desde` y `hasta`. Paginación (query): `pagina` y `tamanoPagina`.
- **RN-02:** Hay que enviar **al menos uno** de los cinco filtros. Los parámetros de paginación no cuentan como filtro.
  Un filtro enviado vacío o con solo espacios (ej.: `estado=`) cuenta como **no enviado**. Sin filtros → `FILTRO_REQUERIDO`.
- **RN-03:** `clienteId` se compara completo con `Pedido.ClienteId`, **sin distinguir mayúsculas** (`c-004` = `C-004`).
  Un cliente que no existe no es error: devuelve 200 con `pedidos: []` y `total: 0`.
- **RN-04:** Los filtros se combinan con **Y**: un pedido aparece en el resultado solo si cumple todos los filtros enviados.
- **RN-05:** El resultado se ordena por `Fecha` ascendente y, a igual fecha, por `Id` ascendente. Como los datos de ejemplo
  no repiten fechas, el desempate se prueba con un repositorio de test (CA-23).
- **RN-06:** `desde` y `hasta` usan el formato `yyyy-MM-dd` y son **inclusivos**: `desde ≤ Fecha ≤ hasta`.
  - Se puede enviar solo `desde` (desde esa fecha en adelante) o solo `hasta` (hasta esa fecha).
  - Una fecha que no cumple `yyyy-MM-dd` o no existe (ej.: `31-10-2026`, `2026-02-30`) → `FECHA_INVALIDA`.
  - Si `desde > hasta` → `FECHAS_INVERTIDAS`. Si `desde = hasta`, es válido (un solo día).
- **RN-07:** Estados válidos: `Pendiente`, `Confirmado`, `Despachado`, `Entregado` y `Anulado`, **sin distinguir mayúsculas**.
  Cualquier otro valor → `ESTADO_INVALIDO`.
- **RN-08:** Sucursales válidas: `ANT`, `CON`, `VAL` y `STGO`, **sin distinguir mayúsculas**. Cualquier otro valor → `SUCURSAL_INVALIDA`.
- **RN-09:** Paginación: `pagina` empieza en 1 (por defecto 1). `tamanoPagina` va de 1 a **50** (por defecto **10**).
  - `pagina < 1`, `tamanoPagina < 1`, `tamanoPagina > 50`, o un valor que no es un número entero (ej.: `pagina=abc`)
    → `PAGINACION_INVALIDA`.
  - Un parámetro de paginación enviado vacío o con solo espacios (ej.: `pagina=`) cuenta como **no enviado** y usa
    su valor por defecto.
  - Una página más allá del final no es error: devuelve 200 con `pedidos: []` y el `total` real.
  - Respuesta exitosa: `{ "pedidos": [...], "total": N, "pagina": P, "tamanoPagina": T }`, donde `total` es la cantidad
    de pedidos que cumplen los filtros (no solo los de la página).
- **RN-10:** Formato de error: HTTP **400** con cuerpo `{ "codigo": "...", "mensaje": "..." }`.
- **RN-11:** Si hay varios errores, se informa **solo el primero**, en este orden: `FILTRO_REQUERIDO` → `FECHA_INVALIDA` →
  `FECHAS_INVERTIDAS` → `ESTADO_INVALIDO` → `SUCURSAL_INVALIDA` → `PAGINACION_INVALIDA`.

## 4. Criterios de aceptación

Todos parten **dado** los 45 pedidos de ejemplo del repositorio. Salvo que se diga otra cosa, se usa la paginación por defecto.

**Filtro por cliente**
- [ ] **CA-01 — Cliente existente.** **Cuando** busco con `clienteId=C-004`, **entonces** recibo ids **15, 7, 39, 31, 23** en ese orden y `total: 5`.
- [ ] **CA-02 — Cliente sin distinguir mayúsculas.** **Cuando** busco con `clienteId=c-004`, **entonces** recibo los mismos ids que en CA-01.
- [ ] **CA-03 — Cliente inexistente.** **Cuando** busco con `clienteId=C-999`, **entonces** recibo 200 con `pedidos: []` y `total: 0`.

**Orden**
- [ ] **CA-04 — Orden por fecha ascendente.** **Cuando** busco con `clienteId=C-001`, **entonces** recibo ids **40, 32, 24, 16, 8**
  en ese orden (fechas 2026-09-02, 09-14, 09-26, 10-08 y 10-20).

**Filtro por estado**
- [ ] **CA-05 — Estado válido.** **Cuando** busco con `estado=Anulado`, **entonces** recibo ids **38, 11, 20, 29, 2** y `total: 5`.
- [ ] **CA-06 — Estado sin distinguir mayúsculas y página por defecto.** **Cuando** busco con `estado=entregado`, **entonces**
  recibo 10 pedidos (ids 40, 17, 15, 13, 26, 24, 22, 35, 33, 31), todos `Entregado`, con `total: 15`, `pagina: 1` y `tamanoPagina: 10`.
- [ ] **CA-07 — Estado inválido.** **Cuando** busco con `estado=Cancelado`, **entonces** recibo 400 con `codigo: ESTADO_INVALIDO`.

**Filtro por sucursal**
- [ ] **CA-08 — Sucursal válida.** **Cuando** busco con `sucursal=stgo&tamanoPagina=50`, **entonces** recibo 11 pedidos, todos de
  `STGO`, con `total: 11`.
- [ ] **CA-09 — Sucursal inválida.** **Cuando** busco con `sucursal=SCL`, **entonces** recibo 400 con `codigo: SUCURSAL_INVALIDA`.

**Rango de fechas**
- [ ] **CA-10 — Rango inclusivo.** **Cuando** busco con `desde=2026-10-01&hasta=2026-10-31&tamanoPagina=50`, **entonces** recibo
  23 pedidos con `total: 23`; el primero es el id 41 (2026-10-01) y el último el id 21 (2026-10-31).
- [ ] **CA-11 — Solo `desde`.** **Cuando** busco con `desde=2026-10-28`, **entonces** recibo ids **23, 2, 42, 21** y `total: 4`.
- [ ] **CA-12 — Solo `hasta`.** **Cuando** busco con `hasta=2026-09-05`, **entonces** recibo ids **40, 19, 38** y `total: 3`.
- [ ] **CA-13 — Fechas invertidas.** **Cuando** busco con `desde=2026-10-31&hasta=2026-10-01`, **entonces** recibo 400 con
  `codigo: FECHAS_INVERTIDAS`.
- [ ] **CA-14 — Fecha mal formada.** **Cuando** busco con `desde=31-10-2026`, **entonces** recibo 400 con `codigo: FECHA_INVALIDA`.

**Combinación y filtros obligatorios**
- [ ] **CA-15 — Filtros con Y.** **Cuando** busco con `clienteId=C-004&estado=Entregado`, **entonces** recibo ids **15, 31** y `total: 2`.
- [ ] **CA-16 — Sin filtros.** **Cuando** busco sin filtros (o solo con `pagina=1`), **entonces** recibo 400 con `codigo: FILTRO_REQUERIDO`.

**Paginación**
- [ ] **CA-17 — Página intermedia.** **Cuando** busco con `desde=2026-09-01&pagina=3&tamanoPagina=20`, **entonces** recibo ids
  **44, 23, 2, 42, 21**, con `total: 45`, `pagina: 3` y `tamanoPagina: 20`.
- [ ] **CA-18 — Página más allá del final.** **Cuando** busco con `clienteId=C-004&pagina=2`, **entonces** recibo 200 con
  `pedidos: []` y `total: 5`.
- [ ] **CA-19 — Paginación fuera de rango.** **Cuando** busco con `clienteId=C-004&tamanoPagina=51` y, aparte, con
  `clienteId=C-004&pagina=0`, **entonces** en ambos casos recibo 400 con `codigo: PAGINACION_INVALIDA`.

**Orden de los errores**
- [ ] **CA-20 — Se informa solo el primer error.** **Cuando** busco con `estado=Cancelado&desde=2026-10-31&hasta=2026-10-01`,
  **entonces** recibo 400 con `codigo: FECHAS_INVERTIDAS` (va antes que `ESTADO_INVALIDO`).

**HTTP (tests del endpoint)**
- [ ] **CA-21 — Respuesta exitosa por HTTP.** **Cuando** hago `GET /api/pedidos/buscar?clienteId=C-004`, **entonces** recibo
  200 y un JSON con `pedidos` (5 elementos con los campos del Ejemplo 1), `total: 5`, `pagina: 1` y `tamanoPagina: 10`.
- [ ] **CA-22 — Error por HTTP.** **Cuando** hago `GET /api/pedidos/buscar`, **entonces** recibo 400 y exactamente el cuerpo del Ejemplo 3.

**Bordes resueltos en la v0.3**
- [ ] **CA-23 — Desempate por Id.** **Dado** un repositorio de test con dos pedidos del 2026-09-15, cargados en este orden:
  id 9 y luego id 3, **cuando** busco con `desde=2026-09-15&hasta=2026-09-15`, **entonces** recibo los ids **3, 9** en ese orden.
- [ ] **CA-24 — Paginación no numérica.** **Cuando** busco con `clienteId=C-004&pagina=abc`, **entonces** recibo 400 con
  `codigo: PAGINACION_INVALIDA`.
- [ ] **CA-25 — Filtro vacío.** **Cuando** busco solo con `estado=`, **entonces** recibo 400 con `codigo: FILTRO_REQUERIDO`;
  y **cuando** busco con `estado=&clienteId=C-004`, **entonces** recibo los mismos ids que en CA-01.

**Bordes resueltos en la v1.1**
- [ ] **CA-26 — Paginación vacía.** **Cuando** busco con `clienteId=C-004&pagina=&tamanoPagina=`, **entonces** recibo 200
  con los mismos ids que en CA-01, `total: 5`, `pagina: 1` y `tamanoPagina: 10`.

## 5. Ejemplos de entrada y salida

### Ejemplo 1 — Pedidos de C-004

`GET /api/pedidos/buscar?clienteId=C-004` → **200**

```json
{
  "pedidos": [
    { "id": 15, "folio": "PED-VAL-000015", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-09-09", "estado": "Entregado",  "total": 23750,  "sucursal": "VAL" },
    { "id": 7,  "folio": "PED-VAL-000007", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-09-21", "estado": "Confirmado", "total": 33150,  "sucursal": "VAL" },
    { "id": 39, "folio": "PED-VAL-000039", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-10-04", "estado": "Despachado", "total": 113050, "sucursal": "VAL" },
    { "id": 31, "folio": "PED-VAL-000031", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-10-16", "estado": "Entregado",  "total": 122450, "sucursal": "VAL" },
    { "id": 23, "folio": "PED-VAL-000023", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-10-28", "estado": "Confirmado", "total": 14350,  "sucursal": "VAL" }
  ],
  "total": 5,
  "pagina": 1,
  "tamanoPagina": 10
}
```

### Ejemplo 2 — Página 3 de todo el período

`GET /api/pedidos/buscar?desde=2026-09-01&pagina=3&tamanoPagina=20` → **200**. `pedidos` trae los ids 44, 23, 2, 42 y 21
(fechas 2026-10-27 a 2026-10-31), con `"total": 45, "pagina": 3, "tamanoPagina": 20`.

### Ejemplo 3 — Sin filtros (error)

`GET /api/pedidos/buscar` → **400**

```json
{ "codigo": "FILTRO_REQUERIDO", "mensaje": "Debes indicar al menos un filtro: clienteId, estado, sucursal, desde o hasta." }
```

### Ejemplo 4 — Fechas invertidas (error)

`GET /api/pedidos/buscar?desde=2026-10-31&hasta=2026-10-01` → **400**

```json
{ "codigo": "FECHAS_INVERTIDAS", "mensaje": "La fecha desde no puede ser posterior a la fecha hasta." }
```

## 6. Errores esperados

Todos responden HTTP 400 con `{ "codigo", "mensaje" }` y se evalúan en el orden de la tabla (RN-11).

| Código | HTTP | Cuándo ocurre | Mensaje | Estado después |
|---|---|---|---|---|
| `FILTRO_REQUERIDO` | 400 | Ningún filtro enviado (RN-02) | Debes indicar al menos un filtro: clienteId, estado, sucursal, desde o hasta. | Sin cambios (es una consulta) |
| `FECHA_INVALIDA` | 400 | `desde` o `hasta` no cumple `yyyy-MM-dd` o no existe (RN-06) | La fecha debe tener el formato yyyy-MM-dd. | Sin cambios |
| `FECHAS_INVERTIDAS` | 400 | `desde > hasta` (RN-06) | La fecha desde no puede ser posterior a la fecha hasta. | Sin cambios |
| `ESTADO_INVALIDO` | 400 | `estado` fuera de la lista de RN-07 | El estado debe ser Pendiente, Confirmado, Despachado, Entregado o Anulado. | Sin cambios |
| `SUCURSAL_INVALIDA` | 400 | `sucursal` fuera de la lista de RN-08 | La sucursal debe ser ANT, CON, VAL o STGO. | Sin cambios |
| `PAGINACION_INVALIDA` | 400 | `pagina < 1`, `tamanoPagina < 1` o `> 50`, o no es un número entero (RN-09) | La página debe ser 1 o mayor y el tamaño de página, entre 1 y 50. | Sin cambios |

## 7. Fuera de alcance

- Interfaz web (frontend): solo se construye el endpoint de la API.
- Autenticación: queda para otra historia (además, `Pedido` no tiene un campo de vendedor para filtrar por quien consulta).
- Búsqueda por nombre de cliente: solo se filtra por `ClienteId` (RN-03).
- Persistencia en base de datos: los datos siguen en memoria; PostgreSQL llega en la Unidad 5 (README).
- Otros filtros (folio, monto) y otros órdenes que no sean el de RN-05.

## 8. Preguntas abiertas

Sin preguntas abiertas. Las resueltas quedan aquí como registro:

| ID | Pregunta | Respuesta (v0.3) | Responsable |
|---|---|---|---|
| P-01 | ¿Cómo se prueba el desempate por `Id`, si los datos de ejemplo no repiten fechas? | Con un repositorio de test (RN-05, CA-23) | Dev (Claudio) |
| P-02 | ¿`pagina=abc` responde `PAGINACION_INVALIDA`? | Sí (RN-09, CA-24) | PO (Claudio) |
| P-03 | ¿Un filtro vacío (`estado=`) cuenta como no enviado? | Sí (RN-02, CA-25) | PO (Claudio) |
| P-04 | ¿Un parámetro de paginación vacío (`pagina=`) es error o usa el valor por defecto? | Usa el valor por defecto (v1.1: RN-09, CA-26) | PO (Claudio) |

## 9. Historial de cambios

| Versión | Fecha | Cambio | Aprobó |
|---|---|---|---|
| 0.1 | 2026-11-09 | Borrador inicial (paso 1, prompt maestro) | — |
| 0.2 | 2026-11-09 | Paso 2: se resuelven todos los TODO; se agregan el filtro por sucursal, la paginación, el formato y el orden de errores, y los tests HTTP; 22 criterios | — |
| 0.3 | 2026-11-09 | Se resuelven P-01 a P-03 (desempate con repositorio de test, paginación no numérica y filtro vacío); se agregan CA-23 a CA-25; intent de origen `intent.md` | — |
| 1.0 | 2026-11-10 | Aprobada sin cambios de contenido respecto de la v0.3 (incluye los mensajes de error de §6) | Claudio Ortiz Castillo (PO) |
| 1.1 | 2026-11-10 | Se resuelve P-04 (surgió en T3): la paginación vacía usa el valor por defecto (RN-09); se agrega CA-26 | Claudio Ortiz Castillo (PO) |
