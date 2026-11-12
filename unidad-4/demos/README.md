# Demos de la Unidad 4 · ArriendoLegado

Las cuatro demos de la unidad ocurren sobre **un solo proyecto**, `arriendo-legado/`, y cuentan una historia seguida: entender un sistema que nadie documentó, buscar sus reglas con ayuda, comprobarlas leyendo el código y decidir qué hacer con cada módulo.

| Demo | Lámina | Concepto | Qué se hace |
|---|---|---|---|
| 1 · Arqueología en plan mode | 7 | Entender sin tocar | Recorrer el proyecto en plan mode: inventario de archivos y diagrama de dependencias con la base de datos. |
| 2 · Mi subagente analista | 11 | Subagentes de solo lectura | Crear el subagente `analista-legado` (`tools: Read, Grep, Glob`) y pedirle reglas con evidencia `archivo:línea`. |
| 3 · Reglas ocultas | 15–16 | Verificar leyendo el código | En parejas, leer el método `btnCalcularClick` de `UFrmArriendo.pas` y anotar sus reglas; después comparar con lo que encuentra Claude. |
| 4 · Matriz de decisión | 19 | Decidir con criterios | Puntuar los módulos del sistema (mantener, refactorizar, modernizar o reemplazar) y sacar una recomendación de la matriz. |

Después de la Demo 4 viene la votación «¿Qué harías tú?» (lámina 20), que no usa el proyecto.

> El paso a paso completo (prompts exactos y resultados esperados) está en las notas del instructor de la Unidad 4.

## Qué hay aquí

```
demos/
├── arriendo-legado/          ← estado inicial de las cuatro demos
│   ├── ArriendoLegado.dpr
│   ├── README.md             ← escueto, como el de un legado real
│   ├── src/                  ← 5 unidades .pas y 2 formularios .dfm (formato texto)
│   └── db/                   ← esquema DBISAM y datos de ejemplo en CSV
└── analista-legado.md        ← el subagente de la Demo 2, listo para pegar en .claude/agents/
```

ArriendoLegado es un sistema **ficticio** de arriendo de autos, hecho en Delphi 7 con DBISAM 4. **No compila ni hay que instalar Delphi**: el código se lee, no se ejecuta. No trae `.claude/` ni `AGENTS.md`: el subagente se crea en vivo en la Demo 2.

## Cómo prepararlas

1. Copia la carpeta `arriendo-legado/` a una carpeta de trabajo **fuera** de tu clon de este repo, para que la demo tenga su propio historial de Git.
2. Ábrela en Rider como carpeta (no tiene `.sln`). *(Verificar en tu versión de Rider: la opción para abrir una carpeta puede cambiar de nombre.)* No se compila.
3. Crea un repositorio Git en esa carpeta y haz el primer commit en la rama `main`: `chore: estado inicial de ArriendoLegado para la U4`.
4. Para repetir las demos desde cero, borra la carpeta de trabajo y vuelve a copiarla desde aquí.

No se usan ramas: las cuatro demos ocurren sobre `main`.

## Licencia

Claude Team Standard + Claude Code. Los alumnos no repiten las demos en clase; pueden hacerlo en casa.

> Todo el contenido es ficticio y tiene fines educativos.
