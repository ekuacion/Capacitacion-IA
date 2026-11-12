# Intent: HU-01 · Listar pedidos con filtros

> Autor: Claudio Ortiz Castillo (PO) · Fecha: 2026-11-09 · Estado: listo para spec

> Formato: `specs/plantillas/intent-plantilla.md`. Se escribió después de la spec v0.2, con la autorización del
> profesor para usar las plantillas; recoge el pedido original del PO tal como llegó al backlog.

## Qué queremos

Que el vendedor vea desde el navegador los pedidos de un cliente, de un estado o de un período.

## Por qué lo queremos

Hoy, para responder a sus clientes sobre el estado de sus pedidos, el vendedor tiene que entrar al sistema de
escritorio Pedidos Legado (Delphi). Este es el primer sprint del portal web que lo moderniza.

## Para quién

Los vendedores de la distribuidora.

## Reglas que ya conocemos

Tal como las dijo el negocio:

> «Como vendedor, quiero listar pedidos filtrando por cliente, estado y rango de fechas.»

No dice qué estados son válidos, qué pasa con fechas invertidas, cuántos resultados se devuelven ni cómo se
informa un error: eso lo precisa la spec.

## Restricciones

- .NET 10 y ASP.NET Core (Minimal API), con tests xUnit.
- Los datos viven en memoria (45 pedidos ficticios, septiembre y octubre de 2026) hasta que llegue PostgreSQL en la Unidad 5.
- Montos en CLP como `decimal` sin decimales; nunca `double` ni `float`.
- Ningún paquete NuGet sin aprobación.
- Se construye dentro del taller de la Unidad 2 (80 minutos).

## Cómo sabremos que funcionó

- Todos los criterios de aceptación de la spec en verde con `dotnet test`.
- En una demo al PO, se responde una consulta real sobre el estado de un pedido sin abrir el Delphi.

## Lo que NO es

- No reemplaza el Delphi completo: solo la consulta de pedidos.
- No es una interfaz web: es el endpoint de la API.
- No incluye autenticación.
- No crea ni modifica pedidos: solo los consulta.
