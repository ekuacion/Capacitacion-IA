namespace PedidosWeb.Core.Pedidos;

/// <summary>
/// Códigos y mensajes de error de la búsqueda de pedidos, copiados tal cual de la spec HU-01 §6.
/// Se evalúan en el orden en que aparecen aquí (RN-11).
/// </summary>
public static class CodigosErrorBusqueda
{
    public const string FiltroRequerido = "FILTRO_REQUERIDO";
    public const string FechaInvalida = "FECHA_INVALIDA";
    public const string FechasInvertidas = "FECHAS_INVERTIDAS";
    public const string EstadoInvalido = "ESTADO_INVALIDO";
    public const string SucursalInvalida = "SUCURSAL_INVALIDA";
    public const string PaginacionInvalida = "PAGINACION_INVALIDA";

    public const string MensajeFiltroRequerido =
        "Debes indicar al menos un filtro: clienteId, estado, sucursal, desde o hasta.";
    public const string MensajeFechaInvalida =
        "La fecha debe tener el formato yyyy-MM-dd.";
    public const string MensajeFechasInvertidas =
        "La fecha desde no puede ser posterior a la fecha hasta.";
    public const string MensajeEstadoInvalido =
        "El estado debe ser Pendiente, Confirmado, Despachado, Entregado o Anulado.";
    public const string MensajeSucursalInvalida =
        "La sucursal debe ser ANT, CON, VAL o STGO.";
    public const string MensajePaginacionInvalida =
        "La página debe ser 1 o mayor y el tamaño de página, entre 1 y 50.";
}
