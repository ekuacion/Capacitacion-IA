using System.Globalization;

namespace PedidosWeb.Core.Pedidos;

/// <summary>
/// Lista pedidos con filtros, orden fijo y paginación (HU-01, RN-02 a RN-11).
/// </summary>
public sealed class BuscadorPedidos
{
    private const int PaginaPorDefecto = 1;
    private const int TamanoPaginaPorDefecto = 10;
    private const int TamanoPaginaMaximo = 50;
    private const string FormatoFecha = "yyyy-MM-dd";

    /// <summary>RN-07: estados válidos (se comparan sin distinguir mayúsculas).</summary>
    private static readonly string[] EstadosValidos = ["Pendiente", "Confirmado", "Despachado", "Entregado", "Anulado"];

    /// <summary>RN-08: sucursales válidas (se comparan sin distinguir mayúsculas).</summary>
    private static readonly string[] SucursalesValidas = ["ANT", "CON", "VAL", "STGO"];

    private readonly IPedidoRepositorio _repositorio;

    public BuscadorPedidos(IPedidoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    /// <summary>Valida la consulta y devuelve una página de pedidos o el primer error (RN-11).</summary>
    public ResultadoBusqueda Buscar(ConsultaPedidos consulta)
    {
        // RN-02: un filtro vacío o con solo espacios cuenta como no enviado.
        var clienteId = Normalizar(consulta.ClienteId);
        var estado = Normalizar(consulta.Estado);
        var sucursal = Normalizar(consulta.Sucursal);
        var desdeTexto = Normalizar(consulta.Desde);
        var hastaTexto = Normalizar(consulta.Hasta);

        if (clienteId is null && estado is null && sucursal is null && desdeTexto is null && hastaTexto is null)
        {
            return Fallo(CodigosErrorBusqueda.FiltroRequerido, CodigosErrorBusqueda.MensajeFiltroRequerido);
        }

        // RN-06: formato yyyy-MM-dd y fecha existente.
        if (!TryParsearFecha(desdeTexto, out var desde) || !TryParsearFecha(hastaTexto, out var hasta))
        {
            return Fallo(CodigosErrorBusqueda.FechaInvalida, CodigosErrorBusqueda.MensajeFechaInvalida);
        }

        // RN-06: desde = hasta es válido (un solo día).
        if (desde > hasta)
        {
            return Fallo(CodigosErrorBusqueda.FechasInvertidas, CodigosErrorBusqueda.MensajeFechasInvertidas);
        }

        // RN-07
        if (estado is not null && !EstadosValidos.Contains(estado, StringComparer.OrdinalIgnoreCase))
        {
            return Fallo(CodigosErrorBusqueda.EstadoInvalido, CodigosErrorBusqueda.MensajeEstadoInvalido);
        }

        // RN-08
        if (sucursal is not null && !SucursalesValidas.Contains(sucursal, StringComparer.OrdinalIgnoreCase))
        {
            return Fallo(CodigosErrorBusqueda.SucursalInvalida, CodigosErrorBusqueda.MensajeSucursalInvalida);
        }

        // RN-09: pagina ≥ 1; tamanoPagina de 1 a 50; un valor no entero también es error.
        if (!TryParsearEntero(consulta.Pagina, PaginaPorDefecto, out var pagina) || pagina < 1
            || !TryParsearEntero(consulta.TamanoPagina, TamanoPaginaPorDefecto, out var tamanoPagina)
            || tamanoPagina < 1 || tamanoPagina > TamanoPaginaMaximo)
        {
            return Fallo(CodigosErrorBusqueda.PaginacionInvalida, CodigosErrorBusqueda.MensajePaginacionInvalida);
        }

        // RN-03, RN-04, RN-06, RN-07, RN-08: todos los filtros enviados se combinan con Y.
        var filtrados = _repositorio.ObtenerTodos()
            .Where(p => clienteId is null || string.Equals(p.ClienteId, clienteId, StringComparison.OrdinalIgnoreCase))
            .Where(p => estado is null || string.Equals(p.Estado, estado, StringComparison.OrdinalIgnoreCase))
            .Where(p => sucursal is null || string.Equals(p.Sucursal, sucursal, StringComparison.OrdinalIgnoreCase))
            .Where(p => desde is null || p.Fecha >= desde)
            .Where(p => hasta is null || p.Fecha <= hasta)
            // RN-05: Fecha ascendente y, a igual fecha, Id ascendente.
            .OrderBy(p => p.Fecha)
            .ThenBy(p => p.Id)
            .ToList();

        // RN-09: total = todos los que cumplen los filtros; una página más allá del final devuelve lista vacía.
        // El desplazamiento se calcula en long para que una página muy grande no desborde int.
        var desplazamiento = (long)(pagina - 1) * tamanoPagina;
        var paginaPedidos = desplazamiento >= filtrados.Count
            ? new List<Pedido>()
            : filtrados.Skip((int)desplazamiento).Take(tamanoPagina).ToList();

        return ResultadoBusqueda.Exito(new PaginaPedidos(paginaPedidos, filtrados.Count, pagina, tamanoPagina));
    }

    private static string? Normalizar(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor;

    /// <summary>Sin texto → true con null; con texto → true solo si cumple yyyy-MM-dd y la fecha existe.</summary>
    private static bool TryParsearFecha(string? texto, out DateOnly? fecha)
    {
        fecha = null;
        if (texto is null)
        {
            return true;
        }

        if (!DateOnly.TryParseExact(texto, FormatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out var valor))
        {
            return false;
        }

        fecha = valor;
        return true;
    }

    /// <summary>
    /// Sin texto (null, vacío o solo espacios: RN-09, v1.1) → true con el valor por defecto;
    /// con texto → true solo si es un número entero.
    /// </summary>
    private static bool TryParsearEntero(string? texto, int porDefecto, out int numero)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            numero = porDefecto;
            return true;
        }

        return int.TryParse(texto, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out numero);
    }

    private static ResultadoBusqueda Fallo(string codigo, string mensaje) =>
        ResultadoBusqueda.Fallo(new ErrorBusqueda(codigo, mensaje));
}
