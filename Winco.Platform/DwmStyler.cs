namespace Winco.Platform;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.UI.WindowsAndMessaging;

// Os tipos que o CsWin32 gera (HWND, HRESULT, DWM_...) são "internal": só existem dentro
// do Winco.Platform. Por isso, os métodos públicos usam só tipos normais de C#
// (nint para a janela, bool para o resultado, enums nossos) e convertem lá dentro.
// É a mesma ideia do Wincoenumerador, que já devolve a janela como nint.

public enum CornerStyle
{
    Default = 0,      // o Windows decide
    Square = 1,       // sem arredondar
    Round = 2,
    RoundSmall = 3,
}

public enum BackdropStyle
{
    Auto = 0,
    None = 1,
    Mica = 2,
    Acrylic = 3,
    MicaAlt = 4,
}

public static class DwmStyler
{
    private static readonly Dictionary<nint, nint> OriginalExtendedStyles = new();

    // Cores no formato COLORREF: 0x00BBGGRR (vermelho = 0x000000FF).
    // 0xFFFFFFFF = cor por omissão; 0xFFFFFFFE = sem borda.

    public static unsafe bool SetCorners(nint window, CornerStyle corners)
    {
        var value = (DWM_WINDOW_CORNER_PREFERENCE)corners;
        HRESULT hr = PInvoke.DwmSetWindowAttribute(
            (HWND)window,
            DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
            &value,
            (uint)sizeof(DWM_WINDOW_CORNER_PREFERENCE));
        return hr.Succeeded;
    }

    public static bool SetBorderColor(nint window, uint color) =>
        SetUInt(window, DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, color);

    public static bool SetCaptionColor(nint window, uint color) =>
        SetUInt(window, DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR, color);

    public static bool SetCaptionTextColor(nint window, uint color) =>
        SetUInt(window, DWMWINDOWATTRIBUTE.DWMWA_TEXT_COLOR, color);

    public static bool SetDarkTitleBar(nint window, bool enabled) =>
        SetUInt(window, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, enabled ? 1u : 0u);

    // Só funciona no Windows 11 22H2 (build 22621) ou mais recente.
    public static unsafe bool SetBackdrop(nint window, BackdropStyle backdrop)
    {
        var value = (DWM_SYSTEMBACKDROP_TYPE)backdrop;
        HRESULT hr = PInvoke.DwmSetWindowAttribute(
            (HWND)window,
            DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            &value,
            (uint)sizeof(DWM_SYSTEMBACKDROP_TYPE));
        return hr.Succeeded;
    }

    // Os quatro métodos de cor/modo escuro fazem exatamente o mesmo com um uint,
    // por isso ficam todos neste método auxiliar (private: não precisa de ser público).
    private static unsafe bool SetUInt(nint window, DWMWINDOWATTRIBUTE attribute, uint value)
    {
        HRESULT hr = PInvoke.DwmSetWindowAttribute((HWND)window, attribute, &value, sizeof(uint));
        return hr.Succeeded;
    }

    // Devolve false se a janela já era transparente por iniciativa da própria app,
    // ou se o Windows recusou (ex.: app aberta como administrador).
    public static bool SetOpacity(nint window, byte alpha)
    {
        var hwnd = (HWND)window;
        nint style = PInvoke.GetWindowLongPtr(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        bool alreadyLayered = (style & (nint)WINDOW_EX_STYLE.WS_EX_LAYERED) != 0;

        if (!OriginalExtendedStyles.ContainsKey(window))
        {
            if (alreadyLayered)
            {
                return false;
            }

            OriginalExtendedStyles[window] = style;
            PInvoke.SetWindowLongPtr(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, style | (nint)WINDOW_EX_STYLE.WS_EX_LAYERED);
        }

        return PInvoke.SetLayeredWindowAttributes(hwnd, new COLORREF(0), alpha, LAYERED_WINDOW_ATTRIBUTES_FLAGS.LWA_ALPHA);
    }

    public static bool ResetOpacity(nint window)
    {
        if (!OriginalExtendedStyles.TryGetValue(window, out nint originalStyle))
        {
            return false;   // nunca lhe mexemos
        }

        PInvoke.SetWindowLongPtr((HWND)window, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, originalStyle);
        OriginalExtendedStyles.Remove(window);
        return true;
    }
}
