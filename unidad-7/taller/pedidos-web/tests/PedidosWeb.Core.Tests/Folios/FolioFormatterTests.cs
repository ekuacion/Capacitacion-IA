using PedidosWeb.Core.Folios;

namespace PedidosWeb.Core.Tests.Folios;

public class FolioFormatterTests
{
    // Casos base del README (tres primeras filas de la tabla de ejemplos).
    [Theory]
    [InlineData(123, "STGO", "PED-STGO-000123")]
    [InlineData(1, "VAL", "PED-VAL-000001")]
    [InlineData(999999, "CO", "PED-CO-999999")]
    public void FormatearFolio_CasosBase_DevuelveElFolioEsperado(int numero, string sucursal, string esperado)
    {
        var folio = FolioFormatter.FormatearFolio(numero, sucursal);

        Assert.Equal(esperado, folio);
    }

    // R2: se quitan los espacios del inicio y del final, y se pasa a mayúsculas.
    [Fact]
    public void FormatearFolio_SucursalConEspaciosYMinusculas_LaNormaliza()
    {
        var folio = FolioFormatter.FormatearFolio(45, " stgo ");

        Assert.Equal("PED-STGO-000045", folio);
    }

    // R1: numero entre 1 y 999999, ambos incluidos.
    [Theory]
    [InlineData(0, "STGO")]
    [InlineData(1000000, "STGO")]
    [InlineData(-5, "VAL")]
    public void FormatearFolio_NumeroFueraDeRango_LanzaArgumentOutOfRangeException(int numero, string sucursal)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => FolioFormatter.FormatearFolio(numero, sucursal));
    }

    // R3: sucursal vacía o solo con espacios.
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void FormatearFolio_SucursalVaciaOEnBlanco_LanzaArgumentException(string sucursal)
    {
        AssertErrorDeSucursal(() => FolioFormatter.FormatearFolio(7, sucursal));
    }

    // R3: sucursal null (vale ArgumentNullException, que deriva de ArgumentException).
    [Fact]
    public void FormatearFolio_SucursalNull_LanzaArgumentException()
    {
        AssertErrorDeSucursal(() => FolioFormatter.FormatearFolio(7, null!));
    }

    // R3: entre 2 y 4 letras de la A a la Z, sin Ñ, tildes, números ni espacios internos.
    // Las cuatro primeras filas son de la tabla del README; las dos últimas salen del texto de R3.
    [Theory]
    [InlineData("S")]      // 1 letra
    [InlineData("SANTI")]  // 5 letras
    [InlineData("ÑUÑO")]   // la Ñ no es A-Z
    [InlineData("ST1")]    // tiene un número
    [InlineData("ST GO")]  // espacio interno
    [InlineData("ÁNT")]    // tilde
    public void FormatearFolio_SucursalInvalida_LanzaArgumentException(string sucursal)
    {
        AssertErrorDeSucursal(() => FolioFormatter.FormatearFolio(7, sucursal));
    }

    // R5: si numero y sucursal son inválidos a la vez, se valida primero numero.
    [Fact]
    public void FormatearFolio_NumeroYSucursalInvalidos_ValidaPrimeroElNumero()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => FolioFormatter.FormatearFolio(0, "S"));
    }

    // ArgumentOutOfRangeException también deriva de ArgumentException: se descarta para que
    // un error de sucursal no pase el test con la excepción que corresponde a numero (R1).
    private static void AssertErrorDeSucursal(Action accion)
    {
        var excepcion = Assert.ThrowsAny<ArgumentException>(accion);

        Assert.IsNotType<ArgumentOutOfRangeException>(excepcion);
    }
}
