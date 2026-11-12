# Tasks: TEC-04 · Migrar el cálculo de totales a C#

> Plan de origen: `specs/TEC-04-migrar-calculo-totales/plan.md` · Spec: v1.0 (aprobada). Una tarea = una etapa con su verificación y su commit.

## Resumen

| ID | Tarea | Min | CA cubiertos | Estado |
|---|---|---|---|---|
| T0 | Aprobar spec y plan; rama `feature/u5-taller-5` | 10 | — | hecha |
| T1 | Etapa 1 · Congelar: 60 tests rojos y firma vacía | 20 | CA-01, CA-02 | hecha (60 rojos por NotImplementedException, 78 previos en verde, CSV intacto) |
| T2 | Etapa 2 · Migrar: hook y C# en verde, igual al legado | 25 | CA-01, CA-03, CA-04, CA-05 | hecha (138 en verde, suma 58.202.135, sin double/float; hook confirmado: una edición con tests en rojo devolvió el error (CA-05)) |
| T3 | Etapa 3 · Corregir R05 y documentar | 20 | CA-06, CA-07, CA-08 | hecha (R05 en 345a1a9: 13 casos, 58.202.144; H1 y plan documentados) |
| T4 | Cierre: comprobaciones finales | 5 | CA-09 | hecha (legado y sln sin cambios; 143 tests en verde) |

## T0 — Aprobar y abrir la rama
- **Hecho cuando:** spec y plan en «aprobada/aprobado» y `git branch --show-current` → `feature/u5-taller-5`.
- **Commit:** `docs(u5): planificación y aprobación de TEC-04 (migrar el cálculo de totales)`

## T1 — Etapa 1 · Congelar
- **Archivos:** `tests/PedidosWeb.Core.Tests/Totales/GoldenMasterTests.cs`, `PedidosWeb.Core.Tests.csproj` (copia del CSV), `src/PedidosWeb.Core/Totales/CalculoTotales.cs` (solo firma).
- **No tocar:** `migracion/golden-master.csv`, `legado/`, `PedidosWeb.sln`.
- **Verificación:** `dotnet test` → 60 rojos de `GoldenMasterTests` por `NotImplementedException`; `git diff -- migracion` vacío.
- **Commit:** `test(u5): golden master del cálculo de totales, 60 casos en rojo (T1)`

## T2 — Etapa 2 · Migrar
- **Archivos:** `src/PedidosWeb.Core/Totales/CalculoTotales.cs`, `.claude/settings.json` (hook sumado a la regla `deny`).
- **Qué hacer:** probar el hook, reiniciar Claude Code y confirmar con `/hooks`; `analista-legado` explica `UCalculoTotales.pas` (con `archivo:línea`); implementar en `decimal` igual al legado (incluido el redondeo `+0,49`).
- **Verificación:** 60 de 60 en verde; suma de `total` = 58.202.135; `grep -nE "double|float" src/PedidosWeb.Core/Totales` vacío; los 83 tests previos en verde.
- **Commit:** `feat(u5): cálculo de totales en C#, igual al legado (T2)`

## T3 — Etapa 3 · Corregir y planificar
- **Archivos:** `CalculoTotales.cs` (una línea), `migracion/golden-master.csv` (13 casos), `docs/migracion/*.md`.
- **Qué hacer:** commit 1 con R05 y el CSV (13 casos, suma 58.202.144); commit 2 con los dos documentos (R05, H1 y el plan).
- **Verificación:** 60 de 60 en verde con el CSV nuevo; `git log --stat` muestra R05 aislado.
- **Commits:** `fix(u5): R05 redondeo al peso, 0,50 sube (T3)` y `docs(u5): defectos corregidos y plan de migración (T3)`

## T4 — Cierre
- **Verificación:** `git status -- legado PedidosWeb.sln` vacío; `dotnet test` todo en verde; spec en «implementada»; integrar a `develop` lo hace Claudio.
- **Commit:** `docs(u5): cierre de TEC-04, cálculo de totales migrado (T4)`
