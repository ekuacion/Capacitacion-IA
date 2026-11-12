# Tasks: TEC-05 · Calidad, pruebas y CI/CD (GitHub + Docker local)

> Plan de origen: `specs/TEC-05-calidad-pruebas-ci/plan.md` · Spec: v0.5 (implementada parcialmente). Una tarea = una etapa con su verificación y su commit.

## Resumen

| ID | Tarea | Min | CA cubiertos | Estado |
|---|---|---|---|---|
| T0 | Aprobar spec y plan; rama `feature/u6-taller-6`; fila en `specs/README.md` | 5 | CA-01 | hecha |
| T1 | Etapa 1 · Normalizar H1 (`>=`): código, 13 casos del CSV y documento | 15 | CA-03 | hecha |
| T2 | Etapa 2 · 5 tests de borde y 2 de umbral de `CalculoTotales`, cada uno roto a mano | 20 | CA-02 | hecha (sin verificar por mutación) |
| T3 | Etapa 3b · `ci.yml` y `claude.yml`; Claudio sube, instala la App y crea el secreto | 15 | CA-10, CA-11 | hecha (falta el verde en Actions) |
| T4 | Etapa 3 · `Dockerfile`, `.dockerignore` y `scripts/ci-local.ps1`; API desplegada en Docker | 15 | CA-04, CA-05, CA-06 | hecha (CA-05 sin probar) |
| T5 | Etapa 4 · Falla a mano (IVA), PR, `@claude`, corrección y merge (Claudio) | 20 | CA-07 | no ejecutada (por tiempo) |
| T6 | Cierre · registro completo y comprobaciones finales | 5 | CA-08, CA-09 | hecha |

## T0 — Aprobar y abrir la rama
- **Hecho cuando:** spec y plan en «aprobada/aprobado», D-07 a D-10 resueltas (v0.5), `git branch --show-current` → `feature/u6-taller-6` y `dotnet test` → 143 en verde.
- **Commit:** `docs(u6): planificación y aprobación de TEC-05 (calidad, pruebas y CI)`

## T1 — Etapa 1 · Normalizar H1
- **Archivos:** `src/PedidosWeb.Core/Totales/CalculoTotales.cs` (dos operadores y su comentario), `migracion/golden-master.csv` (13 casos), `docs/migracion/defectos-corregidos.md` (y `plan-migracion.md` si menciona H1).
- **No tocar:** `legado/`, `PedidosWeb.sln`.
- **Qué hacer:** commit 1 con el cambio de código y los 13 casos (columnas `porc_descuento`, `descuento`, `neto`, `iva`, `total`); 60 de 60 en verde y suma de `total` 57.773.744. Commit 2 con la documentación.
- **Verificación:** commit 1 solo con código y CSV; `git diff -- legado` vacío; H1 aislado en `git log --stat`.
- **Commits:** `fix(u6): H1 umbrales de descuento con >=, como en el grabado (T1)` y `docs(u6): H1 corregido en defectos-corregidos (T1)`

## T2 — Etapa 2 · Tests que sí prueban
- **Archivos:** `tests/PedidosWeb.Core.Tests/Totales/CalculoTotalesBordeTests.cs`.
- **Qué hacer:** Claude explica cada caso (B1–B5 y los dos de umbral) y espera aprobación; escribe un test por caso; Claudio rompe el código en Rider, ve el test fallar y revierte.
- **Verificación:** 143 + los tests que sobrevivan, en verde; `git diff -- migracion legado` vacío; cada test falló una vez.
- **Commit:** `test(u6): tests de borde y de umbral de CalculoTotales revisados a mano (T2)`

## T3 — Etapa 3b · Pipeline en GitHub
- **Archivos:** `.github/workflows/ci.yml`, `.github/workflows/claude.yml` (en la **raíz del repo**, no en `pedidos-web`).
- **Qué hacer:** Claude explica cada bloque y escribe los dos archivos; Claudio sube el código al repo, mergea los dos workflows a `main` (`claude.yml` solo responde desde ahí), instala la App, ejecuta `claude setup-token` y crea el secreto; luego comprueba el verde de `ci.yml`.
- **Verificación:** `contents: read`, timeout, versiones fijas, token solo como secreto; `git grep -n -iE "oauth|token"` solo con el nombre del secreto; `ci.yml` en verde en Actions.
- **Commit:** `ci(u6): ci.yml y claude.yml con permisos mínimos (T3)`

## T4 — Etapa 3 · CI y despliegue local
- **Archivos:** `Dockerfile`, `.dockerignore`, `scripts/ci-local.ps1`, `.gitignore` (suma `ci-local.log`).
- **Qué hacer:** Claude explica cada bloque antes de escribirlo; Claudio abre Docker Desktop; se ejecuta `scripts/ci-local.ps1`; se comprueba `/salud`; con un test en rojo (lo rompe Claudio) se ve que la compuerta cierra, y se revierte.
- **Verificación:** CA-04, CA-05 y CA-06.
- **Commit:** `ci(u6): CI local y despliegue en Docker (T4)`

## T5 — Etapa 4 · La falla
- **Archivos:** los que Claudio rompa y corrija; `docs/calidad/registro-u6.md`.
- **Qué hacer:** Claudio rompe la tasa de IVA en Rider, abre el PR, comenta `@claude`, decide, corrige y hace el merge con el CI en verde; Claude solo anota en el registro lo que se le dicte.
- **Verificación:** PR en verde y merge hecho; el script local también en verde.
- **Commit:** `docs(u6): registro de la falla y su resolución (T5)`

## T6 — Cierre
- **Verificación:** `git status -- legado PedidosWeb.sln AGENTS.md` vacío; `dotnet test` en verde; contenedor detenido con `-Detener`; spec en «implementada».
- **Commit:** `docs(u6): cierre de TEC-05, calidad y CI (T6)`
