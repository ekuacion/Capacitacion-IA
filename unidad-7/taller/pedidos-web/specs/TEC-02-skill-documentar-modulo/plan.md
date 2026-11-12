# Plan: TEC-02 · Skill `documentar-modulo`

> Spec de origen: `specs/TEC-02-skill-documentar-modulo/spec.md` v1.0 · Autor: Claude Code · Revisado por: Claudio Ortiz Castillo
> Estado: aprobado (Claudio Ortiz Castillo, 2026-11-10), con la descripción propuesta, RD-05 y el `flowchart`

> Formato: `specs/plantillas/plan-plantilla.md`. Responde **cómo** se construye y se prueba la Skill; no repite la spec.
> Las correcciones del dev se dejan visibles con el formato `> **[Revisión del dev]** qué está mal, por qué y qué hacer en su lugar.`
> No se escribe la Skill hasta que la spec esté «aprobada» (v1.0) y este plan «aprobado» (regla 1 de `AGENTS.md`).

## Enfoque

Se construye en tres pasos chicos, cada uno con su commit: primero la **Skill** a partir de la plantilla, después la
**prueba de que se activa sola** (y el ajuste de su descripción si hace falta) y al final el **documento** de
`ClientesService`, revisado contra el código. La parte difícil no es escribir el `SKILL.md`, sino que la descripción
haga que Claude lo use sin que nadie lo nombre: por eso se prueba antes de generar el documento final.

## Decisiones técnicas

- **Ubicación: `.claude/skills/documentar-modulo/SKILL.md` dentro de `pedidos-web`**, que es donde se abre Claude Code
  en este taller. Descartado: una Skill global del usuario, porque no viajaría con el repositorio y el equipo no la tendría.
- **Descripción (aprobada por Claudio; se ajusta solo si la prueba de activación del T2 lo exige):**

  > `"Documenta un módulo de Pedidos Web (una clase o servicio de PedidosWeb.Core) en docs/modulos/<Modulo>.md, con propósito, API pública, ejemplo de uso y diagrama Mermaid, leyendo el código y los tests. Úsala cuando pidan documentar, explicar o dejar escrito cómo funciona un servicio o un módulo para el equipo, aunque no nombren la Skill."`

  Por qué así: dice **qué hace** (documenta un módulo en un archivo con cuatro partes) y **cuándo** (documentar o
  explicar un servicio o módulo para el equipo), y trae las palabras de un pedido real (`documenta`, `servicio`,
  `módulo`, `para el equipo`). Va entre comillas porque tiene dos puntos. Descartado: una descripción corta como
  «Documenta módulos», porque Claude no sabría cuándo aplicarla.
- **Pasos de la Skill, en este orden:** (1) identificar la clase principal del módulo; (2) leer el código y los tests
  de esa clase; (3) escribir `docs/modulos/<Modulo>.md` con las partes de la spec; (4) comprobar cada ejemplo y cada
  firma contra el código; (5) no tocar el código ni los tests.
- **Regla de la Skill: «no inventar».** Si algo no está en el código ni en los tests, se pregunta o se omite.
  Descartado: dejar que Claude complete «lo razonable», que es justo lo que hace que cada documento salga distinto.
- **Diagrama Mermaid (aprobado):** un `flowchart` de `ObtenerFicha` (existe o no, zona según región, estado).
  Descartado: `classDiagram`, porque con una sola clase aporta poco.
- **Los ejemplos del documento salen de los tests y de los datos de ejemplo**, por ejemplo `C-001` →
  `Ferretería El Clavo | 76123456-0 | Zona centro | Activo`. Así son comprobables y no se inventan.
- **Sin paquetes ni código nuevo.** Solo archivos Markdown.

## Archivos afectados

| Archivo | Nuevo / Modificado | Tarea | Para qué |
|---|---|---|---|
| `.claude/skills/documentar-modulo/SKILL.md` | Nuevo | T1 (ajuste en T2) | La Skill: nombre, descripción y pasos |
| `docs/modulos/ClientesService.md` | Nuevo | T3 | La documentación generada y revisada |
| `docs/README.md` | Sin cambios | — | Ya lista `modulos/` y `plantilla-skill.md` |

No se tocan: `src/`, `tests/`, `Program.cs` ni los documentos de TEC-01 y HU-02.

## Riesgos

| Riesgo | Mitigación |
|---|---|
| La Skill no se activa sola | T2 prueba el pedido real sin nombrar la Skill; si falla, se mejora la descripción con palabras de ese pedido. Mientras tanto se puede invocar por su nombre. |
| Una Skill creada en medio de una sesión puede no aparecer hasta abrir una sesión nueva | T2 se prueba en una sesión nueva; es un punto a **verificar**, no una certeza. |
| El documento dice cosas que el código no hace | CA-03 y la regla «no inventar»; en T3 se compara cada ejemplo con los tests y con una ejecución real. |
| El diagrama Mermaid tiene un error de sintaxis y no se dibuja | Se revisa el bloque en la vista previa del editor antes del commit. |
| El documento «corrige» o esconde las rarezas de `ClientesService` | RD-05 las pide como notas y la regla de la Skill dice no corregir. |
| Claude modifica código al «documentar mejor» | La spec (CA-04) y la Skill lo prohíben; se verifica con `git diff --stat` de `src/` y `tests/`. |

## Pasos

1. Aprobar la spec (v1.0) y este plan. → «aprobada» y «aprobado».
2. T1, escribir la Skill desde `docs/plantilla-skill.md`. → el archivo existe y su cabecera es válida.
3. T2, probar que se activa sola, sin nombrarla. → Claude la usa con «Documenta el servicio de clientes para el equipo».
4. T3, generar y revisar `docs/modulos/ClientesService.md`. → cada ejemplo y firma coincide con el código.

El detalle de cada paso está en `tasks.md`.

## Fuera del plan

- Documentar otros módulos y cambiar código o tests.
- Documentación de usuario final o de la API HTTP.
- Publicar la Skill fuera del repositorio.
