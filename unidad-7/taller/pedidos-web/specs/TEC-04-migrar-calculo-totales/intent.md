# Intent: TEC-04 · Migrar el cálculo de totales a C#

> Autor: Claudio Ortiz Castillo (PO) · Fecha: 2026-11-11 · Estado: aprobado (Claudio Ortiz Castillo, 2026-11-11)

> Formato: `specs/plantillas/intent-plantilla.md`. Trabajo técnico (prefijo `TEC`): el vendedor no ve ningún cambio.
> Es el taller de la Unidad 5: «Migrar el cálculo de totales sin perder un peso» (80 minutos).

## Qué queremos

Llevar `legado/src/UCalculoTotales.pas` a C# en `src/PedidosWeb.Core/Totales/`, de modo que para los 60 pedidos del golden master
(`migracion/golden-master.csv`) el C# calcule **exactamente lo mismo que el legado**, peso a peso. Después, corregir un único
defecto (R05, el redondeo) en un commit aparte, y dejar escritos el plan de migración y el registro de defectos.

## Por qué lo queremos

El diagnóstico de la U4 recomendó modernizar primero el cálculo de totales: es el módulo más aislado y el que más dinero mueve.
Migrar sin una red es cambiar el total de un pedido sin darse cuenta. El golden master es esa red: congela lo que hace hoy el
legado, **defectos incluidos**, y obliga a que cada diferencia sea una decisión registrada y no un accidente.

## Para quién

El equipo de desarrollo (el C# y los tests) y contabilidad (el registro de lo que cambia y de lo que queda por decidir).
El vendedor no ve nada nuevo.

## Reglas que ya conocemos

- **Igual al legado primero, corregir después.** Primero 60 de 60 en verde con los defectos incluidos; la corrección va en un commit aparte.
- **El golden master no se edita para que un test pase.** Solo cambia en la etapa 3, y cada caso que cambia queda registrado.
- **Dinero en `decimal`.** El legado usa `Double`; el C# no (regla de estilo de `AGENTS.md`).
- **`legado/` es solo lectura** (regla `deny` y regla 10 de `AGENTS.md`).
- **Lo que no está en la spec se pregunta** (regla 11): por ejemplo, qué versión de los umbrales replica el C#.

## Restricciones

- **Plazo:** 80 minutos (preparación 10, congelar 20, migrar 25, corregir y planificar 20, cierre 5).
- **Herramientas:** Claude Code, subagente `analista-legado` (solo lectura) y un hook `PostToolUse` con `dotnet test`.
- **Sin paquetes nuevos:** el lector del CSV se escribe a mano (regla 8). No se toca `PedidosWeb.sln` (las carpetas se crean y el SDK las incluye).
- **Plan B:** `unidad-5/taller/u5-respaldo/` (lector del golden master y `CalculoTotales` vacío) si la etapa 1 no queda a tiempo.

## Cómo sabremos que funcionó

60 de 60 tests en verde leyendo el CSV, la columna `total` sumando 58.202.135 antes de corregir y 58.202.144 después, R05 en su
propio commit y los dos documentos de `docs/migracion/` completos.

## Lo que NO es

- No es conectar el C# a la API ni al portal: solo la biblioteca de cálculo y sus tests.
- No es corregir los umbrales de la pantalla frente al grabado (hallazgo H1): se registra y decide contabilidad.
- No es migrar otro módulo, ni cargar datos en PostgreSQL (extensión opcional fuera del entregable).
