#!/usr/bin/env bash
# Creates a source-package zip for hackathon submission.
# Key variables:
# - OUTPUT_ZIP: generated source package path.
# - INCLUDE_PATHS: repository paths included in the source package.

set -euo pipefail

OUTPUT_ZIP="${1:-Submission/Craftsmen-and-Homo-sapiens_Source.zip}"
STAGING_ROOT="$(mktemp -d)"
STAGING_PROJECT="$STAGING_ROOT/Craftsmen-and-Homo-sapiens"
mkdir -p "$STAGING_PROJECT"

cleanup() {
  rm -rf "$STAGING_ROOT"
}
trap cleanup EXIT

INCLUDE_PATHS=(
  "Assets"
  "Packages"
  "ProjectSettings"
  "docs"
  "Submission"
  "README.md"
  "AGENTS.md"
  ".gitignore"
)

for item in "${INCLUDE_PATHS[@]}"; do
  if [ -e "$item" ]; then
    mkdir -p "$STAGING_PROJECT/$(dirname "$item")"
    cp -R "$item" "$STAGING_PROJECT/$item"
  fi
done

find "$STAGING_PROJECT" \( \
  -name Library -o -name Temp -o -name Logs -o -name Obj -o -name UserSettings -o \
  -name Build -o -name Builds -o -name .git -o -name .vs -o -name .idea -o \
  -name '*.csproj' -o -name '*.sln' -o -name '*.zip' \
\) -prune -exec rm -rf {} +

mkdir -p "$(dirname "$OUTPUT_ZIP")"
rm -f "$OUTPUT_ZIP"

if ! command -v zip >/dev/null 2>&1; then
  echo "zip command is required to create $OUTPUT_ZIP" >&2
  exit 1
fi

(cd "$STAGING_PROJECT" && zip -qr "$OLDPWD/$OUTPUT_ZIP" .)
echo "Created source package: $OUTPUT_ZIP"
