# Apply final external links to submission-facing markdown/txt files.
# Keep this script ASCII-only so Windows PowerShell 5 can parse it reliably.

param(
    [string]$InputPath = "submissions/link_backfill_values.local.json",
    [switch]$DryRun,
    [switch]$AllowTodo
)

$ErrorActionPreference = "Stop"

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
Set-Location $repoRoot

$script:ChangedFiles = New-Object System.Collections.Generic.List[string]
$script:FailureCount = 0

function Write-Info {
    param([string]$Message)
    Write-Host "[INFO] $Message"
}

function Write-Failure {
    param([string]$Message)
    $script:FailureCount += 1
    Write-Host "[FAIL] $Message" -ForegroundColor Red
}

function Get-JsonValue {
    param(
        [object]$Data,
        [string]$Name
    )

    $property = $Data.PSObject.Properties[$Name]
    if ($null -eq $property -or $null -eq $property.Value) {
        return ""
    }

    return [string]$property.Value
}

function Test-LinkValue {
    param(
        [string]$Name,
        [string]$Value,
        [bool]$Required
    )

    if (-not $Required -and [string]::IsNullOrWhiteSpace($Value)) {
        return
    }

    if ([string]::IsNullOrWhiteSpace($Value)) {
        Write-Failure "$Name is empty."
        return
    }

    if (-not $AllowTodo -and $Value -match "TODO|localhost|127\.0\.0\.1|file://|C:\\Users") {
        Write-Failure "$Name is not a final public link: $Value"
    }
}

function Get-ColonIndex {
    param([string]$Line)

    $asciiColon = $Line.IndexOf(":")
    $fullWidthColon = $Line.IndexOf([string]([char]0xFF1A))

    if ($asciiColon -lt 0) {
        return $fullWidthColon
    }

    if ($fullWidthColon -lt 0) {
        return $asciiColon
    }

    return [Math]::Min($asciiColon, $fullWidthColon)
}

function Test-PlaceholderLine {
    param([string]$Line)

    if ($Line -match "TODO|PLACEHOLDER|localhost|127\.0\.0\.1|file://|C:\\Users") {
        return $true
    }

    if ($Line.Contains($pendingBackfillText) -or $Line.Contains($pendingPatchText)) {
        return $true
    }

    return $false
}

function Replace-LinkLine {
    param(
        [string[]]$Lines,
        [string]$Key,
        [string]$Value
    )

    if ([string]::IsNullOrWhiteSpace($Value)) {
        return $Lines
    }

    $result = New-Object System.Collections.Generic.List[string]

    foreach ($line in $Lines) {
        if ($line.Contains($Key) -and (Test-PlaceholderLine $line)) {
            $colonIndex = Get-ColonIndex $line
            if ($colonIndex -ge 0) {
                $prefix = $line.Substring(0, $colonIndex + 1)
                $result.Add("$prefix $Value")
                continue
            }
        }

        $result.Add($line)
    }

    return $result.ToArray()
}

function Replace-CodeBlockTodoAfterKey {
    param(
        [string[]]$Lines,
        [string]$Key,
        [string]$Value
    )

    if ([string]::IsNullOrWhiteSpace($Value)) {
        return $Lines
    }

    $result = New-Object System.Collections.Generic.List[string]
    $pendingKey = $false

    foreach ($line in $Lines) {
        if ($line.Contains($Key)) {
            $pendingKey = $true
            $result.Add($line)
            continue
        }

        if ($pendingKey -and $line.Trim() -eq "TODO") {
            $result.Add($Value)
            $pendingKey = $false
            continue
        }

        if ($pendingKey -and -not [string]::IsNullOrWhiteSpace($line) -and $line.Trim() -ne '```text' -and $line.Trim() -ne '```') {
            $pendingKey = $false
        }

        $result.Add($line)
    }

    return $result.ToArray()
}

function Set-TextFileIfChanged {
    param(
        [string]$RelativePath,
        [string[]]$NewLines
    )

    $path = Join-Path $repoRoot $RelativePath
    if (-not (Test-Path -LiteralPath $path)) {
        Write-Failure "Missing file: $RelativePath"
        return
    }

    $oldLines = Get-Content -LiteralPath $path -Encoding UTF8
    $oldText = $oldLines -join "`n"
    $newText = $NewLines -join "`n"

    if ($oldText -eq $newText) {
        return
    }

    $script:ChangedFiles.Add($RelativePath)

    if ($DryRun) {
        Write-Info "Would update $RelativePath"
        return
    }

    Set-Content -LiteralPath $path -Value $NewLines -Encoding UTF8
    Write-Info "Updated $RelativePath"
}

function Update-MarkdownFile {
    param([string]$RelativePath)

    $path = Join-Path $repoRoot $RelativePath
    if (-not (Test-Path -LiteralPath $path)) {
        Write-Failure "Missing file: $RelativePath"
        return
    }

    $lines = Get-Content -LiteralPath $path -Encoding UTF8
    $lines = Replace-LinkLine -Lines $lines -Key "WebGL" -Value $webglUrl
    $lines = Replace-LinkLine -Lines $lines -Key "Demo" -Value $demoVideoUrl
    $lines = Replace-LinkLine -Lines $lines -Key "PPT" -Value $pptFileUrl
    $lines = Replace-LinkLine -Lines $lines -Key "CodeBuddy" -Value $codeBuddyUrl
    $lines = Replace-LinkLine -Lines $lines -Key $socialMediaKey -Value $socialMediaUrl
    $lines = Replace-LinkLine -Lines $lines -Key $projectBookKey -Value $projectBookUrl
    $lines = Replace-LinkLine -Lines $lines -Key $judgeBriefKey -Value $judgeBriefUrl
    $lines = Replace-CodeBlockTodoAfterKey -Lines $lines -Key "WebGL" -Value $webglUrl
    $lines = Replace-CodeBlockTodoAfterKey -Lines $lines -Key "Demo" -Value $demoVideoUrl

    Set-TextFileIfChanged -RelativePath $RelativePath -NewLines $lines
}

function Update-TxtTemplate {
    param(
        [string]$RelativePath,
        [string]$Value
    )

    if ([string]::IsNullOrWhiteSpace($Value)) {
        return
    }

    $path = Join-Path $repoRoot $RelativePath
    if (-not (Test-Path -LiteralPath $path)) {
        Write-Failure "Missing file: $RelativePath"
        return
    }

    $lines = Get-Content -LiteralPath $path -Encoding UTF8
    $result = New-Object System.Collections.Generic.List[string]

    foreach ($line in $lines) {
        if ($line.Trim() -eq "TODO") {
            $result.Add($Value)
        } else {
            $result.Add($line)
        }
    }

    Set-TextFileIfChanged -RelativePath $RelativePath -NewLines $result.ToArray()
}

$inputFullPath = Join-Path $repoRoot $InputPath
if (-not (Test-Path -LiteralPath $inputFullPath)) {
    Write-Failure "Input file not found: $InputPath"
    Write-Info "Copy submissions/link_backfill_values.example.json to submissions/link_backfill_values.local.json, then fill real links."
    exit 1
}

$linkData = Get-Content -LiteralPath $inputFullPath -Encoding UTF8 -Raw | ConvertFrom-Json

$webglUrl = Get-JsonValue $linkData "webglUrl"
$demoVideoUrl = Get-JsonValue $linkData "demoVideoUrl"
$pptFileUrl = Get-JsonValue $linkData "pptFileUrl"
$codeBuddyUrl = Get-JsonValue $linkData "codeBuddyUrl"
$githubUrl = Get-JsonValue $linkData "githubUrl"
$projectBookUrl = Get-JsonValue $linkData "projectBookUrl"
$judgeBriefUrl = Get-JsonValue $linkData "judgeBriefUrl"
$socialMediaUrl = Get-JsonValue $linkData "socialMediaUrl"

if ([string]::IsNullOrWhiteSpace($projectBookUrl)) {
    $projectBookUrl = "see repository: submissions/PROJECT_BOOK_FINAL_ZH.pdf"
}

if ([string]::IsNullOrWhiteSpace($judgeBriefUrl)) {
    $judgeBriefUrl = "see repository: submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf"
}

if ([string]::IsNullOrWhiteSpace($socialMediaUrl)) {
    $socialMediaUrl = "N/A (optional, not published)"
}

Test-LinkValue -Name "webglUrl" -Value $webglUrl -Required $true
Test-LinkValue -Name "demoVideoUrl" -Value $demoVideoUrl -Required $true
Test-LinkValue -Name "pptFileUrl" -Value $pptFileUrl -Required $true
Test-LinkValue -Name "codeBuddyUrl" -Value $codeBuddyUrl -Required $true
Test-LinkValue -Name "githubUrl" -Value $githubUrl -Required $true
Test-LinkValue -Name "projectBookUrl" -Value $projectBookUrl -Required $false
Test-LinkValue -Name "judgeBriefUrl" -Value $judgeBriefUrl -Required $false
Test-LinkValue -Name "socialMediaUrl" -Value $socialMediaUrl -Required $false

if ($script:FailureCount -gt 0) {
    Write-Host "Result: input links are not ready." -ForegroundColor Red
    exit 1
}

# Unicode code points keep the source ASCII-only.
$pendingBackfillText = -join ([char[]](0x5F85, 0x56DE, 0x586B))
$pendingPatchText = -join ([char[]](0x5F85, 0x8865))
$socialMediaKey = -join ([char[]](0x793E, 0x4EA4, 0x5A92, 0x4F53))
$projectBookKey = -join ([char[]](0x9879, 0x76EE, 0x4E66))
$judgeBriefKey = -join ([char[]](0x8BC4, 0x59D4))

Write-Host ""
Write-Host "Applying link backfill"
Write-Host "Input: $InputPath"
Write-Host "DryRun: $DryRun"
Write-Host ""

Update-MarkdownFile "submissions/LINKS_TO_FILL.md"
Update-MarkdownFile "submissions/FINAL_SUBMISSION_INFO.md"
Update-MarkdownFile "submissions/FORM_ANSWERS_COPYPASTE_ZH.md"
Update-MarkdownFile "submissions/JUDGE_QUICK_START.md"
Update-MarkdownFile "submissions/WEBGL_PAGE_COPY.md"
Update-MarkdownFile "submissions/DEMO_VIDEO_UPLOAD_COPY.md"
Update-MarkdownFile "submissions/00_README_FIRST_ZH.md"

Update-TxtTemplate "submissions/package_templates/WebGL_Link.txt" $webglUrl
Update-TxtTemplate "submissions/package_templates/Demo_Video_Link.txt" $demoVideoUrl
Update-TxtTemplate "submissions/package_templates/PPT_File_Link.txt" $pptFileUrl
Update-TxtTemplate "submissions/package_templates/CodeBuddy_History_Link.txt" $codeBuddyUrl
Update-TxtTemplate "submissions/package_templates/Social_Media_Link_Optional.txt" $socialMediaUrl

Write-Host ""
Write-Host "Link backfill summary"
Write-Host "Changed files: $($script:ChangedFiles.Count)"
foreach ($file in $script:ChangedFiles) {
    Write-Host " - $file"
}

if ($DryRun) {
    Write-Host "Result: dry run completed. No files were written." -ForegroundColor Green
} else {
    Write-Host "Result: link backfill completed. Run Run-FinalSubmissionAudit.ps1 next." -ForegroundColor Green
}

exit 0
