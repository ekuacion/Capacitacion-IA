using PedidosWeb.Core.Lineas;
using PedidosWeb.Core.Pedidos;

namespace PedidosWeb.Core.Tests.Pedidos;

public class CalculadoraTotalPedidoTests
{
    [Fact]
    public void Calcular_LineasDelPedido8_Suma120100()
    {
        var lineas = new LineaPedidoRepositorioEnMemoria().ObtenerPorPedido(8);

        var total = CalculadoraTotalPedido.Calcular(lineas);

        Assert.Equal(120100m, total);
    }

    [Fact]
    public void Calcular_UnaSolaLinea_DevuelveCantidadPorPrecio()
    {
        var lineas = new[] { new LineaPedido(1, 1, "Sal fina 1 kg", 4, 550m) };

        var total = CalculadoraTotalPedido.Calcular(lineas);

        Assert.Equal(2200m, total);
    }

    [Fact]
    public void Calcular_SinLineas_DevuelveCero()
    {
        var total = CalculadoraTotalPedido.Calcular(Array.Empty<LineaPedido>());

        Assert.Equal(0m, total);
    }

    [Fact]
    public void Calcular_CantidadCero_AportaCero()
    {
        var lineas = new[]
        {
            new LineaPedido(1, 1, "Sal fina 1 kg", 0, 550m),
            new LineaPedido(1, 2, "Agua mineral 1,6 L", 2, 900m),
        };

        var total = CalculadoraTotalPedido.Calcular(lineas);

        Assert.Equal(1800m, total);
    }

    [Fact]
    public void Calcular_ListaNull_LanzaArgumentNullException()
    {
        var excepcion = Assert.Throws<ArgumentNullException>(() => CalculadoraTotalPedido.Calcular(null!));

        Assert.Equal("lineas", excepcion.ParamName);
    }

    [Fact]
    public void Calcular_LineaNull_LanzaArgumentNullException()
    {
        var lineas = new LineaPedido[] { new(1, 1, "Sal fina 1 kg", 4, 550m), null! };

        Assert.Throws<ArgumentNullException>(() => CalculadoraTotalPedido.Calcular(lineas));
    }

    [Fact]
    public void Calcular_LineasDeLos45Pedidos_CoincideConElTotalDelPedido()
    {
        var pedidos = new PedidoRepositorioEnMemoria().ObtenerTodos();
        var lineas = new LineaPedidoRepositorioEnMemoria();

        var distintos = pedidos
            .Where(p => CalculadoraTotalPedido.Calcular(lineas.ObtenerPorPedido(p.Id)) != p.Total)
            .Select(p => p.Id)
            .ToList();

        Assert.Empty(distintos);
    }
}
