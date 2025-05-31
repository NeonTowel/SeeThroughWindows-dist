# GitHub Actions Workflows

This repository contains automated workflows for building and distributing the SeeThroughWindows application.

## Workflows Overview

### 1. CI Build (`ci.yml`)

**Purpose**: Quick feedback for development work
**Triggers**:

- Push to `main`, `develop`, or `feature/*` branches
- Pull requests to `main` or `develop`

**What it does**:

- Sets up .NET 9
- Restores dependencies
- Builds the solution in Debug configuration
- Runs tests (if any exist)

### 2. Build and Package (`build-and-package.yml`)

**Purpose**: Complete build and packaging for testing
**Triggers**:

- Push to `main` or `develop` branches
- Pull requests to `main`
- Manual trigger via GitHub UI
- Release creation

**What it does**:

- Builds in Release configuration
- Creates a packaged ZIP file with version number
- Uploads artifacts to GitHub Actions
- Automatically uploads to GitHub Releases (if triggered by release)

### 3. Release (`release.yml`)

**Purpose**: Create official releases with packaged binaries
**Triggers**:

- Push of version tags (e.g., `v1.0.8`, `v2.1.0`)
- Manual trigger via GitHub UI

**What it does**:

- Builds both framework-dependent and self-contained versions
- Creates two ZIP packages:
  - Framework-dependent (requires .NET 9 runtime)
  - Self-contained (includes .NET 9 runtime)
- Generates changelog from git commits
- Creates GitHub Release with packages attached

## How to Use

### For Development (CI Build)

Just push your code or create a pull request. The CI workflow will automatically run and give you feedback.

### For Testing Builds (Build and Package)

1. Push to `main` or `develop` branch
2. Go to the Actions tab in GitHub
3. Find the completed workflow run
4. Download the ZIP artifact from the workflow

### For Official Releases

#### Option 1: Using Git Tags (Recommended)

```bash
# Create and push a version tag
git tag v1.0.8
git push origin v1.0.8
```

#### Option 2: Manual Release

1. Go to Actions tab in GitHub
2. Select "Release" workflow
3. Click "Run workflow"
4. Enter the version number (e.g., `1.0.8`)
5. Click "Run workflow"

## Package Types

### Framework-dependent

- **File**: `SeeThroughWindows-v{version}-framework-dependent.zip`
- **Requirements**: .NET 9 runtime must be installed on target machine
- **Size**: Smaller download
- **Use case**: When you know .NET 9 is available

### Self-contained

- **File**: `SeeThroughWindows-v{version}-self-contained-win-x64.zip`
- **Requirements**: No .NET installation needed
- **Size**: Larger download (~100MB+)
- **Use case**: For distribution to machines without .NET

## Workflow Configuration

Key environment variables in the workflows:

- `DOTNET_VERSION`: '9.0.x' (Latest .NET 9)
- `PROJECT_PATH`: 'SeeThroughWindows/SeeThroughWindows.csproj'
- `SOLUTION_PATH`: 'SeeThroughWindows.sln'
- `BUILD_CONFIGURATION`: 'Release' (for packaging workflows)

## Troubleshooting

### .NET 9 Support

The workflows use `actions/setup-dotnet@v4` with `dotnet-version: '9.0.x'` to ensure .NET 9 support, as referenced in the [GitHub Actions setup-dotnet documentation](https://github.com/actions/setup-dotnet/issues/562).

### Windows-specific

All workflows run on `windows-latest` runners since this is a Windows Forms application.

### Artifacts Retention

Build artifacts are kept for 30 days by default. You can download them from the Actions tab.

## Status Badges

Add these to your main README.md to show build status:

```markdown
[![CI Build](https://github.com/YOUR_USERNAME/YOUR_REPO/actions/workflows/ci.yml/badge.svg)](https://github.com/YOUR_USERNAME/YOUR_REPO/actions/workflows/ci.yml)
[![Build and Package](https://github.com/YOUR_USERNAME/YOUR_REPO/actions/workflows/build-and-package.yml/badge.svg)](https://github.com/YOUR_USERNAME/YOUR_REPO/actions/workflows/build-and-package.yml)
```
