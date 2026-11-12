namespace PedidosWeb.Core.Pedidos;

/// <summary>
/// Parámetros de <c>GET /api/pedidos/buscar</c> tal como llegan en la query (HU-01, RN-01).
/// Todo llega como texto para que <see cref="BuscadorPedidos"/> decida los errores con el formato de la spec.
/// </summary>
/// <param name="ClienteId">Código del cliente (RN-03).</param>
/// <param name="Estado">Estado del pedido (RN-07).</param>
/// <param name="Sucursal">Código de la sucursal (RN-08).</param>
/// <param name="Desde">Fecha inicial inclusiva, formato yyyy-MM-dd (RN-06).</param>
/// <param name="Hasta">Fecha final inclusiva, formato yyyy-MM-dd (RN-06).</param>
/// <param name="Pagina">Número de página, desde 1 (RN-09).</param>
/// <param name="TamanoPagina">Pedidos por página, de 1 a 50 (RN-09).</param>
public sealed record ConsultaPedidos(
    string? ClienteId = null,
    string? Estado = null,
    string? Sucursal = null,
    string? Desde = null,
    string? Hasta = null,
    string? Pagina = null,
    string? TamanoPagina = null);
