# Inventario de Pedidos Legado

> Etapa 1 · Entender (T2 de `specs/TEC-03-diagnostico-legado/`). Solo lectura: no se modificó nada de `legado/`.
> Las líneas de cada archivo se contaron con `wc -l`; las de las tablas son las líneas de su bloque `CREATE TABLE` en
> `legado/db/esquema-dbisam.sql`. Total de `legado/src/`: **16 archivos, 2.375 líneas** (10 `.pas` y 6 `.dfm`).

| Archivo | Tipo | Responsabilidad | Líneas |
|---|---|---|---|
| `legado/PedidosLegado.dpr` | Programa principal | Arranca la aplicación: crea el data module y el formulario principal y lista las 10 unidades del proyecto. | 24 |
| `legado/src/UGlobales.pas` | Globales | Variables globales mutables (usuario, nivel, IVA, flete base, folio, modo prueba, promo de Navidad) y su inicialización. | 41 |
| `legado/src/UUtilidades.pas` | Utilidades | Funciones sueltas: redondeo y formato de pesos, validación y formato de RUT, fechas, día hábil, fecha del pedido según cierre, escape de comillas. | 189 |
| `legado/src/UCalculoTotales.pas` | Unidad de lógica | Cálculo de los totales de un pedido: subtotal, descuento por volumen y VIP, flete por región, IVA y total; manejo de las líneas en memoria. | 147 |
| `legado/src/UStock.pas` | Unidad de lógica | Consulta y descuento de stock de productos (por producto y por pedido completo) y registro de movimientos de stock. | 93 |
| `legado/src/UDMPrincipal.pas` | Data module | Conecta a la base DBISAM, abre las tablas, lee parámetros y ejecuta SQL para el resto del sistema. | 100 |
| `legado/src/UDMPrincipal.dfm` | Data module (diseño) | Componentes DBISAM del data module: base, tablas y consultas sobre el alias `PEDIDOS`. | 65 |
| `legado/src/UFrmPrincipal.pas` | Formulario | Ventana principal con el menú; carga parámetros y abre los formularios de clientes, productos, pedidos e informes. | 104 |
| `legado/src/UFrmPrincipal.dfm` | Formulario (diseño) | Menú principal «Pedidos - Distribuidora». | 62 |
| `legado/src/UFrmClientes.pas` | Formulario | Mantenedor de clientes: buscar, crear, grabar y eliminar, con validación de RUT y control de pedidos asociados. | 215 |
| `legado/src/UFrmClientes.dfm` | Formulario (diseño) | Pantalla «Clientes»: grilla, campos y botones. | 213 |
| `legado/src/UFrmProductos.pas` | Formulario | Mantenedor de productos: filtrar por descripción y validar antes de grabar. | 105 |
| `legado/src/UFrmProductos.dfm` | Formulario (diseño) | Pantalla «Productos»: grilla, filtro y botones. | 98 |
| `legado/src/UFrmPedidos.pas` | Formulario | Toma de pedidos: busca cliente por RUT, agrega y quita líneas, calcula totales, graba pedido y líneas y descuenta stock. | 420 |
| `legado/src/UFrmPedidos.dfm` | Formulario (diseño) | Pantalla «Toma de pedidos»: cliente, líneas, totales y botones. | 226 |
| `legado/src/UFrmInformes.pas` | Formulario | Informe de ventas entre dos fechas (pedidos con cliente) y exportación a CSV para contabilidad. | 137 |
| `legado/src/UFrmInformes.dfm` | Formulario (diseño) | Pantalla «Informe de ventas»: rango de fechas, grilla y botones. | 160 |
| `legado/db/esquema-dbisam.sql` · `REGIONES` | Tabla | Regiones con su código, nombre y flete base. | 4–10 (7) |
| `legado/db/esquema-dbisam.sql` · `CLIENTES` | Tabla | Clientes: RUT, razón social, tipo, región, dirección, saldo vencido, fecha de vencimiento y si está activo. | 12–25 (14) |
| `legado/db/esquema-dbisam.sql` · `PRODUCTOS` | Tabla | Productos: código, descripción, categoría, precio, stock y stock mínimo. | 29–39 (11) |
| `legado/db/esquema-dbisam.sql` · `PEDIDOS` | Tabla | Cabecera del pedido: folio, fecha, cliente, usuario, estado, observación y total. | 43–53 (11) |
| `legado/db/esquema-dbisam.sql` · `PEDIDO_LINEAS` | Tabla | Líneas del pedido: folio, número de línea, producto, cantidad y precio unitario. | 58–66 (9) |
| `legado/db/esquema-dbisam.sql` · `PARAMETROS` | Tabla | Parámetros del sistema en formato clave y valor (por ejemplo, tasas). | 68–74 (7) |

## Lo que me llamó la atención

- **El README del legado no calza con el código.** `legado/README.md` dice «Delphi 5 + Paradox vía BDE + Crystal Reports 8», pero el código usa
  DBISAM (`dbisamtb` en `UDMPrincipal.pas:6` y el esquema `esquema-dbisam.sql`); lo único de BDE es un comentario («antes era alias BDE PEDIDOS»,
  `UDMPrincipal.pas:58`), no hay rastro de Paradox ni de Crystal (el informe exporta a CSV, `UFrmInformes.pas:100`) y el README del proyecto
  dice «Delphi 7 + DBISAM 4». Se confía en el código, no en el README.
- **Una tabla que el esquema no define.** `UStock.pas:85` inserta en `MOV_STOCK`, pero `esquema-dbisam.sql` solo trae 6 tablas y ninguna se
  llama así: o el esquema está incompleto o ese registro falla en silencio (a confirmar en la etapa 2).
- **El redondeo está escrito dos veces y hay una advertencia en el código.** `Calc3` (`UCalculoTotales.pas:48`) y `RedondeaPesos`
  (`UUtilidades.pas:28`, «igual que en totales»); y la cabecera de `UCalculoTotales.pas` dice «NO TOCAR sin hablar con contabilidad»
  (descuento por volumen del acta comercial de 2011).
- **Estado global compartido.** `UGlobales.pas` guarda variables mutables que, según su propio comentario, «se modifican desde varios
  formularios» (`gDescNavidad` y `gModoPrueba`, por ejemplo, se leen en `UFrmPedidos.pas:304` y `:330`).
- **Nombres que no dicen qué hacen:** `Calc3` y `Proc2` (`UFrmPedidos.pas:367`, que en realidad muestra los totales en pantalla).
