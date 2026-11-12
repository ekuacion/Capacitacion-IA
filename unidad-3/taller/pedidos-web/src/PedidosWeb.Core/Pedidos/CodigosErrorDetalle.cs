namespace PedidosWeb.Core.Pedidos;

/// <summary>
/// Códigos y mensajes de error del detalle de un pedido, copiados de la spec HU-02 (RN-05).
/// </summary>
public static class CodigosErrorDetalle
{
    public const string PedidoNoEncontrado = "PEDIDO_NO_ENCONTRADO";

    /// <summary>Mensaje para el usuario: «No existe el pedido {id}.»</summary>
    public static string MensajePedidoNoEncontrado(int id) => $"No existe el pedido {id}.";
}
