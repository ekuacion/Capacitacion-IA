# specs/

Una carpeta por historia de usuario, con su `spec.md` (y su `plan.md` cuando corresponde).

| Carpeta | Unidad | Historia |
|---|---|---|
| `HU-01-listar-pedidos/` | U2 | Como vendedor, quiero listar pedidos filtrando por cliente, estado y rango de fechas. |
| `HU-02-detalle-pedido/` | U3 | Como vendedor, quiero ver el detalle de un pedido: cliente, líneas, total y estado. |
| `TEC-01-ordenar-clientes-service/` | U3 | Trabajo técnico, no una historia: ordenar `ClientesService` sin cambiar su comportamiento. |
| `TEC-02-skill-documentar-modulo/` | U3 | Trabajo técnico: la Skill `documentar-modulo` y la documentación de `ClientesService`. |

Las carpetas `HU-` son historias de usuario (comportamiento nuevo). Las `TEC-` son trabajo técnico que no cambia lo que
el usuario ve, como un refactor; su spec es el comportamiento que debe quedar idéntico.

No se escribe código sin una spec aprobada (ver `AGENTS.md`).
