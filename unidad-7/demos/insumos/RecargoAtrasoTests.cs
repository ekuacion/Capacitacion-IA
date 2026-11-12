using ArriendoWeb.Core;

namespace ArriendoWeb.Tests;

/// <summary>Tests del recargo por atraso (Unidad 7, demo 1).</summary>
public class RecargoAtrasoTests
{
    [Fact]
    public void Sin_atraso_no_hay_recargo()
    {
        Assert.Equal(0m, Recargos.RecargoAtraso(25000m, 0));
    }

    [Fact]
    public void Dos_dias_justos_cobran_dos_medias_tarifas()
    {
        Assert.Equal(25000m, Recargos.RecargoAtraso(25000m, 48));
    }

    [Fact]
    public void Con_atraso_hay_recargo()
    {
        var recargo = Recargos.RecargoAtraso(25000m, 50);

        Assert.True(recargo > 0);
    }
}
