# Intent: TEC-01 · Ordenar `ClientesService`

> Autor: Claudio Ortiz Castillo (PO) · Fecha: 2026-11-10 · Estado: borrador para revisión

> Formato: `specs/plantillas/intent-plantilla.md`. Es trabajo técnico, no una historia de usuario: no agrega funciones,
> por eso lleva el prefijo `TEC` y no `HU`. Lo que se pide es que el comportamiento **no cambie**.

## Qué queremos

Que `ClientesService` se pueda leer, entender y mantener, sin cambiar lo que hace.

## Por qué lo queremos

`ClientesService` lo escribió un ex integrante del equipo, con apuro. Funciona y tiene 4 tests, pero:

- Sus métodos se llaman `proc2` y `proc3`, y sus variables `_l`, `c`, `r`, `t` y `e`: nadie sabe qué hacen sin leerlos completos.
- La búsqueda del cliente por id está copiada en los dos métodos.
- Tiene condiciones redundantes (`== true`, `if/else` que devuelven un booleano) y el texto «Zona centro» repetido.

Nadie se ha atrevido a tocarlo. Es el módulo que el equipo documentará en la etapa 3 del taller, y documentar
`proc2` tal como está sería documentar un nombre que no explica nada.

## Para quién

El equipo de desarrollo que mantiene Pedidos Web. No hay cambio visible para el vendedor.

## Reglas que ya conocemos

Tal como lo plantea el taller: *refactorizar es cambiar la forma, no el comportamiento*. Los 4 tests existentes son la
red de seguridad: si siguen en verde, no se rompió nada. Lo que el servicio hace hoy, incluidas sus rarezas, se deja
escrito en la spec como el contrato que se preserva.

## Restricciones

- .NET 10, xUnit y los 4 tests actuales.
- Un cambio por vez, cada uno con los tests en verde y su commit; si algo queda en rojo, se vuelve al último verde.
- Los tests solo pueden cambiar en **cómo llaman** a los métodos renombrados, nunca en lo que verifican.
- Ningún paquete nuevo.
- Se hace dentro del taller de la Unidad 3 (etapa 2, 20 minutos).

## Cómo sabremos que funcionó

- Los 4 tests de `ClientesService` siguen en verde en cada paso y al final.
- `dotnet test` completo sigue en verde.
- El código se entiende mejor: nombres claros, sin búsqueda repetida y sin condiciones redundantes.

## Lo que NO es

- No corrige errores ni «mejora» el servicio por su cuenta: si aparece un comportamiento raro, se anota y no se toca.
- No cambia qué devuelve el servicio, incluidos el formato de la ficha y las zonas.
- No cambia `Cliente` ni `ClientesDeEjemplo`.
- No conecta `ClientesService` con la API ni con el detalle del pedido (HU-02 no lo usa).
- No lo documenta: eso es la etapa 3, con la Skill.
