namespace Winco.Platform;

using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;

public static class DwmStyler
{
    // A DWM usa atributos com tamanho fixo; o resultado do Win32 é devolvido como HRESULT.
    // Se o valor for válido, o código tem sucesso (S_OK / HRESULT.Succeeded == true).

    public static unsafe HRESULT SetCorners(HWND hwnd, DWM_WINDOW_CORNER_PREFERENCE corners)
    {
        DWM_WINDOW_CORNER_PREFERENCE value = corners;
        return PInvoke.DwmSetWindowAttribute(
            hwnd,
            DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
            &value,
            (uint)sizeof(DWM_WINDOW_CORNER_PREFERENCE));
    }

    public static unsafe HRESULT SetBorderColor(HWND hwnd, uint color)
    {
        uint value = color;
        return PInvoke.DwmSetWindowAttribute(
            hwnd,
            DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR,
            &value,
            sizeof(uint));
    }

    public static unsafe HRESULT SetCaptionColor(HWND hwnd, uint color)
    {
        uint value = color;
        return PInvoke.DwmSetWindowAttribute(
            hwnd,
            DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR,
            &value,
            sizeof(uint));
    }

    public static unsafe HRESULT SetCaptionTextColor(HWND hwnd, uint color)
    {
        uint value = color;
        return PInvoke.DwmSetWindowAttribute(
            hwnd,
            DWMWINDOWATTRIBUTE.DWMWA_TEXT_COLOR,
            &value,
            sizeof(uint));
    }

    public static unsafe HRESULT SetDarkTitleBar(HWND hwnd, bool enabled)
    {
        uint value = enabled ? 1u : 0u;
        return PInvoke.DwmSetWindowAttribute(
            hwnd,
            DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
            &value,
            sizeof(uint));
    }

    public static unsafe HRESULT SetBackdrop(HWND hwnd, DWM_SYSTEMBACKDROP_TYPE backdrop)
    {
        DWM_SYSTEMBACKDROP_TYPE value = backdrop;
        return PInvoke.DwmSetWindowAttribute(
            hwnd,
            DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            &value,
            (uint)sizeof(DWM_SYSTEMBACKDROP_TYPE));
    }
}
