# Intent: TEC-03 · Diagnóstico de Pedidos Legado

> Autor: Claudio Ortiz Castillo (PO) · Fecha: 2026-11-11 · Estado: aprobado (2026-11-11)

> Formato: `specs/plantillas/intent-plantilla.md`. Trabajo técnico (prefijo `TEC`): no cambia lo que ve el vendedor ni toca código.
> Es el taller de la Unidad 4: «Diagnosticar antes de modernizar». Se hace en 60 minutos (8:00 a 9:00 del 2026-11-12).

## Qué queremos

Un diagnóstico escrito de Pedidos Legado, el sistema Delphi de hace 18 años que va a modernizarse, con tres respuestas:
**qué hay** (inventario y mapa de dependencias), **qué es riesgoso** (reglas de negocio ocultas y riesgos, cada uno con su
evidencia) y **por dónde empezar** (una recomendación que salga de una matriz de decisión, con un resumen para la gerencia).

## Por qué lo queremos

La gerencia no aprobará presupuesto para modernizar un sistema que nadie ha mapeado. Hoy las reglas del negocio (descuentos,
topes, impuestos) viven escondidas dentro del código Delphi, y los comentarios y el README no son confiables: por ejemplo,
`legado/README.md` dice «Delphi 5 + Paradox/BDE» mientras que el README del proyecto dice «Delphi 7 + DBISAM 4». Migrar sin
este diagnóstico es el camino más corto a perder reglas por el camino y a cambiar el total de un pedido sin darse cuenta.
La Unidad 5 migrará el cálculo de totales; este diagnóstico decide si esa es la mejor primera pieza.

## Para quién

La gerencia (el resumen de 5 líneas, sin jerga) y el equipo de desarrollo que hará la migración (inventario, reglas, riesgos).
No hay cambio visible para el vendedor.

## Reglas que ya conocemos

- **El legado se lee, no se toca:** `legado/` es solo lectura, y una regla `deny` en `.claude/settings.json` lo garantiza.
- **Sin evidencia no hay hallazgo:** toda regla y todo riesgo cita `archivo:línea`; lo dudoso se marca «por verificar».
- **No fiarse de los comentarios:** el código manda; un comentario puede estar desactualizado o mentir.
- **Las personas verifican:** de las reglas que encuentre la IA, Claudio revisa al menos 5 abriendo el código en la línea citada;
  lo que no se ubique, sale del catálogo.
- **Decide una persona:** Claude propone pesos y puntajes y los explica; la recomendación final la decide Claudio.

## Restricciones

- **Plazo:** 60 minutos, entre las 8:00 y las 9:00 del 2026-11-12 (el curso empieza a las 9:00). Hoy se deja lista toda la
  preparación (specs, regla `deny` y subagente) para que mañana solo se ejecute.
- **Herramientas:** Claude Code (plan mode en la etapa 1, subagente `analista-legado` con solo Read, Grep y Glob en la etapa 2).
- **Solo documentos:** se entregan 5 archivos en `docs/diagnostico/`; `src/` y `tests/` no se tocan.
- **Plan B:** si la etapa 1 no queda a tiempo, se usa `unidad-4/taller/u4-respaldo/` (inventario y dependencias de referencia).

## Cómo sabremos que funcionó

Al cierre existen los 5 archivos de `docs/diagnostico/` completos, `legado/` no tiene ningún cambio en `git status`, y la
recomendación se puede explicar a la gerencia en 5 líneas apuntando a la matriz.

## Lo que NO es

- No es migrar ni arreglar nada: ni una línea de `legado/` ni de `src/` cambia.
- No es el plan de migración completo (eso viene después) ni la migración del cálculo de totales (Unidad 5).
- No es ejecutar el sistema Delphi: no se instala ni se compila; solo se lee.
