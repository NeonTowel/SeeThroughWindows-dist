using SeeThroughWindows.Models;
using SeeThroughWindows.Services;
using SeeThroughWindows.Themes;

namespace SeeThroughWindows.Services
{
    /// <summary>
    /// Main application service that coordinates window transparency operations
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// Event fired when a window transparency operation should show a notification
        /// </summary>
        event EventHandler<NotificationEventArgs>? NotificationRequested;

        /// <summary>
        /// Initialize the application service
        /// </summary>
        void Initialize();

        /// <summary>
        /// Handle the user hotkey press (toggle transparency)
        /// </summary>
        void HandleUserHotkeyPress();

        /// <summary>
        /// Handle maximize hotkey press
        /// </summary>
        void HandleMaximizeHotkeyPress();

        /// <summary>
        /// Handle minimize hotkey press
        /// </summary>
        void HandleMinimizeHotkeyPress();

        /// <summary>
        /// Handle previous screen hotkey press
        /// </summary>
        void HandlePreviousScreenHotkeyPress();

        /// <summary>
        /// Handle next screen hotkey press
        /// </summary>
        void HandleNextScreenHotkeyPress();

        /// <summary>
        /// Handle more transparent hotkey press
        /// </summary>
        void HandleMoreTransparentHotkeyPress();

        /// <summary>
        /// Handle less transparent hotkey press
        /// </summary>
        void HandleLessTransparentHotkeyPress();

        /// <summary>
        /// Get current transparency settings
        /// </summary>
        AppSettings GetCurrentSettings();

        /// <summary>
        /// Update transparency settings
        /// </summary>
        void UpdateSettings(AppSettings settings);

        /// <summary>
        /// Get all currently transparent windows
        /// </summary>
        IReadOnlyDictionary<IntPtr, WindowInfo> GetTransparentWindows();

        /// <summary>
        /// Get the count of currently hijacked windows
        /// </summary>
        int GetHijackedWindowCount();

        /// <summary>
        /// Restore all windows to their original state
        /// </summary>
        void RestoreAllWindows();
    }

    /// <summary>
    /// Notification event arguments
    /// </summary>
    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; }
        public string Message { get; }
        public ToolTipIcon Icon { get; }

        public NotificationEventArgs(string title, string message, ToolTipIcon icon = ToolTipIcon.Info)
        {
            Title = title;
            Message = message;
            Icon = icon;
        }
    }

    /// <summary>
    /// Implementation of the main application service
    /// </summary>
    public class ApplicationService : IApplicationService
    {
        private const short OPAQUE = 255;
        private const short DEFAULT_SEMITRANSPARENT = 64;

        private readonly IWindowManager _windowManager;
        private readonly ISettingsManager _settingsManager;
        private readonly Dictionary<IntPtr, WindowInfo> _hijackedWindows = new();

        private AppSettings _currentSettings;

        public event EventHandler<NotificationEventArgs>? NotificationRequested;

        public ApplicationService(IWindowManager windowManager, ISettingsManager settingsManager)
        {
            _windowManager = windowManager;
            _settingsManager = settingsManager;
            _currentSettings = new AppSettings();
        }

        public void Initialize()
        {
            _currentSettings = _settingsManager.LoadSettings();
        }

        public void HandleUserHotkeyPress()
        {
            var activeWindowHandle = Win32Api.GetForegroundWindow();

            if (activeWindowHandle == IntPtr.Zero)
                return;

            if (_windowManager.IsDesktopWindow(activeWindowHandle))
            {
                NotificationRequested?.Invoke(this, new NotificationEventArgs(
                    "See Through Windows",
                    "Cannot make desktop transparent",
                    ToolTipIcon.Warning));
                return;
            }

            // Get or create window info
            WindowInfo windowInfo;
            if (_hijackedWindows.ContainsKey(activeWindowHandle))
            {
                windowInfo = _hijackedWindows[activeWindowHandle];
            }
            else
            {
                windowInfo = _windowManager.HijackWindow(activeWindowHandle);
                _hijackedWindows[activeWindowHandle] = windowInfo;
            }

            // Toggle transparency
            short newAlpha;
            if (windowInfo.CurrentAlpha == OPAQUE)
            {
                // Make transparent
                newAlpha = _currentSettings.SemiTransparentValue;
            }
            else
            {
                // Make opaque
                newAlpha = OPAQUE;
            }

            if (newAlpha == OPAQUE)
            {
                // Restore window and remove from tracking
                _windowManager.RestoreWindow(activeWindowHandle, windowInfo);
                _hijackedWindows.Remove(activeWindowHandle);
            }
            else
            {
                // Apply transparency
                _windowManager.ApplyTransparency(activeWindowHandle, windowInfo, newAlpha,
                    _currentSettings.ClickThrough, _currentSettings.TopMost);
            }
        }

        public void HandleMaximizeHotkeyPress()
        {
            var activeWindowHandle = Win32Api.GetForegroundWindow();
            if (activeWindowHandle != IntPtr.Zero)
            {
                _windowManager.MaximizeWindow(activeWindowHandle);
            }
        }

        public void HandleMinimizeHotkeyPress()
        {
            var activeWindowHandle = Win32Api.GetForegroundWindow();
            if (activeWindowHandle != IntPtr.Zero)
            {
                _windowManager.MinimizeWindow(activeWindowHandle);
            }
        }

        public void HandlePreviousScreenHotkeyPress()
        {
            var activeWindowHandle = Win32Api.GetForegroundWindow();
            if (activeWindowHandle != IntPtr.Zero)
            {
                _windowManager.MoveToPreviousMonitor(activeWindowHandle);
            }
        }

        public void HandleNextScreenHotkeyPress()
        {
            var activeWindowHandle = Win32Api.GetForegroundWindow();
            if (activeWindowHandle != IntPtr.Zero)
            {
                _windowManager.MoveToNextMonitor(activeWindowHandle);
            }
        }

        public void HandleMoreTransparentHotkeyPress()
        {
            ChangeActiveWindowTransparency(-20);
        }

        public void HandleLessTransparentHotkeyPress()
        {
            ChangeActiveWindowTransparency(20);
        }

        private void ChangeActiveWindowTransparency(short delta)
        {
            var activeWindowHandle = Win32Api.GetForegroundWindow();
            if (activeWindowHandle == IntPtr.Zero || !_hijackedWindows.ContainsKey(activeWindowHandle))
                return;

            var windowInfo = _hijackedWindows[activeWindowHandle];
            short newAlpha = (short)Math.Max(20, Math.Min(255, windowInfo.CurrentAlpha + delta));

            if (newAlpha == OPAQUE)
            {
                _windowManager.RestoreWindow(activeWindowHandle, windowInfo);
                _hijackedWindows.Remove(activeWindowHandle);
            }
            else
            {
                _windowManager.ApplyTransparency(activeWindowHandle, windowInfo, newAlpha,
                    _currentSettings.ClickThrough, _currentSettings.TopMost);
            }
        }

        public AppSettings GetCurrentSettings()
        {
            return _currentSettings;
        }

        public void UpdateSettings(AppSettings settings)
        {
            _currentSettings = settings;
            _settingsManager.SaveSettings(settings);

            // Apply theme changes
            ThemeManager.SetTheme(settings.ThemeFlavor, settings.AccentColor);
        }

        public IReadOnlyDictionary<IntPtr, WindowInfo> GetTransparentWindows()
        {
            return _hijackedWindows.AsReadOnly();
        }

        public int GetHijackedWindowCount()
        {
            return _hijackedWindows.Count;
        }

        public void RestoreAllWindows()
        {
            var windowHandles = _hijackedWindows.Keys.ToList();
            foreach (var handle in windowHandles)
            {
                var windowInfo = _hijackedWindows[handle];
                _windowManager.RestoreWindow(handle, windowInfo);
            }
            _hijackedWindows.Clear();
        }
    }
}
