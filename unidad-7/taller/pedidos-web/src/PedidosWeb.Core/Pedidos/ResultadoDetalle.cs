namespace PedidosWeb.Core.Pedidos;

/// <summary>Error al obtener el detalle de un pedido (HU-02, RN-05).</summary>
/// <param name="Codigo">Uno de los códigos de <see cref="CodigosErrorDetalle"/>.</param>
/// <param name="Mensaje">Mensaje para el usuario.</param>
public sealed record ErrorDetalle(string Codigo, string Mensaje);

/// <summary>Resultado de <see cref="ServicioDetallePedido.Obtener"/>: el detalle o un error, nunca ambos.</summary>
public sealed class ResultadoDetalle
{
    private ResultadoDetalle(DetallePedido? detalle, ErrorDetalle? error)
    {
        Detalle = detalle;
        Error = error;
    }

    /// <summary>El detalle del pedido, o null si hubo error.</summary>
    public DetallePedido? Detalle { get; }

    /// <summary>El error, o null si se obtuvo el detalle.</summary>
    public ErrorDetalle? Error { get; }

    /// <summary>true si se obtuvo el detalle.</summary>
    public bool EsExito => Error is null;

    /// <summary>Resultado exitoso con el detalle.</summary>
    public static ResultadoDetalle Exito(DetallePedido detalle) => new(detalle, null);

    /// <summary>Resultado fallido con su error.</summary>
    public static ResultadoDetalle Fallo(ErrorDetalle error) => new(null, error);
}
