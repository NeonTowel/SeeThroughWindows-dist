using SeeThroughWindows.Models;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SeeThroughWindows.Services
{
    /// <summary>
    /// Service responsible for automatically applying transparency to visible windows on startup
    /// </summary>
    public interface IAutoApplyService
    {
        /// <summary>
        /// Apply transparency to all eligible visible windows
        /// </summary>
        /// <param name="transparencyValue">The transparency value to apply (0-255)</param>
        /// <param name="clickThrough">Whether to enable click-through</param>
        /// <param name="topMost">Whether to make windows top-most</param>
        /// <returns>The number of windows that had transparency applied</returns>
        int ApplyTransparencyToVisibleWindows(short transparencyValue, bool clickThrough, bool topMost);

        /// <summary>
        /// Get a list of all eligible windows for transparency application
        /// </summary>
        /// <returns>List of window handles and their titles</returns>
        List<(IntPtr Handle, string Title)> GetEligibleWindows();

        /// <summary>
        /// Get a list of all visible windows for debugging purposes
        /// </summary>
        /// <returns>List of all visible windows with their titles and process names</returns>
        List<(IntPtr Handle, string Title, string ProcessName)> GetAllVisibleWindows();
    }

    /// <summary>
    /// Implementation of the auto-apply transparency service
    /// </summary>
    public class AutoApplyService : IAutoApplyService
    {
        private readonly IWindowManager _windowManager;
        private readonly List<string> _excludedProcessNames;
        private readonly List<string> _excludedWindowTitles;
        private readonly List<string> _criticalProcessNames;
        private readonly List<string> _criticalWindowTitles;

        public AutoApplyService(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            // Initialize lists of excluded processes and window titles
            _excludedProcessNames = new List<string>
            {
                "explorer",      // Windows Explorer
                "dwm",          // Desktop Window Manager
                "winlogon",     // Windows Logon
                "csrss",        // Client/Server Runtime Subsystem
                "seethroughwindows", // Our own application
                "taskmgr",      // Task Manager
                "yasb",         // Yasb
            };

            _excludedWindowTitles = new List<string>
            {
                "Program Manager",
                "Desktop",
                "",  // Empty titles - this should now be handled correctly
                "Microsoft Text Input Application",
                "Windows Security"
            };

            // Initialize critical system components that should never be made transparent
            _criticalProcessNames = new List<string>
            {
                "dwm",
                "winlogon",
                "csrss",
                "lsass"
            };

            _criticalWindowTitles = new List<string>
            {
                "User Account Control",
                "Windows Security"
            };

            // Debug: Log the exclusion lists
            Debug.WriteLine($"AutoApplyService: Excluded processes: {string.Join(", ", _excludedProcessNames)}");
            Debug.WriteLine($"AutoApplyService: Excluded window titles: {string.Join(", ", _excludedWindowTitles.Where(t => !string.IsNullOrEmpty(t)))} + [empty titles]");
        }

        /// <summary>
        /// Enhanced visibility check that ensures only actually visible windows are considered
        /// </summary>
        /// <param name="hWnd">Window handle to check</param>
        /// <returns>True if the window is actually visible and should be considered for transparency</returns>
        private bool IsWindowActuallyVisible(IntPtr hWnd)
        {
            try
            {
                // Basic visibility check
                if (!Win32Api.IsWindowVisible(hWnd))
                    return false;

                // Skip minimized windows
                if (Win32Api.IsIconic(hWnd))
                    return false;

                // Skip desktop and shell windows
                if (hWnd == Win32Api.GetDesktopWindow() || hWnd == Win32Api.GetShellWindow())
                    return false;

                // Check if window has actual screen real estate
                if (!Win32Api.GetWindowRect(hWnd, out Win32Api.RECT rect))
                    return false;

                if (rect.Right <= rect.Left || rect.Bottom <= rect.Top)
                    return false;

                // Check if window is actually on screen (not moved off-screen)
                var screenBounds = Screen.AllScreens.Select(s => s.Bounds).ToArray();
                bool isOnScreen = screenBounds.Any(screen =>
                    rect.Left < screen.Right && rect.Right > screen.Left &&
                    rect.Top < screen.Bottom && rect.Bottom > screen.Top);

                return isOnScreen;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AutoApplyService: Error checking window visibility for {hWnd}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Check if window is in the foreground layer and properly displayed
        /// </summary>
        /// <param name="hWnd">Window handle to check</param>
        /// <returns>True if the window is in a normal display state</returns>
        private bool IsWindowInForegroundLayer(IntPtr hWnd)
        {
            try
            {
                var placement = new Win32Api.WINDOWPLACEMENT();
                placement.length = (uint)Marshal.SizeOf(placement);

                if (!Win32Api.GetWindowPlacement(hWnd, ref placement))
                    return false;

                // SW_SHOWNORMAL = 1, SW_SHOWMAXIMIZED = 3
                return placement.showCmd == Win32Api.SW_SHOWNORMAL || placement.showCmd == Win32Api.SW_SHOWMAXIMIZED;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AutoApplyService: Error checking window placement for {hWnd}: {ex.Message}");
                return true; // Default to true if we can't determine placement
            }
        }

        /// <summary>
        /// Check if a window is critical system component that should never be made transparent
        /// </summary>
        /// <param name="processName">Process name</param>
        /// <param name="windowTitle">Window title</param>
        /// <returns>True if the window is critical and should be excluded</returns>
        private bool IsCriticalWindow(string processName, string windowTitle)
        {
            return _criticalProcessNames.Contains(processName.ToLower()) ||
                   _criticalWindowTitles.Any(title => windowTitle.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        public int ApplyTransparencyToVisibleWindows(short transparencyValue, bool clickThrough, bool topMost)
        {
            var eligibleWindows = GetEligibleWindows();
            int appliedCount = 0;

            Debug.WriteLine($"AutoApplyService: Found {eligibleWindows.Count} eligible windows");

            foreach (var (handle, title) in eligibleWindows)
            {
                try
                {
                    Debug.WriteLine($"AutoApplyService: Processing window '{title}' (Handle: {handle})");

                    // Double-check visibility before applying (window state might have changed)
                    if (!IsWindowActuallyVisible(handle))
                    {
                        Debug.WriteLine($"AutoApplyService: Window '{title}' is no longer actually visible, skipping");
                        continue;
                    }

                    // Skip if it's a desktop window
                    if (_windowManager.IsDesktopWindow(handle))
                    {
                        Debug.WriteLine($"AutoApplyService: Window '{title}' is desktop window, skipping");
                        continue;
                    }

                    // Create window info and apply transparency
                    var windowInfo = _windowManager.HijackWindow(handle);
                    _windowManager.ApplyTransparency(handle, windowInfo, transparencyValue, clickThrough, topMost);

                    Debug.WriteLine($"AutoApplyService: Successfully applied transparency to '{title}'");
                    appliedCount++;
                }
                catch (Exception ex)
                {
                    // Log the error but continue with other windows
                    Debug.WriteLine($"AutoApplyService: Failed to apply transparency to window '{title}' (Handle: {handle}): {ex.Message}");
                }
            }

            Debug.WriteLine($"AutoApplyService: Applied transparency to {appliedCount} windows");
            return appliedCount;
        }

        public List<(IntPtr Handle, string Title)> GetEligibleWindows()
        {
            var windows = new List<(IntPtr Handle, string Title)>();
            var allWindows = new List<(IntPtr Handle, string Title, string Reason)>();

            Debug.WriteLine("AutoApplyService: Starting enhanced window enumeration for auto-apply");

            Win32Api.EnumWindows((hWnd, lParam) =>
            {
                try
                {
                    // Get window title and process name first for logging
                    var title = GetWindowTitle(hWnd);
                    var processName = GetProcessName(hWnd);

                    Debug.WriteLine($"AutoApplyService: Examining window '{title}' (Process: {processName}, Handle: {hWnd})");

                    // Enhanced visibility check - only process actually visible windows
                    if (!IsWindowActuallyVisible(hWnd))
                    {
                        allWindows.Add((hWnd, title, "Not actually visible"));
                        Debug.WriteLine($"AutoApplyService: Window '{title}' is not actually visible, skipping");
                        return true;
                    }

                    // Check if window is in proper foreground layer
                    if (!IsWindowInForegroundLayer(hWnd))
                    {
                        allWindows.Add((hWnd, title, "Not in foreground layer"));
                        Debug.WriteLine($"AutoApplyService: Window '{title}' is not in foreground layer, skipping");
                        return true;
                    }

                    // Check for critical system windows first (highest priority exclusion)
                    if (IsCriticalWindow(processName, title))
                    {
                        allWindows.Add((hWnd, title, "Critical system window"));
                        Debug.WriteLine($"AutoApplyService: Window '{title}' is critical system window, skipping");
                        return true;
                    }

                    // Skip windows with excluded titles
                    if (IsExcludedTitle(title))
                    {
                        allWindows.Add((hWnd, title, "Excluded title"));
                        Debug.WriteLine($"AutoApplyService: Window '{title}' has excluded title, skipping");
                        return true;
                    }

                    // Skip excluded processes
                    if (IsExcludedProcess(processName))
                    {
                        allWindows.Add((hWnd, title, $"Excluded process: {processName}"));
                        Debug.WriteLine($"AutoApplyService: Window '{title}' has excluded process '{processName}', skipping");
                        return true;
                    }

                    // Skip desktop and shell windows
                    if (_windowManager.IsDesktopWindow(hWnd))
                    {
                        allWindows.Add((hWnd, title, "Desktop window"));
                        Debug.WriteLine($"AutoApplyService: Window '{title}' is desktop window, skipping");
                        return true;
                    }

                    // Skip windows that are already layered (might already have transparency)
                    var exStyle = Win32Api.GetWindowLong(hWnd, Win32Api.GWL_EX_STYLE);
                    if ((exStyle & Win32Api.WS_EX_LAYERED) != 0)
                    {
                        // Check if it already has transparency applied
                        if (Win32Api.GetLayeredWindowAttributes(hWnd, out _, out short alpha, out _) && alpha < 255)
                        {
                            allWindows.Add((hWnd, title, $"Already transparent (alpha: {alpha})"));
                            Debug.WriteLine($"AutoApplyService: Window '{title}' already has transparency (alpha: {alpha}), skipping");
                            return true;
                        }
                        else
                        {
                            Debug.WriteLine($"AutoApplyService: Window '{title}' is layered but opaque (alpha: {alpha}), including");
                        }
                    }

                    // This window passed all filters
                    allWindows.Add((hWnd, title, "ELIGIBLE"));
                    windows.Add((hWnd, title));
                    Debug.WriteLine($"AutoApplyService: Window '{title}' is ELIGIBLE for transparency");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"AutoApplyService: Error examining window {hWnd}: {ex.Message}");
                }

                return true;
            }, IntPtr.Zero);

            // Summary logging
            Debug.WriteLine($"AutoApplyService: Enhanced window enumeration complete. Found {windows.Count} eligible windows out of {allWindows.Count} total windows examined");

            // Log summary of exclusion reasons
            var exclusionSummary = allWindows
                .Where(w => w.Reason != "ELIGIBLE")
                .GroupBy(w => w.Reason)
                .Select(g => $"{g.Key}: {g.Count()}")
                .ToList();

            if (exclusionSummary.Any())
            {
                Debug.WriteLine($"AutoApplyService: Exclusion summary: {string.Join(", ", exclusionSummary)}");
            }

            return windows;
        }

        private string GetWindowTitle(IntPtr hWnd)
        {
            try
            {
                var length = Win32Api.GetWindowTextLength(hWnd);
                if (length == 0)
                    return string.Empty;

                var sb = new StringBuilder(length + 1);
                Win32Api.GetWindowText(hWnd, sb, sb.Capacity);
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetProcessName(IntPtr hWnd)
        {
            try
            {
                Win32Api.GetWindowThreadProcessId(hWnd, out uint processId);
                using var process = Process.GetProcessById((int)processId);
                return process.ProcessName.ToLowerInvariant();
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool IsExcludedProcess(string processName)
        {
            return _excludedProcessNames.Any(excluded =>
                processName.Contains(excluded, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsExcludedTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return true;

            return _excludedWindowTitles.Any(excluded =>
                {
                    // Skip empty excluded titles to avoid the bug where Contains("") always returns true
                    if (string.IsNullOrEmpty(excluded))
                        return false;

                    return title.Equals(excluded, StringComparison.OrdinalIgnoreCase) ||
                           title.Contains(excluded, StringComparison.OrdinalIgnoreCase);
                });
        }

        /// <summary>
        /// Get a list of all visible windows for debugging purposes
        /// </summary>
        /// <returns>List of all visible windows with their titles and process names</returns>
        public List<(IntPtr Handle, string Title, string ProcessName)> GetAllVisibleWindows()
        {
            var windows = new List<(IntPtr Handle, string Title, string ProcessName)>();

            Debug.WriteLine("AutoApplyService: Starting enhanced window enumeration for all actually visible windows (no filtering)");

            Win32Api.EnumWindows((hWnd, lParam) =>
            {
                try
                {
                    // Use enhanced visibility check
                    if (!IsWindowActuallyVisible(hWnd))
                        return true;

                    var title = GetWindowTitle(hWnd);
                    var processName = GetProcessName(hWnd);

                    windows.Add((hWnd, title, processName));
                    Debug.WriteLine($"AutoApplyService: Found actually visible window '{title}' (Process: {processName}, Handle: {hWnd})");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"AutoApplyService: Error examining window {hWnd}: {ex.Message}");
                }

                return true;
            }, IntPtr.Zero);

            Debug.WriteLine($"AutoApplyService: Found {windows.Count} total actually visible windows");

            return windows;
        }
    }
}
