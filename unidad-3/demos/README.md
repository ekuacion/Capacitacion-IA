# Demos de la Unidad 3 · TodoMini

Las tres demos de la unidad ocurren sobre **un solo proyecto**, `todomini/`, y cuentan una historia seguida:

| Demo | Lámina | Concepto | Qué se hace |
|---|---|---|---|
| 1 · Etiquetas | 7 | C1 · Pasos chicos | Agregar etiquetas a las tareas en un solo pedido (camino A) y en tres incrementos (camino B). |
| 2 · Refactor con red | 12 | C2 · Red de tests | Escribir tests de caracterización para `ResumenTexto()` y refactorizarlo en pasos chicos. |
| 3 · Skill | 18 | C3 · Skills | Crear la Skill `escribir-readme` y pedir un README sin nombrarla. |

Después de la Demo 3 viene el recorrido por [`planificador-proyectos-web`](https://github.com/ffelipecuevasc/planificador-proyectos-web) (láminas 19–21), que no usa TodoMini.

> El paso a paso completo (prompts exactos, resultados esperados y mensajes de commit) está en las notas del instructor de la Unidad 3.

## Qué hay aquí

```
demos/
├── todomini/                  ← estado inicial de las tres demos
│   ├── AGENTS.md              ← reglas del proyecto (Claude nunca hace commit)
│   ├── TodoMini.sln
│   ├── src/TodoMini/          ← Tarea.cs y TareaService.cs (incluye ResumenTexto() heredado)
│   └── tests/TodoMini.Tests/  ← 5 tests; ResumenTexto() todavía no tiene tests
└── escribir-readme/
    └── SKILL.md               ← la Skill de la Demo 3, lista para pegar
```

TodoMini es una biblioteca .NET 10 con xUnit: **no tiene API ni endpoints** y **no tiene README de producto** (lo crea la Skill en la Demo 3).

## Cómo prepararlas

1. Copia la carpeta `todomini/` a una carpeta de trabajo **fuera** de tu clon de este repo, para que la demo tenga su propio historial de Git.
2. Ábrela en Rider (`TodoMini.sln`), compila y corre los tests: deben pasar los 5.
3. Crea un repositorio Git en esa carpeta y haz el primer commit en la rama `main`: `chore: estado inicial de TodoMini para la U3`.
4. Para repetir la demo desde cero, borra la carpeta de trabajo y vuelve a copiarla desde aquí.

Las ramas que se usan en vivo son `camino-a` y `camino-b` (Demo 1). Al final de la Demo 1, `camino-b` se fusiona en `main` y las Demos 2 y 3 siguen sobre `main`.

## Licencia

Claude Team Standard + Claude Code. Los alumnos no repiten las demos en clase; pueden hacerlo en casa.

> Todo el contenido es ficticio y tiene fines educativos.
