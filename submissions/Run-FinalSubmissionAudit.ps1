# Final submission audit helper for the Tencent Cloud Hackathon package.
# Keep this script ASCII-only so Windows PowerShell 5 can parse it reliably.

$ErrorActionPreference = "Stop"

$script:FailureCount = 0
$script:WarningCount = 0

function Write-CheckResult {
    param(
        [string]$Level,
        [string]$Message
    )

    if ($Level -eq "FAIL") {
        $script:FailureCount += 1
        Write-Host "[FAIL] $Message" -ForegroundColor Red
        return
    }

    if ($Level -eq "WARN") {
        $script:WarningCount += 1
        Write-Host "[WARN] $Message" -ForegroundColor Yellow
        return
    }

    Write-Host "[ OK ] $Message" -ForegroundColor Green
}

function Test-RequiredFile {
    param([string]$RelativePath)

    $fullPath = Join-Path $repoRoot $RelativePath
    if (Test-Path -LiteralPath $fullPath) {
        Write-CheckResult "OK" "Found $RelativePath"
    } else {
        Write-CheckResult "FAIL" "Missing required file: $RelativePath"
    }
}

function Search-FileForRedFlags {
    param(
        [string]$RelativePath,
        [string[]]$Patterns
    )

    $fullPath = Join-Path $repoRoot $RelativePath
    if (-not (Test-Path -LiteralPath $fullPath)) {
        Write-CheckResult "FAIL" "Cannot scan missing file: $RelativePath"
        return
    }

    $content = Get-Content -LiteralPath $fullPath -Encoding UTF8
    $matches = @()

    foreach ($pattern in $Patterns) {
        $found = $content | Select-String -Pattern $pattern
        if ($found) {
            $matches += $found
        }
    }

    if ($matches.Count -eq 0) {
        Write-CheckResult "OK" "No red flags in $RelativePath"
        return
    }

    Write-CheckResult "FAIL" "Red flags found in $RelativePath"
    foreach ($match in $matches | Select-Object -First 8) {
        Write-Host "       line $($match.LineNumber): $($match.Line.Trim())"
    }
}

function Test-GitTrackedForbiddenPaths {
    $forbiddenPattern = '(^|/)(Library|Temp|Logs|UserSettings|Build|Builds)/'
    $trackedFiles = & git @gitSafeArgs ls-files 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-CheckResult "FAIL" "git ls-files failed: $($trackedFiles -join ' ')"
        return
    }

    $badFiles = $trackedFiles | Select-String -Pattern $forbiddenPattern

    if ($badFiles) {
        Write-CheckResult "FAIL" "Forbidden Unity cache/build paths are tracked by Git."
        foreach ($badFile in $badFiles | Select-Object -First 10) {
            Write-Host "       $($badFile.Line)"
        }
        return
    }

    Write-CheckResult "OK" "No tracked Library/Temp/Logs/UserSettings/Build folders."
}

function Test-GitWorkingTree {
    $status = & git @gitSafeArgs status --short 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-CheckResult "FAIL" "git status failed: $($status -join ' ')"
        return
    }

    if ($status) {
        Write-CheckResult "WARN" "Working tree has uncommitted changes."
        foreach ($line in $status | Select-Object -First 12) {
            Write-Host "       $line"
        }
        return
    }

    Write-CheckResult "OK" "Git working tree is clean."
}

function Test-DoNotUploadLocalManifest {
    $manifestPath = Join-Path $repoRoot "submissions/artifact-build-manifest.json"
    if (Test-Path -LiteralPath $manifestPath) {
        Write-CheckResult "WARN" "submissions/artifact-build-manifest.json exists; keep it out of final upload packages because it contains local build paths."
        return
    }

    Write-CheckResult "OK" "No artifact-build-manifest.json found in submissions."
}

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$gitSafeArgs = @("-c", "safe.directory=$repoRoot")
Set-Location $repoRoot

Write-Host ""
Write-Host "Tencent Cloud Hackathon final submission audit"
Write-Host "Repository: $repoRoot"
Write-Host ""

$requiredFiles = @(
    "README.md",
    "submissions/README.md",
    "submissions/FINAL_DELIVERABLES_MANIFEST.md",
    "submissions/SUBMISSION_READINESS_AUDIT.md",
    "submissions/PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md",
    "submissions/Craftsmen_Hackathon_Deck.pptx",
    "submissions/Craftsmen_Hackathon_Deck_Preview.pdf",
    "submissions/PROJECT_BOOK_FINAL_ZH.pdf",
    "submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf",
    "submissions/FINAL_SUBMISSION_INFO.md",
    "submissions/FORM_ANSWERS_COPYPASTE_ZH.md",
    "submissions/JUDGE_QUICK_START.md",
    "submissions/WEBGL_UPLOAD_RUNBOOK.md",
    "submissions/DEMO_RECORDING_RUNBOOK.md",
    "submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md"
)

Write-Host "Required file check"
foreach ($file in $requiredFiles) {
    Test-RequiredFile $file
}

Write-Host ""
Write-Host "Git safety check"
Test-GitWorkingTree
Test-GitTrackedForbiddenPaths
Test-DoNotUploadLocalManifest

Write-Host ""
Write-Host "Final-facing placeholder scan"

# Unicode code points keep the source ASCII-only:
# U+5F85 U+56DE U+586B = Chinese phrase for pending link backfill.
# U+5F85 U+8865 = Chinese phrase for pending supplement.
# U+FF1A = full-width colon used in Chinese copy.
$pendingBackfillPattern = -join ([char[]](0x5F85, 0x56DE, 0x586B))
$pendingPatchPattern = -join ([char[]](0x5F85, 0x8865))
$fullWidthColon = [char]0xFF1A

$redFlagPatterns = @(
    "^\s*TODO\s*$",
    ":\s*TODO\b",
    "$fullWidthColon\s*TODO\b",
    ":\s*$pendingBackfillPattern",
    "$fullWidthColon\s*$pendingBackfillPattern",
    ":\s*$pendingPatchPattern",
    "$fullWidthColon\s*$pendingPatchPattern",
    "PLACEHOLDER",
    "localhost",
    "file://",
    "127\.0\.0\.1",
    "C:\\Users"
)

$finalFacingFiles = @(
    "submissions/FINAL_SUBMISSION_INFO.md",
    "submissions/FORM_ANSWERS_COPYPASTE_ZH.md",
    "submissions/JUDGE_QUICK_START.md",
    "submissions/JUDGE_ONE_PAGE_BRIEF_ZH.md",
    "submissions/WEBGL_PAGE_COPY.md",
    "submissions/DEMO_VIDEO_UPLOAD_COPY.md",
    "submissions/00_README_FIRST_ZH.md"
)

foreach ($file in $finalFacingFiles) {
    Search-FileForRedFlags -RelativePath $file -Patterns $redFlagPatterns
}

Write-Host ""
Write-Host "Audit summary"
Write-Host "Failures: $script:FailureCount"
Write-Host "Warnings: $script:WarningCount"

if ($script:FailureCount -gt 0) {
    Write-Host "Result: NOT READY. Fill external links and clear final-facing placeholders before submission." -ForegroundColor Red
    exit 1
}

Write-Host "Result: READY ENOUGH FOR FINAL HUMAN REVIEW. Still open every external link in an incognito browser." -ForegroundColor Green
exit 0
