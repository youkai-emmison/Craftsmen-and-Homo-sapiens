#!/usr/bin/env bash
# Validates that the Unity WebGL static site exists before Render publishes it.
# Key variables:
# - SITE_PATH: static publish directory expected by Render.
# - REQUIRED_PATHS: Unity WebGL files/folders required for a playable build.

set -euo pipefail

SITE_PATH="${1:-Submission/WebGLSite}"
REQUIRED_PATHS=(
  "$SITE_PATH/index.html"
  "$SITE_PATH/Build"
  "$SITE_PATH/TemplateData"
)

for required_path in "${REQUIRED_PATHS[@]}"; do
  if [ ! -e "$required_path" ]; then
    echo "Missing Unity WebGL build. Please build WebGL locally and run tools/prepare_webgl_site.sh or tools/prepare_webgl_site.ps1 first." >&2
    echo "Missing path: $required_path" >&2
    exit 1
  fi
done

echo "Unity WebGL static site is ready for Render."
