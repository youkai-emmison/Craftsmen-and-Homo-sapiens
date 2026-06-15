#!/usr/bin/env bash
# Prepares a static-site folder from an existing Unity WebGL build.
# Key variables:
# - SOURCE_PATH: completed Unity WebGL build folder.
# - OUTPUT_PATH: folder to upload to static hosting later.

set -euo pipefail

SOURCE_PATH="${1:-Build/WebGL}"
OUTPUT_PATH="${2:-Submission/WebGLSite}"

if [ ! -d "$SOURCE_PATH" ]; then
  echo "SourcePath does not exist: $SOURCE_PATH. Build WebGL in Unity first." >&2
  exit 1
fi

rm -rf "$OUTPUT_PATH"
mkdir -p "$OUTPUT_PATH"
cp -R "$SOURCE_PATH"/. "$OUTPUT_PATH"/

cat > "$OUTPUT_PATH/README_DEPLOY.txt" <<'EOF'
Static Unity WebGL deploy folder.

Upload the contents of this folder to Render Static Site, Cloudflare Pages, GitHub Pages, or another static host.
This script only prepares files. It does not deploy anything.
EOF

echo "Prepared static WebGL deploy folder: $OUTPUT_PATH"
