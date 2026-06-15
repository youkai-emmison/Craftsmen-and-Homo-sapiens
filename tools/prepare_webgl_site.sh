#!/usr/bin/env bash
# Prepares a static-site folder from an existing Unity WebGL build.
# Key variables:
# - SOURCE_PATH: completed Unity WebGL build folder.
# - OUTPUT_PATH: folder to upload to static hosting later.

set -euo pipefail

SOURCE_PATH="${1:-Build/WebGL}"
OUTPUT_PATH="${2:-Submission/WebGLSite}"

if [ ! -d "$SOURCE_PATH" ]; then
  echo "Please open Unity and run Tools/Hackathon/Build WebGL first. SourcePath does not exist: $SOURCE_PATH" >&2
  exit 1
fi

rm -rf "$OUTPUT_PATH"
mkdir -p "$OUTPUT_PATH"
cp -R "$SOURCE_PATH"/. "$OUTPUT_PATH"/

for required_path in "$OUTPUT_PATH/index.html" "$OUTPUT_PATH/Build" "$OUTPUT_PATH/TemplateData"; do
  if [ ! -e "$required_path" ]; then
    echo "Prepared WebGLSite is incomplete. Missing required path: $required_path" >&2
    exit 1
  fi
done

cat > "$OUTPUT_PATH/DEPLOYMENT_README.md" <<'EOF'
# Unity WebGL Static Deploy Folder

This folder was copied from `Build/WebGL` for static hosting.

Expected Render settings:

- Build Command: `bash tools/render_validate_static_site.sh`
- Publish Directory: `Submission/WebGLSite`

This script only prepares files. It does not deploy anything.
EOF

echo "Prepared static WebGL deploy folder: $OUTPUT_PATH"
