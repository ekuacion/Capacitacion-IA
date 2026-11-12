# Inventario de Pedidos Legado (respaldo)

> Respaldo de la etapa 1 de la Unidad 4. Úsalo solo si te trabaste: hacer tu propio inventario es parte del aprendizaje.
> Las líneas se contaron con un script (`wc -l`), no a ojo.

| Archivo | Tipo | Responsabilidad | Líneas |
|---|---|---|---|
| `legado/src/UCalculoTotales.pas` | Unidad de lógica | Cálculo de los totales del pedido: subtotal, descuento, flete, IVA y total. Funciones puras: no usa pantallas ni base de datos. | 147 |
| `legado/src/UDMPrincipal.pas` | Data module | Conexión a la base DBISAM, tablas y consultas compartidas; lectura de parámetros y ejecución de SQL. | 100 |
| `legado/src/UDMPrincipal.dfm` | Data module (diseño) | Componentes de acceso a datos: base, 5 tablas y 2 consultas. | 65 |
| `legado/src/UFrmClientes.pas` | Formulario | Mantenedor (ABM) de clientes, con validaciones antes de grabar y búsqueda por razón social. | 215 |
| `legado/src/UFrmClientes.dfm` | Formulario (diseño) | Pantalla de clientes: grilla, campos del cliente y botones. | 213 |
| `legado/src/UFrmInformes.pas` | Formulario | Informe de ventas por rango de fechas y cliente, con totales y exportación a CSV. | 137 |
| `legado/src/UFrmInformes.dfm` | Formulario (diseño) | Pantalla del informe: filtros, grilla y resumen. | 160 |
| `legado/src/UFrmPedidos.pas` | Formulario | Toma de pedidos: busca cliente y productos, muestra totales, graba el pedido y descuenta stock. | 420 |
| `legado/src/UFrmPedidos.dfm` | Formulario (diseño) | Pantalla de toma de pedidos: cliente, líneas, totales y observaciones. | 226 |
| `legado/src/UFrmPrincipal.pas` | Formulario | Menú principal: ingreso del usuario, conexión, carga de parámetros y apertura de las demás pantallas. | 104 |
| `legado/src/UFrmPrincipal.dfm` | Formulario (diseño) | Menú principal y barra de estado. | 62 |
| `legado/src/UFrmProductos.pas` | Formulario | Mantenedor (ABM) de productos, con filtro y validaciones antes de grabar. | 105 |
| `legado/src/UFrmProductos.dfm` | Formulario (diseño) | Pantalla de productos: filtro, grilla y navegador. | 98 |
| `legado/src/UGlobales.pas` | Globales | Variables globales del sistema (usuario, nivel, tasa de IVA, banderas) y su inicialización. | 41 |
| `legado/src/UStock.pas` | Unidad de lógica | Consulta y descuento del stock de productos al confirmar un pedido; registro de movimientos. | 93 |
| `legado/src/UUtilidades.pas` | Utilidades | Funciones sueltas: redondeo, formato de pesos, RUT, fechas, día hábil y textos. | 189 |
| `legado/PedidosLegado.dpr` | Proyecto | Programa principal: crea el data module y el menú. | 24 |
| **Total** | | 17 archivos | **2399** |

## Observaciones

- 10 unidades `.pas` y 6 formularios `.dfm`. Los formularios guardan el diseño de la pantalla y los nombres de los eventos (por ejemplo, `OnClick = btnGrabarClick`).
- `legado/README.md` dice Delphi 5, BDE, Paradox y Crystal Reports, pero el código usa componentes DBISAM (`TDBISAMDatabase`, `TDBISAMTable`, `TDBISAMQuery`) y no hay rastro de Crystal Reports. El README está desactualizado.
- `UFrmPedidos.pas` es el archivo más grande del sistema.
