namespace Winco.Platform;

using System.Collections.Generic;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.System.Threading;
using Windows.Win32.UI.WindowsAndMessaging;
using Winco.Core.Windows;

public static class WindowEnumerator
{
    public static List<WindowInfo> GetTopLevelWindows()
    {
        var result = new List<WindowInfo>();

        WNDENUMPROC callback = (hwnd, _) =>
        {
            if (IsTopLevelWindow(hwnd))
            {
                uint processId = GetProcessId(hwnd);
                string? processPath = GetProcessPath(processId);
                result.Add(new WindowInfo(
                    (nint)hwnd,
                    GetWindowTitle(hwnd),
                    GetClassName(hwnd),
                    processId,
                    processPath is null ? "?" : Path.GetFileName(processPath),
                    processPath));
            }

            return true; // TRUE = continuar a enumerar
        };

        PInvoke.EnumWindows(callback, 0);
        GC.KeepAlive(callback); // o delegate tem de viver até o EnumWindows terminar

        return result;
    }

    internal static bool IsTopLevelWindow(HWND hwnd)
    {
        return PInvoke.IsWindowVisible(hwnd)
            && PInvoke.GetWindow(hwnd, GET_WINDOW_CMD.GW_OWNER).IsNull
            && !HasWindowStyle(hwnd, WINDOW_EX_STYLE.WS_EX_TOOLWINDOW)
            && IsNotCloaked(hwnd)
            && !string.IsNullOrEmpty(GetWindowTitle(hwnd));
    }

    private static bool HasWindowStyle(HWND hwnd, WINDOW_EX_STYLE style)
    {
        var exStyle = (WINDOW_EX_STYLE)PInvoke.GetWindowLongPtr(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        return (exStyle & style) != 0;
    }

    // DWMWA_CLOAKED escreve um DWORD (4 bytes), por isso usa-se uint e não bool.
    private static unsafe bool IsNotCloaked(HWND hwnd)
    {
        uint cloaked;
        HRESULT hr = PInvoke.DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_CLOAKED, &cloaked, sizeof(uint));
        return hr.Succeeded && cloaked == 0;
    }

    private static unsafe string GetWindowTitle(HWND hwnd)
    {
        int length = PInvoke.GetWindowTextLength(hwnd);
        if (length == 0)
        {
            return string.Empty;
        }

        // +1 para o '\0' final
        char* buffer = stackalloc char[length + 1];
        int copied = PInvoke.GetWindowText(hwnd, buffer, length + 1);
        return new string(buffer, 0, copied);
    }

    private static unsafe string GetClassName(HWND hwnd)
    {
        const int MaxClassName = 256;
        char* buffer = stackalloc char[MaxClassName];
        int copied = PInvoke.GetClassName(hwnd, buffer, MaxClassName);
        return new string(buffer, 0, copied);
    }

    private static unsafe uint GetProcessId(HWND hwnd)
    {
        uint processId;
        PInvoke.GetWindowThreadProcessId(hwnd, &processId);
        return processId;
    }

    // Devolve null quando o Windows nega acesso (ex.: processos de administrador).
    private static unsafe string? GetProcessPath(uint processId)
    {
        HANDLE process = PInvoke.OpenProcess(PROCESS_ACCESS_RIGHTS.PROCESS_QUERY_LIMITED_INFORMATION, false, processId);
        if (process.IsNull)
        {
            return null;
        }

        try
        {
            const int MaxPath = 1024;
            char* buffer = stackalloc char[MaxPath];
            uint size = MaxPath;
            if (!PInvoke.QueryFullProcessImageName(process, PROCESS_NAME_FORMAT.PROCESS_NAME_WIN32, buffer, &size))
            {
                return null;
            }

            return new string(buffer, 0, (int)size);
        }
        finally
        {
            PInvoke.CloseHandle(process);
        }
    }
}
