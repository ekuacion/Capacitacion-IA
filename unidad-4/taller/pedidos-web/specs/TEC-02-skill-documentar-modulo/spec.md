# Spec: TEC-02 · Skill `documentar-modulo`

> Versión: 1.0 · Estado: aprobada (Claudio Ortiz Castillo, 2026-11-10) · Intent de origen: `specs/TEC-02-skill-documentar-modulo/intent.md`
> Autor del borrador: Claude Code · Aprobada por: Claudio Ortiz Castillo (regla 1 de `AGENTS.md`)

> Formato: adaptado de `docs/plantilla-spec-breve.md`. Aquí la spec dice **qué debe cumplir** la Skill y el documento
> que produce. Las decisiones marcadas con **[Decidir]** son tuyas; la descripción de la Skill es, además, la clave.

## Qué se entrega

1. **La Skill:** `.claude/skills/documentar-modulo/SKILL.md`, con tres partes: nombre, descripción y pasos.
2. **El documento:** `docs/modulos/ClientesService.md`, generado con la Skill.

## Reglas de la Skill

- **RS-01 · Nombre.** `documentar-modulo`, igual que la carpeta.
- **RS-02 · Descripción.** Una frase que dice **qué hace** y **cuándo usarla**, con palabras de un pedido real
  («documenta», «servicio», «módulo», «para el equipo») y que cubre el caso en que **no** nombren la Skill. Si tiene dos
  puntos (`:`), va entre comillas.
- **RS-03 · Pasos.** Indica a Claude que lea primero el código y los tests del módulo, que escriba solo lo que el
  código hace y que revise cada ejemplo contra el código antes de terminar.
- **RS-04 · Sin inventar.** Si algo no está en el código ni en los tests, no se escribe; si hay una duda, se pregunta.
- **RS-05 · Dónde escribe.** En `docs/modulos/<Modulo>.md`, con el nombre de la clase principal del módulo.

## Qué debe tener `docs/modulos/<Modulo>.md`

- **RD-01 · Propósito.** Para qué existe el módulo, en pocas líneas.
- **RD-02 · API pública.** Cada método público con su firma, qué recibe, qué devuelve y los casos límite (id que no
  existe, `null`, etc.). Lo privado no se documenta como API.
- **RD-03 · Ejemplo de uso.** Código C# corto y real, con el resultado que de verdad produce.
- **RD-04 · Diagrama Mermaid.** Un bloque ```` ```mermaid ````, válido, que muestre cómo funciona el módulo.
- **RD-05 · Comportamientos a tener en cuenta.** Las rarezas conocidas (por ejemplo, que `RM` y `V` comparten zona o
  que la región y el id se comparan de forma exacta), escritas como notas y sin corregirlas.

> **Decidido por Claudio:** RD-05 se incluye, aunque no está en la lista del taller (propósito, API pública, ejemplo,
> diagrama): las rarezas ya están identificadas en TEC-01 y es justo lo que alguien nuevo necesita saber.

## Criterios de aceptación

- **CA-01 · Se activa sola.** Dado el pedido «Documenta el servicio de clientes para el equipo», cuando se envía sin
  nombrar la Skill, entonces Claude usa `documentar-modulo`.
- **CA-02 · Estructura completa.** Dado `docs/modulos/ClientesService.md`, entonces tiene propósito, API pública,
  ejemplo de uso y un diagrama Mermaid válido.
- **CA-03 · Coincide con el código.** Dado cada método y cada ejemplo del documento, entonces coincide con
  `ClientesService.cs` y con lo que verifican sus tests: firmas, textos de salida y casos límite.
- **CA-04 · Sin cambios de código.** Dado el trabajo terminado, entonces `src/` y `tests/` no cambiaron y
  `dotnet test` sigue en 83 en verde.

## Decisiones abiertas

- **Decidido por Claudio:** la **descripción** (RS-02) es la propuesta del plan, tal cual. Si en la prueba de
  activación (T2) no funciona, se ajusta con las palabras del pedido real.
- **Decidido por Claudio:** el **diagrama Mermaid** (RD-04) es un `flowchart` del método `ObtenerFicha`, porque la
  estructura es una sola clase y lo que vale explicar es la decisión de zona y estado.

## Fuera de alcance

- Documentar otros módulos (`Lineas`, `Pedidos`, `Folios`).
- Cambiar el código o los tests para que «documenten mejor».
- Publicar la documentación fuera del repositorio.

## Historial

| Versión | Fecha | Cambio | Quién |
|---|---|---|---|
| 0.1 | 2026-11-10 | Borrador inicial a partir del intent y de la guía del taller 3 | Claude Code |
| 1.0 | 2026-11-10 | Spec aprobada; se aceptan la descripción propuesta, RD-05 y el `flowchart` | Claudio Ortiz Castillo |
