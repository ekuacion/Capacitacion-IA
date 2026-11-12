using PedidosWeb.Core.Pedidos;

namespace PedidosWeb.Core.Tests.Pedidos;

/// <summary>Tests de HU-03 (specs/HU-03-pedidos-por-fecha/spec.md) y de RN-07 de TEC-06: un test por criterio.</summary>
public class FiltroPedidosTests
{
    private static readonly DateOnly Desde = new(2026, 3, 1);
    private static readonly DateOnly Hasta = new(2026, 3, 31);

    private static Pedido PedidoDelDia(int id, DateOnly fecha) =>
        new(id, $"PED-STGO-{id:000000}", "C-001", "Cliente de prueba", fecha, "Pendiente", 10000m, "STGO");

    [Fact]
    public void EntreFechas_FechaIgualADesde_IncluyeElPedido()
    {
        // HU-03/CA-01
        var resultado = FiltroPedidos.EntreFechas(new[] { PedidoDelDia(1, Desde) }, Desde, Hasta);

        Assert.Equal(new[] { 1 }, resultado.Select(p => p.Id));
    }

    [Fact]
    public void EntreFechas_FechaIgualAHasta_IncluyeElPedido()
    {
        // HU-03/CA-02
        var resultado = FiltroPedidos.EntreFechas(new[] { PedidoDelDia(2, Hasta) }, Desde, Hasta);

        Assert.Equal(new[] { 2 }, resultado.Select(p => p.Id));
    }

    [Fact]
    public void EntreFechas_FechasFueraDelRango_NoLosIncluye()
    {
        // HU-03/CA-03: un día antes de desde y un día después de hasta
        var pedidos = new[]
        {
            PedidoDelDia(3, new DateOnly(2026, 2, 28)),
            PedidoDelDia(4, new DateOnly(2026, 4, 1)),
        };

        var resultado = FiltroPedidos.EntreFechas(pedidos, Desde, Hasta);

        Assert.Empty(resultado);
    }

    [Fact]
    public void EntreFechas_DesdeMayorQueHasta_LanzaArgumentException()
    {
        // HU-03/CA-04
        var ex = Assert.Throws<ArgumentException>(
            () => FiltroPedidos.EntreFechas(new[] { PedidoDelDia(5, Desde) }, Hasta, Desde));

        Assert.Contains("desde", ex.Message);
        Assert.Contains("hasta", ex.Message);
    }

    [Fact]
    public void EntreFechas_PedidosNulo_LanzaArgumentNullException()
    {
        // RN-07 de TEC-06 (decisión del PO)
        Assert.Throws<ArgumentNullException>(() => FiltroPedidos.EntreFechas(null!, Desde, Hasta));
    }

    [Fact]
    public void EntreFechas_VariosEnRango_ConservaElOrdenDeEntrada()
    {
        // RN-07 de TEC-06 (decisión del PO): mismo orden que se recibió, no por fecha
        var pedidos = new[]
        {
            PedidoDelDia(6, Hasta),
            PedidoDelDia(7, Desde),
            PedidoDelDia(8, new DateOnly(2026, 3, 15)),
        };

        var resultado = FiltroPedidos.EntreFechas(pedidos, Desde, Hasta);

        Assert.Equal(new[] { 6, 7, 8 }, resultado.Select(p => p.Id));
    }
}
