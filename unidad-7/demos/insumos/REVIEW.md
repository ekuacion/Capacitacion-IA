# Instrucciones de revisión · arriendo-web

## Qué es Importante aquí
Lo que cambia un monto cobrado al cliente (tarifas y recargos) o se aleja del legado sin una
decisión escrita. Estilo, nombres y patrones de diseño son Detalle como máximo.

## Tope de detalles
Máximo 3 detalles por revisión.

## No reportar
- Lo que ya valida el CI (compilación y tests en verde).
- migracion/tarifas-golden.csv y todo lo que está en legado/.

## Revisar siempre
1. Montos en decimal, nunca double ni float.
2. Cada regla de negocio nueva tiene un test con un valor esperado concreto.
3. Las fracciones de tiempo (horas, días) se calculan como dice la regla.

## Evidencia
Cada hallazgo cita archivo:línea.
