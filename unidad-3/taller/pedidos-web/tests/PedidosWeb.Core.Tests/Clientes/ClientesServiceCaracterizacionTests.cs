using PedidosWeb.Core.Clientes;

namespace PedidosWeb.Core.Tests.Clientes;

/// <summary>
/// Tests de caracterización de <see cref="ClientesService"/> (TEC-01, T1).
/// Fijan lo que el código hace HOY en las partes que los 4 tests originales no cubren, para poder refactorizar con
/// una red más completa. Si uno falla durante el refactor, el comportamiento cambió: se vuelve al último verde.
/// </summary>
public class ClientesServiceCaracterizacionTests
{
    private readonly ClientesService _servicio = new();

    [Fact]
    public void ObtenerFicha_RegionVIII_DiceZonaSur()
    {
        // CP-04: C-003 es de la región VIII.
        var ficha = _servicio.ObtenerFicha("C-003");

        Assert.Equal("Botillería Los Andes | 78345612-5 | Zona sur | Activo", ficha);
    }

    [Fact]
    public void ObtenerFicha_RegionII_DiceZonaNorte()
    {
        // CP-04: C-002 es de la región II.
        var ficha = _servicio.ObtenerFicha("C-002");

        Assert.Equal("Minimarket Doña Rosa | 77654321-7 | Zona norte | Activo", ficha);
    }

    [Fact]
    public void ObtenerFicha_RegionDistintaDeLasConocidas_DiceSinZona()
    {
        // CP-04: los 8 clientes de ejemplo siempre tienen zona; se usa una lista propia.
        var servicio = new ClientesService(new List<Cliente>
        {
            new() { Id = "X-1", Nombre = "Cliente de otra región", Rut = "11.111.111-1", Region = "XV", Activo = true },
        });

        var ficha = servicio.ObtenerFicha("X-1");

        Assert.Equal("Cliente de otra región | 11111111-1 | Sin zona | Activo", ficha);
    }

    [Fact]
    public void ObtenerFicha_RegionEnMinusculas_NoSeNormalizaYDiceSinZona()
    {
        // CP-04: la región se compara de forma exacta, sin ignorar mayúsculas.
        var servicio = new ClientesService(new List<Cliente>
        {
            new() { Id = "X-1", Nombre = "Cliente", Rut = "11.111.111-1", Region = "rm", Activo = true },
        });

        var ficha = servicio.ObtenerFicha("X-1");

        Assert.Equal("Cliente | 11111111-1 | Sin zona | Activo", ficha);
    }

    [Fact]
    public void ObtenerFicha_ClienteInactivo_DiceInactivoEnLaFicha()
    {
        // CP-05: C-007 está inactivo y es de la región VIII.
        var ficha = _servicio.ObtenerFicha("C-007");

        Assert.Equal("Kiosco Central | 77321654-1 | Zona sur | Inactivo", ficha);
    }

    [Fact]
    public void ObtenerFicha_RutConEspacios_LosQuita()
    {
        // CP-03: C-008 trae el RUT como "76 432 198-7".
        var ficha = _servicio.ObtenerFicha("C-008");

        Assert.Equal("Distribuidora Norte | 76432198-7 | Zona centro | Activo", ficha);
    }

    [Fact]
    public void ObtenerFicha_IdConOtraCapitalizacion_NoLoEncuentra()
    {
        // CP-07: el id se compara de forma exacta.
        Assert.Equal("", _servicio.ObtenerFicha("c-001"));
    }

    [Fact]
    public void ObtenerFicha_IdsRepetidos_GanaElPrimero()
    {
        // CP-07: ante ids repetidos, gana el primero de la lista.
        var servicio = new ClientesService(new List<Cliente>
        {
            new() { Id = "X-1", Nombre = "Primero", Rut = "1-9", Region = "RM", Activo = true },
            new() { Id = "X-1", Nombre = "Segundo", Rut = "2-7", Region = "II", Activo = false },
        });

        Assert.Equal("Primero | 1-9 | Zona centro | Activo", servicio.ObtenerFicha("X-1"));
        Assert.True(servicio.ExisteYEstaActivo("X-1"));
    }

    [Fact]
    public void ExisteYEstaActivo_IdNull_DevuelveFalseSinLanzar()
    {
        // CP-06: un id inexistente (también null) devuelve false, no una excepción.
        Assert.False(_servicio.ExisteYEstaActivo(null!));
        Assert.Equal("", _servicio.ObtenerFicha(null!));
    }

    [Fact]
    public void ConstructorConLista_UsaLaListaRecibidaSinCopiarla()
    {
        // CP-08: la lista se usa tal cual; un cliente agregado después se ve en el servicio.
        var lista = new List<Cliente>();
        var servicio = new ClientesService(lista);
        Assert.False(servicio.ExisteYEstaActivo("X-1"));

        lista.Add(new Cliente { Id = "X-1", Nombre = "Nuevo", Rut = "1-9", Region = "RM", Activo = true });

        Assert.True(servicio.ExisteYEstaActivo("X-1"));
    }
}
