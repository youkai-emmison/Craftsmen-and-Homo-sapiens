# Build a timestamped final upload package folder for the hackathon submission.
# This script is ASCII-only for Windows PowerShell 5 compatibility.
# It never deletes existing folders or overwrites existing files.

param(
    [string]$OutputRoot = "",
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"

if ([string]::IsNullOrWhiteSpace($OutputRoot)) {
    $OutputRoot = Join-Path $repoRoot "submissions/final_upload_packages/Craftsmen_Submission_$timestamp"
}

$script:FailureCount = 0
$script:CopyCount = 0

function Write-Step {
    param([string]$Message)
    Write-Host "[INFO] $Message"
}

function Write-Failure {
    param([string]$Message)
    $script:FailureCount += 1
    Write-Host "[FAIL] $Message" -ForegroundColor Red
}

function Ensure-PackageDirectory {
    param([string]$DirectoryPath)

    if ($DryRun) {
        Write-Step "Would ensure directory: $DirectoryPath"
        return
    }

    if (-not (Test-Path -LiteralPath $DirectoryPath)) {
        New-Item -ItemType Directory -Path $DirectoryPath -Force | Out-Null
    }
}

function Copy-PackageFile {
    param(
        [string]$SourceRelativePath,
        [string]$DestinationRelativeDirectory,
        [string]$DestinationFileName = ""
    )

    $sourcePath = Join-Path $repoRoot $SourceRelativePath
    if (-not (Test-Path -LiteralPath $sourcePath)) {
        Write-Failure "Missing source file: $SourceRelativePath"
        return
    }

    if ([string]::IsNullOrWhiteSpace($DestinationFileName)) {
        $DestinationFileName = Split-Path $SourceRelativePath -Leaf
    }

    $destinationDirectory = Join-Path $OutputRoot $DestinationRelativeDirectory
    $destinationPath = Join-Path $destinationDirectory $DestinationFileName

    if ($DryRun) {
        Write-Step "Would copy $SourceRelativePath -> $DestinationRelativeDirectory/$DestinationFileName"
        return
    }

    Ensure-PackageDirectory $destinationDirectory

    if (Test-Path -LiteralPath $destinationPath) {
        Write-Failure "Destination already exists, skipped to avoid overwrite: $destinationPath"
        return
    }

    Copy-Item -LiteralPath $sourcePath -Destination $destinationPath
    $script:CopyCount += 1
}

function Write-NextStepsFile {
    $nextStepsPath = Join-Path $OutputRoot "NEXT_STEPS_BEFORE_UPLOAD.txt"
    $lines = @(
        "Before uploading this package:",
        "1. Replace TODO values in link txt files with real external links.",
        "2. Open the WebGL link in an incognito browser.",
        "3. Open the demo video link in an incognito browser.",
        "4. Confirm the PPT/PDF files are the latest version.",
        "5. Add CodeBuddy export files to 04_CodeBuddy if the platform requires attachments.",
        "6. Do not add Unity Library, Temp, Logs, UserSettings, Build, or artifact-build-manifest.json.",
        "7. Run submissions/Run-FinalSubmissionAudit.ps1 before final submission."
    )

    if ($DryRun) {
        Write-Step "Would write NEXT_STEPS_BEFORE_UPLOAD.txt"
        return
    }

    Set-Content -LiteralPath $nextStepsPath -Value $lines -Encoding UTF8
}

Set-Location $repoRoot

Write-Host ""
Write-Host "Final upload package builder"
Write-Host "Repository: $repoRoot"
Write-Host "Output: $OutputRoot"
Write-Host "DryRun: $DryRun"
Write-Host ""

Ensure-PackageDirectory $OutputRoot

Copy-PackageFile "submissions/00_README_FIRST_ZH.md" "." "00_README_FIRST.md"

Copy-PackageFile "submissions/PROJECT_BOOK_FINAL_ZH.pdf" "01_ProjectBook"
Copy-PackageFile "submissions/PROJECT_BOOK_FINAL_ZH.docx" "01_ProjectBook"

Copy-PackageFile "submissions/Craftsmen_Hackathon_Deck.pptx" "02_Presentation"
Copy-PackageFile "submissions/Craftsmen_Hackathon_Deck_Preview.pdf" "02_Presentation"

Copy-PackageFile "submissions/package_templates/WebGL_Link.txt" "03_Demo"
Copy-PackageFile "submissions/package_templates/Demo_Video_Link.txt" "03_Demo"
Copy-PackageFile "submissions/JUDGE_QUICK_START.md" "03_Demo"

Copy-PackageFile "submissions/package_templates/CodeBuddy_History_Link.txt" "04_CodeBuddy"
Copy-PackageFile "submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md" "04_CodeBuddy"

Copy-PackageFile "submissions/package_templates/GitHub_Link.txt" "05_Source"

Copy-PackageFile "submissions/FINAL_SUBMISSION_INFO.md" "06_BackupDocs"
Copy-PackageFile "submissions/FORM_ANSWERS_COPYPASTE_ZH.md" "06_BackupDocs"
Copy-PackageFile "submissions/SCORING_EVIDENCE_MAP_ZH.md" "06_BackupDocs"
Copy-PackageFile "submissions/ROADSHOW_QA_CHEATSHEET_ZH.md" "06_BackupDocs"
Copy-PackageFile "submissions/JUDGE_ONE_PAGE_BRIEF_ZH.md" "06_BackupDocs"
Copy-PackageFile "submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf" "06_BackupDocs"
Copy-PackageFile "submissions/package_templates/PPT_File_Link.txt" "06_BackupDocs"
Copy-PackageFile "submissions/package_templates/Social_Media_Link_Optional.txt" "06_BackupDocs"

Write-NextStepsFile

Write-Host ""
Write-Host "Package build summary"
Write-Host "Copied files: $script:CopyCount"
Write-Host "Failures: $script:FailureCount"

if ($script:FailureCount -gt 0) {
    Write-Host "Result: package build needs attention." -ForegroundColor Red
    exit 1
}

if ($DryRun) {
    Write-Host "Result: dry run completed. No files were written." -ForegroundColor Green
} else {
    Write-Host "Result: package folder created. Fill link txt files before uploading." -ForegroundColor Green
}

exit 0
