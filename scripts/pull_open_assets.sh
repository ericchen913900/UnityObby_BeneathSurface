#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
RAW_DIR="$ROOT_DIR/Assets/OpenSource/Raw"
mkdir -p "$RAW_DIR"

download() {
  local url="$1"
  local out="$2"
  if [ -f "$out" ]; then
    echo "skip  $out"
    return
  fi
  echo "pull  $url"
  curl -L --fail --retry 3 --retry-delay 2 -o "$out" "$url"
}

download "https://opengameart.org/sites/default/files/kenney_prototype-kit.zip" "$RAW_DIR/kenney_prototype-kit.zip"
download "https://opengameart.org/sites/default/files/kenney_prototypeTextures.zip" "$RAW_DIR/kenney_prototypeTextures.zip"
download "https://opengameart.org/sites/default/files/100-CC0-SFX_0.zip" "$RAW_DIR/100-CC0-SFX_0.zip"
download "https://opengameart.org/sites/default/files/GameLoops.zip" "$RAW_DIR/GameLoops.zip"
download "https://opengameart.org/sites/default/files/3xBlast%20-%20Free%20Music%20Pack.zip" "$RAW_DIR/3xBlast-Free-Music-Pack.zip"

echo "done  Assets/OpenSource/Raw"
