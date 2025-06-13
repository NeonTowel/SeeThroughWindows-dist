#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Create a beta release from the devel branch

.DESCRIPTION
    This script helps create beta releases with proper semantic versioning.
    It will create a tag like v1.0.9-beta.1 and push it to trigger the release workflow.

.PARAMETER Version
    The base version number (e.g., "1.0.9")

.PARAMETER BetaNumber
    The beta number (e.g., 1 for beta.1, 2 for beta.2)

.PARAMETER Push
    Whether to push the tag immediately (default: false, will ask for confirmation)

.EXAMPLE
    .\scripts\create-beta-release.ps1 -Version "1.0.9" -BetaNumber 1
    Creates and pushes tag v1.0.9-beta.1

.EXAMPLE
    .\scripts\create-beta-release.ps1 -Version "1.0.9" -BetaNumber 2 -Push
    Creates and immediately pushes tag v1.0.9-beta.2
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Version,

    [Parameter(Mandatory = $true)]
    [int]$BetaNumber,

    [switch]$Push
)

# Ensure we're on devel branch
$currentBranch = git branch --show-current
if ($currentBranch -ne "devel") {
    Write-Error "❌ You must be on the 'devel' branch to create a beta release. Current branch: $currentBranch"
    exit 1
}

# Check if working directory is clean
$status = git status --porcelain
if ($status) {
    Write-Error "❌ Working directory is not clean. Please commit or stash your changes first."
    git status
    exit 1
}

# Validate version format
if ($Version -notmatch '^\d+\.\d+\.\d+$') {
    Write-Error "❌ Version must be in format X.Y.Z (e.g., 1.0.9)"
    exit 1
}

# Create beta tag
$betaTag = "v$Version-beta.$BetaNumber"

# Check if tag already exists
$existingTag = git tag -l $betaTag
if ($existingTag) {
    Write-Error "❌ Tag $betaTag already exists!"
    exit 1
}

Write-Host "🏷️  Creating beta release tag: $betaTag" -ForegroundColor Green
Write-Host "📦 Version: $Version" -ForegroundColor Cyan
Write-Host "🧪 Beta: $BetaNumber" -ForegroundColor Cyan
Write-Host "🌿 Branch: devel" -ForegroundColor Cyan

# Get recent commits for preview
Write-Host "`n📝 Recent commits that will be included:" -ForegroundColor Yellow
git log --oneline -5

Write-Host "`n" -NoNewline

if (-not $Push) {
    $confirmation = Read-Host "Do you want to create and push this beta release? (y/N)"
    if ($confirmation -notmatch '^[Yy]') {
        Write-Host "❌ Beta release cancelled." -ForegroundColor Red
        exit 0
    }
}

# Create the tag
Write-Host "🏷️  Creating tag $betaTag..." -ForegroundColor Green
git tag -a $betaTag -m "Beta release $betaTag

This beta release contains the latest features and improvements from the devel branch.
Please test thoroughly and report any issues.

Created from devel branch on $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Failed to create tag"
    exit 1
}

# Push the tag
Write-Host "🚀 Pushing tag to trigger release workflow..." -ForegroundColor Green
git push origin $betaTag

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Failed to push tag"
    # Clean up local tag
    git tag -d $betaTag
    exit 1
}

Write-Host "`n✅ Beta release $betaTag created successfully!" -ForegroundColor Green
Write-Host "🔗 Check the release workflow at: https://github.com/NeonTowel/SeeThroughWindows-dist/actions" -ForegroundColor Cyan
Write-Host "📦 The release will be available at: https://github.com/NeonTowel/SeeThroughWindows-dist/releases/tag/$betaTag" -ForegroundColor Cyan

Write-Host "`n🎯 Next steps:" -ForegroundColor Yellow
Write-Host "  1. Monitor the GitHub Actions workflow" -ForegroundColor White
Write-Host "  2. Test the beta release thoroughly" -ForegroundColor White
Write-Host "  3. Collect feedback from beta testers" -ForegroundColor White
Write-Host "  4. When ready, merge devel → main and create stable release" -ForegroundColor White
