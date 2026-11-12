namespace PedidosWeb.Core.Pedidos;

/// <summary>Una línea del detalle de un pedido (HU-02, RN-02).</summary>
/// <param name="Numero">Número de la línea dentro del pedido, desde 1.</param>
/// <param name="Producto">Descripción del producto.</param>
/// <param name="Cantidad">Unidades pedidas.</param>
/// <param name="PrecioUnitario">Precio de una unidad en CLP, sin IVA.</param>
/// <param name="Subtotal"><c>Cantidad × PrecioUnitario</c>, en CLP.</param>
public sealed record LineaDetalle(
    int Numero,
    string Producto,
    int Cantidad,
    decimal PrecioUnitario,
    decimal Subtotal);

/// <summary>
/// El detalle de un pedido: sus datos, su cliente, sus líneas y el total calculado desde ellas (HU-02).
/// </summary>
/// <param name="Id">Id del pedido.</param>
/// <param name="Folio">Folio con formato PED-{SUCURSAL}-{NUMERO}.</param>
/// <param name="Fecha">Fecha en que se emitió el pedido.</param>
/// <param name="Estado">Estado del pedido.</param>
/// <param name="Sucursal">Código de la sucursal que lo emitió.</param>
/// <param name="ClienteId">Código del cliente.</param>
/// <param name="ClienteNombre">Nombre del cliente, tal como viene en el pedido.</param>
/// <param name="Lineas">Las líneas, ordenadas por número (RN-03). Vacía si el pedido no tiene (RN-04).</param>
/// <param name="Total">Suma de los subtotales de las líneas, en CLP, sin IVA (RN-01). 0 si no hay líneas.</param>
public sealed record DetallePedido(
    int Id,
    string Folio,
    DateOnly Fecha,
    string Estado,
    string Sucursal,
    string ClienteId,
    string ClienteNombre,
    IReadOnlyList<LineaDetalle> Lineas,
    decimal Total);
