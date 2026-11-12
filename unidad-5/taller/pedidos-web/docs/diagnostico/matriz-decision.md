# Matriz de decisión de Pedidos Legado

> Etapa 3 · Decidir (T4 de `specs/TEC-03-diagnostico-legado/`). Los **pesos** y los **módulos** los decidió Claudio. Los **puntajes** los propuso
> Claude Code con la explicación de cada uno y Claudio los aceptó **sin revisión técnica propia** (no conoce Delphi); Claudio revisa solo la
> aritmética. Las cuentas las hizo un script y se pueden repetir a mano (ejemplo más abajo).
> Estrategias: **Mantener** = dejar el módulo como está; **Refactorizar** = ordenarlo por dentro, en Delphi; **Modernizar** = pasarlo a .NET
> conservando su lógica; **Reemplazar** = rediseñarlo desde cero.

## Pesos

| Criterio | Qué mide | Peso |
|---|---|---|
| Costo | Cuánto cuesta en horas y dinero | 15 % |
| Complejidad | Qué tan difícil es técnicamente | 15 % |
| Riesgo | Qué puede salir mal en el camino | 35 % |
| Impacto | Cuánto valor entrega al negocio | 25 % |
| Personas | Conocimiento disponible en el equipo | 10 % |
| **Total** | | **100 %** |

Por qué estos pesos (decididos por Claudio): el diagnóstico encontró 7 riesgos en prioridad Alta (seguridad y datos), así que Riesgo e Impacto
suman 60 %. Personas pesa poco y se apoya en que el equipo domina más .NET que Delphi (dato de Claudio).

## Matriz

Cada celda es el puntaje ponderado de la estrategia (suma de puntaje 1–5 × peso, entre 100). En cada puntaje, 5 = la mejor opción para ese
criterio (por ejemplo, en Costo, Mantener = 5 porque es lo más barato). Va en negrita el mayor de la fila.

| Módulo | Mantener | Refactorizar | Modernizar | Reemplazar | Recomendación |
|---|---|---|---|---|---|
| Cálculo de totales | 2,30 | 2,65 | **4,20** | 2,95 | **Modernizar** |
| Stock | 2,30 | 2,30 | 2,75 | **4,05** | **Reemplazar** |
| Clientes y RUT | 2,65 | 2,95 | **3,80** | 3,30 | **Modernizar** |
| Informes | 2,65 | 2,95 | 3,20 | **3,95** | **Reemplazar** |
| Toma y grabado de pedidos | 2,30 | 2,15 | 3,05 | **3,75** | **Reemplazar** |

Ninguna fila queda empatada: cada recomendación es el mayor puntaje ponderado de su fila.

### Puntajes de partida (Costo / Complejidad / Riesgo / Impacto / Personas)

| Módulo | Mantener | Refactorizar | Modernizar | Reemplazar |
|---|---|---|---|---|
| Cálculo de totales | 5/5/1/1/2 | 3/3/3/2/2 | 3/4/4/5/5 | 2/3/2/4/5 |
| Stock | 5/5/1/1/2 | 3/3/2/2/2 | 3/3/2/3/4 | 3/3/4/5/5 |
| Clientes y RUT | 5/5/2/1/2 | 4/4/3/2/2 | 3/3/4/4/5 | 2/3/3/4/5 |
| Informes | 5/5/2/1/2 | 4/4/3/2/2 | 3/3/3/3/5 | 3/4/4/4/5 |
| Toma y grabado de pedidos | 5/5/1/1/2 | 3/2/2/2/2 | 2/2/3/4/4 | 2/2/4/5/5 |

**Cuenta de ejemplo para revisar a mano** (Cálculo de totales, Modernizar): 3×0,15 + 4×0,15 + 4×0,35 + 5×0,25 + 5×0,10 = 0,45 + 0,60 + 1,40 + 1,25 + 0,50 = **4,20**.
*Aritmética revisada por Claudio: confirmada (4,20).*

### Por qué se puntuó así

- **Mantener** es lo más barato y simple (5/5) en todos los módulos, pero deja vivos los riesgos Alta del diagnóstico (RS-01 a RS-07): saca 1 o 2 en Riesgo e Impacto.
- **Cálculo de totales → Modernizar.** Es lógica pura, sin pantallas ni base (`dependencias.md`, pregunta 2), así que se porta y se prueba con poco riesgo. Reemplazar puntúa peor porque la cabecera del código pide no tocarlo sin hablar con contabilidad y reescribir de cero arriesga perder reglas (RS-07; reglas 1, 3, 5 a 7).
- **Stock → Reemplazar.** El defecto es de diseño: avisa el rechazo después de grabar y registra en una tabla que no existe (RS-04, RS-06; regla 10). Portarlo tal cual (Modernizar) conserva el defecto.
- **Clientes y RUT → Modernizar.** Es un mantenedor simple; al portarlo se unifica la validación del RUT, hoy con dos criterios (RS-08; reglas 13 y 15).
- **Informes → Reemplazar.** Es un informe de lectura sobre la base nueva; hoy tiene IVA fijo y fechas de 2 dígitos (RS-09, RS-11; regla 7). Es la diferencia más estrecha de la matriz (3,95 frente a 3,20 de Modernizar).
- **Toma y grabado → Reemplazar.** Concentra folio `MAX + 1`, falta de transacción y lógica de negocio dentro del formulario (RS-05, RS-13; reglas 8, 9 y 11). Su costo es el más alto, pero Riesgo e Impacto pesan 60 % entre los dos.

## Resumen ejecutivo

1. **Qué es y su estado:** Pedidos Legado es el sistema de toma de pedidos de la distribuidora, escrito en Delphi sobre la base DBISAM (16 archivos y 2.375 líneas); funciona, pero su documentación no calza con el código y sus reglas de negocio están repetidas y dispersas.
2. **Hallazgo principal (con evidencia):** el total de un pedido se calcula dos veces con reglas distintas, y el monto que ve el vendedor en pantalla difiere del que se guarda justo en $500.000 y $1.000.000 (`legado/src/UCalculoTotales.pas:77-80` frente a `legado/src/UFrmPedidos.pas:296-301`, verificado por Claudio).
3. **Riesgo más urgente:** la seguridad; la clave de la base está escrita en el código y cualquiera entra como administrador escribiendo `ADMIN`, sin contraseña, además de consultas armadas con lo que teclea el usuario, que permiten manipular la base (`legado/src/UDMPrincipal.pas:41`, `legado/src/UFrmPrincipal.pas:46-50`, `legado/src/UFrmPedidos.pas:102`).
4. **Estrategia y por dónde empezar:** según la matriz, pasar a .NET conservando las reglas el cálculo de totales y los clientes, y rediseñar el stock, los informes y la toma de pedidos; empezar por el cálculo de totales (puntaje 4,20), porque no depende de pantallas ni de la base.
5. **Próximo paso que se pide aprobar:** iniciar la migración del cálculo de totales y que contabilidad confirme qué regla de descuento vale en $500.000 y $1.000.000, y que el pedido mínimo es $30.000 (hoy el parámetro de la base dice $25.000).
