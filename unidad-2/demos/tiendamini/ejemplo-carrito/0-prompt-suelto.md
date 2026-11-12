# 0. El prompt suelto

Así llega muchas veces un pedido al chat de un agente:

> **"Haz un carrito con descuentos."**

Parece claro. No lo es. El agente va a escribir código igual, porque eso es lo que hace.
Cada duda que el pedido no responde, el agente la resuelve **inventando**.
Esas decisiones quedan escondidas en el código y nadie las revisa.

## 6 cosas que el agente tendría que inventar

| # | Pregunta sin respuesta | Lo que un agente probablemente supondría | Por qué es un problema |
|---|---|---|---|
| 1 | ¿Qué descuentos existen y cuánto valen? | Un campo `porcentajeDescuento` libre que manda el cliente. | Cualquiera podría pedir un 90% de descuento. |
| 2 | ¿Cómo se sabe si es la primera compra del cliente? | Un `esPrimeraCompra: true` en el request. | El dato viene del cliente: se puede falsear. |
| 3 | ¿El cupón y el despacho gratis se suman? | Sí, se aplican ambos. | El negocio dice que no son acumulables. Se pierde plata en cada venta. |
| 4 | ¿Cómo se redondea el 10% de $12.345? | `Math.Round` con `decimal`, que en .NET redondea "al par" por defecto. | Montos con centavos o redondeos distintos entre front y back. |
| 5 | ¿Qué pasa si agrego la unidad número 11? | La recorta a 10 sin avisar, o no valida nada. | El cliente no entiende por qué cambió su carrito. |
| 6 | ¿Cómo se informa un cupón inválido o vencido? | Un `400 Bad Request` con un texto en inglés. | El frontend no puede mostrar mensajes distintos por tipo de error. |

## Para conversar en clase

- ¿Cuál de estas 6 decisiones te parece más peligrosa en producción? ¿Por qué?
- ¿Cuántas de ellas descubrirías revisando el código? ¿Y revisando solo la demo funcionando?
- Ahora sigue con `1-intent.md` y luego `2-spec.md`. Busca dónde quedó resuelta cada pregunta.
