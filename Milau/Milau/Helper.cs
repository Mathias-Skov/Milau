using System;
using System.Numerics;
using System.Runtime.InteropServices;

public static class WindowUtils
{
    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

    [DllImport("user32.dll")]
    static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

    [DllImport("Shcore.dll")]
    static extern int GetDpiForMonitor(IntPtr hmonitor, int dpiType, out uint dpiX, out uint dpiY);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT { public int Left, Top, Right, Bottom; }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT { public int X, Y; }

    public static Vector2 GetGameWindowSize(string windowTitle)
    {
        IntPtr hWnd = FindWindow(null, windowTitle);
        if (hWnd == IntPtr.Zero) return new Vector2(1920, 1080); // fallback

        GetClientRect(hWnd, out RECT rect);

        IntPtr monitor = MonitorFromWindow(hWnd, 2 /*MONITOR_DEFAULTTONEAREST*/);
        GetDpiForMonitor(monitor, 0 /*MDT_EFFECTIVE_DPI*/, out uint dpiX, out uint dpiY);

        float scale = dpiX / 96.0f; // 96 is default DPI (100%)
        float width = (rect.Right - rect.Left) * scale;
        float height = (rect.Bottom - rect.Top) * scale;

        return new Vector2(width, height);
    }
}