# Registro de calidad · Unidad 7

> Taller hecho por Claudio Ortiz Castillo con Claude Code (spec TEC-06 v1.1). Fecha: 2026-11-13. Lo que no se verificó o no se hizo queda dicho como tal. Sin pareja: la revisión cruzada fue sobre mi propio PR y **no la verificó otra persona**.

## Estado por criterio

| CA | Estado | Evidencia o motivo |
|---|---|---|
| CA-01 Base sembrada | **Cumplido** | T0 hecho. Hoy `dotnet test`: 158 superados (153 Core + 5 API), 0 omitidos. |
| CA-02 CI en unidad-7 | **Cumplido** | El CI de unidad-7 pasó en el PR #1 (3 checks), según reporte del PO. |
| CA-03 REVIEW.md | **Cumplido** | Tiene Importante, No reportar, tope de 5 Nits y 3 chequeos de «Revisar siempre». |
| CA-04 Revisor solo lectura | **Parcial** | Verificado por lectura: `tools: Read, Grep, Glob`. **No verificado:** que cargue al reiniciar Claude Code; la revisión local de HU-03 la hizo un agente de solo lectura equivalente. **Desviación de RN-02:** el revisor lee los archivos de la carpeta local, no de la rama base. |
| CA-05 Workflow seguro | **Cumplido por lectura** | `opened`/`reopened`/`ready_for_review`, `contents: read`, timeout 10 min, versiones fijas, solo aparece el nombre del secreto. **Sin verificar:** el SHA fijado de `claude-code-action` (los `uses` van por tag; el SHA está solo en un comentario). `id-token: write` y `pull-requests: write` superan «solo lectura»; el primero lo usó la acción (OIDC). |
| CA-06 Reglas en `main` antes del PR | **No cumplido (PR #1)** | `REVIEW.md` y `claude-review.yml` no estaban en `main` al abrirlo. |
| CA-07 HU-03 con tests | **Cumplido** | 8 tests de `FiltroPedidos` y suite en verde. **No verificado por mutación.** |
| CA-08 Descripción de PR | **No cumplido (PR #1)** | Descripción igual al mensaje de commit; sin revisor local previo. |
| CA-09 Claude comenta | **No cumplido** | PR #1: 0 comentarios. PR #2: el primer run falló por secreto vacío; el run #3 salió en verde en 40 s y dejó 0 comentarios. **Causa no determinada:** no se vio el log del paso `claude-code-action`. |
| CA-10 Revisión cruzada | **No cumplido** | No hubo comentarios de la IA que responder; sin pareja. `docs/checklist-revision.md` queda **sin completar**. |
| CA-11 Plantilla de PR | **No verificado** | El archivo está en la rama (commit `0f54f1a`). Falta verlo en un PR nuevo, después del merge a `main`. |
| CA-12 Registro | **Cumplido** | Este archivo. |

## Hallazgo humano (no estilístico)

El CI no corrió en el PR #2: `ci.yml` filtra por `paths` (`unidad-7/taller/pedidos-web/**` y `.github/workflows/ci.yml`) y ese PR solo toca `.github/pull_request_template.md`. **Confirmado:** el PR #2 mostró un solo check (Claude Review) y el botón de merge quedó habilitado. Consecuencia: RN-06 («merge con CI en verde») no se puede cumplir en PR que solo cambien `.github/`, y un ruleset que exija el check de CI bloquearía esos PR. **Pendiente de decidir:** añadir `.github/**` a los `paths` en un PR propio (opción recomendada).

## Otros puntos

- **Secreto `CLAUDE_CODE_OAUTH_TOKEN`:** el run #3 ya no falló por el token, así que llegó al workflow. Su valor se pegó en el chat de la sesión: **debe revocarse y regenerarse** (pendiente del PO; no verificado).
- **Orden de tareas:** T6 se hizo antes que T5, por el plan acordado tras el PR #1.
- **PR #2 (plantilla):** abierto, sin mergear al cierre de este registro.
- **Recomendación:** ruleset en `main` que exija PR y CI (lo configura el PO).

## Pendientes de la U6 (sin cambios)

- CA-02: mutación de los 7 tests de `CalculoTotalesBordeTests`.
- CA-05: compuerta de `scripts/ci-local.ps1` con un test en rojo.
- CA-07: falla con `@claude`.

Detalle en `docs/calidad/registro-u6.md`.

## Pendiente para completarlo

1. Ver el log del paso `claude-code-action` y determinar por qué Claude no comentó (CA-09).
2. Revocar el token expuesto y regenerarlo.
3. Decidir el cambio de `paths` en `ci.yml`.
4. Mergear el PR #2 y abrir un PR nuevo para verificar la plantilla (CA-11).
5. Responder los comentarios de la IA y completar `docs/checklist-revision.md` (CA-10).
6. Reiniciar Claude Code y comprobar que el subagente `revisor` carga (CA-04).
