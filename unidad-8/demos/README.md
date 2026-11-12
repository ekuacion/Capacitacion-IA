# Demos de la Unidad 8 · ArriendoWeb

Las tres demos de la unidad ocurren sobre **un solo proyecto**, `arriendo-web/`, y siguen la historia de las demos de la Unidad 7: *ArriendoWeb ya tiene CI y reglas de revisión. Ahora, ¿qué puede leer y tocar el agente?*

`arriendo-web` es el **estado de la U7 con las reglas de revisión ya incorporadas**: `TarifaBase` migrada (20 de 20 casos del golden master), el hook `PostToolUse` de la U5, `ci.yml` y `claude.yml` (U6), y `REVIEW.md` con el subagente `revisor` (U7). Lo que se construye en vivo **no** viene incluido: se pega desde `insumos/`.

Historia: **1 Una regla → 2 Un guardián → 3 La política en el repo**

| Demo | Lámina | Concepto | Qué se hace |
|---|---|---|---|
| 1 · Una regla no basta | 11 | Mínimo privilegio | Se suma `"deny": ["Read(./.env)"]` a `.claude/settings.json`. «Muéstrame el contenido de .env» se bloquea; «Busca DB_PASSWORD en todo el proyecto con grep -r» se filtra. |
| 2 · Un guardián que no se olvida | 14 | Mínimo privilegio | Se copia `insumos/guardian.sh` a `.claude/hooks/` y se registra como hook `PreToolUse`. «Ejecuta grep DB_PASSWORD .env» se bloquea; el `grep -r` sigue pasando. |
| 3 · La política vive en el repo | 21 | Política ejecutable | `deny` sobre `legado/` y `ask` sobre `migracion/tarifas-golden.csv`, `curl` y `WebFetch`. Se revisa con `/permissions` y se intenta violar cada regla. |

> El paso a paso completo (bloques para pegar, prompts exactos, resultados esperados, commits y puntos de restauración) está en las notas del instructor de la Unidad 8.

## Qué hay aquí

```
demos/
├── arriendo-web/                      ← proyecto de las 3 demos (estado inicial)
│   ├── AGENTS.md
│   ├── REVIEW.md                      ← U7
│   ├── ArriendoWeb.sln
│   ├── .claude/                       ← hook PostToolUse (U5), analista-legado y revisor (U7)
│   ├── .github/workflows/             ← ci.yml y claude.yml (U6)
│   ├── legado/                        ← solo lectura
│   ├── migracion/tarifas-golden.csv
│   ├── src/ArriendoWeb.Core/Tarifas/Tarifas.cs
│   └── tests/ArriendoWeb.Tests/Tarifas/TarifaBaseGoldenTests.cs
└── insumos/                           ← se pegan en vivo
    ├── env-falso.txt                  → raíz de arriendo-web, con el nombre .env   (antes de la demo 1)
    └── guardian.sh                    → .claude/hooks/                             (demo 2)
```

`env-falso.txt` trae **solo valores inventados**. Se guarda con otro nombre para que este repo público no tenga un `.env`.

## Cómo prepararlas

1. Copia la carpeta `arriendo-web/` a una carpeta de trabajo **fuera** de tu clon de este repo, para que tenga su propio historial de Git.
2. Ábrela en Rider con `ArriendoWeb.sln` y compila. En la terminal, `dotnet test` debe dar **20 tests en verde**.
3. Crea el repositorio y el primer commit en `main`:

   ```bash
   git init -b main
   git add .
   git commit -m "chore: estado inicial de arriendo-web para la U8"
   ```

4. Copia `insumos/env-falso.txt` a la raíz de tu copia con el nombre `.env`. `git status` no debe mostrarlo: lo ignora `.gitignore`.
5. Abre Claude Code **desde la raíz** del proyecto. Después de cada cambio en `.claude/settings.json`, sal (`/exit`), vuelve a abrir y confírmalo con `/permissions` o `/hooks`.
6. Ensaya las tres demos y anota tus resultados. Los puntos de restauración (los commits de las demos 1, 2 y 3) los creas tú al ensayar.
7. Para repetir desde cero, borra la carpeta de trabajo y vuelve a copiarla desde aquí.

Las demos son **locales**: no necesitan push, GitHub Actions ni token.

## Regla de las demos

Las reglas y el guardián se **pegan a mano**, no se le piden a Claude: así la configuración es siempre la misma y la demo es repetible. Usa solo el `.env` falso: nunca hagas estas demos en una carpeta con secretos reales.

## Licencias

- Claude Pro (incluye Claude Code). Permisos y hooks funcionan en cualquier plan.
- JetBrains Rider y .NET 10 SDK.
- `guardian.sh` es bash: en Windows, Claude Code corre los hooks con Git Bash. *(Verificar en tu equipo: el guardián solo se probó en bash de Linux; en Windows no está probado.)*
- No se necesita licencia de Delphi ni de DBISAM: el legado no se compila.

*(Verificar en tu equipo: el C# no fue compilado al preparar este material.)*
