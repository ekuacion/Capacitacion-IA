namespace PedidosWeb.Core.Pedidos;

/// <summary>HU-03: filtra pedidos por rango de fechas (specs/HU-03-pedidos-por-fecha/spec.md).</summary>
public static class FiltroPedidos
{
    /// <summary>
    /// Pedidos cuya <c>Fecha</c> está entre <paramref name="desde"/> y <paramref name="hasta"/>, ambos inclusive, en el orden recibido.
    /// Acepta cualquier <see cref="IEnumerable{T}"/> (no solo listas) a propósito; devuelve una lista de solo lectura.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="pedidos"/> es nulo.</exception>
    /// <exception cref="ArgumentException"><paramref name="desde"/> es mayor que <paramref name="hasta"/>, o <paramref name="pedidos"/> contiene un elemento nulo.</exception>
    public static IReadOnlyList<Pedido> EntreFechas(IEnumerable<Pedido> pedidos, DateOnly desde, DateOnly hasta)
    {
        ArgumentNullException.ThrowIfNull(pedidos);
        if (desde > hasta)
            throw new ArgumentException(
                $"'desde' ({desde:yyyy-MM-dd}) no puede ser mayor que 'hasta' ({hasta:yyyy-MM-dd}).", nameof(desde));

        var resultado = new List<Pedido>();
        foreach (var pedido in pedidos)
        {
            if (pedido is null)
                throw new ArgumentException("'pedidos' no puede contener elementos nulos.", nameof(pedidos));
            if (pedido.Fecha >= desde && pedido.Fecha <= hasta)
                resultado.Add(pedido);
        }

        return resultado;
    }
}
