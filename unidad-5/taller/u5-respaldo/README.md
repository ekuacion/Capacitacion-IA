# u5-respaldo

Plan B de la **etapa 1 (Congelar)** del taller de la Unidad 5: el lector del golden master y un `CalculoTotales` vacío, para que puedas saltar a la **etapa 2 (Migrar)** si la etapa 1 no quedó lista a tiempo. **No es una rama de Git**: es una carpeta de este repositorio.

Úsala solo si te trabaste. Pedirle a Claude Code tus propios tests es parte del aprendizaje.

## Qué trae

```
u5-respaldo/
├── src/PedidosWeb.Core/Totales/
│   └── CalculoTotales.cs          ← los records y la firma de Calcular (lanza NotImplementedException)
└── tests/PedidosWeb.Core.Tests/Totales/
    └── GoldenMasterTests.cs       ← un [Theory] por caso de migracion/golden-master.csv
```

No trae la implementación del cálculo: eso lo migras tú en la etapa 2.

## Cómo usarla

Lo más simple es pedírselo a Claude Code, abierto en tu `pedidos-web`:

> Copia CalculoTotales.cs y GoldenMasterTests.cs de la carpeta unidad-5/taller/u5-respaldo/ del repo de material a las mismas rutas de este repo, y haz que el proyecto de tests copie migracion/golden-master.csv a su carpeta de salida. No toques nada más.

Si prefieres hacerlo tú, desde la carpeta que contiene tu `pedidos-web` y el material, uno al lado del otro:

```bash
# macOS, Linux o WSL
R5=material-curso-ia-desarrolladores/unidad-5/taller/u5-respaldo
mkdir -p pedidos-web/src/PedidosWeb.Core/Totales pedidos-web/tests/PedidosWeb.Core.Tests/Totales
cp $R5/src/PedidosWeb.Core/Totales/CalculoTotales.cs pedidos-web/src/PedidosWeb.Core/Totales/
cp $R5/tests/PedidosWeb.Core.Tests/Totales/GoldenMasterTests.cs pedidos-web/tests/PedidosWeb.Core.Tests/Totales/
```

```powershell
# Windows PowerShell
$R5 = "material-curso-ia-desarrolladores/unidad-5/taller/u5-respaldo"
New-Item -ItemType Directory -Force pedidos-web/src/PedidosWeb.Core/Totales, pedidos-web/tests/PedidosWeb.Core.Tests/Totales | Out-Null
Copy-Item "$R5/src/PedidosWeb.Core/Totales/CalculoTotales.cs" pedidos-web/src/PedidosWeb.Core/Totales/
Copy-Item "$R5/tests/PedidosWeb.Core.Tests/Totales/GoldenMasterTests.cs" pedidos-web/tests/PedidosWeb.Core.Tests/Totales/
```

Y agrega esto a `tests/PedidosWeb.Core.Tests/PedidosWeb.Core.Tests.csproj`, para que el CSV llegue a la carpeta de salida:

```xml
<ItemGroup>
  <None Include="..\..\migracion\golden-master.csv" Link="golden-master.csv" CopyToOutputDirectory="PreserveNewest" />
</ItemGroup>
```

**Resultado esperado:** `dotnet test` muestra **60 tests de `GoldenMasterTests` en rojo**, todos por `NotImplementedException`. Haz tu commit y sigue en la etapa 2.
