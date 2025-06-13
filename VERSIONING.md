# 🏷️ Versioning Strategy for SeeThroughWindows

This document outlines the semantic versioning strategy for SeeThroughWindows, including beta and stable release processes.

## 📋 Overview

We follow [Semantic Versioning 2.0.0](https://semver.org/) with pre-release identifiers for beta testing:

- **Stable releases**: `v1.0.9`, `v1.1.0`, `v2.0.0`
- **Beta releases**: `v1.0.9-beta.1`, `v1.1.0-beta.2`, `v2.0.0-rc.1`

## 🌿 Branch Strategy

| Branch   | Purpose                        | Release Type    | Example Tags                     |
| -------- | ------------------------------ | --------------- | -------------------------------- |
| `main`   | Stable, production-ready code  | Stable releases | `v1.0.9`, `v1.1.0`               |
| `devel`  | Integration branch for testing | Beta releases   | `v1.0.9-beta.1`, `v1.1.0-beta.2` |
| `feat/*` | Feature development            | No releases     | -                                |

## 🧪 Beta Release Process

### When to Create Beta Releases

- New features are ready for testing
- Bug fixes need validation
- Before merging to `main` branch
- Regular testing cycles (weekly/bi-weekly)

### Creating a Beta Release

1. **Ensure you're on `devel` branch**:

   ```bash
   git checkout devel
   git pull origin devel
   ```

2. **Use the beta release script**:

   ```powershell
   .\scripts\create-beta-release.ps1 -Version "1.0.9" -BetaNumber 1
   ```

3. **The script will**:
   - Validate you're on `devel` branch
   - Check for clean working directory
   - Show recent commits for review
   - Create and push tag `v1.0.9-beta.1`
   - Trigger GitHub Actions workflow

### Beta Release Naming

- **First beta**: `v1.0.9-beta.1`
- **Second beta**: `v1.0.9-beta.2`
- **Release candidate**: `v1.0.9-rc.1`

## 🌟 Stable Release Process

### When to Create Stable Releases

- Beta testing is complete
- All critical bugs are fixed
- Features are stable and tested
- Ready for production use

### Creating a Stable Release

1. **Option A: Manual merge then release**:

   ```bash
   git checkout main
   git pull origin main
   git merge origin/devel --no-ff
   git push origin main
   ```

   ```powershell
   .\scripts\create-stable-release.ps1 -Version "1.0.9"
   ```

2. **Option B: Automated merge and release**:
   ```powershell
   .\scripts\create-stable-release.ps1 -Version "1.0.9" -MergeFromDevel -Push
   ```

## 🔄 Complete Workflow Example

### Scenario: Releasing v1.1.0 with new features

1. **Development Phase**:

   ```bash
   # Work on feature branches
   git checkout -b feat/new-feature
   # ... develop feature ...
   git checkout devel
   git merge feat/new-feature
   git push origin devel
   ```

2. **Beta Testing Phase**:

   ```powershell
   # Create first beta
   .\scripts\create-beta-release.ps1 -Version "1.1.0" -BetaNumber 1

   # After testing and fixes, create second beta
   .\scripts\create-beta-release.ps1 -Version "1.1.0" -BetaNumber 2

   # Final release candidate
   .\scripts\create-beta-release.ps1 -Version "1.1.0" -BetaNumber 3
   ```

3. **Stable Release Phase**:
   ```powershell
   # Create stable release
   .\scripts\create-stable-release.ps1 -Version "1.1.0" -MergeFromDevel
   ```

## 📦 Release Artifacts

### Beta Releases

- Marked as "pre-release" on GitHub
- Include warning about beta status
- Built from `devel` branch
- Filename includes beta identifier

### Stable Releases

- Marked as "latest release" on GitHub
- Production-ready documentation
- Built from `main` branch
- Clean version numbers

## 🎯 Version Numbering Guidelines

### Major Version (X.0.0)

- Breaking changes
- Major feature overhauls
- API changes
- Requires user action

### Minor Version (X.Y.0)

- New features
- Enhancements
- Backward compatible
- Optional upgrades

### Patch Version (X.Y.Z)

- Bug fixes
- Security patches
- Performance improvements
- Recommended upgrades

### Pre-release Identifiers

- `beta.N`: Feature complete, needs testing
- `rc.N`: Release candidate, final testing
- `alpha.N`: Early development (if needed)

## 🛠️ Automation Features

### GitHub Actions Integration

- Automatic builds on tag push
- Different workflows for beta vs stable
- Proper changelog generation
- Asset packaging and upload

### Script Features

- Branch validation
- Version format validation
- Existing tag checking
- Interactive confirmation
- Automatic cleanup on failure

## 📋 Checklist Templates

### Beta Release Checklist

- [ ] All features merged to `devel`
- [ ] Basic testing completed
- [ ] No critical bugs
- [ ] Version number decided
- [ ] Beta release script executed
- [ ] GitHub Actions workflow successful
- [ ] Beta testers notified

### Stable Release Checklist

- [ ] Beta testing completed
- [ ] All issues resolved
- [ ] Documentation updated
- [ ] Changelog reviewed
- [ ] `devel` merged to `main`
- [ ] Stable release script executed
- [ ] Release announcement prepared
- [ ] Package managers updated (Scoop, etc.)

## 🔍 Troubleshooting

### Common Issues

**Script fails with "not on correct branch"**:

- Ensure you're on `devel` for beta releases
- Ensure you're on `main` for stable releases
- Use `-MergeFromDevel` for automatic branch handling

**Tag already exists**:

- Check existing tags: `git tag -l`
- Use next beta number or different version
- Delete local tag if needed: `git tag -d v1.0.9-beta.1`

**GitHub Actions workflow fails**:

- Check workflow logs in GitHub Actions tab
- Verify .NET version compatibility
- Check for build errors or test failures

### Recovery Procedures

**Failed beta release**:

```bash
# Delete local and remote tag
git tag -d v1.0.9-beta.1
git push origin :refs/tags/v1.0.9-beta.1
# Fix issues and retry
```

**Failed stable release**:

```bash
# If merge was successful but tag failed
git tag -d v1.0.9
# Fix issues and retry tag creation
```

## 📚 References

- [Semantic Versioning 2.0.0](https://semver.org/)
- [GitHub Releases Documentation](https://docs.github.com/en/repositories/releasing-projects-on-github)
- [Git Tagging Documentation](https://git-scm.com/book/en/v2/Git-Basics-Tagging)
