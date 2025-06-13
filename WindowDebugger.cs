using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowDebugger
{
  class Program
  {
    // Win32 API declarations
    [DllImport("user32.dll")]
    public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    public static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll")]
    public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern bool GetLayeredWindowAttributes(IntPtr hWnd, out int transparentColor, out short alpha, out int action);

    [DllImport("user32.dll")]
    public static extern IntPtr GetShellWindow();

    public const int GWL_EX_STYLE = -20;
    public const int WS_EX_LAYERED = 0x80000;

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    static void Main(string[] args)
    {
      Console.WriteLine("=== Window Debugger for SeeThroughWindows Auto-Apply Issue ===");
      Console.WriteLine();

      var excludedProcessNames = new List<string>
            {
                "explorer", "dwm", "winlogon", "csrss", "seethroughwindows",
                "taskmgr", "msedge", "chrome"
            };

      var excludedWindowTitles = new List<string>
            {
                "Program Manager", "Desktop", "",
                "Microsoft Text Input Application", "Windows Security"
            };

      Console.WriteLine($"Excluded processes: {string.Join(", ", excludedProcessNames)}");
      Console.WriteLine($"Excluded window titles: {string.Join(", ", excludedWindowTitles.Where(t => !string.IsNullOrEmpty(t)))} + [empty titles]");
      Console.WriteLine();

      var allWindows = new List<(IntPtr Handle, string Title, string ProcessName, string Status)>();
      var eligibleWindows = new List<(IntPtr Handle, string Title, string ProcessName)>();

      EnumWindows((hWnd, lParam) =>
      {
        try
        {
          var title = GetWindowTitle(hWnd);
          var processName = GetProcessName(hWnd);

          // Only process visible windows
          if (!IsWindowVisible(hWnd))
          {
            allWindows.Add((hWnd, title, processName, "Not visible"));
            return true;
          }

          // Skip windows with excluded titles
          if (IsExcludedTitle(title, excludedWindowTitles))
          {
            allWindows.Add((hWnd, title, processName, "Excluded title"));
            return true;
          }

          // Skip excluded processes
          if (IsExcludedProcess(processName, excludedProcessNames))
          {
            allWindows.Add((hWnd, title, processName, $"Excluded process"));
            return true;
          }

          // Skip desktop windows
          if (IsDesktopWindow(hWnd))
          {
            allWindows.Add((hWnd, title, processName, "Desktop window"));
            return true;
          }

          // Skip windows that already have transparency
          var exStyle = GetWindowLong(hWnd, GWL_EX_STYLE);
          if ((exStyle & WS_EX_LAYERED) != 0)
          {
            if (GetLayeredWindowAttributes(hWnd, out _, out short alpha, out _) && alpha < 255)
            {
              allWindows.Add((hWnd, title, processName, $"Already transparent (alpha: {alpha})"));
              return true;
            }
          }

          // This window is eligible
          allWindows.Add((hWnd, title, processName, "ELIGIBLE"));
          eligibleWindows.Add((hWnd, title, processName));
        }
        catch (Exception ex)
        {
          allWindows.Add((hWnd, "ERROR", "ERROR", $"Exception: {ex.Message}"));
        }

        return true;
      }, IntPtr.Zero);

      Console.WriteLine($"=== RESULTS ===");
      Console.WriteLine($"Total windows examined: {allWindows.Count}");
      Console.WriteLine($"Eligible windows: {eligibleWindows.Count}");
      Console.WriteLine();

      if (eligibleWindows.Any())
      {
        Console.WriteLine("=== ELIGIBLE WINDOWS ===");
        foreach (var (handle, title, processName) in eligibleWindows)
        {
          Console.WriteLine($"✓ '{title}' (Process: {processName}, Handle: {handle})");
        }
        Console.WriteLine();
      }

      Console.WriteLine("=== EXCLUSION SUMMARY ===");
      var exclusionSummary = allWindows
          .Where(w => w.Status != "ELIGIBLE")
          .GroupBy(w => w.Status)
          .OrderByDescending(g => g.Count())
          .Select(g => $"{g.Key}: {g.Count()}")
          .ToList();

      foreach (var summary in exclusionSummary)
      {
        Console.WriteLine($"  {summary}");
      }

      Console.WriteLine();
      Console.WriteLine("=== DETAILED WINDOW LIST (First 20 visible windows) ===");
      var visibleWindows = allWindows.Where(w => w.Status != "Not visible").Take(20);
      foreach (var (handle, title, processName, status) in visibleWindows)
      {
        var statusIcon = status == "ELIGIBLE" ? "✓" : "✗";
        Console.WriteLine($"{statusIcon} '{title}' | Process: {processName} | Status: {status}");
      }

      if (allWindows.Count(w => w.Status != "Not visible") > 20)
      {
        Console.WriteLine($"... and {allWindows.Count(w => w.Status != "Not visible") - 20} more visible windows");
      }

      Console.WriteLine();
      Console.WriteLine("Press any key to exit...");
      Console.ReadKey();
    }

    static string GetWindowTitle(IntPtr hWnd)
    {
      try
      {
        var length = GetWindowTextLength(hWnd);
        if (length == 0) return string.Empty;

        var sb = new StringBuilder(length + 1);
        GetWindowText(hWnd, sb, sb.Capacity);
        return sb.ToString();
      }
      catch
      {
        return string.Empty;
      }
    }

    static string GetProcessName(IntPtr hWnd)
    {
      try
      {
        GetWindowThreadProcessId(hWnd, out uint processId);
        using var process = Process.GetProcessById((int)processId);
        return process.ProcessName.ToLowerInvariant();
      }
      catch
      {
        return string.Empty;
      }
    }

    static bool IsExcludedProcess(string processName, List<string> excludedProcessNames)
    {
      return excludedProcessNames.Any(excluded =>
          processName.Contains(excluded, StringComparison.OrdinalIgnoreCase));
    }

    static bool IsExcludedTitle(string title, List<string> excludedWindowTitles)
    {
      if (string.IsNullOrWhiteSpace(title))
        return true;

      return excludedWindowTitles.Any(excluded =>
          title.Equals(excluded, StringComparison.OrdinalIgnoreCase) ||
          title.Contains(excluded, StringComparison.OrdinalIgnoreCase));
    }

    static bool IsDesktopWindow(IntPtr windowHandle)
    {
      return windowHandle == GetShellWindow();
    }
  }
}
