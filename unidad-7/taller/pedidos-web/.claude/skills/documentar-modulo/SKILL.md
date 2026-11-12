---
name: documentar-modulo
description: "Documenta un módulo de Pedidos Web (una clase o servicio de PedidosWeb.Core) en docs/modulos/<Modulo>.md, con propósito, API pública, ejemplo de uso y diagrama Mermaid, leyendo el código y los tests. Úsala cuando pidan documentar, explicar o dejar escrito cómo funciona un servicio o un módulo para el equipo, aunque no nombren la Skill."
---

# Documentar un módulo

Deja escrita la documentación de un módulo de Pedidos Web con siempre la misma estructura, a partir de lo que el código
y los tests realmente hacen.

## Pasos
1. Identifica la clase principal del módulo y su carpeta en `src/PedidosWeb.Core/`. Si no queda claro de qué módulo
   hablan, pregunta antes de seguir.
2. Lee el código de esa clase y sus tests en `tests/PedidosWeb.Core.Tests/<Área>/`. Los tests son la mejor fuente de
   casos límite y de ejemplos con resultado real.
3. Escribe `docs/modulos/<Clase>.md` con las partes de la siguiente sección. Si el archivo ya existe, actualízalo; no
   crees otro.
4. Comprueba cada firma, ejemplo y texto de salida contra el código y los tests. Usa los datos de ejemplo del
   repositorio y, si puedes, ejecuta el ejemplo para confirmar el resultado.
5. No modifiques código ni tests. Si encuentras algo raro, anótalo en «Comportamientos a tener en cuenta»; no lo arregles.

## Qué debe tener docs/modulos/<Modulo>.md
- **Propósito:** para qué existe el módulo, en pocas líneas.
- **API pública:** cada método público con su firma, qué recibe, qué devuelve y los casos límite (id que no existe,
  `null`, lista vacía, etc.). Lo privado no es API pública y no se documenta como tal.
- **Ejemplo de uso:** código C# corto y real, con el resultado que de verdad produce.
- **Diagrama Mermaid:** un bloque ` ```mermaid ` válido; un `flowchart` del método principal, que muestre sus decisiones.
- **Comportamientos a tener en cuenta:** las rarezas conocidas del módulo, escritas como notas y sin corregirlas.

## Reglas
- No inventes: si algo no está en el código ni en los tests, no lo escribas; si tienes dudas, pregunta.
- Escribe en español y usa los nombres del dominio tal como están en el código.
- Documenta lo que el módulo hace hoy, no lo que «debería» hacer.
- Verifica que el diagrama Mermaid tenga sintaxis válida antes de terminar.
