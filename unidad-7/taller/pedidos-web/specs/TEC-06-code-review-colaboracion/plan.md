# Plan: TEC-06 · Code review y colaboración

> Spec de origen: `specs/TEC-06-code-review-colaboracion/spec.md` v0.1 · Autor: Claude Code · Revisado por: Claudio Ortiz Castillo
> Estado: propuesto

## Enfoque

Primero se siembra `unidad-7` desde U6 para tener una base en verde (CA-01). Luego se escriben las reglas (RN-01 a RN-03) y se integran a `develop` antes de abrir el PR (RN-04). La HU-03 (RN-07) se implementa en su propia rama con un test por criterio. El PR se abre a mano con la descripción completada por Claudio, y la revisión (IA y persona) deja evidencia en el checklist. La plantilla de PR (RN-08) y el registro de cierre terminan el taller.

## Decisiones técnicas

- **Sembrar desde U6** y no trabajar en unidad-6: decisión de Claudio. Descartado: copiar la carpeta completa de U7 (deja 3 tests en rojo y sin el trabajo de U1 a U6).
- **PR con base `develop`**, decisión de Claudio. Descartado: base `main` (sigue en `abc9635`, sin workflows).
- **Revisor sin herramientas de escritura.** Descartado: `Bash` libre.
- **`IReadOnlyList<Pedido>` en orden de entrada**, sin paquetes nuevos.
- **Claude Code GitHub Actions** con versión fija, como en `claude.yml` (`claude-code-action v1.0.271`); se compara con la documentación antes de guardar.

## Archivos afectados

| Archivo | Nuevo / Modificado | Para qué |
|---|---|---|
| `unidad-7/taller/pedidos-web/**` (siembra desde U6) | Nuevo/Modificado | Base en verde (T0); lista exacta en T0 |
| `.github/workflows/ci.yml` | Modificado | `working-directory` → unidad-7 (CA-02) |
| `unidad-7/.../REVIEW.md` | Nuevo | Reglas de revisión (CA-03) |
| `unidad-7/.../.claude/agents/revisor.md` | Nuevo | Revisor de solo lectura (CA-04) |
| `.github/workflows/claude-review.yml` | Nuevo | Revisión en cada PR (CA-05) |
| `unidad-7/.../src/PedidosWeb.Core/Pedidos/FiltroPedidos.cs` | Nuevo | HU-03 (CA-07) |
| `unidad-7/.../tests/PedidosWeb.Core.Tests/Pedidos/FiltroPedidosTests.cs` | Nuevo | Un test por criterio (CA-07) |
| `unidad-7/.../docs/checklist-revision.md` | Modificado | Checklist completo (CA-10) |
| `.github/pull_request_template.md` | Nuevo | Reto (CA-11) |
| `unidad-7/.../docs/calidad/registro-u7.md` | Nuevo | Registro honesto (CA-12) |
| `unidad-7/.../AGENTS.md`, `specs/README.md` | Modificado | Autorizar `.github/` y `REVIEW.md`; filas TEC-06 y HU-03 |

## Riesgos

| Riesgo | Mitigación |
|---|---|
| Secreto no creado o cuota agotada: el workflow no comenta | Claudio sigue los pasos de configuración; CA-09 queda como no cumplido si falla |
| El workflow puede exigir estar en la rama por defecto | Verificar en la documentación; anotar si no corre antes de promover a `main` |
| Repo privado: la pareja no ve el PR | Dar acceso o hacer la etapa 3 sobre tu PR y anotarlo |
| La siembra pisa archivos de U7 | Copiar con lista explícita; sin pisar `README.md`, HU-03, checklist ni `.agents/` |
| El CI de unidad-6 deja de correr al cambiar `ci.yml` | Se acepta (decisión); queda registrado |
| `.agents/` (PDFs del curso) sin versionar | No se agrega a stage |

## Pasos

1. Aprobar la spec y sembrar la base (T0). Comprobable: `dotnet test` con 150 en verde.
2. Reglas: `ci.yml`, `REVIEW.md`, `revisor.md`, `claude-review.yml` (T1 y T2). Comprobable: lectura de los archivos y CI en verde.
3. HU-03 en su rama, con tests (T3).
4. PR abierto con descripción y revisor local (T4).
5. Revisión cruzada y checklist (T5).
6. Plantilla de PR (T6) y registro (T7).

## Fuera del plan

Lo mismo que «Fuera de alcance» de la spec: API de HU-03, pendientes de la U6, instalar software y promover a `main`.
