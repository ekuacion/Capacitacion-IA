# u3-respaldo

Plan B de la **etapa 1 (Construir)** del taller de la Unidad 3: una spec de referencia de la HU-02, por si la tuya no queda lista a tiempo. **No es una rama de Git**: es una carpeta de este repositorio.

Úsala solo si te trabaste. Escribir tu propia spec es parte del aprendizaje.

## Cómo usarla

Lo más simple es pedírselo a Claude Code, abierto en tu `pedidos-web`:

> Copia la spec de respaldo de la Unidad 3 (`unidad-3/taller/u3-respaldo/specs/HU-02-detalle-pedido/spec.md` del repo de material) a `specs/HU-02-detalle-pedido/` de este repo. No toques nada más.

Si prefieres hacerlo tú, desde la carpeta que contiene tu `pedidos-web` y el material, uno al lado del otro:

```bash
# macOS, Linux o WSL
mkdir -p pedidos-web/specs/HU-02-detalle-pedido
cp material-curso-ia-desarrolladores/unidad-3/taller/u3-respaldo/specs/HU-02-detalle-pedido/spec.md pedidos-web/specs/HU-02-detalle-pedido/
```
```powershell
# Windows PowerShell
New-Item -ItemType Directory -Force pedidos-web/specs/HU-02-detalle-pedido | Out-Null
Copy-Item material-curso-ia-desarrolladores/unidad-3/taller/u3-respaldo/specs/HU-02-detalle-pedido/spec.md pedidos-web/specs/HU-02-detalle-pedido/
```

Después sigue con los 3 incrementos de la etapa 1.
