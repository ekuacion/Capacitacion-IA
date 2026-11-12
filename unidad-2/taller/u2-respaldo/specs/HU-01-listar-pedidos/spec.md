# HU-01 · Listar pedidos con filtros

> Estado: aprobada · Versión 1.0 · Carpeta de respaldo `u2-respaldo` (plan B del paso 2 del taller de la Unidad 2)
> Es una spec de referencia: tu spec puede decidir distinto si es coherente y verificable.

## 1. Historia de usuario

Como **vendedor**, quiero **listar pedidos filtrando por cliente, estado y rango de fechas**, para **responder rápido las consultas de mis clientes sin entrar al sistema Delphi**.

## 2. Objetivo

Exponer un endpoint de solo lectura que devuelva los pedidos del repositorio en memoria, con filtros opcionales y paginación. Los filtros se combinan entre sí: un pedido aparece solo si cumple todos los filtros enviados.

## 3. Reglas de negocio

- **RN-01 · Endpoint:** `GET /api/pedidos`. Todos los parámetros son opcionales y van en la query string: `clienteId`, `estado`, `desde`, `hasta`, `pagina`, `tamano`.
- **RN-02 · Cliente:** `clienteId` filtra por coincidencia exacta (por ejemplo `C-001`). Un cliente sin pedidos devuelve una lista vacía con HTTP 200, no un error.
- **RN-03 · Estados válidos:** `Pendiente`, `Confirmado`, `Despachado`, `Entregado` y `Anulado`. El filtro no distingue mayúsculas de minúsculas (`despachado` vale). Cualquier otro valor es un error `ESTADO_INVALIDO`.
- **RN-04 · Fechas:** `desde` y `hasta` usan el formato `yyyy-MM-dd` y son **inclusivas**. Si `desde` es posterior a `hasta`, es un error `FECHAS_INVERTIDAS`. Una fecha con otro formato es un error `FECHA_INVALIDA`.
- **RN-05 · Paginación:** `pagina` empieza en 1 (por defecto 1). `tamano` va de 1 a 100 (por defecto 20). Fuera de esos rangos: `PAGINA_INVALIDA` o `TAMANO_INVALIDO`. Una página más allá del final devuelve `items` vacío con HTTP 200.
- **RN-06 · Orden:** por `Fecha` descendente (más recientes primero); si dos pedidos tienen la misma fecha, por `Id` descendente.
- **RN-07 · Respuesta:** `{ "items": [...], "total": n, "pagina": p, "tamano": t }`, donde `total` es la cantidad de pedidos que cumplen los filtros, antes de paginar.
- **RN-08 · Errores:** HTTP 400 con cuerpo `{ "codigo": "...", "mensaje": "..." }`. Si hay varios errores, se informa solo el primero en este orden: fechas, estado, paginación.

## 4. Criterios de aceptación

Todos usan los 45 pedidos de ejemplo de `PedidoRepositorioEnMemoria`.

- [ ] **CA-01 · Filtro por cliente.** Dado el repositorio de ejemplo, cuando listo con `clienteId=C-001`, entonces recibo `total` 5 y los ids 8, 16, 24, 32 y 40.
- [ ] **CA-02 · Cliente sin pedidos.** Cuando listo con `clienteId=C-099`, entonces recibo HTTP 200, `total` 0 e `items` vacío.
- [ ] **CA-03 · Filtro por estado.** Cuando listo con `estado=Despachado`, entonces recibo `total` 10 y todos los items tienen estado `Despachado`.
- [ ] **CA-04 · Estado sin distinguir mayúsculas.** Cuando listo con `estado=despachado`, entonces recibo `total` 10.
- [ ] **CA-05 · Estado inválido.** Cuando listo con `estado=Perdido`, entonces recibo HTTP 400 con `codigo` `ESTADO_INVALIDO`.
- [ ] **CA-06 · Rango de fechas.** Cuando listo con `desde=2026-10-01` y `hasta=2026-10-31`, entonces recibo `total` 23.
- [ ] **CA-07 · Fechas inclusivas.** Cuando listo con `desde=2026-10-31` y `hasta=2026-10-31`, entonces recibo `total` 1 y el pedido 21.
- [ ] **CA-08 · Fechas invertidas.** Cuando listo con `desde=2026-10-31` y `hasta=2026-10-01`, entonces recibo HTTP 400 con `codigo` `FECHAS_INVERTIDAS`.
- [ ] **CA-09 · Filtros combinados.** Cuando listo con `clienteId=C-001`, `desde=2026-10-01` y `hasta=2026-10-31`, entonces recibo `total` 2 y los ids 8 y 16, en ese orden.
- [ ] **CA-10 · Orden y página por defecto.** Cuando listo sin parámetros, entonces recibo `pagina` 1, `tamano` 20, `total` 45, 20 items, y los dos primeros son los ids 21 y 42.
- [ ] **CA-11 · Última página parcial.** Cuando listo con `pagina=3`, entonces recibo 5 items (ids 36, 17, 38, 19 y 40) y `total` 45.
- [ ] **CA-12 · Página fuera de rango.** Cuando listo con `pagina=4`, entonces recibo HTTP 200, `items` vacío y `total` 45.
- [ ] **CA-13 · Tamaño inválido.** Cuando listo con `tamano=500`, entonces recibo HTTP 400 con `codigo` `TAMANO_INVALIDO`.

## 5. Ejemplos de entrada y salida

**Ejemplo 1 · Filtros combinados**

```
GET /api/pedidos?clienteId=C-001&desde=2026-10-01&hasta=2026-10-31
→ 200
{ "items": [ { "id": 8, ... }, { "id": 16, ... } ], "total": 2, "pagina": 1, "tamano": 20 }
```

**Ejemplo 2 · Página parcial**

```
GET /api/pedidos?pagina=3&tamano=20
→ 200
{ "items": [ 5 pedidos ], "total": 45, "pagina": 3, "tamano": 20 }
```

**Ejemplo 3 · Error**

```
GET /api/pedidos?desde=2026-10-31&hasta=2026-10-01
→ 400
{ "codigo": "FECHAS_INVERTIDAS", "mensaje": "La fecha desde no puede ser posterior a la fecha hasta." }
```

## 6. Errores esperados

| Código | HTTP | Cuándo | Mensaje |
|---|---|---|---|
| `FECHAS_INVERTIDAS` | 400 | `desde` es posterior a `hasta` (RN-04) | `La fecha desde no puede ser posterior a la fecha hasta.` |
| `FECHA_INVALIDA` | 400 | `desde` o `hasta` no tiene formato `yyyy-MM-dd` (RN-04) | `Las fechas deben tener el formato yyyy-MM-dd.` |
| `ESTADO_INVALIDO` | 400 | El estado no está en la lista de RN-03 | `El estado {estado} no existe. Usa: Pendiente, Confirmado, Despachado, Entregado o Anulado.` |
| `PAGINA_INVALIDA` | 400 | `pagina` es menor que 1 (RN-05) | `La página debe ser 1 o mayor.` |
| `TAMANO_INVALIDO` | 400 | `tamano` es menor que 1 o mayor que 100 (RN-05) | `El tamaño de página debe estar entre 1 y 100.` |

## 7. Fuera de alcance

- Crear, editar o anular pedidos.
- Autenticación y permisos por vendedor (todos ven todos los pedidos).
- Base de datos: los datos siguen en memoria (PostgreSQL llega en la Unidad 5).
- Búsqueda por texto libre o por folio.
- Exportar a Excel o PDF.

## 8. Preguntas abiertas

- ¿Un vendedor debería ver solo los pedidos de sus clientes? (Se resolverá cuando exista autenticación.)
- ¿Los pedidos `Anulado` deberían ocultarse por defecto? Hoy se muestran si no se filtra por estado.

## 9. Historial de cambios

| Versión | Fecha | Cambio |
|---|---|---|
| 1.0 | 2026-11-04 | Versión inicial aprobada para la carpeta de respaldo. |
