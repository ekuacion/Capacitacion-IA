# Material del curso · Desarrollando con IA

Proyectos de las demos y de los talleres del programa **«Desarrollando con Inteligencia — IA para Desarrolladores»** (Vela LATAM, 2026).

Sitio del curso: [ffelipecuevasc.github.io/curso-ia-desarrolladores](https://ffelipecuevasc.github.io/curso-ia-desarrolladores/)

## Cómo está organizado

Cada unidad tiene su carpeta con dos partes:

```
unidad-N/
├── demos/    ← lo que el instructor muestra en clase (puedes reproducirlo en casa)
└── taller/   ← lo que tú usas en el taller práctico
```

| Unidad | Demos | Taller |
|---|---|---|
| 1 · IA generativa y SDLC | `reservacancha/`, `claude-project-reservacancha/`, `rut-chileno/` | `pedidos-web/` (caso del curso), `caso-scrum-y-sdd.pdf` |
| 2 · Desarrollo agéntico y SDD | `todomini/`, `tiendamini/` | `pedidos-web/` (base de la U2), `u2-respaldo/`, `prompt-maestro.md` |
| 3 · Generar, refactorizar y documentar | `todomini/` (las 3 demos), `escribir-readme/` | `pedidos-web/` (base de la U3), `u3-respaldo/` |
| 4 · Análisis y diagnóstico de sistemas legados | `arriendo-legado/` (las 4 demos) | `pedidos-web/` (base de la U4), `u4-respaldo/` |
| 5 · Modernización y migración | `arriendo-web/` (las 3 demos) | `pedidos-web/` (base de la U5), `u5-respaldo/` |
| 6 · Calidad, pruebas y CI/CD | `arriendo-web/` (las 4 demos) | `pedidos-web/` (base de la U6), `u6-respaldo/` |
| 7 · Code review y colaboración | `arriendo-web/` (las 3 demos), `insumos/` | `pedidos-web/` (base de la U7) |
| 8 · Gobernanza, seguridad y uso responsable | `arriendo-web/` (las 3 demos), `insumos/` | `pedidos-web/` (base de la U8) |

## El caso del curso: `pedidos-web`

`unidad-1/taller/pedidos-web` se entrega **una sola vez**. En la Unidad 1 lo copias a **tu propio repositorio de GitHub** y trabajas en él hasta la Unidad 10. Los pasos están en su `README.md`.

Desde la **Unidad 2** no se copia otro repo: actualizas tu repo propio con el **camino A** del README de `unidad-N/taller/pedidos-web`, que trae **solo lo nuevo** de esa unidad.

- **Unidad 2:** API, pedidos de ejemplo y `AGENTS.md` vacío.
- **Unidad 3:** líneas de los pedidos, `ClientesService` heredado y dos plantillas. Solo se suman carpetas: no reemplaza nada tuyo.
- **Unidad 4:** el código de Pedidos Legado en `legado/` (solo lectura) y las plantillas del diagnóstico. Solo se suman carpetas.
- **Unidad 5:** el golden master del cálculo de totales en `migracion/` y dos plantillas en `docs/migracion/`. Solo se suman carpetas.
- **Unidad 6:** una plantilla en `docs/calidad/`. Los workflows de `.github/workflows/` los creas tú en el taller. Solo se suma una carpeta.
- **Unidad 7:** la historia `specs/HU-03-pedidos-por-fecha/` y la plantilla `docs/checklist-revision.md`. `REVIEW.md`, el subagente `revisor` y `claude-review.yml` los creas tú en el taller. Solo se suman archivos.
- **Unidad 8:** dos plantillas en `docs/gobernanza/`. Las reglas nuevas de `.claude/settings.json`, el hook `guardian.sh` y `dependabot.yml` los creas tú en el taller: `settings.json` se amplía, nunca se reemplaza. Solo se suma una carpeta.

Los respaldos (`u2-respaldo/`, `u3-respaldo/`, `u4-respaldo/`, `u5-respaldo/`, `u6-respaldo/`) son **carpetas**, no ramas.

> Todo el contenido es ficticio y tiene fines educativos.
