# u6-respaldo

Plan B de la **etapa 2 (Pipeline seguro)** del taller de la Unidad 6: un `claude.yml` listo, basado en el ejemplo oficial «Respond to @claude mentions» de la documentación de Claude Code. **No es una rama de Git**: es una carpeta de este repositorio.

Úsala solo si el `claude.yml` que escribiste con Claude Code no funciona. El `ci.yml` no viene aquí: escribirlo es parte del aprendizaje.

## Qué trae

```
u6-respaldo/
└── .github/workflows/
    └── claude.yml     ← modo interactivo: responde a @claude en issues y PR
```

Usa el secreto `CLAUDE_CODE_OAUTH_TOKEN` (tu Claude Pro), `--max-turns 5`, `timeout-minutes: 10` y `concurrency`. Le quita a Claude las herramientas de edición: **Claude diagnostica; tú corriges y haces merge con el CI en verde.**

## Cómo usarla

Lo más simple es pedírselo a Claude Code, abierto en tu `pedidos-web`:

> Copia .github/workflows/claude.yml de la carpeta unidad-6/taller/u6-respaldo/ del repo de material a la misma ruta de este repo. Si ya tengo uno, reemplaza solo ese archivo. No toques nada más.

Si prefieres hacerlo tú, desde la carpeta que contiene tu `pedidos-web` y el material, uno al lado del otro:

```bash
# macOS, Linux o WSL
R6=material-curso-ia-desarrolladores/unidad-6/taller/u6-respaldo
mkdir -p pedidos-web/.github/workflows
cp $R6/.github/workflows/claude.yml pedidos-web/.github/workflows/
```

```powershell
# Windows PowerShell
$R6 = "material-curso-ia-desarrolladores/unidad-6/taller/u6-respaldo"
New-Item -ItemType Directory -Force pedidos-web/.github/workflows | Out-Null
Copy-Item "$R6/.github/workflows/claude.yml" pedidos-web/.github/workflows/
```

Después: commit y push a `main`. Los comentarios con `@claude` usan el workflow de la rama por defecto.

**Antes de que funcione** necesitas la [Claude GitHub App](https://github.com/apps/claude) instalada en tu repo y el secreto `CLAUDE_CODE_OAUTH_TOKEN` (*Settings › Secrets and variables › Actions*), generado con `claude setup-token`.

**Resultado esperado:** al comentar `@claude ¿por qué falla este job? Solo diagnostica, no hagas commits.` en tu PR, aparece un run «Claude» en la pestaña Actions y Claude responde con un comentario en el mismo PR.

Si aun así no responde, sigue la etapa 3 en local: `dotnet test 2>&1 | claude -p "Explica en 3 líneas por qué falla"`.
