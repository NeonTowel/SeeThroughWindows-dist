# 🎨 Catppuccin Theme Implementation for SeeThroughWindows

This document describes the modern Catppuccin theme system implemented for SeeThroughWindows, bringing beautiful pastel colors and enhanced visual appeal to the application.

## 🌈 Overview

The Catppuccin theme system provides four beautiful pastel color palettes (flavors) that transform the SeeThroughWindows interface with soothing, eye-friendly colors. Based on the official [Catppuccin color palette](https://catppuccin.com/palette/), this implementation follows the project's design philosophy and style guidelines.

## ✨ Features

### 🎯 Four Theme Flavors

- **🌻 Latte** - Light theme harmoniously inverting the essence of Catppuccin's dark themes
- **🪴 Frappé** - A less vibrant alternative using subdued colors for a muted aesthetic
- **🌺 Macchiato** - Medium contrast with gentle colors creating a soothing atmosphere
- **🌿 Mocha** - The Original — Our darkest variant offering a cozy feeling with color-rich accents

### 🎨 Accent Color Selection

Choose from 10 beautiful Catppuccin accent colors to personalize your experience:

- **💜 Lavender** - Soft purple (default)
- **💙 Blue** - Classic blue
- **🔮 Mauve** - Rich purple
- **💗 Pink** - Playful pink
- **🌊 Teal** - Calming teal
- **💚 Green** - Natural green
- **🍑 Peach** - Warm orange
- **💛 Yellow** - Bright yellow
- **❤️ Red** - Bold red
- **🩵 Sky** - Light blue

Accent colors are used strategically throughout the interface for:

- Button press states
- Checkbox checked indicators
- Link colors
- Track bar thumbs and progress fill
- Menu item highlights

### 🔧 Technical Features

- **Complete Color Palette**: All 26 official Catppuccin colors implemented for each flavor
- **Accent Color System**: 10 selectable accent colors with strategic application throughout the UI
- **Enhanced Checkbox Visibility**: Custom-rendered checkboxes with high contrast for dark themes
- **Custom Track Bar**: Beautiful track bar with accent color thumb and filled progress indication
- **Automatic Theme Application**: Themes are applied to all UI controls automatically
- **Persistent Settings**: Theme and accent color preferences are saved and restored between sessions
- **Real-time Switching**: Change themes and accent colors instantly without restarting the application
- **Comprehensive Coverage**: Themes apply to buttons, checkboxes, combo boxes, track bars, group boxes, panels, links, labels, picture boxes, and context menus
- **Subtle Border System**: Enhanced border rendering using Catppuccin's design philosophy for refined visual hierarchy
- **Improved Spacing**: Better breathing room with enhanced padding and margins throughout the interface

## 🚀 Usage

### Selecting a Theme and Accent Color

1. Open SeeThroughWindows options
2. Navigate to the **Appearance** section
3. Select your preferred theme from the **Theme** dropdown:
   - 🌻 Latte (Light)
   - 🪴 Frappé (Dark, subdued)
   - 🌺 Macchiato (Dark, medium contrast)
   - 🌿 Mocha (Dark, high contrast)
4. Select your preferred accent color from the **Accent Color** dropdown:
   - 💜 Lavender, 💙 Blue, 🔮 Mauve, 💗 Pink, 🌊 Teal
   - 💚 Green, 🍑 Peach, 💛 Yellow, ❤️ Red, 🩵 Sky
5. Both theme and accent color apply immediately with enhanced spacing and improved visual hierarchy

### Default Settings

The application defaults to **Latte** theme with **Sky** accent color for new installations, providing a beautiful light theme with a calming blue accent.

## 🏗️ Technical Implementation

### Architecture

The theme system consists of three main components:

#### 1. `CatppuccinTheme.cs`

- Defines all four flavor enums and color palettes
- Contains complete hex color definitions for all 26 Catppuccin colors
- Provides accent color enum with 10 selectable options
- Provides helper methods for theme information and color access
- Maps semantic colors to UI elements with accent color integration

#### 2. `ThemeManager.cs`

- Manages theme and accent color application and persistence
- Handles registry storage of both theme and accent color preferences
- Applies themes to Windows Forms controls recursively
- Provides custom renderers for ToolStrip controls
- Manages theme change events
- Implements custom checkbox rendering for enhanced visibility

#### 3. Form Integration

- Theme and accent color selectors added to main form in "Appearance" group
- Automatic theme application on form load
- Real-time theme and accent color switching with immediate visual feedback
- Integrate with existing settings system

### Color Mapping

Each theme flavor provides semantic color mappings with accent color integration:

```csharp
Background = palette.Base          // Main background
Surface = palette.Surface0         // Secondary surfaces
Primary = palette.Blue            // Primary accent color
Secondary = palette.Mauve         // Secondary accent color
Accent = selectedAccent           // User-selected accent color
Text = palette.Text               // Primary text color
TextSecondary = palette.Subtext1  // Secondary text color
Border = palette.Overlay0         // Standard border colors
// Accent color integration
TrackBarThumb = selectedAccent    // Track bar uses accent color
LinkColor = selectedAccent        // Links use accent color
// Subtle border system for refined appearance
SubtleBorder = palette.Surface2   // Subtle borders for buttons and controls
PanelBorder = palette.Surface1    // Panel container borders
DividerColor = palette.Surface1/2 // Context-aware divider colors
// ... and more
```

### Control-Specific Theming

The system applies appropriate colors to different control types:

- **Buttons**: Custom flat style with hover effects
- **CheckBoxes**: Themed backgrounds and borders
- **ComboBoxes**: Consistent surface colors
- **TrackBars**: Background theming (limited by WinForms)
- **GroupBoxes**: Flat style with themed borders
- **Panels**: Surface color backgrounds
- **LinkLabels**: Catppuccin blue tones for links
- **Context Menus**: Custom renderer with full theming

### Persistence

Theme and accent color preferences are stored in the Windows Registry under:

```
HKEY_CURRENT_USER\Software\MOBZystems\MOBZXRay\Theme
HKEY_CURRENT_USER\Software\MOBZystems\MOBZXRay\AccentColor
```

## 🎨 Color Palettes

### Latte (Light Theme)

- **Base**: `#eff1f5` - Light background
- **Text**: `#4c4f69` - Dark text for contrast
- **Primary**: `#1e66f5` - Vibrant blue
- **Accent**: `#7287fd` - Soft lavender

### Frappé (Dark, Subdued)

- **Base**: `#303446` - Muted dark background
- **Text**: `#c6d0f5` - Light text
- **Primary**: `#8caaee` - Soft blue
- **Accent**: `#babbf1` - Gentle lavender

### Macchiato (Dark, Medium Contrast)

- **Base**: `#24273a` - Rich dark background
- **Text**: `#cad3f5` - Bright text
- **Primary**: `#8aadf4` - Vivid blue
- **Accent**: `#b7bdf8` - Bright lavender

### Mocha (Dark, High Contrast)

- **Base**: `#1e1e2e` - Deep dark background
- **Text**: `#cdd6f4` - Crisp white text
- **Primary**: `#89b4fa` - Electric blue
- **Accent**: `#b4befe` - Luminous lavender

## 🔄 Theme Switching

Themes can be changed at runtime through the UI:

1. The theme selector populates with all available flavors
2. Selection triggers `ThemeManager.SetTheme()`
3. Theme change event fires to all subscribers
4. All forms and controls are re-themed automatically
5. New theme preference is saved to registry

## 🎯 Design Philosophy

Following Catppuccin's core principles:

- **Colorful is better than colorless**: Rich, meaningful colors enhance UI distinction
- **Balance**: Not too dull, not too bright - suitable for various lighting conditions
- **Harmony**: Colors complement each other for a cohesive experience

### Subtle Border System

The enhanced border system follows Catppuccin's philosophy of balance and harmony:

- **Refined Visual Hierarchy**: Uses `Surface1` and `Surface2` colors for panel borders instead of high-contrast `Overlay0`
- **Contextual Adaptation**: Light themes use `Surface1` for dividers, dark themes use `Surface2` for optimal contrast
- **Intelligent Nesting**: Prevents border overlap by detecting parent-child panel relationships
- **Custom Rendering**: Group boxes and panels receive custom paint handlers for precise border control
- **Pixel-Perfect Borders**: Uses `DrawLine()` instead of `DrawRectangle()` to avoid Windows GDI+ rendering quirks
- **Semantic Color Mapping**: Three levels of border subtlety:
  - `SubtleBorder`: For buttons and interactive elements
  - `PanelBorder`: For container separation
  - `DividerColor`: For menu separators and dividers

#### Technical Note: Windows Border Rendering

Windows Forms' `DrawRectangle()` method has a classic quirk where the bottom and right edges are rendered one pixel outside the specified bounds. This is a legacy behavior from early Windows graphics APIs. Our implementation uses individual `DrawLine()` calls with carefully adjusted coordinates (`Height - 2` instead of `Height - 1`) to ensure perfectly even borders across all sides.

For GroupBox controls, we create a proper text gap in the top border by measuring the text dimensions and drawing the top border in two segments, ensuring the GroupBox label text appears correctly in front of the border rather than being obscured by it.

## 🔧 Customization

The theme system is designed for extensibility:

### Adding New Colors

```csharp
// Add to ThemeColors class
public Color NewSemanticColor { get; set; }

// Map in GetThemeColors method
NewSemanticColor = palette.DesiredCatppuccinColor
```

### Adding New Control Types

```csharp
// Add case to ApplyThemeToControl method
case NewControlType newControl:
    ApplyNewControlTheme(newControl);
    break;
```

### Custom Color Mappings

```csharp
// Override specific colors per flavor
var customTheme = CatppuccinTheme.GetThemeColors(flavor);
customTheme.Primary = palette.Pink; // Use pink instead of blue
```

## 📚 References

- [Official Catppuccin Website](https://catppuccin.com/)
- [Catppuccin Color Palette](https://catppuccin.com/palette/)
- [Catppuccin Style Guide](https://github.com/catppuccin/catppuccin/blob/main/docs/style-guide.md)
- [Catppuccin GitHub Organization](https://github.com/catppuccin)

## 🤝 Contributing

When contributing to the theme system:

1. Follow Catppuccin's official color values exactly
2. Maintain semantic color naming conventions
3. Test all four flavors for consistency
4. Ensure accessibility in light and dark themes
5. Document any new theme-related features

## 📄 License

This theme implementation follows the same MIT license as the main SeeThroughWindows project and respects Catppuccin's MIT license for the color palette.

---

_Made with 💜 by the SeeThroughWindows team, inspired by the beautiful Catppuccin color palette_
