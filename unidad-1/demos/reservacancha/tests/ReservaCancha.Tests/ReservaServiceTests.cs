using ReservaCancha.Dominio;
using ReservaCancha.Servicios;

namespace ReservaCancha.Tests;

public class ReservaServiceTests
{
    // «Ahora» fijo: lunes 2 de noviembre de 2026, 10:00.
    private sealed class RelojFijo : IReloj
    {
        public DateTime Ahora { get; } = new(2026, 11, 2, 10, 0, 0);
    }

    private static ReservaService CrearServicio() => new(Cancha.DelRecinto, new RelojFijo());

    // Martes 3 de noviembre de 2026, 19:00: horario válido.
    private static readonly DateTime MartesALas19 = new(2026, 11, 3, 19, 0, 0);

    [Fact]
    public void Reservar_CanchaLibre_QuedaRegistrada()
    {
        var servicio = CrearServicio();

        var resultado = servicio.Reservar(new Reserva(1, MartesALas19, "Camila Rojas"));

        Assert.True(resultado.Exito);
        Assert.Single(servicio.Reservas);
    }

    [Fact]
    public void Reservar_SinCliente_Falla()
    {
        var resultado = CrearServicio().Reservar(new Reserva(1, MartesALas19, "  "));

        Assert.False(resultado.Exito);
        Assert.Equal("El nombre del cliente es obligatorio.", resultado.Error);
    }

    [Fact]
    public void Reservar_CanchaInexistente_Falla()
    {
        var resultado = CrearServicio().Reservar(new Reserva(99, MartesALas19, "Camila Rojas"));

        Assert.False(resultado.Exito);
        Assert.Equal("La cancha no existe.", resultado.Error);
    }

    [Fact]
    public void Reservar_EnElPasado_Falla()
    {
        // Domingo 1 de noviembre de 2026, 19:00: antes de «ahora».
        var ayer = new DateTime(2026, 11, 1, 19, 0, 0);

        var resultado = CrearServicio().Reservar(new Reserva(1, ayer, "Camila Rojas"));

        Assert.False(resultado.Exito);
        Assert.Equal("No se puede reservar en el pasado.", resultado.Error);
    }

    [Fact]
    public void Reservar_MismaCanchaYMismaHora_LaSegundaFalla()
    {
        var servicio = CrearServicio();
        servicio.Reservar(new Reserva(2, MartesALas19, "Camila Rojas"));

        var resultado = servicio.Reservar(new Reserva(2, MartesALas19, "Diego Soto"));

        Assert.False(resultado.Exito);
        Assert.Equal("La cancha ya está reservada en ese horario.", resultado.Error);
    }

    [Fact]
    public void Reservar_OtraCanchaALaMismaHora_QuedaRegistrada()
    {
        var servicio = CrearServicio();
        servicio.Reservar(new Reserva(2, MartesALas19, "Camila Rojas"));

        var resultado = servicio.Reservar(new Reserva(3, MartesALas19, "Diego Soto"));

        Assert.True(resultado.Exito);
        Assert.Equal(2, servicio.Reservas.Count);
    }
}
