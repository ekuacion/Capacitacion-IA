using System.Globalization;
using ArriendoWeb.Core;

namespace ArriendoWeb.Tests;

/// <summary>
/// Golden master de TarifaBase: un caso por fila de migracion/tarifas-golden.csv.
/// El CSV es el contrato: no se edita para que un test pase.
/// </summary>
public class TarifaBaseGoldenTests
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    public static IEnumerable<object[]> Casos() =>
        File.ReadLines(Path.Combine(AppContext.BaseDirectory, "tarifas-golden.csv"))
            .Skip(1)
            .Where(linea => !string.IsNullOrWhiteSpace(linea))
            .Select(linea => linea.Split(','))
            .Select(c => new object[] { c[0], c[1], c[2], c[3], c[4], c[5] });

    [Theory]
    [MemberData(nameof(Casos))]
    public void TarifaBase_igual_al_legado(string caso, string tarifaDia, string dias,
        string fechaSalida, string cantArriendos, string esperado)
    {
        var resultado = Tarifas.TarifaBase(
            decimal.Parse(tarifaDia, Inv),
            int.Parse(dias, Inv),
            DateTime.ParseExact(fechaSalida, "yyyy-MM-dd", Inv),
            int.Parse(cantArriendos, Inv));

        Assert.True(decimal.Parse(esperado, Inv) == resultado,
            $"Caso {caso}: esperado {esperado}, obtenido {resultado}");
    }
}
