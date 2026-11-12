# Pedidos Web

Portal web de pedidos de la distribuidora. Es la modernización de **Pedidos Legado**, un sistema de escritorio hecho en Delphi con DBISAM que la empresa usa hace 18 años.

Este repositorio es el **caso hilo** del curso *Desarrollando con IA* (Vela LATAM). Crece unidad a unidad: la biblioteca de dominio (Unidad 1), una API en ASP.NET Core con 45 pedidos de ejemplo (Unidad 2), las líneas de cada pedido y un `ClientesService` heredado (Unidad 3), el código de Pedidos Legado para diagnosticarlo sin tocarlo (Unidad 4), el golden master para migrar el cálculo de totales (Unidad 5), el pipeline de CI con Claude diagnosticando las fallas (Unidad 6) y, en la **Unidad 7**, el **proceso de code review**: reglas escritas, revisores de IA y una persona que decide.

> Todo el contenido es ficticio y tiene fines educativos.

## Stack

| Pieza | Versión | Para qué |
|---|---|---|
| .NET SDK | 10 | Compilar y ejecutar |
| C# | el que trae el SDK | Lenguaje del proyecto |
| xUnit | 2.9 | Tests |
| JetBrains Rider | la versión vigente | IDE del curso |
| Claude Code | 2.1.277 o superior, con tu Claude Pro | Agente de código (lee `AGENTS.md`) |
| GitHub Actions | runners estándar (gratis en repos públicos) | CI y revisión en cada PR |
| Delphi 7 + DBISAM 4 | solo lectura · no se instala | El sistema legado que migras por partes |

## Estructura

```
pedidos-web/
├── AGENTS.md                         ← el tuyo (lo escribiste en la U2)
├── REVIEW.md                         ← U7: lo creas tú en la etapa 1
├── PedidosWeb.sln
├── .claude/                          ← U4: regla deny y subagente (U5: el hook)
│   └── agents/revisor.md             ← U7: lo creas tú en la etapa 1
├── .github/
│   ├── workflows/                    ← U6: ci.yml y claude.yml · U7: claude-review.yml (etapa 1)
│   └── pull_request_template.md      ← U7: tu reto
├── src/
│   ├── PedidosWeb.Core/              ← lógica de dominio (U7: HU-03 en Pedidos/)
│   └── PedidosWeb.Api/               ← U2: API REST (Minimal API)
├── tests/
│   └── PedidosWeb.Core.Tests/
├── legado/                           ← U4: código de Pedidos Legado, solo lectura
├── migracion/                        ← U5: el golden master
├── docs/
│   ├── diagnostico/                  ← U4
│   ├── migracion/                    ← U5
│   ├── calidad/                      ← U6
│   └── checklist-revision.md         ← U7: plantilla                                      (nuevo)
└── specs/
    └── HU-03-pedidos-por-fecha/      ← U7: la historia del PR de la etapa 2               (nuevo)
        └── spec.md
```

## Cómo empezar la Unidad 7

### A. Ya tienes tu repo `pedidos-web` (lo normal)

**No copies la carpeta completa**: pisarías tu `README.md`, tu `AGENTS.md`, tu código y tu trabajo de las unidades anteriores. La U7 solo **suma** dos cosas: `specs/HU-03-pedidos-por-fecha/` y `docs/checklist-revision.md`. No reemplaza nada.

La forma recomendada es pedírselo a Claude Code, abierto en tu `pedidos-web`:

> Trae a pedidos-web solo lo nuevo de la Unidad 7 desde el repo de material. No toques nada más.

Si prefieres hacerlo tú, trabaja en la carpeta que contiene tu `pedidos-web` y el material del curso, uno al lado del otro:

```bash
# macOS, Linux o WSL
cd material-curso-ia-desarrolladores && git pull && cd ..
U7=material-curso-ia-desarrolladores/unidad-7/taller/pedidos-web
mkdir -p pedidos-web/specs
cp -r $U7/specs/HU-03-pedidos-por-fecha pedidos-web/specs/
cp $U7/docs/checklist-revision.md pedidos-web/docs/
```

```powershell
# Windows PowerShell
cd material-curso-ia-desarrolladores; git pull; cd ..
$U7 = "material-curso-ia-desarrolladores/unidad-7/taller/pedidos-web"
New-Item -ItemType Directory -Force pedidos-web/specs | Out-Null
Copy-Item -Recurse "$U7/specs/HU-03-pedidos-por-fecha" pedidos-web/specs/
Copy-Item "$U7/docs/checklist-revision.md" pedidos-web/docs/
```

**No se copian:** `README.md`, `AGENTS.md`, `PedidosWeb.sln`, `src/`, `tests/`, `legado/`, `migracion/`, `.claude/`, `.github/`, el resto de `docs/` ni el resto de `specs/`.

### Preparación (9 min)

```bash
cd pedidos-web
claude --version     # 2.1.277 o superior: así Claude Code lee tu AGENTS.md
dotnet test
git add .
git commit -m "chore: base de la Unidad 7 (HU-03 y checklist de revisión)"
git push
```

**Resultado esperado:** `dotnet test` da el mismo resultado que al terminar la U6 (la preparación no suma código C#) y el CI de tu repo sigue en verde. El secreto `CLAUDE_CODE_OAUTH_TOKEN` de la U6 sigue guardado en *Settings › Secrets and variables › Actions*.

Si `claude --version` es menor: `claude update`.

### B. No tienes tu repo de las unidades anteriores

Sigue el paso 1 del README de `unidad-1/taller/pedidos-web` (crear tu repo **público** en GitHub), pero copia esta carpeta (`unidad-7/taller/pedidos-web`) en vez de la de la Unidad 1. En este camino el `AGENTS.md` viene vacío y no hay workflows: antes del taller haz la etapa 2 de la U6 (`ci.yml`, `claude.yml` y el secreto), según el README de `unidad-6/taller/pedidos-web`. Si algún test falla porque esa parte no está terminada, márcalo con `Skip` y un motivo; no lo borres.

---

## Taller de la Unidad 7: lo que encuentras en el repo

La misión: **que cada cambio llegue a `main` revisado con reglas escritas, por la IA y por una persona.** Trabajo individual en tu repo; la etapa 3 es en parejas. 94 minutos.

| Etapa | Min | Dónde queda tu trabajo |
|---|---|---|
| Preparación | 9 | `specs/HU-03-pedidos-por-fecha/` y `docs/checklist-revision.md` en tu repo |
| 1 · Reglas escritas | 25 | `REVIEW.md`, `.claude/agents/revisor.md` y `.github/workflows/claude-review.yml` en `main` |
| 2 · Un PR que se puede revisar | 25 | El PR de HU-03 desde la rama `hu-03-pedidos-por-fecha`, con descripción y comentarios de Claude |
| 3 · Revisión cruzada | 20 | Cada hallazgo respondido, 1 hallazgo humano, `docs/checklist-revision.md` completo y merge en verde |
| Tu reto (obligatorio) | 15 | `.github/pull_request_template.md` en `main` |

La guía del taller, con todo el recorrido, está en el [sitio del curso](https://ffelipecuevasc.github.io/curso-ia-desarrolladores/) → Unidad 7.

### Comandos de cada etapa

**Etapa 1 · Reglas escritas.** Claude Code crea los tres archivos (el pedido está en la guía). Tú los revisas y los subes a `main`:

```bash
git add REVIEW.md .claude/agents/revisor.md .github/workflows/claude-review.yml
git commit -m "chore: reglas de revisión, subagente revisor y workflow de revisión"
git push
```

Si `.claude/agents/` es una carpeta nueva, **reinicia Claude Code** para que cargue el subagente. Pruébalo: «Usa el subagente revisor sobre esta rama».

**Etapa 2 · Un PR que se puede revisar.**

```bash
git switch -c hu-03-pedidos-por-fecha
# Claude Code implementa specs/HU-03-pedidos-por-fecha/spec.md con su test
dotnet test
git add .
git commit -m "feat: filtra pedidos por rango de fechas (HU-03)"
git push -u origin hu-03-pedidos-por-fecha
```

Antes de abrir el PR, corre el revisor: «Usa el subagente revisor sobre esta rama». Luego abre el PR en GitHub (botón *Compare & pull request*) o con `gh pr create --fill` si tienes GitHub CLI, y pega la descripción que redactó Claude, ya completada por ti. Al abrirse el PR, `claude-review.yml` corre y deja comentarios: míralos en la pestaña *Files changed*.

**Etapa 3 · Revisión cruzada.** El instructor arma las parejas al empezar esta etapa. Abre el PR de tu pareja en su repo público y responde en cada hilo de la IA:

- `Acepto: <qué se va a cambiar>`
- `Discutir: <de qué depende>`
- `Descarto: <por qué no aplica, con archivo:línea>`

Agrega al menos un hallazgo humano. Luego, en tu PR, aplica lo que aceptaste, completa `docs/checklist-revision.md` y haz merge con el CI en verde:

```bash
git add .
git commit -m "fix: aplica lo acordado en la revisión de HU-03"
git push
# Con el CI en verde: Merge pull request en GitHub (o gh pr merge)
git switch main && git pull
```

### Etapa extra obligatoria · Tu reto (15 min)

**Tema nuevo:** plantilla de PR en GitHub.

**Tu reto:** crea `.github/pull_request_template.md` para que todo PR de tu repo nazca revisable. Esta vez el pedido a Claude Code lo escribes tú.

**Cómo pensarlo:**
1. ¿Qué secciones tuviste que escribir hoy en tu descripción?
2. ¿Qué debería confirmar el autor antes de pedir revisión?
3. ¿Cómo compruebas que GitHub de verdad la usa?

**Listo cuando:** el archivo está en `main` · un PR nuevo muestra sus secciones · incluye al menos una casilla para confirmar que corriste el revisor.

Si no alcanzas en clase, termínalo en tu casa: es parte del entregable.

### Reglas del taller

- **Las reglas se leen desde `main`.** `REVIEW.md` tiene que estar en `main` antes de abrir el PR de HU-03, para que un PR no pueda aflojar las reglas que lo revisan.
- **El token nunca va en un archivo.** `claude-review.yml` usa el secreto `CLAUDE_CODE_OAUTH_TOKEN` de la U6.
- **La IA comenta; tú decides.** Ningún hallazgo se acepta sin leerlo, y descartar vale si lo argumentas.
- **Cuida tu cuota.** La revisión automática corre al abrir o reabrir el PR y al marcarlo listo, no en cada push. Para revisar de nuevo, usa el revisor en local.

### Si algo falla

- **El revisor no aparece:** pídele a Claude Code que revise si `.claude/agents/revisor.md` está bien escrito y reinicia Claude Code.
- **El workflow corre, pero no comenta:** pídele a Claude Code que compare tu `claude-review.yml` con la documentación de [Claude Code GitHub Actions](https://code.claude.com/docs/en/github-actions). Mira el log del job en la pestaña *Actions*.
- **Rompiste algo que funcionaba:** vuelve al último commit que estaba en verde y repite el paso.

### La plantilla (`docs/checklist-revision.md`)

| Plantilla | Etapa | Qué completas |
|---|---|---|
| `checklist-revision.md` | 3 | Qué revisas antes de pedir revisión, al revisar, con los hallazgos de la IA y antes del merge, más el enlace a tu PR y tu hallazgo humano |

## Licencias

- Claude Pro (incluye Claude Code). La revisión en GitHub Actions usa tu token y gasta tu cuota.
- JetBrains Rider y .NET 10 SDK.
- GitHub gratis con tu repo **público**: los runners estándar de Actions no tienen costo.
- No se necesita Claude Team: el Code Review gestionado de Anthropic (Team y Enterprise) es solo mención en la clase.
- No se necesita licencia de Delphi ni de DBISAM: el legado no se compila.

## Talleres anteriores

- **Unidad 1** (`FormatearFolio()` y `docs/mapa-sdlc.md`): ver el README de `unidad-1/taller/pedidos-web`.
- **Unidad 2** (API, 45 pedidos y HU-01 listar pedidos): ver el README de `unidad-2/taller/pedidos-web`.
- **Unidad 3** (HU-02 detalle del pedido, refactor de `ClientesService` y Skill `documentar-modulo`): ver el README de `unidad-3/taller/pedidos-web`.
- **Unidad 4** (diagnóstico de Pedidos Legado): ver el README de `unidad-4/taller/pedidos-web`.
- **Unidad 5** (migración del cálculo de totales con golden master): ver el README de `unidad-5/taller/pedidos-web`.
- **Unidad 6** (tests que sí prueban, `ci.yml`, `claude.yml` y la falla diagnosticada por `@claude`): ver el README de `unidad-6/taller/pedidos-web`.
