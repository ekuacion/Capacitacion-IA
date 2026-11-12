# Demos de la Unidad 7 · ArriendoWeb

Las tres demos de la unidad ocurren sobre **un solo proyecto**, `arriendo-web/`, y siguen la historia de las demos de la Unidad 6: *ArriendoWeb ya tiene CI. Ahora, ¿quién revisa los cambios y con qué reglas?*

`arriendo-web` es el **estado final de las demos de la U6**: `TarifaBase` migrada (20 de 20 casos del golden master), el hook de la U5, `ci.yml` y `claude.yml`. Lo que se construye en vivo **no** viene incluido: se pega desde `insumos/`.

Historia: **1 Agregar → 2 Revisar → 3 Reglas**

| Demo | Lámina | Concepto | Qué se hace |
|---|---|---|---|
| 1 · Un PR que se puede revisar | 7 | PR chico | Rama `feature/recargo-atraso`. Se pegan `RecargoAtraso.cs` y `RecargoAtrasoTests.cs`, commit, y Claude redacta la descripción del PR con `[COMPLETAR]` en el «por qué». |
| 2 · La IA comenta, tú decides | 11 | La IA comenta, tú decides | `/code-review` sobre la rama. El curso vota cada hallazgo: aceptar, discutir o descartar. Sin commit. |
| 3 · Reglas escritas y tu revisor | 15 | Reglas escritas | Se pegan `REVIEW.md` y `revisor.md`, se reinicia Claude Code y se usa el subagente `revisor` sobre la rama. Se compara con la demo 2. |

> El paso a paso completo (prompts exactos, resultados esperados, commits y puntos de restauración) está en las notas del instructor de la Unidad 7.

## Qué hay aquí

```
demos/
├── arriendo-web/                      ← proyecto de las 3 demos (estado inicial)
│   ├── AGENTS.md
│   ├── ArriendoWeb.sln
│   ├── .claude/                       ← hook de la U5 y subagente analista-legado
│   ├── .github/workflows/             ← ci.yml y claude.yml (U6)
│   ├── legado/                        ← solo lectura
│   ├── migracion/tarifas-golden.csv
│   ├── src/ArriendoWeb.Core/Tarifas/Tarifas.cs
│   └── tests/ArriendoWeb.Tests/Tarifas/TarifaBaseGoldenTests.cs
└── insumos/                           ← se pegan en vivo
    ├── RecargoAtraso.cs               → src/ArriendoWeb.Core/Tarifas/      (demo 1)
    ├── RecargoAtrasoTests.cs          → tests/ArriendoWeb.Tests/Tarifas/   (demo 1)
    ├── REVIEW.md                      → raíz de arriendo-web               (demo 3)
    └── revisor.md                     → .claude/agents/                    (demo 3)
```

## Cómo prepararlas

1. Copia la carpeta `arriendo-web/` a una carpeta de trabajo **fuera** de tu clon de este repo, para que tenga su propio historial de Git. Deja `insumos/` a mano.
2. Ábrela en Rider con `ArriendoWeb.sln` y compila. En la terminal, `dotnet test` debe dar **20 tests en verde**.
3. Crea el repositorio y el primer commit en `main`:

   ```bash
   git init -b main
   git add .
   git commit -m "chore: estado inicial de arriendo-web para la U7"
   ```

4. Ensaya las tres demos y anota tus resultados. Los puntos de restauración (el commit de la demo 1 y el de la demo 3) los creas tú al ensayar.
5. Para repetir desde cero, borra la carpeta de trabajo y vuelve a copiarla desde aquí.

Las demos son **locales**: no necesitan push, GitHub Actions ni token.

## Regla de las demos

Los insumos se **pegan a mano**, no se le piden a Claude: así el código y sus defectos son siempre los mismos y la demo es repetible.

## Licencias

- Claude Pro (incluye Claude Code). `/code-review` funciona en cualquier plan y usa la cuota de tu sesión.
- JetBrains Rider y .NET 10 SDK.
- No se necesita licencia de Delphi ni de DBISAM: el legado no se compila.

*(Verificar en tu equipo: el C# no fue compilado al preparar este material.)*
