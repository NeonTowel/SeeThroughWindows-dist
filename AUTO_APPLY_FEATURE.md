# Auto-Apply Transparency Feature

## Overview

The auto-apply transparency feature automatically applies transparency to all eligible visible windows when the application starts. This feature helps users who want to have transparency applied to multiple windows without having to manually activate it for each window.

## How It Works

### Feature Activation

1. Open the SeeThroughWindows application
2. Check the "Auto-apply transparency to visible windows on startup" checkbox
3. Configure your desired transparency settings (transparency level, click-through, top-most)
4. Restart the application to see the feature in action

### Window Eligibility Criteria

The auto-apply feature uses smart filtering to determine which windows should have transparency applied:

**Included Windows:**

- Regular application windows that are visible
- Windows with titles (non-empty window titles)
- Standard desktop applications

**Excluded Windows:**

- System processes (explorer, dwm, winlogon, csrss, etc.)
- The SeeThroughWindows application itself
- Desktop and shell windows
- Windows with problematic titles (empty, "Program Manager", etc.)
- Browsers (Edge, Chrome) - excluded to prevent stability issues
- Task Manager and security-related windows
- Windows that already have transparency applied

### Technical Implementation

- Uses Win32 `EnumWindows` API to enumerate all visible windows
- Applies transparency using the same settings configured in the main application
- Runs asynchronously on startup with a 2-second delay to ensure all windows are fully loaded
- Provides notifications about the number of windows that had transparency applied
- Integrates with the existing window management system to track applied transparency

### Safety Features

- Comprehensive exclusion list to prevent system instability
- Error handling for individual window operations
- Continues processing other windows if one fails
- Respects existing transparency settings (won't override already transparent windows)

### Settings Persistence

The auto-apply setting is saved to the Windows Registry along with other application settings and will persist between application restarts.

## Benefits

- **Productivity**: No need to manually apply transparency to each window
- **Consistency**: All eligible windows get the same transparency settings
- **Convenience**: Automatic setup on application startup
- **Safety**: Smart filtering prevents system issues

## Technical Architecture

The feature follows the application's existing design patterns:

- **Service-based architecture**: `IAutoApplyService` interface with `AutoApplyService` implementation
- **Dependency injection**: Properly registered in the service container
- **Settings management**: Integrated with existing `ISettingsManager` system
- **UI integration**: Seamlessly added to the existing form with proper event handling
- **Error handling**: Robust error handling with user notifications
