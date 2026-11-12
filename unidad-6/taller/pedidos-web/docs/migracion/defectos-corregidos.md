# Defectos corregidos · migración del cálculo de totales

> Spec: `specs/TEC-04-migrar-calculo-totales/spec.md` v1.0. El C# (`src/PedidosWeb.Core/Totales/CalculoTotales.cs`) primero igualó al legado
> (commit «igual al legado», `b27b485`) y después se corrigió R05 en un commit aparte (`345a1a9`). Las líneas citadas se comprobaron abriendo cada archivo.

## D1 · R05 · Redondeo al peso con 0,50 hacia abajo (CORREGIDO)

- **Qué hace el legado:** `Calc3` hace `Trunc(AValor + 0.49)` (`legado/src/UCalculoTotales.pas:50`), así que una fracción de exactamente 0,50 se
  redondea **hacia abajo** (100,50 → 100) y solo desde 0,51 sube. Se usa para el descuento, el flete y el IVA. La misma fórmula está copiada en
  `RedondeaPesos` (`legado/src/UUtilidades.pas:30`), que usa el grabado de pedidos (`legado/src/UFrmPedidos.pas:308,313,315,318`).
- **Corrección:** el C# redondea con `Math.Truncate(valor + 0.5m)`: 0,50 sube. Es un cambio de una línea en `RedondearAlPeso`.
- **Por qué:** el redondeo comercial normal sube el 0,50. El cliente pagaba hasta un peso menos en el IVA o el descuento de esos pedidos, sin que nadie lo pidiera.
- **Casos del golden master que cambian:** 13 de 60 (en 47 no cambia nada). En los 13 cambian el descuento, el IVA o ambos, y por eso cambian el neto y el total:

  | Caso | Folio | Total antes | Total después | Diferencia |
  |---|---|---|---|---|
  | 2 | 1002 | 231.514 | 231.515 | +1 |
  | 6 | 1006 | 1.041.908 | 1.041.907 | −1 |
  | 8 | 1008 | 627.070 | 627.071 | +1 |
  | 18 | 1018 | 180.360 | 180.361 | +1 |
  | 21 | 1021 | 1.558.138 | 1.558.140 | +2 |
  | 23 | 1023 | 599.716 | 599.717 | +1 |
  | 25 | 1025 | 2.571.287 | 2.571.285 | −2 |
  | 27 | 1027 | 1.510.417 | 1.510.418 | +1 |
  | 36 | 1036 | 1.833.007 | 1.833.008 | +1 |
  | 37 | 1037 | 724.114 | 724.115 | +1 |
  | 42 | 1042 | 1.153.066 | 1.153.067 | +1 |
  | 46 | 1046 | 1.708.835 | 1.708.836 | +1 |
  | 54 | 1054 | 704.391 | 704.392 | +1 |

  Los totales de los otros 47 casos no cambian. Dos totales bajan (casos 6 y 25) porque un descuento que sube un peso baja el neto más de lo que sube el IVA.
- **Suma de la columna total:** 58.202.135 → 58.202.144 (+9 pesos en 60 pedidos).
- **Commit:** `fix(u5): R05 redondeo al peso, 0,50 sube (T3)` (`345a1a9`), separado de `feat(u5): cálculo de totales en C#, igual al legado (T2)` (`b27b485`).
- **Convivencia:** mientras el legado siga calculando con `+0.49`, los mismos pedidos darán 1 o 2 pesos de diferencia en 13 de cada 60 (cerca del 22 %).
  Contabilidad debe conciliar esas diferencias como «redondeo R05» y no como errores. `RedondeaPesos` (`UUtilidades.pas:30`) sigue con el defecto en el legado.

## H1 · Umbrales de descuento distintos entre pantalla y grabado (HALLAZGO, NO CORREGIDO)

- **Qué encontraste:** la regla de volumen está escrita dos veces con distinto operador.
  `CalcularTotales`, que alimenta la pantalla, usa mayor que (`legado/src/UCalculoTotales.pas:77,79`): con una base de exactamente 500.000 o 1.000.000 el
  descuento baja un tramo. El grabado usa mayor o igual (`legado/src/UFrmPedidos.pas:296,298`). Además, al grabar existe un recargo de Navidad de 2 puntos
  (`legado/src/UFrmPedidos.pas:304-305`) que `CalcularTotales` no tiene; hoy está apagado (`gDescNavidad`, `legado/src/UGlobales.pas:33`).
- **Efecto:** en el golden master, **13 de 60 pedidos** tienen una base de exactamente 500.000 o 1.000.000 (casos 8, 11, 19, 23, 26, 32, 35, 39, 42, 48, 50, 55 y 60).
  Para ellos la pantalla muestra un descuento y al grabar se guarda otro (un tramo más alto). La comparación del grabado con `CalcularTotales` (`UFrmPedidos.pas:327`)
  solo avisa en modo prueba, así que en producción nadie lo ve.
- **Quién decide:** contabilidad (la cabecera de `UCalculoTotales.pas` pide hablar con ellos y menciona un acta comercial de 2011 con 400.000, que no coincide con
  ninguno de los dos umbrales del código). El C# replica la versión de **pantalla** (mayor que), que es la que usa el golden master. Cuando contabilidad decida,
  se cambia el operador y se actualizan los casos afectados.
