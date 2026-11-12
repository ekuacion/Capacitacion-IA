# ReservaCancha

Biblioteca de reservas de canchas de fútbol para el recinto deportivo **«La Pelota»**. La usa la app de reservas del recinto.

> Proyecto ficticio para demos del curso *Desarrollando con IA* (Vela LATAM).

## Stack

- .NET 10, C#, xUnit. IDE de referencia: JetBrains Rider.
- Compilar: `dotnet build` · Tests: `dotnet test`

## Estructura

```
reservacancha/
├── AGENTS.md
├── ReservaCancha.sln
├── src/ReservaCancha/
│   ├── Dominio/        ← Cancha, Reserva, ResultadoReserva
│   └── Servicios/      ← ReservaService (validaciones) y el reloj
└── tests/ReservaCancha.Tests/
    └── ReservaServiceTests.cs
```

## Reglas del negocio

### Canchas

| Id | Nombre | Tipo |
|---|---|---|
| 1 | Cancha Norte | Futbolito 5 |
| 2 | Cancha Sur | Futbolito 7 |
| 3 | Cancha Estadio | Fútbol 11 |

### Reservas

| # | Regla | Estado |
|---|---|---|
| R1 | Toda reserva dura **exactamente 1 hora**. | Implementada (la fin se calcula sola) |
| R2 | El nombre del cliente es obligatorio. | Implementada |
| R3 | La cancha debe existir. | Implementada |
| R4 | No se puede reservar en el pasado. | Implementada |
| R5 | Una cancha no puede tener dos reservas que se crucen. | Implementada |
| R6 | La reserva debe estar **dentro del horario del recinto** (tabla de abajo). | **Pendiente · issue #12** |
| R7 | La reserva debe empezar **en hora en punto** (minuto 0, segundo 0). | **Pendiente · issue #12** |

### Horario del recinto

| Días | Abre | Cierra | Último bloque |
|---|---|---|---|
| Lunes a viernes | 08:00 | 23:00 | 22:00 a 23:00 |
| Sábado y domingo | 09:00 | 22:00 | 21:00 a 22:00 |

- La reserva tiene que **terminar a más tardar a la hora de cierre**. Ejemplo: un sábado, una reserva a las 21:00 es válida; a las 22:00 no.
- Las horas son la **hora local del recinto** (Chile). No se manejan zonas horarias.
- **Feriados:** fuera de alcance por ahora (issue #15). No los consideres.

### Mensajes de error

Los mensajes se muestran tal cual al cliente en la app, así que van en español y en una sola frase:

| Regla | Mensaje |
|---|---|
| R2 | `El nombre del cliente es obligatorio.` |
| R3 | `La cancha no existe.` |
| R4 | `No se puede reservar en el pasado.` |
| R5 | `La cancha ya está reservada en ese horario.` |
| R6 | `La reserva está fuera del horario del recinto.` |
| R7 | `La reserva debe comenzar en hora en punto.` |

## Issues abiertos

- **#12 · Validación de horario.** Implementar R6 y R7 en `ReservaService.Reservar`, con sus tests.
- **#15 · Feriados.** Definir con el recinto cómo se atienden los feriados.
