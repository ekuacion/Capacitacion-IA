# Intent: HU-02 · Detalle de un pedido

> Autor: Claudio Ortiz Castillo (PO) · Fecha: 2026-11-10 · Estado: borrador para revisión

> Formato: `specs/plantillas/intent-plantilla.md`. Recoge el pedido del negocio tal como llegó al backlog.
> Es el primer documento de la historia: explica el problema. El «qué devuelve» exacto lo precisa la spec.

## Qué queremos

Que el vendedor vea desde el navegador el detalle de un pedido: a qué cliente pertenece, qué líneas tiene y cuánto suma.

## Por qué lo queremos

Con el listado de la HU-01 el vendedor ya encuentra un pedido, pero para responderle al cliente qué contiene y
cuánto suma todavía tiene que entrar al sistema de escritorio Pedidos Legado (Delphi). Este es el segundo sprint del
portal web que lo moderniza.

## Para quién

Los vendedores de la distribuidora.

## Reglas que ya conocemos

Tal como las dijo el negocio:

> «Como vendedor, quiero ver el detalle de un pedido (cliente, líneas, total y estado), para responderle al cliente
> sin entrar al sistema legado.»

No dice qué datos del cliente se muestran, cómo se ordenan las líneas, de dónde sale el total ni qué pasa si el
pedido no existe o no tiene líneas: eso lo precisa la spec.

## Restricciones

- .NET 10 y ASP.NET Core (Minimal API), con tests xUnit.
- Los datos viven en memoria (45 pedidos con sus líneas, entre 2 y 4 por pedido) hasta que llegue PostgreSQL en la Unidad 5.
- Montos en CLP como `decimal` sin decimales, **sin IVA**; nunca `double` ni `float`.
- Ruta nueva `GET /api/pedidos/{id}/detalle`. La ruta `GET /api/pedidos/{id}` no se toca.
- Ningún paquete NuGet nuevo.
- Se construye dentro del taller de la Unidad 3 (80 minutos), en 3 incrementos, con un commit por incremento.

## Cómo sabremos que funcionó

- Todos los criterios de aceptación de la spec en verde con `dotnet test`.
- En el navegador, el detalle del pedido 8 muestra un total de 120.100.

## Lo que NO es

- No incluye IVA, descuentos ni recargos (llegan con la migración del cálculo de totales, en la Unidad 5).
- No da formato de moneda para pantalla.
- No es una interfaz web: es el endpoint de la API.
- No crea ni modifica pedidos: solo los consulta.
- No toca `ClientesService`: ese módulo se ordena en la etapa 2 del taller.
