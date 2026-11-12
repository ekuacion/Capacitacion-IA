# Intent: TEC-06 · Code review y colaboración

> Autor: Claudio Ortiz Castillo (PO) · Fecha: 2026-11-12 · Estado: **borrador** (pendiente de aprobación)

> Formato: `specs/plantillas/intent-plantilla.md`. Trabajo técnico (prefijo `TEC`): el vendedor no ve ningún cambio, salvo la HU-03 como caso de práctica.
> Es el taller de la Unidad 7: «Que cada cambio llegue a la rama base revisado con reglas escritas, por la IA y por una persona» (94 minutos).

## Qué queremos

Que todo PR de `pedidos-web` sea revisado con las mismas reglas escritas, primero por Claude (subagente local y workflow en el PR) y después por una persona que decide con argumentos. El PR de práctica es la HU-03 (pedidos por rango de fechas).

## Por qué lo queremos

El CI de la U6 prueba cada cambio, pero nadie dice qué se revisa ni con qué criterio. Sin reglas escritas, la IA comenta de todo y la persona aprueba con «ok».

## Para quién

El equipo de desarrollo y quien revisa PR. Punto de partida de la Unidad 8.

## Reglas que ya conocemos

- **Las reglas se leen desde la rama base**, no desde el PR: así un PR no puede aflojar las reglas que lo revisan.
- **La IA comenta; la persona decide.** Cada hallazgo se responde con «Acepto», «Discutir» o «Descarto», con argumento propio. «ok» en todos los hilos no es revisar.
- **El revisor es de solo lectura** (no edita archivos).
- **El token nunca va en un archivo.** Se usa el secreto `CLAUDE_CODE_OAUTH_TOKEN`; los pasos para crearlo los hace Claudio.
- **El PR de la HU-03 es chico a propósito** y su descripción (Qué cambia, Por qué, Cómo probarlo) la completa una persona: el «Por qué» es de Claudio.
- **La revisión automática gasta cuota:** corre al abrir, reabrir o marcar listo el PR, no en cada push.
- **Lo que no se verificó se anota como no verificado**; ningún criterio se da por cumplido sin prueba.

## Restricciones

- **Plazo:** 94 minutos (preparación 9, reglas 25, PR 25, revisión cruzada 20, reto 15).
- **Sin paquetes NuGet nuevos** (regla 8). Sin instalar software (`gh` no está instalado: el PR se abre desde la web).
- **Commit, push, PR y merge los hace Claudio.** Claude deja los cambios en stage y propone el mensaje.
- Base de los PR: `develop` (decisión de Claudio). `main` se promueve aparte.
- Repo privado `ekuacion/Capacitacion-IA` (la guía asume público). La etapa 3 en pareja necesita que la pareja tenga acceso al repo.

## Cómo sabremos que funcionó

Un PR de HU-03 con comentarios de Claude, cada uno respondido con argumento, un hallazgo humano que no es de estilo, el checklist completo y el merge con el CI en verde. Todo PR nuevo nace con la plantilla.

## Lo que NO es

- No cambia la lógica de negocio ni los pedidos de ejemplo.
- No expone HU-03 en la API.
- No cierra los pendientes de la U6 (mutación de los 7 tests, compuerta roja del CI local, falla con `@claude`): quedan anotados, no ejecutados.
