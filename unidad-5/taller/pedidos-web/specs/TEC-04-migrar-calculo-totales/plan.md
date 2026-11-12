# Plan: TEC-04 · Migrar el cálculo de totales a C#

> Spec de origen: `specs/TEC-04-migrar-calculo-totales/spec.md` v1.0 · Autor: Claude Code · Estado: aprobado (Claudio Ortiz Castillo, 2026-11-11)

## Enfoque

Estrategia de la guía: **congelar → migrar → corregir**. La etapa 1 convierte el CSV en 60 tests rojos y deja solo la firma.
La etapa 2 activa el hook, usa `analista-legado` para explicar `UCalculoTotales.pas` y escribe el C# hasta el verde, con los
defectos incluidos. La etapa 3 corrige R05 en un commit aparte, actualiza los casos del CSV y escribe los dos documentos.

## Reparto de los 80 minutos

| Min | Tarea | Qué pasa | Si te atrasas |
|---|---|---|---|
| 10 | T0 · Preparación | Spec aprobada, rama y base verde (el traspaso ya está hecho) | — |
| 20 | T1 · Etapa 1 | Tests desde el CSV y firma vacía | Plan B: `u5-respaldo/` |
| 25 | T2 · Etapa 2 | Hook, explicación del legado, C# en verde | El hook se puede dejar para el final si estorba |
| 20 | T3 · Etapa 3 | R05, CSV, H1 y plan de migración | Los dos documentos pueden ser breves |
| 5 | T4 · Cierre | Comprobar CA-01 a CA-09 | — |

## Decisiones técnicas

- **Una sola clase estática `CalculoTotales`** con los records `LineaCalculo` y `ResultadoTotales`, en `Totales/`; sin dependencias.
- **Redondeo aislado en un método** (`RedondearAlPeso`) para que R05 sea un cambio de una línea en la etapa 3: `Math.Floor(valor + 0.49m)` pasa a `Math.Floor(valor + 0.5m)`.
- **`decimal` en toda la cadena.** El legado usa `Double`, pero una réplica en decimal (hecha como comprobación previa) reproduce los 60 casos del CSV; con +0,5 cambian 13 casos y la suma pasa de 58.202.135 a 58.202.144, como dice la guía.
- **El CSV es el contrato:** el proyecto de tests lo copia a su salida; nadie lo edita para que un test pase.
- **Descartado:** `Math.Round` (no replica el 0,50 hacia abajo), paquetes de lectura de CSV (regla 8) y migrar el `PorcentajeVolumen` del grabado (decide contabilidad).

## Riesgos

| Riesgo | Mitigación |
|---|---|
| El hook (sintaxis bash) falla en Windows | Probarlo con un test en rojo antes de depender de él; si no corre, ejecutar `dotnet test` a mano |
| Claude edita el CSV para que pasen los tests | CA-01 (`git diff`) y revisión antes de cada commit |
| Diferencias de coma flotante `Double` frente a `decimal` | La réplica ya coincide en los 60 casos; si un caso falla, se investiga ese caso y no se toca el CSV |
| Mezclar R05 con «igual al legado» | Commit aparte, verificado con `git log --stat` |
