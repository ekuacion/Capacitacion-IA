# ClientesService

Módulo: `src/PedidosWeb.Core/Clientes/ClientesService.cs` · Tests: `tests/PedidosWeb.Core.Tests/Clientes/`

## Propósito

Servicio de lectura de clientes. Arma la ficha de un cliente para la pantalla y dice si un cliente existe y está activo.
Es un servicio heredado que se ordenó en TEC-01 sin cambiar su comportamiento; 14 tests lo dejan fijado
(`ClientesServiceTests` y `ClientesServiceCaracterizacionTests`).

## API pública

### Constructores

| Firma | Qué hace |
|---|---|
| `ClientesService()` | Usa los 8 clientes de `ClientesDeEjemplo.Crear()`. |
| `ClientesService(List<Cliente> clientes)` | Usa la lista recibida tal cual, sin copiarla. |

### `string ObtenerFicha(string id)`

Devuelve la ficha del cliente con el formato `Nombre | RUT | Zona | Estado`.

- **RUT:** sin puntos ni espacios y en mayúscula (`76.990.011-k` → `76990011-K`).
- **Zona**, según la región (comparación exacta):

  | Región | Zona |
  |---|---|
  | `RM`, `V` | Zona centro |
  | `VIII` | Zona sur |
  | `II` | Zona norte |
  | cualquier otra | Sin zona |

- **Estado:** `Activo` o `Inactivo`.
- **Casos límite:** un id que no existe o `null` devuelve `""` (texto vacío), sin lanzar excepción.

### `bool ExisteYEstaActivo(string id)`

Devuelve `true` solo si el cliente existe y está activo.

- **Casos límite:** un id que no existe o `null` devuelve `false`, sin lanzar excepción. Un cliente inactivo también da `false`.

## Ejemplo de uso

```csharp
var servicio = new ClientesService();

servicio.ObtenerFicha("C-001");
// "Ferretería El Clavo | 76123456-0 | Zona centro | Activo"

servicio.ObtenerFicha("C-004");
// "Almacén La Esquina | 76990011-K | Zona centro | Activo"

servicio.ObtenerFicha("C-007");
// "Kiosco Central | 77321654-1 | Zona sur | Inactivo"

servicio.ObtenerFicha("C-999");
// ""

servicio.ExisteYEstaActivo("C-001"); // true
servicio.ExisteYEstaActivo("C-007"); // false (inactivo)
servicio.ExisteYEstaActivo("C-999"); // false (no existe)
```

## Diagrama de `ObtenerFicha`

```mermaid
flowchart TD
    A([ObtenerFicha id]) --> B{¿Existe un cliente con ese id?}
    B -- No --> C([Devuelve texto vacío])
    B -- Sí --> D[Normaliza el RUT: quita puntos y espacios, mayúscula]
    D --> E{Región}
    E -- RM o V --> F[Zona centro]
    E -- VIII --> G[Zona sur]
    E -- II --> H[Zona norte]
    E -- otra --> I[Sin zona]
    F --> J{¿Activo?}
    G --> J
    H --> J
    I --> J
    J -- Sí --> K[Estado: Activo]
    J -- No --> L[Estado: Inactivo]
    K --> M([Devuelve Nombre, RUT, Zona y Estado separados por ' | '])
    L --> M
```

Si tu editor no dibuja Mermaid, la misma lógica en texto plano:

```text
ObtenerFicha(id)
  |
  +-- ¿existe el cliente con ese id?
        |
        +-- No --> devuelve "" (texto vacío)
        |
        +-- Sí --> RUT: quita puntos y espacios, pasa a mayúscula
                   |
                   +-- Zona según la región:
                   |     RM o V  --> Zona centro
                   |     VIII    --> Zona sur
                   |     II      --> Zona norte
                   |     otra    --> Sin zona
                   |
                   +-- Estado: Activo / Inactivo
                   |
                   +--> devuelve "Nombre | RUT | Zona | Estado"
```

## Comportamientos a tener en cuenta

Son rarezas del código actual; se documentan, no se corrigen.

- **El id se compara de forma exacta:** `c-001` no encuentra a `C-001` y `ObtenerFicha` devuelve `""`.
- **La región también se compara exacta:** `rm` en minúsculas no se normaliza y da «Sin zona».
- **Ids repetidos:** gana el primer cliente de la lista, tanto en `ObtenerFicha` como en `ExisteYEstaActivo`.
- **La lista no se copia:** con `ClientesService(List<Cliente>)`, un cliente agregado a la lista después se ve en el servicio.
- **`""` no distingue casos:** `ObtenerFicha` devuelve `""` tanto para un id inexistente como para `null`.
