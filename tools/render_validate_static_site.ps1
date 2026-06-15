<#
Validates that the Unity WebGL static site exists before Render publishes it.

Key variables:
- SitePath: static publish directory expected by Render.
- RequiredPaths: Unity WebGL files/folders required for a playable build.
#>

param(
    [string]$SitePath = "Submission/WebGLSite"
)

$ErrorActionPreference = "Stop"

$requiredPaths = @(
    (Join-Path $SitePath "index.html"),
    (Join-Path $SitePath "Build"),
    (Join-Path $SitePath "TemplateData")
)

foreach ($requiredPath in $requiredPaths) {
    if (-not (Test-Path -LiteralPath $requiredPath)) {
        Write-Error "Missing Unity WebGL build. Please build WebGL locally and run tools/prepare_webgl_site.sh or tools/prepare_webgl_site.ps1 first. Missing path: $requiredPath"
        exit 1
    }
}

Write-Host "Unity WebGL static site is ready for Render."
