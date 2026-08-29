#!/bin/sh
printf '\033c\033]0;%s\a' GameJAM
base_path="$(dirname "$(realpath "$0")")"
"$base_path/GameJAM.x86_64" "$@"
