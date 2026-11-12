using PedidosWeb.Core.Pedidos;

namespace PedidosWeb.Core.Tests.Pedidos;

/// <summary>Tests de HU-01 (specs/HU-01-listar-pedidos/spec.md v1.0), un test por criterio de aceptación.</summary>
public class BuscadorPedidosTests
{
    private readonly BuscadorPedidos _buscador = new(new PedidoRepositorioEnMemoria());

    // ---------- Filtro por cliente ----------

    [Fact]
    public void Buscar_ClienteExistente_DevuelveSusPedidosEnOrden()
    {
        // CA-01
        var pagina = BuscarConExito(new ConsultaPedidos(ClienteId: "C-004"));

        Assert.Equal(new[] { 15, 7, 39, 31, 23 }, Ids(pagina));
        Assert.Equal(5, pagina.Total);
    }

    [Fact]
    public void Buscar_ClienteEnMinusculas_DevuelveLosMismosPedidos()
    {
        // CA-02
        var pagina = BuscarConExito(new ConsultaPedidos(ClienteId: "c-004"));

        Assert.Equal(new[] { 15, 7, 39, 31, 23 }, Ids(pagina));
        Assert.Equal(5, pagina.Total);
    }

    [Fact]
    public void Buscar_ClienteInexistente_DevuelveListaVacia()
    {
        // CA-03
        var pagina = BuscarConExito(new ConsultaPedidos(ClienteId: "C-999"));

        Assert.Empty(pagina.Pedidos);
        Assert.Equal(0, pagina.Total);
    }

    // ---------- Orden ----------

    [Fact]
    public void Buscar_PorCliente_OrdenaPorFechaAscendente()
    {
        // CA-04
        var pagina = BuscarConExito(new ConsultaPedidos(ClienteId: "C-001"));

        Assert.Equal(new[] { 40, 32, 24, 16, 8 }, Ids(pagina));
        Assert.Equal(
            new[]
            {
                new DateOnly(2026, 9, 2), new DateOnly(2026, 9, 14), new DateOnly(2026, 9, 26),
                new DateOnly(2026, 10, 8), new DateOnly(2026, 10, 20),
            },
            pagina.Pedidos.Select(p => p.Fecha));
    }

    // ---------- Filtro por estado ----------

    [Fact]
    public void Buscar_EstadoValido_DevuelvePedidosDeEseEstado()
    {
        // CA-05
        var pagina = BuscarConExito(new ConsultaPedidos(Estado: "Anulado"));

        Assert.Equal(new[] { 38, 11, 20, 29, 2 }, Ids(pagina));
        Assert.Equal(5, pagina.Total);
    }

    [Fact]
    public void Buscar_EstadoEnMinusculas_DevuelvePrimeraPaginaDeDiez()
    {
        // CA-06
        var pagina = BuscarConExito(new ConsultaPedidos(Estado: "entregado"));

        Assert.Equal(new[] { 40, 17, 15, 13, 26, 24, 22, 35, 33, 31 }, Ids(pagina));
        Assert.All(pagina.Pedidos, p => Assert.Equal("Entregado", p.Estado));
        Assert.Equal(15, pagina.Total);
        Assert.Equal(1, pagina.Pagina);
        Assert.Equal(10, pagina.TamanoPagina);
    }

    // ---------- Filtro por sucursal ----------

    [Fact]
    public void Buscar_SucursalEnMinusculas_DevuelvePedidosDeLaSucursal()
    {
        // CA-08
        var pagina = BuscarConExito(new ConsultaPedidos(Sucursal: "stgo", TamanoPagina: "50"));

        Assert.Equal(11, pagina.Pedidos.Count);
        Assert.All(pagina.Pedidos, p => Assert.Equal("STGO", p.Sucursal));
        Assert.Equal(11, pagina.Total);
    }

    // ---------- Rango de fechas ----------

    [Fact]
    public void Buscar_RangoDeOctubre_IncluyeAmbosLimites()
    {
        // CA-10
        var pagina = BuscarConExito(new ConsultaPedidos(Desde: "2026-10-01", Hasta: "2026-10-31", TamanoPagina: "50"));

        Assert.Equal(23, pagina.Pedidos.Count);
        Assert.Equal(23, pagina.Total);
        Assert.Equal(41, pagina.Pedidos[0].Id);
        Assert.Equal(new DateOnly(2026, 10, 1), pagina.Pedidos[0].Fecha);
        Assert.Equal(21, pagina.Pedidos[^1].Id);
        Assert.Equal(new DateOnly(2026, 10, 31), pagina.Pedidos[^1].Fecha);
    }

    [Fact]
    public void Buscar_SoloDesde_DevuelvePedidosDesdeEsaFecha()
    {
        // CA-11
        var pagina = BuscarConExito(new ConsultaPedidos(Desde: "2026-10-28"));

        Assert.Equal(new[] { 23, 2, 42, 21 }, Ids(pagina));
        Assert.Equal(4, pagina.Total);
    }

    [Fact]
    public void Buscar_SoloHasta_DevuelvePedidosHastaEsaFecha()
    {
        // CA-12
        var pagina = BuscarConExito(new ConsultaPedidos(Hasta: "2026-09-05"));

        Assert.Equal(new[] { 40, 19, 38 }, Ids(pagina));
        Assert.Equal(3, pagina.Total);
    }

    // ---------- Combinación ----------

    [Fact]
    public void Buscar_ClienteYEstado_CombinaConY()
    {
        // CA-15
        var pagina = BuscarConExito(new ConsultaPedidos(ClienteId: "C-004", Estado: "Entregado"));

        Assert.Equal(new[] { 15, 31 }, Ids(pagina));
        Assert.Equal(2, pagina.Total);
    }

    // ---------- Paginación ----------

    [Fact]
    public void Buscar_PaginaTresDeVeinte_DevuelveCincoPedidosYTotal45()
    {
        // CA-17
        var pagina = BuscarConExito(new ConsultaPedidos(Desde: "2026-09-01", Pagina: "3", TamanoPagina: "20"));

        Assert.Equal(new[] { 44, 23, 2, 42, 21 }, Ids(pagina));
        Assert.Equal(45, pagina.Total);
        Assert.Equal(3, pagina.Pagina);
        Assert.Equal(20, pagina.TamanoPagina);
    }

    [Fact]
    public void Buscar_PaginaMasAllaDelFinal_DevuelveListaVacia()
    {
        // CA-18
        var pagina = BuscarConExito(new ConsultaPedidos(ClienteId: "C-004", Pagina: "2"));

        Assert.Empty(pagina.Pedidos);
        Assert.Equal(5, pagina.Total);
    }

    // ---------- Bordes v0.3 ----------

    [Fact]
    public void Buscar_MismaFecha_DesempataPorIdAscendente()
    {
        // CA-23
        var mismaFecha = new DateOnly(2026, 9, 15);
        var repositorio = new RepositorioDeTest(
            new Pedido(9, "PED-STGO-000009", "C-001", "Cliente Nueve", mismaFecha, "Pendiente", 1000m, "STGO"),
            new Pedido(3, "PED-STGO-000003", "C-001", "Cliente Tres", mismaFecha, "Pendiente", 1000m, "STGO"));
        var buscador = new BuscadorPedidos(repositorio);

        var resultado = buscador.Buscar(new ConsultaPedidos(Desde: "2026-09-15", Hasta: "2026-09-15"));

        Assert.True(resultado.EsExito);
        Assert.Equal(new[] { 3, 9 }, Ids(resultado.Pagina!));
    }

    [Fact]
    public void Buscar_FiltroVacio_SeTrataComoNoEnviado()
    {
        // CA-25
        var soloVacio = _buscador.Buscar(new ConsultaPedidos(Estado: ""));

        Assert.False(soloVacio.EsExito);
        Assert.Equal(CodigosErrorBusqueda.FiltroRequerido, soloVacio.Error!.Codigo);

        var vacioYCliente = BuscarConExito(new ConsultaPedidos(Estado: "", ClienteId: "C-004"));

        Assert.Equal(new[] { 15, 7, 39, 31, 23 }, Ids(vacioYCliente));
        Assert.Equal(5, vacioYCliente.Total);
    }

    // ---------- Validaciones (T3) ----------

    [Fact]
    public void Buscar_EstadoInvalido_DevuelveErrorEstadoInvalido()
    {
        // CA-07
        var error = BuscarConError(new ConsultaPedidos(Estado: "Cancelado"));

        Assert.Equal(CodigosErrorBusqueda.EstadoInvalido, error.Codigo);
    }

    [Fact]
    public void Buscar_SucursalInvalida_DevuelveErrorSucursalInvalida()
    {
        // CA-09
        var error = BuscarConError(new ConsultaPedidos(Sucursal: "SCL"));

        Assert.Equal(CodigosErrorBusqueda.SucursalInvalida, error.Codigo);
    }

    [Fact]
    public void Buscar_FechasInvertidas_DevuelveError()
    {
        // CA-13
        var error = BuscarConError(new ConsultaPedidos(Desde: "2026-10-31", Hasta: "2026-10-01"));

        Assert.Equal(CodigosErrorBusqueda.FechasInvertidas, error.Codigo);
        Assert.Equal("La fecha desde no puede ser posterior a la fecha hasta.", error.Mensaje);
    }

    [Fact]
    public void Buscar_FechaMalFormada_DevuelveErrorFechaInvalida()
    {
        // CA-14
        var error = BuscarConError(new ConsultaPedidos(Desde: "31-10-2026"));

        Assert.Equal(CodigosErrorBusqueda.FechaInvalida, error.Codigo);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("1")]
    public void Buscar_SinFiltros_DevuelveErrorFiltroRequerido(string? pagina)
    {
        // CA-16: sin filtros, o solo con pagina=1 (la paginación no cuenta como filtro).
        var error = BuscarConError(new ConsultaPedidos(Pagina: pagina));

        Assert.Equal(CodigosErrorBusqueda.FiltroRequerido, error.Codigo);
        Assert.Equal(
            "Debes indicar al menos un filtro: clienteId, estado, sucursal, desde o hasta.",
            error.Mensaje);
    }

    [Theory]
    [InlineData(null, "51")]
    [InlineData("0", null)]
    public void Buscar_PaginacionFueraDeRango_DevuelveErrorPaginacionInvalida(string? pagina, string? tamanoPagina)
    {
        // CA-19
        var error = BuscarConError(new ConsultaPedidos(ClienteId: "C-004", Pagina: pagina, TamanoPagina: tamanoPagina));

        Assert.Equal(CodigosErrorBusqueda.PaginacionInvalida, error.Codigo);
    }

    [Fact]
    public void Buscar_VariosErrores_InformaSoloElPrimero()
    {
        // CA-20: FECHAS_INVERTIDAS va antes que ESTADO_INVALIDO (RN-11).
        var error = BuscarConError(new ConsultaPedidos(Estado: "Cancelado", Desde: "2026-10-31", Hasta: "2026-10-01"));

        Assert.Equal(CodigosErrorBusqueda.FechasInvertidas, error.Codigo);
    }

    [Fact]
    public void Buscar_PaginaNoNumerica_DevuelveErrorPaginacionInvalida()
    {
        // CA-24
        var error = BuscarConError(new ConsultaPedidos(ClienteId: "C-004", Pagina: "abc"));

        Assert.Equal(CodigosErrorBusqueda.PaginacionInvalida, error.Codigo);
    }

    [Fact]
    public void Buscar_PaginacionVacia_UsaValoresPorDefecto()
    {
        // CA-26
        var pagina = BuscarConExito(new ConsultaPedidos(ClienteId: "C-004", Pagina: "", TamanoPagina: " "));

        Assert.Equal(new[] { 15, 7, 39, 31, 23 }, Ids(pagina));
        Assert.Equal(5, pagina.Total);
        Assert.Equal(1, pagina.Pagina);
        Assert.Equal(10, pagina.TamanoPagina);
    }

    // ---------- Ayudas ----------

    private ErrorBusqueda BuscarConError(ConsultaPedidos consulta)
    {
        var resultado = _buscador.Buscar(consulta);

        Assert.False(resultado.EsExito, "Se esperaba un error y la búsqueda fue exitosa.");
        Assert.Null(resultado.Pagina);
        return resultado.Error!;
    }

    private PaginaPedidos BuscarConExito(ConsultaPedidos consulta)
    {
        var resultado = _buscador.Buscar(consulta);

        Assert.True(resultado.EsExito, $"Se esperaba éxito y llegó {resultado.Error?.Codigo}.");
        return resultado.Pagina!;
    }

    private static int[] Ids(PaginaPedidos pagina) => pagina.Pedidos.Select(p => p.Id).ToArray();

    /// <summary>Repositorio con pedidos fijos, en el orden en que se cargan (CA-23).</summary>
    private sealed class RepositorioDeTest(params Pedido[] pedidos) : IPedidoRepositorio
    {
        public IReadOnlyList<Pedido> ObtenerTodos() => pedidos;

        public Pedido? ObtenerPorId(int id) => pedidos.FirstOrDefault(p => p.Id == id);
    }
}
