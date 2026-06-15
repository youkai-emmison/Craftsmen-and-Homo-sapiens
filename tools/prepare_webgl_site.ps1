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
    throw "SourcePath does not exist: $SourcePath. Build WebGL in Unity first."
}

if (Test-Path -LiteralPath $OutputPath) {
    Remove-Item -LiteralPath $OutputPath -Recurse -Force
}

New-Item -ItemType Directory -Force -Path $OutputPath | Out-Null
Copy-Item -LiteralPath (Join-Path $SourcePath "*") -Destination $OutputPath -Recurse -Force

$readme = @"
Static Unity WebGL deploy folder.

Upload the contents of this folder to Render Static Site, Cloudflare Pages, GitHub Pages, or another static host.
This script only prepares files. It does not deploy anything.
"@

Set-Content -LiteralPath (Join-Path $OutputPath "README_DEPLOY.txt") -Value $readme -Encoding UTF8
Write-Host "Prepared static WebGL deploy folder: $OutputPath"
