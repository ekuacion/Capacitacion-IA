namespace TodoMini.Tests;

public class TareaServiceTests
{
    // Reloj fijo para que los tests no dependan de la hora real.
    private sealed class RelojFijo : TimeProvider
    {
        public static readonly DateTimeOffset Ahora = new(2026, 11, 4, 13, 0, 0, TimeSpan.Zero);

        public override DateTimeOffset GetUtcNow() => Ahora;
    }

    private readonly TareaService _servicio = new(new RelojFijo());

    [Fact]
    public void Crear_TituloValido_AsignaIdYFechaDeCreacion()
    {
        var tarea = _servicio.Crear("  Comprar pan  ");

        Assert.Equal(1, tarea.Id);
        Assert.Equal("Comprar pan", tarea.Titulo);
        Assert.Equal(RelojFijo.Ahora, tarea.CreadaEn);
        Assert.False(tarea.Completada);
    }

    [Fact]
    public void Crear_TituloVacio_LanzaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _servicio.Crear("   "));
    }

    [Fact]
    public void Completar_TareaExistente_QuedaCompletada()
    {
        var tarea = _servicio.Crear("Pagar la luz");

        var resultado = _servicio.Completar(tarea.Id);

        Assert.True(resultado);
        Assert.True(tarea.Completada);
    }

    [Fact]
    public void Completar_IdInexistente_DevuelveFalse()
    {
        Assert.False(_servicio.Completar(99));
    }

    [Fact]
    public void ListarPendientes_ConUnaCompletada_DevuelveSoloLasPendientes()
    {
        var pan = _servicio.Crear("Comprar pan");
        _servicio.Crear("Pagar la luz");
        _servicio.Completar(pan.Id);

        var pendientes = _servicio.ListarPendientes();

        Assert.Single(pendientes);
        Assert.Equal("Pagar la luz", pendientes[0].Titulo);
    }
}
