# Demos de la Unidad 5 · ArriendoWeb

Las tres demos de la unidad ocurren sobre **un solo proyecto**, `arriendo-web/`, y cuentan una historia seguida: migrar `TarifaBase` de `UTarifas.pas` a C# sin cambiar un peso. **Congelar → Traducir → Verificar.**

`arriendo-web` es el `arriendo-legado` de las demos de la Unidad 4 (ahora en `legado/`, solo lectura) más una solución .NET, `ArriendoWeb`, que empieza vacía.

| Demo | Lámina | Concepto | Qué se hace |
|---|---|---|---|
| 1 · Congelar la tarifa | 13 | Congela el comportamiento | Claude escribe un `[Theory]` que lee `migracion/tarifas-golden.csv`. Sin implementación: los 20 casos quedan en rojo. |
| 2 · Traducir no es copiar | 17 | Traducir no es copiar | Claude convierte `TarifaBase` a C#. Con el redondeo «de siempre» falla el caso A08; con el redondeo de Delphi (al par) quedan los 20 en verde. |
| 3 · El hook que no perdona | 20 | Verificación automática | Un hook `PostToolUse` corre `dotnet test` después de cada edición de Claude. Si un cambio rompe un caso, Claude recibe el error y corrige. |

La actividad «Traduce el tipo» (lámina 16) no usa el proyecto.

> El paso a paso completo (prompts exactos, resultados esperados y commits) está en las notas del instructor de la Unidad 5.

## Qué hay aquí

```
demos/
└── arriendo-web/
    ├── AGENTS.md                  ← reglas del proyecto (dinero en decimal, el CSV no se edita)
    ├── ArriendoWeb.sln
    ├── .claude/agents/
    │   └── analista-legado.md     ← el subagente de solo lectura de la U4
    ├── legado/                    ← arriendo-legado (Delphi 7 + DBISAM 4), solo lectura
    ├── migracion/
    │   ├── README.md              ← qué es cada columna
    │   └── tarifas-golden.csv     ← 20 casos de TarifaBase
    ├── src/ArriendoWeb.Core/      ← vacío: el C# se escribe en la demo 2
    └── tests/ArriendoWeb.Tests/   ← xUnit; el .csproj ya copia el CSV a la salida
```

No trae `.claude/settings.json`: el hook se crea en la demo 3.

## Cómo prepararlas

1. Copia la carpeta `arriendo-web/` a una carpeta de trabajo **fuera** de tu clon de este repo, para que tenga su propio historial de Git.
2. Ábrela en Rider con `ArriendoWeb.sln` y compila. En la terminal, `dotnet test` compila y no encuentra tests todavía.
3. Crea el repositorio y el primer commit en la rama `main`:

   ```bash
   git init -b main
   git add .
   git commit -m "chore: estado inicial de arriendo-web para la U5"
   ```

4. Para repetir las demos desde cero, borra la carpeta de trabajo y vuelve a copiarla desde aquí.

No se usan ramas: las tres demos ocurren seguidas sobre `main`.

## El hook de la demo 3

Se pega a mano en `.claude/settings.json` y luego se reinicia Claude Code (compruébalo con `/hooks`):

```json
{ "hooks": { "PostToolUse": [ { "matcher": "Edit|Write", "hooks": [
  { "type": "command", "command": "cd \"$CLAUDE_PROJECT_DIR\" && dotnet test --nologo 1>&2 || exit 2" } ] } ] } }
```

*(Verificar en tu equipo: el comando usa sintaxis de bash; en Windows no está probado.)*

## Licencias

- Claude Team Standard (Claude Code se autentica con la cuenta Team).
- JetBrains All Products Pack (Rider) y .NET 10 SDK.
- No se necesita licencia de Delphi ni de DBISAM: el legado no se compila.
