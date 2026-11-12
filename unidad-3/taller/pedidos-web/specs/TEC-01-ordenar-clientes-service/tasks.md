# Tasks: TEC-01 · Ordenar `ClientesService`

> Plan de origen: `specs/TEC-01-ordenar-clientes-service/plan.md` (aprobado) · Spec: `specs/TEC-01-ordenar-clientes-service/spec.md` v1.1 (aprobada)

> Formato: `specs/plantillas/tasks-plantilla.md`. Una tarea = un cambio pequeño con su verificación y su commit.
> En un refactor, **toda tarea termina con los tests en verde**. Si quedan en rojo, se vuelve al último verde y se
> da un paso más chico; no se arregla encima.

## Resumen

| ID | Tarea | CP / CA cubiertos | Estado |
|---|---|---|---|
| T0 | Aprobar la spec v1.0 y el plan | — | hecha |
| T1 | Tests de caracterización | CP-03, CP-04, CP-05, CP-06, CP-07, CP-08 | hecha |
| T2 | Renombrar `proc2` | CA-01, CA-02 | hecha |
| T3 | Renombrar `proc3` | CA-01, CA-02 | hecha |
| T4 | Renombrar el campo y las variables | CA-01, CA-02 | hecha |
| T5 | Extraer la búsqueda del cliente | CA-01, CA-03, CP-07 | hecha |
| T6 | Simplificar condiciones y zonas | CA-01, CP-04, CP-05, CP-06 | hecha |

> Conteo de tests: 83 en total, de los cuales 14 son de `ClientesService` (4 originales y 10 de caracterización).

---

## T0 — Aprobar la spec v1.0 y el plan

- **Depende de:** ninguna.
- **Archivos:** `specs/TEC-01-ordenar-clientes-service/spec.md`, `plan.md`.
- **Qué hacer:**
  - Claudio decidió los tests de caracterización (T1) y aprobó los nombres nuevos del plan.
  - Claudio dejó la spec en «aprobada» v1.0 y el plan en «aprobado».
- **Verificación:** `grep -n "Estado" specs/TEC-01-ordenar-clientes-service/spec.md specs/TEC-01-ordenar-clientes-service/plan.md` → «aprobada» y «aprobado».
- **Hecho cuando:** los dos documentos están aprobados por una persona.

## T1 — Tests de caracterización

- **CP cubiertos:** CP-03 (espacios en el RUT), CP-04 (zonas sur, norte y `Sin zona`), CP-05 (estado `Inactivo`).
- **Depende de:** T0.
- **Archivos:** `tests/PedidosWeb.Core.Tests/Clientes/ClientesServiceCaracterizacionTests.cs`.
- **No tocar:** `ClientesService.cs`, `ClientesServiceTests.cs` y cualquier otro archivo.
- **Qué hacer:**
  - Tests nuevos que fijen lo que el código hace **hoy**, usando los clientes de ejemplo: C-003 (sur), C-002 (norte), C-007 (`Inactivo`), C-008 (RUT con espacios).
  - `Sin zona` se prueba con una lista propia de un cliente con otra región, porque los 8 de ejemplo siempre tienen zona.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~Clientes"` → en verde **sobre el código sin cambiar**.
- **Hecho cuando:** los tests nuevos pasan sin haber tocado el servicio.
- **Commit:** `test(u3): caracteriza ClientesService antes de refactorizar`

## T2 — Renombrar `proc2`

- **CA cubiertos:** CA-01, CA-02.
- **Depende de:** T1.
- **Archivos:** `src/PedidosWeb.Core/Clientes/ClientesService.cs`, `tests/PedidosWeb.Core.Tests/Clientes/ClientesServiceTests.cs` y `tests/PedidosWeb.Core.Tests/Clientes/ClientesServiceCaracterizacionTests.cs`.
- **No tocar:** el cuerpo del método, lo que verifican los tests y los demás archivos.
- **Qué hacer:**
  - Renombrar `proc2` a `ObtenerFicha` (nombre aprobado en el plan).
  - En los dos archivos de tests, cambiar solo la llamada y el nombre de los métodos `Proc2_...`.
- **Verificación:** `dotnet test` → todo en verde (83); `git diff` de los tests muestra solo llamadas y nombres.
- **Hecho cuando:** los 4 tests originales y los 10 de caracterización pasan, y el diff se lee en 5 minutos.
- **Commit:** `refactor(u3): renombra proc2 a ObtenerFicha`

## T3 — Renombrar `proc3`

- **CA cubiertos:** CA-01, CA-02.
- **Depende de:** T2.
- **Archivos:** `ClientesService.cs`, `ClientesServiceTests.cs` y `ClientesServiceCaracterizacionTests.cs`.
- **No tocar:** el cuerpo del método ni lo que verifican los tests.
- **Qué hacer:** renombrar `proc3` a `ExisteYEstaActivo` y ajustar, en los dos archivos de tests, solo la llamada y el nombre de los tests `Proc3_...`.
- **Verificación:** `dotnet test` → todo en verde (83).
- **Hecho cuando:** los 4 tests originales y los 10 de caracterización pasan, y ya no queda `proc2` ni `proc3` en el código.
- **Commit:** `refactor(u3): renombra proc3 a ExisteYEstaActivo`

## T4 — Renombrar el campo y las variables

- **CA cubiertos:** CA-01, CA-02.
- **Depende de:** T3.
- **Archivos:** `ClientesService.cs`.
- **No tocar:** la lógica, los tests y cualquier otro archivo.
- **Qué hacer:** `_l`/`l` → `_clientes`/`clientes`; `c` → `cliente`; `r` → `rut`; `t` → `zona`; `e` → `estado`.
- **Verificación:** `dotnet test` → todo en verde; `git diff --stat` solo muestra `ClientesService.cs`.
- **Hecho cuando:** no quedan variables de una letra (salvo el índice `i` del `for`, que desaparece en el T5).
- **Commit:** `refactor(u3): nombres claros en ClientesService`

## T5 — Extraer la búsqueda del cliente

- **CA / CP cubiertos:** CA-01, CA-03; protege CP-01, CP-06 y, sobre todo, CP-07 (búsqueda exacta, gana el primero).
- **Depende de:** T4.
- **Archivos:** `src/PedidosWeb.Core/Clientes/ClientesService.cs` (solo ese).
- **No tocar:** los tests, el resultado de los métodos, `== true`, las zonas y el formato de la ficha (eso es el T6).
- **Qué hacer:** quitar la búsqueda duplicada y dejarla en un único método privado.

  Antes (el mismo bloque, copiado en `ObtenerFicha` y en `ExisteYEstaActivo`):

  ```csharp
  Cliente? cliente = null;
  for (int i = 0; i < _clientes.Count; i++)
  {
      if (_clientes[i].Id == id) { cliente = _clientes[i]; break; }
  }
  ```

  Después (una sola vez, y cada método lo llama):

  ```csharp
  private Cliente? BuscarPorId(string id) => _clientes.FirstOrDefault(cliente => cliente.Id == id);

  // en ObtenerFicha y en ExisteYEstaActivo:
  var cliente = BuscarPorId(id);
  ```

  - **Por qué es equivalente:** `FirstOrDefault` devuelve el primer cliente que cumple y `null` si ninguno, igual que el
    `for` con `break`; el `==` entre textos sigue distinguiendo mayúsculas. El índice `i` desaparece.
  - **Lo que queda igual en cada método:** `if (cliente == null) return "";` y `if (cliente == null) return false;`.
- **Verificación:**
  - `dotnet test` → 83 en verde.
  - `git diff --stat` muestra solo `ClientesService.cs`.
  - La palabra `for (` ya no aparece en el archivo y la búsqueda por id existe una sola vez.
- **Verificación extra (mutaciones):** cambiar `FirstOrDefault` por `LastOrDefault` y la comparación por una que ignore
  mayúsculas, y comprobar que los tests de caracterización fallan (ids repetidos y `c-001`). Se restaura después.
- **Riesgo propio de este paso:** es el único que reescribe lógica y no solo nombres. Lo cubren los tests de ids
  repetidos, de otra capitalización, de id `null` y de lista sin copiar (T1).
- **Hecho cuando:** los 14 tests de `ClientesService` (4 originales y 10 de caracterización) pasan y no hay código de
  búsqueda repetido.
- **Commit:** `refactor(u3): extrae la búsqueda del cliente`

## T6 — Simplificar condiciones y zonas

- **CA / CP cubiertos:** CA-01; protege CP-04 (zonas), CP-05 (estado) y CP-06 (existe y activo).
- **Depende de:** T5.
- **Archivos:** `src/PedidosWeb.Core/Clientes/ClientesService.cs` (solo ese).
- **No tocar:** los tests, el formato de la ficha (`Nombre | RUT | Zona | Estado`) y las reglas de zona.
- **Qué hacer:** quitar condiciones redundantes y el texto repetido, sin cambiar qué devuelve cada caso.

  | Antes | Después |
  |---|---|
  | `if (cliente.Activo == true) estado = "Activo"; else estado = "Inactivo";` | `string estado = cliente.Activo ? "Activo" : "Inactivo";` |
  | 4 `if/else if` para la zona, con `"Zona centro"` escrito dos veces | `string zona = cliente.Region switch { "RM" or "V" => "Zona centro", "VIII" => "Zona sur", "II" => "Zona norte", _ => "Sin zona" };` |
  | `if (cliente.Activo == true) return true; else return false;` | `return cliente is { Activo: true };` |

  - **Por qué es equivalente:** `Activo` ya es un `bool`, así que `== true` no agrega nada; el `switch` compara los
    textos de forma exacta, igual que los `==` actuales, y cualquier otra región cae en `Sin zona`.
  - **Se deja igual:** la concatenación del texto de la ficha, y `if (cliente == null) return false;` si hace falta
    para leerse con claridad.
- **Verificación:** `dotnet test` → 83 en verde; `git diff --stat` solo muestra `ClientesService.cs`.
- **Verificación extra (mutaciones):** cambiar una zona y el texto `Inactivo` y comprobar que los tests de
  caracterización fallan.
- **Hecho cuando:** los 14 tests de `ClientesService` pasan y «Zona centro» aparece una sola vez.
- **Commit:** `refactor(u3): simplifica condiciones y zonas`
