namespace PedidosWeb.Core.Pedidos;

/// <summary>Una página del listado de pedidos (HU-01, RN-09).</summary>
/// <param name="Pedidos">Los pedidos de esta página, ya ordenados (RN-05).</param>
/// <param name="Total">Cantidad de pedidos que cumplen los filtros, no solo los de la página.</param>
/// <param name="Pagina">Número de página entregado.</param>
/// <param name="TamanoPagina">Tamaño de página usado.</param>
public sealed record PaginaPedidos(
    IReadOnlyList<Pedido> Pedidos,
    int Total,
    int Pagina,
    int TamanoPagina);

/// <summary>Error de validación de la búsqueda (HU-01, RN-10).</summary>
/// <param name="Codigo">Uno de los códigos de <see cref="CodigosErrorBusqueda"/>.</param>
/// <param name="Mensaje">Mensaje para el usuario (spec §6).</param>
public sealed record ErrorBusqueda(string Codigo, string Mensaje);

/// <summary>Resultado de <see cref="BuscadorPedidos.Buscar"/>: una página o un error, nunca ambos.</summary>
public sealed class ResultadoBusqueda
{
    private ResultadoBusqueda(PaginaPedidos? pagina, ErrorBusqueda? error)
    {
        Pagina = pagina;
        Error = error;
    }

    /// <summary>La página encontrada, o null si hubo error.</summary>
    public PaginaPedidos? Pagina { get; }

    /// <summary>El error de validación, o null si la búsqueda fue exitosa.</summary>
    public ErrorBusqueda? Error { get; }

    /// <summary>true si la búsqueda devolvió una página.</summary>
    public bool EsExito => Error is null;

    /// <summary>Resultado exitoso con una página de pedidos.</summary>
    public static ResultadoBusqueda Exito(PaginaPedidos pagina) => new(pagina, null);

    /// <summary>Resultado fallido con su error.</summary>
    public static ResultadoBusqueda Fallo(ErrorBusqueda error) => new(null, error);
}
