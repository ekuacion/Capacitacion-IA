# Unidad 2 — Prompt maestro del taller

**Para qué sirve:** genera en minutos los borradores de `AGENTS.md` y `spec.md` del taller, para que dediques tu tiempo a **decidir** y no a darle formato.
**Dónde se usa:** en Claude Code, abierto en la raíz de **tu** repositorio `pedidos-web`, ya actualizado con la base de la Unidad 2 (README, «Cómo empezar»), en el **paso 1** del taller.
**Importante:**
- Tu `AGENTS.md` viene **vacío a propósito**. Lo escribes tú: Claude propone un borrador con lo que encuentra en el repo y con lo que tú le respondes.
- El prompt está diseñado para que Claude **no tome las decisiones por ti**. Las deja marcadas como `TODO` para que las completes en el paso 2.
- **No ejecutes `/init`**: crea un `CLAUDE.md`, y mientras exista un `CLAUDE.md`, Claude Code deja de leer tu `AGENTS.md`.

---

## Antes del paso 1 (en el receso, 3 minutos)

Lee la portada de [agents.md](https://agents.md/). Quédate con tres ideas:

1. `AGENTS.md` es **Markdown normal, sin campos obligatorios**. Lo leen varios agentes: Claude Code, Codex, Copilot y otros.
2. Secciones recomendadas: descripción del proyecto, comandos para compilar y testear, estilo de código, instrucciones de tests y consideraciones de seguridad. Se pueden sumar reglas para commits y pull requests.
3. Se escribe lo que el agente **no puede adivinar**. Lo obvio y lo que no es verdad en el repo sobra.

> ¿Vienes de la Unidad 1? Tus reglas anteriores siguen en el historial de Git: `git log -p -- AGENTS.md`. Puedes rescatar las que te sirvan.

---

## Cómo usarlo

1. Abre Rider en tu repositorio `pedidos-web` y, en la terminal integrada, inicia Claude Code directo en plan mode:

   ```bash
   claude --permission-mode plan
   ```

   Si ya lo tenías abierto, presiona `Shift+Tab` hasta que la barra muestre `⏸ plan mode on`. En plan mode, Claude explora y propone, pero no edita tus archivos.
2. Copia y pega el prompt completo.
3. Responde las preguntas de Claude con frases cortas.
4. Cuando Claude te muestre los dos borradores, presiona `Shift+Tab` hasta ver `⏸ manual mode on` y pídele: *«Escribe los dos archivos»*. En modo manual, Claude te pide permiso antes de cada archivo: **léelo antes de aceptar**.

---

## Prompt (copiar desde aquí)

```text
Actúa como analista técnico de un equipo Scrum. Soy el Product Owner y el desarrollador
de esta historia, y quiero aplicar Spec-Driven Development.

CONTEXTO DEL PROYECTO
- Repositorio: portal web "Pedidos". API REST en ASP.NET Core (.NET 10) con tests xUnit.
- Los datos son de ejemplo y viven en memoria (no hay base de datos).
- El AGENTS.md está vacío a propósito: lo vamos a escribir juntos.
- Historia de usuario del backlog:
  "Como vendedor, quiero listar pedidos filtrando por cliente, estado y rango de fechas."

TU TAREA, EN ESTE ORDEN
1. Explora el repositorio SIN modificar nada: estructura, proyectos, modelos, datos de
   ejemplo, endpoints y tests existentes. Anota los comandos y convenciones que puedas
   comprobar en los archivos.
2. Hazme una entrevista de máximo 6 preguntas, de a una por vez. Las 2 primeras sobre cómo
   quiero que trabajes en este repo (por ejemplo, qué puedes hacer sin preguntarme).
   El resto, sobre la historia. Pregunta solo lo que el código no responde.
3. Con lo que encontraste y con mis respuestas, genera DOS borradores:

   a) AGENTS.md (máximo 40 líneas, en español), con las secciones que recomienda agents.md:
      - Proyecto: qué es, en 2 líneas.
      - Comandos: compilar, testear y ejecutar la API. Solo los que comprobaste en el repo.
      - Estilo de código: convenciones detectadas (idioma de los nombres, tipo para el
        dinero, tipo para las fechas, qué va en Core y qué va en Api).
      - Tests: framework, dónde viven, cómo se nombran y cuándo se ejecutan.
      - Seguridad: nunca secretos ni claves en el código.
      - Reglas de trabajo. Incluye siempre estas tres:
        "toda funcionalidad parte de una spec aprobada en specs/<historia>/spec.md",
        "un test xUnit por criterio de aceptación",
        "no hacer commit sin tests en verde".
        Agrega solo las que salgan de mis respuestas.

   b) specs/HU-01-listar-pedidos/spec.md, con estas secciones:
      - Historia de usuario
      - Objetivo (1 párrafo)
      - Reglas de negocio
      - Criterios de aceptación (checklist verificable, con los datos de ejemplo del repo)
      - Ejemplos de entrada y salida
      - Errores esperados
      - Fuera de alcance
      - Preguntas abiertas

REGLAS QUE NO PUEDES ROMPER
- NO inventes reglas de negocio ni reglas de trabajo. Si una decisión no salió de mis
  respuestas, escribe "TODO: decidir ..." y explica en una línea por qué importa.
- En AGENTS.md no escribas comandos ni convenciones que no hayas visto en el repo.
- En la spec, deja como TODO, como mínimo: la lista de estados válidos, qué pasa con
  fechas invertidas, el tamaño de página y el formato de error.
- NO escribas código ni tests. NO crees plan.md: eso lo haremos después en plan mode.
- Al final, muéstrame la lista de TODO pendientes de los dos archivos.
```

---

## Después de ejecutarlo: tu checklist

**AGENTS.md**
- [ ] Lo leí completo y corregí lo que no es verdad en este repo.
- [ ] Probé los comandos que dice (`dotnet build`, `dotnet test`) y funcionan.
- [ ] Tiene las tres reglas SDD y las reglas de trabajo que **yo** decidí.
- [ ] No tiene secretos, obviedades ni más de 40 líneas.
- [ ] Al abrir una sesión nueva de `claude`, la conversación muestra una línea parecida a «no CLAUDE.md found; AGENTS.md loaded». Si no la ves, revisa que no exista un `CLAUDE.md` en el proyecto ni en las carpetas superiores.

**spec.md**
- [ ] Resolví **todos** los `TODO` con decisiones propias.
- [ ] Tengo al menos 5 criterios de aceptación, y cada uno se puede probar con un test.
- [ ] Hay al menos 2 ejemplos de entrada y salida, incluido uno de error.
- [ ] La sección «Fuera de alcance» dice explícitamente qué **no** se va a construir.

> **Recuerda:** si al completar la spec no sabes cómo escribir el test de un criterio, el criterio todavía no está bien escrito.
