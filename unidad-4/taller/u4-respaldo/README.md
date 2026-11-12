# u4-respaldo

Plan B de la **etapa 1 (Entender)** del taller de la Unidad 4: un `inventario.md` y un `dependencias.md` de referencia de Pedidos Legado, para que puedas saltar a la **etapa 2 (Encontrar)** si la etapa 1 no quedó lista a tiempo. **No es una rama de Git**: es una carpeta de este repositorio.

Úsala solo si te trabaste. Hacer tu propio inventario y tu propio diagrama es parte del aprendizaje.

## Qué trae

```
u4-respaldo/
└── docs/diagnostico/
    ├── inventario.md      ← todos los archivos de legado/src/, con su responsabilidad y líneas
    └── dependencias.md    ← diagrama Mermaid de quién usa a quién, con la base de datos
```

No trae reglas, riesgos ni matriz: eso lo construyes tú en las etapas 2 y 3.

## Cómo usarla

Lo más simple es pedírselo a Claude Code, abierto en tu `pedidos-web`:

> Copia inventario.md y dependencias.md de la carpeta unidad-4/taller/u4-respaldo/ del repo de material a docs/diagnostico/ de este repo. No toques nada más.

Si prefieres hacerlo tú, desde la carpeta que contiene tu `pedidos-web` y el material, uno al lado del otro:

```bash
# macOS, Linux o WSL
R4=material-curso-ia-desarrolladores/unidad-4/taller/u4-respaldo/docs/diagnostico
cp $R4/inventario.md $R4/dependencias.md pedidos-web/docs/diagnostico/
```
```powershell
# Windows PowerShell
$R4 = "material-curso-ia-desarrolladores/unidad-4/taller/u4-respaldo/docs/diagnostico"
Copy-Item "$R4/inventario.md", "$R4/dependencias.md" pedidos-web/docs/diagnostico/
```

Estos dos archivos reemplazan las plantillas vacías del mismo nombre. Después sigue con la etapa 2.
