# Tasks: TEC-06 · Code review y colaboración

> Plan de origen: `specs/TEC-06-code-review-colaboracion/plan.md` · Spec: `specs/TEC-06-code-review-colaboracion/spec.md` v0.1
> Regla común: cada tarea se explica antes de hacerla, `dotnet test` en verde antes de dejar algo en stage, y Claudio hace commit, push, PR y merge.

## Resumen

| ID | Tarea | CA cubiertos | Estado |
|---|---|---|---|
| T0 | Aprobar spec y sembrar unidad-7 desde U6 | CA-01 | hecho |
| T1 | Apuntar `ci.yml` y escribir `REVIEW.md` y `revisor.md` | CA-02, CA-03, CA-04 | hecho |
| T2 | Escribir `claude-review.yml` y dejar las reglas en `main` | CA-05, CA-06 | hecho |
| T3 | Implementar HU-03 con tests (rama propia) | CA-07 | pendiente |
| T4 | Descripción del PR, revisor local y abrir PR | CA-08, CA-09 | pendiente |
| T5 | Revisión cruzada y checklist | CA-10 | pendiente |
| T6 | Plantilla de PR | CA-11 | pendiente |
| T7 | Registro de cierre | CA-12 | pendiente |

---

## T0 — Aprobar spec y sembrar la base

- **CA cubiertos:** CA-01
- **Depende de:** ninguna
- **Archivos:** `unidad-7/taller/pedidos-web/**`
- **Qué hacer:**
  - Claudio aprueba la spec (estado → aprobada, v1.0).
  - Mostrar la lista exacta de copia desde `unidad-6/taller/pedidos-web` (sin `bin/`, `obj/` ni `.agents/`; conservando `README.md`, `specs/HU-03-...` y `docs/checklist-revision.md` de U7) y copiar solo tras el visto bueno.
  - `legado/` no se copia: solo cambia el fin de línea.
- **Verificación:** `dotnet test` en `unidad-7/taller/pedidos-web` → 150 superados, 0 omitidos.
- **Hecho cuando:** la base está en verde y en stage con rutas explícitas.

## T1 — `ci.yml`, `REVIEW.md` y `revisor.md`

- **CA cubiertos:** CA-02, CA-03, CA-04
- **Depende de:** T0
- **Archivos:** `.github/workflows/ci.yml`, `unidad-7/.../REVIEW.md`, `unidad-7/.../.claude/agents/revisor.md`, `unidad-7/.../AGENTS.md` (regla 9)
- **Qué hacer:**
  - Cambiar `working-directory` a unidad-7.
  - Redactar `REVIEW.md` (Importante, No reportar, tope, 3 chequeos propuestos para que Claudio apruebe).
  - Crear el subagente de solo lectura que lee `AGENTS.md` y `REVIEW.md` desde la rama base.
- **Verificación:** `grep -n "Edit\|Write" .claude/agents/revisor.md` → sin herramientas de escritura; reiniciar Claude Code y «Usa el subagente revisor sobre esta rama» → responde.
- **Hecho cuando:** Claudio aprobó cada archivo antes de guardarlo.

## T2 — `claude-review.yml` y reglas en `main`

- **CA cubiertos:** CA-05, CA-06
- **Depende de:** T1
- **Archivos:** `.github/workflows/claude-review.yml`
- **Qué hacer:**
  - Escribir el workflow (opened/reopened/ready_for_review, permisos mínimos, timeout, versiones fijas, secreto por nombre) comparando con la documentación.
  - Pasos del secreto: los hace Claudio en Settings › Secrets and variables › Actions.
  - Claudio integra la rama a `main`.
- **Verificación:** `grep -n "token\|TOKEN" .github/workflows/claude-review.yml` → solo el nombre del secreto; CI del merge en verde.
- **Hecho cuando:** `REVIEW.md` y `claude-review.yml` están en `main` con CI en verde.

## T3 — Implementar HU-03

- **CA cubiertos:** CA-07
- **Depende de:** T2
- **Archivos:** `unidad-7/.../src/PedidosWeb.Core/Pedidos/FiltroPedidos.cs`, `unidad-7/.../tests/PedidosWeb.Core.Tests/Pedidos/FiltroPedidosTests.cs`
- **Qué hacer:**
  - Rama `hu-03-pedidos-por-fecha` desde `main`.
  - Un test por criterio (HU-03/CA-01 a CA-04, nulo y orden de entrada).
- **Verificación:** `dotnet test --filter "FullyQualifiedName~FiltroPedidosTests"` → todos en verde; `dotnet test` completo en verde.
- **Hecho cuando:** los tests prueban ambas fechas límite y el caso `desde > hasta`.

## T4 — PR de HU-03

- **CA cubiertos:** CA-08, CA-09
- **Depende de:** T3
- **Archivos:** ninguno (descripción del PR)
- **Qué hacer:**
  - Correr el revisor local y mostrar sus hallazgos.
  - Redactar la descripción con `[COMPLETAR]` donde falte; Claudio escribe el «Por qué».
  - Claudio abre el PR (sin `gh`).
- **Verificación:** la descripción no contiene `[COMPLETAR]`; en el PR aparece un comentario de Claude (si no, se anota).
- **Hecho cuando:** PR abierto con CI corriendo.

## T5 — Revisión cruzada

- **CA cubiertos:** CA-10
- **Depende de:** T4
- **Archivos:** `unidad-7/.../docs/checklist-revision.md`
- **Qué hacer:**
  - Leer los comentarios del PR y proponer respuesta; Claudio decide cada una con su argumento.
  - Sin pareja: revisar el propio PR como segunda lectura y registrar 1 hallazgo humano no estilístico.
  - Completar el checklist.
- **Verificación:** cada hilo respondido; checklist sin marcadores `<…>`; CI en verde antes del merge.
- **Hecho cuando:** Claudio hizo el merge.

## T6 — Plantilla de PR

- **CA cubiertos:** CA-11
- **Depende de:** T5
- **Archivos:** `.github/pull_request_template.md`
- **Qué hacer:** Claudio redacta el pedido (Contexto, Qué quiero, Cómo sé que quedó bien); Claude lo implementa con la casilla del revisor.
- **Verificación:** un PR nuevo en GitHub muestra las secciones (lo ve Claudio).
- **Hecho cuando:** archivo en la rama base y plantilla vista en un PR nuevo.

## T7 — Registro de cierre

- **CA cubiertos:** CA-12
- **Depende de:** T6
- **Archivos:** `unidad-7/.../docs/calidad/registro-u7.md`, `specs/README.md`
- **Qué hacer:** dejar cada CA como cumplido, no cumplido o no verificado; copiar los pendientes de la U6 como pendientes.
- **Verificación:** ningún CA marcado cumplido sin evidencia en el registro.
- **Hecho cuando:** `dotnet test` en verde y todo en stage con mensaje de commit propuesto.
