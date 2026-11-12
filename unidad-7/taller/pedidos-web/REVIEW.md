# REVIEW.md · Reglas de revisión de Pedidos Web

Estas reglas las leen el subagente `revisor` y el workflow `claude-review.yml`. Se leen desde la rama base, no desde el PR. Responde en español de Chile. Complementa a `AGENTS.md`.

## Qué es Importante

Reporta como **Importante** solo lo que puede causar un error real o romper una regla del proyecto:

- Un bug de lógica: límites mal tratados (`>` en vez de `>=`), rangos, nulos, división o redondeo.
- Dinero con `double` o `float` (debe ser `decimal`) o fechas con hora donde corresponde `DateOnly`.
- Una regla de negocio que no está escrita en la spec (regla 11 de `AGENTS.md`).
- Un secreto, clave o cadena de conexión en el código o en un archivo.
- Un cambio en `legado/` (solo lectura) o un paquete NuGet nuevo sin aprobación.
- Un test que no prueba lo que dice su nombre o su criterio de aceptación.

## Qué es un Nit

Nombres poco claros, comentarios, orden de código y formato que no cambian el comportamiento. Un Nit nunca bloquea.

## Tope de detalles

Reporta como máximo **5 Nits** por revisión. Si hay más, di cuántos quedaron fuera. Los Importantes no tienen tope.

## No reportar

- Lo que ya cubren el compilador y el CI (formato, warnings de build).
- `bin/`, `obj/` y archivos generados.
- `legado/` y `migracion/golden-master.csv`: solo se leen.
- Preferencias de estilo que no están en `AGENTS.md`.

## Revisar siempre

1. **Spec y tests:** cada criterio de aceptación de la spec tiene su test, y si hay un rango, el test prueba ambos límites y un valor fuera.
2. **Entradas y tipos:** dinero en `decimal`, fechas en `DateOnly`, y las entradas inválidas (nulo, `desde` mayor que `hasta`) fallan con un error claro.
3. **Seguridad y alcance:** sin secretos, sin paquetes NuGet nuevos, `legado/` intacto y nada fuera del alcance de la spec.

## Formato de la respuesta

Agrupa en **Importante**, **Nit** y **Preguntas**. Cada hallazgo cita `archivo:línea` y dice qué cambiar. Si no hay nada Importante, dilo.
