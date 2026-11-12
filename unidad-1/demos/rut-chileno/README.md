# Demo · Mismo prompt, dos respuestas

Unidad 1 · Concepto 1 (los LLM) · lámina 12 · 5 minutos.

**Qué muestra:** un LLM no es determinista. El mismo prompt da dos funciones distintas, con supuestos distintos, y alguna puede fallar con el dígito verificador «K». Si el resultado varía, la verificación no es opcional.

## Cómo reproducirla

1. Abre **dos chats nuevos** en [claude.ai](https://claude.ai) (no reutilices conversaciones).
2. Pega exactamente el mismo prompt en los dos:

   ```
   Escribe una función JavaScript que valide un RUT chileno.
   ```

3. Compara las dos respuestas: nombres, estructura, si aceptan puntos y guion, si normalizan la `k` minúscula y si validan el largo.
4. Prueba las dos funciones con la tabla de abajo (por ejemplo, en la consola del navegador).

## Casos de prueba

El dígito verificador se calcula con el algoritmo módulo 11 (factores 2 a 7 de derecha a izquierda).

| Entrada | Resultado esperado | Qué revela |
|---|---|---|
| `12.345.678-5` | Válido | Caso base con puntos y guion |
| `12345678-5` | Válido | ¿Acepta el RUT sin puntos? |
| `10.000.013-K` | Válido | Dígito verificador K mayúscula |
| `10000013-k` | Válido | ¿Normaliza la k minúscula? |
| `15.000.013-0` | Válido | Dígito verificador 0 (resto 11) |
| `12.345.678-K` | Inválido | DV incorrecto: debe rechazarlo |
| `12345678` | Inválido (o ambiguo) | Sin DV: ¿qué supuesto tomó el modelo? |
| `""` o `"abc"` | Inválido | Entradas basura: ¿lanza error o devuelve `false`? |

## Preguntas para pensar

- ¿Las dos funciones son iguales?
- ¿Qué supuestos inventó cada una?
- ¿Cuál llevarías a producción sin tests?
