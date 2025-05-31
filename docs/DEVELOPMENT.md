# Development Guide

This document provides detailed information for developers working on SeeThroughWindows using Visual Studio Code.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio Code](https://code.visualstudio.com/)
- [C# Dev Kit Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- Windows OS (required for Windows Forms and Win32 APIs)

## Project Structure

```
SeeThroughWindows/
├── .github/                    # GitHub workflows and templates
│   └── workflows/
│       └── release.yml         # Automated release workflow
├── .vscode/                    # VS Code configuration
│   ├── extensions.json         # Recommended extensions
│   ├── launch.json            # Debug configurations
│   ├── settings.json          # Workspace settings
│   ├── tasks.json             # Build tasks
│   └── csharp.code-snippets   # Code snippets
├── docs/                      # Documentation
│   └── DEVELOPMENT.md         # This file
├── scripts/                   # Build and utility scripts
│   └── build.ps1             # PowerShell build script
├── SeeThroughWindows/         # Main application
│   ├── Properties/            # Assembly info and resources
│   ├── images/               # Application icons
│   ├── Hotkey.cs             # Global hotkey management
│   ├── Program.cs            # Application entry point
│   ├── SeeThrougWindowsForm.cs # Main form logic
│   ├── SeeThrougWindowsForm.Designer.cs # Form designer code
│   └── SeeThroughWindows.csproj # Project file
├── SeeThroughWindowsSetup/    # Installer project
│   └── SeeThroughWindowsSetup.vdproj # Visual Studio installer project
├── .editorconfig             # Code style configuration
├── .gitignore               # Git ignore rules
├── Directory.Build.props     # MSBuild properties
├── global.json              # .NET SDK version requirements
└── SeeThroughWindows.sln    # Solution file
```

## Getting Started

### 1. Clone and Setup

```bash
git clone https://github.com/NeonTowel/SeeThroughWindows-dist.git
cd SeeThroughWindows-dist
code .
```

### 2. Install Extensions

When you open the project, VS Code will prompt you to install recommended extensions. Accept the prompt or run:

```bash
code --install-extension ms-dotnettools.csdevkit
```

### 3. Build and Run

#### Using VS Code Tasks

- **Build**: `Ctrl+Shift+P` → "Tasks: Run Task" → "build"
- **Clean**: `Ctrl+Shift+P` → "Tasks: Run Task" → "clean"
- **Run**: `F5` or `Ctrl+F5`

#### Using Command Line

```bash
# Restore packages
dotnet restore

# Build
dotnet build

# Run
dotnet run --project SeeThroughWindows

# Publish framework-dependent
dotnet publish SeeThroughWindows -c Release --self-contained false

# Publish self-contained
dotnet publish SeeThroughWindows -c Release --self-contained true --runtime win-x64
```

#### Using PowerShell Scripts

```powershell
# Build
.\scripts\build.ps1 -Task Build

# Run
.\scripts\build.ps1 -Task Run

# Publish Release
.\scripts\build.ps1 -Task Publish -Configuration Release

# Clean
.\scripts\build.ps1 -Task Clean

# Format code
.\scripts\build.ps1 -Task Format
```

## Development Workflow

### Code Style

The project uses EditorConfig for consistent formatting:

- **Indentation**: 4 spaces for C#, 2 spaces for JSON/YAML
- **Line endings**: CRLF (Windows standard)
- **Encoding**: UTF-8

Format code with: `Ctrl+Shift+P` → "Format Document" or `Shift+Alt+F`

### Code Analysis

The project includes .NET analyzers for code quality:

- **Nullable reference types**: Enabled for better null safety
- **Code analysis**: Latest rules enabled
- **Warnings**: Configured to not treat as errors during development

### Debugging

#### Debug Configurations

1. **Launch SeeThroughWindows**: Debug the main application
2. **Attach to Process**: Attach to running instance

#### Debugging Tips

- Set breakpoints in VS Code by clicking in the gutter
- Use the Debug Console for evaluating expressions
- Watch variables in the Variables panel
- Use conditional breakpoints for complex scenarios

### Code Snippets

The project includes custom snippets for common patterns:

- `dllimport` - Win32 DLL import declaration
- `eventhandler` - Event handler method
- `trywin32` - Try-catch for Win32 operations
- `registry` - Safe registry access
- `hotkey` - Complete hotkey registration

Type the prefix and press `Tab` to expand.

## Architecture Overview

### Core Components

1. **Program.cs**: Application entry point with single-instance check using named Mutex
2. **SeeThrougWindowsForm.cs**: Main form with UI logic and system tray integration
3. **Hotkey.cs**: Global hotkey management using Win32 APIs
4. **WindowInfo**: Tracks modified window states

### Key Technologies

- **Windows Forms**: UI framework for system tray and configuration
- **Win32 APIs**: Window manipulation and hotkey registration
- **Registry**: Settings persistence
- **Single Instance Pattern**: Prevents multiple instances using named Mutex

### Win32 APIs Used

- `SetLayeredWindowAttributes`: Window transparency
- `RegisterHotKey`/`UnregisterHotKey`: Global hotkeys
- `GetForegroundWindow`: Active window detection
- `SetWindowPos`: Window positioning
- Various window state APIs

## Release Process

### Automated Releases

The project uses GitHub Actions for automated releases:

1. **Tag-based releases**: Push a tag like `v1.0.9` to trigger a release
2. **Manual releases**: Use workflow dispatch with version input
3. **Build artifacts**: Creates both framework-dependent and self-contained builds
4. **Distribution**:
   - GitHub releases at [SeeThroughWindows-dist/releases](https://github.com/NeonTowel/SeeThroughWindows-dist/releases)
   - Scoop package at [scoop-bucket](https://github.com/NeonTowel/scoop-bucket)

### Release Workflow

The `.github/workflows/release.yml` workflow:

- Builds both framework-dependent and self-contained versions
- Runs tests (if available)
- Creates release packages with proper naming
- Generates changelog from git commits
- Uploads assets to GitHub releases

### Manual Release Steps

1. Update version in `SeeThroughWindows.csproj`
2. Commit changes
3. Create and push a version tag:
   ```bash
   git tag v1.0.10
   git push origin v1.0.10
   ```
4. GitHub Actions will automatically create the release

## Common Development Tasks

### Adding a New Hotkey

1. Declare the hotkey field:

```csharp
private Hotkey newHotkey = new Hotkey(Keys.F12, false, true, false, false);
```

2. Register in constructor:

```csharp
this.newHotkey.Pressed += NewHotkey_Pressed;
this.newHotkey.Register(this);
```

3. Implement handler:

```csharp
private void NewHotkey_Pressed(object sender, HandledEventArgs e)
{
    // Implementation
    e.Handled = true;
}
```

### Adding Settings

1. Add registry key in `SaveSettings()`:

```csharp
root.SetValue("NewSetting", value);
```

2. Read in constructor:

```csharp
var setting = root.GetValue("NewSetting", defaultValue);
```

### Win32 API Integration

1. Add DLL import:

```csharp
[DllImport("user32.dll")]
private static extern bool NewWin32Function(IntPtr hWnd, int param);
```

2. Use with error handling:

```csharp
try
{
    if (!NewWin32Function(handle, param))
    {
        throw new Win32Exception();
    }
}
catch (Win32Exception ex)
{
    // Handle error
}
```

## Testing

### Manual Testing

1. Build and run the application
2. Test hotkey registration and functionality
3. Verify window transparency operations
4. Check system tray integration
5. Test settings persistence
6. Verify single instance behavior

### Debugging Win32 Issues

- Use Windows Event Viewer for system-level errors
- Check `GetLastError()` for Win32 API failures
- Use Process Monitor to track registry/file access
- Test on different Windows versions

## Troubleshooting

### Common Issues

1. **Hotkey registration fails**

   - Check if hotkey is already registered by another app
   - Verify modifier key combinations
   - Run as administrator if needed

2. **Transparency not working**

   - Ensure target window supports layered windows
   - Check if window is already transparent
   - Verify Win32 API return values

3. **Build errors**

   - Restore NuGet packages: `dotnet restore`
   - Clean and rebuild: `dotnet clean && dotnet build`
   - Check .NET SDK version in `global.json`

4. **Release workflow issues**
   - Verify GitHub Actions permissions
   - Check workflow file syntax
   - Ensure proper version tagging

### Performance Considerations

- Minimize Win32 API calls in hot paths
- Cache window information when possible
- Use proper disposal patterns for resources
- Avoid blocking the UI thread

## Contributing

1. Follow the existing code style and patterns
2. Add XML documentation for public APIs
3. Include error handling for Win32 operations
4. Test on multiple Windows versions
5. Update documentation for new features
6. Ensure compatibility with both framework-dependent and self-contained builds

## Distribution

### Scoop Package

The application is available via Scoop package manager:

```powershell
scoop bucket add neontowel https://github.com/NeonTowel/scoop-bucket
scoop install neontowel/seethroughwindows
```

### GitHub Releases

Direct downloads are available from [GitHub Releases](https://github.com/NeonTowel/SeeThroughWindows-dist/releases):

- Framework-dependent builds (require .NET 9 runtime)
- Self-contained builds (include .NET 9 runtime)

## Resources

- [Windows Forms Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)
- [Win32 API Reference](https://docs.microsoft.com/en-us/windows/win32/api/)
- [.NET 9 Documentation](https://docs.microsoft.com/en-us/dotnet/core/)
- [VS Code C# Documentation](https://code.visualstudio.com/docs/languages/csharp)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Scoop Documentation](https://scoop.sh/)
