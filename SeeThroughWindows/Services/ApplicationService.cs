using SeeThroughWindows.Models;
using SeeThroughWindows.Services;
using SeeThroughWindows.Themes;
using System.Diagnostics;
using System.IO;
using System.Text;

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
        /// Event fired when the hijacked window count changes (for UI updates)
        /// </summary>
        event EventHandler? HijackedWindowCountChanged;

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

        /// <summary>
        /// Apply transparency to all visible windows (for startup auto-apply)
        /// </summary>
        /// <returns>The number of windows that had transparency applied</returns>
        int ApplyTransparencyToAllVisibleWindows();
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
        private readonly IAutoApplyService _autoApplyService;
        private readonly Dictionary<IntPtr, WindowInfo> _hijackedWindows = new();

        private AppSettings _currentSettings;

        public event EventHandler<NotificationEventArgs>? NotificationRequested;
        public event EventHandler? HijackedWindowCountChanged;

        public ApplicationService(IWindowManager windowManager, ISettingsManager settingsManager, IAutoApplyService autoApplyService)
        {
            _windowManager = windowManager;
            _settingsManager = settingsManager;
            _autoApplyService = autoApplyService;
            _currentSettings = new AppSettings();
        }
        public void Initialize()
        {
            _currentSettings = _settingsManager.LoadSettings();

            Debug.WriteLine($"ApplicationService: AutoApplyOnStartup = {_currentSettings.AutoApplyOnStartup}");
            Console.WriteLine($"ApplicationService: AutoApplyOnStartup = {_currentSettings.AutoApplyOnStartup}");

            // Apply auto-transparency if enabled
            if (_currentSettings.AutoApplyOnStartup)
            {
                Debug.WriteLine("ApplicationService: Auto-apply is enabled, starting background task");
                Console.WriteLine("ApplicationService: Auto-apply is enabled, starting background task");

                Task.Run(() =>
                {
                    // Add a small delay to ensure all windows are fully loaded
                    Debug.WriteLine("ApplicationService: Waiting 2 seconds before applying transparency");
                    Console.WriteLine("ApplicationService: Waiting 2 seconds before applying transparency");
                    Thread.Sleep(2000);

                    try
                    {
                        Debug.WriteLine("ApplicationService: Starting auto-apply transparency");
                        Console.WriteLine("ApplicationService: Starting auto-apply transparency");
                        var appliedCount = ApplyTransparencyToAllVisibleWindows();

                        if (appliedCount > 0)
                        {
                            Debug.WriteLine($"ApplicationService: Auto-apply completed successfully, applied to {appliedCount} windows");
                            Console.WriteLine($"ApplicationService: Auto-apply completed successfully, applied to {appliedCount} windows");

                            // Notify UI that window count has changed
                            HijackedWindowCountChanged?.Invoke(this, EventArgs.Empty);

                            // Show notification
                            NotificationRequested?.Invoke(this, new NotificationEventArgs(
                                "See Through Windows",
                                $"Auto-applied transparency to {appliedCount} window{(appliedCount == 1 ? "" : "s")}",
                                ToolTipIcon.Info));
                        }
                        else
                        {
                            Debug.WriteLine("ApplicationService: Auto-apply completed, but no eligible windows found");
                            Console.WriteLine("ApplicationService: Auto-apply completed, but no eligible windows found");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ApplicationService: Auto-apply failed: {ex.Message}");
                        Console.WriteLine($"ApplicationService: Auto-apply failed: {ex.Message}");

                        NotificationRequested?.Invoke(this, new NotificationEventArgs(
                            "See Through Windows",
                            "Auto-apply transparency failed. Check debug log for details.",
                            ToolTipIcon.Error));
                    }
                });
            }
            else
            {
                Debug.WriteLine("ApplicationService: Auto-apply is disabled");
                Console.WriteLine("ApplicationService: Auto-apply is disabled");
            }
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

                // Notify UI that window count has changed
                HijackedWindowCountChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // Apply transparency
                _windowManager.ApplyTransparency(activeWindowHandle, windowInfo, newAlpha,
                    _currentSettings.ClickThrough, _currentSettings.TopMost);

                // Notify UI that window count has changed (if this was a new window)
                if (windowInfo.CurrentAlpha == OPAQUE)
                {
                    HijackedWindowCountChanged?.Invoke(this, EventArgs.Empty);
                }
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
            var activeWindowHandle = Win32Api.GetForegroundWindow();

            if (activeWindowHandle == IntPtr.Zero || !_hijackedWindows.ContainsKey(activeWindowHandle))
                return;

            var windowInfo = _hijackedWindows[activeWindowHandle];
            short newAlpha = (short)Math.Max(16, windowInfo.CurrentAlpha - 32);

            if (newAlpha == OPAQUE)
            {
                _windowManager.RestoreWindow(activeWindowHandle, windowInfo);
                _hijackedWindows.Remove(activeWindowHandle);

                // Notify UI that window count has changed
                HijackedWindowCountChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _windowManager.ApplyTransparency(activeWindowHandle, windowInfo, newAlpha,
                    _currentSettings.ClickThrough, _currentSettings.TopMost);
            }
        }

        public void HandleLessTransparentHotkeyPress()
        {
            var activeWindowHandle = Win32Api.GetForegroundWindow();

            if (activeWindowHandle == IntPtr.Zero || !_hijackedWindows.ContainsKey(activeWindowHandle))
                return;

            var windowInfo = _hijackedWindows[activeWindowHandle];
            short newAlpha = (short)Math.Min(255, windowInfo.CurrentAlpha + 32);

            if (newAlpha == OPAQUE)
            {
                _windowManager.RestoreWindow(activeWindowHandle, windowInfo);
                _hijackedWindows.Remove(activeWindowHandle);

                // Notify UI that window count has changed
                HijackedWindowCountChanged?.Invoke(this, EventArgs.Empty);
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
                try
                {
                    var windowInfo = _hijackedWindows[handle];

                    // Check if window still exists before trying to restore it
                    if (Win32Api.IsWindow(handle))
                    {
                        _windowManager.RestoreWindow(handle, windowInfo);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but continue with other windows
                    Debug.WriteLine($"ApplicationService: Failed to restore window {handle}: {ex.Message}");
                }
            }

            // Clear all tracked windows
            _hijackedWindows.Clear();

            // Notify UI that window count has changed
            HijackedWindowCountChanged?.Invoke(this, EventArgs.Empty);
        }
        public int ApplyTransparencyToAllVisibleWindows()
        {
            // Debug: Write basic window information to a log file for troubleshooting
            try
            {
                var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SeeThroughWindows_Debug.txt");
                using (var writer = new StreamWriter(logPath, false))
                {
                    writer.WriteLine($"=== SeeThroughWindows Auto-Apply Debug Log ===");
                    writer.WriteLine($"Timestamp: {DateTime.Now}");
                    writer.WriteLine($"Settings - SemiTransparentValue: {_currentSettings.SemiTransparentValue}");
                    writer.WriteLine($"Settings - ClickThrough: {_currentSettings.ClickThrough}");
                    writer.WriteLine($"Settings - TopMost: {_currentSettings.TopMost}");
                    writer.WriteLine($"Settings - AutoApplyOnStartup: {_currentSettings.AutoApplyOnStartup}");
                    writer.WriteLine();

                    // Get eligible windows
                    var eligibleWindows = _autoApplyService.GetEligibleWindows();
                    writer.WriteLine($"Eligible windows found: {eligibleWindows.Count}");
                    writer.WriteLine();

                    if (eligibleWindows.Any())
                    {
                        writer.WriteLine("=== ELIGIBLE WINDOWS ===");
                        foreach (var (handle, title) in eligibleWindows)
                        {
                            writer.WriteLine($"Handle: {handle}, Title: '{title}'");
                        }
                    }
                    else
                    {
                        writer.WriteLine("No eligible windows found for auto-apply transparency.");
                    }

                    writer.WriteLine();
                    writer.WriteLine("=== END DEBUG LOG ===");
                }

                Debug.WriteLine($"ApplicationService: Debug log written to {logPath}");
                Console.WriteLine($"ApplicationService: Debug log written to {logPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ApplicationService: Failed to write debug log: {ex.Message}");
                Console.WriteLine($"ApplicationService: Failed to write debug log: {ex.Message}");
            }

            var appliedCount = _autoApplyService.ApplyTransparencyToVisibleWindows(
                _currentSettings.SemiTransparentValue,
                _currentSettings.ClickThrough,
                _currentSettings.TopMost);

            // Get the windows that were actually processed and add them to our tracking
            var processedWindows = _autoApplyService.GetEligibleWindows();
            int trackedCount = 0;

            foreach (var (handle, title) in processedWindows)
            {
                if (!_hijackedWindows.ContainsKey(handle))
                {
                    try
                    {
                        // Create a basic window info for tracking
                        // The transparency was already applied by the AutoApplyService
                        var originalStyle = Win32Api.GetWindowLong(handle, Win32Api.GWL_EX_STYLE);
                        var windowInfo = new WindowInfo(originalStyle, 255) // Assume it was originally opaque
                        {
                            CurrentAlpha = _currentSettings.SemiTransparentValue
                        };
                        _hijackedWindows[handle] = windowInfo;
                        trackedCount++;
                        Debug.WriteLine($"ApplicationService: Added window '{title}' to tracking");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ApplicationService: Failed to track window {title}: {ex.Message}");
                    }
                }
            }

            // Notify UI that window count has changed if we tracked any windows
            if (trackedCount > 0)
            {
                HijackedWindowCountChanged?.Invoke(this, EventArgs.Empty);
            }

            return appliedCount;
        }


    }
}
