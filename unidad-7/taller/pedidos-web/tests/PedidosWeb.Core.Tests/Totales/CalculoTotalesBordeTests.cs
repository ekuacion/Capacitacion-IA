using PedidosWeb.Core.Totales;

namespace PedidosWeb.Core.Tests.Totales;

/// <summary>
/// Casos de borde que el golden master no cubre (TEC-05, etapa 2). Los valores esperados están escritos a mano
/// desde las reglas de TEC-04/spec.md (RG-01 a RG-08); nunca se calculan con CalculoTotales.
/// </summary>
public class CalculoTotalesBordeTests
{
    private const decimal Iva = 0.19m;

    private static LineaCalculo[] Una(int cantidad, decimal precio, string categoria)
        => [new LineaCalculo(cantidad, precio, categoria)];

    [Fact]
    public void Calcular_PedidoSinLineas_FleteYTotalesEnCero()
    {
        var r = CalculoTotales.Calcular([], esVip: false, region: 13, fleteBase: 3_500m, tasaIva: Iva);

        Assert.Equal(new ResultadoTotales(0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m), r);
    }

    [Fact]
    public void Calcular_CategoriaOfertaConEspaciosYMinusculas_NoEntraALaBase()
    {
        LineaCalculo[] lineas = [new(1, 450_000m, "BEB"), new(1, 100_000m, " ofe ")];

        var r = CalculoTotales.Calcular(lineas, esVip: false, region: 5, fleteBase: 5_000m, tasaIva: Iva);

        Assert.Equal(new ResultadoTotales(550_000m, 450_000m, 0m, 0m, 550_000m, 5_000m, 105_450m, 660_450m), r);
    }

    [Fact]
    public void Calcular_TasaIvaCero_IvaCeroYTotalSinIva()
    {
        var r = CalculoTotales.Calcular(Una(1, 100_000m, "BEB"), esVip: false, region: 5, fleteBase: 5_000m, tasaIva: 0m);

        Assert.Equal(new ResultadoTotales(100_000m, 100_000m, 0m, 0m, 100_000m, 5_000m, 0m, 105_000m), r);
    }

    [Fact]
    public void Calcular_FleteBaseCeroEnRegion12_FleteCero()
    {
        var r = CalculoTotales.Calcular(Una(1, 100_000m, "BEB"), esVip: false, region: 12, fleteBase: 0m, tasaIva: Iva);

        Assert.Equal(new ResultadoTotales(100_000m, 100_000m, 0m, 0m, 100_000m, 0m, 19_000m, 119_000m), r);
    }

    [Fact]
    public void Calcular_FleteBase10EnRegion12_FraccionDeCincuentaCentavosSube()
    {
        var r = CalculoTotales.Calcular(Una(1, 100_000m, "BEB"), esVip: false, region: 12, fleteBase: 10m, tasaIva: Iva);

        Assert.Equal(new ResultadoTotales(100_000m, 100_000m, 0m, 0m, 100_000m, 12m, 19_002m, 119_014m), r);
    }

    [Fact]
    public void Calcular_BaseDeExactamente500000_AplicaCincoPorCiento()
    {
        var r = CalculoTotales.Calcular(Una(1, 500_000m, "BEB"), esVip: false, region: 5, fleteBase: 5_000m, tasaIva: Iva);

        Assert.Equal(new ResultadoTotales(500_000m, 500_000m, 0.05m, 25_000m, 475_000m, 5_000m, 91_200m, 571_200m), r);
    }

    [Fact]
    public void Calcular_BaseDeExactamente1000000_AplicaOchoPorCiento()
    {
        var r = CalculoTotales.Calcular(Una(1, 1_000_000m, "BEB"), esVip: false, region: 5, fleteBase: 5_000m, tasaIva: Iva);

        Assert.Equal(new ResultadoTotales(1_000_000m, 1_000_000m, 0.08m, 80_000m, 920_000m, 5_000m, 175_750m, 1_100_750m), r);
    }
}
