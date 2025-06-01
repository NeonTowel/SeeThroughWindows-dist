using System.Drawing;

namespace SeeThroughWindows.Themes
{
    /// <summary>
    /// Catppuccin theme implementation with all four flavors
    /// Based on the official Catppuccin color palette: https://catppuccin.com/palette/
    /// </summary>
    public static class CatppuccinTheme
    {
        public enum Flavor
        {
            Latte,    // Light theme
            Frappe,   // Dark theme with subdued colors
            Macchiato, // Medium contrast dark theme
            Mocha     // Darkest theme (original)
        }

        public enum AccentColor
        {
            Lavender,  // Default - soft purple
            Blue,      // Classic blue
            Mauve,     // Rich purple
            Pink,      // Playful pink
            Teal,      // Calming teal
            Green,     // Natural green
            Peach,     // Warm orange
            Yellow,    // Bright yellow
            Red,       // Bold red
            Sky        // Light blue
        }

        public class ColorPalette
        {
            // Accent colors
            public Color Rosewater { get; set; }
            public Color Flamingo { get; set; }
            public Color Pink { get; set; }
            public Color Mauve { get; set; }
            public Color Red { get; set; }
            public Color Maroon { get; set; }
            public Color Peach { get; set; }
            public Color Yellow { get; set; }
            public Color Green { get; set; }
            public Color Teal { get; set; }
            public Color Sky { get; set; }
            public Color Sapphire { get; set; }
            public Color Blue { get; set; }
            public Color Lavender { get; set; }

            // Neutral colors
            public Color Text { get; set; }
            public Color Subtext1 { get; set; }
            public Color Subtext0 { get; set; }
            public Color Overlay2 { get; set; }
            public Color Overlay1 { get; set; }
            public Color Overlay0 { get; set; }
            public Color Surface2 { get; set; }
            public Color Surface1 { get; set; }
            public Color Surface0 { get; set; }
            public Color Base { get; set; }
            public Color Mantle { get; set; }
            public Color Crust { get; set; }
        }

        public class ThemeColors
        {
            public Color Background { get; set; }
            public Color Surface { get; set; }
            public Color Primary { get; set; }
            public Color Secondary { get; set; }
            public Color Accent { get; set; }
            public Color Text { get; set; }
            public Color TextSecondary { get; set; }
            public Color Border { get; set; }
            public Color ButtonBackground { get; set; }
            public Color ButtonText { get; set; }
            public Color ButtonHover { get; set; }
            public Color CheckboxBackground { get; set; }
            public Color CheckboxBorder { get; set; }
            public Color TrackBarThumb { get; set; }
            public Color TrackBarTrack { get; set; }
            public Color GroupBoxBorder { get; set; }
            public Color LinkColor { get; set; }
            public Color LinkHover { get; set; }
            // Subtle border colors for a more refined appearance
            public Color SubtleBorder { get; set; }
            public Color PanelBorder { get; set; }
            public Color DividerColor { get; set; }
        }

        private static readonly Dictionary<Flavor, ColorPalette> Palettes = new()
        {
            [Flavor.Latte] = new ColorPalette
            {
                // Latte (Light theme)
                Rosewater = ColorTranslator.FromHtml("#dc8a78"),
                Flamingo = ColorTranslator.FromHtml("#dd7878"),
                Pink = ColorTranslator.FromHtml("#ea76cb"),
                Mauve = ColorTranslator.FromHtml("#8839ef"),
                Red = ColorTranslator.FromHtml("#d20f39"),
                Maroon = ColorTranslator.FromHtml("#e64553"),
                Peach = ColorTranslator.FromHtml("#fe640b"),
                Yellow = ColorTranslator.FromHtml("#df8e1d"),
                Green = ColorTranslator.FromHtml("#40a02b"),
                Teal = ColorTranslator.FromHtml("#179299"),
                Sky = ColorTranslator.FromHtml("#04a5e5"),
                Sapphire = ColorTranslator.FromHtml("#209fb5"),
                Blue = ColorTranslator.FromHtml("#1e66f5"),
                Lavender = ColorTranslator.FromHtml("#7287fd"),
                Text = ColorTranslator.FromHtml("#4c4f69"),
                Subtext1 = ColorTranslator.FromHtml("#5c5f77"),
                Subtext0 = ColorTranslator.FromHtml("#6c6f85"),
                Overlay2 = ColorTranslator.FromHtml("#7c7f93"),
                Overlay1 = ColorTranslator.FromHtml("#8c8fa1"),
                Overlay0 = ColorTranslator.FromHtml("#9ca0b0"),
                Surface2 = ColorTranslator.FromHtml("#acb0be"),
                Surface1 = ColorTranslator.FromHtml("#bcc0cc"),
                Surface0 = ColorTranslator.FromHtml("#ccd0da"),
                Base = ColorTranslator.FromHtml("#eff1f5"),
                Mantle = ColorTranslator.FromHtml("#e6e9ef"),
                Crust = ColorTranslator.FromHtml("#dce0e8")
            },
            [Flavor.Frappe] = new ColorPalette
            {
                // Frappé (Dark theme with subdued colors)
                Rosewater = ColorTranslator.FromHtml("#f2d5cf"),
                Flamingo = ColorTranslator.FromHtml("#eebebe"),
                Pink = ColorTranslator.FromHtml("#f4b8e4"),
                Mauve = ColorTranslator.FromHtml("#ca9ee6"),
                Red = ColorTranslator.FromHtml("#e78284"),
                Maroon = ColorTranslator.FromHtml("#ea999c"),
                Peach = ColorTranslator.FromHtml("#ef9f76"),
                Yellow = ColorTranslator.FromHtml("#e5c890"),
                Green = ColorTranslator.FromHtml("#a6d189"),
                Teal = ColorTranslator.FromHtml("#81c8be"),
                Sky = ColorTranslator.FromHtml("#99d1db"),
                Sapphire = ColorTranslator.FromHtml("#85c1dc"),
                Blue = ColorTranslator.FromHtml("#8caaee"),
                Lavender = ColorTranslator.FromHtml("#babbf1"),
                Text = ColorTranslator.FromHtml("#c6d0f5"),
                Subtext1 = ColorTranslator.FromHtml("#b5bfe2"),
                Subtext0 = ColorTranslator.FromHtml("#a5adce"),
                Overlay2 = ColorTranslator.FromHtml("#949cbb"),
                Overlay1 = ColorTranslator.FromHtml("#838ba7"),
                Overlay0 = ColorTranslator.FromHtml("#737994"),
                Surface2 = ColorTranslator.FromHtml("#626880"),
                Surface1 = ColorTranslator.FromHtml("#51576d"),
                Surface0 = ColorTranslator.FromHtml("#414559"),
                Base = ColorTranslator.FromHtml("#303446"),
                Mantle = ColorTranslator.FromHtml("#292c3c"),
                Crust = ColorTranslator.FromHtml("#232634")
            },
            [Flavor.Macchiato] = new ColorPalette
            {
                // Macchiato (Medium contrast dark theme)
                Rosewater = ColorTranslator.FromHtml("#f4dbd6"),
                Flamingo = ColorTranslator.FromHtml("#f0c6c6"),
                Pink = ColorTranslator.FromHtml("#f5bde6"),
                Mauve = ColorTranslator.FromHtml("#c6a0f6"),
                Red = ColorTranslator.FromHtml("#ed8796"),
                Maroon = ColorTranslator.FromHtml("#ee99a0"),
                Peach = ColorTranslator.FromHtml("#f5a97f"),
                Yellow = ColorTranslator.FromHtml("#eed49f"),
                Green = ColorTranslator.FromHtml("#a6da95"),
                Teal = ColorTranslator.FromHtml("#8bd5ca"),
                Sky = ColorTranslator.FromHtml("#91d7e3"),
                Sapphire = ColorTranslator.FromHtml("#7dc4e4"),
                Blue = ColorTranslator.FromHtml("#8aadf4"),
                Lavender = ColorTranslator.FromHtml("#b7bdf8"),
                Text = ColorTranslator.FromHtml("#cad3f5"),
                Subtext1 = ColorTranslator.FromHtml("#b8c0e0"),
                Subtext0 = ColorTranslator.FromHtml("#a5adcb"),
                Overlay2 = ColorTranslator.FromHtml("#939ab7"),
                Overlay1 = ColorTranslator.FromHtml("#8087a2"),
                Overlay0 = ColorTranslator.FromHtml("#6e738d"),
                Surface2 = ColorTranslator.FromHtml("#5b6078"),
                Surface1 = ColorTranslator.FromHtml("#494d64"),
                Surface0 = ColorTranslator.FromHtml("#363a4f"),
                Base = ColorTranslator.FromHtml("#24273a"),
                Mantle = ColorTranslator.FromHtml("#1e2030"),
                Crust = ColorTranslator.FromHtml("#181926")
            },
            [Flavor.Mocha] = new ColorPalette
            {
                // Mocha (Darkest theme - original)
                Rosewater = ColorTranslator.FromHtml("#f5e0dc"),
                Flamingo = ColorTranslator.FromHtml("#f2cdcd"),
                Pink = ColorTranslator.FromHtml("#f5c2e7"),
                Mauve = ColorTranslator.FromHtml("#cba6f7"),
                Red = ColorTranslator.FromHtml("#f38ba8"),
                Maroon = ColorTranslator.FromHtml("#eba0ac"),
                Peach = ColorTranslator.FromHtml("#fab387"),
                Yellow = ColorTranslator.FromHtml("#f9e2af"),
                Green = ColorTranslator.FromHtml("#a6e3a1"),
                Teal = ColorTranslator.FromHtml("#94e2d5"),
                Sky = ColorTranslator.FromHtml("#89dceb"),
                Sapphire = ColorTranslator.FromHtml("#74c7ec"),
                Blue = ColorTranslator.FromHtml("#89b4fa"),
                Lavender = ColorTranslator.FromHtml("#b4befe"),
                Text = ColorTranslator.FromHtml("#cdd6f4"),
                Subtext1 = ColorTranslator.FromHtml("#bac2de"),
                Subtext0 = ColorTranslator.FromHtml("#a6adc8"),
                Overlay2 = ColorTranslator.FromHtml("#9399b2"),
                Overlay1 = ColorTranslator.FromHtml("#7f849c"),
                Overlay0 = ColorTranslator.FromHtml("#6c7086"),
                Surface2 = ColorTranslator.FromHtml("#585b70"),
                Surface1 = ColorTranslator.FromHtml("#45475a"),
                Surface0 = ColorTranslator.FromHtml("#313244"),
                Base = ColorTranslator.FromHtml("#1e1e2e"),
                Mantle = ColorTranslator.FromHtml("#181825"),
                Crust = ColorTranslator.FromHtml("#11111b")
            }
        };

        public static ColorPalette GetPalette(Flavor flavor)
        {
            return Palettes[flavor];
        }

        public static Color GetAccentColor(ColorPalette palette, AccentColor accentColor)
        {
            return accentColor switch
            {
                AccentColor.Lavender => palette.Lavender,
                AccentColor.Blue => palette.Blue,
                AccentColor.Mauve => palette.Mauve,
                AccentColor.Pink => palette.Pink,
                AccentColor.Teal => palette.Teal,
                AccentColor.Green => palette.Green,
                AccentColor.Peach => palette.Peach,
                AccentColor.Yellow => palette.Yellow,
                AccentColor.Red => palette.Red,
                AccentColor.Sky => palette.Sky,
                _ => palette.Lavender
            };
        }

        public static ThemeColors GetThemeColors(Flavor flavor, AccentColor accentColor = AccentColor.Lavender)
        {
            var palette = GetPalette(flavor);
            var selectedAccent = GetAccentColor(palette, accentColor);

            return new ThemeColors
            {
                Background = palette.Base,           // Background Pane -> Base
                Surface = palette.Surface0,          // Surface Elements -> Surface0
                Primary = palette.Blue,
                Secondary = palette.Mauve,
                Accent = selectedAccent,
                Text = palette.Text,                 // Body Copy -> Text
                TextSecondary = palette.Subtext1,    // Sub-Headlines, Labels -> Subtext1
                Border = palette.Overlay1,           // Overlays -> Overlay1 (more visible than Overlay0)
                ButtonBackground = palette.Surface1, // Surface Elements -> Surface1
                ButtonText = palette.Text,
                ButtonHover = palette.Surface2,      // Surface Elements -> Surface2
                CheckboxBackground = palette.Surface0,
                CheckboxBorder = palette.Overlay1,
                TrackBarThumb = selectedAccent,
                TrackBarTrack = palette.Surface1,
                GroupBoxBorder = palette.Overlay0,
                LinkColor = selectedAccent,          // Links should use accent color
                LinkHover = palette.Blue,            // Links, URLs -> Blue
                SubtleBorder = palette.Surface2,     // Subtle borders
                PanelBorder = palette.Surface1,      // Panel borders
                DividerColor = CatppuccinTheme.IsLightTheme(flavor) ? palette.Surface1 : palette.Surface2
            };
        }

        public static string GetFlavorDisplayName(Flavor flavor)
        {
            return flavor switch
            {
                Flavor.Latte => "🌻 Latte",
                Flavor.Frappe => "🪴 Frappé",
                Flavor.Macchiato => "🌺 Macchiato",
                Flavor.Mocha => "🌿 Mocha",
                _ => flavor.ToString()
            };
        }

        public static string GetFlavorDescription(Flavor flavor)
        {
            return flavor switch
            {
                Flavor.Latte => "Our lightest theme harmoniously inverting the essence of Catppuccin's dark themes",
                Flavor.Frappe => "A less vibrant alternative using subdued colors for a muted aesthetic",
                Flavor.Macchiato => "Medium contrast with gentle colors creating a soothing atmosphere",
                Flavor.Mocha => "The Original — Our darkest variant offering a cozy feeling with color-rich accents",
                _ => ""
            };
        }

        public static bool IsLightTheme(Flavor flavor)
        {
            return flavor == Flavor.Latte;
        }

        public static string GetAccentColorDisplayName(AccentColor accentColor)
        {
            return accentColor switch
            {
                AccentColor.Lavender => "💜 Lavender",
                AccentColor.Blue => "💙 Blue",
                AccentColor.Mauve => "🔮 Mauve",
                AccentColor.Pink => "💗 Pink",
                AccentColor.Teal => "🌊 Teal",
                AccentColor.Green => "💚 Green",
                AccentColor.Peach => "🍑 Peach",
                AccentColor.Yellow => "💛 Yellow",
                AccentColor.Red => "❤️ Red",
                AccentColor.Sky => "🩵 Sky",
                _ => accentColor.ToString()
            };
        }
    }
}
