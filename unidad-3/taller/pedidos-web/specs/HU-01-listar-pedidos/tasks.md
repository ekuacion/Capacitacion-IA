# Tasks: HU-01 · Listar pedidos con filtros

> Plan de origen: `specs/HU-01-listar-pedidos/plan.md` (aprobado) · Spec: `specs/HU-01-listar-pedidos/spec.md` v1.1 (aprobada)

> Formato: `specs/plantillas/tasks-plantilla.md`. Una tarea = un cambio pequeño con su verificación.
> Las tareas de abajo pueden depender de las de arriba, nunca al revés.

## Resumen

| ID | Tarea | CA cubiertos | Estado |
|---|---|---|---|
| T0 | Aprobar la spec v1.0 y el plan | — | hecha |
| T1 | Crear los tipos de la búsqueda en Core | — | hecha |
| T2 | Filtrar, ordenar y paginar | CA-01 a CA-06, CA-08, CA-10, CA-11, CA-12, CA-15, CA-17, CA-18, CA-23, CA-25 | hecha |
| T3 | Validar en el orden de RN-11 | CA-07, CA-09, CA-13, CA-14, CA-16, CA-19, CA-20, CA-24, CA-26 | hecha |
| T4 | Exponer `GET /api/pedidos/buscar` | — (se prueba en T5) | hecha |
| T5 | Tests HTTP del endpoint | CA-21, CA-22 | hecha |
| T6 | Revisar el diff y hacer commit | todos | hecha |

---

## T0 — Aprobar la spec v1.0 y el plan

- **CA cubiertos:** ninguno.
- **Depende de:** ninguna.
- **Archivos:** `specs/HU-01-listar-pedidos/spec.md`, `specs/HU-01-listar-pedidos/plan.md`.
- **Qué hacer:**
  - Claudio revisa la spec (incluidos los mensajes de §6) y la deja en «aprobada», versión 1.0, con su nombre en el historial.
  - Claudio revisa el plan, deja sus correcciones como `> **[Revisión del dev]**` y lo deja en «aprobado».
- **Verificación:** `grep -n "Estado" specs/HU-01-listar-pedidos/spec.md specs/HU-01-listar-pedidos/plan.md` → «aprobada» y «aprobado».
- **Hecho cuando:** los dos documentos están aprobados por una persona.

## T1 — Crear los tipos de la búsqueda en Core

- **CA cubiertos:** ninguno (es la base de T2 y T3).
- **Depende de:** T0.
- **Archivos:** `src/PedidosWeb.Core/Pedidos/ConsultaPedidos.cs`, `src/PedidosWeb.Core/Pedidos/ResultadoBusqueda.cs`,
  `src/PedidosWeb.Core/Pedidos/CodigosErrorBusqueda.cs`, `src/PedidosWeb.Core/Pedidos/BuscadorPedidos.cs` (solo la firma).
- **Qué hacer:**
  - `ConsultaPedidos`: record con los 7 parámetros como `string?`.
  - `ResultadoBusqueda`, `PaginaPedidos` y `ErrorBusqueda`, como dice el plan.
  - `CodigosErrorBusqueda`: los 6 códigos y sus mensajes, copiados de §6.
  - `BuscadorPedidos`: constructor con `IPedidoRepositorio` y `Buscar(ConsultaPedidos)`, todavía sin lógica.
- **Verificación:** `dotnet build` → 0 errores y 0 advertencias nuevas.
- **Hecho cuando:** compila y `dotnet test` sigue en verde (21 tests).

## T2 — Filtrar, ordenar y paginar

- **CA cubiertos:** CA-01, CA-02, CA-03, CA-04, CA-05, CA-06, CA-08, CA-10, CA-11, CA-12, CA-15, CA-17, CA-18, CA-23, CA-25.
- **Depende de:** T1.
- **Archivos:** `src/PedidosWeb.Core/Pedidos/BuscadorPedidos.cs`, `tests/PedidosWeb.Core.Tests/Pedidos/BuscadorPedidosTests.cs`.
- **Qué hacer:**
  - Tratar los filtros vacíos o con solo espacios como no enviados (RN-02).
  - Filtrar con Y por cliente, estado, sucursal y rango inclusivo de fechas, sin distinguir mayúsculas (RN-03 a RN-08).
  - Ordenar por `Fecha` y luego `Id`; paginar con los valores por defecto 1 y 10 e informar el `total` filtrado (RN-05, RN-09).
  - Un test por criterio, con `// CA-xx` en el comentario y Assert sobre ids y `total` exactos. CA-23 usa un `IPedidoRepositorio` de test.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~BuscadorPedidos"` → 15 tests en verde.
- **Hecho cuando:** los 15 tests pasan y `dotnet test` completo sigue en verde.

## T3 — Validar en el orden de RN-11

- **CA cubiertos:** CA-07, CA-09, CA-13, CA-14, CA-16, CA-19, CA-20, CA-24, CA-26 (agregado en la spec v1.1).
- **Depende de:** T2.
- **Archivos:** `src/PedidosWeb.Core/Pedidos/BuscadorPedidos.cs`, `tests/PedidosWeb.Core.Tests/Pedidos/BuscadorPedidosTests.cs`.
- **Qué hacer:**
  - Validar en este orden y devolver solo el primer error: `FILTRO_REQUERIDO` → `FECHA_INVALIDA` → `FECHAS_INVERTIDAS` →
    `ESTADO_INVALIDO` → `SUCURSAL_INVALIDA` → `PAGINACION_INVALIDA`.
  - Fechas con `DateOnly.TryParseExact` y formato `yyyy-MM-dd`; paginación con `int.TryParse` y rangos 1+ y 1 a 50.
  - Un test por criterio; CA-16 y CA-19 como `[Theory]` con sus dos casos.
- **Verificación:** `dotnet test --filter "FullyQualifiedName~BuscadorPedidos"` → 26 casos en verde: 15 de T2 y 11 de T3
  (9 tests; CA-16 y CA-19 son `[Theory]` con 2 casos cada uno).
- **Hecho cuando:** los 26 casos pasan y `dotnet test` completo sigue en verde.

## T4 — Exponer `GET /api/pedidos/buscar`

- **CA cubiertos:** ninguno directo (CA-21 y CA-22 lo prueban en T5).
- **Depende de:** T3.
- **Archivos:** `src/PedidosWeb.Api/Program.cs`.
- **Qué hacer:**
  - Registrar `BuscadorPedidos` como singleton.
  - `MapGet("/api/pedidos/buscar")` con los 7 parámetros de query como `string?`: `200` con la página o `400` con el error.
- **Verificación:** `dotnet run --project src/PedidosWeb.Api` y en el navegador:
  `http://localhost:5080/api/pedidos/buscar?clienteId=C-004` → el JSON del Ejemplo 1;
  `http://localhost:5080/api/pedidos/buscar` → el JSON del Ejemplo 3; `http://localhost:5080/api/pedidos/8` sigue funcionando.
- **Hecho cuando:** las 3 URL responden como se espera y `dotnet test` sigue en verde.

## T5 — Tests HTTP del endpoint

- **CA cubiertos:** CA-21, CA-22.
- **Depende de:** T4.
- **Archivos:** `tests/PedidosWeb.Api.Tests/PedidosWeb.Api.Tests.csproj`,
  `tests/PedidosWeb.Api.Tests/Pedidos/BuscarPedidosEndpointTests.cs`, `PedidosWeb.sln`, `AGENTS.md`.
- **Qué hacer:**
  - Crear el proyecto (xUnit con las mismas versiones que Core.Tests, `Microsoft.AspNetCore.Mvc.Testing` 10.0.14 y una referencia a `PedidosWeb.Api`).
  - Agregarlo a `PedidosWeb.sln`, en la carpeta `tests`.
  - Escribir CA-21 y CA-22 con `WebApplicationFactory<Program>`, comparando el código HTTP y el JSON exactos.
  - Proponer a Claudio la línea de `AGENTS.md` sobre `tests/PedidosWeb.Api.Tests/<Área>/` (regla 9).
- **Verificación:** `dotnet test` → 49 casos en verde: 21 previos + 26 de `BuscadorPedidos` + 2 HTTP.
- **Hecho cuando:** `dotnet test` completo está en verde y `AGENTS.md` está actualizado.

## T6 — Revisar el diff y hacer commit

- **CA cubiertos:** todos (revisión final).
- **Depende de:** T5.
- **Archivos:** todo el diff de la rama.
- **Qué hacer:**
  - Responder las 5 preguntas del paso 5 de la guía: cumple la spec criterio por criterio · los tests prueban algo real
    (romper el código a propósito y ver que fallen) · casos borde sin cubrir · secretos en el código · cambios que nadie pidió.
  - Actualizar la columna «Estado» de la tabla de trazabilidad.
  - Hacer el commit (persona): `git commit -m "feat: HU-01 listar pedidos con filtros (spec + tests)"`.
- **Verificación:** `git diff develop --stat` muestra solo los archivos de «Archivos afectados» del plan.
- **Hecho cuando:** hay un commit revisado por una persona, con los tests en verde.

---

## Trazabilidad CA → test

| CA | Qué verifica | Test xUnit | Tarea | Estado |
|---|---|---|---|---|
| CA-01 | Cliente C-004 → ids 15, 7, 39, 31, 23 | `Buscar_ClienteExistente_DevuelveSusPedidosEnOrden` | T2 | en verde |
| CA-02 | `c-004` = `C-004` | `Buscar_ClienteEnMinusculas_DevuelveLosMismosPedidos` | T2 | en verde |
| CA-03 | C-999 → lista vacía, total 0 | `Buscar_ClienteInexistente_DevuelveListaVacia` | T2 | en verde |
| CA-04 | C-001 → ids 40, 32, 24, 16, 8 | `Buscar_PorCliente_OrdenaPorFechaAscendente` | T2 | en verde |
| CA-05 | Anulado → ids 38, 11, 20, 29, 2 | `Buscar_EstadoValido_DevuelvePedidosDeEseEstado` | T2 | en verde |
| CA-06 | `entregado` → 10 de 15, página por defecto | `Buscar_EstadoEnMinusculas_DevuelvePrimeraPaginaDeDiez` | T2 | en verde |
| CA-07 | `Cancelado` → `ESTADO_INVALIDO` | `Buscar_EstadoInvalido_DevuelveErrorEstadoInvalido` | T3 | en verde |
| CA-08 | `stgo` → 11 pedidos de STGO | `Buscar_SucursalEnMinusculas_DevuelvePedidosDeLaSucursal` | T2 | en verde |
| CA-09 | `SCL` → `SUCURSAL_INVALIDA` | `Buscar_SucursalInvalida_DevuelveErrorSucursalInvalida` | T3 | en verde |
| CA-10 | Octubre → 23 pedidos, incluye ambos límites | `Buscar_RangoDeOctubre_IncluyeAmbosLimites` | T2 | en verde |
| CA-11 | Solo `desde=2026-10-28` → ids 23, 2, 42, 21 | `Buscar_SoloDesde_DevuelvePedidosDesdeEsaFecha` | T2 | en verde |
| CA-12 | Solo `hasta=2026-09-05` → ids 40, 19, 38 | `Buscar_SoloHasta_DevuelvePedidosHastaEsaFecha` | T2 | en verde |
| CA-13 | `desde` > `hasta` → `FECHAS_INVERTIDAS` | `Buscar_FechasInvertidas_DevuelveError` | T3 | en verde |
| CA-14 | `31-10-2026` → `FECHA_INVALIDA` | `Buscar_FechaMalFormada_DevuelveErrorFechaInvalida` | T3 | en verde |
| CA-15 | C-004 + Entregado → ids 15, 31 | `Buscar_ClienteYEstado_CombinaConY` | T2 | en verde |
| CA-16 | Sin filtros → `FILTRO_REQUERIDO` | `Buscar_SinFiltros_DevuelveErrorFiltroRequerido` (`[Theory]`) | T3 | en verde |
| CA-17 | Página 3 de 20 → ids 44, 23, 2, 42, 21, total 45 | `Buscar_PaginaTresDeVeinte_DevuelveCincoPedidosYTotal45` | T2 | en verde |
| CA-18 | Página más allá del final → lista vacía, total 5 | `Buscar_PaginaMasAllaDelFinal_DevuelveListaVacia` | T2 | en verde |
| CA-19 | `tamanoPagina=51` / `pagina=0` → `PAGINACION_INVALIDA` | `Buscar_PaginacionFueraDeRango_DevuelveErrorPaginacionInvalida` (`[Theory]`) | T3 | en verde |
| CA-20 | Varios errores → solo `FECHAS_INVERTIDAS` | `Buscar_VariosErrores_InformaSoloElPrimero` | T3 | en verde |
| CA-21 | HTTP 200 + JSON del Ejemplo 1 | `GetBuscar_ClienteExistente_Devuelve200ConPaginaJson` | T5 | en verde |
| CA-22 | HTTP 400 + cuerpo del Ejemplo 3 | `GetBuscar_SinFiltros_Devuelve400ConCuerpoDeError` | T5 | en verde |
| CA-23 | Misma fecha → ids 3, 9 | `Buscar_MismaFecha_DesempataPorIdAscendente` | T2 | en verde |
| CA-24 | `pagina=abc` → `PAGINACION_INVALIDA` | `Buscar_PaginaNoNumerica_DevuelveErrorPaginacionInvalida` | T3 | en verde |
| CA-25 | `estado=` cuenta como no enviado | `Buscar_FiltroVacio_SeTrataComoNoEnviado` | T2 | en verde |
| CA-26 | `pagina=&tamanoPagina=` → valores por defecto 1 y 10 | `Buscar_PaginacionVacia_UsaValoresPorDefecto` | T3 | en verde |
