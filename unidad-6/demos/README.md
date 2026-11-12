# Demos de la Unidad 6 · ArriendoWeb

Las cuatro demos de la unidad ocurren sobre **un solo proyecto**, `arriendo-web/`, y siguen la historia de las demos de la Unidad 5: *ArriendoWeb ya calcula bien la tarifa. ¿Cómo sabemos que no se rompe cuando nadie mira?*

`arriendo-web` es el **estado final de las demos de la U5**: `TarifaBase` migrada a C#, 20 de 20 casos del golden master en verde y el hook que corre los tests después de cada edición de Claude. Además trae `claude.yml`, que se usa en la demo 4.

| Demo | Lámina | Concepto | Qué se hace |
|---|---|---|---|
| 1 · El test mentiroso | 7 | Tests con criterio | Claude Code propone tests de borde para `TarifaBase`. El instructor rompe `FactorTemporada` **a mano** en Rider: el test que sigue en verde es el mentiroso. |
| 2 · Cobertura 100 %, verificación 0 | 10 | Cobertura ≠ calidad | Claude suma `coverlet.collector` y lee el XML de cobertura: el test mentiroso cubre todo y no verifica nada. |
| 3 · Mi primer workflow | 14 | Pipeline seguro | Claude Code escribe `.github/workflows/ci.yml`; push al repo público y verde en Actions. |
| 4 · @claude, ¿por qué falla? | 22 | Claude en CI | Un PR vuelve el redondeo a `AwayFromZero`: solo A08 queda en rojo. `@claude` diagnostica; el instructor corrige y hace merge en verde. |

> El paso a paso completo (prompts exactos, resultados esperados, commits y contingencias) está en las notas del instructor de la Unidad 6.

## Qué hay aquí

```
demos/
└── arriendo-web/
    ├── AGENTS.md
    ├── ArriendoWeb.sln
    ├── .claude/
    │   ├── settings.json              ← hook PostToolUse de la U5 (corre dotnet test)
    │   └── agents/analista-legado.md
    ├── .github/workflows/
    │   └── claude.yml                 ← @claude en issues y PR (demo 4)
    ├── legado/                        ← arriendo-legado, solo lectura
    ├── migracion/tarifas-golden.csv   ← 20 casos de TarifaBase
    ├── src/ArriendoWeb.Core/Tarifas/
    │   └── Tarifas.cs                 ← TarifaBase migrada (redondeo al par)
    └── tests/ArriendoWeb.Tests/Tarifas/
        └── TarifaBaseGoldenTests.cs   ← un caso por fila del CSV
```

No trae `ci.yml`: se crea en la demo 3.

## Cómo prepararlas

1. Copia la carpeta `arriendo-web/` a una carpeta de trabajo **fuera** de tu clon de este repo, para que tenga su propio historial de Git.
2. Ábrela en Rider con `ArriendoWeb.sln` y compila. En la terminal, `dotnet test` debe dar **20 tests en verde**.
3. Crea el repositorio y el primer commit en la rama `main`:

   ```bash
   git init -b main
   git add .
   git commit -m "chore: estado inicial de arriendo-web para la U6"
   ```

4. Crea en GitHub un repositorio **público** vacío llamado `arriendo-web` y sube `main`:

   ```bash
   git remote add origin https://github.com/TU-USUARIO/arriendo-web.git
   git push -u origin main
   ```

5. Para la demo 4: instala la [Claude GitHub App](https://github.com/apps/claude) solo en ese repo, genera tu token con `claude setup-token` y guárdalo en *Settings › Secrets and variables › Actions* como `CLAUDE_CODE_OAUTH_TOKEN`.
6. Para repetir las demos desde cero, borra la carpeta de trabajo y vuelve a copiarla desde aquí.

No hay ramas preparadas: las demos 1 a 3 ocurren sobre `main`, y la demo 4 crea en vivo la rama de su PR.

## Regla de las demos

Las fallas se introducen **a mano en Rider**, nunca pidiéndoselas a Claude: el hook de la U5 le devolvería el error y Claude lo «arreglaría».

## Licencias

- Claude Pro (incluye Claude Code). El token de la demo 4 gasta la cuota de tu suscripción.
- JetBrains Rider y .NET 10 SDK. GitHub gratis: en repos públicos, los runners estándar de Actions no tienen costo.
- La demo 2 necesita acceso a NuGet para instalar `coverlet.collector`.
- No se necesita licencia de Delphi ni de DBISAM: el legado no se compila.

*(Verificar en tu equipo: el C# no fue compilado al preparar este material; la lógica se validó con una réplica en decimales, 20/20 contra el golden master. El hook usa sintaxis de bash; en Windows no está probado.)*
