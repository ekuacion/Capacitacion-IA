using PedidosWeb.Core.Lineas;
using PedidosWeb.Core.Pedidos;

namespace PedidosWeb.Core.Tests.Pedidos;

public class ServicioDetallePedidoTests
{
    private readonly ServicioDetallePedido _servicio =
        new(new PedidoRepositorioEnMemoria(), new LineaPedidoRepositorioEnMemoria());

    [Fact]
    public void Obtener_Pedido8_TotalEs120100YSumaLosSubtotales()
    {
        var detalle = _servicio.Obtener(8).Detalle!;

        Assert.Equal(120100m, detalle.Total);
        Assert.Equal(detalle.Lineas.Sum(l => l.Subtotal), detalle.Total);
    }

    [Fact]
    public void Obtener_Pedido8_TraeDatosClienteYLineasOrdenadas()
    {
        var resultado = _servicio.Obtener(8);

        Assert.True(resultado.EsExito);
        Assert.Null(resultado.Error);
        var detalle = resultado.Detalle!;
        Assert.Equal(8, detalle.Id);
        Assert.Equal("PED-STGO-000008", detalle.Folio);
        Assert.Equal(new DateOnly(2026, 10, 20), detalle.Fecha);
        Assert.Equal("Entregado", detalle.Estado);
        Assert.Equal("STGO", detalle.Sucursal);
        Assert.Equal("C-001", detalle.ClienteId);
        Assert.Equal("Ferretería El Clavo", detalle.ClienteNombre);
        Assert.Equal(new[] { 1, 2, 3 }, detalle.Lineas.Select(l => l.Numero));
        Assert.Equal(new[] { 64500m, 35400m, 20200m }, detalle.Lineas.Select(l => l.Subtotal));
        Assert.Equal("Harina para pan 25 kg", detalle.Lineas[0].Producto);
    }

    [Fact]
    public void Obtener_Pedido8_SubtotalEsCantidadPorPrecio()
    {
        var detalle = _servicio.Obtener(8).Detalle!;

        Assert.All(detalle.Lineas, l => Assert.Equal(l.Cantidad * l.PrecioUnitario, l.Subtotal));
    }

    [Fact]
    public void Obtener_LosPedidosDeEjemplo_ElTotalCoincideConElTotalDelPedido()
    {
        var pedidos = new PedidoRepositorioEnMemoria().ObtenerTodos();

        var distintos = pedidos
            .Where(p => _servicio.Obtener(p.Id).Detalle!.Total != p.Total)
            .Select(p => p.Id)
            .ToList();

        Assert.Empty(distintos);
    }

    [Fact]
    public void Obtener_LineasDesordenadas_LasDevuelvePorNumero()
    {
        var lineas = new LineasFalsas(new LineaPedido[]
        {
            new(8, 3, "C", 1, 300m),
            new(8, 1, "A", 1, 100m),
            new(8, 2, "B", 1, 200m),
        });
        var servicio = new ServicioDetallePedido(new PedidoRepositorioEnMemoria(), lineas);

        var detalle = servicio.Obtener(8).Detalle!;

        Assert.Equal(new[] { 1, 2, 3 }, detalle.Lineas.Select(l => l.Numero));
        Assert.Equal(600m, detalle.Total);
    }

    [Fact]
    public void Obtener_PedidoSinLineas_DevuelveLineasVaciasYTotalCero()
    {
        var servicio = new ServicioDetallePedido(
            new PedidoRepositorioEnMemoria(),
            new LineasFalsas(Array.Empty<LineaPedido>()));

        var resultado = servicio.Obtener(8);

        Assert.True(resultado.EsExito);
        Assert.Empty(resultado.Detalle!.Lineas);
        Assert.Equal(0m, resultado.Detalle.Total);
    }

    [Fact]
    public void Obtener_IdInexistente_DevuelveErrorConCodigoYMensaje()
    {
        var resultado = _servicio.Obtener(999);

        Assert.False(resultado.EsExito);
        Assert.Null(resultado.Detalle);
        Assert.Equal("PEDIDO_NO_ENCONTRADO", resultado.Error!.Codigo);
        Assert.Equal("No existe el pedido 999.", resultado.Error.Mensaje);
    }

    /// <summary>Repositorio de líneas de prueba: devuelve siempre las mismas líneas, sin ordenar.</summary>
    private sealed class LineasFalsas : ILineaPedidoRepositorio
    {
        private readonly IReadOnlyList<LineaPedido> _lineas;

        public LineasFalsas(IReadOnlyList<LineaPedido> lineas) => _lineas = lineas;

        public IReadOnlyList<LineaPedido> ObtenerPorPedido(int pedidoId) => _lineas;
    }
}
