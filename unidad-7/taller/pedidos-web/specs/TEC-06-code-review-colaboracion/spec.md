# Spec: TEC-06 · Code review y colaboración

| Campo | Valor |
|---|---|
| Estado | aprobada |
| Versión | 1.1 |
| Fecha | 2026-11-12 |
| Autor(es) | Claudio Ortiz Castillo (PO) con Claude Code |
| Intent de origen | `specs/TEC-06-code-review-colaboracion/intent.md` |

---

## 1. Contexto y objetivo

El CI de la U6 prueba cada PR. Falta definir quién lo revisa y con qué reglas. Se crean reglas escritas (`REVIEW.md`), un subagente revisor local, un workflow que revisa cada PR y una plantilla de PR; se practican con el PR de la HU-03.

## 2. Historia de usuario

Como integrante del equipo de desarrollo, quiero que cada PR se revise con reglas escritas por la IA y por una persona, para que los cambios lleguen a la rama base con hallazgos respondidos y no con aprobaciones automáticas.

## 3. Reglas de negocio

- **RN-01:** `REVIEW.md` define qué es *Importante*, qué *no reportar* y un tope de detalles menores. Incluye **3 chequeos de «Revisar siempre»** (los propone Claude en T1; Claudio los aprueba).
- **RN-02:** `.claude/agents/revisor.md` es de solo lectura (sin herramientas de escritura) y lee `AGENTS.md` y `REVIEW.md` desde la rama base.
- **RN-03:** `claude-review.yml` corre en `opened`, `reopened` y `ready_for_review`, no en cada push; con permisos mínimos, timeout, versiones fijas y el token solo como secreto.
- **RN-04:** `REVIEW.md` y `claude-review.yml` están en la rama base (`main`) **antes** de abrir el PR de HU-03.
- **RN-05:** Cada hallazgo de la IA recibe «Acepto», «Discutir» o «Descarto» con argumento propio; el PR tiene al menos 1 hallazgo humano que no es de estilo.
- **RN-06:** El merge se hace con el CI en verde.
- **RN-07 (HU-03; decisión del PO, 2026-11-12):** `FiltroPedidos.EntreFechas(pedidos, desde, hasta)` en `src/PedidosWeb.Core/Pedidos/`; límites inclusivos; `desde > hasta` lanza `ArgumentException`; `pedidos` nulo lanza `ArgumentNullException`; devuelve `IReadOnlyList<Pedido>` en el orden recibido. (Nulo y orden no están en la spec de HU-03: los decidió el PO.)
- **RN-09 (HU-03; decisión del PO, 2026-11-12, tras la revisión local):** si `pedidos` contiene un elemento nulo, `EntreFechas` lanza `ArgumentException` con mensaje claro (dato inválido: falla rápido, no se ignora). Se agrega también un test de borde para `desde == hasta` (un solo día, sin criterio numerado).
- **RN-08:** `.github/pull_request_template.md` pide Contexto, Qué cambia, Por qué, Cómo probarlo y una casilla «Corrí el revisor antes de pedir revisión». El pedido a Claude para crearla lo redacta Claudio.

## 4. Criterios de aceptación

**CA-01 — Base sembrada**
- **Dado** `unidad-7/taller/pedidos-web` con 3 tests en rojo (base del curso), **cuando** se siembra desde U6, **entonces** `dotnet test` da 150 superados y 0 omitidos, y el `README.md`, la HU-03 y `checklist-revision.md` de U7 se conservan.

**CA-02 — CI apunta a unidad-7**
- **Dado** `ci.yml` con `working-directory: unidad-6/...`, **cuando** se cambia a `unidad-7/...`, **entonces** el CI del PR corre en unidad-7 y queda en verde (no verificado hasta ver el run).

**CA-03 — REVIEW.md**
- **Dado** `REVIEW.md`, **cuando** se lee, **entonces** tiene Importante, No reportar, tope de detalles y 3 chequeos de «Revisar siempre».

**CA-04 — Revisor solo lectura**
- **Dado** `revisor.md`, **cuando** se lee su frontmatter, **entonces** no lista `Edit`, `Write` ni `Bash` sin restricción, y manda leer `AGENTS.md` y `REVIEW.md` desde la rama base. Además aparece al reiniciar Claude Code.

**CA-05 — Workflow seguro**
- **Dado** `claude-review.yml`, **cuando** se revisa, **entonces** cumple RN-03 y una búsqueda no encuentra ningún valor de token (solo el nombre `CLAUDE_CODE_OAUTH_TOKEN`).

**CA-06 — Reglas en la base antes del PR**
- **Dado** el PR de HU-03, **cuando** se abre, **entonces** `REVIEW.md` y `claude-review.yml` ya están en `main` y el CI sigue en verde.

**CA-07 — HU-03 implementada con tests**
- **Dado** `desde = 2026-03-01` y `hasta = 2026-03-31`, **cuando** se filtra, **entonces**: aparece el pedido del 03-01 (HU-03/CA-01), aparece el del 03-31 (HU-03/CA-02), no aparecen los del 02-28 ni 04-01 (HU-03/CA-03), `desde > hasta` lanza `ArgumentException` con mensaje claro (HU-03/CA-04), `pedidos` nulo lanza `ArgumentNullException` y se conserva el orden de entrada (RN-07); un elemento nulo en `pedidos` lanza `ArgumentException` (RN-09). Un test por criterio y `dotnet test` en verde.

**CA-08 — Descripción de PR**
- **Dado** el PR de HU-03, **cuando** se abre, **entonces** su descripción tiene Qué cambia, Por qué (escrito por Claudio) y Cómo probarlo, sin `[COMPLETAR]`, y el revisor local se corrió antes.

**CA-09 — Claude comenta el PR**
- **Dado** el PR abierto, **cuando** corre `claude-review.yml`, **entonces** aparece al menos un comentario de Claude. Si no aparece, se anota como no cumplido.

**CA-10 — Revisión cruzada**
- **Dado** los comentarios de la IA en el PR, **cuando** Claudio los responde, **entonces** cada hilo tiene Acepto/Discutir/Descarto con argumento propio, hay 1 hallazgo humano no estilístico, `docs/checklist-revision.md` está completo y el merge se hizo con CI en verde.

**CA-11 — Plantilla de PR**
- **Dado** `.github/pull_request_template.md` en la rama base, **cuando** se abre un PR nuevo, **entonces** GitHub muestra sus secciones y la casilla del revisor.

**CA-12 — Registro honesto**
- **Dado** el cierre, **cuando** se escribe `docs/calidad/registro-u7.md`, **entonces** cada criterio dice cumplido, no cumplido o no verificado, y los pendientes de la U6 (CA-02, CA-05, CA-07) figuran como pendientes.

## 5. Ejemplos de entrada y salida

- **Éxito:** `EntreFechas(pedidos, 2026-03-01, 2026-03-31)` con pedidos de 02-28, 03-01, 03-15, 03-31 y 04-01 → devuelve los de 03-01, 03-15 y 03-31, en ese orden.
- **Éxito (un día):** `desde = hasta = 2026-03-15` → solo los pedidos del 03-15.
- **Error:** `EntreFechas(pedidos, 2026-03-31, 2026-03-01)` → `ArgumentException` con un mensaje como «`desde` (2026-03-31) no puede ser mayor que `hasta` (2026-03-01)».

## 6. Errores esperados

| Situación | Resultado |
|---|---|
| `desde > hasta` | `ArgumentException` con mensaje claro |
| `pedidos` nulo | `ArgumentNullException` |
| Workflow sin secreto | No comenta; se revisa el log en Actions y se anota |
| Workflow no corre en el PR | Se compara con la documentación de Claude Code GitHub Actions; CA-09 no se da por cumplido |

## 7. Fuera de alcance

- Exponer HU-03 en la API; cambiar `IPedidoRepositorio` o los pedidos de ejemplo.
- Pendientes de la U6: mutación de los 7 tests (CA-02), compuerta roja de `ci-local.ps1` (CA-05), falla con `@claude` (CA-07). Se anotan, no se ejecutan.
- Instalar `gh` u otro software; paquetes NuGet nuevos.
- Proteger la rama `main` con un ruleset (recomendado por la documentación; no se configura aquí).

## 8. Preguntas abiertas

Resueltas el 2026-11-12 por el PO:

- **Base de los PR:** `main` (ya trae `develop` y `claude.yml`).
- **Ubicación de `REVIEW.md`:** carpeta `unidad-7/taller/pedidos-web`. La documentación lo pone en la raíz del repo, para el Code Review gestionado (Team/Enterprise); aquí el workflow y el revisor apuntan a esa ruta.
- **`ci.yml`:** solo unidad-7 (unidad-6 deja de correr en CI).
- **Rama de HU-03:** `hu-03-pedidos-por-fecha` (la de la guía).
- **Pareja:** no hay. La etapa 3 se hace sobre el PR propio y la revisión por otra persona queda como **no verificada**.
- **Secreto:** `CLAUDE_CODE_OAUTH_TOKEN` está creado según el PO; no verificado por Claude.
- **Regla 9 de `AGENTS.md`:** ampliada para `.github/` y `REVIEW.md` solo en tareas de TEC-06.

Abierta: ninguna.

## 9. Historial de cambios

| Versión | Fecha | Cambio |
|---|---|---|
| 0.1 | 2026-11-12 | Borrador |
| 1.0 | 2026-11-12 | Aprobada por el PO; base `main`, decisiones de la sección 8 cerradas |
| 1.1 | 2026-11-12 | RN-09: elemento nulo en `pedidos` lanza `ArgumentException`; test de borde `desde == hasta` (revisión local de HU-03) |
