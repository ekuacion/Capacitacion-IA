#!/usr/bin/env bash
# guardian.sh · hook PreToolUse · bloquea acciones que nombran .env o un .pem
entrada=$(cat)
# Solo mira rutas, patrones y comandos; no el contenido que se escribe.
claves='"(file_path|path|pattern|command)"[[:space:]]*:[[:space:]]*"[^"]*"'
campos=$(printf '%s' "$entrada" | grep -oE "$claves")
b='[^[:alnum:]_]'
if printf '%s' "$campos" | grep -qE "(^|$b)\.env($b|\$)|\.pem($b|\$)"; then
  echo "guardian.sh: bloqueado, la accion nombra .env o un archivo .pem" >&2
  exit 2
fi
exit 0
