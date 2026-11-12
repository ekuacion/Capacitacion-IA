using System.Globalization;

namespace PedidosWeb.Core.Folios;

/// <summary>
/// Genera el folio de un pedido con el mismo formato del sistema legado.
/// Las reglas completas y los ejemplos están en el README, sección «La función del taller».
/// </summary>
public static class FolioFormatter
{
    /// <summary>
    /// Devuelve el folio con el formato PED-{SUCURSAL}-{NUMERO}, por ejemplo "PED-STGO-000123".
    /// </summary>
    /// <param name="numero">Número correlativo del pedido, entre 1 y 999999.</param>
    /// <param name="sucursal">Código de la sucursal: de 2 a 4 letras de la A a la Z.</param>
    public static string FormatearFolio(int numero, string sucursal)
    {
        // R5: numero se valida antes que sucursal
        // R1
        if (numero < 1 || numero > 999_999)
            throw new ArgumentOutOfRangeException(nameof(numero), numero,
                "El número debe estar entre 1 y 999999.");

        // R3 (caso null; ArgumentNullException deriva de ArgumentException)
        if (sucursal is null)
            throw new ArgumentNullException(nameof(sucursal), "La sucursal es obligatoria.");

        // R2
        string suc = sucursal.Trim().ToUpperInvariant();

        // R3 (vacía, longitud y solo A-Z ASCII)
        if (suc.Length < 2 || suc.Length > 4 || !suc.All(c => c >= 'A' && c <= 'Z'))
            throw new ArgumentException(
                "La sucursal debe tener entre 2 y 4 letras A-Z (sin Ñ, tildes, números ni espacios).",
                nameof(sucursal));

        // R4
        string num = numero.ToString("D6", CultureInfo.InvariantCulture);

        //Salida
        return $"PED-{suc}-{num}";

    }
}

/*

R1 numero debe estar entre 1 y 999999 (ambos incluidos). Si no, lanza ArgumentOutOfRangeException.
R2 A sucursal se le quitan los espacios del inicio y del final, y se pasa a mayúsculas.
R3 Después de R2, sucursal debe tener entre 2 y 4 letras de la A a la Z.Sin Ñ, sin tildes, sin números ni espacios internos: el legado guarda el código en un campo ASCII de 4 caracteres.Si no se cumple, o si sucursal es null o vacía, lanza ArgumentException(vale una derivada, como ArgumentNullException).
R4 numero se completa con ceros a la izquierda hasta 6 dígitos.
R5 Si numero y sucursal son inválidos a la vez, se valida primero numero(R1). 
*/