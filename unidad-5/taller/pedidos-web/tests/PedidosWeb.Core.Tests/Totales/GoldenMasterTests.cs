using System.Globalization;
using PedidosWeb.Core.Totales;

namespace PedidosWeb.Core.Tests.Totales;

/// <summary>
/// Un caso por cada fila de migracion/golden-master.csv: lo que calcula el legado (defectos incluidos).
/// El CSV es el contrato y no se edita para que un test pase.
/// </summary>
public class GoldenMasterTests
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    // Columnas: caso,folio,tipo_cliente,region,flete_base,tasa_iva,lineas,subtotal,
    //           base_descuento,porc_descuento,descuento,flete,neto,iva,total
    public static IEnumerable<object[]> Casos() =>
        File.ReadLines(Path.Combine(AppContext.BaseDirectory, "golden-master.csv"))
            .Skip(1)
            .Where(fila => !string.IsNullOrWhiteSpace(fila))
            .Select(fila => new object[] { int.Parse(fila.Split(',')[0], Inv), fila });

    [Theory]
    [MemberData(nameof(Casos))]
    public void Calcular_CasoDelGoldenMaster_IgualAlLegado(int caso, string fila)
    {
        var c = fila.Split(',');
        var lineas = c[6]
            .Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split('x'))
            .Select(p => new LineaCalculo(int.Parse(p[0], Inv), decimal.Parse(p[1], Inv), p[2]))
            .ToList();

        var r = CalculoTotales.Calcular(
            lineas, esVip: c[2] == "V", region: int.Parse(c[3], Inv),
            fleteBase: decimal.Parse(c[4], Inv), tasaIva: decimal.Parse(c[5], Inv));

        Assert.Equal(decimal.Parse(c[10], Inv), r.Descuento);
        Assert.Equal(decimal.Parse(c[11], Inv), r.Flete);
        Assert.Equal(decimal.Parse(c[13], Inv), r.Iva);
        Assert.Equal(decimal.Parse(c[14], Inv), r.Total);
    }
}
