using PedidosWeb.Core.Lineas;

namespace PedidosWeb.Core.Pedidos;

/// <summary>
/// Arma el detalle de un pedido: sus datos, su cliente, sus líneas ordenadas y el total calculado desde ellas (HU-02).
/// </summary>
public sealed class ServicioDetallePedido
{
    private readonly IPedidoRepositorio _pedidos;
    private readonly ILineaPedidoRepositorio _lineas;

    public ServicioDetallePedido(IPedidoRepositorio pedidos, ILineaPedidoRepositorio lineas)
    {
        _pedidos = pedidos;
        _lineas = lineas;
    }

    /// <summary>
    /// Devuelve el detalle del pedido, o el error <see cref="CodigosErrorDetalle.PedidoNoEncontrado"/> si no existe.
    /// </summary>
    /// <param name="id">Id del pedido.</param>
    public ResultadoDetalle Obtener(int id)
    {
        var pedido = _pedidos.ObtenerPorId(id);
        if (pedido is null)
        {
            return ResultadoDetalle.Fallo(new ErrorDetalle(
                CodigosErrorDetalle.PedidoNoEncontrado,
                CodigosErrorDetalle.MensajePedidoNoEncontrado(id)));
        }

        var lineasPedido = _lineas.ObtenerPorPedido(id)
            .OrderBy(linea => linea.Numero)
            .ToList();

        var lineasDetalle = lineasPedido
            .Select(linea => new LineaDetalle(
                linea.Numero,
                linea.Producto,
                linea.Cantidad,
                linea.PrecioUnitario,
                linea.Cantidad * linea.PrecioUnitario))
            .ToList();

        var detalle = new DetallePedido(
            pedido.Id,
            pedido.Folio,
            pedido.Fecha,
            pedido.Estado,
            pedido.Sucursal,
            pedido.ClienteId,
            pedido.ClienteNombre,
            lineasDetalle,
            CalculadoraTotalPedido.Calcular(lineasPedido));

        return ResultadoDetalle.Exito(detalle);
    }
}
