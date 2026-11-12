# TodoMini

Lista de tareas mínima: crear, completar y listar tareas. Es una biblioteca C# con tests xUnit.

> Proyecto ficticio para demos del curso *Desarrollando con IA* (Vela LATAM).

## Stack

- .NET 10, C#, xUnit. IDE de referencia: JetBrains Rider.
- Compilar: `dotnet build` · Tests: `dotnet test`

## Estructura

```
todomini/
├── AGENTS.md
├── TodoMini.sln
├── src/TodoMini/
│   ├── Tarea.cs           ← la tarea: id, título, completada, fecha de creación
│   └── TareaService.cs    ← crear, completar y listar
└── tests/TodoMini.Tests/
    └── TareaServiceTests.cs
```

## Qué hace hoy

| Operación | Comportamiento |
|---|---|
| `Crear(titulo)` | Crea una tarea pendiente con id correlativo y fecha de creación (UTC). Quita los espacios del título. Si el título está vacío, lanza `ArgumentException`. |
| `Completar(id)` | Marca la tarea como completada. Devuelve `false` si el id no existe. |
| `Listar()` | Todas las tareas, en orden de creación. |
| `ListarPendientes()` | Solo las tareas no completadas. |

## Backlog

- **Fecha de vencimiento.** Las tareas todavía no tienen fecha de vencimiento.
- **Prioridad.** Alta, media o baja.
