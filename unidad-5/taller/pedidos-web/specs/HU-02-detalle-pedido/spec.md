# Spec: HU-02 · Detalle de un pedido

> Versión: 1.1 · Estado: aprobada (Claudio Ortiz Castillo, 2026-11-10) · Intent de origen: `specs/HU-02-detalle-pedido/intent.md`
> Autor del borrador: Claude Code · Aprobada por: Claudio Ortiz Castillo (regla 1 de `AGENTS.md`)

> Formato: `docs/plantilla-spec-breve.md`. Una spec breve cabe en una pantalla.
> Las decisiones marcadas con **[Decidir]** son tuyas; Claude solo propone. Hoy no queda ninguna abierta.

**Historia:** Como vendedor, quiero ver el detalle de un pedido (cliente, líneas, total y estado), para responderle al
cliente sin entrar al sistema legado.

## Qué devuelve

La ruta `GET /api/pedidos/{id}/detalle` devuelve un JSON con:

- Del pedido: `id`, `folio`, `fecha`, `estado` y `sucursal`.
- Del cliente: `clienteId` y `clienteNombre` (ya vienen en el pedido).
- `lineas`: cada una con `numero`, `producto`, `cantidad`, `precioUnitario` y `subtotal` (`cantidad × precioUnitario`),
  ordenadas por `numero`.
- `total`: la suma de los subtotales de las líneas.

Los montos son pesos chilenos (CLP), `decimal` sin decimales, **sin IVA**. La fecha va como `"2026-10-20"`.

### Ejemplo: pedido 8

```json
{
  "id": 8,
  "folio": "PED-STGO-000008",
  "fecha": "2026-10-20",
  "estado": "Entregado",
  "sucursal": "STGO",
  "clienteId": "C-001",
  "clienteNombre": "Ferretería El Clavo",
  "lineas": [
    { "numero": 1, "producto": "Harina para pan 25 kg", "cantidad": 3,  "precioUnitario": 21500, "subtotal": 64500 },
    { "numero": 2, "producto": "Mantequilla 250 g",     "cantidad": 12, "precioUnitario": 2950,  "subtotal": 35400 },
    { "numero": 3, "producto": "Bebida cola 1,5 L",     "cantidad": 10, "precioUnitario": 2020,  "subtotal": 20200 }
  ],
  "total": 120100
}
```

## Reglas de negocio

- **RN-01** · El `total` se calcula desde las líneas, no se copia de `Pedido.Total`.
- **RN-02** · El `subtotal` de una línea es `cantidad × precioUnitario`.
- **RN-03** · Las líneas salen ordenadas por `numero`, de menor a mayor.
- **RN-04** · Un pedido que existe pero no tiene líneas devuelve `lineas: []` y `total: 0` (decidido por Claudio).
- **RN-05** · Los errores salen como `{codigo, mensaje}`, con el código en una constante de Core (hoy solo `PEDIDO_NO_ENCONTRADO`).

## Criterios de aceptación

- **CA-01 · Total desde las líneas.** Dado el pedido 8, cuando pido su detalle, entonces el `total` es 120100 y es
  igual a la suma de los subtotales de sus 3 líneas.
- **CA-02 · Contenido del detalle.** Dado el pedido 8, cuando pido su detalle, entonces trae el folio
  `PED-STGO-000008`, el estado `Entregado`, el cliente `C-001` (`Ferretería El Clavo`) y sus 3 líneas ordenadas por
  número.
- **CA-03 · Pedido inexistente.** Dado un id que no existe (por ejemplo 999), cuando pido su detalle, entonces la API
  responde 404 con el cuerpo `{ "codigo": "PEDIDO_NO_ENCONTRADO", "mensaje": "No existe el pedido 999." }`.
- **CA-04 · La ruta existente no cambia.** Dado el pedido 8, cuando pido `GET /api/pedidos/8`, entonces responde 200
  con el pedido tal como antes (sin `lineas`), y la ruta nueva no la afecta.

## Si el pedido no existe

La API responde `404` con un cuerpo `{codigo, mensaje}`, el mismo formato de error de la HU-01 (RN-10), para que un
futuro frontend pueda explicarle al usuario qué pasó:

```json
{ "codigo": "PEDIDO_NO_ENCONTRADO", "mensaje": "No existe el pedido 999." }
```

En Core, el servicio devuelve un resultado que contiene o el detalle o un error con ese `codigo` y `mensaje` (RN-05).
El texto del mensaje, con el id incluido, lo aprobó Claudio.

## Fuera de alcance

- IVA, descuentos y recargos (Unidad 5).
- Formato de moneda para mostrar en pantalla.
- Usar `ClientesService`: el nombre del cliente sale del pedido.

## Historial

| Versión | Fecha | Cambio | Quién |
|---|---|---|---|
| 0.1 | 2026-11-10 | Borrador inicial a partir del intent y de la spec de respaldo | Claude Code |
| 0.2 | 2026-11-10 | El 404 lleva cuerpo `{codigo, mensaje}` (RN-05, CA-03) para que un frontend pueda informar al usuario | Claude Code, a pedido de Claudio |
| 0.3 | 2026-11-10 | Decididas: RN-04 (sin líneas: `lineas: []` y `total: 0`) y el texto del mensaje de error | Claudio |
| 1.0 | 2026-11-10 | Spec aprobada por el PO | Claudio Ortiz Castillo |
| 1.1 | 2026-11-10 | Los tests HTTP pasan a estar dentro del alcance (T4); nuevo CA-04: la ruta `GET /api/pedidos/{id}` no cambia | Claudio Ortiz Castillo |
