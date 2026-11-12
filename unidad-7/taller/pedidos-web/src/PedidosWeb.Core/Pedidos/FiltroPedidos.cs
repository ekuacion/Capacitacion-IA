namespace PedidosWeb.Core.Pedidos;

/// <summary>HU-03: filtra pedidos por rango de fechas (specs/HU-03-pedidos-por-fecha/spec.md).</summary>
public static class FiltroPedidos
{
    /// <summary>Pedidos cuya <c>Fecha</c> está entre <paramref name="desde"/> y <paramref name="hasta"/>, ambos inclusive, en el orden recibido.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="pedidos"/> es nulo.</exception>
    /// <exception cref="ArgumentException"><paramref name="desde"/> es mayor que <paramref name="hasta"/>.</exception>
    public static IReadOnlyList<Pedido> EntreFechas(IEnumerable<Pedido> pedidos, DateOnly desde, DateOnly hasta)
    {
        ArgumentNullException.ThrowIfNull(pedidos);
        if (desde > hasta)
            throw new ArgumentException(
                $"'desde' ({desde:yyyy-MM-dd}) no puede ser mayor que 'hasta' ({hasta:yyyy-MM-dd}).", nameof(desde));

        return pedidos.Where(p => p.Fecha >= desde && p.Fecha <= hasta).ToList();
    }
}
