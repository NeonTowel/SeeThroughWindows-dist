#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Build script for SeeThroughWindows
.DESCRIPTION
    This script provides various build and development tasks for the SeeThroughWindows project.
.PARAMETER Task
    The task to execute: Build, Clean, Restore, Publish, Run, Test
.PARAMETER Configuration
    Build configuration: Debug or Release (default: Debug)
.EXAMPLE
    .\build.ps1 -Task Build
    .\build.ps1 -Task Publish -Configuration Release
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet("Build", "Clean", "Restore", "Publish", "Run", "Test", "Format")]
    [string]$Task,

    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"
$ProjectPath = Join-Path $PSScriptRoot ".." "SeeThroughWindows" "SeeThroughWindows.csproj"
$SolutionPath = Join-Path $PSScriptRoot ".." "SeeThroughWindows.sln"

function Write-Header {
    param([string]$Message)
    Write-Host "=== $Message ===" -ForegroundColor Cyan
}

function Invoke-DotNet {
    param([string[]]$Arguments)
    Write-Host "dotnet $($Arguments -join ' ')" -ForegroundColor Yellow
    & dotnet @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet command failed with exit code $LASTEXITCODE"
    }
}

switch ($Task) {
    "Restore" {
        Write-Header "Restoring packages"
        Invoke-DotNet @("restore", $SolutionPath)
    }

    "Clean" {
        Write-Header "Cleaning solution"
        Invoke-DotNet @("clean", $SolutionPath, "--configuration", $Configuration)
    }

    "Build" {
        Write-Header "Building solution ($Configuration)"
        Invoke-DotNet @("build", $SolutionPath, "--configuration", $Configuration, "--no-restore")
    }

    "Publish" {
        Write-Header "Publishing application ($Configuration)"
        $PublishPath = Join-Path $PSScriptRoot ".." "publish"
        Invoke-DotNet @("publish", $ProjectPath, "--configuration", $Configuration, "--output", $PublishPath, "--self-contained", "true", "--runtime", "win-x64")
        Write-Host "Published to: $PublishPath" -ForegroundColor Green
    }

    "Run" {
        Write-Header "Running application"
        Invoke-DotNet @("run", "--project", $ProjectPath, "--configuration", $Configuration)
    }

    "Test" {
        Write-Header "Running tests"
        # Add test projects when available
        Write-Host "No test projects found" -ForegroundColor Yellow
    }

    "Format" {
        Write-Header "Formatting code"
        Invoke-DotNet @("format", $SolutionPath)
    }
}

Write-Host "Task '$Task' completed successfully!" -ForegroundColor Green
