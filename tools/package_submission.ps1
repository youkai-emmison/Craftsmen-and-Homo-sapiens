<#
Creates a source-package zip for hackathon submission.

Key variables:
- OutputZip: generated source package path.
- IncludePaths: repository paths included in the source package.
#>

param(
    [string]$OutputZip = "Submission/Craftsmen-and-Homo-sapiens_Source.zip"
)

$ErrorActionPreference = "Stop"

$includePaths = @(
    "Assets",
    "Packages",
    "ProjectSettings",
    "docs",
    "Submission",
    "README.md",
    "AGENTS.md",
    ".gitignore"
)

$stagingRoot = Join-Path $env:TEMP ("CraftsmenSubmission_" + [Guid]::NewGuid().ToString("N"))
$stagingProject = Join-Path $stagingRoot "Craftsmen-and-Homo-sapiens"
New-Item -ItemType Directory -Force -Path $stagingProject | Out-Null

try {
    foreach ($path in $includePaths) {
        if (-not (Test-Path -LiteralPath $path)) {
            continue
        }

        $destination = Join-Path $stagingProject $path
        $destinationParent = Split-Path -Parent $destination
        if ($destinationParent) {
            New-Item -ItemType Directory -Force -Path $destinationParent | Out-Null
        }

        Copy-Item -LiteralPath $path -Destination $destination -Recurse -Force
    }

    $excludedNames = @("Library", "Temp", "Logs", "Obj", "UserSettings", "Build", "Builds", ".git", ".vs", ".idea")
    Get-ChildItem -LiteralPath $stagingProject -Recurse -Force |
        Where-Object { $excludedNames -contains $_.Name -or $_.Name.EndsWith(".csproj") -or $_.Name.EndsWith(".sln") -or $_.Name.EndsWith(".zip") } |
        Sort-Object FullName -Descending |
        ForEach-Object {
            Remove-Item -LiteralPath $_.FullName -Recurse -Force
        }

    $outputParent = Split-Path -Parent $OutputZip
    if ($outputParent) {
        New-Item -ItemType Directory -Force -Path $outputParent | Out-Null
    }

    if (Test-Path -LiteralPath $OutputZip) {
        Remove-Item -LiteralPath $OutputZip -Force
    }

    Compress-Archive -Path (Join-Path $stagingProject "*") -DestinationPath $OutputZip -Force
    Write-Host "Created source package: $OutputZip"
}
finally {
    if (Test-Path -LiteralPath $stagingRoot) {
        Remove-Item -LiteralPath $stagingRoot -Recurse -Force
    }
}
