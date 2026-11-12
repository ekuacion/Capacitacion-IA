# Plan: TEC-01 · Ordenar `ClientesService`

> Spec de origen: `specs/TEC-01-ordenar-clientes-service/spec.md` v1.1 · Autor: Claude Code · Revisado por: Claudio Ortiz Castillo
> Estado: aprobado (Claudio Ortiz Castillo, 2026-11-10), con los nombres propuestos y el T1 de caracterización · Alcance ajustado en el T2: los tests de caracterización también se actualizan en la llamada (Claudio)

> Formato: `specs/plantillas/plan-plantilla.md`. Responde **cómo** se ordena el servicio; no repite la spec.
> Las correcciones del dev se dejan visibles con el formato `> **[Revisión del dev]** qué está mal, por qué y qué hacer en su lugar.`
> No se toca código hasta que la spec esté «aprobada» (v1.0) y este plan «aprobado» (regla 1 de `AGENTS.md`).

## Enfoque

Se refactoriza en **pasos chicos, cada uno con sus tests en verde y su commit**: primero se afianza la red de
seguridad, después se renombra (qué hace cada cosa), luego se extrae lo repetido y al final se simplifica. Cada paso
cambia una sola clase de cosa, para que, si un test se pone rojo, se sepa de qué paso viene y se pueda volver al
último verde. El comportamiento queda fijado por los criterios CP-01 a CP-08 de la spec.

## Decisiones técnicas

- **Nombres nuevos (aprobados por Claudio):**

  | Hoy | Propuesta | Por qué |
  |---|---|---|
  | `proc2` | `ObtenerFicha` | Devuelve la ficha de texto del cliente |
  | `proc3` | `ExisteYEstaActivo` | Dice las dos condiciones que comprueba; `EstaActivo` sugeriría que un id inexistente es un error |
  | `_l` / `l` | `_clientes` / `clientes` | Es la lista de clientes |
  | `c` | `cliente` | El cliente encontrado |
  | `r` | `rut` | El RUT normalizado |
  | `t` | `zona` | La zona según la región |
  | `e` | `estado` | `Activo` o `Inactivo` |

  Nombres del dominio en español, como pide `AGENTS.md`.
- **Los tests cambian solo en cómo llaman al método**, tanto los 4 originales como los 10 de caracterización del T1
  (también llaman a `proc2` y `proc3` por su nombre). Renombrar `proc2` obliga a cambiar la llamada
  `_servicio.proc2(...)`; lo que se verifica no cambia. Los nombres de los tests (`Proc2_...`) se renombran
  en el mismo paso para que no queden desfasados. Descartado: dejar los nombres viejos como alias, porque dejaría el
  nombre confuso dentro de la clase.
- **Búsqueda extraída a un método privado `BuscarPorId(string id)` que devuelve `Cliente?`**, con
  `_clientes.FirstOrDefault(cliente => cliente.Id == id)`, que además elimina el índice `i` del `for`. Mantiene el primer elemento ante ids repetidos y la comparación exacta
  (CP-07). Descartado: un diccionario por id, que cambiaría el comportamiento con ids repetidos.
- **La zona con una expresión `switch`**, uniendo `RM` y `V` en una sola rama `"Zona centro"`. Mantiene la
  comparación exacta (CP-04) y deja el texto una sola vez. Descartado: ignorar mayúsculas, que cambiaría el comportamiento.
- **`cliente.Activo` en vez de `cliente.Activo == true`**, y `ExisteYEstaActivo` como una sola expresión
  (`cliente is { Activo: true }`). Mismo resultado para `bool`, menos ruido.
- **No se arregla nada de paso.** Rarezas como que `RM` y `V` compartan zona se dejan como están (spec, fuera de alcance).
- **Sin paquetes nuevos.**

## Archivos afectados

| Archivo | Nuevo / Modificado | Tarea | Para qué |
|---|---|---|---|
| `tests/PedidosWeb.Core.Tests/Clientes/ClientesServiceCaracterizacionTests.cs` | Nuevo | T1 | Tests que fijan lo que el código hace hoy y que los 4 existentes no cubren (aprobado) |
| `src/PedidosWeb.Core/Clientes/ClientesService.cs` | Modificado | T2 a T6 | Renombres, extracción de la búsqueda y simplificación |
| `tests/PedidosWeb.Core.Tests/Clientes/ClientesServiceTests.cs` | Modificado | T2, T3 | **Solo** la llamada y el nombre de los métodos renombrados; nada de lo que verifican |
| `tests/PedidosWeb.Core.Tests/Clientes/ClientesServiceCaracterizacionTests.cs` | Modificado | T2, T3 | Lo mismo que el anterior: los tests del T1 también llaman a `proc2` y `proc3` por su nombre |

No se tocan: `Cliente.cs`, `ClientesDeEjemplo.cs`, `Program.cs`, `Pedidos/`, `Lineas/` ni los tests de otras áreas.

## Riesgos

| Riesgo | Mitigación |
|---|---|
| Los 4 tests no cubren zonas sur y norte, `Sin zona`, `Inactivo` ni el RUT con espacios, y un refactor podría cambiarlos sin que nadie lo note | T1: tests de caracterización antes de tocar el código (aprobado). |
| Claude «arregla» algo de paso, y eso ya no es refactor | Cada tarea declara qué toca y qué no; se revisa `git diff` antes de cada commit. |
| Un paso sale gigante y los tests quedan rojos | Un cambio por commit; con rojo se vuelve al último verde y se da un paso más chico. |
| Renombrar los métodos obliga a tocar los tests (los 4 originales y los 10 de caracterización) y se confunde con cambiar su contenido | El plan dice qué se puede cambiar de los tests: solo la llamada y el nombre. Se verifica con `git diff` de los dos archivos de tests. |
| Unir las ramas de `RM` y `V` cambia el comportamiento sin querer | El test de `C-004` (`V`) y el de `C-001` (`RM`) lo cubren; T1 agrega más. |

## Pasos

1. Aprobar la spec (v1.0) y este plan. → «aprobada» y «aprobado».
2. T1, tests de caracterización. → `dotnet test` en verde sobre el código **sin cambiar**.
3. T2, renombrar `proc2`. → los tests en verde.
4. T3, renombrar `proc3`. → los tests en verde.
5. T4, renombrar el campo y las variables. → los tests en verde.
6. T5, extraer la búsqueda del cliente (`BuscarPorId`, sin el `for`). → los 83 tests en verde.
7. T6, simplificar condiciones y zonas (`switch` para la zona, `cliente.Activo` sin `== true`). → los 83 tests en verde.

Estado: T0 a T6 hechas. Refactor completo.

El detalle de cada paso está en `tasks.md`.

## Fuera del plan

- Corregir comportamientos que parezcan errores.
- Cambiar `Cliente`, `ClientesDeEjemplo`, la API o el detalle del pedido.
- Documentar el módulo (etapa 3).
