# Intent: TEC-02 · Skill `documentar-modulo`

> Autor: Claudio Ortiz Castillo (PO) · Fecha: 2026-11-10 · Estado: borrador para revisión

> Formato: `specs/plantillas/intent-plantilla.md`. Trabajo técnico (prefijo `TEC`): no cambia lo que ve el vendedor.
> Es la etapa 3 del taller («Explicar»): enseñarle a Claude a documentar un módulo como lo hace el equipo.

## Qué queremos

Una **Skill** del proyecto, `documentar-modulo`, que haga que Claude escriba la documentación de un módulo siempre con
la misma estructura, leyendo el código y los tests, y que la use sola cuando alguien del equipo pida documentar un módulo.
Con ella, dejar escrita la documentación de `ClientesService` en `docs/modulos/ClientesService.md`.

## Por qué lo queremos

`ClientesService` acaba de ordenarse (TEC-01) y hoy no tiene documentación: quien llega nuevo al equipo tiene que leer
el código para saber qué hace. Si cada persona le pide a Claude «documenta esto» a su manera, cada documento sale
distinto: uno con diagrama, otro sin ejemplo, otro con cosas inventadas. Una Skill fija la forma una vez y se reutiliza
para los próximos módulos (la migración del cálculo de totales, en la Unidad 5, será el siguiente).

## Para quién

El equipo de desarrollo que mantiene Pedidos Web, y las personas que se sumen. No hay cambio visible para el vendedor.

## Reglas que ya conocemos

Tal como lo plantea el taller: una Skill es una carpeta con un `SKILL.md`, que Claude usa **sola** cuando el pedido
calza con su **descripción**; por eso la descripción es la parte más importante. Cada documento debe tener: propósito,
API pública, ejemplo de uso y un diagrama Mermaid.

## Restricciones

- La Skill vive en `.claude/skills/documentar-modulo/SKILL.md` del proyecto.
- Se escribe a partir de `docs/plantilla-skill.md`; la descripción la redacta Claudio.
- Lo documentado sale **del código y de los tests**, no de lo que Claude «supone»: nada inventado.
- La documentación va en `docs/modulos/<Modulo>.md`.
- No se modifica código ni tests: esta etapa solo agrega documentación y la Skill.
- Se hace dentro del taller de la Unidad 3 (etapa 3, 20 minutos).

## Cómo sabremos que funcionó

- Al pedir «Documenta el servicio de clientes para el equipo», **sin nombrar la Skill**, Claude la usa.
- `docs/modulos/ClientesService.md` coincide con el código: cada ejemplo se puede comprobar.
- Dos personas del equipo podrían explicar en una frase para qué sirve la Skill leyendo su descripción.

## Lo que NO es

- No es documentación de usuario final ni de la API HTTP.
- No documenta otros módulos todavía (solo `ClientesService`); la Skill queda lista para ellos.
- No corrige comportamientos raros que descubra al documentar: los anota como notas, no los arregla.
- No reemplaza la spec ni el plan de cada historia; documenta lo que ya existe.
