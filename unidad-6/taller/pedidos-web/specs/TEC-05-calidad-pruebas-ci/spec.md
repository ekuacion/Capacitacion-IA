# Spec: TEC-05 · Calidad, pruebas y CI/CD local con Docker

> Versión: 0.5 · Estado: **aprobada** (Claudio Ortiz Castillo, 2026-11-12; regla 1 de `AGENTS.md`)
> Intent de origen: `specs/TEC-05-calidad-pruebas-ci/intent.md` · Autor del borrador: Claude Code
> Cambio v0.2: el pipeline local en Docker (idea del profesor). Cambio v0.4 (2026-11-12): Claudio recibió autorización para usar GitHub y ya creó el repo,
> así que **vuelven `ci.yml`, `claude.yml` y el PR con `@claude`** (como pide la guía) y el Docker local se conserva como segunda compuerta y despliegue.
> Cambio v0.5 (2026-11-12): el repo es `ekuacion/Capacitacion-IA`, **privado**, y `pedidos-web` es la subcarpeta `unidad-6/taller/pedidos-web` (no la raíz): los workflows van en `.github/workflows/` de la raíz del repo.
> Claude no hace push ni merge: los hace Claudio. El token nunca va en un archivo.

> Las decisiones marcadas con **[Decidir]** eran tuyas; quedaron resueltas (ver más abajo).

## Qué se entrega

| Archivo | Etapa | Contenido |
|---|---|---|
| `docs/calidad/registro-u6.md` | Todas | Registro completado; los campos de GitHub (enlace al run, PR, `claude.yml`) se adaptan a su equivalente local |
| `src/PedidosWeb.Core/Totales/CalculoTotales.cs` | 1 | H1: `PorcentajeVolumen` pasa de `>` a `>=` (regla del grabado) |
| `migracion/golden-master.csv` | 1 | Solo cambian los 13 casos afectados por H1 (ver tabla) |
| `docs/migracion/defectos-corregidos.md` | 1 | H1 pasa de «hallazgo» a «corregido», con casos antes y después |
| `tests/PedidosWeb.Core.Tests/Totales/CalculoTotalesBordeTests.cs` | 2 | 5 tests de borde + 2 de umbral (H1), cada uno probado rompiendo el código a mano |
| `.github/workflows/ci.yml` (raíz del repo) | 3b | Build y test en push y PR; solo si cambia `unidad-6/taller/pedidos-web/**`; `working-directory` en esa carpeta; permisos mínimos, timeout 10 min, versiones fijas |
| `.github/workflows/claude.yml` (raíz del repo) | 3b | `@claude` en issues y PR; secreto, 5 turnos, sin edición |
| `Dockerfile` | 3 | Multietapa: `restore/build` → `test` (la imagen no se construye si hay un test rojo) → `final` (la API) |
| `.dockerignore` | 2 | Deja fuera `bin/`, `obj/`, `.git/`, `legado/` y `.claude/` |
| `scripts/ci-local.ps1` | 2 | Compuerta local: build y test en contenedor; si pasa, construye y despliega la API; si falla, guarda el log en `ci-local.log` (ignorado por git) |
| `specs/README.md` | 0 | Fila de TEC-05 |

Los archivos nuevos van en la raíz de `pedidos-web`, salvo los dos workflows, que van en `.github/workflows/` de la **raíz del repo** (`d:\GitHub\Capacitacion-IA`), porque GitHub solo lee workflows ahí. Los que no son de `src/`, `tests/`, `specs/` o `docs/` están autorizados por esta spec (regla 9).

## Estado de partida (comprobado)

- `dotnet test`: **143 en verde, 0 omitidos** (138 Core + 5 Api; 60 del golden master). No hay tests pendientes que marcar con `Skip` (registro: «ninguno»).
- El hook `PostToolUse` con `dotnet test` está activo; `legado/` es solo lectura.
- Docker 29.9.2 está instalado, pero el motor no respondió: hay que **abrir Docker Desktop** (no es instalar nada).

## Etapa 1 · Normalizar H1: umbrales de descuento iguales a los del grabado

- **Decisión (Claudio, 2026-11-11):** la regla vigente es la del grabado (`>=`: base ≥ 500.000 → 5 %, base ≥ 1.000.000 → 8 %). No hay apoyo de contabilidad porque es un taller de capacitación, y en el repo no existe un documento oficial que fije el umbral (el comentario de `UCalculoTotales.pas:4` habla de 400.000 y no coincide con el código). Es una decisión de negocio **tomada por el PO**, no una inferencia de Claude.
- **Qué se hace:** el C# deja de replicar la pantalla (`>`) y pasa a la regla del grabado (`>=`), en `PorcentajeVolumen`. `legado/` no se toca (solo lectura): el legado sigue con su defecto en pantalla; el C# queda igual al grabado.
- **Cómo:** mismo patrón que R05: un commit aparte con el cambio de código y el CSV, y otro con `defectos-corregidos.md`.
- **Casos del golden master que cambian (13 de 60; calculados con una réplica en `decimal` que reproduce los 60 totales actuales):**

  | Caso | Tipo | Base | % antes → después | Total antes | Total después | Diferencia |
  |---|---|---|---|---|---|---|
  | 8 | N | 500.000 | 0 → 5 % | 627.071 | 597.321 | −29.750 |
  | 11 | V | 500.000 | 3 → 8 % | 588.931 | 559.181 | −29.750 |
  | 19 | N | 500.000 | 0 → 5 % | 605.591 | 575.841 | −29.750 |
  | 23 | V | 500.000 | 3 → 8 % | 599.717 | 569.967 | −29.750 |
  | 26 | N | 1.000.000 | 5 → 8 % | 1.310.023 | 1.274.323 | −35.700 |
  | 32 | N | 1.000.000 | 5 → 8 % | 1.266.017 | 1.230.317 | −35.700 |
  | 35 | N | 1.000.000 | 5 → 8 % | 1.136.331 | 1.100.631 | −35.700 |
  | 39 | N | 1.000.000 | 5 → 8 % | 1.134.665 | 1.098.965 | −35.700 |
  | 42 | N | 1.000.000 | 5 → 8 % | 1.153.067 | 1.117.367 | −35.700 |
  | 48 | N | 1.000.000 | 5 → 8 % | 1.312.403 | 1.276.703 | −35.700 |
  | 50 | N | 500.000 | 0 → 5 % | 727.649 | 697.899 | −29.750 |
  | 55 | N | 500.000 | 0 → 5 % | 602.021 | 572.271 | −29.750 |
  | 60 | N | 1.000.000 | 5 → 8 % | 1.137.521 | 1.101.821 | −35.700 |

  Suma de la columna `total`: **58.202.144 → 57.773.744** (−428.400). Los otros 47 casos no cambian. Las columnas que cambian en cada caso son `porc_descuento`, `descuento`, `neto`, `iva` y `total`; `flete` no cambia.
- **Tests nuevos del umbral (con valores escritos a mano):** una base de 500.000 da 5 % y una de 1.000.000 da 8 %; reemplazan al test «H1 explícito» que habíamos descartado.
- **Recargo de Navidad (`UFrmPedidos.pas:304-305`):** sigue fuera de alcance (apagado y no está en el C#).
- **Convivencia:** mientras el legado muestre el descuento de pantalla con `>`, esos 13 de cada 60 pedidos darán una diferencia de 29.750 o 35.700 pesos entre la pantalla del legado y el C#; el grabado del legado coincide con el C#. Se anota en `defectos-corregidos.md`.

## Etapa 2 · Tests de borde (módulo: `CalculoTotales`, decidido por Claudio)

El golden master no cubre estos casos (se comprobó en el CSV: siempre IVA 0,19, nunca un pedido sin líneas, ninguna categoría con espacios o minúsculas, ningún flete base en 0).
Cada uno sale de una regla ya escrita en `TEC-04/spec.md` y se explica antes de escribirlo:

| # | Caso | Regla | Esperado | Cambio a mano para probarlo |
|---|---|---|---|---|
| B1 | Pedido sin líneas, región 13, flete base 3.500 | RG-06 | flete 0; subtotal, descuento, IVA y total 0 | quitar la condición `lineas.Count == 0` |
| B2 | Línea con categoría `" ofe "` (espacios y minúsculas) | RG-02 | no entra a la base del descuento | quitar `.Trim()` o cambiar a `Ordinal` |
| B3 | Tasa de IVA 0 | RG-07 | IVA 0 y total = neto + flete | fijar la tasa en 0,19 dentro del método |
| B4 | Flete base 0 en región 12 | RG-06 | flete 0 (0 × 1,15) | sumar 1 al flete |
| B5 | Flete base 10 en región 12 (10 × 1,15 = 11,5) | RG-06 + R05 | flete 12 (la fracción 0,50 sube) | volver `RedondearAlPeso` a `+ 0.49m` |

Reglas de la etapa: un test = un caso; nombres `Metodo_Escenario_Resultado`; valores esperados escritos a mano desde la regla (nunca calculados con el mismo código); si un test no falla al romper el código, se borra y se anota en el registro.

## Etapa 3 · CI y despliegue local en Docker

- **Dockerfile multietapa** con imágenes oficiales de versión fija (`mcr.microsoft.com/dotnet/sdk:10.0` y `.../aspnet:10.0`), sin secretos, sin `COPY` de `.env` y con la API corriendo con un usuario sin privilegios.
  - Etapa `test`: `dotnet restore`, `dotnet build -c Release` y `dotnet test -c Release`. Si un test falla, `docker build` termina con error: **esa es la compuerta del CI**.
  - Etapa `final`: `dotnet publish` de `PedidosWeb.Api` y ejecución en `http://+:8080`.
- **`scripts/ci-local.ps1`:** ejecuta `docker build --target test`; si pasa, `docker build` de la imagen final y `docker run -d -p 5080:8080 --name pedidos-web`; después comprueba `GET /salud` → `{"estado":"ok"}`. Con `-Detener` baja y borra el contenedor. Si el build falla, deja el log en `ci-local.log`.
- **Diagnóstico agéntico local:** cuando el script falla, Claudio me pega o apunta `ci-local.log` en Claude Code y yo **solo diagnostico** (cuál test, qué línea, por qué). No edito ni hago commits en esa etapa. Si más adelante `claude -p` está en el PATH, el script puede encadenarlo; hoy se hace a mano.
- **Fuera de esta spec:** `act` (no se instala nada) y despliegue en la nube.

## Etapa 3b · Pipeline en GitHub (`ci.yml` y `claude.yml`)

Se hace **antes** del Docker, porque el verde en Actions se ve solo después de que Claudio suba el código.

- **`.github/workflows/ci.yml`:** disparadores `push` y `pull_request` filtrados con `paths: unidad-6/taller/pedidos-web/**` (el repo tiene las 8 unidades) y `defaults.run.working-directory: unidad-6/taller/pedidos-web`; `permissions: contents: read`; `timeout-minutes: 10`; versiones fijas (`runs-on: ubuntu-24.04`, `actions/checkout` y `actions/setup-dotnet` con versión exacta, SDK .NET 10); pasos `dotnet restore`, `dotnet build --no-restore -c Release`, `dotnet test --no-build -c Release`. Cero secretos.
- **`.github/workflows/claude.yml`:** modo interactivo (`@claude` en issues y PR), basado en el ejemplo oficial; si falla, se usa `unidad-6/taller/u6-respaldo/.github/workflows/claude.yml`. Usa `${{ secrets.CLAUDE_CODE_OAUTH_TOKEN }}`, `--max-turns 5`, `timeout-minutes: 10`, `concurrency`, sin herramientas de edición.
- **Secretos (los configura Claudio; Claude no los toca ni los ve):** (1) instalar la [Claude GitHub App](https://github.com/apps/claude) solo en su repo; (2) `claude setup-token` en su terminal; (3) *Settings › Secrets and variables › Actions › New repository secret* con el nombre `CLAUDE_CODE_OAUTH_TOKEN`.
- **Push y merge:** los hace Claudio. Claude deja commits locales y le indica el comando.
- **Orden hacia `main`:** `claude.yml` solo responde si está en la rama por defecto (`main`). Claudio mergea primero `ci.yml` y `claude.yml` a `main`; recién después abre el PR de la falla (etapa 4).
- **Repo privado:** Actions consume los minutos del plan de Claudio (el build y test dura pocos minutos).
- **Rama protegida (recomendado, lo hace Claudio):** exigir el job de `ci.yml` en verde para mergear a `main`.

## Etapa 4 · La falla

1. Claudio **rompe a mano en Rider** la tasa de IVA en `CalculoTotales.cs` (no Claude), en una rama nueva.
2. **Local:** ejecuta `scripts/ci-local.ps1`; el CI local queda rojo y la API no se despliega.
3. **GitHub:** sube la rama, abre el PR y el job de `ci.yml` queda rojo.
4. Claudio comenta en el PR: `@claude ¿por qué falla este job? Solo diagnostica, no hagas commits.` Claude (en Actions) responde; el diagnóstico local de `ci-local.log` sirve de comparación o de plan B si `@claude` no responde.
5. Claudio decide (aceptar, ajustar o rechazar el diagnóstico), corrige, espera `ci.yml` en verde (y el script local) y hace el merge.
6. Claude ayuda a completar `registro-u6.md` con lo que Claudio le dicte.

## Decisiones que necesito **[Decidir]**

- **D-07 · Repo en GitHub:** `https://github.com/ekuacion/Capacitacion-IA`, **privado**. **Resuelto.**
- **D-08 · Cómo llega el código:** `pedidos-web` es la subcarpeta `unidad-6/taller/pedidos-web` del repo ya subido por Claudio; no se usa `git subtree split`. **Resuelto.**
- **D-09 · Push:** lo hace Claudio. **Resuelto.**
- **D-10 · Versiones fijas:** tags exactos, verificados contra GitHub al escribir los workflows (T3) y mostrados a Claudio para confirmar. **Resuelto.**

- **D-01 · Módulo de la etapa 1:** `CalculoTotales`. **Decidido por Claudio.**
- **D-02 · Puerto del host:** `5080`. **Aprobado por Claudio.**
- **D-03 · Script:** solo `ci-local.ps1` (PowerShell). **Aprobado por Claudio.**
- **D-04 · Registro:** con GitHub de vuelta se **mantienen** los campos originales (run de Actions, PR, `claude.yml` en `main`, diagnóstico de `@claude`) y se **suman** «resultado de `ci-local.ps1`» y «log local». **Aprobado por Claudio.**
- **D-05 · H1:** se corrige dentro de TEC-05 (etapa 1) a la regla del grabado `>=`, con el C# como único cambio de código. **Decidido por Claudio.**
- **D-06 · Falla de la etapa 4:** opción 1, la tasa de IVA en `Calcular` (p. ej. `* tasaIva` por `* 0.18m`), rota a mano por Claudio en Rider. B5 se prueba con el redondeo. **Aprobado por Claudio.**

## Criterios de aceptación

- **CA-01 · Base verde.** `dotnet test` da 143 en verde y 0 omitidos antes de empezar y no baja de ahí.
- **CA-02 · Tests nuevos que sí prueban.** Los 5 tests de borde y los 2 de umbral fallaron al menos una vez al romper el código a mano; los que no fallaron se borraron y quedan anotados.
- **CA-03 · H1 corregido y aislado.** `PorcentajeVolumen` usa `>=`; el commit de H1 cambia solo `CalculoTotales.cs` y 13 casos del CSV (suma de `total` 57.773.744); `git diff -- legado` vacío; 60 de 60 en verde con el CSV nuevo.
- **CA-10 · `ci.yml` seguro.** `contents: read`, `timeout-minutes: 10`, versiones fijas, sin secretos; verde en Actions (lo comprueba Claudio tras el push).
- **CA-11 · `claude.yml` seguro.** Secreto `CLAUDE_CODE_OAUTH_TOKEN` solo en GitHub, `--max-turns 5`, sin edición; en `main`; `git grep -n -iE "oauth|token"` solo muestra el nombre del secreto.
- **CA-04 · CI local verde.** `scripts/ci-local.ps1` construye, pasa los tests en el contenedor y despliega la API; `GET http://localhost:5080/salud` responde `{"estado":"ok"}`.
- **CA-05 · La compuerta cierra.** Con un test en rojo, el script termina con error, no despliega y deja `ci-local.log`.
- **CA-06 · Docker seguro.** Imágenes de versión fija, usuario sin privilegios, sin secretos; `git grep -n -iE "token|secret|password"` sin coincidencias nuevas.
- **CA-07 · Falla resuelta.** Falla provocada por Claudio, PR con el diagnóstico de `@claude` (solo lectura), corrección de Claudio, `ci.yml` y CI local en verde, y merge hecho por Claudio.
- **CA-08 · Registro completo.** `docs/calidad/registro-u6.md` sin comentarios de plantilla.
- **CA-09 · Nada más cambia.** Sin paquetes NuGet ni software instalado; `PedidosWeb.sln`, `legado/` y `AGENTS.md` sin cambios; Claude no hace push ni merge.

## Fuera de alcance

`act`, el recargo de Navidad del grabado, editar `legado/` (solo lectura), despliegue en la nube, base de datos real (la API usa datos en memoria), cobertura de código y la extensión opcional (`schedule`).
