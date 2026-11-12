# migracion/

`tarifas-golden.csv`: 20 casos de `TarifaBase` de `legado/src/UTarifas.pas`.

| Columna | Qué es |
|---|---|
| `caso` | `A01`–`A15`: los 15 arriendos de `legado/db/datos/ARRIENDOS.csv`. `B01`–`B05`: casos borde agregados a mano. |
| `tarifa_dia` | Tarifa diaria del auto (`AUTOS.csv`). |
| `dias` | Días entre la fecha de salida y la pactada (mínimo 1). |
| `fecha_salida` | Fecha de salida, formato `yyyy-MM-dd`. |
| `cant_arriendos` | Arriendos del cliente en `ARRIENDOS.csv`. |
| `tarifa_base` | Lo que calcula hoy `TarifaBase`: el valor esperado. |

El legado no compila, así que estos valores salen de una réplica de `UTarifas.pas` hecha por el instructor. En un proyecto real se capturan ejecutando el sistema viejo.
