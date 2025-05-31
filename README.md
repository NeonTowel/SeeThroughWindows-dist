# SeeThroughWindows

Press a hotkey to make the current window transparent, allowing you to see through it!

## Installation

### Option 1: Scoop Package Manager (Recommended)

If you have [Scoop](https://scoop.sh/) installed, you can easily install SeeThroughWindows using the package manager:

```powershell
# Add the NeonTowel bucket
scoop bucket add neontowel https://github.com/NeonTowel/scoop-bucket

# Install SeeThroughWindows
scoop install neontowel/seethroughwindows
```

This method automatically handles:

- Downloading the latest version
- Installing to the correct location
- Adding to your PATH
- Easy updates with `scoop update seethroughwindows`

### Option 2: Direct Download

Releases can also be downloaded directly from the [GitHub Releases page](https://github.com/NeonTowel/SeeThroughWindows-dist/releases/latest).

The current release is [v1.0.9](https://github.com/NeonTowel/SeeThroughWindows-dist/releases/tag/v1.0.9), which includes both:

- **Framework-dependent**: Requires .NET 9 runtime to be installed
- **Self-contained**: Includes .NET 9 runtime (larger file size)

## Features

- **Global Hotkeys**: Configure custom hotkey combinations to toggle window transparency
- **Multi-Monitor Support**: Move windows between monitors with hotkeys
- **Window Management**: Minimize/maximize windows with hotkeys
- **Click-Through Mode**: Make windows transparent to mouse clicks
- **System Tray Integration**: Runs minimized in the system tray
- **Single Instance**: Prevents multiple instances from running simultaneously

## Usage

1. Run the application (it will appear in the system tray)
2. Configure your preferred hotkey combination
3. Press the hotkey while any window is active to toggle its transparency
4. Use additional hotkeys for window management:
   - `Ctrl+Win+Up/Down`: Maximize/minimize windows
   - `Ctrl+Win+Left/Right`: Move windows between monitors
   - `Ctrl+Win+PageUp/PageDown`: Adjust transparency levels

## System Requirements

- Windows 7 or later
- .NET 9.0 Runtime (included in standalone releases)

**Note**: See Through Windows is Windows-only - it will not run on Linux or Mac!

## Development

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio Code](https://code.visualstudio.com/) with C# Dev Kit extension
- Windows OS (this is a Windows-only application)

### Getting Started with VS Code

1. **Clone the repository**:

   ```bash
   git clone <repository-url>
   cd SeeThroughWindows-dist
   code .
   ```

2. **Install recommended extensions**: Open the project in VS Code and install the recommended extensions when prompted, or run:

   ```bash
   code --install-extension ms-dotnettools.csdevkit
   ```

3. **Build the project**:

   ```bash
   dotnet build
   ```

4. **Run the application**:

   ```bash
   dotnet run --project SeeThroughWindows
   ```

5. **Debug**: Press `F5` or use the "Launch SeeThroughWindows" configuration

### Available Commands

#### VS Code Tasks

- **Build**: `Ctrl+Shift+P` → "Tasks: Run Task" → "build"
- **Clean**: `Ctrl+Shift+P` → "Tasks: Run Task" → "clean"
- **Publish**: `Ctrl+Shift+P` → "Tasks: Run Task" → "publish"

#### PowerShell Scripts

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

#### Command Line

```bash
# Restore packages
dotnet restore

# Build
dotnet build

# Run
dotnet run --project SeeThroughWindows

# Publish self-contained
dotnet publish SeeThroughWindows -c Release --self-contained true --runtime win-x64

# Publish framework-dependent
dotnet publish SeeThroughWindows -c Release --self-contained false
```

### Project Structure

```
SeeThroughWindows/
├── .github/                    # GitHub workflows and templates
│   └── workflows/
│       └── release.yml         # Automated release workflow
├── .vscode/                    # VS Code configuration
│   ├── extensions.json         # Recommended extensions
│   ├── launch.json            # Debug configuration
│   ├── settings.json          # Workspace settings
│   └── tasks.json             # Build tasks
├── docs/                      # Documentation
│   └── DEVELOPMENT.md         # Detailed development guide
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
├── Directory.Build.props    # MSBuild properties
├── global.json             # .NET SDK version requirements
└── SeeThroughWindows.sln   # Solution file
```

### Architecture

The application is built using:

- **Windows Forms**: UI framework for the system tray and configuration
- **Win32 APIs**: Window manipulation and global hotkey registration
- **Registry**: Settings persistence
- **Single Instance Pattern**: Prevents multiple instances using a named Mutex

### Key Components

- **Program.cs**: Application entry point with single-instance enforcement
- **SeeThrougWindowsForm.cs**: Main form handling UI and window management logic
- **Hotkey.cs**: Global hotkey registration and management using Win32 APIs
- **System Tray Integration**: Minimizes to system tray for background operation

### Development Workflow

1. **Code Style**: The project uses EditorConfig for consistent formatting
2. **Code Analysis**: .NET analyzers enabled for code quality
3. **Debugging**: Full VS Code debugging support with breakpoints and watch variables
4. **Testing**: Manual testing required (Windows Forms application)

For detailed development information, see [docs/DEVELOPMENT.md](docs/DEVELOPMENT.md).

### Release Process

Releases are automated through GitHub Actions:

- Push a tag like `v1.0.9` to trigger a release
- Or manually trigger with workflow dispatch
- Creates both framework-dependent and self-contained builds
- Automatically generates changelog and uploads release assets

## License

Copyright © 2008-2024, MOBZystems BV, Amsterdam

See [LICENSE](LICENSE) file for details.
