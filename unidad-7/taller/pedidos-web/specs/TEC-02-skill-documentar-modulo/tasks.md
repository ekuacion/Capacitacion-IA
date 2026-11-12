# Tasks: TEC-02 · Skill `documentar-modulo`

> Plan de origen: `specs/TEC-02-skill-documentar-modulo/plan.md` (aprobado) · Spec: `specs/TEC-02-skill-documentar-modulo/spec.md` v1.0 (aprobada)

> Formato: `specs/plantillas/tasks-plantilla.md`. Una tarea = un cambio pequeño con su verificación y su commit.
> Aquí no hay código: todo son archivos Markdown. `src/` y `tests/` no se tocan en ninguna tarea.

## Resumen

| ID | Tarea | CA cubiertos | Estado |
|---|---|---|---|
| T0 | Aprobar la spec v1.0 y el plan | — | hecha |
| T1 | Escribir la Skill `documentar-modulo` | CA-02 (la base) | hecha |
| T2 | Probar que se activa sola | CA-01 | hecha (probada en la misma sesión, no en una nueva) |
| T3 | Generar y revisar `docs/modulos/ClientesService.md` | CA-02, CA-03, CA-04 | hecha |

---

## T0 — Aprobar la spec v1.0 y el plan

- **Depende de:** ninguna.
- **Archivos:** `specs/TEC-02-skill-documentar-modulo/spec.md`, `plan.md`.
- **Qué hacer:**
  - Claudio aprobó el texto de la **descripción** de la Skill (RS-02) tal como está en el plan.
  - Claudio decidió RD-05 (notas de comportamientos: sí) y el tipo de diagrama (`flowchart`).
  - Claudio dejó la spec en «aprobada» v1.0 y el plan en «aprobado».
- **Verificación:** `grep -n "Estado" specs/TEC-02-skill-documentar-modulo/spec.md specs/TEC-02-skill-documentar-modulo/plan.md` → «aprobada» y «aprobado».
- **Hecho cuando:** los dos documentos están aprobados por una persona.

## T1 — Escribir la Skill `documentar-modulo`

- **CA cubiertos:** base de CA-02 (la Skill define la estructura).
- **Depende de:** T0.
- **Archivos:** `.claude/skills/documentar-modulo/SKILL.md`.
- **No tocar:** `src/`, `tests/` y cualquier otro archivo.
- **Qué hacer:**
  - Crear las carpetas y copiar el bloque de `docs/plantilla-skill.md`, con `---` en la primera línea.
  - Completar `name`, la `description` (la aprobada en T0, entre comillas) y los pasos.
  - Incluir las partes del documento (propósito, API pública, ejemplo de uso, diagrama Mermaid y notas de comportamientos)
    y las reglas: no inventar, no tocar código ni tests, escribir en `docs/modulos/<Modulo>.md`.
- **Verificación:** el archivo empieza con `---`, tiene `name: documentar-modulo` y una `description` válida; el YAML se
  lee sin errores.
- **Hecho cuando:** la Skill existe en su carpeta y se lee completa en una pantalla.
- **Commit:** `feat(u3): skill documentar-modulo`

## T2 — Probar que se activa sola

- **CA cubiertos:** CA-01.
- **Depende de:** T1.
- **Archivos:** `.claude/skills/documentar-modulo/SKILL.md` (solo si hay que ajustar la descripción).
- **No tocar:** `src/` y `tests/`.
- **Qué hacer:**
  - En una **sesión nueva** abierta en `pedidos-web`, pedir exactamente: «Documenta el servicio de clientes para el equipo», **sin nombrar la Skill**.
  - Observar si Claude carga `documentar-modulo`.
  - Si no se activa, mejorar la descripción con las palabras de ese pedido y repetir; mientras tanto se puede invocar por su nombre.
- **Verificación:** Claude usa la Skill ante el pedido sin nombrarla; se anota qué palabras de la descripción lo permitieron.
- **Hecho cuando:** la Skill se activó sola (CA-01).
- **Commit:** `fix(u3): ajusta la descripción de documentar-modulo` (solo si hubo ajuste).

## T3 — Generar y revisar `docs/modulos/ClientesService.md`

- **CA cubiertos:** CA-02, CA-03, CA-04.
- **Depende de:** T2.
- **Archivos:** `docs/modulos/ClientesService.md`.
- **No tocar:** `src/` y `tests/`.
- **Qué hacer:**
  - Generar el documento con la Skill y revisarlo contra `ClientesService.cs` y sus 14 tests.
  - Comprobar que tiene propósito, API pública (`ObtenerFicha` y `ExisteYEstaActivo`, con casos límite), un ejemplo de
    uso con salida real, el diagrama Mermaid y las notas de comportamientos.
  - Comprobar cada ejemplo con los datos de ejemplo (por ejemplo `C-001` y `C-007`) y que el diagrama se dibuje.
- **Verificación:**
  - Cada firma y cada texto de salida del documento coincide con el código y los tests.
  - `git diff --stat -- src tests` no muestra cambios; `dotnet test` → 83 en verde.
- **Hecho cuando:** el documento coincide con el código y no hay nada inventado.
- **Commit:** `docs(u3): documenta ClientesService`
