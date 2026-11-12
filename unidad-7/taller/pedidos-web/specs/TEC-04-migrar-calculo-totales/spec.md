# Spec: TEC-04 · Migrar el cálculo de totales a C#

> Versión: 1.0 · Estado: implementada (Claudio Ortiz Castillo, 2026-11-11; regla 1 de `AGENTS.md`)
> Intent de origen: `specs/TEC-04-migrar-calculo-totales/intent.md` · Autor del borrador: Claude Code

> Formato: adaptado de la spec de TEC-03. Las decisiones marcadas con **[Decidir]** son tuyas.

## Qué se entrega

| Archivo | Etapa | Contenido |
|---|---|---|
| `tests/PedidosWeb.Core.Tests/Totales/GoldenMasterTests.cs` | 1 | `[Theory]` que lee `migracion/golden-master.csv` y compara descuento, flete, IVA y total |
| `tests/PedidosWeb.Core.Tests/PedidosWeb.Core.Tests.csproj` | 1 | Copia el CSV a la carpeta de salida (`<None Include=... Link="golden-master.csv">`) |
| `src/PedidosWeb.Core/Totales/CalculoTotales.cs` | 1 → 2 | Etapa 1: solo la firma de `Calcular` (lanza `NotImplementedException`). Etapa 2: el cálculo |
| `.claude/settings.json` | 2 | Se **suma** el hook `PostToolUse` (`dotnet test`) y se conserva la regla `deny` |
| `migracion/golden-master.csv` | 3 | Solo cambian los casos afectados por R05 |
| `docs/migracion/defectos-corregidos.md` | 3 | R05 (corregido) y H1 (hallazgo, no corregido) |
| `docs/migracion/plan-migracion.md` | 3 | Etapas, convivencia, riesgos y criterio de corte, en una página |

## Reglas del cálculo (lo que hoy hace `UCalculoTotales.pas`)

- **RG-01 · Subtotal:** suma de cantidad × precio de todas las líneas (`CalcularSubtotal`).
- **RG-02 · Base del descuento:** suma de las líneas cuya categoría, sin espacios ni mayúsculas, no es `OFE` (`CalcularBaseDescuento`).
- **RG-03 · Porcentaje por volumen:** base > 1.000.000 → 8 %; base > 500.000 → 5 %; si no, 0 (`PorcentajeVolumen`). Es la versión de **pantalla** (`>`).
- **RG-04 · Porcentaje total:** volumen + 3 % si el cliente es VIP, con tope de 10 % (`PorcentajeDescuento`).
- **RG-05 · Descuento:** redondeo(base × porcentaje). Neto = subtotal − descuento (`CalcularTotales`).
- **RG-06 · Flete:** 0 si el pedido no tiene líneas; si no, redondeo(flete base × 1,15) en las regiones 11 y 12 y redondeo(flete base) en el resto (`CalcularFlete`).
- **RG-07 · IVA:** redondeo((neto + flete) × tasa) (`CalcularIva`). Total = neto + flete + IVA.
- **RG-08 · Redondeo (R05):** `Trunc(valor + 0,49)`: una fracción de 0,50 baja (`Calc3`). La etapa 2 lo replica; la etapa 3 lo corrige a «0,50 sube».

> Los números de línea se **verifican con `wc -l` y abriendo el archivo** al implementar y al documentar (regla 11). Aquí se cita
> por nombre de función hasta entonces. Las reglas ya catalogadas están en `docs/diagnostico/reglas-negocio.md` (#1, #5, #6, #7).

## Decisiones tomadas por Claude (a confirmar) **[Decidir]**

- **D-01 · Umbrales:** el C# replica la versión de pantalla (`>`), que es la que usa el golden master. La del grabado (`>=`,
  `UFrmPedidos.pas:296-301`) difiere en los 13 pedidos que caen justo en 500.000 o 1.000.000. Se registra como H1 y decide contabilidad.
- **D-02 · Lector del CSV sin paquetes:** `File.ReadAllLines` + `Split` con `CultureInfo.InvariantCulture`.
- **D-03 · Firma:** `CalculoTotales.Calcular(IReadOnlyList<LineaCalculo> lineas, bool esVip, int region, decimal fleteBase, decimal tasaIva)` devuelve un `record ResultadoTotales` (el esqueleto de `u5-respaldo` sirve de referencia).
- **D-04 · Hook en Windows:** el comando de la guía usa sintaxis bash y no está probado en Windows; se prueba antes de confiar en él.

## Criterios de aceptación

- **CA-01 · Contrato intacto.** Al cerrar las etapas 1 y 2, `git diff -- migracion/golden-master.csv` está vacío.
- **CA-02 · 60 en rojo.** Al cerrar la etapa 1 hay 60 casos de `GoldenMasterTests`, leídos del CSV, todos rojos por `NotImplementedException`.
- **CA-03 · Dinero en `decimal`.** `Totales/` no contiene `double` ni `float`.
- **CA-04 · Igual al legado.** Al cerrar la etapa 2, 60 de 60 en verde y la suma de la columna `total` es 58.202.135. Commit «igual al legado».
- **CA-05 · Hook activo.** `/hooks` lo muestra y una edición con un test en rojo devuelve el error a Claude.
- **CA-06 · R05 corregido aparte.** La corrección va en un commit propio: 13 casos del CSV cambian y la suma pasa a 58.202.144.
- **CA-07 · H1 registrado, no corregido.** `defectos-corregidos.md` describe la diferencia pantalla/grabado con `archivo:línea` y quién decide.
- **CA-08 · Documentos completos.** `defectos-corregidos.md` (R05 con casos afectados, antes/después y commit) y `plan-migracion.md` (etapas, convivencia, ≥ 3 riesgos, criterio de corte) sin comentarios de plantilla.
- **CA-09 · Nada más cambia.** `legado/` y `PedidosWeb.sln` sin cambios; los 83 tests previos siguen en verde.

## Fuera de alcance

Conectar el cálculo a la API, corregir H1, el pedido mínimo (30.000 frente a 25.000) y la tasa de IVA parametrizada del informe.
