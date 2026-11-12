using PedidosWeb.Core.Lineas;

namespace PedidosWeb.Core.Pedidos;

/// <summary>
/// Calcula el total de un pedido a partir de sus líneas (HU-02, RN-01 y RN-02).
/// </summary>
public static class CalculadoraTotalPedido
{
    /// <summary>
    /// Suma <c>Cantidad × PrecioUnitario</c> de cada línea. Montos en CLP, sin IVA.
    /// Devuelve 0 si no hay líneas. No valida cantidades ni precios negativos: eso se garantiza en la fuente de datos.
    /// </summary>
    /// <param name="lineas">Las líneas del pedido.</param>
    /// <exception cref="ArgumentNullException">Si la lista o alguna de sus líneas es <c>null</c>: es un error de programación, no un pedido sin líneas.</exception>
    public static decimal Calcular(IEnumerable<LineaPedido> lineas)
    {
        ArgumentNullException.ThrowIfNull(lineas);

        return lineas.Sum(linea =>
        {
            ArgumentNullException.ThrowIfNull(linea);
            return linea.Cantidad * linea.PrecioUnitario;
        });
    }
}
