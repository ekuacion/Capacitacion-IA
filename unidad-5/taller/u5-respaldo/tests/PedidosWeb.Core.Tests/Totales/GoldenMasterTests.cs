using System.Globalization;
using PedidosWeb.Core.Totales;

namespace PedidosWeb.Core.Tests.Totales;

/// <summary>Un test por cada caso de migracion/golden-master.csv.</summary>
public class GoldenMasterTests
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    public static IEnumerable<object[]> Casos() =>
        File.ReadLines(Path.Combine(AppContext.BaseDirectory, "golden-master.csv"))
            .Skip(1)
            .Select(fila => new object[] { int.Parse(fila.Split(',')[1], Inv), fila });

    [Theory]
    [MemberData(nameof(Casos))]
    public void Totales_igual_al_golden_master(int folio, string fila)
    {
        // Columnas: caso,folio,tipo_cliente,region,flete_base,tasa_iva,lineas,subtotal,
        //           base_descuento,porc_descuento,descuento,flete,neto,iva,total
        var c = fila.Split(',');
        var lineas = c[6].Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split('x'))
            .Select(p => new LineaCalculo(int.Parse(p[0], Inv), decimal.Parse(p[1], Inv), p[2]))
            .ToList();

        var r = CalculoTotales.Calcular(lineas, c[2] == "V", int.Parse(c[3], Inv),
            decimal.Parse(c[4], Inv), decimal.Parse(c[5], Inv));

        Assert.Equal(decimal.Parse(c[10], Inv), r.Descuento);
        Assert.Equal(decimal.Parse(c[11], Inv), r.Flete);
        Assert.Equal(decimal.Parse(c[13], Inv), r.Iva);
        Assert.Equal(decimal.Parse(c[14], Inv), r.Total);
    }
}
