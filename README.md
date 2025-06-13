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

### 🎨 Beautiful Catppuccin Theming

- **Four stunning theme flavors**: Latte (light), Frappé, Macchiato, and Mocha (dark variants)
- **10 customizable accent colors**: Lavender, Blue, Mauve, Pink, Teal, Green, Peach, Yellow, Red, and Sky
- **Real-time theme switching**: Change themes and accent colors instantly without restarting
- **Enhanced UI elements**: Custom-rendered checkboxes, track bars, and controls with improved visibility

### 🚀 Advanced Window Management

- **Global Hotkeys**: Configure custom hotkey combinations to toggle window transparency
- **Auto-Apply on Startup**: Automatically apply transparency to all eligible windows when the application starts
- **Global Reset Function**: Reset transparency for all non-opaque windows with a single button
- **Multi-Monitor Support**: Move windows between monitors with hotkeys
- **Transparency Levels**: Fine-tune transparency with increment/decrement hotkeys
- **Click-Through Mode**: Make windows transparent to mouse clicks

### 🔧 Enhanced System Integration

- **Service-Based Architecture**: Modular design with dedicated services for window management, settings, and auto-apply functionality
- **System Tray Integration**: Runs minimized in the system tray with comprehensive context menu
- **Single Instance**: Prevents multiple instances from running simultaneously
- **Persistent Settings**: All preferences including themes and hotkeys are saved between sessions

### 🛠️ Developer Tools

- **WindowDebugger**: Standalone debugging tool for window transparency management and testing
- **Comprehensive Logging**: Enhanced debug output for troubleshooting window operations
- **Modular Service Container**: Dependency injection pattern for better testability and maintainability

## Usage

1. Run the application (it will appear in the system tray)
2. Configure your preferred theme and accent color in the Appearance section
3. Set up your hotkey combinations in the Hotkeys section
4. Enable "Auto-Apply on Startup" if you want transparency applied automatically
5. Press your configured hotkey while any window is active to toggle its transparency

### Default Hotkeys

- **Toggle Transparency**: `Ctrl+Win+T` (configurable)
- **Maximize/Minimize**: `Ctrl+Win+Up/Down`
- **Move Between Monitors**: `Ctrl+Win+Left/Right`
- **Adjust Transparency**: `Ctrl+Win+PageUp/PageDown`

### New Features in v1.0.9

- **Auto-Apply on Startup**: Automatically make all eligible windows transparent when the app starts
- **Global Reset**: Reset transparency for all windows with the "Reset Transparency Globally" button
- **Enhanced Theming**: Beautiful Catppuccin themes with customizable accent colors
- **Improved Service Architecture**: More reliable window management with better error handling

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

# Run main application
dotnet run --project SeeThroughWindows

# Run WindowDebugger tool
dotnet run --project WindowDebugger

# Publish self-contained
dotnet publish SeeThroughWindows -c Release --self-contained true --runtime win-x64

# Publish framework-dependent
dotnet publish SeeThroughWindows -c Release --self-contained false
```

### Project Structure

```
SeeThroughWindows/
├── .cursorrules/                   # Cursor AI coding standards and guidelines
├── .github/                        # GitHub workflows and templates
│   └── workflows/
│       ├── build-and-package.yml   # Build and packaging workflow
│       └── ci.yml                  # Continuous integration workflow
├── .vscode/                        # VS Code configuration
│   ├── extensions.json             # Recommended extensions
│   ├── launch.json                # Debug configuration
│   ├── settings.json              # Workspace settings
│   └── tasks.json                 # Build tasks
├── docs/                          # Documentation
│   └── DEVELOPMENT.md             # Detailed development guide
├── scripts/                       # Build and utility scripts
│   └── build.ps1                 # PowerShell build script
├── SeeThroughWindows/             # Main application
│   ├── Infrastructure/            # Dependency injection and service container
│   ├── Models/                    # Data models and DTOs
│   ├── Properties/                # Assembly info and resources
│   ├── Services/                  # Business logic services
│   │   ├── ApplicationService.cs  # Main application coordination
│   │   ├── AutoApplyService.cs   # Auto-apply transparency functionality
│   │   ├── HotkeyManager.cs      # Global hotkey management
│   │   ├── SettingsManager.cs    # Settings persistence
│   │   ├── UpdateChecker.cs      # Update checking service
│   │   ├── Win32Api.cs           # Windows API wrappers
│   │   └── WindowManager.cs      # Window manipulation service
│   ├── Themes/                   # Catppuccin theme system
│   │   ├── CatppuccinTheme.cs    # Theme definitions and color palettes
│   │   └── ThemeManager.cs       # Theme application and management
│   ├── images/                   # Application icons
│   ├── Hotkey.cs                 # Global hotkey registration
│   ├── Program.cs                # Application entry point
│   ├── SeeThrougWindowsForm.cs   # Main form logic
│   ├── SeeThrougWindowsForm.Designer.cs # Form designer code
│   └── SeeThroughWindows.csproj  # Project file
├── WindowDebugger/               # Standalone debugging tool
│   └── WindowDebugger.csproj    # Debug tool project file
├── SeeThroughWindowsSetup/       # Installer project
│   └── SeeThroughWindowsSetup.vdproj # Visual Studio installer project
├── .editorconfig                # Code style configuration
├── .gitignore                  # Git ignore rules
├── CATPPUCCIN_THEME_README.md  # Detailed theming documentation
├── REFACTORING_README.md       # Architecture and refactoring notes
├── Directory.Build.props       # MSBuild properties
├── global.json                # .NET SDK version requirements
└── SeeThroughWindows.sln      # Solution file
```

### Architecture

The application now features a modern service-based architecture:

#### Core Technologies

- **Windows Forms**: UI framework for the system tray and configuration
- **Win32 APIs**: Window manipulation and global hotkey registration
- **Service Container**: Dependency injection for modular design
- **Registry**: Settings and theme persistence

#### Service Architecture

- **ApplicationService**: Main coordination service for window transparency operations
- **WindowManager**: Low-level window manipulation and Win32 API interactions
- **SettingsManager**: Configuration persistence and retrieval
- **AutoApplyService**: Automatic transparency application on startup
- **HotkeyManager**: Global hotkey registration and management
- **ThemeManager**: Catppuccin theme application and customization

#### Key Components

- **Program.cs**: Application entry point with service container initialization
- **SeeThrougWindowsForm.cs**: Main UI with enhanced theming and new features
- **CatppuccinTheme.cs**: Complete implementation of Catppuccin color system
- **WindowDebugger**: Standalone tool for debugging window transparency issues

### Development Workflow

1. **Code Style**: The project uses EditorConfig and Cursor AI coding standards for consistent formatting
2. **Code Analysis**: .NET analyzers enabled for code quality
3. **Service-Based Testing**: Modular architecture allows for better unit testing
4. **Theme Development**: Real-time theme switching for rapid UI development
5. **Debugging**: Enhanced logging and dedicated WindowDebugger tool

### New in v1.0.9 Development Features

- **Enhanced CI/CD**: Updated GitHub workflows for automated building and packaging
- **Cursor AI Integration**: Comprehensive coding standards and guidelines in `.cursorrules/`
- **Modular Architecture**: Service-based design with dependency injection
- **Theme System**: Complete Catppuccin implementation with 4 flavors and 10 accent colors
- **Auto-Apply Feature**: Background service for automatic transparency application
- **Global Reset**: System-wide transparency reset functionality
- **WindowDebugger Tool**: Standalone debugging utility for development and troubleshooting

## Release Process

### 🧪 Beta Releases (from `devel` branch)

Beta releases use semantic versioning with pre-release identifiers:

```powershell
# Create a beta release
.\scripts\create-beta-release.ps1 -Version "1.0.9" -BetaNumber 1
# This creates tag: v1.0.9-beta.1
```

Beta releases are:

- Built from the `devel` branch
- Marked as "pre-release" on GitHub
- Include latest features but may be unstable
- Perfect for testing new functionality

### 🌟 Stable Releases (from `main` branch)

Stable releases are created after beta testing is complete:

```powershell
# Create a stable release (after merging devel to main)
.\scripts\create-stable-release.ps1 -Version "1.0.9" -MergeFromDevel
# This creates tag: v1.0.9
```

Stable releases are:

- Built from the `main` branch
- Thoroughly tested through beta releases
- Ready for production use
- Follow semantic versioning (X.Y.Z)

### 📋 Versioning Strategy

- **`main` branch**: Stable releases (`v1.0.9`, `v1.1.0`)
- **`devel` branch**: Beta releases (`v1.0.9-beta.1`, `v1.1.0-beta.2`)
- **Feature branches**: No automatic releases

### 🔄 Workflow

1. **Development**: Work on feature branches, merge to `devel`
2. **Beta Testing**: Create beta releases from `devel` using the script
3. **Stabilization**: Fix issues, create additional beta releases as needed
4. **Release**: Merge `devel` to `main`, create stable release

## License

Copyright © 2008-2024, MOBZystems BV, Amsterdam

See [LICENSE](LICENSE) file for details.
