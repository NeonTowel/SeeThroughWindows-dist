# SeeThroughWindows

Press a hotkey to make the current window transparent, allowing you to see through it!

## Development

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio Code](https://code.visualstudio.com/) with C# Dev Kit extension
- Windows OS (this is a Windows-only application)

### Getting Started with VS Code

1. **Install recommended extensions**: Open the project in VS Code and install the recommended extensions when prompted, or run:

   ```bash
   code --install-extension ms-dotnettools.csdevkit
   ```

2. **Build the project**:

   ```bash
   dotnet build
   ```

3. **Run the application**:

   ```bash
   dotnet run --project SeeThroughWindows
   ```

4. **Debug**: Press `F5` or use the "Launch SeeThroughWindows" configuration

### Available Commands

- **Build**: `Ctrl+Shift+P` → "Tasks: Run Task" → "build"
- **Clean**: `Ctrl+Shift+P` → "Tasks: Run Task" → "clean"
- **Publish**: `Ctrl+Shift+P` → "Tasks: Run Task" → "publish"

### Project Structure

```
SeeThroughWindows/
├── .vscode/                 # VS Code configuration
│   ├── extensions.json      # Recommended extensions
│   ├── launch.json         # Debug configuration
│   ├── settings.json       # Workspace settings
│   └── tasks.json          # Build tasks
├── SeeThroughWindows/       # Main application
│   ├── Properties/          # Assembly info and resources
│   ├── images/             # Application icons
│   ├── Hotkey.cs           # Global hotkey management
│   ├── Program.cs          # Application entry point
│   ├── SeeThrougWindowsForm.cs # Main form logic
│   └── SeeThroughWindows.csproj # Project file
├── SeeThroughWindowsSetup/  # Installer project
├── .editorconfig           # Code style configuration
└── SeeThroughWindows.sln   # Solution file
```

## Features

- **Global Hotkeys**: Configure custom hotkey combinations to toggle window transparency
- **Multi-Monitor Support**: Move windows between monitors with hotkeys
- **Window Management**: Minimize/maximize windows with hotkeys
- **Click-Through Mode**: Make windows transparent to mouse clicks
- **System Tray Integration**: Runs minimized in the system tray

## Usage

1. Run the application (it will appear in the system tray)
2. Configure your preferred hotkey combination
3. Press the hotkey while any window is active to toggle its transparency
4. Use additional hotkeys for window management:
   - `Ctrl+Win+Up/Down`: Maximize/minimize windows
   - `Ctrl+Win+Left/Right`: Move windows between monitors
   - `Ctrl+Win+PageUp/PageDown`: Adjust transparency levels

## Downloads

Releases can be downloaded from https://www.mobzystems.com/Tools/SeeThroughWindows

The current release is 1.0.8, which runs standalone on .NET 8.0, so downloading .NET is no longer necessary.

## System Requirements

- Windows 7 or later
- .NET 8.0 Runtime (included in standalone releases)

**Note**: See Through Windows is Windows-only - it will not run on Linux or Mac!
