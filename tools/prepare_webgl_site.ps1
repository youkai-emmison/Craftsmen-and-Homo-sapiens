<#
Prepares a static-site folder from an existing Unity WebGL build.

Key variables:
- SourcePath: completed Unity WebGL build folder.
- OutputPath: folder to upload to static hosting later.
#>

param(
    [string]$SourcePath = "Build/WebGL",
    [string]$OutputPath = "Submission/WebGLSite"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $SourcePath -PathType Container)) {
    throw "Please open Unity and run Tools/Hackathon/Build WebGL first. SourcePath does not exist: $SourcePath"
}

if (Test-Path -LiteralPath $OutputPath) {
    Remove-Item -LiteralPath $OutputPath -Recurse -Force
}

New-Item -ItemType Directory -Force -Path $OutputPath | Out-Null
Copy-Item -LiteralPath (Join-Path $SourcePath "*") -Destination $OutputPath -Recurse -Force

$requiredPaths = @(
    (Join-Path $OutputPath "index.html"),
    (Join-Path $OutputPath "Build"),
    (Join-Path $OutputPath "TemplateData")
)

foreach ($requiredPath in $requiredPaths) {
    if (-not (Test-Path -LiteralPath $requiredPath)) {
        throw "Prepared WebGLSite is incomplete. Missing required path: $requiredPath"
    }
}

$readme = @"
# Unity WebGL Static Deploy Folder

This folder was copied from `Build/WebGL` for static hosting.

Expected Render settings:

- Build Command: `bash tools/render_validate_static_site.sh`
- Publish Directory: `Submission/WebGLSite`

This script only prepares files. It does not deploy anything.
"@

Set-Content -LiteralPath (Join-Path $OutputPath "DEPLOYMENT_README.md") -Value $readme -Encoding UTF8
Write-Host "Prepared static WebGL deploy folder: $OutputPath"
