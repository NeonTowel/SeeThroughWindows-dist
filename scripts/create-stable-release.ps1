#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Create a stable release from the main branch

.DESCRIPTION
    This script helps create stable releases after beta testing is complete.
    It will create a tag like v1.0.9 and push it to trigger the release workflow.

.PARAMETER Version
    The version number (e.g., "1.0.9")

.PARAMETER Push
    Whether to push the tag immediately (default: false, will ask for confirmation)

.PARAMETER MergeFromDevel
    Whether to merge devel into main first (default: false, will ask for confirmation)

.EXAMPLE
    .\scripts\create-stable-release.ps1 -Version "1.0.9"
    Creates and pushes tag v1.0.9 from current main branch

.EXAMPLE
    .\scripts\create-stable-release.ps1 -Version "1.0.9" -MergeFromDevel -Push
    Merges devel to main, then creates and immediately pushes tag v1.0.9
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Version,

    [switch]$Push,
    [switch]$MergeFromDevel
)

# Validate version format
if ($Version -notmatch '^\d+\.\d+\.\d+$') {
    Write-Error "❌ Version must be in format X.Y.Z (e.g., 1.0.9)"
    exit 1
}

# Check if working directory is clean
$status = git status --porcelain
if ($status) {
    Write-Error "❌ Working directory is not clean. Please commit or stash your changes first."
    git status
    exit 1
}

# Create stable tag
$stableTag = "v$Version"

# Check if tag already exists
$existingTag = git tag -l $stableTag
if ($existingTag) {
    Write-Error "❌ Tag $stableTag already exists!"
    exit 1
}

Write-Host "🏷️  Creating stable release tag: $stableTag" -ForegroundColor Green
Write-Host "📦 Version: $Version" -ForegroundColor Cyan

# Handle merge from devel if requested
if ($MergeFromDevel) {
    Write-Host "🔄 Merge from devel requested" -ForegroundColor Yellow

    # Ensure we're on main branch
    $currentBranch = git branch --show-current
    if ($currentBranch -ne "main") {
        Write-Host "📍 Switching to main branch..." -ForegroundColor Cyan
        git checkout main
        if ($LASTEXITCODE -ne 0) {
            Write-Error "❌ Failed to switch to main branch"
            exit 1
        }
    }

    # Pull latest main
    Write-Host "⬇️  Pulling latest main..." -ForegroundColor Cyan
    git pull origin main
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Failed to pull latest main"
        exit 1
    }

    # Merge devel
    Write-Host "🔀 Merging devel into main..." -ForegroundColor Cyan
    git merge origin/devel --no-ff -m "Merge devel for stable release $stableTag

This merge brings all tested features from devel branch into main
for the stable $stableTag release.

Merged on $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"

    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Failed to merge devel into main. Please resolve conflicts manually."
        exit 1
    }

    Write-Host "✅ Successfully merged devel into main" -ForegroundColor Green
} else {
    # Ensure we're on main branch
    $currentBranch = git branch --show-current
    if ($currentBranch -ne "main") {
        Write-Error "❌ You must be on the 'main' branch to create a stable release. Current branch: $currentBranch"
        Write-Host "💡 Use -MergeFromDevel to automatically merge devel and switch to main" -ForegroundColor Yellow
        exit 1
    }
}

Write-Host "🌟 Branch: main" -ForegroundColor Cyan

# Get recent commits for preview
Write-Host "`n📝 Recent commits that will be included:" -ForegroundColor Yellow
git log --oneline -5

Write-Host "`n" -NoNewline

if (-not $Push) {
    $confirmation = Read-Host "Do you want to create and push this stable release? (y/N)"
    if ($confirmation -notmatch '^[Yy]') {
        Write-Host "❌ Stable release cancelled." -ForegroundColor Red
        exit 0
    }
}

# Create the tag
Write-Host "🏷️  Creating tag $stableTag..." -ForegroundColor Green
git tag -a $stableTag -m "Stable release $stableTag

This stable release has been thoroughly tested and is ready for production use.

Created from main branch on $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Failed to create tag"
    exit 1
}

# Push the tag (and main branch if we merged)
if ($MergeFromDevel) {
    Write-Host "🚀 Pushing main branch with merge..." -ForegroundColor Green
    git push origin main
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Failed to push main branch"
        exit 1
    }
}

Write-Host "🚀 Pushing tag to trigger release workflow..." -ForegroundColor Green
git push origin $stableTag

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Failed to push tag"
    # Clean up local tag
    git tag -d $stableTag
    exit 1
}

Write-Host "`n✅ Stable release $stableTag created successfully!" -ForegroundColor Green
Write-Host "🔗 Check the release workflow at: https://github.com/NeonTowel/SeeThroughWindows-dist/actions" -ForegroundColor Cyan
Write-Host "📦 The release will be available at: https://github.com/NeonTowel/SeeThroughWindows-dist/releases/tag/$stableTag" -ForegroundColor Cyan

Write-Host "`n🎯 Next steps:" -ForegroundColor Yellow
Write-Host "  1. Monitor the GitHub Actions workflow" -ForegroundColor White
Write-Host "  2. Verify the stable release works correctly" -ForegroundColor White
Write-Host "  3. Update any documentation or announcements" -ForegroundColor White
Write-Host "  4. Consider updating package managers (Scoop, etc.)" -ForegroundColor White
