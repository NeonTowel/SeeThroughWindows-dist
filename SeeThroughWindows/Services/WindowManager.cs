using SeeThroughWindows.Models;

namespace SeeThroughWindows.Services
{
    /// <summary>
    /// Interface for window management operations
    /// </summary>
    public interface IWindowManager
    {
        /// <summary>
        /// Apply transparency and window effects to the specified window
        /// </summary>
        WindowInfo HijackWindow(IntPtr windowHandle);

        /// <summary>
        /// Apply transparency settings to a window
        /// </summary>
        void ApplyTransparency(IntPtr windowHandle, WindowInfo windowInfo, short alpha, bool clickThrough, bool topMost);

        /// <summary>
        /// Restore a window to its original state
        /// </summary>
        void RestoreWindow(IntPtr windowHandle, WindowInfo windowInfo);

        /// <summary>
        /// Move window to next monitor
        /// </summary>
        void MoveToNextMonitor(IntPtr windowHandle);

        /// <summary>
        /// Move window to previous monitor
        /// </summary>
        void MoveToPreviousMonitor(IntPtr windowHandle);

        /// <summary>
        /// Maximize the specified window
        /// </summary>
        void MaximizeWindow(IntPtr windowHandle);

        /// <summary>
        /// Minimize the specified window
        /// </summary>
        void MinimizeWindow(IntPtr windowHandle);

        /// <summary>
        /// Check if the specified window handle is the desktop
        /// </summary>
        bool IsDesktopWindow(IntPtr windowHandle);
    }

    /// <summary>
    /// Implementation of window management using Win32 APIs
    /// </summary>
    public class WindowManager : IWindowManager
    {
        private const short OPAQUE = 255;

        // Window styles for transparency
        private const uint NEW_STYLE_TRANSPARENT = Win32Api.WS_EX_LAYERED;
        private const uint NEW_STYLE_CLICKTHROUGH = Win32Api.WS_EX_LAYERED | Win32Api.WS_EX_TRANSPARENT;
        private const uint NEW_STYLE_CLICKTHROUGH_TOPMOST = Win32Api.WS_EX_LAYERED | Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_TOPMOST;
        private const uint NEW_STYLE_ALL = Win32Api.WS_EX_LAYERED | Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_TOPMOST;

        public WindowInfo HijackWindow(IntPtr windowHandle)
        {
            // Get the original window style
            uint originalStyle = Win32Api.GetWindowLong(windowHandle, Win32Api.GWL_EX_STYLE);

            // Get the original alpha value
            short originalAlpha = OPAQUE;
            if ((originalStyle & Win32Api.WS_EX_LAYERED) != 0)
            {
                // Window is already layered, get the alpha value
                if (!Win32Api.GetLayeredWindowAttributes(windowHandle, out _, out originalAlpha, out _))
                {
                    originalAlpha = OPAQUE;
                }
            }

            // Create and return window info
            return new WindowInfo(originalStyle, originalAlpha);
        }

        public void ApplyTransparency(IntPtr windowHandle, WindowInfo windowInfo, short alpha, bool clickThrough, bool topMost)
        {
            // Determine the style to apply
            uint styleToApply = NEW_STYLE_TRANSPARENT;
            if (clickThrough && topMost)
            {
                styleToApply = NEW_STYLE_CLICKTHROUGH_TOPMOST;
            }
            else if (clickThrough)
            {
                styleToApply = NEW_STYLE_CLICKTHROUGH;
            }

            // Apply the new window style
            uint currentStyle = Win32Api.GetWindowLong(windowHandle, Win32Api.GWL_EX_STYLE);
            uint newStyle = (currentStyle & ~NEW_STYLE_ALL) | styleToApply;
            Win32Api.SetWindowLong(windowHandle, Win32Api.GWL_EX_STYLE, newStyle);

            // Set the transparency
            Win32Api.SetLayeredWindowAttributes(windowHandle, 0, alpha, Win32Api.LWA_ALPHA);

            // Set topmost if required
            if (topMost)
            {
                Win32Api.SetWindowPos(windowHandle, new IntPtr(Win32Api.HWND_TOPMOST), 0, 0, 0, 0,
                    Win32Api.SWP_NOMOVE | Win32Api.SWP_NOSIZE);
            }

            // Update the window info
            windowInfo.CurrentAlpha = alpha;
        }

        public void RestoreWindow(IntPtr windowHandle, WindowInfo windowInfo)
        {
            // Restore the original window style
            uint currentStyle = Win32Api.GetWindowLong(windowHandle, Win32Api.GWL_EX_STYLE);
            uint restoredStyle = (currentStyle & ~NEW_STYLE_ALL) | (windowInfo.Style & NEW_STYLE_ALL);
            Win32Api.SetWindowLong(windowHandle, Win32Api.GWL_EX_STYLE, restoredStyle);

            // Remove topmost style if not set in original style
            if ((windowInfo.Style & Win32Api.WS_EX_TOPMOST) == 0)
            {
                Win32Api.SetWindowPos(windowHandle, new IntPtr(Win32Api.HWND_NOTOPMOST), 0, 0, 0, 0,
                    Win32Api.SWP_NOSIZE | Win32Api.SWP_NOMOVE);
            }

            // Restore original transparency if the window was originally layered
            if ((windowInfo.Style & Win32Api.WS_EX_LAYERED) != 0)
            {
                Win32Api.SetLayeredWindowAttributes(windowHandle, 0, windowInfo.OriginalAlpha, Win32Api.LWA_ALPHA);
            }
        }

        public void MoveToNextMonitor(IntPtr windowHandle)
        {
            MoveToMonitor(windowHandle, true);
        }

        public void MoveToPreviousMonitor(IntPtr windowHandle)
        {
            MoveToMonitor(windowHandle, false);
        }

        private void MoveToMonitor(IntPtr windowHandle, bool next)
        {
            // Get current window position
            if (!Win32Api.GetWindowRect(windowHandle, out var rect))
                return;

            // Get all screens
            var screens = Screen.AllScreens;
            if (screens.Length <= 1)
                return;

            // Find current screen
            var currentScreen = Screen.FromHandle(windowHandle);
            int currentIndex = Array.IndexOf(screens, currentScreen);

            // Calculate target screen index
            int targetIndex;
            if (next)
            {
                targetIndex = (currentIndex + 1) % screens.Length;
            }
            else
            {
                targetIndex = (currentIndex - 1 + screens.Length) % screens.Length;
            }

            var targetScreen = screens[targetIndex];

            // Calculate new position (keep relative position within screen)
            var currentBounds = currentScreen.Bounds;
            var targetBounds = targetScreen.Bounds;

            int relativeX = rect.Left - currentBounds.X;
            int relativeY = rect.Top - currentBounds.Y;
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            // Scale position to target screen
            float scaleX = (float)targetBounds.Width / currentBounds.Width;
            float scaleY = (float)targetBounds.Height / currentBounds.Height;

            int newX = targetBounds.X + (int)(relativeX * scaleX);
            int newY = targetBounds.Y + (int)(relativeY * scaleY);

            // Ensure window fits on target screen
            if (newX + width > targetBounds.Right)
                newX = targetBounds.Right - width;
            if (newY + height > targetBounds.Bottom)
                newY = targetBounds.Bottom - height;
            if (newX < targetBounds.Left)
                newX = targetBounds.Left;
            if (newY < targetBounds.Top)
                newY = targetBounds.Top;

            Win32Api.MoveWindow(windowHandle, newX, newY, width, height, true);
        }

        public void MaximizeWindow(IntPtr windowHandle)
        {
            if (!Win32Api.IsZoomed(windowHandle))
            {
                Win32Api.ShowWindow(windowHandle, Win32Api.SW_SHOWMAXIMIZED);
            }
        }

        public void MinimizeWindow(IntPtr windowHandle)
        {
            if (!Win32Api.IsIconic(windowHandle))
            {
                Win32Api.ShowWindow(windowHandle, Win32Api.SW_SHOWMINIMIZED);
            }
        }

        public bool IsDesktopWindow(IntPtr windowHandle)
        {
            return windowHandle == Win32Api.GetShellWindow();
        }
    }
}
