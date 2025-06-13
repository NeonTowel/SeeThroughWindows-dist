using Microsoft.Win32;
using SeeThroughWindows.Core;
using SeeThroughWindows.Themes;

namespace SeeThroughWindows.Services
{
    /// <summary>
    /// Manages application settings persistence using the Windows Registry
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        /// Load all settings from the registry
        /// </summary>
        AppSettings LoadSettings();

        /// <summary>
        /// Save all settings to the registry
        /// </summary>
        void SaveSettings(AppSettings settings);

        /// <summary>
        /// Save hotkey settings to the registry
        /// </summary>
        void SaveHotkeySettings(Hotkey hotkey);

        /// <summary>
        /// Save theme settings to the registry
        /// </summary>
        void SaveThemeSettings(CatppuccinTheme.Flavor flavor, CatppuccinTheme.AccentColor accentColor);
    }

    /// <summary>
    /// Implementation of settings management using Windows Registry
    /// </summary>
    public class RegistrySettingsManager : ISettingsManager
    {
        private const string REGROOT = "Software\\MOBZystems\\MOBZXRay\\";
        private const short DEFAULT_SEMITRANSPARENT = 64;

        public AppSettings LoadSettings()
        {
            try
            {
                using var root = Registry.CurrentUser.OpenSubKey(REGROOT);
                if (root == null)
                {
                    return GetDefaultSettings();
                }

                var settings = new AppSettings();

                // Load transparency
                if (short.TryParse(root.GetValue("Transparency", DEFAULT_SEMITRANSPARENT.ToString())?.ToString(), out short transparency))
                {
                    settings.SemiTransparentValue = transparency;
                }

                // Load checkboxes
                settings.ClickThrough = BoolFromString(root.GetValue("ClickThrough", "0")?.ToString() ?? "0");
                settings.TopMost = BoolFromString(root.GetValue("TopMost", "0")?.ToString() ?? "0");

                // Load hotkey settings
                var hotkeyStr = root.GetValue("Hotkey", "Z")?.ToString() ?? "Z";
                var hotkey = Hotkey.KeyCodeFromString(hotkeyStr);
                var shift = BoolFromString(root.GetValue("Shift", "1")?.ToString() ?? "1");
                var control = BoolFromString(root.GetValue("Control", "1")?.ToString() ?? "1");
                var alt = BoolFromString(root.GetValue("Alt", "1")?.ToString() ?? "1");
                var windows = BoolFromString(root.GetValue("Windows", "0")?.ToString() ?? "0");

                settings.HotkeySettings = new HotkeySettings
                {
                    KeyCode = hotkey,
                    Shift = shift,
                    Control = control,
                    Alt = alt,
                    Windows = windows
                };

                // Load feature enable checkboxes
                settings.EnableLeftRight = BoolFromString(root.GetValue("EnableLeftRight", "1")?.ToString() ?? "1");
                settings.EnableUpDown = BoolFromString(root.GetValue("EnableUpDown", "1")?.ToString() ?? "1");
                settings.EnablePageUpDown = BoolFromString(root.GetValue("EnablePageUpDown", "1")?.ToString() ?? "1");

                // Load theme settings
                var themeStr = root.GetValue("Theme", "Mocha")?.ToString() ?? "Mocha";
                if (Enum.TryParse<CatppuccinTheme.Flavor>(themeStr, out var flavor))
                {
                    settings.ThemeFlavor = flavor;
                }

                var accentStr = root.GetValue("AccentColor", "Lavender")?.ToString() ?? "Lavender";
                if (Enum.TryParse<CatppuccinTheme.AccentColor>(accentStr, out var accent))
                {
                    settings.AccentColor = accent;
                }

                return settings;
            }
            catch
            {
                return GetDefaultSettings();
            }
        }

        public void SaveSettings(AppSettings settings)
        {
            try
            {
                using var root = Registry.CurrentUser.CreateSubKey(REGROOT);
                if (root == null) return;

                root.SetValue("Transparency", settings.SemiTransparentValue.ToString());
                root.SetValue("ClickThrough", BoolToString(settings.ClickThrough));
                root.SetValue("TopMost", BoolToString(settings.TopMost));

                SaveHotkeySettingsInternal(root, settings.HotkeySettings);

                root.SetValue("EnableLeftRight", BoolToString(settings.EnableLeftRight));
                root.SetValue("EnableUpDown", BoolToString(settings.EnableUpDown));
                root.SetValue("EnablePageUpDown", BoolToString(settings.EnablePageUpDown));

                root.SetValue("Theme", settings.ThemeFlavor.ToString());
                root.SetValue("AccentColor", settings.AccentColor.ToString());
            }
            catch
            {
                // Ignore errors when saving settings
            }
        }

        public void SaveHotkeySettings(Hotkey hotkey)
        {
            try
            {
                using var root = Registry.CurrentUser.CreateSubKey(REGROOT);
                if (root == null) return;

                var hotkeySettings = new HotkeySettings
                {
                    KeyCode = hotkey.KeyCode,
                    Shift = hotkey.Shift,
                    Control = hotkey.Control,
                    Alt = hotkey.Alt,
                    Windows = hotkey.Windows
                };

                SaveHotkeySettingsInternal(root, hotkeySettings);
            }
            catch
            {
                // Ignore errors when saving settings
            }
        }

        public void SaveThemeSettings(CatppuccinTheme.Flavor flavor, CatppuccinTheme.AccentColor accentColor)
        {
            try
            {
                using var root = Registry.CurrentUser.CreateSubKey(REGROOT);
                if (root == null) return;

                root.SetValue("Theme", flavor.ToString());
                root.SetValue("AccentColor", accentColor.ToString());
            }
            catch
            {
                // Ignore errors when saving settings
            }
        }

        private void SaveHotkeySettingsInternal(RegistryKey root, HotkeySettings hotkeySettings)
        {
            root.SetValue("Hotkey", Hotkey.KeyCodeToString(hotkeySettings.KeyCode));
            root.SetValue("Shift", BoolToString(hotkeySettings.Shift));
            root.SetValue("Control", BoolToString(hotkeySettings.Control));
            root.SetValue("Alt", BoolToString(hotkeySettings.Alt));
            root.SetValue("Windows", BoolToString(hotkeySettings.Windows));
        }

        private AppSettings GetDefaultSettings()
        {
            return new AppSettings
            {
                SemiTransparentValue = DEFAULT_SEMITRANSPARENT,
                ClickThrough = false,
                TopMost = false,
                HotkeySettings = new HotkeySettings
                {
                    KeyCode = Keys.Z,
                    Shift = true,
                    Control = true,
                    Alt = true,
                    Windows = false
                },
                EnableLeftRight = true,
                EnableUpDown = true,
                EnablePageUpDown = true,
                ThemeFlavor = CatppuccinTheme.Flavor.Mocha,
                AccentColor = CatppuccinTheme.AccentColor.Lavender
            };
        }

        private string BoolToString(bool b) => b ? "1" : "0";

        private bool BoolFromString(string s) => s == "1";
    }

    /// <summary>
    /// Application settings data model
    /// </summary>
    public class AppSettings
    {
        public short SemiTransparentValue { get; set; } = 64;
        public bool ClickThrough { get; set; }
        public bool TopMost { get; set; }
        public HotkeySettings HotkeySettings { get; set; } = new();
        public bool EnableLeftRight { get; set; } = true;
        public bool EnableUpDown { get; set; } = true;
        public bool EnablePageUpDown { get; set; } = true;
        public CatppuccinTheme.Flavor ThemeFlavor { get; set; } = CatppuccinTheme.Flavor.Mocha;
        public CatppuccinTheme.AccentColor AccentColor { get; set; } = CatppuccinTheme.AccentColor.Lavender;
    }

    /// <summary>
    /// Hotkey configuration settings
    /// </summary>
    public class HotkeySettings
    {
        public Keys KeyCode { get; set; } = Keys.Z;
        public bool Shift { get; set; } = true;
        public bool Control { get; set; } = true;
        public bool Alt { get; set; } = true;
        public bool Windows { get; set; } = false;
    }
}
