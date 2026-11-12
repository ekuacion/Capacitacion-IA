# Intent: Carrito de compras con descuentos

> Autor: Camila Soto (Product Owner, TiendaMini) · Fecha: 2026-10-20 · Estado: listo para spec

## Qué queremos
Que el cliente arme su carrito y vea el total final, con descuentos y despacho, antes de pagar.

## Por qué lo queremos
- Hoy el costo de despacho aparece recién en el pago. Muchos clientes abandonan en ese paso.
- Queremos atraer clientes nuevos con un cupón de bienvenida.
- Queremos subir el ticket promedio con despacho gratis desde cierto monto.

## Para quién
Clientes registrados de TiendaMini que compran desde la web.

## Reglas que ya conocemos
- Los precios están en pesos chilenos (CLP), enteros, sin decimales.
- El cupón "BIENVENIDA10" da 10% de descuento, solo en la primera compra.
- Compras desde $50.000 tienen despacho gratis. Si no, el despacho cuesta $3.990.
- Los descuentos no son acumulables: se aplica el mayor.
- Máximo 10 unidades por producto.
- Un cupón inválido o vencido debe mostrar un error claro.

## Restricciones
- Debe estar listo para la campaña de diciembre de 2026.
- Se construye sobre la API actual (ASP.NET Core). No hay presupuesto para servicios externos.
- El catálogo y el historial de pedidos ya existen. No se modifican.

## Cómo sabremos que funcionó
- El abandono en el paso de pago baja al menos 15% en los primeros 2 meses.
- Al menos 200 clientes nuevos usan BIENVENIDA10 en diciembre.

## Lo que NO es
- No es el proceso de pago ni la integración con la pasarela.
- No es un administrador de cupones. Por ahora hay un solo cupón.
- No incluye carritos para usuarios sin cuenta.
